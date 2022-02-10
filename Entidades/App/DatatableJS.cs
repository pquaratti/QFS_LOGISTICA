using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.App
{
    public class DatatableJS
    {
        public string Draw { get; set; }
        public string Start { get; set; }
        public string Lenght { get; set; }
        public string SearchValue { get; set; }
        public bool MostrarTodos { get; set; }
        public string sortColumnName { get; set; }
        public string direccion { get; set; }
        public int pageSize { get; set; }
        public int skip { get; set; }
        public int totalRecords { get; set; }

        public object dataReturn { get; set; }

    }
}
