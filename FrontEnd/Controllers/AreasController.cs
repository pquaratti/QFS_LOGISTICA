using Entidades.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrontEnd.Controllers
{
    public class AreasController : ControllerBaseV2
    {
        // GET: Areas
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PartialModalABM(string cabeceraID, string detalleID, string subdetalleID)
        {
            Entidades.App.SIS_Area item = new Entidades.App.SIS_Area();
            item.area_id = Convert.ToInt32(cabeceraID);

            if (Convert.ToInt32(cabeceraID) > 0)
                item = new Negocio.App.SIS_Areas(GetToken()).ObtenerPorID(cabeceraID);

            ViewBag.AreasPadre = Negocio.DDL.ListarAreasPadre(GetToken(), "-");

            string _partialViewName = "_ModalABM";
            return PartialView(_partialViewName, item);
        }

        [HttpGet]
        public ActionResult PartialGridData()
        {
            List<Entidades.App.SIS_Area> lst = new List<Entidades.App.SIS_Area>();
            lst = new Negocio.App.SIS_Areas(GetToken()).Listar();

            if (lst.Count > 0)
            {
                string _partialViewName = "_GridData";
                return PartialView(_partialViewName, lst);
            }
            else
            {
                Entidades.App.ObjectMessage oM = new ObjectMessage();
                oM.Message = "Sin resultados";
                return PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", oM);
            }
        }

        [HttpPost]
        public JsonResult Delete(string BorrarID)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            Negocio.App.SIS_Areas n = new Negocio.App.SIS_Areas(GetToken());
            string usu = Filters.VerificarToken.ConsultarToken().UserID;
            oM = n.Delete(Convert.ToInt32(BorrarID));
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveModal(Entidades.App.SIS_Area obj)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            Entidades.App.Token oToken = GetToken();

            obj.Organizacion = new SIS_Organizacion() { org_id = Convert.ToInt32(oToken.OrganizacionID) };

            oM = new Negocio.App.SIS_Areas(GetToken()).Save(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }
    }
}