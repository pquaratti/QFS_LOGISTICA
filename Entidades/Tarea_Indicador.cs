using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Tarea_Indicador : EntidadBase
    {
        public int tarind_id { get; set; }

        [Display(Name = "Incidencia (%)")]
        [Required(ErrorMessage = "Campo requerido")]
        public decimal tarind_incidencia { get; set; }

        [Display(Name = "Tarea")]
        [Required(ErrorMessage = "Campo requerido")]
        public Tarea Tarea { get; set; }

        [Display(Name = "Indicador")]
        [Required(ErrorMessage = "Campo requerido")]
        public Proyecto_Indicador Indicador { get; set; }

        [Display(Name = "Cobertura")]
        [Required(ErrorMessage = "Campo requerido")]
        public decimal Cobertura { get; set; }

        public Tarea_Indicador()
        {
            this.Tarea = new Tarea();
            this.Indicador = new Proyecto_Indicador();
            this.tarind_id = 0;
        }

    }
}