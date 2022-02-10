using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrontEnd.Controllers
{
    [Filters.VerificarToken]
    public class NotificacionesController : ControllerBaseV2
    {
        public ActionResult Index()
        {
           return View();
        }

        //[HttpGet]
        //public ActionResult PartialGridData(int distritoID)
        //{
        //    List<Entidades.Notificacion> lst = new List<Entidades.Notificacion>();

        //    if (GetToken().Administrador)
        //        lst = new Negocio.Notificaciones(GetToken()).ListarPorDistrito(distritoID);
        //    else
        //        lst = new Negocio.Notificaciones(GetToken()).ListarPorDistrito(Convert.ToInt32(GetToken().DistritoID));

        //    string _partialViewName = "_GridData";
        //    return PartialView(_partialViewName, lst);
        //}

        [HttpGet]
        public ActionResult PartialModalABM(string cabeceraID, string detalleID, string subdetalleID)
        {
            Entidades.Notificacion item = new Entidades.Notificacion();
            item.not_id = Convert.ToInt32(cabeceraID);

            if (Convert.ToInt32(cabeceraID) > 0)
                item = new Negocio.Notificaciones(GetToken()).ObtenerPorID(cabeceraID);

            string _partialViewName = "_ModalABM";
            return PartialView(_partialViewName, item);
        }

        //[HttpPost]
        //public JsonResult SaveModal(Entidades.Notificacion obj)
        //{
        //    Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
        //    oM = new Negocio.Notificaciones(GetToken()).Save(obj);
        //    return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        //}

        //[HttpGet]
        //public ActionResult PartialVistaDistrito()
        //{
        //    List<Entidades.Notificacion> lst = new List<Entidades.Notificacion>();
        //    lst = new Negocio.Notificaciones(GetToken()).ListarPorDistrito(Convert.ToInt32(GetToken().DistritoID));
        //    string _partialViewName = "_PartialVistaDistrito";
        //    return PartialView(_partialViewName, lst);
        //}

    }
}