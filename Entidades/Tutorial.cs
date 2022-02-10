using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Tutorial
    {
        public int tut_id { get; set; }

        [Display(Name = "Título")]
        [Required(ErrorMessage = "Campo requerido")]
        public string tut_titulo { get; set; }

        [Display(Name = "Tutorial")]
        [Required(ErrorMessage = "Campo requerido")]
        public string tut_archivo { get; set; }

        [Display(Name = "Acción")]
        public Entidades.App.SIS_Accion Accion { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "Campo requerido")]
        public string tut_descrip { get; set; }

        [Display(Name = "Ícono")]
        [Required(ErrorMessage = "Campo requerido")]
        public string tut_icono { get; set; }

        public bool tut_activo { get; set; }

        public Tutorial()
        {
            this.Accion = new App.SIS_Accion();
            this.tut_id = 0;
        }

    }
}
