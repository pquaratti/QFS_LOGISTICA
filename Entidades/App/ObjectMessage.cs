using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.App
{
    public class ObjectMessage
    {
        public bool TokenExist { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public object ObjectRelation { get; set; }
        public bool RedirectNew { get; set; }
        public string urlRedirect { get; set; }

        public ObjectMessage()
        {
            this.TokenExist = true;
            this.RedirectNew = false;
            this.urlRedirect = "";
        }

    }
}
