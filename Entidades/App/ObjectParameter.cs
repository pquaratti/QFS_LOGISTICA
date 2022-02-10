using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.App
{
    public class ObjectParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public ObjectParameter() { }

        public ObjectParameter(string pName, object pValue)
        {
            this.Name = pName;
            this.Value = pValue;
        }

    }
}
