using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrontEnd.Controllers
{
    public abstract class ControllerBaseV2 : Controller
    {
        public Negocio.App.Security securityBusiness = new Negocio.App.Security();

        public Entidades.App.Token GetToken()
        {
            Entidades.App.Token oToken = Filters.VerificarToken.ConsultarToken();
            return oToken;
        }
        
        public Entidades.App.DatatableJS GetListTableAjax<B>(Negocio.NegocioBase<B> negocioOriginal)
        {
            List<B> lst = new List<B>();

            Entidades.App.DatatableJS datatableFilters = new Entidades.App.DatatableJS();
           
            datatableFilters.Draw = Request.Form.GetValues("draw").FirstOrDefault();
            datatableFilters.Start = Request.Form.GetValues("start").FirstOrDefault();
            datatableFilters.Lenght = Request.Form.GetValues("length").FirstOrDefault();
            datatableFilters.SearchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
            datatableFilters.MostrarTodos = Convert.ToBoolean(Request.Form.GetValues("mostrar_todos").FirstOrDefault());
            datatableFilters.sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            datatableFilters.direccion = Request["order[0][dir]"];
            datatableFilters.pageSize = datatableFilters.Lenght != null ? Convert.ToInt32(datatableFilters.Lenght) : 0;
            datatableFilters.skip = datatableFilters.Start != null ? Convert.ToInt32(datatableFilters.Start) : 0;
            datatableFilters.totalRecords = 0;
            
            lst = negocioOriginal.ListarParaTableAjax(datatableFilters);
            datatableFilters.totalRecords = lst.Count();
            var data = lst.Skip(datatableFilters.skip).Take(datatableFilters.pageSize).ToList();

            datatableFilters.dataReturn = data;

            return datatableFilters;
        }

        public virtual JsonResult GetSelect2Items(string q)
        {
            return null;
        }

        public virtual JsonResult LoadTableAjax()
        {
            return null;
        }

    }
}