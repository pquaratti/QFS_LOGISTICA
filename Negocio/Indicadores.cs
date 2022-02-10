using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Negocio
{
    public class Indicadores
    {
        public Helpers.SQLDb db;
        Entidades.App.Token Token;
        string sQuery = "";

        public Indicadores(Entidades.App.Token paramToken)
        {
            Token = paramToken;
            db = new Helpers.SQLDb();
        }

        public Entidades.Vistas.DashboardSistemas IndicadoresPrincipalesSistemas()
        {
            Entidades.Vistas.DashboardSistemas obj = new Entidades.Vistas.DashboardSistemas();

            sQuery = "select ";
            sQuery += " (select COUNT(*) from SIS_Usuarios where usu_fec_eliminado is null) as 'CantidadUsuarios', ";
            sQuery += " (select COUNT(*) from Distritos where dis_activo=1) as 'CantidadDistritos', ";
            sQuery += " (select COUNT(*) from SIS_Usuarios where usu_fec_eliminado is null and usu_administrador=1) as 'CantidadUsuariosAdministradores', ";
            sQuery += " (select COUNT(*) from SIS_Usuarios where usu_fec_eliminado is null and usu_intentos > 3) as 'CantidadUsuariosBloqueados' ";

            DataTable dt_bus = db.SQLSelect(sQuery);

            foreach (DataRow row in dt_bus.Rows)
            {
                obj.CantidadUsuarios = Resources.Validaciones.valNULLINT(row["CantidadUsuarios"]);
                obj.CantidadDistritos = Resources.Validaciones.valNULLINT(row["CantidadDistritos"]);
                obj.CantidadUsuariosAdministradores = Resources.Validaciones.valNULLINT(row["CantidadUsuariosAdministradores"]);
                obj.CantidadUsuariosBloqueados = Resources.Validaciones.valNULLINT(row["CantidadUsuariosBloqueados"]);
            }

            return obj;
        }

    }
}
