using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Provincia : EntidadBase
    {
        public int prv_id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo requerido")]
        public string prv_nombre { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "Campo requerido")]
        public Boolean prv_activo { get; set; }

        [Display(Name = "Abreviatura")]
        [Required(ErrorMessage = "Campo requerido")]
        public string prv_nombre_abreviado { get; set; }
        
        public Provincia()
        {
            this.prv_activo = true;
        }

    }
}
