using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Vistas.Graficos
{
    public class PieChartElement
    {
        public string label { get; set; }
        public double value { get; set; }
        public string formatted { get; set; }

        public PieChartElement(string _label, decimal _value, decimal total)
        {
            label = _label;
            decimal mult = 100 / total;
            value = Math.Round(Convert.ToDouble(_value * mult), 1);
            formatted = Convert.ToString(Math.Round(Convert.ToDouble(_value * mult), 1)) + "%";
        }
    }
}
