using Entidades;
using Entidades.App;
using Entidades.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class Encuestas : NegocioBase<Entidades.Encuesta>
    {


        public Encuestas(Entidades.App.Token paramToken) : base("enc_id", "enc_activo", "Encuestas_Cabecera", "enc")
        {
            Token = paramToken;
        }

        #region Funcionalidad

        public override Encuesta ObjetoNuevo()
        {
            Entidades.Encuesta obj = new Entidades.Encuesta();
            obj.enc_id = 0;
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(Convert.ToString(obj.enc_id));
            return obj;
        }

        public override void PermiteGuardar(Encuesta obj)
        {
            if (obj.enc_fec_hasta < DateTime.Now)
                throw new Exception("La fecha ingresada no es válida.");
        }

        public override ObjectMessage Save(Encuesta Obj)
        {
            ObjectMessage oM = new ObjectMessage();
          
            try
            {
                PermiteGuardar(Obj);
                oM = SaveReflection(Obj, true); 
            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.Message = ex.Message;
            }

            return oM;
        }

        public List<Encuesta> ListarPorTipoEncuesta(int tenc_id)
        {
                List<ObjectParameter> paramsFilter = new List<ObjectParameter>();

            if (tenc_id > 0)
                paramsFilter.Add(new ObjectParameter() { Name = "enc_tenc_id", Value = tenc_id });

            List<Entidades.Encuesta> encuestas = new List<Entidades.Encuesta>();

            encuestas = ListarConFiltros(paramsFilter);

            return encuestas;
        }


        #endregion

        #region Consultas y Listados

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public List<Encuesta_Usuario> ListarEncuestasParaUsuarios(int estado, string textoLibre = "")
        {
            List<Encuesta_Usuario> lst = new List<Encuesta_Usuario>();
            List<Encuesta> encuestas = ListarPorTexto(textoLibre);
            Negocio.EncuestasUsuarios encuestasUsuarios = new Negocio.EncuestasUsuarios(Token);
            lst = encuestasUsuarios.EstadoDeEncuestas(encuestas, estado);

            return lst;

        }

        public List<Encuesta> ListarPorTexto(string textoLibre = "")
        {

            List<Entidades.Encuesta> lst = new List<Entidades.Encuesta>();

            string _where = " 1=1 ";

            if (textoLibre.Trim().Length > 0)
                _where += " and (enc_titulo like @textoBusqueda or enc_descripcion like @textoBusqueda)  ";

            _where += " and enc_org_id=@organizacion ";
            _where += " and ( enc_fec_cerrado IS NOT NULL ) ";

            sQuery = QueryDefault("", _where, "");

            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("organizacion",Token.OrganizacionID),
                new System.Data.SqlClient.SqlParameter("textoBusqueda", "%" + textoLibre + "%")
            });

            foreach (DataRow row in dt_bus.Rows)
            {
                lst.Add(Mapear(row));
            }

            return lst;
        }

        public List<Encuesta_Pregunta> ListarPreguntas(Encuesta encuesta)
        {
            List<Encuesta_Pregunta> lst = new List<Encuesta_Pregunta>();
            lst = new Negocio.EncuestasPreguntas(Token).ListarRenglonesPorEncuesta(encuesta.enc_id);

            return lst;
        }

        public List<Encuesta_Respuesta> ListarRespuestas(Encuesta encuesta)
        {
            List<Encuesta_Respuesta> lst = new List<Encuesta_Respuesta>();
            lst = new Negocio.EncuestasRespuestas(Token).ListarRenglonesPorEncuesta(encuesta.enc_id);
            return lst;
        }
        #endregion

        #region Mappers
        public override Encuesta Mapear(DataRow dr)
        {
            Entidades.Encuesta obj = MapearSimple(dr);
            obj.Tipo = Negocio.Tipo_Encuestas.MapearStatic(dr);
            obj.Area = Negocio.App.SIS_Areas.MapearStatic(dr);
            return obj;
        }

        public override Encuesta MapearCompleto(DataRow dr)
        {
            Entidades.Encuesta obj = Mapear(dr);
            return obj;
        }

        public override Encuesta MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static Encuesta MapearStatic(DataRow dr)
        {
            Entidades.Encuesta obj = new Entidades.Encuesta();
            //obj.enc_id = Resources.Validaciones.valNULLINT(dr["enc_id"]);
            //obj.IdEncriptado = Negocio.App.Security.EncriptarID(Convert.ToString(obj.enc_id));
            //obj.Area = new Entidades.App.SIS_Area() { area_id = Resources.Validaciones.valNULLINT(dr["enc_area_id"]) };
            //obj.Area.IdEncriptado = Negocio.App.Security.EncriptarID(Convert.ToString(obj.Area.area_id));
            //obj.Tipo = new Entidades.Tipo_Encuesta() { tenc_id = Resources.Validaciones.valNULLINT(dr["enc_tenc_id"]) };
            //obj.enc_titulo = Resources.Validaciones.valNULLString(dr["enc_titulo"]);
            //obj.enc_descripcion = Resources.Validaciones.valNULLString(dr["enc_descripcion"]);
            //obj.enc_fec_desde = Resources.Validaciones.valNULLDateTime(dr["enc_fec_desde"]);
            //obj.enc_fec_hasta = Resources.Validaciones.valNULLDateTime(dr["enc_fec_hasta"]);
            //obj.enc_activo = Resources.Validaciones.valNULLBool(dr["enc_activo"]);
            obj = MapearReflection(obj, dr);
            if (dr["enc_fec_cerrado"] != DBNull.Value)
                obj.Cerrado = true;
            else
                obj.Cerrado = false;
            return obj;
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM Encuestas_Cabecera ";
            sQuery += " LEFT JOIN Tipo_Encuestas on tenc_id = enc_tenc_id ";
            sQuery += " LEFT JOIN SIS_Areas on area_id = enc_area_id ";

            if ((sWHERE != ""))
            {
                sQuery += " WHERE " + sWHERE;
            }

            if ((sOrderBy != ""))
            {
                sQuery += " ORDER BY " + sOrderBy;
            }

            return sQuery;
        }


        public List<Encuesta> ListarPorTipoEncuestaArea(int tipoEncuestaID, int areaID)
        {
            string _where = "1=1";
            List<Entidades.Encuesta> lst = new List<Entidades.Encuesta>();
            if (tipoEncuestaID > 0)
                _where += " and enc_tenc_id=@tipoEncuestaID ";

            if (areaID > 0)
                _where += " and enc_area_id=@areaID ";


            sQuery = QueryDefault("", _where, "");

            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("tipoEncuestaID",tipoEncuestaID),
                new System.Data.SqlClient.SqlParameter("areaID",areaID )
            });

            foreach (DataRow row in dt_bus.Rows)
            {
                lst.Add(Mapear(row));
            }

            return lst;
        }

        #endregion

        #region Funcionalidad Especial

        public Entidades.Encuesta_Usuario InicializarIntento(Encuesta encuesta)
        {
            Negocio.EncuestasUsuarios negocioENCUSU = new EncuestasUsuarios(Token);
            Entidades.Encuesta_Usuario encuestaUsuario = negocioENCUSU.ObtenerPorEncuestaUsuario(encuesta,
                Token.UserID);

            if (encuestaUsuario.encusu_id.Equals(0))
            {
                negocioENCUSU.Save(encuestaUsuario);
            }
            return new Negocio.EncuestasUsuarios(Token).ObtenerPorEncuestaUsuario(encuesta,
                Token.UserID);
        }

        public int CantidadDeRespuestas(string enc_id)
        {
            string sQuery = "SELECT COUNT(*) AS cantidad FROM Encuestas_Respuestas WHERE encres_enc_id=@enc_id ";
            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("enc_id",enc_id)
            });
            return Resources.Validaciones.valNULLINT(dt_bus.Rows[0]["cantidad"]);
        }

        public int CantidadDePreguntas(string enc_id)
        {
            string sQuery = "SELECT COUNT(*) AS cantidad FROM Encuestas_Preguntas WHERE encpreg_enc_id=@enc_id ";
            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("enc_id",enc_id)
            });
            return Resources.Validaciones.valNULLINT(dt_bus.Rows[0]["cantidad"]);
        }

        public Entidades.App.ObjectMessage CerrarEncuesta(string enc_id)
        {
            Entidades.App.ObjectMessage oM = new ObjectMessage();

            if (CantidadDePreguntas(enc_id).Equals(0))
            {
                oM.Success = false;
                oM.Message = "No puede cerrar una Encuesta sin respuestas";
                return oM;
            }

            if (CantidadDeRespuestas(enc_id).Equals(0))
            {
                oM.Success = false;
                oM.Message = "No puede cerrar una Encuesta sin preguntas";
                return oM;
            }

            try
            {
                Entidades.Encuesta datosProyecto = ObtenerPorID(enc_id, true);

                db.SQLExecuteNonQuery("UPDATE Encuestas_Cabecera SET enc_fec_cerrado=GETDATE() WHERE enc_id=@enc_id", new List<System.Data.SqlClient.SqlParameter>() {
                    new System.Data.SqlClient.SqlParameter("enc_id",enc_id)
                });

                oM.Success = true;
                oM.Message = "OK";
            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.Message = ex.Message;
            }

            return oM;
        }

        public Entidades.Vistas.EncuestaResultado ResultadoEncuesta(string enc_id)
        {
            Entidades.Encuesta encuesta = ObtenerPorID(enc_id);
            Entidades.Vistas.EncuestaResultado resultado = new Entidades.Vistas.EncuestaResultado();
            resultado.Encuesta = encuesta;
            resultado.CantidadUsuariosActualmente = CantidadUsuariosActualmente(enc_id);
            resultado.CantidadUsuariosFinalizaron = CantidadUsuariosFinalizaron(enc_id);
            resultado.CantidadUsuariosEnProgreso = CantidadUsuariosEnProgreso(enc_id);
            resultado.TiempoPromedio = CalcularTiempoPromedio(encuesta);
            return resultado;
        }

        public int CantidadUsuariosActualmente(string enc_id)
        {
            string sQuery = "SELECT COUNT(*) AS cantidad FROM Encuestas_Usuarios WHERE encusu_enc_id=@enc_id" +
                " AND convert(varchar(10), encusu_fec_ini, 102) = convert(varchar(10), GETDATE(), 102) ";
            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("enc_id",Negocio.App.Security.DesencriptarID(enc_id))
            });
            return Resources.Validaciones.valNULLINT(dt_bus.Rows[0]["cantidad"]);
        }

        public int CantidadUsuariosFinalizaron(string enc_id)
        {
            string sQuery = "SELECT COUNT(*) AS cantidad FROM Encuestas_Usuarios WHERE encusu_enc_id=@enc_id" +
                " AND encusu_fec_fin IS NOT NULL ";
            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("enc_id",Negocio.App.Security.DesencriptarID(enc_id))
            });
            return Resources.Validaciones.valNULLINT(dt_bus.Rows[0]["cantidad"]);
        }

        public int CantidadUsuariosEnProgreso(string enc_id)
        {
            string sQuery = "SELECT COUNT(*) AS cantidad FROM Encuestas_Usuarios WHERE encusu_enc_id=@enc_id" +
                " AND encusu_fec_fin IS NULL ";
            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("enc_id",Negocio.App.Security.DesencriptarID(enc_id))
            });
            return Resources.Validaciones.valNULLINT(dt_bus.Rows[0]["cantidad"]);
        }

        public List<Entidades.Vistas.PreguntaCantidad> ListarPreguntasPorResultado(Encuesta encuesta, bool ascen = false)
        {
            Negocio.EncuestasUsuarios encuestasUsuarios = new Negocio.EncuestasUsuarios(Token);
            List<Entidades.Vistas.PreguntaCantidad> lst = new List<Entidades.Vistas.PreguntaCantidad>();
            List<Encuesta_Pregunta> preguntas = ListarPreguntas(encuesta);
            foreach (Encuesta_Pregunta pregunta in preguntas)
            {
                Entidades.Vistas.PreguntaCantidad preguntaCantidad = new Entidades.Vistas.PreguntaCantidad
                {
                    Cantidad = encuestasUsuarios.PuntajePregunta(pregunta),
                    Pregunta = pregunta
                };
                lst.Add(preguntaCantidad);
            }
            if (ascen)
                return lst.OrderBy(x => x.Cantidad).ToList();
            else
                return lst.OrderByDescending(x => x.Cantidad).ToList();
        }

        public List<Entidades.Vistas.PreguntaCantidad> ListarPreguntasCantidadRespuestas(Encuesta encuesta)
        {
            List<Entidades.Vistas.PreguntaCantidad> lst = new List<Entidades.Vistas.PreguntaCantidad>();
            List<Encuesta_Pregunta> preguntas = ListarPreguntas(encuesta);
            foreach (Encuesta_Pregunta pregunta in preguntas)
            {
                Entidades.Vistas.PreguntaCantidad preguntaCantidad = new Entidades.Vistas.PreguntaCantidad
                {
                    Cantidad = CantidadDeRespuestasPorPregunta(pregunta),
                    Pregunta = pregunta
                };
                lst.Add(preguntaCantidad);
            }
            return lst.OrderBy(x => x.Cantidad).ToList();
        }


        decimal CalcularTiempoPromedio(Encuesta encuesta)
        {
            double acumulador = 0;
            List<Encuesta_Usuario> lst = new List<Encuesta_Usuario>();
            Negocio.EncuestasUsuarios encuestasUsuarios = new EncuestasUsuarios(Token);
            lst = encuestasUsuarios.ListarPorEncuesta(encuesta.enc_id);

            foreach (Encuesta_Usuario elemento in lst)
            {
                acumulador += (elemento.encusu_fec_fin - elemento.encusu_fec_ini).TotalSeconds;
            }
            if (lst.Count>0)
                return (decimal)acumulador / lst.Count();
            return 0;
        }

        int CantidadDeRespuestasPorPregunta(Encuesta_Pregunta pregunta)
        {
            List<Encuesta_Usuario_Respuesta> lst = new Negocio.EncuestasUsuariosRespuestas(Token).ListarPorPregunta(pregunta.encpreg_id);
            return lst.Count();
        }

        public List<Entidades.Vistas.TarjetaCabecera> ArmarCabeceraInforme (string usuActual, string usuProgre, string usuFin, string tiempoPromedio)
        {
            List<Entidades.Vistas.TarjetaCabecera> lst = new List<Entidades.Vistas.TarjetaCabecera>();
            lst.Add(new Entidades.Vistas.TarjetaCabecera
            {
                titulo = "Finalizados",
                descripcion = "Cantidad de Usuarios que finalizaron la encuesta. ",
                span = "Usuarios",
                valor = Convert.ToInt32(usuFin)
            });

            lst.Add(new Entidades.Vistas.TarjetaCabecera
            {
                titulo = "Actualmente",
                descripcion = "Cantidad de Usuarios que realizaron la encuesta en el día. ",
                span = "Usuarios",
                valor = Convert.ToInt32(usuActual)
            });
            lst.Add(new Entidades.Vistas.TarjetaCabecera
            {
                titulo = "En Progreso",
                descripcion = "Cantidad de Usuarios que no finalizaron la encuesta todavía. ",
                span = "Usuarios",
                valor = Convert.ToInt32(usuProgre)
            });

            lst.Add(new Entidades.Vistas.TarjetaCabecera
            {
                titulo = "Duración",
                descripcion = "Tiempo promedio que tardan los usuarios en realizar la encuesta. ",
                span = "Segundos",
                valor = Convert.ToInt32(usuFin)
            });
            return lst;
        }

        #endregion
    }
}
