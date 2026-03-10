using Entidades.App;
using Entidades.Inventario;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Negocio.Inventario
{
    public class Stocks : NegocioBase<Stock>
    {
        public Stocks(Token token) : base("stock_id", "stock_activo", "Stocks", "stock")
        {
            Token = token;
            TokenFilter = true;
        }

        public override Stock ObjetoNuevo()
        {
            var obj = new Stock();
            obj.IdEncriptado = App.Security.EncriptarID("0");
            return obj;
        }

        public override ObjectMessage Save(Stock Obj)
        {
            ObjectMessage oM = new ObjectMessage();
            try
            {
                DataRow row = db.Estructura(nombreTablaPrincipal);
                oM = SaveReflection(Obj, row, true);
            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.Message = ex.Message;
            }
            return oM;
        }

        public override Stock Mapear(DataRow dr)
        {
            var obj = MapearSimple(dr);
            obj.Deposito = Depositos.MapearStatic(dr);
            obj.Producto = Productos.MapearStatic(dr);
            return obj;
        }

        public override Stock MapearCompleto(DataRow dr) => Mapear(dr);
        public override Stock MapearSimple(DataRow dr) => MapearStatic(dr);

        public static Stock MapearStatic(DataRow dr)
        {
            var obj = new Stock();
            return MapearReflection(obj, dr);
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "SELECT * FROM Stocks ";
            sQuery += "INNER JOIN Depositos ON depo_id = stock_depo_id ";
            sQuery += "INNER JOIN Productos ON pro_id = stock_pro_id ";
            if (sWHERE != "") sQuery += " WHERE " + sWHERE;
            if (sOrderBy != "") sQuery += " ORDER BY " + sOrderBy;
            return sQuery;
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            return new List<DLLObject>();
        }

        public ObjectMessage AplicarMovimiento(MovimientoInventario movimiento, decimal signo)
        {
            var stock = ListarConFiltros(new List<ObjectParameter>() {
                new ObjectParameter(){ Name="stock_depo_id", Value=movimiento.DepositoOrigen.depo_id },
                new ObjectParameter(){ Name="stock_pro_id", Value=movimiento.Producto.pro_id }
            }).FirstOrDefault();

            if (stock == null)
            {
                stock = new Stock()
                {
                    Deposito = movimiento.DepositoOrigen,
                    Producto = movimiento.Producto,
                    stock_actual = 0,
                    stock_reservado = 0,
                    stock_disponible = 0
                };
            }

            var nuevoStock = stock.stock_actual + (movimiento.movinv_cantidad * signo);
            if (nuevoStock < 0)
                throw new Exception("No hay stock suficiente para realizar el movimiento.");

            stock.stock_actual = nuevoStock;
            stock.stock_disponible = stock.stock_actual - stock.stock_reservado;
            return Save(stock);
        }
    }
}
