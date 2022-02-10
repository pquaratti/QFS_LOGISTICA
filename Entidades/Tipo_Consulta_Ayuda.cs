using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Tipo_Consulta_Ayuda
    {
        public int tipoconsulta_id { get; set; }
        public string tipoconsulta_nombre { get; set; }

        public Tipo_Consulta_Ayuda()
        {
            this.tipoconsulta_id = 0;
            this.tipoconsulta_nombre = "";
        }

    }
}
