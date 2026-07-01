using Entidades;
using Entidades.App;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Negocio
{
    public class UbicacionesLogisticas : NegocioBase<Entidades.UbicacionLogistica>
    {
        public UbicacionesLogisticas(Entidades.App.Token paramToken) : base("ubilog_id", "ubilog_activo", "Ubicaciones_Logisticas", "ubilog")
        {
            Token = paramToken;
            TokenFilter = false;
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            List<DLLObject> lst = new List<DLLObject>();
            if (agregaDefault)
                lst.Add(new DLLObject() { Value = "0", Text = "Seleccione" });

            foreach (var item in ListarActivos())
                lst.Add(new DLLObject() { Value = item.ubilog_id.ToString(), Text = item.ubilog_codigo });

            return lst;
        }

        public List<UbicacionLogistica> ListarPorPasillo(int pasilloID)
        {
            List<ObjectParameter> filtros = new List<ObjectParameter>();
            filtros.Add(new ObjectParameter() { Name = "ubilog_pasillo_id", Value = pasilloID });
            filtros.Add(new ObjectParameter() { Name = "ubilog_activo", Value = 1 });
            return ListarConFiltros(filtros);
        }

        public ObjectMessage RegenerarDesdePasillo(DepositoPasillo pasillo)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                DesactivarUbicacionesPasillo(pasillo.depopas_id);

                for (int posicion = 1; posicion <= pasillo.depopas_cantidad_posiciones; posicion++)
                {
                    for (int nivel = 1; nivel <= pasillo.depopas_cantidad_alturas; nivel++)
                    {
                        UbicacionLogistica ubicacion = new UbicacionLogistica();
                        ubicacion.ubilog_codigo = CrearCodigoUbicacion(pasillo.depopas_codigo, posicion, nivel);
                        ubicacion.Deposito.depo_id = pasillo.Deposito.depo_id;
                        ubicacion.Pasillo.depopas_id = pasillo.depopas_id;
                        ubicacion.ubilog_posicion = posicion;
                        ubicacion.ubilog_nivel = nivel.ToString();
                        ubicacion.ubilog_altura = pasillo.depopas_altura_nivel;
                        ubicacion.ubilog_longitud = pasillo.depopas_largo / pasillo.depopas_cantidad_posiciones;
                        ubicacion.ubilog_anchura = pasillo.depopas_ancho;
                        ubicacion.ubilog_capacidad_cubica = ubicacion.ubilog_altura * ubicacion.ubilog_longitud * ubicacion.ubilog_anchura;
                        ubicacion.ubilog_peso_maximo = pasillo.depopas_peso_maximo;
                        ubicacion.ubilog_multiples_articulos = false;
                        ubicacion.ubilog_multiples_lotes = false;
                        Save(ubicacion);
                    }
                }

                oM.Success = true;
                oM.Message = "Pasillo y ubicaciones generadas correctamente.";
            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.Message = ex.Message;
            }

            return oM;
        }

        public override UbicacionLogistica ObjetoNuevo()
        {
            UbicacionLogistica obj = new UbicacionLogistica();
            obj.ubilog_id = 0;
            obj.IdEncriptado = App.Security.EncriptarID(Convert.ToString(obj.ubilog_id));
            return obj;
        }

        public override ObjectMessage Save(UbicacionLogistica Obj)
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

        public override UbicacionLogistica Mapear(DataRow dr)
        {
            UbicacionLogistica obj = MapearSimple(dr);
            obj.Deposito = Depositos.MapearStatic(dr);
            obj.Pasillo = DepositosPasillos.MapearStatic(dr);
            obj.TipoEstado = TipoEstadosUbicacionesLogisticas.MapearStatic(dr);
            return obj;
        }

        public override UbicacionLogistica MapearCompleto(DataRow dr)
        {
            return Mapear(dr);
        }

        public override UbicacionLogistica MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static UbicacionLogistica MapearStatic(DataRow dr)
        {
            UbicacionLogistica obj = new UbicacionLogistica();
            obj = MapearReflection(obj, dr);
            return obj;
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT " + sTOP + " *, ubilog_trotlog_id AS ubilog_trolog_id, ubilog_pasillo_id AS ubilog_depopas_id FROM Ubicaciones_Logisticas ";
            sQuery += " LEFT JOIN Depositos ON depo_id = ubilog_depo_id ";
            sQuery += " LEFT JOIN Depositos_Pasillos ON depopas_id = ubilog_pasillo_id ";
            sQuery += " LEFT JOIN Tipo_Estado_Ubicacion_Logistica ON teubilog_id = ubilog_teubilog_id ";

            if ((sWHERE != ""))
                sQuery += " WHERE " + sWHERE;

            if ((sOrderBy != ""))
                sQuery += " ORDER BY " + sOrderBy;

            return sQuery;
        }

        public void DesactivarUbicacionesPasillo(int pasilloID)
        {
            string query = "UPDATE Ubicaciones_Logisticas SET ubilog_activo = 0, ubilog_fec_mod = @fecha, ubilog_usu_id_mod = @usuario WHERE ubilog_pasillo_id = @pasilloID";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("fecha", DateTime.Now));
            parametros.Add(new SqlParameter("usuario", Token.UserID));
            parametros.Add(new SqlParameter("pasilloID", pasilloID));
            db.SQLExecuteNonQuery(query, parametros);
        }

        public ObjectMessage GuardarEstadoWms(int ubicacionID, string estado, int pasilloID = 0, int posicion = 0, string nivel = "")
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                estado = NormalizarEstadoWms(estado);
                int tipoEstadoID = ObtenerTipoEstadoID(estado);

                if (tipoEstadoID <= 0)
                    throw new Exception("El estado de ubicación logística no existe o no está activo: " + estado);

                if (ubicacionID <= 0)
                    ubicacionID = ObtenerOCrearUbicacionWms(pasilloID, posicion, nivel, tipoEstadoID);

                if (ubicacionID <= 0)
                    throw new Exception("No se pudo identificar la ubicación logística para guardar el estado.");

                string query = "UPDATE Ubicaciones_Logisticas SET ubilog_teubilog_id = @tipoEstadoID, ubilog_activo = 1, ubilog_fec_mod = @fecha, ubilog_usu_id_mod = @usuario WHERE ubilog_id = @ubicacionID";
                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter("tipoEstadoID", tipoEstadoID > 0 ? (object)tipoEstadoID : DBNull.Value));
                parametros.Add(new SqlParameter("fecha", DateTime.Now));
                parametros.Add(new SqlParameter("usuario", Token.UserID));
                parametros.Add(new SqlParameter("ubicacionID", ubicacionID));
                db.SQLExecuteNonQuery(query, parametros);

                oM.Success = true;
                oM.Message = "Estado guardado correctamente.";
                oM.ObjectRelation = ubicacionID;
            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.Message = ex.Message;
            }

            return oM;
        }

        private int ObtenerOCrearUbicacionWms(int pasilloID, int posicion, string nivel, int tipoEstadoID)
        {
            if (pasilloID <= 0 || posicion <= 0 || string.IsNullOrWhiteSpace(nivel))
                return 0;

            string queryExistente = "SELECT TOP 1 ubilog_id FROM Ubicaciones_Logisticas WHERE ubilog_pasillo_id = @pasilloID AND ubilog_posicion = @posicion AND ubilog_nivel = @nivel ORDER BY ubilog_activo DESC, ubilog_id DESC";
            List<SqlParameter> parametrosExistente = new List<SqlParameter>();
            parametrosExistente.Add(new SqlParameter("pasilloID", pasilloID));
            parametrosExistente.Add(new SqlParameter("posicion", posicion));
            parametrosExistente.Add(new SqlParameter("nivel", nivel));
            DataTable dtExistente = db.SQLSelect(queryExistente, parametrosExistente);

            if (dtExistente.Rows.Count > 0)
                return Convert.ToInt32(dtExistente.Rows[0]["ubilog_id"]);

            DepositoPasillo pasillo = new DepositosPasillos(Token).ObtenerPorID(Convert.ToString(pasilloID));
            if (pasillo == null || pasillo.depopas_id <= 0)
                return 0;

            int nivelNumero = Convert.ToInt32(nivel);
            string codigo = CrearCodigoUbicacion(pasillo.depopas_codigo, posicion, nivelNumero);
            decimal longitud = pasillo.depopas_cantidad_posiciones > 0 ? pasillo.depopas_largo / pasillo.depopas_cantidad_posiciones : pasillo.depopas_largo;
            decimal capacidadCubica = pasillo.depopas_altura_nivel * longitud * pasillo.depopas_ancho;
            int depositoID = pasillo.Deposito != null ? pasillo.Deposito.depo_id : 0;
            int plantaID = pasillo.Deposito != null && pasillo.Deposito.Planta != null ? pasillo.Deposito.Planta.planta_id : 0;

            //TODO: hacerlo en sp en vez de que query en el proximo commit
            string queryInsert = @"
                DECLARE @id INT;
                SELECT @id = ISNULL(MAX(ubilog_id), 0) + 1 FROM Ubicaciones_Logisticas WITH (UPDLOCK, HOLDLOCK);

                INSERT INTO Ubicaciones_Logisticas
                (
                    ubilog_id,
                    ubilog_codigo,
                    ubilog_teubilog_id,
                    ubilog_planta_id,
                    ubilog_depo_id,
                    ubilog_pasillo_id,
                    ubilog_posicion,
                    ubilog_nivel,
                    ubilog_altura,
                    ubilog_longitud,
                    ubilog_anchura,
                    ubilog_capacidad_cubica,
                    ubilog_peso_maximo,
                    ubilog_multiples_articulos,
                    ubilog_multiples_lotes,
                    ubilog_usu_id_alta,
                    ubilog_fec_alta,
                    ubilog_usu_id_mod,
                    ubilog_fec_mod,
                    ubilog_activo
                )
                VALUES
                (
                    @id,
                    @codigo,
                    @tipoEstadoID,
                    @plantaID,
                    @depositoID,
                    @pasilloID,
                    @posicion,
                    @nivel,
                    @altura,
                    @longitud,
                    @anchura,
                    @capacidadCubica,
                    @pesoMaximo,
                    0,
                    0,
                    @usuario,
                    @fecha,
                    @usuario,
                    @fecha,
                    1
                );

                SELECT @id AS ubilog_id;";

            List<SqlParameter> parametrosInsert = new List<SqlParameter>();
            parametrosInsert.Add(new SqlParameter("codigo", codigo));
            parametrosInsert.Add(new SqlParameter("tipoEstadoID", tipoEstadoID));
            parametrosInsert.Add(new SqlParameter("plantaID", plantaID > 0 ? (object)plantaID : DBNull.Value));
            parametrosInsert.Add(new SqlParameter("depositoID", depositoID > 0 ? (object)depositoID : DBNull.Value));
            parametrosInsert.Add(new SqlParameter("pasilloID", pasillo.depopas_id));
            parametrosInsert.Add(new SqlParameter("posicion", posicion));
            parametrosInsert.Add(new SqlParameter("nivel", nivel));
            parametrosInsert.Add(new SqlParameter("altura", pasillo.depopas_altura_nivel));
            parametrosInsert.Add(new SqlParameter("longitud", longitud));
            parametrosInsert.Add(new SqlParameter("anchura", pasillo.depopas_ancho));
            parametrosInsert.Add(new SqlParameter("capacidadCubica", capacidadCubica));
            parametrosInsert.Add(new SqlParameter("pesoMaximo", pasillo.depopas_peso_maximo));
            parametrosInsert.Add(new SqlParameter("usuario", Token.UserID));
            parametrosInsert.Add(new SqlParameter("fecha", DateTime.Now));

            DataTable dtInsert = db.SQLSelect(queryInsert, parametrosInsert);
            if (dtInsert.Rows.Count == 0)
                return 0;

            return Convert.ToInt32(dtInsert.Rows[0]["ubilog_id"]);
        }

        private string NormalizarEstadoWms(string estado)
        {
            if (string.IsNullOrWhiteSpace(estado))
                return "Libre";

            string estadoLower = estado.Trim().ToLowerInvariant();

            if (estadoLower.Contains("bloq")) return "Bloqueada";
            if (estadoLower.Contains("reserv")) return "Reservada";
            if (estadoLower.Contains("parcial")) return "Parcial";
            if (estadoLower.Contains("ocup")) return "Ocupada";

            return "Libre";
        }

        private int ObtenerTipoEstadoID(string estado)
        {
            if (string.IsNullOrWhiteSpace(estado))
                return 0;

            string query = "SELECT TOP 1 teubilog_id FROM Tipo_Estado_Ubicacion_Logistica WHERE teubilog_nombre = @estado AND ISNULL(teubilog_activo, 1) = 1";
            DataTable dt = db.SQLSelect(query, new List<SqlParameter>() { new SqlParameter("estado", estado) });

            if (dt.Rows.Count == 0)
                return 0;

            return Convert.ToInt32(dt.Rows[0]["teubilog_id"]);
        }

        private string CrearCodigoUbicacion(string codigoPasillo, int posicion, int nivel)
        {
            return codigoPasillo + "-P" + posicion.ToString("000") + "-N" + nivel.ToString("00");
        }
    }
}
