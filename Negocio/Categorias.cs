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
    public class Categorias : NegocioBase<Entidades.Categoria>
    {
        public Categorias(Entidades.App.Token paramToken) : base("cat_id", "sin", "Categorias", "cat") 
        {
            Token = paramToken;
        }

        public override Entidades.App.ObjectMessage Save(Entidades.Categoria Obj)
        {
            Entidades.App.ObjectMessage oM = new ObjectMessage();

            try
            {
                DataRow row = db.Estructura(this.nombreTablaPrincipal);
                row["cat_nombre"] = Obj.cat_nombre;
                row["cat_descripcion"] = Obj.cat_descripcion;
                row["cat_activo"] = Obj.cat_activo;

                if (Obj.cat_id.Equals(0))
                {
                    row["cat_org_id"] = Token.OrganizacionID;
                    db.SQLInsert(row, "cat_id");
                }
                else
                {
                    db.SQLUpdate(row, "cat_id=@id", "cat_id", new List<System.Data.SqlClient.SqlParameter>()
                    {
                        new System.Data.SqlClient.SqlParameter("id",Obj.cat_id)
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

        public override Entidades.Categoria MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static Entidades.Categoria MapearStatic(DataRow dr)
        {
            Entidades.Categoria obj = new Entidades.Categoria();
            obj.cat_id = Resources.Validaciones.valNULLINT(dr["cat_id"]);
            obj.cat_nombre = Resources.Validaciones.valNULLString(dr["cat_nombre"]);
            obj.cat_descripcion = Resources.Validaciones.valNULLString(dr["cat_descripcion"]);
            obj.cat_activo = Resources.Validaciones.valNULLBool(dr["cat_activo"]);
            obj.descripcion_combo = Resources.Validaciones.valNULLString(dr["cat_descripcion"]);
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(obj.cat_id.ToString());
            return obj;
        }


        public override Entidades.Categoria Mapear(DataRow dr)
        {
            Entidades.Categoria obj = MapearSimple(dr);
            return obj;
        }

        public override Entidades.Categoria MapearCompleto(DataRow dr)
        {
            Entidades.Categoria obj = Mapear(dr);
            return obj;
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM Categorias ";

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

        public override Entidades.Categoria ObjetoNuevo()
        {
            return new Entidades.Categoria();
        }

    }
}