using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Vistas.Graficos
{
    public class PieChart
    {
 
        public List<PieChartElement> Elementos { get; set; }

        public PieChart()
        {
            Elementos = new List<PieChartElement>();
        }
        public PieChart(List<PieChartElement> elementos)
        {
            Elementos = elementos;
        }
    }
}
