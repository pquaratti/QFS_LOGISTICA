using Entidades.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Hospitales.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        [HttpGet]
        public ActionResult Login()
        {
            Entidades.App.Token tokenActual = Filters.VerificarToken.ConsultarToken();

            if (tokenActual != null)
                return RedirectToAction("Modules", "Home");
            
            return View();
        }

        [HttpPost]
        public JsonResult Login(Entidades.App.SIS_Usuario usuario)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            
            try
            {
                Negocio.App.SIS_Usuarios negocioUSU = new Negocio.App.SIS_Usuarios();
                Entidades.App.Token token = negocioUSU.Autenticar(usuario);
                token.IPAddress = Request.UserHostAddress;
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, "APP", DateTime.Now, DateTime.Now.AddHours(8), false, Resources.Repositorio.JSONSerialize(token), FormsAuthentication.FormsCookiePath);
                string ticketEncriptado = FormsAuthentication.Encrypt(ticket);
                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, ticketEncriptado) { Expires = DateTime.Now.AddHours(8) });
                Response.Cookies.Add(new HttpCookie(Negocio.App.Security.CookieSessionName(), Negocio.App.Security.Encriptar(token.ID)) { Expires = DateTime.Now.AddHours(8) });
              //  Session["Acciones"] = negocioUSU.ListarAcciones(Convert.ToInt32(token.UserID), "GLO");
                oM.Success = true;
                oM.TokenExist = true;
                oM.Message = "Usuario Autenticado!";
                oM.RedirectNew = true;
                oM.urlRedirect = Url.Action("Modules", "Home");

                negocioUSU.RegistrarLogin(token);
            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.TokenExist = false;
                oM.Message = ex.Message;
            }

            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CloseSession()
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();

            Entidades.App.Token tokenActual = Filters.VerificarToken.ConsultarToken();

            Negocio.App.SIS_Usuarios negocioUSU = new Negocio.App.SIS_Usuarios();
            negocioUSU.CloseLogin(tokenActual);

            Request.Cookies.Remove(Negocio.App.Security.CookieSessionName());
            Response.Cookies[Negocio.App.Security.CookieSessionName()].Expires = DateTime.Now.AddDays(-1);
            FormsAuthentication.SignOut();

            Session["Acciones"] = null;

            return View("Login");
        }

        [HttpGet]
        public ActionResult CloseModule()
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            Filters.VerificarToken.ChangeTokenModuleID("0");
            Session["Acciones"] = null;
            return RedirectToAction("Modules","Home");
        }
        
        [HttpGet]
        public string PasswordDefault()
        {
            string sPassword = Negocio.App.Security.Encriptar("SISTEMASALUD");
            return sPassword;
        }

        [HttpGet]
        public ActionResult AccesoDenegado()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Setup(string pClave)
        {
            if (pClave == "pabale20")
            {
                Negocio.App.Setup negocioSETUP = new Negocio.App.Setup();
                negocioSETUP.Iniciar();
            }
            
            return View();
        }

        [HttpGet]
        public ActionResult Recuperar()
        {
            return View();
        }

        [HttpPost]
        public JsonResult RequestRecoveryAccount(Entidades.App.SIS_Usuario obj)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();

            oM = new Negocio.App.SIS_Usuarios_Password_Recovery().SolicitarRecupero(obj, Server.MapPath("~/MailTemplates/RecoveryPasswordCode.html"));

            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PartialRecoveryCode(string tokenID)
        {
            Entidades.App.SIS_Usuario_Password_Recovery obj = new SIS_Usuario_Password_Recovery();

            obj = new Negocio.App.SIS_Usuarios_Password_Recovery().ObtenerPorRecoveryToken(Negocio.App.Security.DesencriptarBasico(tokenID));

            obj.LeyendaEnvioCodigo = "Te enviamos en código a la siguiente dirección de correo electrónico <b>" + obj.upr_mail + "</b>";

            obj.recoveryTokenID = tokenID;

            string _partialViewName = "_PartialRecoveryCode";
            return PartialView(_partialViewName, obj);
        }

        [HttpPost]
        public ActionResult RequestRecoveryAccountCode(Entidades.App.SIS_Usuario_Password_Recovery obj)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();

            oM = new Negocio.App.SIS_Usuarios_Password_Recovery().ValidarChallenge(obj);

            string _partialViewName = "_PartialRecoveryPassword";

            if (oM.Success == false)
            {
                _partialViewName = "_PartialRecoveryCode";
                obj.customMessageError = "Código ingresado incorrecto, vuelva a intentar nuevamente.";
            }
                
            return PartialView(_partialViewName, obj);
        }

        [HttpPost]
        public JsonResult RequestRecoveryPassword(Entidades.App.SIS_Usuario_Password_Recovery obj)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            obj.MailTemplatePath = Server.MapPath("~/MailTemplates/RecoveryPasswordNotification.html");

            oM = new Negocio.App.SIS_Usuarios_Password_Recovery().RealizarCambioPassword(obj);

            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult SaveModalTerminosYCondiciones()
        {
            Entidades.App.Token tokenActual = Filters.VerificarToken.ConsultarToken();

            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            oM = new Negocio.App.SIS_Usuarios(tokenActual).ConfirmarTerminosYCondiciones(tokenActual.UserID);

            if (oM.Success)
                Filters.VerificarToken.UpdateTerminosYCondiciones();
            
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

    }
}