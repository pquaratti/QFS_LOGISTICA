using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Marco_Estrategico
    {
        public int marc_id { get; set; }
        public Entidades.App.SIS_Organizacion Organizacion { get; set; }
        public int marc_per_ini { get; set; }
        public int marc_per_fin { get; set; }
        public string marc_nombre { get; set; }
        public string marc_descripcion { get; set; }

        public Marco_Estrategico()
        {
            this.marc_id = 0;
        }

    }
}
