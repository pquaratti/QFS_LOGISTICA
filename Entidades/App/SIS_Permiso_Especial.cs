using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.App
{
    public class SIS_Permiso_Especial : EntidadBase
    {
        public int pee_id { get; set; }
        public string pee_nombre { get; set; }
        public string pee_descripcion { get; set; }
        public bool pee_activo { get; set; }
        public SIS_Permiso_Especial()
        {
            this.pee_id = 0;
        }

    }
}
