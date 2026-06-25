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

        public List<UbicacionLogistica> ListarPorRack(int rackID)
        {
            List<ObjectParameter> filtros = new List<ObjectParameter>();
            filtros.Add(new ObjectParameter() { Name = "ubilog_deprack_id", Value = rackID });
            filtros.Add(new ObjectParameter() { Name = "ubilog_activo", Value = 1 });
            return ListarConFiltros(filtros);
        }

        public List<UbicacionLogistica> ListarPorDeposito(int depoID)
        {
            List<ObjectParameter> filtros = new List<ObjectParameter>();
            filtros.Add(new ObjectParameter() { Name = "ubilog_depo_id", Value = depoID });
            filtros.Add(new ObjectParameter() { Name = "ubilog_activo", Value = 1 });
            return ListarConFiltros(filtros);
        }

        public UbicacionLogistica ObtenerUbicacion(int ubicacionID)
        {
            List<ObjectParameter> filtros = new List<ObjectParameter>();
            filtros.Add(new ObjectParameter() { Name = "ubilog_id", Value = ubicacionID });
            List<UbicacionLogistica> lst = ListarConFiltros(filtros);
            return lst.Count > 0 ? lst[0] : new UbicacionLogistica();
        }

        public ObjectMessage RegenerarDesdeRack(DepositoRack rack, string codigoDeposito, string codigoZona, string codigoPasillo)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                DesactivarUbicacionesRack(rack.deprack_id);

                int columnas = rack.deprack_cantidad_columnas > 0 ? rack.deprack_cantidad_columnas : 1;
                int niveles = rack.deprack_cantidad_niveles > 0 ? rack.deprack_cantidad_niveles : 1;
                decimal anchoColumna = columnas > 0 ? rack.deprack_largo / columnas : rack.deprack_largo;

                for (int columna = 1; columna <= columnas; columna++)
                {
                    for (int nivel = 1; nivel <= niveles; nivel++)
                    {
                        UbicacionLogistica ubicacion = new UbicacionLogistica();
                        ubicacion.ubilog_codigo = CrearCodigoUbicacionRack(codigoDeposito, codigoZona, codigoPasillo, rack.deprack_codigo, columna, nivel);
                        ubicacion.ubilog_depo_id = rack.Deposito != null ? rack.Deposito.depo_id : (int?)null;
                        if (rack.deprack_zonlog_id.HasValue && rack.deprack_zonlog_id.Value > 0)
                            ubicacion.ubilog_zonlog_id = rack.deprack_zonlog_id;
                        if (rack.deprack_pasillo_id.HasValue && rack.deprack_pasillo_id.Value > 0)
                            ubicacion.ubilog_pasillo_id = rack.deprack_pasillo_id;
                        ubicacion.ubilog_deprack_id = rack.deprack_id;
                        ubicacion.ubilog_posicion = columna;
                        ubicacion.ubilog_columna = columna;
                        ubicacion.ubilog_nivel = nivel.ToString();
                        ubicacion.ubilog_coord_x = rack.deprack_x + (anchoColumna * (columna - 1));
                        ubicacion.ubilog_coord_y = rack.deprack_y;
                        ubicacion.ubilog_coord_z = rack.deprack_altura_nivel * (nivel - 1);
                        ubicacion.ubilog_altura = rack.deprack_altura_nivel;
                        ubicacion.ubilog_longitud = anchoColumna;
                        ubicacion.ubilog_anchura = rack.deprack_ancho;
                        ubicacion.ubilog_capacidad_cubica = ubicacion.ubilog_altura * ubicacion.ubilog_longitud * ubicacion.ubilog_anchura;
                        ubicacion.ubilog_volumen_maximo = ubicacion.ubilog_capacidad_cubica;
                        ubicacion.ubilog_peso_maximo = rack.deprack_peso_maximo;
                        ubicacion.ubilog_multiples_articulos = false;
                        ubicacion.ubilog_multiples_lotes = false;
                        Save(ubicacion);
                    }
                }

                oM.Success = true;
                oM.Message = "Rack y ubicaciones generadas correctamente.";
            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.Message = ex.Message;
            }

            return oM;
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
                        ubicacion.ubilog_depo_id = pasillo.Deposito != null ? pasillo.Deposito.depo_id : (int?)null;
                        ubicacion.ubilog_pasillo_id = pasillo.depopas_id;
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
            return MapearSimple(dr);
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
            sQuery = "  SELECT " + sTOP + " * FROM Ubicaciones_Logisticas ";

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

        private string CrearCodigoUbicacion(string codigoPasillo, int posicion, int nivel)
        {
            return codigoPasillo + "-P" + posicion.ToString("000") + "-N" + nivel.ToString("00");
        }

        public void DesactivarUbicacionesRack(int rackID)
        {
            string query = "UPDATE Ubicaciones_Logisticas SET ubilog_activo = 0, ubilog_fec_mod = @fecha, ubilog_usu_id_mod = @usuario WHERE ubilog_deprack_id = @rackID";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("fecha", DateTime.Now));
            parametros.Add(new SqlParameter("usuario", Token.UserID));
            parametros.Add(new SqlParameter("rackID", rackID));
            db.SQLExecuteNonQuery(query, parametros);
        }

        private string CrearCodigoUbicacionRack(string codigoDeposito, string codigoZona, string codigoPasillo, string codigoRack, int columna, int nivel)
        {
            string codigo = "";
            if (!string.IsNullOrWhiteSpace(codigoDeposito)) codigo += codigoDeposito + "-";
            if (!string.IsNullOrWhiteSpace(codigoZona)) codigo += codigoZona + "-";
            if (!string.IsNullOrWhiteSpace(codigoPasillo)) codigo += codigoPasillo + "-";
            codigo += codigoRack;
            codigo += "-C" + columna.ToString("00");
            codigo += "-N" + nivel.ToString("00");
            return codigo;
        }
    }
}
