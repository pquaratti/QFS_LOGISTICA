using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Resources
{
    public class CredencialReporte : Microsoft.Reporting.WebForms.IReportServerCredentials
    {
        public WindowsIdentity ImpersonationUser
        {
            get
            {
                return null;
            }
        }

        public ICredentials NetworkCredentials
        {
            get
            {
                var _usuario = System.Configuration.ConfigurationManager.AppSettings["reportes_usuario"];
                var _password = System.Configuration.ConfigurationManager.AppSettings["reportes_password"];
                var _dominio = System.Configuration.ConfigurationManager.AppSettings["reportes_dominio"];

                return new System.Net.NetworkCredential(_usuario, _password, _dominio);
            }
        }

        public bool GetFormsCredentials(out Cookie authCookie, out string userName, out string password, out string authority)
        {
            userName = "";
            password = "";
            authority = "";
            authCookie = null;
            return false;
        }
    }
}
