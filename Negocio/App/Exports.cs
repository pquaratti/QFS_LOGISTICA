using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;

namespace Negocio.App
{
    public class Exports
    {

        public Exports()
        {

        }

        //public void ReturnXLS(object collectionGeneric, HttpResponseBase resp, string fileName)
        //{
        //    var grid = new GridView();

        //    grid.DataSource = collectionGeneric;
        //    grid.DataBind();

        //    StringWriter sw = new StringWriter();
        //    HtmlTextWriter htmlTextWrite = new HtmlTextWriter(sw);
        //    grid.RenderControl(htmlTextWrite);

        //    resp.ClearContent();
        //    resp.AddHeader("content-disposition", "attachment; filename=" + fileName + ".xls");
        //    resp.ContentType = "application/excel";
        //    resp.Write(sw.ToString());
        //    resp.End();
        //}


    }
}
