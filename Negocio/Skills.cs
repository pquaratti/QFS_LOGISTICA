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
    public class Skills : NegocioBase<Entidades.Skill>
    {
        public Skills(Entidades.App.Token paramToken) : base("skill_id", "skill_activo", "Skills", "skill") 
        {
            Token = paramToken;
        }

        public override Entidades.App.ObjectMessage Save(Entidades.Skill Obj)
        {
            Entidades.App.ObjectMessage oM = new ObjectMessage();

            try
            {
                DataRow row = db.Estructura("Skills");
                row["skill_nombre"] = Obj.skill_nombre;
                row["skill_org_id"] = Token.OrganizacionID;
                row["skill_descripcion"] = Obj.skill_descripcion;
                row["skill_nombre_abreviado"] = Obj.skill_nombre_abreviado;

                if (Obj.skill_id.Equals(0))
                {
                    row["skill_activo"] = 1;
                    db.SQLInsert(row, "skill_id");
                }
                else
                {
                    db.SQLUpdate(row, "skill_id=@id", "skill_id", new List<System.Data.SqlClient.SqlParameter>()
                    {
                        new System.Data.SqlClient.SqlParameter("id",Obj.skill_id)
                    });
                }
                oM.Success = true;
                oM.Message = "La operación se realizó con éxito.";
            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.Message = ex.Message;
            }

            return oM;
        }

        public override Entidades.Skill MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static Entidades.Skill MapearStatic(DataRow dr)
        {
            Entidades.Skill obj = new Entidades.Skill();
            obj.skill_id = Resources.Validaciones.valNULLINT(dr["skill_id"]);
            obj.skill_nombre = Resources.Validaciones.valNULLString(dr["skill_nombre"]);
            obj.skill_descripcion = Resources.Validaciones.valNULLString(dr["skill_descripcion"]);
            obj.skill_nombre_abreviado = Resources.Validaciones.valNULLString(dr["skill_nombre_abreviado"]);
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(obj.skill_id.ToString());
            return obj;
        }


        public override Entidades.Skill Mapear(DataRow dr)
        {
            Entidades.Skill obj = MapearSimple(dr);
            return obj;
        }

        public override Entidades.Skill MapearCompleto(DataRow dr)
        {
            Entidades.Skill obj = Mapear(dr);
            return obj;
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM Skills ";
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
        
        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public override Entidades.Skill ObjetoNuevo()
        {
            return new Entidades.Skill();
        }

    }
}