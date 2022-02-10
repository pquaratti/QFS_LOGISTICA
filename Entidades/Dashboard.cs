using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Dashboard
    {
        public int TotalAfiliadosActivos { get; set; }
        public int TotalAltasMes { get; set; }
        public int TotalObrasSociales { get; set; }
        public int TotalPlanes { get; set; }

        public string NombreUsuario { get; set; }

        public Dashboard() { }
        
    }
}
