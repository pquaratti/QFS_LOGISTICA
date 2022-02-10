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
    public class TareasController : ControllerBaseV2
    {

        #region Gestion Tareas

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddOrEdit(string id)
        {
            Entidades.Tarea item = new Entidades.Tarea();
            if (id != null)
                item = new Negocio.Tareas(GetToken()).ObtenerPorID(id);
            else
                item = new Negocio.Tareas(GetToken()).ObjetoNuevo();

            Entidades.App.Token oToken = GetToken();

            ViewBag.Areas = new Negocio.App.SIS_Areas(oToken).ListarSimple();

            return View(item);
        }

        [HttpGet]
        public ActionResult PartialGridDataTareas(string indicadorID, string estadoID, string textoBusqueda)
        {
            List<Entidades.Tarea> lst = new List<Entidades.Tarea>();
            lst = new Negocio.Tareas(GetToken()).ListarPorIndicadorEstadoTexto(Convert.ToInt32(indicadorID), Convert.ToInt32(estadoID), textoBusqueda);

            if (lst.Count > 0)
            {
                string _partialViewName = "_GridDataTareas";
                return PartialView(_partialViewName, lst);
            }
            else
            {
                Entidades.App.ObjectMessage oM = new ObjectMessage();
                oM.Message = "No hay Tareas para mostrar";
                return PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", oM);
            }
        }

        [HttpPost]
        public JsonResult Delete(string BorrarID)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            Negocio.Tareas n = new Negocio.Tareas(GetToken());
            string usu = Filters.VerificarToken.ConsultarToken().UserID;
            oM = n.Delete(Convert.ToInt32(BorrarID));
            if (oM.Success)
                oM = new Negocio.TareasIndicadores(GetToken()).DeletePorTarea(BorrarID);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save(Entidades.Tarea data)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            var esNuevo = false;

            try
            {
                if (Convert.ToInt32(Negocio.App.Security.DesencriptarID(data.IdEncriptado)).Equals(0))
                {
                    esNuevo = true;
                }

                Negocio.Tareas negocio = new Negocio.Tareas(GetToken());
                oM = negocio.Save(data);

                if (oM.Success)
                {
                    oM.RedirectNew = esNuevo;
                    oM.urlRedirect = Url.Action("AddOrEdit", "Tareas", new { id = data.IdEncriptado });
                }

            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.Message = ex.Message;
            }

            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PublicarTarea(string accionID)
        {
            string tar_id = Negocio.App.Security.DesencriptarID(accionID);
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            oM = new Negocio.Tareas(GetToken()).PublicarTarea(tar_id);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult PartialModalRealizarAvance(string cabeceraID, string detalleID, string auxID)
        {
            Entidades.Tarea item = new Entidades.Tarea();
            if (cabeceraID != null)
                item = new Negocio.Tareas(GetToken()).ObtenerPorID(cabeceraID);
            string _partialViewName = "_ModalAvanceTarea";
            return PartialView(_partialViewName, item);
        }

        [HttpPost]
        public JsonResult SaveModalCambiarPorcentaje(Entidades.Tarea obj)
        {
            obj.tar_id = Convert.ToInt32(Negocio.App.Security.DesencriptarID(obj.IdEncriptado));
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            oM = new Negocio.Tareas(GetToken()).RealizarAvance(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Resumen de Tarea

        [HttpGet]
        public ActionResult ResumenTareas(string id)
        {
            Entidades.Tarea item = new Entidades.Tarea();
            if (id != null)
                item = new Negocio.Tareas(GetToken()).ObtenerPorID(id);
            else
            {
                item = new Negocio.Tareas(GetToken()).ObjetoNuevo();
            }
            return View(item);
        }

        [HttpGet]
        public ActionResult PartialContentDatosTarea(string tareaID)
        {
            Entidades.Tarea tarea = new Negocio.Tareas(GetToken()).ObtenerPorIDEncriptado(tareaID);
            string _partialViewName = "_DatosTareas";
            return PartialView(_partialViewName, tarea);
        }


        #endregion

        #region Vincular Indicador

        [HttpGet]
        public ActionResult PartialModalABMVincularIndicador(string cabeceraID, string detalleID, string auxID)
        {
            Entidades.Tarea_Indicador item = new Entidades.Tarea_Indicador();
            if (cabeceraID != null)
                item.Tarea.IdEncriptado = cabeceraID;
            if(detalleID!="0")
                item.Indicador.IdEncriptado = detalleID;

            string _partialViewName = "_ModalABMTareaIndicadores";
            return PartialView(_partialViewName, item);
        }

        [HttpGet]
        public ActionResult PartialGridDataIndicadoresTarea(string tareaID)
        {
            List<Entidades.Tarea_Indicador> lst = new List<Entidades.Tarea_Indicador>();
            string tar_id = Negocio.App.Security.DesencriptarID(tareaID);
            lst = new Negocio.TareasIndicadores(GetToken()).ListarIndicadoresPorTarea(Convert.ToInt32(tar_id));

            if (lst.Count > 0)
            {
                string _partialViewName = "_GridDataIndicadores";
                return PartialView(_partialViewName, lst);
            }
            else
            {
                Entidades.App.ObjectMessage oM = new ObjectMessage();
                oM.Message = "No hay indicadores asignados.";
                return PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", oM);
            }
        }

        [HttpPost]
        public JsonResult SaveModalTareaIndicador(Entidades.Tarea_Indicador obj)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            obj.Tarea.tar_id = Convert.ToInt32(Negocio.App.Security.DesencriptarID(obj.Tarea.IdEncriptado));
            oM = new Negocio.TareasIndicadores(GetToken()).Save(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DesvincularIndicador(string BorrarID)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            Negocio.TareasIndicadores n = new Negocio.TareasIndicadores(GetToken());
            oM = n.Delete(Convert.ToInt32(BorrarID));
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        #endregion
     
    }
}