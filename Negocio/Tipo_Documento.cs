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
    public class Tipo_Documento : NegocioBase<Entidades.Tipo_Documento>
    {
                
        public Tipo_Documento(Entidades.App.Token paramToken) : base("tid_id", "tid_activo", "Tipo_Documento", "tid")
        {
            Token = paramToken;
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public override Entidades.Tipo_Documento Mapear(DataRow dr)
        {
            Entidades.Tipo_Documento obj = MapearSimple(dr);
            return obj;
        }

        public override Entidades.Tipo_Documento MapearCompleto(DataRow dr)
        {
            Entidades.Tipo_Documento obj = Mapear(dr);
            return obj;
        }

        public override Entidades.Tipo_Documento MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static Entidades.Tipo_Documento MapearStatic(DataRow dr)
        {
            Entidades.Tipo_Documento obj = new Entidades.Tipo_Documento();
            obj.tid_id = Resources.Validaciones.valNULLINT(dr["tid_id"]);
            obj.tid_nombre = Resources.Validaciones.valNULLString(dr["tid_nombre"]);
            return obj;
        }

        public override Entidades.Tipo_Documento ObjetoNuevo()
        {
            Entidades.Tipo_Documento obj = new Entidades.Tipo_Documento();
            return obj;
        }

        public override ObjectMessage Save(Entidades.Tipo_Documento Obj)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                DataRow row = db.Estructura(nombreTablaPrincipal);
                row["tid_nombre"] = Obj.tid_nombre;
          
                if (Obj.tid_id.Equals(0))
                {
                    row["tid_usu_id_alta"] = Convert.ToInt32(Token.UserID);
                    row["tid_fec_alta"] = DateTime.Now;
                    Obj.tid_id = db.SQLInsert(row, "tid_id").Valor;
                    oM.Message = "Datos ingresados";
                }
                else
                {
                    row["tid_usu_id_mod"] = Convert.ToInt32(Token.UserID);
                    row["tid_fec_mod"] = DateTime.Now;
                    db.SQLUpdate(row, "tid_id=@tid_id", "tid_id", new List<System.Data.SqlClient.SqlParameter>() {
                        new System.Data.SqlClient.SqlParameter("tid_id",Obj.tid_id)
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
            sQuery = "Select * from Tipo_Documento ";

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
