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
    public class EncuestasPreguntas : NegocioBase<Entidades.Encuesta_Pregunta>
    {
        Negocio.Encuestas negocioENC;

        public EncuestasPreguntas(Entidades.App.Token paramToken) : base("encpreg_id", "sin", "Encuestas_Preguntas", "ever")
        {
            Token = paramToken;
            negocioENC = new Encuestas(Token);
        }

        #region Maps
        public override Encuesta_Pregunta Mapear(DataRow dr)
        {
            Entidades.Encuesta_Pregunta obj = MapearSimple(dr);
            obj.Encuesta = negocioENC.MapearSimple(dr);
         
            return obj;
        }

        public override Encuesta_Pregunta MapearCompleto(DataRow dr)
        {
            Entidades.Encuesta_Pregunta obj = Mapear(dr);
            return obj;
        }

        public override Encuesta_Pregunta MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static Encuesta_Pregunta MapearStatic(DataRow dr)
        {
            Entidades.Encuesta_Pregunta obj = new Entidades.Encuesta_Pregunta();
            obj.encpreg_id = Resources.Validaciones.valNULLINT(dr["encpreg_id"]);
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(Convert.ToString(obj.encpreg_id));
            obj.Encuesta = new Entidades.Encuesta() { enc_id = Resources.Validaciones.valNULLINT(dr["encpreg_enc_id"]) };
            obj.Encuesta.IdEncriptado = Negocio.App.Security.EncriptarID(Convert.ToString(obj.Encuesta.enc_id));
            obj.encpreg_contenido = Resources.Validaciones.valNULLString(dr["encpreg_contenido"]);

            return obj;
        }

        #endregion

        #region Save
        public override Encuesta_Pregunta ObjetoNuevo()
        {
            Entidades.Encuesta_Pregunta obj = new Entidades.Encuesta_Pregunta();
            obj.encpreg_id = 0;
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(Convert.ToString(obj.encpreg_id));
            return obj;
        }

        public override void PermiteGuardar(Encuesta_Pregunta Obj)
        {
            if (!(Obj.encpreg_contenido.Length > 0))
                throw new Exception("La pregunta es inválida.");

        }

        public override void DatosObligatorios(Encuesta_Pregunta Obj)
        {

        }

        public override ObjectMessage Save(Encuesta_Pregunta Obj)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                PermiteGuardar(Obj);
                DataRow row = db.Estructura("Encuestas_Preguntas");
                row["encpreg_id"] = Obj.encpreg_id;
                row["encpreg_enc_id"] = Obj.Encuesta.enc_id;
                row["encpreg_contenido"] = Obj.encpreg_contenido;


                if (Obj.encpreg_id.Equals(0))
                {
                    Obj.encpreg_id = db.SQLInsert(row, "encpreg_id").Valor;
                    oM.Message = "Datos ingresados";
                }
                else
                {
                    db.SQLUpdate(row, "encpreg_id=@encpreg_id", "encpreg_id", new List<System.Data.SqlClient.SqlParameter>() {
                        new System.Data.SqlClient.SqlParameter("encpreg_id",Obj.encpreg_id)
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

        public List<Entidades.Encuesta_Pregunta> ListarRenglonesPorEncuesta(int enc_id)
        {

            sQuery = QueryDefault("", "", "");

            List<ObjectParameter> paramsFilter = new List<ObjectParameter>();

            if (enc_id > 0)
                paramsFilter.Add(new ObjectParameter() { Name = "encpreg_enc_id", Value = enc_id });

            List<Entidades.Encuesta_Pregunta> renglones = new List<Entidades.Encuesta_Pregunta>();

            renglones = ListarConFiltros(paramsFilter);

            return renglones;

        }

        #endregion

        #region Querys

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM Encuestas_Preguntas ";
            sQuery += " LEFT JOIN Encuestas_Cabecera ON enc_id=encpreg_enc_id ";



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
