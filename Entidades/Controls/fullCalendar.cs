using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Controls
{
    public class fullCalendar
    {
        public string ID { get; set; }
        public bool EventClick { get; set; }
        public bool EventRender { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }

        public List<Entidades.Controls.eventCalendar> Eventos { get; set; }

        public fullCalendar()
        {
            this.EventClick = false;
            this.EventRender = false;
            this.Eventos = new List<Entidades.Controls.eventCalendar>();
        }

    }
}
