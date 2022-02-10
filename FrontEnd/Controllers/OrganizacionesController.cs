using Entidades.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrontEnd.Controllers
{
    [Filters.VerificarToken]
    [Filters.VerificarModule]
    public class OrganizacionesController : ControllerBaseV2
    {
        // GET: Dependencia
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PartialGridData()
        {
            Entidades.App.Token oToken = GetToken();
            List<Entidades.App.SIS_Organizacion> lst = new List<Entidades.App.SIS_Organizacion>();
            lst = new Negocio.App.SIS_Organizaciones(GetToken()).Listar();
            string _partialViewName = "_GridData";
            return PartialView(_partialViewName, lst);
        }

        [HttpGet]
        public ActionResult PartialModalABM(string cabeceraID, string detalleID, string subdetalleID)
        {
            Entidades.App.Token oToken = GetToken();
            Entidades.App.SIS_Organizacion item = new Entidades.App.SIS_Organizacion();
            item.org_id = Convert.ToInt32(cabeceraID);
            if (Convert.ToInt32(cabeceraID) > 0)
                item = new Negocio.App.SIS_Organizaciones(GetToken()).ObtenerPorID(cabeceraID);
            string _partialViewName = "_ModalABM";
            return PartialView(_partialViewName, item);
        }

        [HttpPost]
        public JsonResult SaveModal(Entidades.App.SIS_Organizacion obj)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            Entidades.App.Token oToken = GetToken();
            oM = new Negocio.App.SIS_Organizaciones(GetToken()).Save(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(string BorrarID)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            Negocio.App.SIS_Organizaciones n = new Negocio.App.SIS_Organizaciones(GetToken());
            string usu = Filters.VerificarToken.ConsultarToken().UserID;
            oM = n.DeleteLogico(Convert.ToInt32(usu), Convert.ToInt32(BorrarID));
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        public override JsonResult GetSelect2Items(string q)
        {
            var list = new List<Entidades.App.Select2Item>();
            Negocio.App.SIS_Organizaciones negocio = new Negocio.App.SIS_Organizaciones(GetToken());
            foreach (var item in negocio.ListarPorNombre(q, "30"))
            {
                list.Add(new Select2Item(item.org_id.ToString(), item.org_nombre.ToString()));
            };
            list.Add(new Select2Item("-1", "Crear nuevo"));
            return Json(new { items = list }, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public override JsonResult GetSelect2Items(string q)
        //{
        //    var list = new List<Entidades.App.Select2Item>();
        //    Negocio.App.SIS_Organizaciones negocio = new Negocio.App.SIS_Organizaciones(GetToken());

        //    foreach (var item in negocio.ListarPorNombre(q, "30"))
        //    {
        //        list.Add(new Select2Item(item.func_id.ToString(), item.func_fullname.ToString() + " - " + item.func_cargo.ToString()));
        //    };

        //    list.Add(new Select2Item("-1", "Crear nuevo"));

        //    return Json(new { items = list }, JsonRequestBehavior.AllowGet);
        //}
    }
}