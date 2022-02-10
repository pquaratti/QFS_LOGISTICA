using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Proyecto_Objetivo_Indicador : EntidadBase
    {
        public int poi_id { get; set; }

        [Display(Name = "Indicador")]
        [Required(ErrorMessage = "Campo requerido")]
        public Proyecto_Indicador Indicador { get; set; }

        [Display(Name = "Objetivo")]
        [Required(ErrorMessage = "Campo requerido")]
        public Proyecto_Objetivo Objetivo { get; set; }
        
        
        [Display(Name = "Total")]
        [Required(ErrorMessage = "Campo requerido")]
        public decimal pit_porcentaje { get; set; }

        public Proyecto_Objetivo_Indicador()
        {
            this.Indicador = new Proyecto_Indicador();
            this.Objetivo = new Proyecto_Objetivo();
        }
    }
}
