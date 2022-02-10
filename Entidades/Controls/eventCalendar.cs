using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Controls
{
    public class eventCalendar
    {    

        public string ID { get; set; }
        public string Titulo { get; set; }
        public int Dia { get; set; }
        public int Mes { get; set; }
        public int Anio { get; set; }
        public int Hour { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }

        public int Dia_Hasta { get; set; }
        public int Mes_Hasta { get; set; }
        public int Anio_Hasta { get; set; }
        public int Hour_Hasta { get; set; }
        public int Minutes_Hasta { get; set; }
        public int Seconds_Hasta { get; set; }

        public string ContentHtmlPopup { get; set; }

        public string ClassNameBackground { get; set; }
        public string UrlRedirect { get; set; }

        public string UrlController { get; set; }
        public string UrlAction { get; set; }

        public eventCalendar()
        {

        }

        public static string Verde()
        {
            return "fc-event-success";
        }

        public static string Amarillo()
        {
            return "fc-event-warning";
        }

        public static string Naranja()
        {
            return "fc-event-semiwarning";
        }

        public static string Rojo()
        {
            return "fc-event-danger";
        }

        public static string Azul()
        {
            return "fc-event-primary";
        }

        public static string Default()
        {
            return "fc-event-default";
        }

        public static string Violeta()
        {
            return "fc-event-violeta";
        }

        public static string Rojo2()
        {
            return "fc-event-danger2";
        }

        public static string Verde2()
        {
            return "fc-event-green";
        }

        public static string Verde3()
        {
            return "fc-event-green3";
        }

        public static string Violeta2()
        {
            return "fc-event-violeta2";
        }

        public static string Violeta3()
        {
            return "fc-event-violeta3";
        }
    }
}
