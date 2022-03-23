using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class ZonaLogistica : EntidadBase
    {

        [KeyAttribute]
        public int zonlog_id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo requerido")]
        public string zonlog_nombre { get; set; }

        [Display(Name = "Descipción")]
        [Required(ErrorMessage = "Campo requerido")]
        public string zonlog_descripcion { get; set; }
      
        [Display(Name = "Código")]
        [Required(ErrorMessage = "Campo requerido")]
        public string zonlog_codigo{ get; set; }

        [Display(Name = "Activo")]
        [Required(ErrorMessage = "Campo requerido")]
        public bool zonlog_activo { get; set; }

        public ZonaLogistica()
        {
            this.zonlog_activo = true;
            this.zonlog_id = 0;

        }

    }

  
}
