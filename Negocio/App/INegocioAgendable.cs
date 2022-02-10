using Entidades.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Negocio.App
{
    public interface INegocioAgendable
    {
        fullCalendar CrearCalendario(string id);
        eventCalendar MaquetarAgendable(IAgendable item, eventCalendar oEvento);
    }
}
