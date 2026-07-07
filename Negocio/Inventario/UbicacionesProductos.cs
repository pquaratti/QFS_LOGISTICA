using Entidades.App;
using Entidades.Inventario;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

                PermiteGuardar(ubicacionProducto);

                bool esNuevo = ubicacionProducto.ubipro_id <= 0;
                if (esNuevo)
                    ubicacionProducto.ubipro_id = InsertarAsignacion(ubicacionProducto);
                else
                    ActualizarAsignacion(ubicacionProducto);

                ActualizarEstadoUbicacion(ubicacionProducto);

                ubicacionProducto.IdEncriptado = App.Security.EncriptarID(Convert.ToString(ubicacionProducto.ubipro_id));
                oM.Success = true;
                oM.Message = esNuevo ? "Datos ingresados" : "Datos actualizados";
                oM.ObjectRelation = ubicacionProducto.IdEncriptado;
                return oM;
            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.Message = ex.Message;
                return oM;
            }
        }

        private void ActualizarEstadoUbicacion(UbicacionProducto ubicacionProducto)
        {
            string estado = ObtenerEstadoUbicacion(ubicacionProducto);
            string query = @"
                UPDATE Ubicaciones_Logisticas
                SET ubilog_teubilog_id = (
                        SELECT TOP 1 teubilog_id
                        FROM Tipo_Estado_Ubicacion_Logistica
                        WHERE teubilog_nombre = @estado
                            AND ISNULL(teubilog_activo, 1) = 1
                    ),
                    ubilog_activo = 1,
                    ubilog_fec_mod = @fecha,
                    ubilog_usu_id_mod = @usuario
                WHERE ubilog_id = @ubicacionID";

            var parametros = new List<SqlParameter>()
            {
                new SqlParameter("estado", estado),
                new SqlParameter("fecha", DateTime.Now),
                new SqlParameter("usuario", Token.UserID),
                new SqlParameter("ubicacionID", ubicacionProducto.UbicacionLogistica.ubilog_id)
            };

            db.SQLExecuteNonQuery(query, parametros);
        }

        private string ObtenerEstadoUbicacion(UbicacionProducto ubicacionProducto)
        {
            if (ubicacionProducto.ubipro_cantidad <= 0)
                return "Libre";

            return ubicacionProducto.ubipro_cantidad >= ubicacionProducto.ubipro_cantidad_maxima ? "Ocupada" : "Parcial";
        }

        private void ActualizarAsignacion(UbicacionProducto ubicacionProducto)
        {
            string query = @"
                UPDATE Ubicaciones_Productos
                SET ubipro_pro_id = @productoID,
                    ubipro_cantidad = @cantidad,
                    ubipro_cantidad_maxima = @cantidadMaxima,
                    ubipro_activo = 1,
                    ubipro_fec_mod = @fecha,
                    ubipro_usu_id_mod = @usuario
                WHERE ubipro_id = @ubicacionProductoID";

            db.SQLExecuteNonQuery(query, ParametrosAsignacion(ubicacionProducto, true));
        }

        private int InsertarAsignacion(UbicacionProducto ubicacionProducto)
        {
            string query = @"
                INSERT INTO Ubicaciones_Productos
                (
                    ubipro_ubilog_id,
                    ubipro_pro_id,
                    ubipro_cantidad,
                    ubipro_cantidad_maxima,
                    ubipro_activo,
                    ubipro_fec_alta,
                    ubipro_fec_mod,
                    ubipro_usu_id_alta,
                    ubipro_usu_id_mod
                )
                VALUES
                (
                    @ubicacionID,
                    @productoID,
                    @cantidad,
                    @cantidadMaxima,
                    1,
                    @fecha,
                    @fecha,
                    @usuario,
                    @usuario
                );

                SELECT SCOPE_IDENTITY() AS ubipro_id;";

            DataTable dt = db.SQLSelect(query, ParametrosAsignacion(ubicacionProducto, false));
            if (dt.Rows.Count == 0)
                return 0;

            return Convert.ToInt32(dt.Rows[0]["ubipro_id"]);
        }

        private List<SqlParameter> ParametrosAsignacion(UbicacionProducto ubicacionProducto, bool incluyeID)
        {
            var parametros = new List<SqlParameter>()
            {
                new SqlParameter("ubicacionID", ubicacionProducto.UbicacionLogistica.ubilog_id),
                new SqlParameter("productoID", ubicacionProducto.Producto.pro_id),
                new SqlParameter("cantidad", ubicacionProducto.ubipro_cantidad),
                new SqlParameter("cantidadMaxima", ubicacionProducto.ubipro_cantidad_maxima),
                new SqlParameter("fecha", DateTime.Now),
                new SqlParameter("usuario", Token.UserID)
            };

            if (incluyeID)
                parametros.Add(new SqlParameter("ubicacionProductoID", ubicacionProducto.ubipro_id));

            return parametros;
        }
    }
}