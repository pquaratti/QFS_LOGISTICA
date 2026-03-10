using Entidades.App;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FrontEnd.Controllers
{
    [Filters.VerificarToken]
    [Filters.VerificarModule]
    public class InventarioUnidadesMedidasController : ControllerBaseV2
    {
        public ActionResult Index() => View();

        [HttpGet]
        public ActionResult PartialGridData()
        {
            List<Entidades.Inventario.UnidadMedida> lst = new Negocio.Inventario.UnidadesMedidas(GetToken()).ListarActivos();
            return lst.Count > 0
                ? (ActionResult)PartialView("_GridData", lst)
                : PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", new ObjectMessage() { Message = "No hay unidades de medida para mostrar." });
        }

        [HttpGet]
        public ActionResult PartialModalABM(string cabeceraID, string detalleID, string subdetalleID)
            => PartialView("_ModalABM", new Negocio.Inventario.UnidadesMedidas(GetToken()).ObtenerPorID(cabeceraID));

        [HttpPost]
        public JsonResult SaveModal(Entidades.Inventario.UnidadMedida obj)
            => Json(new { Result = new Negocio.Inventario.UnidadesMedidas(GetToken()).Save(obj) }, JsonRequestBehavior.AllowGet);

        [HttpPost]
        public JsonResult Delete(string BorrarID)
            => Json(new { Result = new Negocio.Inventario.UnidadesMedidas(GetToken()).DeleteLogico(BorrarID) }, JsonRequestBehavior.AllowGet);
    }
}
