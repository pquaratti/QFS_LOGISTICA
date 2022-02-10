using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Colaborador_Skill_Evolucion : EntidadBase
    {
        public int cse_id { get; set; }

        [Display(Name = "Colaborador")]
        [Required(ErrorMessage = "Campo requerido")]
        public Entidades.App.SIS_Usuario Colaborador { get; set; }

        [Display(Name = "Skill")]
        [Required(ErrorMessage = "Campo requerido")]
        public Entidades.Skill Skill { get; set; }

        [Display(Name = "Valor")]
        [Required(ErrorMessage = "Campo requerido")]
        public int cse_valor { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "Campo requerido")]
        public string cse_descripcion { get; set; }

        [Display(Name = "Fecha")]
        [Required(ErrorMessage = "Campo requerido")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime cse_fecha { get; set; }


        public Colaborador_Skill_Evolucion()
        {
            this.cse_id = 0;
            this.Skill = new Skill();
            this.Colaborador = new App.SIS_Usuario();
        }
    }
}
