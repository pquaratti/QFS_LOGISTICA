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
            ViewBag.RubrosDDL = new Negocio.Inventario.RubrosProductos(GetToken()).ListarDLL(true);
            ViewBag.CategoriasDDL = new Negocio.Inventario.CategoriasProductos(GetToken()).ListarDLL(true);
            ViewBag.SubcategoriasDDL = new Negocio.Inventario.SubcategoriasProductos(GetToken()).ListarDLL(true);
            return View();
        }

        [HttpGet]
        public ActionResult PartialGridData(string rubroID = "0", string categoriaID = "0", string subcategoriaID = "0", string texto = "")
        {
            List<Entidades.Inventario.Producto> lst = new Negocio.Inventario.Productos(GetToken()).ListarActivos();
            if (rubroID != "0") lst = lst.FindAll(x => x.RubroProducto.rubpro_id.ToString() == rubroID);
            if (categoriaID != "0") lst = lst.FindAll(x => x.CategoriaProducto.catpro_id.ToString() == categoriaID);
            if (subcategoriaID != "0") lst = lst.FindAll(x => x.SubcategoriaProducto.subcatpro_id.ToString() == subcategoriaID);
            if (!string.IsNullOrWhiteSpace(texto))
                lst = lst.FindAll(x => (x.pro_codigo_interno + " " + x.pro_descripcion_corta).ToLower().Contains(texto.ToLower()));

            return lst.Count > 0
                ? (ActionResult)PartialView("_GridData", lst)
                : PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", new ObjectMessage() { Message = "No hay productos para mostrar." });
        }

        [HttpGet]
        public ActionResult PartialModalABM(string cabeceraID, string detalleID, string subdetalleID)
        {
            ViewBag.RubrosDDL = new Negocio.Inventario.RubrosProductos(GetToken()).ListarDLL(true);
            ViewBag.CategoriasDDL = new Negocio.Inventario.CategoriasProductos(GetToken()).ListarDLL(true);
            ViewBag.SubcategoriasDDL = new Negocio.Inventario.SubcategoriasProductos(GetToken()).ListarDLL(true);
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
