using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Colaborador_Skill : EntidadBase
    {
        public int colskill_id { get; set; }

        [Display(Name = "Colaborador")]
        [Required(ErrorMessage = "Campo requerido")]
        public App.SIS_Usuario Colaborador { get; set; }

        [Display(Name = "Skill")]
        [Required(ErrorMessage = "Campo requerido")]
        public Skill Skill { get; set; }

        [Display(Name = "Puntaje")]
        [Required(ErrorMessage = "Campo requerido")]
        public int colskill_puntaje { get; set; }


        public Colaborador_Skill()
        {
            this.Colaborador = new App.SIS_Usuario();
            this.Skill = new Skill();
        }
    }
}
