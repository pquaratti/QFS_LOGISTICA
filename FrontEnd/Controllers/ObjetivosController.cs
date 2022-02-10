using Entidades.App;
using Newtonsoft.Json;
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
    public class ObjetivosController : ControllerBaseV2
    {

        #region Gestión de Objetivos

        [HttpGet]
        public ActionResult Configuracion()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PartialGridDataObjetivosMetas(string proyectoID)
        {
            Entidades.App.Token oToken = GetToken();
            Negocio.ProyectosObjetivos negocioPRY = new Negocio.ProyectosObjetivos(oToken);

            List<Entidades.Proyecto_Objetivo> objetivos = negocioPRY.ListarPorProyecto(Convert.ToInt32(proyectoID));

            if (objetivos.Count > 0)
            {
                string _partialViewName = "_GridDataObjetivoMetas";
                return PartialView(_partialViewName, objetivos);
            }
            else
            {
                Entidades.App.ObjectMessage oM = new ObjectMessage();
                oM.Message = "No hay objetivos que mostrar, para el filtro seleccionado.";
                return PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", oM);
            }
        }

        [HttpGet]
        public ActionResult PartialInformeIndicadoresObjetivo(string ID)
        {
            List<Entidades.Proyecto_Indicador> lst = new List<Entidades.Proyecto_Indicador>();

            Negocio.ProyectosObjetivosIndicadores ne = new Negocio.ProyectosObjetivosIndicadores(GetToken());
            lst = ne.ListarIndicadoresPorObjetivo(Convert.ToInt32(Negocio.App.Security.DesencriptarID(ID)));
            
            ViewBag.ID = "000" + Negocio.App.Security.DesencriptarID(ID);
            if (lst.Count > 0)
            {

                string _partialViewName = "_GridInfoObjetivoIndicadores";
                return PartialView(_partialViewName, lst);
            }
            else
            {
                Entidades.App.ObjectMessage oM = new ObjectMessage();
                oM.Message = "No hay indicadores en el objetivo.";
                return PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", oM);
            }
        }

        #endregion

        #region Gestión de Indicadores

        [HttpGet]
        public ActionResult PartialModalVincularIndicador(string cabeceraID, string detalleID, string subdetalleID)
        {
            Entidades.Proyecto_Objetivo_Indicador item = new Entidades.Proyecto_Objetivo_Indicador();
            item.Objetivo = new Negocio.ProyectosObjetivos(GetToken()).ObtenerPorID(cabeceraID);

            List<Entidades.Proyecto_Indicador> lstIndicadores = new Negocio.ProyectosIndicadores(GetToken()).ListarPorProyecto(item.Objetivo.ProyectoVinculado.proy_id);

            lstIndicadores.Add(new Entidades.Proyecto_Indicador() { pryind_id = -1, IdEncriptado = Negocio.App.Security.EncriptarID("-1"), descripcion_combo = "Crear nuevo" });
            lstIndicadores.Add(new Entidades.Proyecto_Indicador() { pryind_id = 0, IdEncriptado = Negocio.App.Security.EncriptarID("0"), descripcion_combo = "Seleccione" });

            ViewBag.Indicadores = lstIndicadores.OrderBy(o => o.pryind_id);

            string _partialViewName = "_ModalVincularIndicador";
            return PartialView(_partialViewName, item);
        }

        [HttpPost]
        public JsonResult SaveVinculacionIndicador(Entidades.Proyecto_Objetivo_Indicador obj)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();

            if (obj.Indicador.pryind_id.Equals(-1))
            {
                obj.Indicador.pryind_id = 0;
                obj.Indicador.Proyecto.proy_id = Convert.ToInt32(Negocio.App.Security.DesencriptarID(obj.Objetivo.ProyectoVinculado.IdEncriptado));
                oM = new Negocio.ProyectosIndicadores(GetToken()).Save(obj.Indicador);
            }

            obj.Objetivo.pryobj_id = Convert.ToInt32(Negocio.App.Security.DesencriptarID(obj.Objetivo.IdEncriptado));
            obj.Objetivo.ProyectoVinculado.proy_id = Convert.ToInt32(Negocio.App.Security.DesencriptarID(obj.Objetivo.ProyectoVinculado.IdEncriptado));

            oM = new Negocio.ProyectosObjetivosIndicadores(GetToken()).Save(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DesvincularIndicador(string BorrarID)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            Negocio.ProyectosObjetivosIndicadores n = new Negocio.ProyectosObjetivosIndicadores(GetToken());
            string usu = Filters.VerificarToken.ConsultarToken().UserID;
            oM = n.Delete(Convert.ToInt32(Negocio.App.Security.DesencriptarID(BorrarID)));
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }



        #endregion

    }
}