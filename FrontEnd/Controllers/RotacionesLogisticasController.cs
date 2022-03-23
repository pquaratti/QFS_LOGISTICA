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
    public class RotacionesLogisticasController : ControllerBaseV2
    {

        #region Gestión Tipos de Rotaciones

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PartialGridDataTipoRotaciones()
        {
            List<Entidades.TipoRotacionLogistica> lst = new Negocio.TipoRotacionesLogisticas(GetToken()).ListarActivos();

            return lst.Count > 0 ?
                PartialView("_GridDataTipoRotaciones", lst) :
                PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", new ObjectMessage() { Message = "No Hay Tipos de Rotaciones para mostrar." });
        }

        [HttpPost]
        public JsonResult Delete(string BorrarID)
        {
            ObjectMessage oM = new Negocio.TipoRotacionesLogisticas(GetToken()).DeleteLogico(BorrarID);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult PartialModalABMTipoRotaciones(string cabeceraID, string detalleID, string subdetalleID)
        {
            return PartialView("_ModalABMTipoRotaciones", new Negocio.TipoRotacionesLogisticas(GetToken()).ObtenerPorID(cabeceraID));
        }

        [HttpPost]
        public JsonResult SaveModalTipoRotaciones(Entidades.TipoRotacionLogistica obj)
        {
            ObjectMessage oM = new Negocio.TipoRotacionesLogisticas(GetToken()).Save(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}