using Entidades.App;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FrontEnd.Controllers
{
    [Filters.VerificarToken]
    [Filters.VerificarModule]
    public class InventarioRubrosController : ControllerBaseV2
    {
        public ActionResult Index() => View();

        [HttpGet]
        public ActionResult PartialGridData()
        {
            List<Entidades.Inventario.RubroProducto> lst = new Negocio.Inventario.RubrosProductos(GetToken()).ListarActivos();
            return lst.Count > 0
                ? (ActionResult)PartialView("_GridData", lst)
                : PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", new ObjectMessage() { Message = "No hay rubros para mostrar." });
        }

        [HttpGet]
        public ActionResult PartialModalABM(string cabeceraID, string detalleID, string subdetalleID)
            => PartialView("_ModalABM", new Negocio.Inventario.RubrosProductos(GetToken()).ObtenerPorID(cabeceraID));

        [HttpPost]
        public JsonResult SaveModal(Entidades.Inventario.RubroProducto obj)
            => Json(new { Result = new Negocio.Inventario.RubrosProductos(GetToken()).Save(obj) }, JsonRequestBehavior.AllowGet);

        [HttpPost]
        public JsonResult Delete(string BorrarID)
            => Json(new { Result = new Negocio.Inventario.RubrosProductos(GetToken()).DeleteLogico(BorrarID) }, JsonRequestBehavior.AllowGet);
    }
}
