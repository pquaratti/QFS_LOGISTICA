using Entidades.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FrontEnd.Controllers
{
    [Filters.VerificarToken]
    [Filters.VerificarModule]
    public class PreguntasFrecuentesController : ControllerBaseV2
    {
        // GET: Informes
        public ActionResult PreguntasFrecuentes()
        {
            List<Entidades.Pregunta_Frecuente> lst = new List<Entidades.Pregunta_Frecuente>();
            lst = new Negocio.PreguntasFrecuentes(GetToken()).Listar();
            ViewBag.Categorias = new Negocio.PreguntasFrecuentesCategorias(GetToken()).Listar();
            return View(lst);
        }

        public ActionResult PreguntasFrecuentesABM()
        {
            return View();
        }



        [HttpPost]
        public JsonResult Delete(string BorrarID)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            Negocio.PreguntasFrecuentes n = new Negocio.PreguntasFrecuentes(GetToken());
            string usu = Filters.VerificarToken.ConsultarToken().UserID;
            oM = n.Delete(Convert.ToInt32(BorrarID));
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PartialGridDataPreguntas()
        {
            List<Entidades.Pregunta_Frecuente> lst = new List<Entidades.Pregunta_Frecuente>();
            lst = new Negocio.PreguntasFrecuentes(GetToken()).Listar();
           
            string _partialViewName = "_GridDataPreguntas";
            return PartialView(_partialViewName, lst);
        }

        [HttpGet]
        public ActionResult PartialModalABMPreguntasFrecuentes(string cabeceraID, string detalleID, string subdetalleID)
        {
            Entidades.App.Token oToken = GetToken();

            Entidades.Pregunta_Frecuente item = new Entidades.Pregunta_Frecuente();
            item.pgf_id = Convert.ToInt32(cabeceraID);

            if (Convert.ToInt32(cabeceraID) > 0)
                item = new Negocio.PreguntasFrecuentes(GetToken()).ObtenerPorID(cabeceraID);

            ViewBag.Categorias = Negocio.DDL.ListarCategoriaDePreguntas(oToken);

            string _partialViewName = "_ModalABMPreguntasFrecuentes";
            return PartialView(_partialViewName, item);
        }

        [HttpPost]
        public JsonResult SaveModalPreguntasFrecuentes(Entidades.Pregunta_Frecuente obj)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();

            oM = new Negocio.PreguntasFrecuentes(GetToken()).Save(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ActivarDesactivarPregunta(string preguntaID, bool changeStatus)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();

            if (preguntaID.Trim().Length == 0)
            {
                oM.Success = false;
                oM.Message = "No se pudo cambiar el estado de la pregunta";
                return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
            }
            Negocio.PreguntasFrecuentes negocio = new Negocio.PreguntasFrecuentes(GetToken());
            oM = negocio.ActivarDesactivar(Convert.ToInt32(preguntaID), changeStatus);

            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        #region Partials Acordeón

        [HttpGet]
        public ActionResult PartialAccordionDataPreguntas(int categoriaID)
        {
            List<Entidades.Pregunta_Frecuente> lst = new List<Entidades.Pregunta_Frecuente>();

            lst = new Negocio.PreguntasFrecuentes(GetToken()).ListarPorCategoria(categoriaID);

            if (lst.Count > 0)
            {
                string _partialViewName = "_GridDataPreguntasFrecuentes";
                return PartialView(_partialViewName, lst);
            }
            else
            {
                Entidades.App.ObjectMessage oM = new ObjectMessage();
                oM.Message = "No hay preguntas frecuentes para la categoría seleccionada";
                return PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", oM);
            }
        }
        #endregion
    }
}