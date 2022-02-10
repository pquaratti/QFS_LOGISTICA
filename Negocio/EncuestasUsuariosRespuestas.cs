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
    public class EncuestasUsuariosRespuestas : NegocioBase<Entidades.Encuesta_Usuario_Respuesta>
    {

        public EncuestasUsuariosRespuestas(Entidades.App.Token paramToken) : base("eur_id", "sin", "Encuestas_Usuarios_Respuestas", "eur")
        {
            Token = paramToken;
        }

        #region Maps
        public override Encuesta_Usuario_Respuesta Mapear(DataRow dr)
        {
            Entidades.Encuesta_Usuario_Respuesta obj = MapearSimple(dr);
            obj.Pregunta = Negocio.EncuestasPreguntas.MapearStatic(dr);
            obj.Respuesta =Negocio.EncuestasRespuestas.MapearStatic(dr);
            obj.Intento = Negocio.EncuestasUsuarios.MapearStatic(dr);

            return obj;
        }

        public override Encuesta_Usuario_Respuesta MapearCompleto(DataRow dr)
        {
            Entidades.Encuesta_Usuario_Respuesta obj = Mapear(dr);
            return obj;
        }

        public override Encuesta_Usuario_Respuesta MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static Encuesta_Usuario_Respuesta MapearStatic(DataRow dr)
        {
            Entidades.Encuesta_Usuario_Respuesta obj = new Entidades.Encuesta_Usuario_Respuesta();
            obj.eur_fecha = Resources.Validaciones.valNULLDateTime(dr["eur_fecha"]);
            obj.eur_id = Resources.Validaciones.valNULLINT(dr["eur_id"]);
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(Convert.ToString(obj.eur_id));
            obj.Pregunta = new Entidades.Encuesta_Pregunta() { encpreg_id = Resources.Validaciones.valNULLINT(dr["eur_encpreg_id"]) };
            obj.Pregunta.IdEncriptado = Negocio.App.Security.EncriptarID(Convert.ToString(obj.Pregunta.encpreg_id));
            obj.Respuesta = new Entidades.Encuesta_Respuesta() { encres_id = Resources.Validaciones.valNULLINT(dr["eur_encres_id"]) };
            obj.Respuesta.IdEncriptado = Negocio.App.Security.EncriptarID(Convert.ToString(obj.Respuesta.encres_id));
            obj.Intento = new Entidades.Encuesta_Usuario() { encusu_id = Resources.Validaciones.valNULLINT(dr["eur_encusu_id"]) };
            obj.Intento.IdEncriptado = Negocio.App.Security.EncriptarID(Convert.ToString(obj.Intento.encusu_id));

            return obj;
        }

        #endregion

        #region Save
        public override Encuesta_Usuario_Respuesta ObjetoNuevo()
        {
            Encuesta_Usuario_Respuesta obj = new Encuesta_Usuario_Respuesta();
            obj.eur_id = 0;
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(Convert.ToString(obj.eur_id));
            return obj;
        }

        public override void PermiteGuardar(Encuesta_Usuario_Respuesta Obj)
        {

        }

        public override void DatosObligatorios(Encuesta_Usuario_Respuesta Obj)
        {

        }

        public override ObjectMessage Save(Encuesta_Usuario_Respuesta Obj)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                PermiteGuardar(Obj);
                DataRow row = db.Estructura("Encuestas_Usuarios_Respuestas");
                row["eur_id"] = Obj.eur_id;
                row["eur_encpreg_id"] = Obj.Pregunta.encpreg_id;
                row["eur_encres_id"] = Obj.Respuesta.encres_id;
                row["eur_encusu_id"] = Obj.Intento.encusu_id;
                row["eur_fecha"] = DateTime.Now;

                if (Obj.eur_id.Equals(0))
                {
                    Obj.eur_id = db.SQLInsert(row, "eur_id").Valor;
                    oM.Message = "Datos ingresados";
                }
                else
                {
                    db.SQLUpdate(row, "eur_id=@eur_id", "eur_id", new List<System.Data.SqlClient.SqlParameter>() {
                        new System.Data.SqlClient.SqlParameter("eur_id",Obj.eur_id)
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

        #region Listar

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public List<Entidades.Encuesta_Usuario_Respuesta> ListarRenglonesPorEncuesta(int encusu_id)
        {

            List<ObjectParameter> paramsFilter = new List<ObjectParameter>();

            if (encusu_id > 0)
                paramsFilter.Add(new ObjectParameter() { Name = "eur_encusu_id", Value = encusu_id });

            List<Entidades.Encuesta_Usuario_Respuesta> renglones = new List<Entidades.Encuesta_Usuario_Respuesta>();

            renglones = ListarConFiltros(paramsFilter);

            return renglones;

        }

        public List<Entidades.Encuesta_Usuario_Respuesta> ListarPorPregunta(int preg_id)
        {

            List<ObjectParameter> paramsFilter = new List<ObjectParameter>();

            if (preg_id > 0)
                paramsFilter.Add(new ObjectParameter() { Name = "eur_encpreg_id", Value = preg_id });

            List<Entidades.Encuesta_Usuario_Respuesta> renglones = new List<Entidades.Encuesta_Usuario_Respuesta>();

            renglones = ListarConFiltros(paramsFilter);

            return renglones;

        }

        #endregion

        #region Querys

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM Encuestas_Usuarios_Respuestas ";
            sQuery += " LEFT JOIN Encuestas_Preguntas ON encpreg_id=eur_encpreg_id ";
            sQuery += " LEFT JOIN Encuestas_Respuestas ON encres_id=eur_encres_id ";
            sQuery += " LEFT JOIN Encuestas_Usuarios ON encusu_id=eur_encusu_id ";



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

    }
}
