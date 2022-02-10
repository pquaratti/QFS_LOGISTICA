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
    public class ActualizacionesController : ControllerBaseV2
    {
        // GET: Mantenimiento
        public ActionResult Masivas()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PartialModalActualizarCBUS(string cabeceraID, string detalleID, string subdetalleID)
        {
            Entidades.App.Token oToken = GetToken();
            string _partialViewName = "_ModalActualizacionCBUs";
            return PartialView(_partialViewName);
        }

    }
}