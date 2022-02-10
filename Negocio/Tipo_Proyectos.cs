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
    public class Tipo_Proyectos : NegocioBase<Entidades.Tipo_Proyecto>
    {
        public Tipo_Proyectos(Entidades.App.Token paramToken) : base("tproy_id", "sin", "Tipo_Proyecto", "tproy")
        {
            Token = paramToken;
            TokenFilter = true;
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public override Tipo_Proyecto Mapear(DataRow dr)
        {
            Entidades.Tipo_Proyecto obj = MapearSimple(dr);
                               
            return obj;
        }

        public override Tipo_Proyecto MapearCompleto(DataRow dr)
        {
            Entidades.Tipo_Proyecto obj = Mapear(dr);
            return obj;
        }

        public override Tipo_Proyecto MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static Tipo_Proyecto MapearStatic(DataRow dr)
        {
            Entidades.Tipo_Proyecto obj = new Entidades.Tipo_Proyecto();
            obj.tproy_id = Resources.Validaciones.valNULLINT(dr["tproy_id"]);
            obj.tproy_nombre = Resources.Validaciones.valNULLString(dr["tproy_nombre"]);

            return obj;
        }

        public override Tipo_Proyecto ObjetoNuevo()
        {
            Entidades.Tipo_Proyecto obj = new Entidades.Tipo_Proyecto();
            return obj;
        }

        public override ObjectMessage Save(Tipo_Proyecto Obj)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                PermiteGuardar(Obj);
                DataRow row = db.Estructura("Tipo_Proyecto");
                row["tproy_id"] = Obj.tproy_id;
                row["tproy_nombre"] = Obj.tproy_nombre;

                if (Obj.tproy_id.Equals(0))
                {
                    row["tproy_org_id"] = Token.OrganizacionID;
                    row["tproy_usu_id_alta"] = Token.UserID;
                    row["tproy_fec_alta"] = DateTime.Now;
                    Obj.tproy_id = db.SQLInsert(row, "tproy_id").Valor;
                    oM.Message = "Datos ingresados";
                }
                else
                {
                    row["tproy_usu_id_mod"] = Token.UserID;
                    row["tproy_fec_mod"] = DateTime.Now;

                    db.SQLUpdate(row, "tproy_id=@tproy_id", "tproy_id", new List<System.Data.SqlClient.SqlParameter>() {
                        new System.Data.SqlClient.SqlParameter("tproy_id",Obj.tproy_id)
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
            sQuery = "  SELECT * FROM Tipo_Proyecto ";
            sQuery += " left join SIS_Organizaciones on org_id=tproy_org_id ";
          
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

        public List<Entidades.Tipo_Proyecto> Listar()
        {
            List<Entidades.Tipo_Proyecto> lst = new List<Entidades.Tipo_Proyecto>();

            lst = Listar("", "", "");

            return lst;
        }

    }
}
