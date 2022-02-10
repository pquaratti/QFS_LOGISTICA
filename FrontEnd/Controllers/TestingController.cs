using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrontEnd.Controllers
{
    public class TestingController : Controller
    {
        // GET: Testing
        //public ActionResult TestReporte()
        //{
        //    Negocio.Reportes negocio = new Negocio.Reportes();

        //    byte[] bytes = negocio.ReportePlanificacion();

        //    System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
        //    string mimeType = string.Empty;
        //    return new FileStreamResult(ms, "application/pdf");
        //}

        public ActionResult TestMail()
        {
            Negocio.App.MailManager.SendMessage("pquaratti@gmail.com", "Testing", "", null, null);

            return Content("OK");
        }

        /*Ejemplo de calendar */
        //[HttpGet]
        //public ActionResult PartialContentCalendarioPrincipal()
        //{
        //    Entidades.Controls.fullCalendar oCal = negocio.ObtenerCalendarioGeneral(GetToken().Id);

        //    foreach (Entidades.Controls.eventCalendar itemEvento in oCal.Eventos)
        //    {
        //        if (itemEvento.UrlController.Length > 0 && itemEvento.UrlAction.Length > 0)
        //            itemEvento.UrlRedirect = Url.RouteUrl("Default", new { action = itemEvento.UrlAction, controller = itemEvento.UrlController });
        //    }

        //    string _partialViewName = "_PartialCalendarioGeneral";
        //    return PartialView(_partialViewName, oCal);
        //}

        //public Entidades.Controls.fullCalendar ObtenerCalendarioGeneral(int usu_id)
        //{
        //    Entidades.Controls.fullCalendar obj = new Entidades.Controls.fullCalendar();
        //    obj.EventClick = false;
        //    obj.EventRender = false;
        //    obj.ID = "calendar";

        //    obj.fechaInicio = DateTime.Now;
        //    obj.fechaFin = DateTime.Now;

        //    Negocio.Entrega negocioENT = new Entrega();
        //    Negocio.Turno negocioTUR = new Turno();
        //    Negocio.Agenda negocioAGE = new Agenda();

        //    List<Entidades.Entrega> lstEntregas = new List<Entidades.Entrega>();
        //    lstEntregas = negocioENT.ListarDesdeHoy();
        //    Entidades.Controls.fullCalendar calendarioEntregas = negocioENT.CrearCalentar(lstEntregas);
        //    obj.Eventos.AddRange(calendarioEntregas.Eventos);

        //    List<Entidades.Turno> lstTurnos = new List<Entidades.Turno>();
        //    lstTurnos.AddRange(negocioTUR.ListarTurnosVigentesDesdeHoy());
        //    Entidades.Controls.fullCalendar calendarioTurnos = negocioTUR.CrearCalentar(lstTurnos);
        //    obj.Eventos.AddRange(calendarioTurnos.Eventos);

        //    List<Entidades.Agenda> lstEventosAgenda = new List<Entidades.Agenda>();
        //    lstEventosAgenda.AddRange(negocioAGE.ListarPorUsuarioProximos(usu_id));
        //    Entidades.Controls.fullCalendar calendarioAgenda = negocioAGE.CrearCalentar(lstEventosAgenda);
        //    obj.Eventos.AddRange(calendarioAgenda.Eventos);

        //    return obj;
        //}

        //public Entidades.Controls.fullCalendar CrearCalentar(List<Entidades.Entrega> lst)
        //{
        //    Entidades.Controls.fullCalendar obj = new Entidades.Controls.fullCalendar();
        //    obj.EventClick = false;
        //    obj.EventRender = false;
        //    obj.ID = "calendar";

        //    if (lst.Count > 0)
        //    {
        //        obj.fechaInicio = lst.Min(m => m.ent_fecha);
        //        obj.fechaFin = lst.Max(m => m.ent_fecha);
        //    }
        //    else
        //    {
        //        obj.fechaInicio = DateTime.Now;
        //        obj.fechaFin = DateTime.Now;
        //    }

        //    foreach (Entidades.Entrega item in lst)
        //    {
        //        string contentHTMLPreview = "";


        //        Entidades.Controls.eventCalendar oEvento = new Entidades.Controls.eventCalendar();
        //        oEvento.Titulo = "ENTREGA #" + item.ent_id.ToString() + " - " + item.ent_nombre_cliente;
        //        oEvento.ID = item.ent_id.ToString();

        //        oEvento.Anio = item.ent_fecha.Year;
        //        oEvento.Mes = item.ent_fecha.Month;
        //        oEvento.Dia = item.ent_fecha.Day;
        //        oEvento.Hour = item.ent_fecha.Hour;
        //        oEvento.Minutes = item.ent_fecha.Minute;
        //        oEvento.Seconds = item.ent_fecha.Second;

        //        oEvento.Anio_Hasta = item.ent_fecha_hasta.Year;
        //        oEvento.Mes_Hasta = item.ent_fecha_hasta.Month;
        //        oEvento.Dia_Hasta = item.ent_fecha_hasta.Day;
        //        oEvento.Hour_Hasta = item.ent_fecha_hasta.Hour;
        //        oEvento.Minutes_Hasta = item.ent_fecha_hasta.Minute;
        //        oEvento.Seconds_Hasta = item.ent_fecha_hasta.Second;

        //        contentHTMLPreview += "<b>Observacion: </b>" + item.ent_observaciones.Replace("\r\n", "<br/>");

        //        if (item.ent_confirmado == false)
        //            contentHTMLPreview += $"<br/> <span class=\"text-danger\"> Pendiente de confirmación </span> ";

        //        if (item.Entregado == true)
        //            contentHTMLPreview += $"<br/> <span class=\"text-success\">Entregado </span> ";

        //        oEvento.ContentHtmlPopup = contentHTMLPreview;

        //        oEvento.UrlRedirect = "";
        //        oEvento.UrlController = "";
        //        oEvento.UrlAction = "";

        //        if (item.ent_fecha.ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy"))
        //        {
        //            oEvento.ClassNameBackground = Entidades.Controls.eventCalendar.Azul();
        //        }
        //        else
        //        {
        //            if (item.ent_fecha < DateTime.Now)
        //                oEvento.ClassNameBackground = Entidades.Controls.eventCalendar.Default();

        //            if (item.ent_fecha > DateTime.Now)
        //            {
        //                if (item.ent_confirmado == false)
        //                    oEvento.ClassNameBackground = Entidades.Controls.eventCalendar.Rojo2();
        //                else
        //                    oEvento.ClassNameBackground = Entidades.Controls.eventCalendar.Verde2();
        //            }
        //        }

        //        if (item.Entregado)
        //            oEvento.ClassNameBackground = Entidades.Controls.eventCalendar.Verde();

        //        obj.Eventos.Add(oEvento);
        //    }

        //    return obj;
        //}


    }
}