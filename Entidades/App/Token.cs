using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.App
{
    public class Token
    {
        public string ID { get; set; }
        public string UserID { get; set; }
        public DateTime CreateAt { get; set; }
        public string IPAddress { get; set; }
        public string UserName { get; set; }
        public string ModuloID { get; set; }
        public bool Administrador { get; set; }
        public string NavigatorID { get; set; }
        public bool AceptaTerminosYCondiciones { get; set; }
        public string OrganizacionID { get; set; }
        public string AreaID { get; set; }

        public Token()
        {
            this.NavigatorID = "";
        }

        public bool FilterBusinessToken { get; set; }

        public Token EnableTokenFilter()
        {
            this.FilterBusinessToken = true;
            return this;
        }

        public Token DisabledTokenFilter()
        {
            this.FilterBusinessToken = false;
            return this;
        }

    }
}
