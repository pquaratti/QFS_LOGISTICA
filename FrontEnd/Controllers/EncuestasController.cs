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
    public class EncuestasController : ControllerBaseV2
    {

        #region Gestión Encuestas

        public ActionResult Gestion()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            return View("AddOrEdit", new Negocio.Encuestas(GetToken()).ObtenerPorID(id));
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View("AddOrEdit", new Negocio.Encuestas(GetToken()).ObjetoNuevo());
        }

        [HttpGet]
        public ActionResult PartialGridDataEncuestas(string tipoEncuestaID, string areaID)
        {
            List<Entidades.Encuesta> lst = new List<Entidades.Encuesta>();
            lst = new Negocio.Encuestas(GetToken()).ListarPorTipoEncuestaArea(
                Convert.ToInt32(tipoEncuestaID),
                Convert.ToInt32(areaID)
                );

            if (lst.Count > 0)
            {
                string _partialViewName = "_GridDataEncuestas";
                return PartialView(_partialViewName, lst);
            }
            else
            {
                Entidades.App.ObjectMessage oM = new ObjectMessage();
                oM.Message = "No hay Encuestas para mostrar";
                return PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", oM);
            }
        }

        [HttpPost]
        public JsonResult Delete(string BorrarID)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            Negocio.Encuestas n = new Negocio.Encuestas(GetToken());
            string usu = Filters.VerificarToken.ConsultarToken().UserID;
            oM = n.DeleteLogico(BorrarID);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save(Entidades.Encuesta data)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            var esNuevo = false;

            try
            {
                if (Convert.ToInt32(Negocio.App.Security.DesencriptarID(data.IdEncriptado)).Equals(0))
                {
                    esNuevo = true;
                }

                Negocio.Encuestas negocio = new Negocio.Encuestas(GetToken());
                oM = negocio.Save(data);

                if (oM.Success)
                {
                    oM.RedirectNew = esNuevo;
                    oM.urlRedirect = Url.Action("Edit", "Encuestas", new { id = oM.ObjectRelation });
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
        public JsonResult CerrarEncuesta(string accionID)
        {
            string enc_id = Negocio.App.Security.DesencriptarID(accionID);
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            oM = new Negocio.Encuestas(GetToken()).CerrarEncuesta(enc_id);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Preguntas

        [HttpGet]
        public ActionResult PartialModalPreguntasVista(string cabeceraID, string detalleID, string subdetalleID)
        {
            List<Entidades.Encuesta_Pregunta> lst = new List<Entidades.Encuesta_Pregunta>();
            string encuesta = Negocio.App.Security.DesencriptarID(cabeceraID);
            lst = new Negocio.EncuestasPreguntas(GetToken()).ListarRenglonesPorEncuesta(Convert.ToInt32(encuesta));

            if (lst.Count > 0)
            {
                string _partialViewName = "_ModalPreguntasVista";
                return PartialView(_partialViewName, lst);
            }
            else
            {
                Entidades.App.ObjectMessage oM = new ObjectMessage();
                oM.Message = "No hay Preguntas";
                return PartialView("~/Views/Shared/_ModalMensajeSinResultados.cshtml", oM);
            }
        }

        [HttpGet]
        public ActionResult PartialGridDataPreguntas(string encuestaID)
        {
            List<Entidades.Encuesta_Pregunta> lst = new List<Entidades.Encuesta_Pregunta>();
            string encuesta = Negocio.App.Security.DesencriptarID(encuestaID);

            lst = new Negocio.EncuestasPreguntas(GetToken()).ListarRenglonesPorEncuesta(Convert.ToInt32(encuesta));

            if (lst.Count > 0)
            {
                string _partialViewName = "_GridDataPreguntas";
                return PartialView(_partialViewName, lst);
            }
            else
            {
                Entidades.App.ObjectMessage oM = new ObjectMessage();
                oM.Message = "El encuesta no tiene preguntas.";
                return PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", oM);
            }
        }

        [HttpGet]
        public ActionResult PartialModalABMEncuestaPregunta(string cabeceraID, string detalleID, string auxID)
        {
            Entidades.Encuesta_Pregunta item = new Entidades.Encuesta_Pregunta();
            item = new Negocio.EncuestasPreguntas(GetToken()).ObtenerPorID(detalleID);
            item.Encuesta = new Negocio.Encuestas(GetToken()).ObtenerPorID(cabeceraID);
            string _partialViewName = "_ModalABMEncuestaPreguntas";
            return PartialView(_partialViewName, item);
        }

        [HttpPost]
        public JsonResult DeletePregunta(string BorrarID)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            Negocio.EncuestasPreguntas n = new Negocio.EncuestasPreguntas(GetToken());
            string usu = Filters.VerificarToken.ConsultarToken().UserID;
            oM = n.Delete(Convert.ToInt32(BorrarID));
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveModalEncuestaPregunta(Entidades.Encuesta_Pregunta obj)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            oM = new Negocio.EncuestasPreguntas(GetToken()).Save(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Respuestas

        [HttpGet]
        public ActionResult PartialGridDataRespuestas(string encuestaID)
        {
            List<Entidades.Encuesta_Respuesta> lst = new List<Entidades.Encuesta_Respuesta>();
            string encuesta = Negocio.App.Security.DesencriptarID(encuestaID);
            lst = new Negocio.EncuestasRespuestas(GetToken()).ListarRenglonesPorEncuesta(Convert.ToInt32(encuesta));

            if (lst.Count > 0)
            {
                string _partialViewName = "_GridDataRespuestas";
                return PartialView(_partialViewName, lst);
            }
            else
            {
                Entidades.App.ObjectMessage oM = new ObjectMessage();
                oM.Message = "El encuesta no tiene respuestas.";
                return PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", oM);
            }
        }

        [HttpGet]
        public ActionResult PartialModalABMEncuestaRespuesta(string cabeceraID, string detalleID, string auxID)
        {
            Entidades.Encuesta_Respuesta item = new Entidades.Encuesta_Respuesta();
            item = new Negocio.EncuestasRespuestas(GetToken()).ObtenerPorID(detalleID);
            item.Encuesta = new Negocio.Encuestas(GetToken()).ObtenerPorID(cabeceraID);
            string _partialViewName = "_ModalABMEncuestaRespuestas";
            return PartialView(_partialViewName, item);
        }

        [HttpPost]
        public JsonResult SaveModalEncuestaRespuesta(Entidades.Encuesta_Respuesta obj)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            oM = new Negocio.EncuestasRespuestas(GetToken()).Save(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteRespuesta(string BorrarID)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            Negocio.EncuestasRespuestas n = new Negocio.EncuestasRespuestas(GetToken());
            string usu = Filters.VerificarToken.ConsultarToken().UserID;
            oM = n.Delete(Convert.ToInt32(BorrarID));
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PartialModalRespuestasVista(string cabeceraID, string detalleID, string subdetalleID)
        {
            List<Entidades.Encuesta_Respuesta> lst = new List<Entidades.Encuesta_Respuesta>();
            string encuesta = Negocio.App.Security.DesencriptarID(cabeceraID);
            lst = new Negocio.EncuestasRespuestas(GetToken()).ListarRenglonesPorEncuesta(Convert.ToInt32(encuesta));

            if (lst.Count > 0)
            {
                string _partialViewName = "_ModalRespuestasVista";
                return PartialView(_partialViewName, lst);
            }
            else
            {
                Entidades.App.ObjectMessage oM = new ObjectMessage();
                oM.Message = "No hay Respuestas";
                return PartialView("~/Views/Shared/_ModalMensajeSinResultados.cshtml", oM);
            }
        }
        #endregion

        #region Tipo de Encuestas

        public ActionResult Tipos()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PartialModalABMTipoEncuesta(string cabeceraID, string detalleID, string subdetalleID)
        {
            Entidades.Tipo_Encuesta item = new Entidades.Tipo_Encuesta();
            item.tenc_id = Convert.ToInt32(cabeceraID);

            if (Convert.ToInt32(cabeceraID) > 0)
                item = new Negocio.Tipo_Encuestas(GetToken()).ObtenerPorID(cabeceraID);

            string _partialViewName = "_ModalABMTipoEncuesta";
            return PartialView(_partialViewName, item);
        }

        [HttpGet]
        public ActionResult PartialGridDataTipoEncuesta()
        {
            List<Entidades.Tipo_Encuesta> lst = new List<Entidades.Tipo_Encuesta>();
            lst = new Negocio.Tipo_Encuestas(GetToken()).Listar();

            if (lst.Count > 0)
            {
                string _partialViewName = "_GridDataTipoEncuesta";
                return PartialView(_partialViewName, lst);
            }
            else
            {
                Entidades.App.ObjectMessage oM = new ObjectMessage();
                oM.Message = "No hay Tipos de Encuestas para mostrar";
                return PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", oM);
            }
        }


        [HttpPost]
        public JsonResult DeleteTipoEncuesta(string BorrarID)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            Negocio.Tipo_Encuestas n = new Negocio.Tipo_Encuestas(GetToken());
            string usu = Filters.VerificarToken.ConsultarToken().UserID;
            oM = n.Delete(Convert.ToInt32(BorrarID));
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveModalTipoEncuesta(Entidades.Tipo_Encuesta obj)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();

            oM = new Negocio.Tipo_Encuestas(GetToken()).Save(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Participación

        public ActionResult Consulta()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PartialDataConsultaEncuestas(string estado, string txtBusqueda = "")
        {
            List<Entidades.Encuesta_Usuario> lst = new List<Entidades.Encuesta_Usuario>();

            lst = new Negocio.Encuestas(GetToken()).ListarEncuestasParaUsuarios(
                Convert.ToInt32(estado),
                txtBusqueda
                );

            if (lst.Count > 0)
            {
                string _partialViewName = "_DataConsultaEncuestas";
                return PartialView(_partialViewName, lst);
            }
            else
            {
                Entidades.App.ObjectMessage oM = new ObjectMessage();
                oM.Message = "No hay encuestas para mostrar.";
                return PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", oM);
            }
        }


        [HttpGet]
        public ActionResult PartialModalIniciarEncuesta(string cabeceraID, string detalleID, string subdetalleID)
        {
            Entidades.Encuesta encuesta = new Negocio.Encuestas(GetToken()).ObtenerPorID(cabeceraID);
            string _partialViewName = "_ModalIniciarEncuesta";
            return PartialView(_partialViewName, encuesta);
        }

        [HttpGet]
        public ActionResult Responder(string id)
        {
            Negocio.Encuestas negocioEncuesta = new Negocio.Encuestas(GetToken());
            Entidades.Vistas.EncuestaVista encuestaVista = new Entidades.Vistas.EncuestaVista();
            Entidades.Encuesta encuesta = negocioEncuesta.ObtenerPorID(id);
            encuestaVista.Intento = negocioEncuesta.InicializarIntento(encuesta);
            encuestaVista.Preguntas = negocioEncuesta.ListarPreguntas(encuesta);
            encuestaVista.Respuestas = negocioEncuesta.ListarRespuestas(encuesta);

            return View("Responder", encuestaVista);
        }

        [HttpPost]
        public JsonResult SaveRespuestasIntento(List<string> respuestas, int totalPreguntas, string intento)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();

            try
            {
                Negocio.EncuestasUsuarios neg = new Negocio.EncuestasUsuarios(GetToken());

                neg.GuardarIntento(respuestas, totalPreguntas, intento);

                oM.Success = true;
                oM.Message = "OK";
                oM.RedirectNew = true;
                oM.urlRedirect = Url.Action("Finalizado", "Encuestas", new { id = intento });
            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.Message = ex.Message;
            }

            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Finalizado(string id)
        {
            Entidades.Encuesta_Usuario intento = new Negocio.EncuestasUsuarios(GetToken()).ObtenerPorID(id);
            return View(intento);
        }

        public ActionResult VerFinalizado(string id)
        {
            Entidades.Encuesta_Usuario intento = new Negocio.EncuestasUsuarios(GetToken()).ObtenerPorEncuestaUsuario(
                new Negocio.Encuestas(GetToken()).ObtenerPorID(id),GetToken().UserID);
            return View("Finalizado",intento);
        }
        [HttpGet]
        public ActionResult PartialVerEncuesta(string intentoID)
        {
            Negocio.EncuestasUsuarios negocioEncuesta = new Negocio.EncuestasUsuarios(GetToken());
            List<Entidades.Encuesta_Usuario_Respuesta> encuestaVista = negocioEncuesta.ListarSeleccionesPorIntento(
                Negocio.App.Security.DesencriptarID(intentoID));
            return PartialView("_VistaEncuestaFinalizada", encuestaVista);
        }

        public ActionResult Resultado(string id)
        {
            return View(new Negocio.Encuestas(GetToken()).ResultadoEncuesta(id));
        }


        #endregion

        #region Informe de Encuesta

        [HttpGet]
        public ActionResult PartialResultadoCantidades(string usuActual, string usuProgre, string usuFin, string tiempoPromedio)
        {
            List<Entidades.Vistas.TarjetaCabecera> lst = new List<Entidades.Vistas.TarjetaCabecera>();
            lst = new Negocio.Encuestas(GetToken()).ArmarCabeceraInforme(
                usuActual, usuProgre, usuFin, tiempoPromedio
                );
            string _partialViewName = "Informe/_PartialDetalleCabeceraEncuesta";
            return PartialView(_partialViewName, lst);

        }

        [HttpGet]
        public ActionResult PartialResultadoPreguntasCantidades(string encuestaID)
        {
            Negocio.Encuestas negocioEncuesta = new Negocio.Encuestas(GetToken());
            List<Entidades.Vistas.PreguntaCantidad> lst = negocioEncuesta.ListarPreguntasCantidadRespuestas(
                negocioEncuesta.ObtenerPorID(encuestaID));
            ViewBag.Badge = "primary";
            ViewBag.Titulo = "Respuestas por Pregunta";
            ViewBag.Campo = "Cantidad";
            string _partialViewName = "Informe/_GridDataPreguntaValor";
            return PartialView(_partialViewName, lst);

        }

        [HttpGet]
        public ActionResult PartialResultadoPeores(string encuestaID)
        {
            Negocio.Encuestas negocioEncuesta = new Negocio.Encuestas(GetToken());
            List<Entidades.Vistas.PreguntaCantidad> lst = negocioEncuesta.ListarPreguntasPorResultado(
                negocioEncuesta.ObtenerPorID(encuestaID)).OrderBy(x=>x.Cantidad).ToList();
            ViewBag.Badge = "danger";
            ViewBag.Titulo = "Peores Puntajes";
            ViewBag.Campo = "Puntaje";
            string _partialViewName = "Informe/_GridDataPreguntaValor";
            return PartialView(_partialViewName, lst);
        }

        [HttpGet]
        public ActionResult PartialResultadoMejores(string encuestaID)
        {
            Negocio.Encuestas negocioEncuesta = new Negocio.Encuestas(GetToken());
            List<Entidades.Vistas.PreguntaCantidad> lst = negocioEncuesta.ListarPreguntasPorResultado(
                negocioEncuesta.ObtenerPorID(encuestaID)).OrderByDescending(x => x.Cantidad).ToList();
            ViewBag.Badge = "success";
            ViewBag.Titulo = "Mejores Puntajes";
            ViewBag.Campo = "Puntaje";
            string _partialViewName = "Informe/_GridDataPreguntaValor";
            return PartialView(_partialViewName, lst);
        }


        #endregion
    }
}