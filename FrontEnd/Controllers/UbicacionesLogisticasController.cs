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
    public class UbicacionesLogisticasController : ControllerBaseV2
    {

        #region Gestión Tipos de Ubicaciones

        public ActionResult Tipos()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PartialGridDataTipoUbicaciones()
        {
            List<Entidades.TipoUbicacionLogistica> lst = new Negocio.TipoUbicacionesLogisticas(GetToken()).ListarActivos();

            return lst.Count > 0 ?
                PartialView("_GridDataTipoUbicaciones", lst) :
                PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", new ObjectMessage() { Message = "No Hay Tipos de Ubicaciones para mostrar." });
        }

        [HttpPost]
        public JsonResult Delete(string BorrarID)
        {
            ObjectMessage oM = new Negocio.TipoUbicacionesLogisticas(GetToken()).DeleteLogico(BorrarID);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult PartialModalABMTipoUbicaciones(string cabeceraID, string detalleID, string subdetalleID)
        {
            return PartialView("_ModalABMTipoUbicaciones", new Negocio.TipoUbicacionesLogisticas(GetToken()).ObtenerPorID(cabeceraID));
        }

        [HttpPost]
        public JsonResult SaveModalTipoUbicaciones(Entidades.TipoUbicacionLogistica obj)
        {
            ObjectMessage oM = new Negocio.TipoUbicacionesLogisticas(GetToken()).Save(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}