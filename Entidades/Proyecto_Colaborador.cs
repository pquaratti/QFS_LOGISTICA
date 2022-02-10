using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Proyecto_Colaborador : EntidadBase
    {
        public int prycolab_id { get; set; }

        [Display(Name = "Proyecto")]
        [Required(ErrorMessage = "Campo requerido")]
        public Proyecto Proyecto { get; set; }

        [Display(Name = "Colaborador")]
        [Required(ErrorMessage = "Campo requerido")]
        public App.SIS_Usuario Legajo { get; set; }

        public Proyecto_Colaborador()
        {
            this.Proyecto = new Proyecto();
            this.Legajo = new App.SIS_Usuario();
        }
    }
}
