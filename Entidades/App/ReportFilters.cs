using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.App
{
    public class ReportFilters
    {
        public string ID { get; set; }
        public string fuerzaID { get; set; }
        public string distritoID { get; set; }
        public string subdistritoID { get; set; }
        public string tipoeleccionID { get; set; }
        public string eleccionID { get; set; }

        public string customSeedKey { get; set; }

        public ReportFilters()
        {

        }

    }
}
