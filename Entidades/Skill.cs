using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Skill : EntidadBase
    {
        public int skill_id { get; set; }

        [Display(Name = "Organizacion")]
        [Required(ErrorMessage = "Campo requerido")]
        public Entidades.App.SIS_Organizacion Organizacion { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo requerido")]
        public string skill_nombre { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "Campo requerido")]
        public string skill_descripcion { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "Campo requerido")]
        public Boolean skill_activo { get; set; }

        [Display(Name = "Abreviatura")]
        [Required(ErrorMessage = "Campo requerido")]
        public string skill_nombre_abreviado { get; set; }
        
        public Skill()
        {
            this.skill_activo = true;
        }

    }
}
