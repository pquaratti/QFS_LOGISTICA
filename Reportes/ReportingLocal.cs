using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportManager
{

    public class ReportingLocal
    {
        public byte[] PDF_StreamByte(string ReportNameRDLC, List<Entitites.ReportParameterCustom> datatablesSources)
        {
            //reportes_carpeta_local
            string reportPathFiles = System.Configuration.ConfigurationManager.AppSettings["reportes_carpeta_local"];
            // Variables
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            // Setup the report viewer object and get the array of bytes
            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;

            if (reportPathFiles.Length.Equals(0))
            {
                viewer.LocalReport.ReportPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Reportes"), ReportNameRDLC);
            }
            else
                viewer.LocalReport.ReportPath = reportPathFiles + ReportNameRDLC;
            
            foreach (Entitites.ReportParameterCustom item in datatablesSources)
            {
                ReportDataSource rds = new ReportDataSource(item.DataSourceName,item.DataSourceObject);
                viewer.LocalReport.DataSources.Add(rds);
            }

            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

            return bytes;
        }

    }
}
