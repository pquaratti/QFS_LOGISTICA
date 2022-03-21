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
    public class ClientesController : ControllerBaseV2
    {

        #region Gestión de Clientes

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PartialGridDataClientes()
        {
            List<Entidades.Cliente> lst = new Negocio.Clientes(GetToken()).ListarActivos();

            return lst.Count > 0 ?
                PartialView("_GridDataClientes", lst) :
                PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", new ObjectMessage() { Message = "No Hay Clientes para mostrar." });
        }

        [HttpPost]
        public JsonResult Delete(string BorrarID)
        {
            ObjectMessage oM = new Negocio.Clientes(GetToken()).DeleteLogico(BorrarID);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult PartialModalABMClientes(string cabeceraID, string detalleID, string subdetalleID)
        {
            return PartialView("_ModalABMClientes", new Negocio.Clientes(GetToken()).ObtenerPorID(cabeceraID));
        }

        [HttpPost]
        public JsonResult SaveModalClientes(Entidades.Cliente obj)
        {
            ObjectMessage oM = new Negocio.Clientes(GetToken()).Save(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}