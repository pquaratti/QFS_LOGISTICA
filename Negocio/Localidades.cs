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
    public class Localidades : NegocioBase<Entidades.Localidades>
    {
        Negocio.Departamentos negocioDTO;

        public Localidades(Entidades.App.Token paramToken) : base("loc_id", "loc_activo", "Localidades", "loc")
        {
            Token = paramToken;
            negocioDTO = new Departamentos(Token);
        }

        public override Entidades.App.ObjectMessage Save(Entidades.Localidades Obj)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();

            try
            {
                DataRow row = db.Estructura("Localidades");
                row["loc_nombre"] = Obj.loc_nombre;
                row["loc_dto_id"] = Obj.Departamento.dto_id;
                row["loc_codigo"] = Obj.loc_codigo;
                if (Obj.loc_id.Equals(0))
                {
                    row["loc_usu_id_alta"] = Obj.usu_id_alta;
                    row["loc_usu_fec_alta"] = DateTime.Now;
                    db.SQLInsert(row, "loc_id");
                }
                else
                {
                    row["loc_usu_id_mod"] = Obj.usu_id_mod;
                    row["loc_usu_fec_mod"] = DateTime.Now;
                    db.SQLUpdate(row, "loc_id=@id", "loc_id", new List<System.Data.SqlClient.SqlParameter>()
                        {
                        new System.Data.SqlClient.SqlParameter("id",Obj.loc_id)
                        });
                }
                return oM;
            }
            catch (Exception)
            {
                return oM;
            }
        }

        public string GuardaLocalidad(Entidades.Localidades Obj)
        {
            if (Obj.Departamento.dto_id == 0) { return "Localidad"; };
            //if (Obj.Departamentos.dto_id == 1 && Obj.Partidos.par_id == 0) {return "Partido"; }
            //bool duplicado = Negocio.DDL.NombreyClaveRepetidos("Localidades", "loc_id", Obj.loc_id.ToString(), "loc_nombre", Obj.loc_nombre);
            //bool duplicado = Negocio.DDL.NombreyClaveRepetidos_2_Niveles("Localidades", "loc_nombre", Obj.loc_nombre, "loc_id", Obj.loc_id.ToString(), "loc_dto_id", Obj.Departamentos.dto_id.ToString());


            //if (duplicado)
            //{
            //    return "Localidad Existente";
            //}

            DataRow row = db.Estructura("Localidades");
            row["loc_nombre"] = Obj.loc_nombre;
            //row["loc_par_id"] = Obj.Partidos.par_id;
            row["loc_dto_id"] = Obj.Departamento.dto_id;
            row["loc_codigo"] = Obj.loc_codigo;
            if (Obj.loc_id.Equals(0))
            {
                db.SQLInsert(row, "loc_id");
            }
            else
            {
                db.SQLUpdate(row, "loc_id=@id", "loc_id", new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("id",Obj.loc_id)
                });
            }
            return "";
        }

        public override Entidades.Localidades MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static Entidades.Localidades MapearStatic(DataRow dr)
        {
            Entidades.Localidades obj = new Entidades.Localidades();
            obj.loc_id = Resources.Validaciones.valNULLINT(dr["loc_id"]);
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(obj.loc_id.ToString());
            obj.loc_nombre = Resources.Validaciones.valNULLString(dr["loc_nombre"]);
            obj.loc_codigo = Resources.Validaciones.valNULLString(dr["loc_codigo"]);
            obj.loc_activo = Resources.Validaciones.valNULLBool(dr["loc_activo"]);
            return obj;
        }

        public override Entidades.Localidades Mapear(DataRow dr)
        {
            Entidades.Localidades obj = MapearSimple(dr);
            obj.Departamento = negocioDTO.MapearSimple(dr);
            return obj;
        }

        public override Entidades.Localidades MapearCompleto(DataRow dr)
        {
            Entidades.Localidades obj = Mapear(dr);

            // Mapear las propiedades o listas de propiedades que complementan al objeto

            return obj;
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = " SELECT * From Localidades LEFT JOIN Departamentos on dto_id = loc_dto_id ";
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

        public List<Entidades.Localidades> ListarPorCentro()
        {
            List<Entidades.Localidades> lst = new List<Entidades.Localidades>();
            //sQuery = QueryDefault("", "loc_cem_id=@cem_id or loc_cem_id=0", "");
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

        public List<Entidades.Localidades> ListarPorNombre(string texto, string cantidadRegistros = "")
        {
            List<Entidades.Localidades> lst = new List<Entidades.Localidades>();

            string sWhere = " (loc_nombre like @searchText) ";

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



        public List<Entidades.Localidades> ListarPorUbicaciones(string pais, int provin, int localidad)
        {
            List<Entidades.App.ObjectParameter> lstFiltros = new List<Entidades.App.ObjectParameter>();
            List<Entidades.Localidades> lst = ListarConFiltros(lstFiltros);
            return lst;
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public override List<Entidades.Localidades> ListarParaTableAjax(DatatableJS datatableFilters)
        {
            List<Entidades.Localidades> lst = new List<Entidades.Localidades>();
            string sWhere = "(loc_nombre like @searchText) ";
            string sOrderBy = "";
            if (datatableFilters.MostrarTodos == false)
            {
                sWhere += " and loc_activo=1 ";
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


        public string ObtieneCodigoPostal(string loc_id)
        {
            string sQuery = "SELECT loc_cp FROM Localidades WHERE loc_id = @loc_id";
            DataTable dt_loc = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("loc_id", loc_id)
                });
            if (dt_loc.Rows.Count > 0)
            {
                DataRow row = dt_loc.Rows[0];
                string cp = row["loc_cp"].ToString();
                return cp;
            }
            else
            {
                return "";
            }

        }

        public string ObtieneCodigoPostalPorNombre(string loc_nombre)
        {
            string sQuery = "SELECT loc_cp FROM Localidades WHERE loc_nombre = @loc_nombre";
            DataTable dt_loc = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("loc_nombre", loc_nombre)
                });
            if (dt_loc.Rows.Count > 0)
            {
                DataRow row = dt_loc.Rows[0];
                string cp = row["loc_cp"].ToString();
                return cp;
            }
            else
            {
                return "";
            }

        }

        public override Entidades.Localidades ObjetoNuevo()
        {
            return new Entidades.Localidades();
        }


        public Entidades.Localidades Busca_Localidad(string cp)
        {
            Negocio.Localidades Obj = new Negocio.Localidades(Token);
            return Obj.ObtenerPorID(cp.ToString());
        }

    }
}
