using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using ExcelDataReader;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace Resources
{
    public class ExcelManager
    {

        public DataSet ExcelToDataSet(string pathFile)
        {
            try
            {
                using (var stream = System.IO.File.Open(pathFile, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        return reader.AsDataSet();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error excel manager: " + ex.Message);
            }
         
        }

        public string DatatableToExcel(DataTable dt)
        {
            var grid = new GridView();
            grid.DataSource = dt;
            grid.DataBind();
            StringWriter sw = new StringWriter();
            HtmlTextWriter htmlTextWrite = new HtmlTextWriter(sw);
            grid.RenderControl(htmlTextWrite);
            return sw.ToString();
        }

        public string ConvertDataTableToHTML(DataTable dt)
        {
            string html = "<table>";
            //add header row
            html += "<tr>";
            for (int i = 0; i < dt.Columns.Count; i++)
                html += "<td>" + dt.Columns[i].ColumnName + "</td>";
            html += "</tr>";
            //add rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                html += "<tr>";
                for (int j = 0; j < dt.Columns.Count; j++)
                    html += "<td>" + dt.Rows[i][j].ToString() + "</td>";
                html += "</tr>";
            }
            html += "</table>";
            return html;
        }


    }
}
