using Entidades.App;
using Newtonsoft.Json;
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
    public class ColaboradoresController : ControllerBaseV2
    {

        #region Asignación de Tareas a Colaboradores


        public ActionResult AsignacionTareas()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PartialModalABMAsignacionTareas(string cabeceraID, string detalleID, string subdetalleID)
        {
            Entidades.Tarea_Colaborador item = new Entidades.Tarea_Colaborador();
            item.Tarea.tar_id = Convert.ToInt32(detalleID);
            item.Tarea = new Negocio.Tareas(GetToken()).ObtenerPorID(Convert.ToString(item.Tarea.tar_id));

            string _partialViewName = "_ModalABMAsignacionTareas";
            return PartialView(_partialViewName, item);
        }

        [HttpGet]
        public ActionResult PartialGridDataAsignacionTareas(string proyectoID, string objetivoID, string tareaID)
        {
            List<Entidades.Tarea_Colaborador> lst = new List<Entidades.Tarea_Colaborador>();
          //  lst = new Negocio.TareasColaboradores(GetToken()).ListarPorTarea(Convert.ToInt32(tareaID));

            if (lst.Count > 0)
            {
                string _partialViewName = "_GridDataAsignacionTareas";
                return PartialView(_partialViewName, lst);
            }
            else
            {
                Entidades.App.ObjectMessage oM = new ObjectMessage();
                oM.Message = "No hay colaboradores para mostrar.";
                return PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", oM);
            }
        }


        //[HttpPost]
        //public JsonResult DeleteAsignacionTareas(string BorrarID)
        //{
        //    Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
        //    Negocio.TareasColaboradores n = new Negocio.TareasColaboradores(GetToken());
        //    string usu = Filters.VerificarToken.ConsultarToken().UserID;
        //    oM = n.Delete(Convert.ToInt32(BorrarID));
        //    return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public JsonResult SaveModalAsignacionTareas(Entidades.Tarea_Colaborador obj)
        //{
        //    Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
        //    oM = new Negocio.TareasColaboradores(GetToken()).Save(obj);
        //    return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        //}

        #endregion

        #region Evolucion

        public ActionResult Evolucion()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PartialModalColaboradorCategoria(string cabeceraID, string detalleID, string auxID)
        {
            Entidades.App.SIS_Usuario item = new Entidades.App.SIS_Usuario();
            if (cabeceraID != "0")
                item = new Negocio.App.SIS_Usuarios(GetToken()).ObtenerPorID(cabeceraID);
            string _partialViewName = "_ModalColaboradorCategoria";
            return PartialView(_partialViewName, item);
        }

        [HttpGet]
        public ActionResult PartialModalColaboradorSkills(string cabeceraID, string detalleID, string auxID)
        {
            Entidades.Colaborador_Skill item = new Negocio.ColaboradoresSkills(GetToken()).ObjetoNuevo();
            if (cabeceraID != "0")
                item.Colaborador = new Negocio.App.SIS_Usuarios(GetToken()).ObtenerPorID(cabeceraID);
            string _partialViewName = "_ModalColaboradorSkills";
            return PartialView(_partialViewName, item);
        }

        [HttpPost]
        public JsonResult SaveModalColaboradorCategoria(Entidades.App.SIS_Usuario obj, string txtExtra)
        {
            obj.usu_id = Convert.ToInt32(Negocio.App.Security.DesencriptarID(obj.IdEncriptado));
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            oM = new Negocio.App.SIS_Usuarios(GetToken()).ActualizarCategoria(obj, txtExtra);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveModalColaboradorSkill(Entidades.Colaborador_Skill obj, string txtExtra)
        {
            obj.colskill_id = Convert.ToInt32(Negocio.App.Security.DesencriptarID(obj.IdEncriptado));
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            oM = new Negocio.ColaboradoresSkills(GetToken()).Save(obj, txtExtra);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult PartialModalEvolucionCategoria(string cabeceraID, string detalleID, string auxID)
        {
            Entidades.App.SIS_Usuario usu = new Entidades.App.SIS_Usuario();
            List<Entidades.Categoria_Colaborador_Evolucion> lst = new List<Entidades.Categoria_Colaborador_Evolucion>();
            Negocio.CategoriasColaboradoresEvolucion negocio = new Negocio.CategoriasColaboradoresEvolucion(GetToken());
            if (cabeceraID != "0")
            {
                usu = new Negocio.App.SIS_Usuarios(GetToken()).ObtenerPorID(cabeceraID);
                lst = negocio.ListarPorColaborador(Convert.ToString(usu.usu_id));
            }
            ViewBag.Colaborador = usu.usu_nombre + " " + usu.usu_apellido;
            string _partialViewName = "_ModalEvolucionCategoria";
            return PartialView(_partialViewName, lst);
        }

        public ActionResult PartialModalEvolucionSkills(string cabeceraID, string detalleID, string auxID)
        {
            Entidades.App.SIS_Usuario usu = new Entidades.App.SIS_Usuario();
            List<Entidades.Colaborador_Skill_Evolucion> lst = new List<Entidades.Colaborador_Skill_Evolucion>();
            Negocio.ColaboradoresSkillsEvolucion negocio = new Negocio.ColaboradoresSkillsEvolucion(GetToken());
            if (cabeceraID != "0")
            {
                usu = new Negocio.App.SIS_Usuarios(GetToken()).ObtenerPorID(cabeceraID);
                lst = negocio.ListarPorColaborador(Convert.ToString(usu.usu_id));
            }
            ViewBag.Colaborador = usu.usu_nombre + " " + usu.usu_apellido;
            string _partialViewName = "_ModalEvolucionSkills";
            return PartialView(_partialViewName, lst);
        }


        [HttpGet]
        public ActionResult PartialGridDataColaboradores(string categoriaID, string skillID, string textoBusqueda)
        {
            List<Entidades.App.SIS_Usuario> lst = new List<Entidades.App.SIS_Usuario>();
            lst = new Negocio.App.SIS_Usuarios(GetToken()).ListarPorSkillCategoriaTexto(Convert.ToInt32(categoriaID), Convert.ToInt32(skillID), textoBusqueda);

            if (lst.Count > 0)
            {
                string _partialViewName = "_GridDataColaboradores";
                return PartialView(_partialViewName, lst);
            }
            else
            {
                Entidades.App.ObjectMessage oM = new ObjectMessage();
                oM.Message = "No hay Colaboradores para mostrar";
                return PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", oM);
            }
        }

        [HttpPost]
        public JsonResult PuntajeSkill(string skillID, string usuarioID)
        {
            string usu_id = Negocio.App.Security.DesencriptarID(usuarioID);
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            oM = new Negocio.ColaboradoresSkills(GetToken()).PuntajeActual(skillID, usu_id);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Administracion

        public ActionResult Administracion()
        {
            var token = GetToken();

            Negocio.App.SIS_Areas negocioAreas = new Negocio.App.SIS_Areas(token);

            List<Entidades.App.SIS_Area> lstAreas = negocioAreas.ListarSimple();
            lstAreas.Add(new SIS_Area() { area_id = 0, IdEncriptado = Negocio.App.Security.EncriptarID("0"), descripcion_combo = "Sin asignar" });

            ViewBag.Areas = lstAreas.OrderBy(o=>o.area_id);

            return View();
        }

        [HttpGet]
        public ActionResult PartialGridDataColaboradoresAdministracion(string areaID, string textoBusqueda)
        {
            var _areaID = Negocio.App.Security.GetID(areaID);

            List<Entidades.App.SIS_Usuario> lst = new List<Entidades.App.SIS_Usuario>();
            lst = new Negocio.App.SIS_Usuarios(GetToken()).ListarPorArea(_areaID, textoBusqueda);

            if (lst.Count > 0)
            {
                string _partialViewName = "_GridDataColaboradoresAdministracion";
                return PartialView(_partialViewName, lst);
            }
            else
            {
                Entidades.App.ObjectMessage oM = new ObjectMessage();
                oM.Message = "No hay Colaboradores para mostrar";
                return PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", oM);
            }
        }

        [HttpGet]
        public ActionResult PartialModalActualizarDatos(string cabeceraID, string detalleID, string auxID)
        {
            Entidades.App.SIS_Usuario item = new Entidades.App.SIS_Usuario();
            item = new Negocio.App.SIS_Usuarios(GetToken()).ObtenerPorID(cabeceraID);

            //Combo de Areas
            List<Entidades.App.SIS_Area> lstAreas = new Negocio.App.SIS_Areas(GetToken()).ListarSimple();
            lstAreas.Add(new SIS_Area() { area_id = 0, IdEncriptado = Negocio.App.Security.EncriptarID("0"), descripcion_combo = "Sin asignar" });
            ViewBag.Areas = lstAreas.OrderBy(o => o.area_id);

            string _partialViewName = "_ModalABM";
            return PartialView(_partialViewName, item);
        }

        [HttpPost]
        public JsonResult SaveModalColaborador(Entidades.App.SIS_Usuario obj)
        {
            obj.Area.area_id = Negocio.App.Security.GetID(obj.Area.IdEncriptado);
            obj.usu_id = Negocio.App.Security.GetID(obj.IdEncriptado);
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            oM = new Negocio.App.SIS_Usuarios(GetToken()).SaveModalColaborador(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}