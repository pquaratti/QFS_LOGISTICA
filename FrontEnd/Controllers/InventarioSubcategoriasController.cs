using Entidades.App;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FrontEnd.Controllers
{
    [Filters.VerificarToken]
    [Filters.VerificarModule]
    public class InventarioSubcategoriasController : ControllerBaseV2
    {
        public ActionResult Index() => View();

        [HttpGet]
        public ActionResult PartialGridData()
        {
            List<Entidades.Inventario.SubcategoriaProducto> lst = new Negocio.Inventario.SubcategoriasProductos(GetToken()).ListarActivos();
            return lst.Count > 0
                ? (ActionResult)PartialView("_GridData", lst)
                : PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", new ObjectMessage() { Message = "No hay subcategorías para mostrar." });
        }

        [HttpGet]
        public ActionResult PartialModalABM(string cabeceraID, string detalleID, string subdetalleID)
        {
            ViewBag.RubrosDDL = new Negocio.Inventario.RubrosProductos(GetToken()).ListarDLL(true);
            ViewBag.CategoriasDDL = new Negocio.Inventario.CategoriasProductos(GetToken()).ListarDLL(true);
            return PartialView("_ModalABM", new Negocio.Inventario.SubcategoriasProductos(GetToken()).ObtenerPorID(cabeceraID));
        }

        [HttpPost]
        public JsonResult SaveModal(Entidades.Inventario.SubcategoriaProducto obj)
            => Json(new { Result = new Negocio.Inventario.SubcategoriasProductos(GetToken()).Save(obj) }, JsonRequestBehavior.AllowGet);

        [HttpPost]
        public JsonResult Delete(string BorrarID)
            => Json(new { Result = new Negocio.Inventario.SubcategoriasProductos(GetToken()).DeleteLogico(BorrarID) }, JsonRequestBehavior.AllowGet);
    }
}
