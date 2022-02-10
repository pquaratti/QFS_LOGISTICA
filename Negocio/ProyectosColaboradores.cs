using Entidades;
using Entidades.App;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class ProyectosColaboradores : NegocioBase<Entidades.Proyecto_Colaborador>
    {
        public ProyectosColaboradores(Entidades.App.Token paramToken) : base("prycolab_id", "prycolab_activo", "Proyectos_Colaboradores", "prycolab")
        {
            Token = paramToken;
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public override Proyecto_Colaborador MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static Proyecto_Colaborador MapearStatic(DataRow dr)
        {
            Entidades.Proyecto_Colaborador obj = new Entidades.Proyecto_Colaborador();
            obj.prycolab_id = Resources.Validaciones.valNULLINT(dr["prycolab_id"]);
            obj.Legajo = new Entidades.App.SIS_Usuario() { usu_id = Resources.Validaciones.valNULLINT(dr["prycolab_usu_id"]) };
            obj.Proyecto = new Entidades.Proyecto() { proy_id = Resources.Validaciones.valNULLINT(dr["prycolab_proy_id"]) };

            return obj;
        }

        public override Proyecto_Colaborador Mapear(DataRow dr)
        {
            Entidades.Proyecto_Colaborador obj = MapearSimple(dr);
            obj.Legajo = Negocio.App.SIS_Usuarios.MapearStatic(dr);
            return obj;
        }

        public override void PermiteGuardar(Proyecto_Colaborador obj)
        {
            if (ColaboradorAsignado(obj.Legajo.usu_id, obj.Proyecto.proy_id))
                throw new Exception("La persona ya está asignada como colaborador del proyecto.");
        }

        public bool ColaboradorAsignado(int usu_id, int proy_id)
        {
            string sQuery = QueryDefault("", " prycolab_usu_id = @usu_id and prycolab_proy_id = @proy_id ", "");
            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("usu_id",usu_id),
                    new System.Data.SqlClient.SqlParameter("proy_id",proy_id)
                });
            return (dt_bus.Rows.Count > 0);
        }

        public override ObjectMessage Save(Proyecto_Colaborador Obj)
        {
            ObjectMessage oM = new ObjectMessage();


            try
            {
                PermiteGuardar(Obj);
                DataRow row = db.Estructura("Proyectos_Colaboradores");
                row["prycolab_usu_id"] = Obj.Legajo.usu_id;
                row["prycolab_proy_id"] = Obj.Proyecto.proy_id;

                if (Obj.prycolab_id.Equals(0))
                {
                    Obj.prycolab_id = db.SQLInsert(row, "prycolab_id").Valor;
                    oM.Message = "Datos ingresados";
                }
                else
                {
                    db.SQLUpdate(row, "prycolab_id=@prycolab_id", "prycolab_id", new List<System.Data.SqlClient.SqlParameter>() {
                        new System.Data.SqlClient.SqlParameter("prycolab_id",Obj.prycolab_id)
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
            sQuery = "  SELECT * FROM Proyectos_Colaboradores " +
                "LEFT JOIN Proyectos ON proy_id = prycolab_proy_id " +
                "LEFT JOIN SIS_Usuarios ON usu_id = prycolab_usu_id ";

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

        public List<Entidades.Proyecto_Colaborador> ListarColaboradores()
        {
            List<Entidades.Proyecto_Colaborador> lst = new List<Entidades.Proyecto_Colaborador>();

            sQuery = QueryDefault("", "", "");

            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>());

            foreach (DataRow row in dt_bus.Rows)
            {
                lst.Add(Mapear(row));
            }
            return lst;
        }

        public List<Entidades.Proyecto_Colaborador> ListarColaboradoresProyecto(string proy_id)
        {

            List<Entidades.Proyecto_Colaborador> lst = new List<Entidades.Proyecto_Colaborador>();

            sQuery = QueryDefault("", " proy_id = @proy_id ", "");

            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("proy_id",Negocio.App.Security.DesencriptarID(proy_id))
            });

            foreach (DataRow row in dt_bus.Rows)
            {
                lst.Add(Mapear(row));
            }
            return lst;
        }

        public List<Entidades.Proyecto_Colaborador> ListarProyectosColaborador(int usu_id)
        {
            List<Entidades.Proyecto_Colaborador> lst = new List<Entidades.Proyecto_Colaborador>();

            sQuery = QueryDefault("", " usu_id = @usu_id ", "");

            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("usu_id",usu_id)
            });

            foreach (DataRow row in dt_bus.Rows)
            {
                lst.Add(Mapear(row));
            }
            return lst;
        }

        public override Proyecto_Colaborador MapearCompleto(DataRow dr)
        {
            throw new NotImplementedException();
        }

        public override Proyecto_Colaborador ObjetoNuevo()
        {
            throw new NotImplementedException();
        }

    }
}
