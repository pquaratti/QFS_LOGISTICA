using Entidades.App;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FrontEnd.Controllers
{
    [Filters.VerificarToken]
    [Filters.VerificarModule]
    public class InventarioCategoriasController : ControllerBaseV2
    {
        public ActionResult Index() => View();

        [HttpGet]
        public ActionResult PartialGridData()
        {
            List<Entidades.Inventario.CategoriaProducto> lst = new Negocio.Inventario.CategoriasProductos(GetToken()).ListarActivos();
            return lst.Count > 0
                ? (ActionResult)PartialView("_GridData", lst)
                : PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", new ObjectMessage() { Message = "No hay categorías para mostrar." });
        }

        [HttpGet]
        public ActionResult PartialModalABM(string cabeceraID, string detalleID, string subdetalleID)
        {
            ViewBag.RubrosDDL = new Negocio.Inventario.RubrosProductos(GetToken()).ListarDLL(true);
            return PartialView("_ModalABM", new Negocio.Inventario.CategoriasProductos(GetToken()).ObtenerPorID(cabeceraID));
        }

        [HttpPost]
        public JsonResult SaveModal(Entidades.Inventario.CategoriaProducto obj)
            => Json(new { Result = new Negocio.Inventario.CategoriasProductos(GetToken()).Save(obj) }, JsonRequestBehavior.AllowGet);

        [HttpPost]
        public JsonResult Delete(string BorrarID)
            => Json(new { Result = new Negocio.Inventario.CategoriasProductos(GetToken()).DeleteLogico(BorrarID) }, JsonRequestBehavior.AllowGet);
    }
}
