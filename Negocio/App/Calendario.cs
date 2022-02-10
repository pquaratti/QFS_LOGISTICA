using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Negocio.App
{
    public class Calendario 
    {
        public Entidades.Controls.fullCalendar CalendarioGeneral(string id, List<Negocio.App.INegocioAgendable> agendables)
        {
            Entidades.Controls.fullCalendar obj = new Entidades.Controls.fullCalendar();
            obj.EventClick = false;
            obj.EventRender = false;
            obj.ID = "calendar";

            obj.fechaInicio = DateTime.Now;
            obj.fechaFin = DateTime.Now;

            foreach (Negocio.App.INegocioAgendable agendable in agendables)
            {
                Entidades.Controls.fullCalendar calendario = agendable.CrearCalendario(id);
                obj.Eventos.AddRange(calendario.Eventos);
            }
            return obj;
        }
        public Entidades.Controls.fullCalendar CargarParametros(IEnumerable<object> lst, INegocioAgendable negocioAgendable, Entidades.Controls.fullCalendar obj)
        {
            List<Entidades.Controls.IAgendable> lista = (List<Entidades.Controls.IAgendable>)lst;

            obj.EventClick = false;
            obj.EventRender = false;
            obj.ID = "calendar";
            if ( lista.Count  > 0)
            {
                obj.fechaInicio = lista.Min(m => m.agendable_fecha);
                obj.fechaFin = lista.Max(m => m.agendable_fecha);
            }
            else
            {
                obj.fechaInicio = DateTime.Now;
                obj.fechaFin = DateTime.Now;
            }
            foreach (Entidades.Controls.IAgendable item in lista)
            {

                Entidades.Controls.eventCalendar oEvento = new Entidades.Controls.eventCalendar();
                oEvento.Titulo = item.agendable_titulo;
                oEvento.ID = item.agendable_id.ToString();

                oEvento.Anio = item.agendable_fecha.Year;
                oEvento.Mes = item.agendable_fecha.Month;
                oEvento.Dia = item.agendable_fecha.Day;
                oEvento.Hour = item.agendable_fecha.Hour;
                oEvento.Minutes = item.agendable_fecha.Minute;
                oEvento.Seconds = item.agendable_fecha.Second;

                oEvento.Anio_Hasta = item.agendable_fecha_hasta.Year;
                oEvento.Mes_Hasta = item.agendable_fecha_hasta.Month;
                oEvento.Dia_Hasta = item.agendable_fecha_hasta.Day;
                oEvento.Hour_Hasta = item.agendable_fecha_hasta.Hour;
                oEvento.Minutes_Hasta = item.agendable_fecha_hasta.Minute;
                oEvento.Seconds_Hasta = item.agendable_fecha_hasta.Second;

                oEvento = negocioAgendable.MaquetarAgendable(item, oEvento);

                obj.Eventos.Add(oEvento);
            }

            return obj;
        }
    }
}
