using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Vistas
{
    public class ObjetivoResumenVista
    {
        public string titulo { get; set; }
        public decimal valor_inicial { get; set; }
        public decimal valor_meta { get; set; }
        public string url_imagen { get; set; }
        public string fecha_fin { get; set; }
        public object cantidadIndicadores { get; set; }
    }
}
