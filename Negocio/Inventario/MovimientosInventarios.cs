using Entidades.App;
using Entidades.Inventario;
using System;
using System.Collections.Generic;
using System.Data;

namespace Negocio.Inventario
{
    public class MovimientosInventarios : NegocioBase<MovimientoInventario>
    {
        public MovimientosInventarios(Token token) : base("movinv_id", "movinv_activo", "Movimientos_Inventario", "movinv")
        {
            Token = token;
            TokenFilter = true;
        }

        public override MovimientoInventario ObjetoNuevo()
        {
            var obj = new MovimientoInventario();
            obj.IdEncriptado = App.Security.EncriptarID("0");
            return obj;
        }

        public override void PermiteGuardar(MovimientoInventario obj)
        {
            if (obj.movinv_cantidad <= 0)
                throw new Exception("La cantidad debe ser mayor a cero.");
            if (obj.Producto == null || obj.Producto.pro_id <= 0)
                throw new Exception("Debe seleccionar un producto.");

            var producto = new Productos(Token).ObtenerPorID(obj.Producto.pro_id.ToString());
            if (producto.pro_requiere_lote && string.IsNullOrWhiteSpace(obj.movinv_lote))
                throw new Exception("El producto requiere lote.");
            if (producto.pro_requiere_vencimiento && !obj.movinv_vencimiento.HasValue)
                throw new Exception("El producto requiere fecha de vencimiento.");
            if (producto.pro_requiere_serie && string.IsNullOrWhiteSpace(obj.movinv_serie))
                throw new Exception("El producto requiere serie.");
        }

        public override ObjectMessage Save(MovimientoInventario Obj)
        {
            ObjectMessage oM = new ObjectMessage();
            try
            {
                PermiteGuardar(Obj);
                DataRow row = db.Estructura(nombreTablaPrincipal);
                oM = SaveReflection(Obj, row, true);

                if (oM.Success)
                {
                    var tipo = new TiposMovimientosInventarios(Token).ObtenerPorID(Obj.TipoMovimientoInventario.timi_id.ToString());
                    decimal signo = tipo.timi_signo == "-" ? -1 : 1;
                    new Stocks(Token).AplicarMovimiento(Obj, signo);
                }
            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.Message = ex.Message;
            }
            return oM;
        }

        public override MovimientoInventario Mapear(DataRow dr)
        {
            var obj = MapearSimple(dr);
            obj.TipoMovimientoInventario = TiposMovimientosInventarios.MapearStatic(dr);
            obj.DepositoOrigen = Depositos.MapearStatic(dr);
            obj.DepositoDestino = Depositos.MapearStatic(dr);
            obj.Producto = Productos.MapearStatic(dr);
            return obj;
        }

        public override MovimientoInventario MapearCompleto(DataRow dr) => Mapear(dr);
        public override MovimientoInventario MapearSimple(DataRow dr) => MapearStatic(dr);

        public static MovimientoInventario MapearStatic(DataRow dr)
        {
            var obj = new MovimientoInventario();
            return MapearReflection(obj, dr);
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "SELECT * FROM Movimientos_Inventario ";
            sQuery += "INNER JOIN Tipos_Movimientos_Inventario ON timi_id = movinv_timi_id ";
            sQuery += "INNER JOIN Depositos ON depo_id = movinv_depo_origen_id ";
            sQuery += "INNER JOIN Productos ON pro_id = movinv_pro_id ";
            if (sWHERE != "") sQuery += " WHERE " + sWHERE;
            if (sOrderBy != "") sQuery += " ORDER BY " + sOrderBy;
            return sQuery;
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            return new List<DLLObject>();
        }
    }
}
