using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace Resources
{
    public class Reportes
    {
        ReportViewer oRS;
        string _nombreCarpeta = "";
        public Reportes()
        {
            var _urlReporte = System.Configuration.ConfigurationManager.AppSettings["reportes_url"];
            _nombreCarpeta = System.Configuration.ConfigurationManager.AppSettings["reportes_carpeta"];

            oRS = new ReportViewer();
            oRS.ServerReport.ReportServerUrl = new Uri(_urlReporte);
            oRS.ServerReport.ReportServerCredentials = new Resources.CredencialReporte();
        }

        public System.IO.MemoryStream PDF_Stream(Microsoft.Reporting.WebForms.ReportParameter[] parametros, string nombreReporte, string nombreArchivo)
        {

            oRS.ServerReport.ReportPath = String.Concat(_nombreCarpeta, nombreReporte);
            oRS.ServerReport.SetParameters(parametros);
            Byte[] bytes;
            bytes = oRS.ServerReport.Render("PDF", null);
            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
            return ms;
        }

        public System.IO.MemoryStream PDF_Stream(List<Microsoft.Reporting.WebForms.ReportParameter> parametros, string nombreReporte, string nombreArchivo)
        {

            oRS.ServerReport.ReportPath = String.Concat(_nombreCarpeta, nombreReporte);
            oRS.ServerReport.SetParameters(parametros);
            Byte[] bytes;
            bytes = oRS.ServerReport.Render("PDF", null);
            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
            return ms;
        }

        public System.IO.MemoryStream XLS_Stream(List<Microsoft.Reporting.WebForms.ReportParameter> parametros, string nombreReporte, string nombreArchivo)
        {
            oRS.ServerReport.ReportPath = String.Concat(_nombreCarpeta, nombreReporte);
            oRS.ServerReport.SetParameters(parametros);
            Byte[] bytes;
            bytes = oRS.ServerReport.Render("Excel", null);
            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
            return ms;
        }
        
        public byte[] PDF_StreamByte(Microsoft.Reporting.WebForms.ReportParameter[] parametros, string nombreReporte, string nombreArchivo)
        {
            oRS.ServerReport.ReportPath = String.Concat(_nombreCarpeta, nombreReporte);
            oRS.ServerReport.SetParameters(parametros);
            Byte[] bytes;
            bytes = oRS.ServerReport.Render("PDF", null);
            return bytes;
        }

        public System.IO.MemoryStream PDF_MERGE(List<byte[]> sourceFiles)
        {
            Byte[] bytes = MergeFiles(sourceFiles);
            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
            return ms;
        }
        
        public static byte[] MergeFiles(List<byte[]> sourceFiles)
        {
            Document document = new Document();
            using (MemoryStream ms = new MemoryStream())
            {
                PdfCopy copy = new PdfCopy(document, ms);
                document.Open();
                int documentPageCounter = 0;

                // Iterate through all pdf documents
                for (int fileCounter = 0; fileCounter < sourceFiles.Count; fileCounter++)
                {
                    // Create pdf reader
                    PdfReader reader = new PdfReader(sourceFiles[fileCounter]);
                    int numberOfPages = reader.NumberOfPages;

                    // Iterate through all pages
                    for (int currentPageIndex = 1; currentPageIndex <= numberOfPages; currentPageIndex++)
                    {
                        documentPageCounter++;
                        PdfImportedPage importedPage = copy.GetImportedPage(reader, currentPageIndex);
                        PdfCopy.PageStamp pageStamp = copy.CreatePageStamp(importedPage);

                        // Write header
                        //ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_CENTER,
                        //    new Phrase("PDF Merger by Helvetic Solutions"), importedPage.Width / 2, importedPage.Height - 30,
                        //    importedPage.Width < importedPage.Height ? 0 : 1);

                        // Write footer
                        //ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_CENTER,
                        //    new Phrase(String.Format("Page {0}", documentPageCounter)), importedPage.Width / 2, 30,
                        //    importedPage.Width < importedPage.Height ? 0 : 1);

                        pageStamp.AlterContents();

                        copy.AddPage(importedPage);
                    }

                    copy.FreeReader(reader);
                    reader.Close();
                }

                document.Close();
                return ms.GetBuffer();
            }
        }
    }
    
}
