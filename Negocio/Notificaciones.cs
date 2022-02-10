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
    public class Notificaciones : NegocioBase<Entidades.Notificacion>
    {
     
        public Notificaciones(Entidades.App.Token paramToken) : base("not_id", "not_activo", "Notificaciones", "not")
        {
            Token = paramToken;
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public override Entidades.Notificacion Mapear(DataRow dr)
        {
            Entidades.Notificacion obj = MapearSimple(dr);
            return obj;
        }

        public override Entidades.Notificacion MapearCompleto(DataRow dr)
        {
            Entidades.Notificacion obj = Mapear(dr);
            return obj;
        }

        public override Entidades.Notificacion MapearSimple(DataRow dr)
        {
            Entidades.Notificacion obj = new Entidades.Notificacion();
            obj.not_id = Resources.Validaciones.valNULLINT(dr["not_id"]);
            obj.not_texto = Resources.Validaciones.valNULLString(dr["not_texto"]);
            obj.not_visto = Resources.Validaciones.valNULLBool(dr["not_visto"]);
            return obj;
        }

        public override Entidades.Notificacion ObjetoNuevo()
        {
            Entidades.Notificacion obj = new Entidades.Notificacion();
            return obj;
        }

        public override void DatosObligatorios(Entidades.Notificacion obj)
        {
            
        }

        public override ObjectMessage Save(Entidades.Notificacion Obj)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                DatosObligatorios(Obj);
                DataRow row = db.Estructura("Notificaciones");
                row["not_texto"] = Obj.not_texto;
                
                if (Obj.not_id.Equals(0))
                {
                    row["not_usu_id_alta"] = Convert.ToInt32(Token.UserID);
                    row["not_fec_alta"] = DateTime.Now;
                    Obj.not_id = db.SQLInsert(row, "not_id").Valor;
                    oM.Message = "Datos ingresados";
                }
                else
                {
                    row["not_usu_id_mod"] = Convert.ToInt32(Token.UserID);
                    row["not_fec_mod"] = DateTime.Now;
                    db.SQLUpdate(row, "not_id=@not_id", "not_id", new List<System.Data.SqlClient.SqlParameter>() {
                        new System.Data.SqlClient.SqlParameter("not_id",Obj.not_id)
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
            sQuery = "  Select * from Notificaciones ";
            sQuery += " LEFT JOIN Distritos on dis_id=not_dis_id ";
            
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

        #region Funcionalidad particular

        public List<Entidades.Notificacion> ListarPorDistrito(int dis_id = 0)
        {
            List<Entidades.Notificacion> lst = new List<Entidades.Notificacion>();
            if (dis_id == 0)
            {
                lst = ListarConFiltros(new List<ObjectParameter>() { });
            }
            else
            {
                lst = ListarConFiltros(new List<ObjectParameter>() { new ObjectParameter(){ Name= "not_dis_id", Value = dis_id } });
            }
            return lst;
        }

        #endregion

    }
}
