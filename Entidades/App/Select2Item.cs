using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.App
{
    public class Select2Item
    {
        public Select2Item() { }
        public Select2Item(string pID, string pText)
        {
            id = pID;
            text = pText;
        }
        public string id { get; set; }
        public string text { get; set; }
        
    }
}
