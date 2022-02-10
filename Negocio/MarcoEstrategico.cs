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
    public class MarcoEstrategico : NegocioBase<Entidades.Marco_Estrategico>
    {
        public MarcoEstrategico(Entidades.App.Token paramToken) : base("marc_id", "marc_activo", "Marco_Estrategico", "marc") 
        {
            Token = paramToken;
        }

        public override Entidades.App.ObjectMessage Save(Entidades.Marco_Estrategico Obj)
        {
            Entidades.App.ObjectMessage oM = new ObjectMessage();

            try
            {
                DataRow row = db.Estructura("Marco_Estrategico");
                row["marc_per_ini"] = Obj.marc_per_ini;
                row["marc_per_fin"] = Obj.marc_per_fin;
                row["marc_nombre"] = Obj.marc_nombre;
                row["marc_descripcion"] = Obj.marc_descripcion;

                if (Obj.marc_id.Equals(0))
                {
                    row["marc_org_id"] = Token.OrganizacionID;
                    row["marc_activo"] = 1;
                    db.SQLInsert(row, "marc_id");
                }
                else
                {
                    db.SQLUpdate(row, "marc_id=@id", "marc_id", new List<System.Data.SqlClient.SqlParameter>()
                    {
                        new System.Data.SqlClient.SqlParameter("id",Obj.marc_id)
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

        public override Entidades.Marco_Estrategico MapearSimple(DataRow dr)
        {
            Entidades.Marco_Estrategico obj = new Entidades.Marco_Estrategico();
            obj.marc_id = Resources.Validaciones.valNULLINT(dr["marc_id"]);
            obj.marc_nombre = Resources.Validaciones.valNULLString(dr["marc_nombre"]);
            obj.marc_descripcion = Resources.Validaciones.valNULLString(dr["marc_descripcion"]);
            obj.marc_per_ini = Resources.Validaciones.valNULLINT(dr["marc_per_ini"]);
            obj.marc_per_fin = Resources.Validaciones.valNULLINT(dr["marc_per_fin"]);
            obj.Organizacion = new SIS_Organizacion() { org_id = Resources.Validaciones.valNULLINT(dr["marc_org_id"]) };
            return obj;
        }

        public override Entidades.Marco_Estrategico Mapear(DataRow dr)
        {
            Entidades.Marco_Estrategico obj = MapearSimple(dr);
            return obj;
        }

        public override Entidades.Marco_Estrategico MapearCompleto(DataRow dr)
        {
            Entidades.Marco_Estrategico obj = Mapear(dr);
            return obj;
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM Marco_Estrategico ";
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

        public override Entidades.Marco_Estrategico ObjetoNuevo()
        {
            return new Entidades.Marco_Estrategico();
        }

        public List<Entidades.Marco_Estrategico> ListarPorOrganizacion()
        {
            List<Entidades.Marco_Estrategico> lst = new List<Marco_Estrategico>();

            lst = ListarConFiltros(new List<ObjectParameter>() {
                    new ObjectParameter("marc_org_id", Token.OrganizacionID) 
                  });
            
            return lst;
        }
    }
}