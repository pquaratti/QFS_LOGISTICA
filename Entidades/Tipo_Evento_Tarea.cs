using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Tipo_Evento_Tarea
    {
        public int tevetar_id { get; set; }

        public string tevetar_nombre { get; set; }

        public Tipo_Evento_Tarea()
        {
            this.tevetar_id = 0;
            this.tevetar_nombre = "";
        }
    }
}
