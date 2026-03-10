using Entidades.App;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FrontEnd.Controllers
{
    [Filters.VerificarToken]
    [Filters.VerificarModule]
    public class InventarioMovimientosController : ControllerBaseV2
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PartialGridData()
        {
            List<Entidades.Inventario.MovimientoInventario> lst = new Negocio.Inventario.MovimientosInventarios(GetToken()).Listar();
            return lst.Count > 0
                ? (ActionResult)PartialView("_GridData", lst)
                : PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", new ObjectMessage() { Message = "No hay movimientos para mostrar." });
        }

        [HttpGet]
        public ActionResult PartialModalABM(string cabeceraID, string detalleID, string subdetalleID)
        {
            ViewBag.ProductosDDL = new Negocio.Inventario.Productos(GetToken()).ListarDLL(true);
            ViewBag.DepositosDDL = new Negocio.Depositos(GetToken()).ListarDLL(true);
            ViewBag.TiposMovimientosDDL = new Negocio.Inventario.TiposMovimientosInventarios(GetToken()).ListarDLL(true);
            return PartialView("_ModalABM", new Negocio.Inventario.MovimientosInventarios(GetToken()).ObtenerPorID(cabeceraID));
        }

        [HttpPost]
        public JsonResult SaveModal(Entidades.Inventario.MovimientoInventario obj)
        {
            ObjectMessage oM = new Negocio.Inventario.MovimientosInventarios(GetToken()).Save(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }
    }
}
