using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hospitales.Controllers
{
    [Filters.VerificarToken]
    public class HomeController : Controller
    {
        public ActionResult Modules()
        {
            try
            {
                Entidades.App.Token oToken = Filters.VerificarToken.ConsultarToken();

                Negocio.App.SIS_Usuarios negocio = new Negocio.App.SIS_Usuarios(oToken);

                Entidades.App.SIS_Sistema obj = new Entidades.App.SIS_Sistema();
                obj.Modulos = negocio.ListarModulos(Convert.ToInt32(oToken.UserID));
                obj.UsuarioActual = negocio.ObtenerPorID(oToken.UserID);

                ViewBag.Token = oToken;

                return View(obj);
            }
            catch (Exception ex)
            {
                return RedirectToRoute("CerrarSesion");
                throw;
            }
          
        }

        public ActionResult Perfil()
        {
            Entidades.App.Token oToken = Filters.VerificarToken.ConsultarToken();
            Negocio.App.SIS_Usuarios negocio = new Negocio.App.SIS_Usuarios(oToken);

            Entidades.App.SIS_Usuario obj = negocio.ObtenerPorID(oToken.UserID);

            return View(obj);
        }

        [HttpPost]
        public JsonResult RedirectToModule(string moduloID)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            Entidades.App.Token oToken = Filters.VerificarToken.ConsultarToken();
            Negocio.App.SIS_Usuarios negocioUSU = new Negocio.App.SIS_Usuarios();

            var _moduloID = Negocio.App.Security.DesencriptarID(moduloID);

            try
            {
                Filters.VerificarToken.ChangeTokenModuleID(_moduloID);
                //Session["Acciones"] = negocioUSU.ListarAcciones(Convert.ToInt32(oToken.UserID), _moduloID);
                oM.Success = true;
                oM.TokenExist = true;
                oM.RedirectNew = true;
                oM.urlRedirect = Url.Action("Dashboard", "Home");
            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.TokenExist = false;
                oM.Message = ex.Message;
            }

            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [Filters.VerificarModule]
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult ChoosenDropDown()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PartialModalChangeConfig(string cabeceraID, string detalleID, string subdetalleID)
        {
            string _partialViewName = "_ModalChangeConfig";
            return PartialView(_partialViewName);
        }

        [HttpGet]
        public ActionResult PartialGestionIndicadoresPrincipales()
        {
            Entidades.App.Token oToken = Filters.VerificarToken.ConsultarToken();

            List<Entidades.Vistas.TotalesGenerico> lst = new List<Entidades.Vistas.TotalesGenerico>();
           // lst = new Negocio.Indicadores(oToken).IndicadoresPrincipalesGestion();
            string _partialViewName = "~/Views/Home/PartialDashboard/_PartialGestionIndicadoresPrincipales.cshtml";
            return PartialView(_partialViewName, lst);
        }

        [HttpGet]
        public ActionResult PartialSistemasIndicadoresPrincipales()
        {
            Entidades.App.Token oToken = Filters.VerificarToken.ConsultarToken();

            Entidades.Vistas.DashboardSistemas item = new Entidades.Vistas.DashboardSistemas();
            item = new Negocio.Indicadores(oToken).IndicadoresPrincipalesSistemas();
            string _partialViewName = "~/Views/Home/PartialDashboard/_PartialSistemasIndicadoresPrincipales.cshtml";
            return PartialView(_partialViewName, item);
        }

        [HttpGet]
        public ActionResult PartialGridUsuariosLogueados()
        {
            Entidades.App.Token oToken = Filters.VerificarToken.ConsultarToken();

            List<Entidades.App.SIS_Usuario_Login> lst = new List<Entidades.App.SIS_Usuario_Login>();
            lst = new Negocio.App.SIS_Usuarios(oToken).ListarUsuariosLogueados();
            string _partialViewName = "~/Views/Home/PartialDashboard/_PartialGridUsuariosLogueados.cshtml";
            return PartialView(_partialViewName, lst);
        }

    }
}