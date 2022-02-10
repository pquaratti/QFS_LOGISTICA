using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrontEnd.Controllers
{
    public class MesaAyudaController : ControllerBaseV2
    {
        public ActionResult Administrador()
        {
            ViewBag.EstadoSolicitudes = Negocio.DDL.ListarEstadosRegistrosDefault();
            return View();
        }

        public ActionResult Index()
        {
            ViewBag.EstadoSolicitudes = Negocio.DDL.ListarEstadosRegistrosDefault();
            return View();
        }

        [HttpGet]
        public ActionResult PartialModalABM(string cabeceraID, string detalleID, string subdetalleID)
        {
            Entidades.Mesa_Ayuda item = new Entidades.Mesa_Ayuda();
            item.mesa_id = Convert.ToInt32(cabeceraID);
            string _partialViewName = "_ModalABM";
            return PartialView(_partialViewName, item);
        }

        [HttpPost]
        public JsonResult SaveModal(Entidades.Mesa_Ayuda obj)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            oM = new Negocio.MesaAyuda(GetToken()).Save(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Delete(string BorrarID)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            Negocio.MesaAyuda n = new Negocio.MesaAyuda(GetToken());
            string usu = Filters.VerificarToken.ConsultarToken().UserID;
            oM = n.Delete(Convert.ToInt32(BorrarID));
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PartialGridData()
        {
            Entidades.App.Token oToken = GetToken();
            List<Entidades.Mesa_Ayuda> lst = new List<Entidades.Mesa_Ayuda>();
            lst = new Negocio.MesaAyuda(oToken).ListarPorUsuarioSolicitante(Convert.ToInt32(oToken.UserID));
            string _partialViewName = "_GridData";
            return PartialView(_partialViewName, lst);
        }

        [HttpGet]
        public ActionResult PartialGridDataAdministrador(int estadoID)
        {
            Entidades.App.Token oToken = GetToken();
            List<Entidades.Mesa_Ayuda> lst = new List<Entidades.Mesa_Ayuda>();
            lst = new Negocio.MesaAyuda(oToken).ListarPorUsuarioResponsable(Convert.ToInt32(0), estadoID);
            string _partialViewName = "_GridDataAdministrador";
            return PartialView(_partialViewName, lst);
        }

        [HttpGet]
        public ActionResult PartialModalRegistrarSolucion(string cabeceraID, string detalleID, string subdetalleID)
        {
            Entidades.Mesa_Ayuda item = new Entidades.Mesa_Ayuda();
            item.mesa_id = Convert.ToInt32(cabeceraID);
            string _partialViewName = "_ModalSolucion";
            return PartialView(_partialViewName, item);
        }

        [HttpPost]
        public JsonResult SaveModalSolucion(Entidades.Mesa_Ayuda obj)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            oM = new Negocio.MesaAyuda(GetToken()).CerrarAyuda(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        #region Interaccion

        [HttpGet]
        public ActionResult PartialModalInteraccion(string cabeceraID, string detalleID, string subdetalleID)
        {
            Entidades.Mesa_Ayuda item = new Entidades.Mesa_Ayuda();
            item.mesa_id = Convert.ToInt32(cabeceraID);

            string _partialViewName = "_ModalInteraccion";
            return PartialView(_partialViewName, item);
        }

        [HttpGet]
        public ActionResult MostrarInteraccion(string mesaID)
        {
            List<Entidades.Mesa_Ayuda_Interaccion> lstMensajes = new List<Entidades.Mesa_Ayuda_Interaccion>();

            Negocio.MesaAyuda negocio = new Negocio.MesaAyuda(GetToken());

            lstMensajes = negocio.ListarInteracciones(Convert.ToInt32(mesaID));

            string _partialViewName = "_ListaDeMensajes";
            return PartialView(_partialViewName, lstMensajes);
        }

        [HttpPost]
        public JsonResult EnviarInteraccion(string mesaID, string mensaje)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            
            try
            {
                Negocio.MesaAyuda negocio = new Negocio.MesaAyuda(GetToken());

                Entidades.Mesa_Ayuda_Interaccion unMensaje = new Entidades.Mesa_Ayuda_Interaccion();
                unMensaje.mesainteraccion_mensaje = mensaje;
                unMensaje.DatosAyuda = new Entidades.Mesa_Ayuda() { mesa_id = Convert.ToInt32(mesaID) };
                unMensaje.mesainteraccion_fecha = DateTime.Now;
                unMensaje.UsuarioInteraccion = new Entidades.App.SIS_Usuario() { usu_id = Convert.ToInt32(GetToken().UserID) };
                oM = negocio.GrabarInteraccion(unMensaje);
            }
            catch (Exception ex)
            {
                oM.Message = ex.Message;
                oM.Success = false;
            }

            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        #endregion


    }
}