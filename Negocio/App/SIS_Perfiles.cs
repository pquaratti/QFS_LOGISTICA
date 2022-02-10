using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades.App;

namespace Negocio.App
{
    public class SIS_Perfiles : NegocioBase<Entidades.App.SIS_Perfil>
    {
        public SIS_Perfiles(Entidades.App.Token unToken) : base("prf_id") { Token = unToken; }

        public override SIS_Perfil MapearSimple(DataRow dr)
        {
            Entidades.App.SIS_Perfil obj = new SIS_Perfil();
            obj.prf_id = Resources.Validaciones.valNULLINT(dr["prf_id"]);
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(obj.prf_id.ToString());
            obj.prf_nombre = Resources.Validaciones.valNULLString(dr["prf_nombre"]);
            obj.prf_activo = Resources.Validaciones.valNULLBool(dr["prf_activo"]);
            obj.prf_mod_id = Resources.Validaciones.valNULLINT(dr["prf_mod_id"]);
            return obj;
        }

        public override SIS_Perfil Mapear(DataRow dr)
        {
            Entidades.App.SIS_Perfil obj = MapearSimple(dr);
            return obj;
        }

        public override SIS_Perfil MapearCompleto(DataRow dr)
        {
            Entidades.App.SIS_Perfil obj = Mapear(dr);

            // Propiedades específicas

            return obj;
        }

        public override Entidades.App.ObjectMessage Save(SIS_Perfil Obj)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();

            try
            {
                DataRow row = db.Estructura("SIS_Perfiles");
                row["prf_id"] = Obj.prf_id;
                row["prf_nombre"] = Obj.prf_nombre.Trim();

                if (Obj.prf_id.Equals(0))
                {
                    row["prf_mod_id"] = Obj.prf_mod_id;
                    Obj.prf_id = db.SQLInsert(row, "prf_id").Valor;

                    oM.Message = "Datos Ingresados exitosamente!";
                }
                else
                {
                    db.SQLUpdate(row, "prf_id=@id", "prf_id", new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("id",Obj.prf_id)
                });

                    oM.Message = "Datos Ingresados actualizados!";
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

        public List<Entidades.App.SIS_Perfil> ListarSimple()
        {
            List<Entidades.App.SIS_Perfil> lst = new List<Entidades.App.SIS_Perfil>();

            sQuery = QueryDefault("", "", "");

            DataTable dt = db.SQLSelect(sQuery);

            foreach (DataRow row in dt.Rows)
            {
                lst.Add(Mapear(row));
            }
            return lst;
        }


        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "SELECT * FROM SIS_PERFILES ";
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

     

        public List<Entidades.App.SIS_Accion> ListarAcciones(int prf_id)
        {
            List<Entidades.App.SIS_Accion> lst = new List<Entidades.App.SIS_Accion>();
            Negocio.App.SIS_Acciones negocioACC = new Negocio.App.SIS_Acciones(Token);

            sQuery = "  select * from SIS_Acciones ";
            sQuery += " left join SIS_Perfiles_Acciones on pac_acc_id=acc_id and pac_prf_id=@prf_id ";

            DataTable dt = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>() {
                new System.Data.SqlClient.SqlParameter("prf_id",prf_id)
            });

            foreach (DataRow rowSistema in dt.Rows)
            {
                Entidades.App.SIS_Accion objAccion = negocioACC.Mapear(rowSistema);

                if (rowSistema["pac_id"] != DBNull.Value)
                    objAccion.VinculadaAPerfil = true;
                else
                    objAccion.VinculadaAPerfil = false;

                lst.Add(objAccion);
            }

            negocioACC = null;

            return lst;
        }

        public ObjectMessage AsignarAccion(int perfilID, int accionID)
        {
            ObjectMessage oM = new ObjectMessage();


            sQuery = "INSERT INTO SIS_Perfiles_Acciones (pac_acc_id,pac_prf_id) ";
            sQuery += "VALUES (@acc_id,@prf_id) ";

            db.SQLExecuteNonQuery(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("acc_id", accionID),
                new System.Data.SqlClient.SqlParameter("prf_id", perfilID)
            });

            oM.Success = true;
            oM.Message = "Acción vinculada al perfil!";

            return oM;
        }

        public ObjectMessage QuitarAccion(string perfilID, string accionID)
        {
            ObjectMessage oM = new ObjectMessage();

            db.SQLExecuteNonQuery("DELETE FROM SIS_Perfiles_Acciones WHERE pac_prf_id=@prf_id and pac_acc_id=@acc_id", new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("prf_id",perfilID),
                new System.Data.SqlClient.SqlParameter("acc_id",accionID)
            });

            oM.Success = true;
            oM.Message = "Acción desvinculada del perfil!";

            return oM;
        }
        
        public DataTable DT_Usuarios(string prf_id)
        {
            sQuery = "  select usu_nickname,usu_fullname,usu_email,prf_nombre from  SIS_usuarios_sistemas ";
            sQuery += " inner join SIS_Usuarios on usu_id=usi_usu_id ";
            sQuery += " inner join SIS_Perfiles on prf_id=usi_prf_id ";
            sQuery += " where usi_prf_id=@prf_id ";

            DataTable dt = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>() {
                new System.Data.SqlClient.SqlParameter("prf_id",prf_id)
            });

            return dt;
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public List<DLLObject> ListarDLL(bool agregaDefault = false, int moduloID = 0)
        {
            List<Entidades.App.DLLObject> lst = new List<DLLObject>();

            List<Entidades.App.SIS_Perfil> lstPerfiles = ListarPorModuloID(moduloID);

            foreach (Entidades.App.SIS_Perfil item in lstPerfiles)
            {
                Entidades.App.DLLObject obj = new DLLObject();
                obj.Value = item.IdEncriptado;
                obj.Text = item.prf_nombre;
                lst.Add(obj);
            }
            
            return lst;
        }

        public List<Entidades.App.SIS_Perfil> ListarPorModuloID(string idEncriptado)
        {
            int id = Convert.ToInt32(Negocio.App.Security.DesencriptarID(idEncriptado));
            return ListarPorModuloID(id);
        }

        public List<Entidades.App.SIS_Perfil> ListarPorModuloID(int id)
        {
            List<Entidades.App.SIS_Perfil> lst = new List<SIS_Perfil>();

            sQuery = QueryDefault("", "prf_mod_id=@id", "");

            DataTable dt = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("id", id)
            });

            foreach (DataRow row in dt.Rows)
            {
                lst.Add(Mapear(row));
            }
            
            return lst;
        }

        public override SIS_Perfil ObjetoNuevo()
        {
            return new SIS_Perfil();
        }
    }
}
