using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Vistas
{
    public class SeguimientoProyecto
    {
        public Entidades.Proyecto  DatosProyecto { get; set; }
        public List<Entidades.Proyecto_Objetivo> Objetivos { get; set; }
        public List<Entidades.Proyecto_Indicador> Indicadores { get; set; }

        public SeguimientoProyecto()
        {

        }

    }
}
