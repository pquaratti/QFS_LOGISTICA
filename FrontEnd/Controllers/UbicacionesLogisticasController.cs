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
    public class UbicacionesLogisticasController : ControllerBaseV2
    {

        #region Gestión Tipos de Ubicaciones

        public ActionResult Tipos()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PartialGridDataTipoUbicaciones()
        {
            List<Entidades.TipoUbicacionLogistica> lst = new Negocio.TipoUbicacionesLogisticas(GetToken()).ListarActivos();

            return lst.Count > 0 ?
                PartialView("_GridDataTipoUbicaciones", lst) :
                PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", new ObjectMessage() { Message = "No Hay Tipos de Ubicaciones para mostrar." });
        }

        [HttpPost]
        public JsonResult Delete(string BorrarID)
        {
            ObjectMessage oM = new Negocio.TipoUbicacionesLogisticas(GetToken()).DeleteLogico(BorrarID);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult PartialModalABMTipoUbicaciones(string cabeceraID, string detalleID, string subdetalleID)
        {
            return PartialView("_ModalABMTipoUbicaciones", new Negocio.TipoUbicacionesLogisticas(GetToken()).ObtenerPorID(cabeceraID));
        }

        [HttpPost]
        public JsonResult SaveModalTipoUbicaciones(Entidades.TipoUbicacionLogistica obj)
        {
            ObjectMessage oM = new Negocio.TipoUbicacionesLogisticas(GetToken()).Save(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Gestión Tipos de EstadoUbicaciones

        public ActionResult Estados()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PartialGridDataTipoEstadoUbicaciones()
        {
            List<Entidades.TipoEstadoUbicacionLogistica> lst = new Negocio.TipoEstadosUbicacionesLogisticas(GetToken()).ListarActivos();

            return lst.Count > 0 ?
                PartialView("_GridDataTipoEstadoUbicaciones", lst) :
                PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", new ObjectMessage() { Message = "No Hay Tipos de Estados de Ubicaciones para mostrar." });
        }

        [HttpPost]
        public JsonResult DeleteEstado(string BorrarID)
        {
            ObjectMessage oM = new Negocio.TipoEstadosUbicacionesLogisticas(GetToken()).DeleteLogico(BorrarID);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult PartialModalABMTipoEstadoUbicaciones(string cabeceraID, string detalleID, string subdetalleID)
        {
            return PartialView("_ModalABMTipoEstadoUbicaciones", new Negocio.TipoEstadosUbicacionesLogisticas(GetToken()).ObtenerPorID(cabeceraID));
        }

        [HttpPost]
        public JsonResult SaveModalTipoEstadoUbicaciones(Entidades.TipoEstadoUbicacionLogistica obj)
        {
            ObjectMessage oM = new Negocio.TipoEstadosUbicacionesLogisticas(GetToken()).Save(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Gestión Tipos de Estados de Stock

        public ActionResult EstadosStock()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PartialGridDataTipoEstadoStock()
        {
            List<Entidades.TipoEstadoStock> lst = new Negocio.TipoEstadosStock(GetToken()).ListarActivos();

            return lst.Count > 0 ?
                PartialView("_GridDataTipoEstadoStock", lst) :
                PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", new ObjectMessage() { Message = "No Hay Tipos de Estados de Stock para mostrar." });
        }

        [HttpPost]
        public JsonResult DeleteEstadoStock(string BorrarID)
        {
            ObjectMessage oM = new Negocio.TipoEstadosStock(GetToken()).DeleteLogico(BorrarID);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PartialModalABMTipoEstadoStock(string cabeceraID, string detalleID, string subdetalleID)
        {
            return PartialView("_ModalABMTipoEstadoStock", new Negocio.TipoEstadosStock(GetToken()).ObtenerPorID(cabeceraID));
        }

        [HttpPost]
        public JsonResult SaveModalTipoEstadoStock(Entidades.TipoEstadoStock obj)
        {
            ObjectMessage oM = new Negocio.TipoEstadosStock(GetToken()).Save(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Gestión Tipos de Estados de Ingreso

        public ActionResult EstadosIngreso()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PartialGridDataTipoEstadoIngreso()
        {
            List<Entidades.TipoEstadoIngreso> lst = new Negocio.TipoEstadosIngreso(GetToken()).ListarActivos();

            return lst.Count > 0 ?
                PartialView("_GridDataTipoEstadoIngreso", lst) :
                PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", new ObjectMessage() { Message = "No Hay Tipos de Estados de Ingreso para mostrar." });
        }

        [HttpPost]
        public JsonResult DeleteEstadoIngreso(string BorrarID)
        {
            ObjectMessage oM = new Negocio.TipoEstadosIngreso(GetToken()).DeleteLogico(BorrarID);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PartialModalABMTipoEstadoIngreso(string cabeceraID, string detalleID, string subdetalleID)
        {
            return PartialView("_ModalABMTipoEstadoIngreso", new Negocio.TipoEstadosIngreso(GetToken()).ObtenerPorID(cabeceraID));
        }

        [HttpPost]
        public JsonResult SaveModalTipoEstadoIngreso(Entidades.TipoEstadoIngreso obj)
        {
            ObjectMessage oM = new Negocio.TipoEstadosIngreso(GetToken()).Save(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Gestión Tipos de Estados de Pedido de Salida

        public ActionResult EstadosPedidoSalida()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PartialGridDataTipoEstadoPedidoSalida()
        {
            List<Entidades.TipoEstadoPedidoSalida> lst = new Negocio.TipoEstadosPedidosSalida(GetToken()).ListarActivos();

            return lst.Count > 0 ?
                PartialView("_GridDataTipoEstadoPedidoSalida", lst) :
                PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", new ObjectMessage() { Message = "No Hay Tipos de Estados de Pedido de Salida para mostrar." });
        }

        [HttpPost]
        public JsonResult DeleteEstadoPedidoSalida(string BorrarID)
        {
            ObjectMessage oM = new Negocio.TipoEstadosPedidosSalida(GetToken()).DeleteLogico(BorrarID);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PartialModalABMTipoEstadoPedidoSalida(string cabeceraID, string detalleID, string subdetalleID)
        {
            return PartialView("_ModalABMTipoEstadoPedidoSalida", new Negocio.TipoEstadosPedidosSalida(GetToken()).ObtenerPorID(cabeceraID));
        }

        [HttpPost]
        public JsonResult SaveModalTipoEstadoPedidoSalida(Entidades.TipoEstadoPedidoSalida obj)
        {
            ObjectMessage oM = new Negocio.TipoEstadosPedidosSalida(GetToken()).Save(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Gestion de Ubicaciones

        public ActionResult Gestion()
        {
            return View();
        }

        public ActionResult Configuracion()
        {
            Entidades.UbicacionLogistica obj = new Entidades.UbicacionLogistica();

            ViewBag.Plantas = new Negocio.Plantas(GetToken()).ListarSimple();
            ViewBag.Depositos = new Negocio.Depositos(GetToken()).ListarSimple();
            ViewBag.Zonas = new Negocio.ZonasLogisticas(GetToken()).ListarSimple();
            ViewBag.TipoUbicacionesLogisticas = new Negocio.TipoUbicacionesLogisticas(GetToken()).ListarSimple();

            return View(obj);
        }


        #endregion

    }
}