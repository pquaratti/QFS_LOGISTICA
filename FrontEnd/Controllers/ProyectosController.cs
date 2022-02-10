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
    public class ProyectosController : ControllerBaseV2
    {

        #region Proyectos

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [CustomAttributes.ViewBagData(Incluir = "Areas")]
        public ActionResult PartialModalABM(string cabeceraID, string detalleID, string subdetalleID)
        {
            Entidades.Proyecto item = new Entidades.Proyecto();
            item.proy_id = Convert.ToInt32(cabeceraID);
            
            if (Convert.ToInt32(cabeceraID) > 0)
                item = new Negocio.Proyectos(GetToken()).ObtenerPorID(cabeceraID);
            else
            {
                item.proy_fec_fin = DateTime.Now;
                item.proy_fec_ini = DateTime.Now;
            }

            string _partialViewName = "_ModalABMProyectos";
            return PartialView(_partialViewName, item);
        }

        [HttpGet]
        public ActionResult PartialGridDataProyectos(string tipoProyectoID, string estadoProyecto)
        {
            List<Entidades.Proyecto> lst = new List<Entidades.Proyecto>();
            lst = new Negocio.Proyectos(GetToken()).ListarPorTipoProyectoYEstado(Convert.ToInt32(tipoProyectoID), estadoProyecto);
           
            if (lst.Count > 0)
            {
                string _partialViewName = "_GridDataProyectos";
                return PartialView(_partialViewName, lst);
            }
            else
            {
                Entidades.App.ObjectMessage oM = new ObjectMessage();
                oM.Message = "No hay Proyectos para mostrar";
                return PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", oM);
            }
        }

        [HttpGet]
        [CustomAttributes.ViewBagData(Incluir = "Areas")]
        public ActionResult Configuracion(string id)
        {
            Entidades.App.Token oToken = GetToken();

            Entidades.Proyecto obj = new Negocio.Proyectos(oToken).ObtenerPorID(id);

            return View(obj);
        }

        [HttpGet]
        public ActionResult PartialProyectoFoto(string proyecto)
        {
            Negocio.Proyectos n = new Negocio.Proyectos(GetToken());
            Entidades.Vistas.FotoVista vista = new Entidades.Vistas.FotoVista
            {
                controller = "Proyectos",
                nombre = n.ObtenerNombreFoto(proyecto),
                idEncriptado = proyecto

            };
            string _partialViewName = "~/Views/Shared/_CargaFotoPartialView.cshtml";
            return PartialView(_partialViewName, vista);
   
        }

        [HttpPost]
        public JsonResult Delete(string BorrarID)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            Negocio.Proyectos n = new Negocio.Proyectos(GetToken());
            string usu = Filters.VerificarToken.ConsultarToken().UserID;
            oM = n.DeleteProyecto(Convert.ToInt32(BorrarID));
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DesvincularImagenProyecto(string BorrarID)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            Negocio.Proyectos n = new Negocio.Proyectos(GetToken());
            string usu = Filters.VerificarToken.ConsultarToken().UserID;
            Entidades.Proyecto proy = n.ObtenerPorIDEncriptado(BorrarID);
            string path = Path.Combine(Server.MapPath("~/UploadFiles"), proy.proy_foto);
            oM = n.DesvincularFoto(Negocio.App.Security.DesencriptarID(BorrarID), path);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveModalProyecto(Entidades.Proyecto obj)
        {
            obj.Area.area_id = Negocio.App.Security.GetID(obj.Area.IdEncriptado);

            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            Negocio.Proyectos n = new Negocio.Proyectos(GetToken());
            oM = n.Save(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveProyecto(Entidades.Proyecto obj)
        {
            obj.Area.area_id = Negocio.App.Security.GetID(obj.Area.IdEncriptado);

            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            Negocio.Proyectos n = new Negocio.Proyectos(GetToken());
            string path;
            string nombrefoto = n.ObtenerNombreFoto(obj.IdEncriptado);
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase files = Request.Files[0];
                string extension = Path.GetExtension(files.FileName);
                if (nombrefoto.Length > 0)
                {
                    path = Path.Combine(Server.MapPath("~/UploadFiles"), nombrefoto);
                    oM = new Negocio.Proyectos(GetToken()).BorrarFoto(path);
                }
                string _fileNameUpload = "Proyecto_" + Convert.ToString(obj.proy_id) + extension;
                path = Path.Combine(Server.MapPath("~/UploadFiles"), Path.GetFileName(_fileNameUpload));

                files.SaveAs(path);
                obj.proy_foto = _fileNameUpload;
            }

            oM = n.Save(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Colaboradores 
        [HttpGet]
        public ActionResult PartialContentColaboradores(string proyecto)
        {
            Entidades.App.Token oToken = Filters.VerificarToken.ConsultarToken();
            Negocio.ProyectosColaboradores ne = new Negocio.ProyectosColaboradores(GetToken());
            List<Entidades.Proyecto_Colaborador> lst = new List<Entidades.Proyecto_Colaborador>();
            lst = ne.ListarColaboradoresProyecto(proyecto);
            string _partialViewName = "~/Views/Proyectos/_PartialContentColaboradores.cshtml";
            ViewBag.Proyecto = proyecto;
            return PartialView(_partialViewName, lst);
        }

        public ActionResult PartialModalAsignarColaborador(string cabeceraID, string detalleID, string subdetalleID)
        {
            Entidades.Proyecto_Colaborador item = new Entidades.Proyecto_Colaborador();
            Negocio.Proyectos ne = new Negocio.Proyectos(GetToken());
            string _partialViewName = "_ModalABMAsignarColaborador";
            item.Proyecto = ne.ObtenerPorID(cabeceraID);
            item.Legajo = new Entidades.App.SIS_Usuario();
            return PartialView(_partialViewName, item);
        }

        [HttpPost]
        public JsonResult SaveModalAsignarColaborador(Entidades.Proyecto_Colaborador obj)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            oM = new Negocio.ProyectosColaboradores(GetToken()).Save(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteProyectoColaborador(string BorrarID)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            Negocio.ProyectosColaboradores n = new Negocio.ProyectosColaboradores(GetToken());
            string usu = Filters.VerificarToken.ConsultarToken().UserID;
            oM = n.Delete(Convert.ToInt32(BorrarID));
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Objetivos 
      
        [HttpGet]
        public ActionResult PartialContentObjetivos(string proyecto)
        {
            Entidades.App.Token oToken = Filters.VerificarToken.ConsultarToken();
            List<Entidades.Proyecto_Objetivo> lst = new List<Entidades.Proyecto_Objetivo>();
            var _proyectoID = Convert.ToInt32(Negocio.App.Security.DesencriptarID(proyecto));
            lst = new Negocio.ProyectosObjetivos(oToken).ListarPorProyecto(_proyectoID);
            string _partialViewName = "~/Views/Proyectos/_PartialContentObjetivos.cshtml";
            return PartialView(_partialViewName, lst);
        }

        public ActionResult PartialModalObjetivo(string cabeceraID, string detalleID, string subdetalleID)
        {
            Entidades.App.Token oToken = GetToken();

            var _proyectoID = Convert.ToInt32(Negocio.App.Security.DesencriptarID(cabeceraID));

            Entidades.Proyecto_Objetivo item = new Entidades.Proyecto_Objetivo();

            if (detalleID != "0")
            {
                Negocio.ProyectosObjetivos negocio = new Negocio.ProyectosObjetivos(oToken);
                item = negocio.ObtenerPorIDSimple(detalleID);
            }
            else
            {
                item.pryobj_fec_ini = DateTime.Now;
                item.pryobj_fec_ven = DateTime.Now;
                item.ProyectoVinculado = new Entidades.Proyecto();
                item.ProyectoVinculado.IdEncriptado = cabeceraID;
                item.ProyectoVinculado.proy_id = _proyectoID;
            }

            ViewBag.Prioridades = Negocio.DDL.ListarTipoPrioridades(oToken, true);

            string _partialViewName = "_ModalABMObjetivo";
            return PartialView(_partialViewName, item);
        }

        [HttpPost]
        public JsonResult SaveModalObjetivo(Entidades.Proyecto_Objetivo obj)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            Negocio.ProyectosObjetivos n = new Negocio.ProyectosObjetivos(GetToken());
            
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase files = Request.Files[0];
                string _fileNameUpload = "Objetivo_" + DateTime.Now.ToString("yyyyMMddHHmmss")  + Path.GetExtension(files.FileName);
                string path = Path.Combine(Server.MapPath("~/UploadFiles"), Path.GetFileName(_fileNameUpload));
                files.SaveAs(path);
                obj.pryobj_foto = _fileNameUpload;
            }

            oM = n.Save(obj);

            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteObjetivo(string BorrarID)
        {
            var _id = Convert.ToInt32(Negocio.App.Security.DesencriptarID(BorrarID));
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            Negocio.ProyectosObjetivos n = new Negocio.ProyectosObjetivos(GetToken());
            string usu = Filters.VerificarToken.ConsultarToken().UserID;
            oM = n.Delete(_id);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ImportarFotoObjetivo(FormCollection forms, string pryobj_id)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();

            HttpPostedFileBase files = Request.Files[0];
            string _fileNameUpload = "Objetivo_" + pryobj_id + Path.GetExtension(files.FileName);
            string path = Path.Combine(Server.MapPath("~/UploadFiles"), Path.GetFileName(_fileNameUpload));
            files.SaveAs(path);

            Negocio.ProyectosObjetivos n = new Negocio.ProyectosObjetivos(GetToken());
            oM = n.ImportFoto(_fileNameUpload, pryobj_id);

            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarSelectObjetivos(string filterID)
        {
            Entidades.App.Token oToken = GetToken();
            List<Entidades.App.DLLObject> items = new List<Entidades.App.DLLObject>();

            items = Negocio.DDL.ListarObjetivosPorProyecto(GetToken(), true, Convert.ToInt32(filterID));

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        #endregion  

        #region Indicadores
        public ActionResult PartialContentIndicadores(string proyecto)
        {
            var _proyectoID = Convert.ToInt32(Negocio.App.Security.DesencriptarID(proyecto));
            Entidades.App.Token oToken = Filters.VerificarToken.ConsultarToken();
            Negocio.ProyectosIndicadores negocio = new Negocio.ProyectosIndicadores(GetToken());
            List<Entidades.Proyecto_Indicador> lst = negocio.ListarPorProyecto(_proyectoID);
            string _partialViewName = "_PartialContentIndicadores";
            return PartialView(_partialViewName, lst);
        }

        #endregion

        #region Tipos de Proyectos

        public ActionResult TipoProyecto()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PartialModalABMTipoProyecto(string cabeceraID, string detalleID, string subdetalleID)
        {
            Entidades.Tipo_Proyecto item = new Entidades.Tipo_Proyecto();
            item.tproy_id = Convert.ToInt32(cabeceraID);

            if (Convert.ToInt32(cabeceraID) > 0)
                item = new Negocio.Tipo_Proyectos(GetToken()).ObtenerPorID(cabeceraID);

            string _partialViewName = "_ModalABMTipoProyecto";
            return PartialView(_partialViewName, item);
        }

        [HttpGet]
        public ActionResult PartialGridDataTipoProyecto()
        {
            List<Entidades.Tipo_Proyecto> lst = new List<Entidades.Tipo_Proyecto>();
            lst = new Negocio.Tipo_Proyectos(GetToken()).Listar();

            if (lst.Count > 0)
            {
                string _partialViewName = "_GridDataTipoProyecto";
                return PartialView(_partialViewName, lst);
            }
            else
            {
                Entidades.App.ObjectMessage oM = new ObjectMessage();
                oM.Message = "No hay Tipos de Proyectos para mostrar";
                return PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", oM);
            }
        }


        [HttpPost]
        public JsonResult DeleteTipoProyecto(string BorrarID)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            Negocio.Tipo_Proyectos n = new Negocio.Tipo_Proyectos(GetToken());
            string usu = Filters.VerificarToken.ConsultarToken().UserID;
            oM = n.Delete(Convert.ToInt32(BorrarID));
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveModalTipoProyecto(Entidades.Tipo_Proyecto obj)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();

            oM = new Negocio.Tipo_Proyectos(GetToken()).Save(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Tipo de Proyecto Recursos

        public ActionResult TipoProyectoRecurso()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PartialModalABMTipoProyectoRecurso(string cabeceraID, string detalleID, string subdetalleID)
        {
            Entidades.Tipo_Proyecto_Recurso item = new Entidades.Tipo_Proyecto_Recurso();
            item.tproyrecurso_id = Convert.ToInt32(cabeceraID);

            if (Convert.ToInt32(cabeceraID) > 0)
                item = new Negocio.Tipo_Proyecto_Recursos(GetToken()).ObtenerPorID(cabeceraID);

            string _partialViewName = "_ModalABMTipoProyectoRecurso";
            return PartialView(_partialViewName, item);
        }

        [HttpGet]
        public ActionResult PartialGridDataTipoProyectoRecurso()
        {
            List<Entidades.Tipo_Proyecto_Recurso> lst = new List<Entidades.Tipo_Proyecto_Recurso>();
            lst = new Negocio.Tipo_Proyecto_Recursos(GetToken()).Listar();

            if (lst.Count > 0)
            {
                string _partialViewName = "_GridDataTipoProyectoRecurso";
                return PartialView(_partialViewName, lst);
            }
            else
            {
                Entidades.App.ObjectMessage oM = new ObjectMessage();
                oM.Message = "No hay Tipos de Recursos para mostrar";
                return PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", oM);
            }
        }


        [HttpPost]
        public JsonResult DeleteTipoProyectoRecurso(string BorrarID)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            Negocio.Tipo_Proyecto_Recursos n = new Negocio.Tipo_Proyecto_Recursos(GetToken());
            string usu = Filters.VerificarToken.ConsultarToken().UserID;
            oM = n.Delete(Convert.ToInt32(BorrarID));
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveModalTipoProyectoRecurso(Entidades.Tipo_Proyecto_Recurso obj)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();

            oM = new Negocio.Tipo_Proyecto_Recursos(GetToken()).Save(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        #endregion 

        #region Mis Proyectos

        public ActionResult MisProyectos()
        {
            return View();
        }

        #endregion

        #region Seguimiento 

        public ActionResult Seguimiento(string id)
        {
            Entidades.Vistas.SeguimientoProyecto datos = new Negocio.Proyectos(GetToken()).ConsultarSeguimiento(id);
            return View(datos);
        }

        #endregion

        //#region Asignación de Tareas a Colaboradores


        //public ActionResult AsignacionTareas()
        //{
        //    return View();
        //}

        //[HttpGet]
        //public ActionResult PartialModalABMAsignacionTareas(string cabeceraID, string detalleID, string subdetalleID)
        //{
        //    Entidades.Proyecto_Tarea_Colaborador item = new Entidades.Proyecto_Tarea_Colaborador();
        //    item.Tarea.prytar_id = Convert.ToInt32(detalleID);
        //    item.Proyecto.proy_id = Convert.ToInt32(cabeceraID);
        //    item.Tarea = new Negocio.ProyectosTareas(GetToken()).ObtenerPorID(Convert.ToString(item.Tarea.prytar_id));

        //    string _partialViewName = "_ModalABMAsignacionTareas";
        //    return PartialView(_partialViewName, item);
        //}

        //[HttpGet]
        //public ActionResult PartialGridDataAsignacionTareas(string proyectoID, string objetivoID, string tareaID)
        //{
        //    List<Entidades.Proyecto_Tarea_Colaborador> lst = new List<Entidades.Proyecto_Tarea_Colaborador>();
        //    lst = new Negocio.ProyectosTareasColaboradores(GetToken()).ListarPorTarea(Convert.ToInt32(tareaID));

        //    if (lst.Count > 0)
        //    {
        //        string _partialViewName = "_GridDataAsignacionTareas";
        //        return PartialView(_partialViewName, lst);
        //    }
        //    else
        //    {
        //        Entidades.App.ObjectMessage oM = new ObjectMessage();
        //        oM.Message = "No hay colaboradores para mostrar.";
        //        return PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", oM);
        //    }
        //}


        //[HttpPost]
        //public JsonResult DeleteAsignacionTareas(string BorrarID)
        //{
        //    Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
        //    Negocio.ProyectosTareasColaboradores n = new Negocio.ProyectosTareasColaboradores(GetToken());
        //    string usu = Filters.VerificarToken.ConsultarToken().UserID;
        //    oM = n.Delete(Convert.ToInt32(BorrarID));
        //    return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public JsonResult SaveModalAsignacionTareas(Entidades.Proyecto_Tarea_Colaborador obj)
        //{
        //    Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
        //    oM = new Negocio.ProyectosTareasColaboradores(GetToken()).Save(obj);
        //    return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        //}

        //#endregion

        #region Resumen

        public ActionResult PartialContentResumen(string proyecto)
        {
            Entidades.App.Token oToken = Filters.VerificarToken.ConsultarToken();

            Negocio.Proyectos negocioPROY = new Negocio.Proyectos(oToken);
            //Negocio.ProyectosIndicadores negocioIND = new Negocio.ProyectosIndicadores(oToken);
            //Negocio.ProyectosObjetivos negocioOBJ = new Negocio.ProyectosObjetivos(oToken);
            //Negocio.ProyectosObjetivosIndicadores negocioOBJIND = new Negocio.ProyectosObjetivosIndicadores(oToken);

            Entidades.Proyecto proy = negocioPROY.ObtenerPorIDEncriptado(proyecto);


            string _partialViewName = "_PartialContentResumen";
            return PartialView(_partialViewName, proy);
        }

        [HttpGet]
        public ActionResult PartialDetalleCabeceraProyecto(string proyecto)
        {
            Entidades.App.Token oToken = Filters.VerificarToken.ConsultarToken();

            List<Entidades.Vistas.TarjetaCabecera> lst = new List<Entidades.Vistas.TarjetaCabecera>();
            lst = new Negocio.Proyectos(oToken).ResumenDetalleCabecera(Negocio.App.Security.DesencriptarID(proyecto));
            string _partialViewName = "~/Views/Proyectos/Resumen/_PartialDetalleCabeceraProyecto.cshtml";
            return PartialView(_partialViewName, lst);
        }

        [HttpGet]
        public ActionResult PartialDetalleObjetivosProyecto(string proyecto)
        {
            List<Entidades.Proyecto_Objetivo> lst = new List<Entidades.Proyecto_Objetivo>();
            Entidades.App.Token oToken = Filters.VerificarToken.ConsultarToken();
            lst = new Negocio.Proyectos(oToken).ResumenDetalleObjetivos(Negocio.App.Security.DesencriptarID(proyecto));
            string _partialViewName = "~/Views/Proyectos/Resumen/_PartialResumenObjetivosProyecto.cshtml";
            return PartialView(_partialViewName, lst);
        }

        [HttpPost]
        public JsonResult CerrarProyecto(string accionID)
        {
            string proy_id = Negocio.App.Security.DesencriptarID(accionID);
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            if (new Negocio.ProyectosObjetivosIndicadores(GetToken()).ObjetivosSinIndicadores(proy_id))
            {
                oM.Success = false;
                oM.Message = "Hay Objetivos sin Indicadores asignados.";
                return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
            }
            oM = new Negocio.Proyectos(GetToken()).CerrarProyecto(proy_id);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);


        }


        #endregion

        #region Evolución del Proyecto

        //// Evolución de objetivos//////
        //[HttpGet]
        //public ActionResult PartialEvolucionObjetivosProyecto(string proyecto)
        //{
        //    Entidades.App.Token oToken = Filters.VerificarToken.ConsultarToken();
        //    Negocio.ProyectosObjetivos negocioPRYOBJ = new Negocio.ProyectosObjetivos(oToken);
        //    Negocio.ProyectosObjetivosIndicadores negocioPOI = new Negocio.ProyectosObjetivosIndicadores(oToken);
        //    Negocio.ProyectosIndicadoresTareas negocioPIT = new Negocio.ProyectosIndicadoresTareas(oToken);

        //    string proy_id = Negocio.App.Security.DesencriptarID(proyecto);

        //    List<Entidades.Vistas.Progreso> lst = new List<Entidades.Vistas.Progreso>();
        //    List<Entidades.Proyecto_Objetivo> objetivos = negocioPRYOBJ.ListarPorProyecto(Convert.ToInt32(proy_id));

        //    foreach (Entidades.Proyecto_Objetivo objetivo in objetivos)
        //    {
        //        List<Entidades.Proyecto_Indicador> indicadores = negocioPOI.ListarIndicadoresPorObjetivo(objetivo.pryobj_id);
        //        decimal acumulador = 0;
        //        foreach (Entidades.Proyecto_Indicador indicador in indicadores)
        //        {
        //            acumulador += negocioPIT.CalcularValorActual(indicador);
        //        }
        //        decimal valor_inicial = negocioPOI.CalcularValorInicial(objetivo.pryobj_id);
        //        decimal valor_meta = negocioPOI.CalcularValorMeta(objetivo.pryobj_id);
        //        decimal porcentaje = 0;

        //        if (!valor_meta.Equals(0))
        //            porcentaje = acumulador * 100 / valor_meta;

        //        lst.Add(
        //            new Entidades.Vistas.Progreso
        //            {
        //                porcentaje = Math.Round(porcentaje,2),
        //                titulo = objetivo.pryobj_nombre,
        //                progreso = acumulador,
        //                valor_inicial = valor_inicial,
        //                valor_meta = valor_meta,
        //                url_imagen = objetivo.pryobj_foto,
        //                fecha_fin = objetivo.pryobj_fec_ven.ToString("dddd, dd MMMM yyyy"),
        //                estado = negocioPRYOBJ.EstadoDelObjetivo(objetivo, acumulador, valor_inicial, valor_meta)
        //            });
        //        ViewBag.Progreso = "Objetivos";
        //    }

        //    string _partialViewName = "~/Views/Proyectos/Resumen/_PartialResumenObjetivosProyecto.cshtml";
        //    return PartialView(_partialViewName, lst);
        //}

        #endregion

    }
}