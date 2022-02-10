using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Tarea_Colaborador : EntidadBase
    {
        public int tarcol_id { get; set; }

        [Display(Name = "Legajo")]
        [Required(ErrorMessage = "Campo requerido")]
        public App.SIS_Usuario Legajo { get; set; }

        [Display(Name = "Tarea")]
        [Required(ErrorMessage = "Campo requerido")]
        public Tarea Tarea { get; set; }
        
        public Tarea_Colaborador()
        {
            this.Legajo = new App.SIS_Usuario();
            this.Tarea = new Tarea();
        }
    }
}
