using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.App
{
    public class Reporte
    {
        public string Nombre { get; set; }
        public bool Excel { get; set; }
       
        public List<ReporteParametro> Parametros { get; set; }

        public Reporte()
        {
            this.Parametros = new List<ReporteParametro>();
            this.Excel = false;
        }
    }

    public class ReporteParametro
    {
        public string Nombre { get; set; }
        public string Valor { get; set; }
        public bool EsFecha { get; set; }
    }
}
