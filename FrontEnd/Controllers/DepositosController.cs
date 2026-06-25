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
                pesoMaximo = x.depopas_peso_maximo
            }).ToList();

            return View(deposito);
        }

        public ActionResult EditorPasillos(string depositoID)
        {
            Entidades.Deposito deposito = new Negocio.Depositos(GetToken()).ObtenerPorID(depositoID);
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
                zonaId = x.Zona != null ? x.Zona.zonlog_id : 0,
                esTransito = x.depopas_es_transito
            });

            return Json(new { Result = true, Data = data }, JsonRequestBehavior.AllowGet);
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

        #endregion

        #region Configuración visual del depósito

        public ActionResult ConfiguracionVisual(string depositoID)
        {
            Entidades.Deposito deposito = new Negocio.Depositos(GetToken()).ObtenerPorID(depositoID);
            return View(deposito);
        }

        // ---- Zonas ----------------------------------------------------------

        [HttpGet]
        public JsonResult ListarZonas(int depositoID)
        {
            List<Entidades.ZonaLogistica> lst = new Negocio.ZonasLogisticas(GetToken()).ListarPorDeposito(depositoID);
            var data = lst.Select(x => new
            {
                id = x.IdEncriptado,
                dbId = x.zonlog_id,
                codigo = x.zonlog_codigo,
                nombre = x.zonlog_nombre,
                descripcion = x.zonlog_descripcion,
                x = x.zonlog_x,
                y = x.zonlog_y,
                largo = x.zonlog_largo,
                ancho = x.zonlog_ancho,
                color = x.zonlog_color
            });

            return Json(new { Result = true, Data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveZona(Entidades.ZonaLogistica obj)
        {
            ObjectMessage oM = new Negocio.ZonasLogisticas(GetToken()).Save(obj);
            return Json(new { Result = oM, Id = oM.ObjectRelation, DbId = obj.zonlog_id }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteZona(string BorrarID)
        {
            ObjectMessage oM = new Negocio.ZonasLogisticas(GetToken()).DeleteLogico(BorrarID);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        // ---- Racks ----------------------------------------------------------

        [HttpGet]
        public JsonResult ListarRacks(int depositoID)
        {
            List<Entidades.DepositoRack> lst = new Negocio.DepositosRacks(GetToken()).ListarPorDeposito(depositoID);
            var data = lst.Select(x => new
            {
                id = x.IdEncriptado,
                dbId = x.deprack_id,
                codigo = x.deprack_codigo,
                nombre = x.deprack_nombre,
                descripcion = x.deprack_descripcion,
                x = x.deprack_x,
                y = x.deprack_y,
                largo = x.deprack_largo,
                ancho = x.deprack_ancho,
                orientacion = x.deprack_orientacion,
                columnas = x.deprack_cantidad_columnas,
                niveles = x.deprack_cantidad_niveles,
                alturaNivel = x.deprack_altura_nivel,
                pesoMaximo = x.deprack_peso_maximo,
                color = x.deprack_color,
                zonaId = x.deprack_zonlog_id ?? 0,
                pasilloId = x.deprack_pasillo_id ?? 0
            });

            return Json(new { Result = true, Data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveRack(Entidades.DepositoRack obj, bool generarUbicaciones = true)
        {
            ObjectMessage oM = generarUbicaciones ?
                new Negocio.DepositosRacks(GetToken()).SaveYGenerarUbicaciones(obj) :
                new Negocio.DepositosRacks(GetToken()).Save(obj);

            return Json(new { Result = oM, Id = oM.ObjectRelation, DbId = obj.deprack_id }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteRack(string BorrarID)
        {
            ObjectMessage oM = new Negocio.DepositosRacks(GetToken()).DeleteLogicoConUbicaciones(BorrarID);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        // ---- Pasillos de tránsito ------------------------------------------

        [HttpPost]
        public JsonResult SavePasilloTransito(Entidades.DepositoPasillo obj)
        {
            obj.depopas_es_transito = true;
            if (obj.depopas_cantidad_posiciones <= 0) obj.depopas_cantidad_posiciones = 1;
            if (obj.depopas_cantidad_alturas <= 0) obj.depopas_cantidad_alturas = 1;
            ObjectMessage oM = new Negocio.DepositosPasillos(GetToken()).Save(obj);
            return Json(new { Result = oM, Id = oM.ObjectRelation, DbId = obj.depopas_id }, JsonRequestBehavior.AllowGet);
        }

        // ---- Posición (drag & drop) ----------------------------------------

        [HttpPost]
        public JsonResult GuardarPosicion(string tipo, int id, decimal x, decimal y, decimal largo, decimal ancho)
        {
            ObjectMessage oM = new ObjectMessage() { Success = true, Message = "Posición actualizada." };
            try
            {
                switch ((tipo ?? "").ToLower())
                {
                    case "zona":
                        new Negocio.ZonasLogisticas(GetToken()).ActualizarGeometria(id, x, y, largo, ancho);
                        break;
                    case "rack":
                        new Negocio.DepositosRacks(GetToken()).ActualizarGeometria(id, x, y, largo, ancho);
                        break;
                    case "pasillo":
                        new Negocio.DepositosPasillos(GetToken()).ActualizarGeometria(id, x, y, largo, ancho);
                        break;
                    default:
                        oM.Success = false;
                        oM.Message = "Tipo de elemento desconocido.";
                        break;
                }
            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.Message = ex.Message;
            }

            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        // ---- Ubicaciones ----------------------------------------------------

        [HttpGet]
        public JsonResult ListarUbicacionesRack(int rackID)
        {
            List<Entidades.UbicacionLogistica> lst = new Negocio.UbicacionesLogisticas(GetToken()).ListarPorRack(rackID);
            var data = lst.Select(u => new
            {
                id = u.IdEncriptado,
                dbId = u.ubilog_id,
                codigo = u.ubilog_codigo,
                columna = u.ubilog_columna,
                nivel = u.ubilog_nivel,
                posicion = u.ubilog_posicion,
                estadoId = u.ubilog_teubilog_id,
                tipoUbicacionId = u.ubilog_tubilog_id
            });

            return Json(new { Result = true, Data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerUbicacion(int ubicacionID)
        {
            Entidades.UbicacionLogistica u = new Negocio.UbicacionesLogisticas(GetToken()).ObtenerUbicacion(ubicacionID);
            var data = new
            {
                dbId = u.ubilog_id,
                codigo = u.ubilog_codigo,
                posicion = u.ubilog_posicion,
                columna = u.ubilog_columna,
                nivel = u.ubilog_nivel,
                coordX = u.ubilog_coord_x,
                coordY = u.ubilog_coord_y,
                coordZ = u.ubilog_coord_z,
                alto = u.ubilog_altura,
                ancho = u.ubilog_anchura,
                profundidad = u.ubilog_longitud,
                capacidadMaxima = u.ubilog_capacidad_maxima,
                pesoMaximo = u.ubilog_peso_maximo,
                volumenMaximo = u.ubilog_volumen_maximo,
                estadoId = u.ubilog_teubilog_id,
                tipoUbicacionId = u.ubilog_tubilog_id,
                multiplesArticulos = u.ubilog_multiples_articulos,
                multiplesLotes = u.ubilog_multiples_lotes,
                tipoProductoPermitido = u.ubilog_tipo_producto_permitido
            };

            return Json(new { Result = true, Data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarUbicacion(Entidades.UbicacionLogistica obj)
        {
            ObjectMessage oM = new Negocio.UbicacionesLogisticas(GetToken()).Save(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarCatalogosUbicacion()
        {
            var estados = new Negocio.TipoEstadosUbicacionesLogisticas(GetToken()).ListarActivos()
                .Select(e => new { id = e.teubilog_id, nombre = e.teubilog_nombre });
            var tipos = new Negocio.TipoUbicacionesLogisticas(GetToken()).ListarActivos()
                .Select(t => new { id = t.tubilog_id, nombre = t.tubilog_nombre });

            return Json(new { Result = true, Estados = estados, Tipos = tipos }, JsonRequestBehavior.AllowGet);
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