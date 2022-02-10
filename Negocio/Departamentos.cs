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
    public class Departamentos : NegocioBase<Entidades.Departamento>
    {
        Negocio.Provincias negocioPRV;

        public Departamentos(Entidades.App.Token paramToken) : base("dto_id", "dto_activo", "Departamentos", "dto")
        {
            Token = paramToken;
            negocioPRV = new Provincias(Token);
        }

        public override Entidades.Departamento MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static Departamento MapearStatic(DataRow dr)
        {
            Entidades.Departamento obj = new Entidades.Departamento();
            obj.dto_id = Resources.Validaciones.valNULLINT(dr["dto_id"]);
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(obj.dto_id.ToString());
            obj.dto_nombre = Resources.Validaciones.valNULLString(dr["dto_nombre"]);
            obj.dto_codigo = Resources.Validaciones.valNULLString(dr["dto_codigo"]);
            obj.descripcion_combo = obj.dto_nombre;

            return obj;
        }

        public override Entidades.Departamento Mapear(DataRow dr)
        {
            Entidades.Departamento obj = MapearSimple(dr);
            obj.Provincia = negocioPRV.MapearSimple(dr);
            return obj;
        }

        public override Entidades.Departamento MapearCompleto(DataRow dr)
        {
            Entidades.Departamento obj = Mapear(dr);

            // Mapear las propiedades o listas de propiedades que complementan al objeto

            return obj;
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = " SELECT * FROM Departamentos LEFT JOIN Provincias ON prv_id = dto_prv_id ";
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

        public List<Entidades.Departamento> ListarPorCentro()
        {
            List<Entidades.Departamento> lst = new List<Entidades.Departamento>();
            //sQuery = QueryDefault("", "dto_cem_id=@cem_id or dto_cem_id=0", "");
            //DataTable dt = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            //{
            //    new System.Data.SqlClient.SqlParameter("cem_id", Token.CentroID)
            //});

            sQuery = QueryDefault("", "", "");
            DataTable dt = db.SQLSelect(sQuery);
            foreach (DataRow row in dt.Rows)
            {
                lst.Add(Mapear(row));
            }
            return lst;
        }

        public List<Entidades.Departamento> ListarPorNombre(string texto, string cantidadRegistros = "")
        {
            List<Entidades.Departamento> lst = new List<Entidades.Departamento>();

            string sWhere = " (dto_nombre like @searchText) ";

            sQuery = QueryDefault(cantidadRegistros, sWhere, "");

            DataTable dt = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>() {
                new System.Data.SqlClient.SqlParameter("searchText", "%" + texto + "%")
            });
            foreach (DataRow row in dt.Rows)
            {
                lst.Add(Mapear(row));
            }
            return lst;
        }

        public List<Entidades.Departamento> ListarPorProvincia(int prv_id)
        {
            List<Entidades.Departamento> lst = new List<Entidades.Departamento>();

            string sWhere = " dto_prv_id=@prv_id ";

            sQuery = QueryDefault("", sWhere, "");

            DataTable dt = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>() {
                new System.Data.SqlClient.SqlParameter("prv_id", prv_id)
            });

            foreach (DataRow row in dt.Rows)
            {
                lst.Add(Mapear(row));
            }
            return lst;
        }



        public List<Entidades.Departamento> ListarPorUbicaciones(int prv_id)
        {
            List<Entidades.App.ObjectParameter> lstFiltros = new List<Entidades.App.ObjectParameter>();
            List<Entidades.Departamento> lst = ListarConFiltros(lstFiltros);
            return lst;
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public override List<Entidades.Departamento> ListarParaTableAjax(DatatableJS datatableFilters)
        {
            List<Entidades.Departamento> lst = new List<Entidades.Departamento>();
            string sWhere = "(dto_nombre like @searchText) ";
            string sOrderBy = "";
            if (datatableFilters.MostrarTodos == false)
            {
                sWhere += " and dto_activo=1 ";
            }
            if (datatableFilters.sortColumnName.Trim().Length > 0)
            {
                sOrderBy += datatableFilters.sortColumnName + " " + datatableFilters.direccion;
            }
            sQuery = QueryDefault("", sWhere, sOrderBy);
            DataTable dt = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>() {
                new System.Data.SqlClient.SqlParameter("searchText", "%" + datatableFilters.SearchValue + "%")
            });
            foreach (DataRow row in dt.Rows)
            {
                lst.Add(Mapear(row));
            }
            return lst;
        }


        public string ObtieneCodigo(string dto_id)
        {
            string sQuery = "SELECT dto_codigo FROM Departamentos WHERE dto_id = @dto_id";
            DataTable dt_loc = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("dto_id", dto_id)
                });
            if (dt_loc.Rows.Count > 0)
            {
                DataRow row = dt_loc.Rows[0];
                string cp = row["dto_codigo"].ToString();
                return cp;
            }
            else
            {
                return "";
            }

        }

        public string ObtieneCodigoPostalPorNombre(string dto_nombre)
        {
            string sQuery = "SELECT dto_codigo FROM Departamentos WHERE dto_nombre = @dto_nombre";
            DataTable dt_loc = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("dto_nombre", dto_nombre)
                });
            if (dt_loc.Rows.Count > 0)
            {
                DataRow row = dt_loc.Rows[0];
                string cp = row["dto_codigo"].ToString();
                return cp;
            }
            else
            {
                return "";
            }

        }

        public override Entidades.Departamento ObjetoNuevo()
        {
            return new Entidades.Departamento();
        }


        public Entidades.Departamento Busca_Localidad(string cod)
        {
            Negocio.Departamentos Obj = new Negocio.Departamentos(Token);
            return Obj.ObtenerPorID(cod.ToString());
        }

        public override ObjectMessage Save(Departamento Obj)
        {
            throw new NotImplementedException();
        }
    }
}
