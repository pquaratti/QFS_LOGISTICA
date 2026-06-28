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
    public class DepositosController : ControllerBaseV2
    {

        #region Gestión de Depositos

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MapaWmsDemo(string depositoID)
        {
            Entidades.Deposito deposito = string.IsNullOrWhiteSpace(depositoID)
                ? new Entidades.Deposito()
                : new Negocio.Depositos(GetToken()).ObtenerPorID(depositoID);

            List<Entidades.DepositoPasillo> pasillos = string.IsNullOrWhiteSpace(depositoID)
                ? new List<Entidades.DepositoPasillo>()
                : new Negocio.DepositosPasillos(GetToken()).ListarPorDeposito(depositoID);

            List<Entidades.DepositoZona> zonas = string.IsNullOrWhiteSpace(depositoID)
                ? new List<Entidades.DepositoZona>()
                : new Negocio.DepositosZonas(GetToken()).ListarPorDeposito(depositoID);

            ViewBag.WmsZonas = zonas.Select(x => new
            {
                id = x.IdEncriptado,
                dbId = x.depzon_id,
                codigo = x.depzon_codigo,
                nombre = x.depzon_nombre,
                descripcion = x.depzon_descripcion,
                x = x.depzon_x,
                y = x.depzon_y,
                largo = x.depzon_largo,
                ancho = x.depzon_ancho
            }).ToList();

            ViewBag.WmsPasillos = pasillos.Select(x => new
            {
                id = x.IdEncriptado,
                dbId = x.depopas_id,
                codigo = x.depopas_codigo,
                nombre = x.depopas_nombre,
                descripcion = x.depopas_descripcion,
                x = x.depopas_x,
                y = x.depopas_y,
                largo = x.depopas_largo,
                ancho = x.depopas_ancho,
                orientacion = x.depopas_orientacion,
                posiciones = x.depopas_cantidad_posiciones,
                alturas = x.depopas_cantidad_alturas,
                alturaNivel = x.depopas_altura_nivel,
                pesoMaximo = x.depopas_peso_maximo,
                zonaId = x.Zona != null ? x.Zona.IdEncriptado : "",
                zonaDbId = x.Zona != null ? x.Zona.depzon_id : 0,
                zonaCodigo = x.Zona != null ? x.Zona.depzon_codigo : "",
                zonaNombre = x.Zona != null ? x.Zona.depzon_nombre : ""
            }).ToList();

            return View(deposito);
        }

        public ActionResult EditorPasillos(string depositoID)
        {
            Entidades.Deposito deposito = new Negocio.Depositos(GetToken()).ObtenerPorID(depositoID);
            ViewBag.EditorZonas = new Negocio.DepositosZonas(GetToken()).ListarPorDeposito(depositoID).Select(x => new
            {
                id = x.IdEncriptado,
                dbId = x.depzon_id,
                codigo = x.depzon_codigo,
                nombre = x.depzon_nombre,
                descripcion = x.depzon_descripcion,
                x = x.depzon_x,
                y = x.depzon_y,
                largo = x.depzon_largo,
                ancho = x.depzon_ancho
            }).ToList();
            return View(deposito);
        }

        [HttpGet]
        public JsonResult ListarPasillos(string depositoID)
        {
            List<Entidades.DepositoPasillo> lst = new Negocio.DepositosPasillos(GetToken()).ListarPorDeposito(depositoID);
            var data = lst.Select(x => new
            {
                id = x.IdEncriptado,
                dbId = x.depopas_id,
                codigo = x.depopas_codigo,
                nombre = x.depopas_nombre,
                descripcion = x.depopas_descripcion,
                x = x.depopas_x,
                y = x.depopas_y,
                largo = x.depopas_largo,
                ancho = x.depopas_ancho,
                orientacion = x.depopas_orientacion,
                posiciones = x.depopas_cantidad_posiciones,
                alturas = x.depopas_cantidad_alturas,
                alturaNivel = x.depopas_altura_nivel,
                pesoMaximo = x.depopas_peso_maximo,
                zonaId = x.Zona != null ? x.Zona.IdEncriptado : "",
                zonaDbId = x.Zona != null ? x.Zona.depzon_id : 0,
                zonaCodigo = x.Zona != null ? x.Zona.depzon_codigo : "",
                zonaNombre = x.Zona != null ? x.Zona.depzon_nombre : ""
            });

            return Json(new { Result = true, Data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarZonas(string depositoID)
        {
            var lst = new Negocio.DepositosZonas(GetToken()).ListarPorDeposito(depositoID);
            var data = lst.Select(x => new { id = x.IdEncriptado, dbId = x.depzon_id, codigo = x.depzon_codigo, nombre = x.depzon_nombre, descripcion = x.depzon_descripcion, x = x.depzon_x, y = x.depzon_y, largo = x.depzon_largo, ancho = x.depzon_ancho });
            return Json(new { Result = true, Data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveZona(Entidades.DepositoZona obj)
        {
            ObjectMessage oM = new Negocio.DepositosZonas(GetToken()).Save(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SavePasillo(Entidades.DepositoPasillo obj, bool generarUbicaciones = true)
        {
            ObjectMessage oM = generarUbicaciones ?
                new Negocio.DepositosPasillos(GetToken()).SaveYGenerarUbicaciones(obj) :
                new Negocio.DepositosPasillos(GetToken()).Save(obj);

            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeletePasillo(string BorrarID)
        {
            ObjectMessage oM = new Negocio.DepositosPasillos(GetToken()).DeleteLogicoConUbicaciones(BorrarID);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PartialGridDataDepositos(string plantaID)
        {
            List<Entidades.Deposito> lst = new Negocio.Depositos(GetToken()).ListarDepositosPorPlanta(plantaID);

            return lst.Count > 0 ?
                PartialView("_GridDataDepositos", lst) :
                PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", new ObjectMessage() { Message = "No Hay Depositos para mostrar." });
        }

        [HttpPost]
        public JsonResult Delete(string BorrarID)
        {
            ObjectMessage oM = new Negocio.Depositos(GetToken()).DeleteLogico(BorrarID);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult PartialModalABMDepositos(string cabeceraID, string detalleID, string subdetalleID)
        {
            return PartialView("_ModalABMDepositos", new Negocio.Depositos(GetToken()).ObtenerPorID(cabeceraID));
        }

        [HttpPost]
        public JsonResult SaveModalDepositos(Entidades.Deposito obj)
        {
            ObjectMessage oM = new Negocio.Depositos(GetToken()).Save(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}