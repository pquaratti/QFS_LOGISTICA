using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class Deposito : EntidadBase
    {

        [KeyAttribute]
        public int depo_id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo requerido")]
        public string depo_nombre { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "Campo requerido")]
        public string depo_descripcion { get; set; }

        [Display(Name = "Código")]
        [Required(ErrorMessage = "Campo requerido")]
        public string depo_codigo { get; set; }

        [KeyRelation]
        [Display(Name = "Planta")]
        [Required(ErrorMessage = "Campo requerido")]
        public Planta Planta { get; set; }

        [Display(Name = "Activo")]
        [Required(ErrorMessage = "Campo requerido")]
        public bool depo_activo { get; set; }

        public Deposito()
        {
            this.depo_activo = true;
            this.depo_id = 0;
            this.Planta = new Planta();

        }

    }

  
}
