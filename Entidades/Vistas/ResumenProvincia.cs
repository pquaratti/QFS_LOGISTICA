using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Vistas
{
    public class ResumenProvincia
    {
        public decimal PorcentajeEscrutado { get; set; }
        public int TelegramaTotal { get; set; }
        public int TelegramasIngresados { get; set; }
        public string Provincia { get; set; }
        public int CantidadUsuarios { get; set; }
        public string ProvinciaAbreviatura { get; set; }
    }
}
