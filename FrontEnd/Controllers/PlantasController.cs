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
    public class PlantasController : ControllerBaseV2
    {

        #region Gestión de Plantas

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PartialGridDataPlantas(string provinciaID, string localidadID)
        {
            List<Entidades.Planta> lst = new Negocio.Plantas(GetToken()).ListarPorProvinciaLocalidad(provinciaID, localidadID);

            return lst.Count > 0 ?
                PartialView("_GridDataPlantas", lst) :
                PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", new ObjectMessage() { Message = "No Hay Plantas para mostrar." });
        }

        [HttpPost]
        public JsonResult Delete(string BorrarID)
        {
            ObjectMessage oM = new Negocio.Plantas(GetToken()).DeleteLogico(BorrarID);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult PartialModalABMPlantas(string cabeceraID, string detalleID, string subdetalleID)
        {
            return PartialView("_ModalABMPlantas", new Negocio.Plantas(GetToken()).ObtenerPorID(cabeceraID));
        }

        [HttpPost]
        public JsonResult SaveModalPlantas(Entidades.Planta obj)
        {
            ObjectMessage oM = new Negocio.Plantas(GetToken()).Save(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}