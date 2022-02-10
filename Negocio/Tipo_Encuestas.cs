using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using Entidades.App;

namespace Negocio
{
    public class Tipo_Encuestas : NegocioBase<Entidades.Tipo_Encuesta>
    {
        public Tipo_Encuestas(Entidades.App.Token paramToken) : base("tenc_id", "sin", "Tipo_Encuestas", "tenc")
        {
            Token = paramToken;
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public override Tipo_Encuesta Mapear(DataRow dr)
        {
            Entidades.Tipo_Encuesta obj = MapearSimple(dr);
                               
            return obj;
        }

        public override Tipo_Encuesta MapearCompleto(DataRow dr)
        {
            Entidades.Tipo_Encuesta obj = Mapear(dr);
            return obj;
        }

        public override Tipo_Encuesta MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static Tipo_Encuesta MapearStatic(DataRow dr)
        {
            Entidades.Tipo_Encuesta obj = new Entidades.Tipo_Encuesta();
            obj.tenc_id = Resources.Validaciones.valNULLINT(dr["tenc_id"]);
            obj.tenc_contenido = Resources.Validaciones.valNULLString(dr["tenc_contenido"]);

            return obj;
        }

        public override Tipo_Encuesta ObjetoNuevo()
        {
            Entidades.Tipo_Encuesta obj = new Entidades.Tipo_Encuesta();
            return obj;
        }

        
        public override ObjectMessage Save(Tipo_Encuesta Obj)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                PermiteGuardar(Obj);
                DataRow row = db.Estructura("Tipo_Encuestas");
                row["tenc_contenido"] = Obj.tenc_contenido;

                if (Obj.tenc_id.Equals(0))
                {
                    Obj.tenc_id = db.SQLInsert(row, "tenc_id").Valor;
                    oM.Message = "Datos ingresados";
                }
                else
                {
                    db.SQLUpdate(row, "tenc_id=@tenc_id", "tenc_id", new List<System.Data.SqlClient.SqlParameter>() {
                        new System.Data.SqlClient.SqlParameter("tenc_id",Obj.tenc_id)
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


        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM Tipo_Encuestas ";
          
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
      
    }
}
