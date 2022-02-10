using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades.App;

namespace Negocio.App
{
    public class SIS_Acciones : NegocioBase<Entidades.App.SIS_Accion>
    {
        public SIS_Acciones(Entidades.App.Token paramToken) : base("acc_id", "acc_activo", "SIS_Acciones", "acc" ){ Token = paramToken; }



        public override SIS_Accion MapearSimple(DataRow dr)
        {
            Entidades.App.SIS_Accion obj = new SIS_Accion();
            obj.acc_id = Resources.Validaciones.valNULLINT(dr["acc_id"]);
            obj.acc_nombre = Resources.Validaciones.valNULLString(dr["acc_nombre"]);
            obj.acc_descripcion = Resources.Validaciones.valNULLString(dr["acc_descripcion"]);
            obj.acc_controller = Resources.Validaciones.valNULLString(dr["acc_controller"]);
            obj.acc_accion = Resources.Validaciones.valNULLString(dr["acc_accion"]);
            obj.acc_icono = Resources.Validaciones.valNULLString(dr["acc_icono"]);
            obj.acc_orden = Resources.Validaciones.valNULLINT(dr["acc_orden"]);
            obj.acc_menu = Resources.Validaciones.valNULLBool(dr["acc_menu"]);
            obj.acc_id_padre = Resources.Validaciones.valNULLINT(dr["acc_id_padre"]);
            if (obj.acc_id_padre > 0)
            {
                obj.AccionPadre = ObtenerPorID(obj.acc_id_padre.ToString(),false);
            }
            else
            {
                obj.AccionPadre = new SIS_Accion();
            }
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(obj.acc_id.ToString());
            obj.estado = Resources.Validaciones.valNULLBool(dr["acc_activo"]);
            return obj;
        }

        public override SIS_Accion Mapear(DataRow dr)
        {
            Entidades.App.SIS_Accion obj = MapearSimple(dr);
            return obj;
        }

        public override SIS_Accion MapearCompleto(DataRow dr)
        {
            Entidades.App.SIS_Accion obj = Mapear(dr);

            // Propiedades específicas

            return obj;
        }

        public override Entidades.App.ObjectMessage Save(SIS_Accion Obj)
        {
            Entidades.App.ObjectMessage oM = new ObjectMessage();

            DataRow row = db.Estructura("SIS_ACCIONES");
            row["acc_nombre"] = Obj.acc_nombre;
            row["acc_descripcion"] = Obj.acc_descripcion;
            row["acc_controller"] = Obj.acc_controller;
            row["acc_accion"] = Obj.acc_accion;
            row["acc_id_padre"] = Obj.acc_id_padre;
            row["acc_icono"] = Obj.acc_icono;
            row["acc_orden"] = Obj.acc_orden;
            row["acc_menu"] = Obj.acc_menu;
     
            if (Obj.acc_id.Equals(0))
            {
                row["acc_activo"] = true;
                db.SQLInsert(row, "acc_id");
                oM.Message = "Datos Ingresados exitosamente";
            }
            else
            {
                db.SQLUpdate(row, "acc_id=@id", "acc_id", new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("id",Obj.acc_id)
                });

                oM.Message = "Datos modificados exitosamente!";
            }

            oM.Success = true;

            return oM;
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM SIS_ACCIONES ";
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



        public Entidades.App.ObjectMessage DeleteLogico(int id)
        {
            Entidades.App.ObjectMessage obj = new Entidades.App.ObjectMessage();
            try
            {

                string sQuery = "Update SIS_Acciones set acc_fec_baja = @fec where acc_id = @acc";
                db.SQLExecuteNonQuery(sQuery, new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("fec", DateTime.Now), new System.Data.SqlClient.SqlParameter("acc", id)
                });
            }
            catch (Exception Ex)
            {
                obj.Success = false;
                obj.Message = Ex.Message;
            }
            return obj;
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public override List<SIS_Accion> ListarParaTableAjax(DatatableJS datatableFilters)
        {
            List<Entidades.App.SIS_Accion> lst = new List<SIS_Accion>();

            string sWhere = "(acc_nombre like @searchText or acc_descripcion like @searchText) ";

            if (datatableFilters.MostrarTodos == false)
                sWhere += " and acc_activo=1 ";
            
            string sOrden = "";
            
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

            lst = lst.OrderByDescending(o => o.acc_menu).ThenBy(o => o.acc_id_padre).ToList();

            return lst;
        }

        public override SIS_Accion ObjetoNuevo()
        {
            return new SIS_Accion();
        }
    }
}
