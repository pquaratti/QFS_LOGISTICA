using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Proyecto_Indicador : EntidadBase
    {
        public int pryind_id { get; set; }

        [Display(Name = "Proyecto")]
        [Required(ErrorMessage = "Campo requerido")]
        public Proyecto Proyecto { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo requerido")]
        public string pryind_nombre { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "Campo requerido")]
        public string pryind_descripcion { get; set; }
       
        [Display(Name = "Valor Base")]
        [Required(ErrorMessage = "Campo requerido")]
        public decimal pryind_valor_base { get; set; }

        [Display(Name = "Valor Meta")]
        [Required(ErrorMessage = "Campo requerido")]
        public decimal pryind_valor_meta { get; set; }

        public bool pryind_cerrado { get; set; }

        [Display(Name = "Porc.Evol")]
        public decimal pryind_porc_evolucion { get; set; }
        public Proyecto_Indicador()
        {
            pryind_cerrado = false;
            this.Proyecto = new Proyecto();
        }
    }
}
