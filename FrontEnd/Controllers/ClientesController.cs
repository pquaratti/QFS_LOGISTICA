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
    public class ClientesController : ControllerBaseV2
    {

        #region Gestión de Clientes

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PartialGridDataClientes()
        {
            List<Entidades.Cliente> lst = new Negocio.Clientes(GetToken()).ListarActivos();

            return lst.Count > 0 ?
                PartialView("_GridDataClientes", lst) :
                PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", new ObjectMessage() { Message = "No Hay Clientes para mostrar." });
        }

        [HttpPost]
        public JsonResult Delete(string BorrarID)
        {
            ObjectMessage oM = new Negocio.Clientes(GetToken()).DeleteLogico(BorrarID);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult PartialModalABMClientes(string cabeceraID, string detalleID, string subdetalleID)
        {
            return PartialView("_ModalABMClientes", new Negocio.Clientes(GetToken()).ObtenerPorID(cabeceraID));
        }

        [HttpPost]
        public JsonResult SaveModalClientes(Entidades.Cliente obj)
        {
            ObjectMessage oM = new Negocio.Clientes(GetToken()).Save(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Contactos

        [HttpGet]
        public ActionResult PartialModalClientesContactosVista(string cabeceraID, string detalleID, string subdetalleID)
        {
            List<Entidades.ClienteContacto> lst = new Negocio.ClientesContactos(GetToken())
                .ListarClientesContactosPorClienteTipo(Negocio.App.Security.DesencriptarID(cabeceraID));

            return lst.Count > 0 ?
                PartialView("_ModalClientesContactosVista", lst) :
                PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", new ObjectMessage() { Message = "No Hay Contactos para mostrar." });
        }

        [HttpGet]
        public ActionResult PartialModalABMClientesContactos(string cabeceraID, string detalleID, string subdetalleID)
        {
            Entidades.ClienteContacto contacto = new Negocio.ClientesContactos(GetToken()).ObtenerPorID(detalleID);
            if(cabeceraID!="0")
                contacto.Cliente = new Negocio.Clientes(GetToken()).ObtenerPorID(cabeceraID);
            return PartialView("_ModalABMClientesContactos", contacto);
        }
        [HttpPost]
        public JsonResult SaveModalClientesContactos(Entidades.ClienteContacto obj)
        {
            ObjectMessage oM = new Negocio.ClientesContactos(GetToken()).Save(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult DeleteContacto(string BorrarID)
        {
            ObjectMessage oM = new ObjectMessage();
            Negocio.ClientesContactos n = new Negocio.ClientesContactos(GetToken());
            string _id = Negocio.App.Security.DesencriptarID(BorrarID);
            oM = n.Delete(Convert.ToInt32(_id));
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}