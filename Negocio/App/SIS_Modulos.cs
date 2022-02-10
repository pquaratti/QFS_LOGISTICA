using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades.App;

namespace Negocio.App
{
    public class SIS_Modulos : NegocioBase<Entidades.App.SIS_Modulo>
    {
        public enum ModulosDisponibles
        {
            Sistemas = 1,
            Gestion = 2,
            Telegramas = 3
        }
        
        public SIS_Modulos(Entidades.App.Token paramToken) : base("mod_id", "mod_activo", "SIS_Modulos", "mod") { Token = paramToken; }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            List<Entidades.App.DLLObject> lst = new List<DLLObject>();
            List<Entidades.App.SIS_Modulo> lstModulos = ListarActivos();

            if (agregaDefault)
            {
                lst.Add(new DLLObject()
                {
                    Value = "",
                    Text = "Seleccione"
                });
            }

            foreach (Entidades.App.SIS_Modulo item in lstModulos)
            {
                Entidades.App.DLLObject obj = new DLLObject();
                obj.Value = item.IdEncriptado;
                obj.Text = item.mod_nombre;
                lst.Add(obj);
            }

            return lst;
        }

        public override SIS_Modulo MapearSimple(DataRow dr)
        {
            Entidades.App.SIS_Modulo oMod = new SIS_Modulo();
            oMod.mod_id = Resources.Validaciones.valNULLINT(dr["mod_id"]);
            oMod.IdEncriptado = Negocio.App.Security.EncriptarID(oMod.mod_id.ToString());
            oMod.mod_nombre = Resources.Validaciones.valNULLString(dr["mod_nombre"]);
            oMod.mod_descripcion = Resources.Validaciones.valNULLString(dr["mod_descripcion"]);
            oMod.mod_activo = Resources.Validaciones.valNULLBool(dr["mod_activo"]);
            return oMod;
        }

        public override SIS_Modulo Mapear(DataRow dr)
        {
            Entidades.App.SIS_Modulo oMod = MapearSimple(dr);
            return oMod;
        }

        public override SIS_Modulo MapearCompleto(DataRow dr)
        {
            Entidades.App.SIS_Modulo oMod = Mapear(dr);

            return oMod;
        }

        public override Entidades.App.ObjectMessage Save(SIS_Modulo Obj)
        {
            Entidades.App.ObjectMessage oM = new ObjectMessage();

            try
            {
                DataRow row = db.Estructura(nombreTablaPrincipal);
                row["mod_nombre"] = Obj.mod_nombre;
                row["mod_descripcion"] = Obj.mod_descripcion;
                row["mod_activo"] = Obj.mod_activo;

                if (Obj.mod_id.Equals(0))
                {
                    Obj.mod_id = MaximoID();
                    row["mod_id"] = Obj.mod_id;
                    db.SQLInsert(row);
                    Obj.IdEncriptado = Negocio.App.Security.EncriptarID(Obj.mod_id.ToString());
                }
                else
                {
                    db.SQLUpdate(row, "mod_id=@id", "mod_id", new List<System.Data.SqlClient.SqlParameter>()
                    {
                        new System.Data.SqlClient.SqlParameter("id",Obj.mod_id)

                    });
                }

                return oM;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en negocio :" + ex.Message);
            }
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "select * from SIS_Modulos ";

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

        public override int MaximoID()
        {
            sQuery = "select isnull(MAX(mod_id),0) + 1 as proximo from SIS_Modulos";
            DataTable dt = db.SQLSelect(sQuery);
            return Convert.ToInt32(dt.Rows[0]["proximo"]);
        }

        public override List<SIS_Modulo> ListarParaTableAjax(DatatableJS datatableFilters)
        {
            List<Entidades.App.SIS_Modulo> lst = new List<SIS_Modulo>();

            string sWhere = "(mod_nombre like @searchText or mod_descripcion like @searchText) ";
            string sOrden = "";

            if (datatableFilters.MostrarTodos == false)
                sWhere += " and mod_activo=1 ";

            if (datatableFilters.sortColumnName.Length > 0)
                sOrden = datatableFilters.sortColumnName + " " + datatableFilters.direccion;
            
            sQuery = QueryDefault("", sWhere, sOrden);

            DataTable dt = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>() {
                new System.Data.SqlClient.SqlParameter("searchText", "%" + datatableFilters.SearchValue + "%")
            });

            foreach (DataRow row in dt.Rows)
            {
                lst.Add(Mapear(row));
            }

            return lst;
        }

        public override SIS_Modulo ObjetoNuevo()
        {
            return new SIS_Modulo();
        }
    }
}
