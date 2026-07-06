using Entidades.App;
using Entidades.Inventario;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Negocio.Inventario
{
    public class UbicacionesProductos : NegocioBase<UbicacionProducto>
    {
        public UbicacionesProductos(Token token) : base("ubipro_id", "ubipro_activo", "Ubicaciones_Productos", "ubipro")
        {
            Token = token;
        }

        public override UbicacionProducto ObjetoNuevo()
        {
            var obj = new UbicacionProducto();
            obj.IdEncriptado = App.Security.EncriptarID("0");
            return obj;
        }

        public override ObjectMessage Save(UbicacionProducto obj)
        {
            ObjectMessage oM = new ObjectMessage();
            try
            {
                PermiteGuardar(obj);
                DataRow row = db.Estructura(nombreTablaPrincipal);
                oM = SaveReflection(obj, row, true);
            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.Message = ex.Message;
            }
            return oM;
        }

        public override void PermiteGuardar(UbicacionProducto obj)
        {
            if (obj.UbicacionLogistica == null || obj.UbicacionLogistica.ubilog_id <= 0)
                throw new Exception("Debe seleccionar una ubicación logística válida.");
            if (obj.Producto == null || obj.Producto.pro_id <= 0)
                throw new Exception("Debe seleccionar un producto.");
            if (obj.ubipro_cantidad_maxima <= 0)
                throw new Exception("La cantidad máxima debe ser mayor a cero.");
            if (obj.ubipro_cantidad < 0)
                throw new Exception("La cantidad actual no puede ser negativa.");
            if (obj.ubipro_cantidad > obj.ubipro_cantidad_maxima)
                throw new Exception("La cantidad actual no puede superar la cantidad máxima de la ubicación.");
        }

        public override UbicacionProducto Mapear(DataRow dr)
        {
            var obj = MapearSimple(dr);
            obj.UbicacionLogistica = UbicacionesLogisticas.MapearStatic(dr);
            obj.Producto = Productos.MapearStatic(dr);
            return obj;
        }

        public override UbicacionProducto MapearCompleto(DataRow dr) => Mapear(dr);
        public override UbicacionProducto MapearSimple(DataRow dr) => MapearStatic(dr);

        public static UbicacionProducto MapearStatic(DataRow dr)
        {
            var obj = new UbicacionProducto();
            return MapearReflection(obj, dr);
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "SELECT " + sTOP + " * FROM Ubicaciones_Productos ";
            sQuery += "INNER JOIN Ubicaciones_Logisticas ON ubilog_id = ubipro_ubilog_id ";
            sQuery += "INNER JOIN Productos ON pro_id = ubipro_pro_id ";
            if (sWHERE != "") sQuery += " WHERE " + sWHERE;
            if (sOrderBy != "") sQuery += " ORDER BY " + sOrderBy;
            return sQuery;
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            return new List<DLLObject>();
        }

        public UbicacionProducto ObtenerActivoPorUbicacion(int ubicacionID)
        {
            return ListarConFiltros(new List<ObjectParameter>() {
                new ObjectParameter(){ Name = "ubipro_ubilog_id", Value = ubicacionID },
                new ObjectParameter(){ Name = "ubipro_activo", Value = true }
            }).FirstOrDefault() ?? ObjetoNuevo();
        }

        public ObjectMessage GuardarAsignacion(int ubicacionID, int productoID, decimal cantidad, int cantidadMaxima)
        {
            ObjectMessage oM = new ObjectMessage();
            try
            {
                var ubicacionProducto = ObtenerActivoPorUbicacion(ubicacionID);
                ubicacionProducto.UbicacionLogistica = new Entidades.UbicacionLogistica() { ubilog_id = ubicacionID };
                ubicacionProducto.Producto = new Producto() { pro_id = productoID };
                ubicacionProducto.ubipro_cantidad = cantidad;
                ubicacionProducto.ubipro_cantidad_maxima = cantidadMaxima;
                ubicacionProducto.ubipro_activo = true;
                return Save(ubicacionProducto);
            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.Message = ex.Message;
                return oM;
            }
        }
    }
}