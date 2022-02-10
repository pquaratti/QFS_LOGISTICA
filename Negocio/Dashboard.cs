using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Negocio
{
    public class Dashboard
    {
        public static Helpers.SQLDb dbStatic = new Helpers.SQLDb();

        public static Entidades.Dashboard ObtenerHome()
        {
            Entidades.Dashboard obj = new Entidades.Dashboard();

            string sQuery = "";
            sQuery += "  select * from ( ";
            sQuery += "  select (select Count(*) as total From afiliado where afi_activo=1) as afiliados,  ";
            sQuery += "  (select Count(*) as total From afiliado where afi_activo=1 and YEAR(afi_fec_alta)=YEAR(GETDATE()) and MONTH(afi_fec_alta)=MONTH(GETDATE())) as altas_mes, ";
            sQuery += "  (select Count(*) as total From Obra_Social where oso_activo=1) as obras_sociales, ";
            sQuery += "  (select Count(*) as total From Planes where pla_activo=1) as planes ";
            sQuery += " ) tb_totales_dashboard ";

            DataTable dt = dbStatic.SQLSelect(sQuery);

            obj.TotalAfiliadosActivos = Resources.Validaciones.valNULLINT(dt.Rows[0]["afiliados"]);
            obj.TotalAltasMes = Resources.Validaciones.valNULLINT(dt.Rows[0]["altas_mes"]);
            obj.TotalObrasSociales = Resources.Validaciones.valNULLINT(dt.Rows[0]["obras_sociales"]);
            obj.TotalPlanes = Resources.Validaciones.valNULLINT(dt.Rows[0]["planes"]);

            return obj;
        }

    }
}
