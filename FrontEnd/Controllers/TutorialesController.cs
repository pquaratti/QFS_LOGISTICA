using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entidades.App;
using Newtonsoft.Json;

namespace FrontEnd.Controllers
{
    [Filters.VerificarToken]
    [Filters.VerificarModule]
    public class TutorialesController : ControllerBaseV2
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Tutoriales()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PartialGridData()
        {
            Entidades.App.Token oToken = GetToken();

            List<Entidades.Tutorial> lst = new List<Entidades.Tutorial>();

            lst = new Negocio.Tutoriales(GetToken()).Listar();
            if (lst.Count > 0)
            {
                string _partialViewName = "_GridData";
                return PartialView(_partialViewName, lst);
            }
            else
            {
                Entidades.App.ObjectMessage oM = new ObjectMessage();
                oM.Message = "No se registran tutoriales.";
                return PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", oM);
            }
        }

        [HttpGet]
        public ActionResult PartialModalABM(string cabeceraID, string detalleID, string subdetalleID)
        {
            Entidades.App.Token oToken = GetToken();

            Entidades.Tutorial item = new Entidades.Tutorial();
            item.tut_id = Convert.ToInt32(cabeceraID);

            if (item.tut_id > 0)
                item = new Negocio.Tutoriales(GetToken()).ObtenerPorID(cabeceraID);

            string _partialViewName = "_ModalABM";
            return PartialView(_partialViewName, item);
        }

      
        [HttpPost]
        public JsonResult SaveModal(Entidades.Tutorial obj)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();

            Entidades.App.Token oToken = GetToken();

            // Guardo el Adjunto
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase files = Request.Files[0];
                string _fileNameUpload = DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + files.FileName;
                string path = Path.Combine(Server.MapPath("~/UploadTutoriales"), Path.GetFileName(_fileNameUpload));
                files.SaveAs(path);
                obj.tut_archivo = _fileNameUpload;
            }

            oM = new Negocio.Tutoriales(GetToken()).Save(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(string BorrarID)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            Negocio.Tutoriales n = new Negocio.Tutoriales(GetToken());
            string usu = Filters.VerificarToken.ConsultarToken().UserID;
            oM = n.DeleteLogico(Convert.ToInt32(usu), Convert.ToInt32(BorrarID));
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ActivarDesactivarTutorial(string tutorialID, bool changeStatus)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();

            if (tutorialID.Trim().Length == 0)
            {
                oM.Success = false;
                oM.Message = "No se pudo cambiar el estado del tutorial";
                return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
            }
            Negocio.Tutoriales negocio = new Negocio.Tutoriales(GetToken());
            oM = negocio.ActivarDesactivar(Convert.ToInt32(tutorialID), changeStatus);

            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ImportarTutoriales(FormCollection forms, string tutorialID)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();

            HttpPostedFileBase files = Request.Files[0];
            string _fileNameUpload = DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + files.FileName;
            string path = Path.Combine(Server.MapPath("~/UploadTutoriales"), Path.GetFileName(_fileNameUpload));
            files.SaveAs(path);

            Negocio.Tutoriales n = new Negocio.Tutoriales(GetToken());
            oM = n.ImportTutorial(_fileNameUpload, tutorialID);

            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PartialModalImportacion(string cabeceraID, string detalleID, string subdetalleID)
        {
            Entidades.App.Token oToken = GetToken();
            string _partialViewName = "_ModalImportacion";
            return PartialView(_partialViewName, cabeceraID);
        }


        [HttpGet]
        public ActionResult PartialItemsTutoriales()
        {
            Entidades.App.Token oToken = GetToken();

            List<Entidades.Tutorial> lst = new List<Entidades.Tutorial>();

            lst = new Negocio.Tutoriales(GetToken()).ListarActivos();
            
            string _partialViewName = "_ItemsTutoriales";
            return PartialView(_partialViewName, lst);
        }


    }
}