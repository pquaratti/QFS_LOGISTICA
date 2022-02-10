using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entidades.App;

namespace FrontEnd.Controllers
{
    [Filters.VerificarToken]
    [Filters.VerificarModule]
    public class ModuloController : ControllerBase<Entidades.App.SIS_Modulo>
    {
        public override JsonResult Active(string id)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public override ActionResult AddOrEdit(string id = "")
        {
            Entidades.App.SIS_Modulo obj;
            
            if (id.Length == 0)
            {
                obj = new SIS_Modulo();
                obj.IdEncriptado = Negocio.App.Security.EncriptarID("0");
            }
            else
            {
                Negocio.App.SIS_Modulos negocio = new Negocio.App.SIS_Modulos(GetToken());
                obj = negocio.ObtenerPorIDEncriptado(id);
            }
            
            return View(obj);
        }

        [HttpPost]
        public override JsonResult Delete(string id)
        {
            id = Negocio.App.Security.DesencriptarID(id);
            Negocio.App.SIS_Modulos negocio = new Negocio.App.SIS_Modulos(GetToken());
            
            Entidades.App.ObjectMessage oM = negocio.DeleteLogico(Convert.ToInt32(GetToken().UserID), Convert.ToInt32(id));
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public override ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public override JsonResult ListarSelect(string filterID)
        {
            Negocio.App.SIS_Modulos negocio = new Negocio.App.SIS_Modulos(GetToken());

            List<Entidades.App.DLLObject> items = negocio.ListarDLL(true);
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        
        public override JsonResult LoadTableAjax()
        {
            Entidades.App.DatatableJS dtAjax = GetListTableAjax<Entidades.App.SIS_Modulo>(new Negocio.App.SIS_Modulos(GetToken()));
            return Json(new { draw = dtAjax.Draw, recordsFiltered = dtAjax.totalRecords, recordsTotal = dtAjax.totalRecords, data = dtAjax.dataReturn }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public override JsonResult Save(SIS_Modulo obj)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            var esNuevo = false;
            try
            {
                obj.mod_id = Convert.ToInt32(Negocio.App.Security.DesencriptarID(obj.IdEncriptado));

                if (obj.mod_id.Equals(0))
                    esNuevo = true;
                
                Negocio.App.SIS_Modulos negocio = new Negocio.App.SIS_Modulos(GetToken());
                negocio.Save(obj);

                oM.Success = true;
                oM.RedirectNew = esNuevo;
                oM.urlRedirect = Url.Action("AddOrEdit", "Modulo", new { id = obj.IdEncriptado });


            }
            catch (Exception ex)
            {

                throw;
            }
            
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }
    }
}