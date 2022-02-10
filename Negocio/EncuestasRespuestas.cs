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
    public class EncuestasRespuestas : NegocioBase<Entidades.Encuesta_Respuesta>
    {
        Negocio.Encuestas negocioENC;

        public EncuestasRespuestas(Entidades.App.Token paramToken) : base("encres_id", "sin", "Encuestas_Respuestas", "ever")
        {
            Token = paramToken;
            negocioENC = new Encuestas(Token);
        }

        #region Maps
        public override Encuesta_Respuesta Mapear(DataRow dr)
        {
            Entidades.Encuesta_Respuesta obj = MapearSimple(dr);
            obj.Encuesta = negocioENC.MapearSimple(dr);
         
            return obj;
        }

        public override Encuesta_Respuesta MapearCompleto(DataRow dr)
        {
            Entidades.Encuesta_Respuesta obj = Mapear(dr);
            return obj;
        }

        public override Encuesta_Respuesta MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static Encuesta_Respuesta MapearStatic(DataRow dr)
        {
            Entidades.Encuesta_Respuesta obj = new Entidades.Encuesta_Respuesta();
            obj.encres_id = Resources.Validaciones.valNULLINT(dr["encres_id"]);
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(Convert.ToString(obj.encres_id));
            obj.Encuesta = new Entidades.Encuesta() { enc_id = Resources.Validaciones.valNULLINT(dr["encres_enc_id"]) };
            obj.Encuesta.IdEncriptado = Negocio.App.Security.EncriptarID(Convert.ToString(obj.Encuesta.enc_id));
            obj.encres_contenido = Resources.Validaciones.valNULLString(dr["encres_contenido"]);
            obj.encres_valor = Resources.Validaciones.valNULLINT(dr["encres_valor"]);

            return obj;
        }

        #endregion

        #region Save
        public override Encuesta_Respuesta ObjetoNuevo()
        {
            Encuesta_Respuesta obj = new Encuesta_Respuesta();
            obj.encres_id = 0;
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(Convert.ToString(obj.encres_id));
            return obj;
        }

        public override void PermiteGuardar(Encuesta_Respuesta Obj)
        {
            if (!(Obj.encres_contenido.Length > 0))
                throw new Exception("La pregunta es inválida.");

        }

        public override void DatosObligatorios(Encuesta_Respuesta Obj)
        {

        }

        public override ObjectMessage Save(Encuesta_Respuesta Obj)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                PermiteGuardar(Obj);
                DataRow row = db.Estructura("Encuestas_Respuestas");
                row["encres_id"] = Obj.encres_id;
                row["encres_enc_id"] = Obj.Encuesta.enc_id;
                row["encres_contenido"] = Obj.encres_contenido;


                if (Obj.encres_id.Equals(0))
                {
                    Obj.encres_id = db.SQLInsert(row, "encres_id").Valor;
                    oM.Message = "Datos ingresados";
                }
                else
                {
                    db.SQLUpdate(row, "encres_id=@encres_id", "encres_id", new List<System.Data.SqlClient.SqlParameter>() {
                        new System.Data.SqlClient.SqlParameter("encres_id",Obj.encres_id)
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

        public List<Entidades.Encuesta_Respuesta> ListarRenglonesPorEncuesta(int enc_id)
        {

            sQuery = QueryDefault("", "", "");

            List<ObjectParameter> paramsFilter = new List<ObjectParameter>();

            if (enc_id > 0)
                paramsFilter.Add(new ObjectParameter() { Name = "encres_enc_id", Value = enc_id });

            List<Entidades.Encuesta_Respuesta> renglones = new List<Entidades.Encuesta_Respuesta>();

            renglones = ListarConFiltros(paramsFilter);

            return renglones;

        }

        #endregion

        #region Querys

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM Encuestas_Respuestas ";
            sQuery += " LEFT JOIN Encuestas_Cabecera ON enc_id=encres_enc_id ";



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
