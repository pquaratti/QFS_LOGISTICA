using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Resources
{
    public class Exports
    {

        public void ReturnXLSTextModeOnly(object collectionGeneric, HttpResponseBase resp, string fileName)
        {
            var grid = new GridView();

            grid.DataSource = collectionGeneric;
            grid.DataBind();

            if (collectionGeneric.GetType() == typeof(System.Data.DataTable))
            {
                List<string> lstColumnsNameDecimal = new List<string>();
                List<int> lstPositionDecimal = new List<int>();

                foreach (System.Data.DataColumn columDT in ((System.Data.DataTable)collectionGeneric).Columns)
                {
                    if (columDT.DataType == typeof(System.Decimal))
                    {
                        lstColumnsNameDecimal.Add(columDT.ColumnName);
                        lstPositionDecimal.Add(columDT.Ordinal);
                    }
                }

                foreach (GridViewRow rowGrid in grid.Rows)
                {
                    foreach (int posDecimal in lstPositionDecimal)
                    {
                        rowGrid.Cells[posDecimal].Attributes.Add("class", "textModeNumber");
                    }
                }
            }

            StringWriter sw = new StringWriter();
            HtmlTextWriter htmlTextWrite = new HtmlTextWriter(sw);
            grid.RenderControl(htmlTextWrite);
            string style = @"<style> .textModeNumber { mso-number-format:\@;text-align:right; } </style>";

            resp.ClearContent();
            resp.AddHeader("content-disposition", "attachment; filename=" + fileName + ".xls");
            resp.ContentType = "application/excel";
            resp.Write(style);
            resp.Write(sw.ToString());
            resp.End();
        }

    }
}
