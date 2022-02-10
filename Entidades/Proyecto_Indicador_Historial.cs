using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Proyecto_Indicador_Historial : EntidadBase
    {
        public int pryindhis_id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo requerido")]
        public string pryindhis_nombre { get; set; }

        [Display(Name = "Indicador")]
        [Required(ErrorMessage = "Campo requerido")]
        public Proyecto_Indicador Indicador { get; set; }
       
        [Display(Name = "Valor")]
        [Required(ErrorMessage = "Campo requerido")]
        public decimal pryindhis_valor { get; set; }

        public Proyecto_Indicador_Historial()
        {
            this.Indicador = new Proyecto_Indicador();
        }
    }
}
