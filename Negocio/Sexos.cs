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
    public class Sexos : NegocioBase<Entidades.Sexo>
    {
                
        public Sexos(Entidades.App.Token paramToken) : base("sex_id", "sex_activo", "Sexos", "sex")
        {
            Token = paramToken;
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public override Entidades.Sexo Mapear(DataRow dr)
        {
            Entidades.Sexo obj = MapearSimple(dr);
            return obj;
        }

        public override Entidades.Sexo MapearCompleto(DataRow dr)
        {
            Entidades.Sexo obj = Mapear(dr);
            return obj;
        }

        public override Entidades.Sexo MapearSimple(DataRow dr)
        {
            Entidades.Sexo obj = new Entidades.Sexo();
            obj.sex_id = Resources.Validaciones.valNULLINT(dr["sex_id"]);
            obj.sex_nombre = Resources.Validaciones.valNULLString(dr["sex_nombre"]);
            obj.sex_abreviatura = Resources.Validaciones.valNULLString(dr["sex_abreviatura"]);
            return obj;
        }

        public override Entidades.Sexo ObjetoNuevo()
        {
            Entidades.Sexo obj = new Entidades.Sexo();
            return obj;
        }

        public override ObjectMessage Save(Entidades.Sexo Obj)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                DataRow row = db.Estructura(nombreTablaPrincipal);
                row["sex_nombre"] = Obj.sex_nombre;
                row["sex_abreviatura"] = Obj.sex_abreviatura;
          
                if (Obj.sex_id.Equals(0))
                {
                    row["sex_usu_id_alta"] = Convert.ToInt32(Token.UserID);
                    row["sex_fec_alta"] = DateTime.Now;
                    Obj.sex_id = db.SQLInsert(row, "sex_id").Valor;
                    oM.Message = "Datos ingresados";
                }
                else
                {
                    row["sex_usu_id_mod"] = Convert.ToInt32(Token.UserID);
                    row["sex_fec_mod"] = DateTime.Now;
                    db.SQLUpdate(row, "sex_id=@sex_id", "sex_id", new List<System.Data.SqlClient.SqlParameter>() {
                        new System.Data.SqlClient.SqlParameter("sex_id",Obj.sex_id)
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
            sQuery = "Select * from Sexos ";

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
