using Entidades.App;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrontEnd.Controllers
{
    [Filters.VerificarToken]
    [Filters.VerificarModule]
    public class EventosController : ControllerBaseV2
    {

        #region Eventos

        public ActionResult Eventos()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddOrEditEVE(string id = "")
        {
            Entidades.Evento obj = new Entidades.Evento();

            Entidades.App.Token oToken = GetToken();

            if (id.Length == 0)
                obj.eve_id = 0;
            else
                obj.eve_id = Convert.ToInt32(id);

            if (obj.eve_id == 0)
            {
                obj = new Entidades.Evento();
                obj.eve_id = 0;
                obj.eve_fecha = DateTime.Now;
            }
            else
            {
                Negocio.Eventos negocio = new Negocio.Eventos(GetToken());
                obj = negocio.ObtenerPorID(id);
            }

            ViewBag.Tipo_Evento = Negocio.DDL.ListarTipoEvento(Filters.VerificarToken.ConsultarToken());

            return View(obj);
        }

        [HttpGet]
        public ActionResult PartialModalRenglonesVista(string cabeceraID, string detalleID, string subdetalleID)
        {
            List<Entidades.Evento_Renglon> lst = new List<Entidades.Evento_Renglon>();

            lst = new Negocio.EventosRenglones(GetToken()).ListarRenglonesPorEvento(Convert.ToInt32(cabeceraID));

            if (lst.Count > 0)
            {
                string _partialViewName = "_ModalRenglonesVista";
                return PartialView(_partialViewName, lst);
            }
            else
            {
                ViewBag.Titulo = "Usuarios";
                Entidades.App.ObjectMessage oM = new ObjectMessage();
                oM.Message = "No hay usuarios invitados";
                return PartialView("~/Views/Shared/_ModalMensajeSinResultados.cshtml", oM);
            }
        }

        [HttpGet]
        public ActionResult PartialModalRenglonesAsistencia(string cabeceraID, string detalleID, string subdetalleID)
        {
            List<Entidades.Evento_Renglon> lst = new List<Entidades.Evento_Renglon>();

            lst = new Negocio.EventosRenglones(GetToken()).ListarRenglonesPorEvento(Convert.ToInt32(cabeceraID));

            if (lst.Count > 0)
            {
                string _partialViewName = "_ModalRenglonesAsistencia";
                return PartialView(_partialViewName, lst);
            }
            else
            {
                ViewBag.Titulo = "Usuarios";
                Entidades.App.ObjectMessage oM = new ObjectMessage();
                oM.Message = "No hay usuarios invitados";
                return PartialView("~/Views/Shared/_ModalMensajeSinResultados.cshtml", oM);
            }
           
        }

        [HttpPost]
        public JsonResult Save(Entidades.Evento obj)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            var esNuevo = false;
            try
            {
                if (obj.eve_id.Equals(0))
                {
                    esNuevo = true;
                }

                Negocio.Eventos negocio = new Negocio.Eventos(GetToken());
                oM = negocio.Save(obj);

                if (oM.Success)
                {
                    oM.RedirectNew = esNuevo;
                    oM.urlRedirect = Url.Action("AddOrEditEVE", "Eventos", new { id = obj.eve_id });
                }

            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.Message = ex.Message;
            }

            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveModalAsistencia(List<Entidades.Evento_Renglon> lstAsistencia)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            oM = new Negocio.EventosRenglones(GetToken()).EventoAsistencia(lstAsistencia);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PartialGridDataEventos(string tipoEventoID, string estadoEvento)
        {
            List<Entidades.Evento> lst = new List<Entidades.Evento>();
            lst = new Negocio.Eventos(GetToken()).ListarPorTipoEvento(Convert.ToInt32(tipoEventoID));
            if (estadoEvento == " Finalizado ")
                lst = lst.Where(m => m.Finalizado).ToList();
            if (estadoEvento == " A Tiempo ")
                lst = lst.Where(m => !m.Finalizado).ToList();

            if (lst.Count > 0)
            {
                string _partialViewName = "_GridDataEventos";
                return PartialView(_partialViewName, lst);
            }
            else
            {
                Entidades.App.ObjectMessage oM = new ObjectMessage();
                oM.Message = "No hay eventos para mostrar";
                return PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", oM);
            }
        }

        [HttpGet]
        public ActionResult PartialGridDataRenglones(string eventoID, string finalizado)
        {
            List<Entidades.Evento_Renglon> lst = new List<Entidades.Evento_Renglon>();

            lst = new Negocio.EventosRenglones(GetToken()).ListarRenglonesPorEvento(Convert.ToInt32(eventoID));

            if (lst.Count > 0)
            {
                ViewBag.Finalizado = finalizado ;
                string _partialViewName = "_GridDataRenglones";
                return PartialView(_partialViewName, lst);
            }
            else
            {
                Entidades.App.ObjectMessage oM = new ObjectMessage();
                oM.Message = "El evento está vacío";
                return PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", oM);
            }
        }


        [HttpPost]
        public JsonResult Delete(string BorrarID)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            Negocio.Eventos n = new Negocio.Eventos(GetToken());
            string usu = Filters.VerificarToken.ConsultarToken().UserID;
            oM = n.DeleteEvento(Convert.ToInt32(BorrarID));
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }
       
        [HttpPost]
        public JsonResult SaveModalEvento(Entidades.Evento obj)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();

            oM = new Negocio.Eventos(GetToken()).Save(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PartialModalABMEventoRenglon(string cabeceraID, string detalleID, string auxID)
        {
            Entidades.Evento_Renglon item = new Entidades.Evento_Renglon();
            item.ever_id = Convert.ToInt32(detalleID);
            
            if (Convert.ToInt32(detalleID) > 0)
                item = new Negocio.EventosRenglones(GetToken()).ObtenerPorID(detalleID);

            item.Evento = new Entidades.Evento() { eve_id = Convert.ToInt32(cabeceraID) };

            ViewBag.Fecha = auxID;
            
            string _partialViewName = "_ModalABMEventoRenglones";
            return PartialView(_partialViewName, item);
        }

        [HttpPost]
        public JsonResult SaveModalEventoRenglon(Entidades.Evento_Renglon obj)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();

            oM = new Negocio.EventosRenglones(GetToken()).Save(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

      

        [HttpPost]
        public JsonResult DeleteDetalle(string BorrarID)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            Negocio.EventosRenglones n = new Negocio.EventosRenglones(GetToken());
            string usu = Filters.VerificarToken.ConsultarToken().UserID;
            oM = n.Delete(Convert.ToInt32(BorrarID));
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CerrarEvento(string accionID)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            oM = new Negocio.Eventos(GetToken()).CerrarEvento(accionID);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Mis Eventos
        public ActionResult MisEventos()
        {
            return View();
        }

        #endregion
    }
}