using Entidades.App;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FrontEnd.Controllers
{
    [Filters.VerificarToken]
    [Filters.VerificarModule]
    public class InventarioProductosController : ControllerBaseV2
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PartialGridData()
        {
            List<Entidades.Inventario.Producto> lst = new Negocio.Inventario.Productos(GetToken()).ListarActivos();
            return lst.Count > 0
                ? (ActionResult)PartialView("_GridData", lst)
                : PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", new ObjectMessage() { Message = "No hay productos para mostrar." });
        }

        [HttpGet]
        public ActionResult PartialModalABM(string cabeceraID, string detalleID, string subdetalleID)
        {
            ViewBag.CategoriasDDL = new Negocio.Inventario.CategoriasProductos(GetToken()).ListarDLL(true);
            ViewBag.UnidadesDDL = new Negocio.Inventario.UnidadesMedidas(GetToken()).ListarDLL(true);
            return PartialView("_ModalABM", new Negocio.Inventario.Productos(GetToken()).ObtenerPorID(cabeceraID));
        }

        [HttpPost]
        public JsonResult SaveModal(Entidades.Inventario.Producto obj)
        {
            ObjectMessage oM = new Negocio.Inventario.Productos(GetToken()).Save(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(string BorrarID)
        {
            ObjectMessage oM = new Negocio.Inventario.Productos(GetToken()).DeleteLogico(BorrarID);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }
    }
}
