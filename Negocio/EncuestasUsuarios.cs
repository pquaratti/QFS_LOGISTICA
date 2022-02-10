using Entidades;
using Entidades.App;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class EncuestasUsuarios : NegocioBase<Entidades.Encuesta_Usuario>
    {
        public enum Estados
        {
            Realizadas = 1,
            Disponibles = 2,

        }

        Negocio.Encuestas negocioENC;

        public EncuestasUsuarios(Entidades.App.Token paramToken) : base("encusu_id", "sin", "Encuestas_Usuarios", "encusu")
        {
            Token = paramToken;
            negocioENC = new Encuestas(Token);
            TokenFilter = true;
        }

        #region Maps
        public override Encuesta_Usuario Mapear(DataRow dr)
        {
            Entidades.Encuesta_Usuario obj = MapearSimple(dr);
            obj.Encuesta = negocioENC.MapearSimple(dr);

            return obj;
        }

        public override Encuesta_Usuario MapearCompleto(DataRow dr)
        {
            Entidades.Encuesta_Usuario obj = Mapear(dr);
            return obj;
        }

        public override Encuesta_Usuario MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static Encuesta_Usuario MapearStatic(DataRow dr)
        {
            Entidades.Encuesta_Usuario obj = new Entidades.Encuesta_Usuario();
            obj.encusu_id = Resources.Validaciones.valNULLINT(dr["encusu_id"]);
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(Convert.ToString(obj.encusu_id));
            obj.Encuesta = new Entidades.Encuesta() { enc_id = Resources.Validaciones.valNULLINT(dr["encusu_enc_id"]) };
            obj.Encuesta.IdEncriptado = Negocio.App.Security.EncriptarID(Convert.ToString(obj.Encuesta.enc_id));
            obj.Usuario = new Entidades.App.SIS_Usuario() { usu_id = Resources.Validaciones.valNULLINT(dr["encusu_usu_id"]) };
            obj.Usuario.IdEncriptado = Negocio.App.Security.EncriptarID(Convert.ToString(obj.Usuario.usu_id));
            obj.encusu_fec_ini = Resources.Validaciones.valNULLDateTime(dr["encusu_fec_ini"]);
            obj.encusu_fec_fin = Resources.Validaciones.valNULLDateTime(dr["encusu_fec_fin"]);
            if (obj.encusu_fec_fin.Equals(DateTime.MinValue))
                obj.EstadoActual = (int)Estados.Disponibles;
            else
                obj.EstadoActual = (int)Estados.Realizadas;
            return obj;
        }

        #endregion

        #region Save
        public override Encuesta_Usuario ObjetoNuevo()
        {
            Encuesta_Usuario obj = new Encuesta_Usuario();
            obj.encusu_id = 0;
            obj.EstadoActual = (int)Estados.Disponibles;
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(Convert.ToString(obj.encusu_id));
            return obj;
        }

        public override void PermiteGuardar(Encuesta_Usuario Obj)
        {
            

        }

        public override void DatosObligatorios(Encuesta_Usuario Obj)
        {

        }

        public override ObjectMessage Save(Encuesta_Usuario Obj)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                PermiteGuardar(Obj);
                DataRow row = db.Estructura("Encuestas_Usuarios");
                
                if (Obj.encusu_id.Equals(0))
                {
                    row["encusu_enc_id"] = Obj.Encuesta.enc_id;
                    row["encusu_usu_id"] = Obj.Usuario.usu_id;
                    row["encusu_org_id"] = Token.OrganizacionID;
                    row["encusu_fec_ini"] = DateTime.Now;
                    Obj.encusu_id = db.SQLInsert(row, "encusu_id").Valor;
                    Obj.IdEncriptado = Negocio.App.Security.EncriptarID(Obj.encusu_id.ToString());
                    oM.Message = "Datos ingresados";
                }
                else
                {
                    row["encusu_fec_fin"] = DateTime.Now;
                    db.SQLUpdate(row, "encusu_id=@encusu_id", "encusu_id", new List<System.Data.SqlClient.SqlParameter>() {
                        new System.Data.SqlClient.SqlParameter("encusu_id",Obj.encusu_id)
                    });

                    oM.Message = "Datos actualizados";
                }

                oM.Success = true;
            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.Message = ex.Message;
            }

            return oM;
        }
        #endregion

        #region Listar / Funcionalidad Especial

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public List<Encuesta_Usuario> EstadoDeEncuestas(List<Encuesta> encuestas, int estado)
        {
            List<Encuesta_Usuario> lst = new List<Encuesta_Usuario>();
            foreach (Encuesta encuesta in encuestas)
            {
                Encuesta_Usuario encusu = new Encuesta_Usuario();
                encusu = ObtenerPorEncuestaUsuario(encuesta, Token.UserID);
                if (estado.Equals(0) | estado.Equals(encusu.EstadoActual))
                    lst.Add(encusu);
            }  
            return lst;

        }

        public List<Encuesta_Usuario_Respuesta> ListarSeleccionesPorIntento (string encusu)
        {
            return new Negocio.EncuestasUsuariosRespuestas(Token).ListarRenglonesPorEncuesta(Convert.ToInt32(encusu));
        }

        public Encuesta_Usuario ObtenerPorEncuestaUsuario(Encuesta encuesta, string usu_id)
        {

            List<ObjectParameter> paramsFilter = new List<ObjectParameter>();

            paramsFilter.Add(new ObjectParameter() { Name = "encusu_enc_id", Value = encuesta.enc_id });
            paramsFilter.Add(new ObjectParameter() { Name = "encusu_usu_id", Value = usu_id });

            List<Encuesta_Usuario> lst = ListarConFiltros(paramsFilter);

            if (lst.Count > 0)
            {
                return lst[0];
            }
            else
            {
                Encuesta_Usuario encusu = ObjetoNuevo();
                encusu.Usuario.usu_id = Convert.ToInt32(usu_id);
                encusu.Encuesta = encuesta;
                return encusu;
            }

        }

        public List<Entidades.Encuesta_Usuario> ListarPorEncuesta(int enc_id)
        {
            List<ObjectParameter> paramsFilter = new List<ObjectParameter>();
            if (enc_id > 0)
                paramsFilter.Add(new ObjectParameter() { Name = "encusu_enc_id", Value = enc_id });
            List<Entidades.Encuesta_Usuario> renglones = new List<Entidades.Encuesta_Usuario>();
            renglones = ListarConFiltros(paramsFilter);
            return renglones;

        }
        #endregion

        #region Querys

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM Encuestas_Usuarios ";
            sQuery += " LEFT JOIN Encuestas_Cabecera ON enc_id=encusu_enc_id ";
            sQuery += " LEFT JOIN SIS_Organizaciones on org_id=encusu_org_id ";

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

        #endregion

        #region Funcionalidad Particular

        public void GuardarIntento(List<string> respuestas, int totalPreguntas, string intento)
        {
            try
            {
                Entidades.Encuesta_Usuario datosIntento = ObtenerPorID(intento);

                if (respuestas == null) 
                    throw new Exception("No se ha seleccionado ninguna respuesta!");

                if (respuestas.Count < totalPreguntas)
                    throw new Exception("Existen preguntas sin responder!");


                List<Entidades.Encuesta_Usuario_Respuesta> lstRespuestas = new List<Entidades.Encuesta_Usuario_Respuesta>();

                foreach (var item in respuestas)
                {
                    var _preguntaID = Negocio.App.Security.GetID(item.Split('_')[0].ToString());
                    var _respuestaID = Negocio.App.Security.GetID(item.Split('_')[1].ToString());

                    Entidades.Encuesta_Usuario_Respuesta itemRespuesta = new Entidades.Encuesta_Usuario_Respuesta();
                    itemRespuesta.Intento = datosIntento;
                    itemRespuesta.Pregunta.encpreg_id = _preguntaID;
                    itemRespuesta.Respuesta.encres_id = _respuestaID;
                    lstRespuestas.Add(itemRespuesta);
                }

                RegistrarRespuestas(datosIntento, lstRespuestas);
                FinalizarIntento(datosIntento);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RegistrarRespuestas(Encuesta_Usuario datosIntento, List<Entidades.Encuesta_Usuario_Respuesta> respuestas)
        {
            try
            {
                Negocio.EncuestasUsuariosRespuestas negRta = new EncuestasUsuariosRespuestas(Token);

                foreach (var item in respuestas)
                {
                    item.Intento = datosIntento;
                    negRta.Save(item);
                }

                FinalizarIntento(datosIntento);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void FinalizarIntento(Encuesta_Usuario datosIntento)
        {
            try
            {
                db.SQLExecuteNonQuery("UPDATE Encuestas_Usuarios SET encusu_fec_fin=@encusu_fec_fin WHERE encusu_id=@encusu_id", new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("encusu_fec_fin",DateTime.Now),
                    new System.Data.SqlClient.SqlParameter("encusu_id",datosIntento.encusu_id)
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public int PuntajePregunta(Encuesta_Pregunta pregunta)
        {

            int acumulador = 0;
            List<Entidades.Encuesta_Usuario_Respuesta> lst = new Negocio.EncuestasUsuariosRespuestas(Token).ListarPorPregunta(pregunta.encpreg_id);
            foreach (Encuesta_Usuario_Respuesta elemento in lst)
                acumulador += elemento.Respuesta.encres_valor;
            return acumulador;
        }

        #endregion

    }
}
