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
    public class SkillsController : ControllerBaseV2
    {

        #region Skills

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PartialModalABM(string cabeceraID, string detalleID, string subdetalleID)
        {
            Entidades.Skill item = new Entidades.Skill();
            item = new Negocio.Skills(GetToken()).ObjetoNuevo();
            if (cabeceraID != "0")
            {
                item = new Negocio.Skills(GetToken()).ObtenerPorID(cabeceraID);
            }
            string _partialViewName = "_ModalABM";
            return PartialView(_partialViewName, item);
        }

        [HttpGet]
        public ActionResult PartialGridData()
        {
            List<Entidades.Skill> lst = new List<Entidades.Skill>();
            lst = new Negocio.Skills(GetToken()).Listar();
           
            if (lst.Count > 0)
            {
                string _partialViewName = "_GridData";
                return PartialView(_partialViewName, lst);
            }
            else
            {
                Entidades.App.ObjectMessage oM = new ObjectMessage();
                oM.Message = "No hay Skills para mostrar";
                return PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", oM);
            }
        }

        [HttpPost]
        public JsonResult Delete(string BorrarID)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            Negocio.Skills n = new Negocio.Skills(GetToken());
            string usu = Filters.VerificarToken.ConsultarToken().UserID;
            oM = n.Delete(Convert.ToInt32(BorrarID));
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }
       
        [HttpPost]
        public JsonResult SaveModal(Entidades.Skill obj)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();

            oM = new Negocio.Skills(GetToken()).Save(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        #endregion

       
    }
}