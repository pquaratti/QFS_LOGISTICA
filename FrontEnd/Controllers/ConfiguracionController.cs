using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrontEnd.Controllers
{
    [Filters.VerificarToken]
    [Filters.VerificarModule]
    public class ConfiguracionController : ControllerBaseV2
    {
        
        [HttpGet]
        public ActionResult PartialModalParametros(string cabeceraID, string detalleID, string subdetalleID)
        {
            Entidades.App.Token token = GetToken();
            string _partialViewName = "_ModalConfiguracionParametros";

            //ViewBag.DistritosDDL = Negocio.DDL.ListarDistritosAdmin(token);
            //ViewBag.SubdistritosDDL = Negocio.DDL.ListarSubDistritosAdmin(Filters.VerificarToken.ConsultarToken(),0);

            //token.DistritoID = "0";

            return PartialView(_partialViewName, token);
        }

        [HttpPost]
        public JsonResult SaveModalConfiguracionParametros(Entidades.App.Token obj)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();

            try
            {
                Filters.VerificarToken.ChangeTokenParameters(obj);
                oM.Message = "Datos actualizados exitosamente!";
                oM.Success = true;
            }
            catch (Exception ex)
            {
                oM.Message = ex.Message;
                oM.Success = false;
            }

            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

    }
}