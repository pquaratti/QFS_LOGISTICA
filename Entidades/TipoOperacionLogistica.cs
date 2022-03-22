using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class TipoOperacionLogistica : EntidadBase
    {

        [KeyAttribute]
        public int topelog_id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo requerido")]
        public string topelog_nombre { get; set; }

        [Display(Name = "Observaciones")]
        [Required(ErrorMessage = "Campo requerido")]
        public string topelog_observaciones { get; set; }

        [Display(Name = "Activo")]
        [Required(ErrorMessage = "Campo requerido")]
        public bool topelog_activo { get; set; }

        public TipoOperacionLogistica()
        {
            this.topelog_activo = true;
            this.topelog_id = 0;

        }

    }

  
}
