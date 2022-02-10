using Hospitales.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Filters
{
    public class VerificarToken : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (filterContext.Controller is AccountController == false)
            {
                Entidades.App.Token currentToken = ConsultarToken();

                if (currentToken == null)
                    filterContext.Result = new RedirectToRouteResult("CerrarSesion", null);
                else
                {
                    if (currentToken.ID != HashToken())
                        filterContext.Result = new RedirectToRouteResult("CerrarSesion", null);
                }
            }

        }

        public static Entidades.App.Token ConsultarToken()
        {
            try
            {
                var request = HttpContext.Current.Request;
                HttpCookie authCookie = request.Cookies[FormsAuthentication.FormsCookieName];

                if (authCookie != null)
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                    Entidades.App.Token userToken = Negocio.App.Security.JsonToToken(ticket.UserData);
                    return userToken;
                }
                else
                    throw new Exception("No token");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void ChangeTokenModuleID(string moduleID)
        {
            Entidades.App.Token oToken = ConsultarToken();
            oToken.ModuloID = moduleID;
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, "APP", DateTime.Now, DateTime.Now.AddHours(8), false, Resources.Repositorio.JSONSerialize(oToken), FormsAuthentication.FormsCookiePath);
            string ticketEncriptado = FormsAuthentication.Encrypt(ticket);
            HttpContext.Current.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, ticketEncriptado) { Expires = DateTime.Now.AddHours(8) });
        }

        public static void UpdateTerminosYCondiciones()
        {
            Entidades.App.Token oToken = ConsultarToken();
            oToken.AceptaTerminosYCondiciones = true;
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, "APP", DateTime.Now, DateTime.Now.AddHours(8), false, Resources.Repositorio.JSONSerialize(oToken), FormsAuthentication.FormsCookiePath);
            string ticketEncriptado = FormsAuthentication.Encrypt(ticket);
            HttpContext.Current.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, ticketEncriptado) { Expires = DateTime.Now.AddHours(8) });
        }

        public static void ChangeTokenParameters(Entidades.App.Token newToken)
        {
            //Negocio.Distritos negocioDIS = new Negocio.Distritos(newToken);
            //Negocio.Subdistrito negocioSUD = new Negocio.Subdistrito(newToken);

            //Entidades.App.Token oToken = ConsultarToken();

            //oToken.DistritoID = newToken.DistritoID;
            //oToken.SubdistritoID = newToken.SubdistritoID;

            //if (Convert.ToInt32(oToken.DistritoID).Equals(0))
            //    oToken.NombreDistrito = "Administrador de Distritos";
            //else
            //    oToken.NombreDistrito = negocioDIS.ObtenerPorID(oToken.DistritoID).dis_nombre;

            //if (Convert.ToInt32(oToken.SubdistritoID).Equals(-1))
            //    oToken.NombreSubdistrito = "Todos los subdistritos";

            //if (Convert.ToInt32(oToken.SubdistritoID).Equals(0))
            //    oToken.NombreSubdistrito = "Nivel Distrito";

            //if (Convert.ToInt32(oToken.SubdistritoID) > 0)
            //    oToken.NombreSubdistrito = negocioSUD.ObtenerPorID(oToken.SubdistritoID).sud_nombre;

            //FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, "APP", DateTime.Now, DateTime.Now.AddHours(8), false, Resources.Repositorio.JSONSerialize(oToken), FormsAuthentication.FormsCookiePath);
            //string ticketEncriptado = FormsAuthentication.Encrypt(ticket);
            //HttpContext.Current.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, ticketEncriptado) { Expires = DateTime.Now.AddHours(8) });
        }

        public static string HashToken()
        {
            try
            {
                var request = HttpContext.Current.Request;
                HttpCookie hashCookie = request.Cookies[Negocio.App.Security.CookieSessionName()];
                return Negocio.App.Security.Desencriptar(hashCookie.Value.ToString());
            }
            catch (Exception)
            {

                return "";
            }

        }

        private Type GetExpectedReturnType(ActionExecutingContext filterContext)
        {
            // Find out what type is expected to be returned
            string actionName = filterContext.ActionDescriptor.ActionName;
            Type controllerType = filterContext.Controller.GetType();
            MethodInfo actionMethodInfo = default(MethodInfo);
            try
            {
                actionMethodInfo = controllerType.GetMethod(actionName);
            }
            catch (AmbiguousMatchException ex)
            {
                // Try to find a match using the parameters passed through
                var actionParams = filterContext.ActionParameters;
                List<Type> paramTypes = new List<Type>();
                foreach (var p in actionParams)
                {
                    paramTypes.Add(p.Value.GetType());
                }

                actionMethodInfo = controllerType.GetMethod(actionName, paramTypes.ToArray());
            }

            return actionMethodInfo.ReturnType;
        }

    }
}