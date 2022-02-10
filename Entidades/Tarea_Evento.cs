using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Tarea_Evento
    {
        public int tareve_id { get; set; }
        public Entidades.Tarea DatosTarea { get; set; }
        public Entidades.Tipo_Evento_Tarea TipoEvento { get; set; }
        public string tareve_detalle { get; set; }
        public DateTime tareve_fecha { get; set; }
        public Entidades.App.SIS_Usuario DatosUsuario { get; set; }

        public Tarea_Evento()
        {
            this.tareve_id = 0;
        }
    }
}
