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

            List<Entidades.UbicacionLogistica> ubicaciones = new List<Entidades.UbicacionLogistica>();
            foreach (Entidades.DepositoPasillo pasillo in pasillos)
                ubicaciones.AddRange(new Negocio.UbicacionesLogisticas(GetToken()).ListarPorPasillo(pasillo.depopas_id));

            ViewBag.WmsUbicaciones = ubicaciones.Select(x => new
            {
                id = x.IdEncriptado,
                dbId = x.ubilog_id,
                codigo = x.ubilog_codigo,
                pasilloDbId = x.Pasillo != null ? x.Pasillo.depopas_id : 0,
                posicion = x.ubilog_posicion,
                nivel = x.ubilog_nivel,
                altura = x.ubilog_altura,
                longitud = x.ubilog_longitud,
                anchura = x.ubilog_anchura,
                capacidadCubica = x.ubilog_capacidad_cubica,
                pesoMaximo = x.ubilog_peso_maximo,
                estado = x.TipoEstado != null ? x.TipoEstado.teubilog_nombre : ""
            }).ToList();

            return View(deposito);
        }

        [HttpPost]
        public JsonResult GuardarEstadoUbicacionWms(FrontEnd.Models.WmsEstadoUbicacionRequest obj)
        {
            ObjectMessage oM = new Negocio.UbicacionesLogisticas(GetToken()).GuardarEstadoWms(obj.UbicacionID, obj.Estado, obj.PasilloID, obj.Posicion, obj.Nivel);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Inventario(string depositoID)
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

            FrontEnd.Models.DepositoOcupacionViewModel model = new FrontEnd.Models.DepositoOcupacionViewModel();
            model.Deposito = deposito;

            foreach (Entidades.DepositoZona zona in zonas)
            {
                FrontEnd.Models.ZonaOcupacionViewModel zonaVm = new FrontEnd.Models.ZonaOcupacionViewModel()
                {
                    Codigo = zona.depzon_codigo,
                    Nombre = zona.depzon_nombre
                };

                foreach (Entidades.DepositoPasillo pasillo in pasillos.Where(x => x.Zona != null && x.Zona.depzon_id == zona.depzon_id))
                    zonaVm.Pasillos.Add(CrearPasilloOcupacion(pasillo));

                CalcularTotalesZona(zonaVm);
                model.Zonas.Add(zonaVm);
            }

            List<Entidades.DepositoPasillo> pasillosSinZona = pasillos.Where(x => x.Zona == null || x.Zona.depzon_id <= 0 || !zonas.Any(z => z.depzon_id == x.Zona.depzon_id)).ToList();
            if (pasillosSinZona.Count > 0)
            {
                FrontEnd.Models.ZonaOcupacionViewModel sinZonaVm = new FrontEnd.Models.ZonaOcupacionViewModel()
                {
                    Codigo = "S/Z",
                    Nombre = "Sin zona"
                };

                foreach (Entidades.DepositoPasillo pasillo in pasillosSinZona)
                    sinZonaVm.Pasillos.Add(CrearPasilloOcupacion(pasillo));

                CalcularTotalesZona(sinZonaVm);
                model.Zonas.Add(sinZonaVm);
            }

            model.TotalLugares = model.Zonas.Sum(x => x.TotalLugares);
            model.LugaresOcupados = model.Zonas.Sum(x => x.LugaresOcupados);
            model.LugaresLibres = model.Zonas.Sum(x => x.LugaresLibres);
            model.LugaresBloqueados = model.Zonas.Sum(x => x.LugaresBloqueados);
            model.PorcentajeOcupacion = CalcularPorcentaje(model.LugaresOcupados, model.TotalLugares);
            model.TieneSaldosPorUbicacion = false;

            return View(model);
        }

        private FrontEnd.Models.PasilloOcupacionViewModel CrearPasilloOcupacion(Entidades.DepositoPasillo pasillo)
        {
            FrontEnd.Models.PasilloOcupacionViewModel pasilloVm = new FrontEnd.Models.PasilloOcupacionViewModel()
            {
                Id = pasillo.depopas_id,
                Codigo = pasillo.depopas_codigo,
                Nombre = pasillo.depopas_nombre,
                Posiciones = pasillo.depopas_cantidad_posiciones,
                Alturas = pasillo.depopas_cantidad_alturas,
                AlturaNivel = pasillo.depopas_altura_nivel,
                PesoMaximo = pasillo.depopas_peso_maximo
            };

            List<Entidades.UbicacionLogistica> ubicaciones = new Negocio.UbicacionesLogisticas(GetToken()).ListarPorPasillo(pasillo.depopas_id);

            for (int nivel = pasillo.depopas_cantidad_alturas; nivel >= 1; nivel--)
            {
                for (int posicion = 1; posicion <= pasillo.depopas_cantidad_posiciones; posicion++)
                {
                    Entidades.UbicacionLogistica ubicacion = ubicaciones.FirstOrDefault(x => x.ubilog_posicion == posicion && Convert.ToString(x.ubilog_nivel) == Convert.ToString(nivel));
                    pasilloVm.Ubicaciones.Add(CrearUbicacionOcupacion(pasillo, ubicacion, posicion, nivel));
                }
            }

            pasilloVm.TotalLugares = pasilloVm.Ubicaciones.Count;
            pasilloVm.LugaresOcupados = pasilloVm.Ubicaciones.Count(x => x.EsOcupada);
            pasilloVm.LugaresBloqueados = pasilloVm.Ubicaciones.Count(x => x.EsBloqueada);
            pasilloVm.LugaresLibres = pasilloVm.TotalLugares - pasilloVm.LugaresOcupados - pasilloVm.LugaresBloqueados;
            pasilloVm.PorcentajeOcupacion = CalcularPorcentaje(pasilloVm.LugaresOcupados, pasilloVm.TotalLugares);

            return pasilloVm;
        }

        private FrontEnd.Models.UbicacionOcupacionViewModel CrearUbicacionOcupacion(Entidades.DepositoPasillo pasillo, Entidades.UbicacionLogistica ubicacion, int posicion, int nivel)
        {
            bool tieneUbicacion = ubicacion != null && ubicacion.ubilog_id > 0;
            string estado = tieneUbicacion && ubicacion.TipoEstado != null && !string.IsNullOrWhiteSpace(ubicacion.TipoEstado.teubilog_nombre)
                ? ubicacion.TipoEstado.teubilog_nombre
                : "Libre";

            bool esBloqueada = estado.IndexOf("bloq", StringComparison.OrdinalIgnoreCase) >= 0;
            bool esOcupada = !esBloqueada && (
                estado.IndexOf("ocup", StringComparison.OrdinalIgnoreCase) >= 0 ||
                estado.IndexOf("parcial", StringComparison.OrdinalIgnoreCase) >= 0 ||
                estado.IndexOf("reserv", StringComparison.OrdinalIgnoreCase) >= 0
            );

            int porcentaje = esBloqueada ? 0 : esOcupada ? 100 : 0;

            return new FrontEnd.Models.UbicacionOcupacionViewModel()
            {
                Id = tieneUbicacion ? ubicacion.ubilog_id : 0,
                Codigo = tieneUbicacion ? ubicacion.ubilog_codigo : pasillo.depopas_codigo + "-P" + posicion.ToString("000") + "-N" + nivel.ToString("00"),
                Posicion = posicion,
                Nivel = nivel.ToString(),
                Altura = tieneUbicacion ? ubicacion.ubilog_altura : pasillo.depopas_altura_nivel,
                Longitud = tieneUbicacion ? ubicacion.ubilog_longitud : pasillo.depopas_largo / pasillo.depopas_cantidad_posiciones,
                Anchura = tieneUbicacion ? ubicacion.ubilog_anchura : pasillo.depopas_ancho,
                CapacidadCubica = tieneUbicacion ? ubicacion.ubilog_capacidad_cubica : pasillo.depopas_altura_nivel * (pasillo.depopas_largo / pasillo.depopas_cantidad_posiciones) * pasillo.depopas_ancho,
                PesoMaximo = tieneUbicacion ? ubicacion.ubilog_peso_maximo : pasillo.depopas_peso_maximo,
                Estado = estado,
                PorcentajeOcupacion = porcentaje,
                EsOcupada = esOcupada,
                EsBloqueada = esBloqueada,
                EsUbicacionReal = tieneUbicacion
            };
        }

        private void CalcularTotalesZona(FrontEnd.Models.ZonaOcupacionViewModel zonaVm)
        {
            zonaVm.TotalLugares = zonaVm.Pasillos.Sum(x => x.TotalLugares);
            zonaVm.LugaresOcupados = zonaVm.Pasillos.Sum(x => x.LugaresOcupados);
            zonaVm.LugaresLibres = zonaVm.Pasillos.Sum(x => x.LugaresLibres);
            zonaVm.LugaresBloqueados = zonaVm.Pasillos.Sum(x => x.LugaresBloqueados);
            zonaVm.PorcentajeOcupacion = CalcularPorcentaje(zonaVm.LugaresOcupados, zonaVm.TotalLugares);
        }

        private decimal CalcularPorcentaje(int cantidad, int total)
        {
            if (total <= 0)
                return 0;

            return Math.Round((Convert.ToDecimal(cantidad) / Convert.ToDecimal(total)) * 100, 2);
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