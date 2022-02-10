using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrontEnd.Controllers
{
    [Filters.VerificarToken]
    [Filters.VerificarModule]
    public class CalendarioController : ControllerBaseV2
    {

        [HttpGet]
        public ActionResult PartialContentCalendarioPrincipal()
        {
            Negocio.App.Calendario negocioCALENDAR = new Negocio.App.Calendario();
            List<Negocio.App.INegocioAgendable> lst = new List<Negocio.App.INegocioAgendable>
            {
                //new Negocio.Eventos(GetToken()),
               // new Negocio.TareasColaboradores(GetToken())
            };

            Entidades.Controls.fullCalendar calendario = negocioCALENDAR.CalendarioGeneral("1", lst);

            foreach (Entidades.Controls.eventCalendar itemEvento in calendario.Eventos)
            {
                if (itemEvento.UrlController.Length > 0 && itemEvento.UrlAction.Length > 0)
                    itemEvento.UrlRedirect = Url.RouteUrl("Default", new { action = itemEvento.UrlAction, controller = itemEvento.UrlController });
            }

            string _partialViewName = "_PartialCalendarioGeneral";
            return PartialView(_partialViewName, calendario);
        }

      

    }
}