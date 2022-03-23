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
    public class ManipulacionesLogisticasController : ControllerBaseV2
    {

        #region Gestión Tipos de Manipulaciones

        public ActionResult Tipos()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PartialGridDataTipoManipulaciones()
        {
            List<Entidades.TipoManipulacionLogistica> lst = new Negocio.TipoManipulacionesLogisticas(GetToken()).ListarActivos();

            return lst.Count > 0 ?
                PartialView("_GridDataTipoManipulaciones", lst) :
                PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", new ObjectMessage() { Message = "No Hay Tipos de Manipulaciones para mostrar." });
        }

        [HttpPost]
        public JsonResult Delete(string BorrarID)
        {
            ObjectMessage oM = new Negocio.TipoManipulacionesLogisticas(GetToken()).DeleteLogico(BorrarID);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult PartialModalABMTipoManipulaciones(string cabeceraID, string detalleID, string subdetalleID)
        {
            return PartialView("_ModalABMTipoManipulaciones", new Negocio.TipoManipulacionesLogisticas(GetToken()).ObtenerPorID(cabeceraID));
        }

        [HttpPost]
        public JsonResult SaveModalTipoManipulaciones(Entidades.TipoManipulacionLogistica obj)
        {
            ObjectMessage oM = new Negocio.TipoManipulacionesLogisticas(GetToken()).Save(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}