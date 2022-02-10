using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades.App;

namespace Negocio.App
{
    public class SIS_Usuarios_Login : NegocioBase<Entidades.App.SIS_Usuario_Login>
    {
       
        public SIS_Usuarios_Login(Entidades.App.Token paramToken) : base("usl_id", "usl_activo", "SIS_Usuarios_Login", "usl" ){ Token = paramToken; }

        public override SIS_Usuario_Login MapearSimple(DataRow dr)
        {
            Entidades.App.SIS_Usuario_Login obj = new SIS_Usuario_Login();
            obj.usl_id = Resources.Validaciones.valNULLINT(dr["usl_id"]);
            obj.usl_fec_ini = Resources.Validaciones.valNULLDateTime(dr["usl_fec_ini"]);
            obj.usl_fec_fin = Resources.Validaciones.valNULLDateTime(dr["usl_fec_fin"]);
            obj.usl_ipv4 = Resources.Validaciones.valNULLString(dr["usl_ipv4"]);
            obj.usl_navigator = Resources.Validaciones.valNULLString(dr["usl_navigator"]);
            obj.Usuario = new Entidades.App.SIS_Usuario() { usu_id = Resources.Validaciones.valNULLINT(dr["usl_usu_id"]) };
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(obj.usl_id.ToString());

            return obj;
        }

        public override SIS_Usuario_Login Mapear(DataRow dr)
        {
            Entidades.App.SIS_Usuario_Login obj = MapearSimple(dr);
            return obj;
        }

        public override SIS_Usuario_Login MapearCompleto(DataRow dr)
        {
            Entidades.App.SIS_Usuario_Login obj = Mapear(dr);

            // Propiedades específicas

            return obj;
        }

        public override Entidades.App.ObjectMessage Save(SIS_Usuario_Login Obj)
        {
            Entidades.App.ObjectMessage oM = new ObjectMessage();


            return oM;
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM SIS_Usuarios_Login ";
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
           
            return obj;
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public override SIS_Usuario_Login ObjetoNuevo()
        {
            return new SIS_Usuario_Login();
        }
    }
}
