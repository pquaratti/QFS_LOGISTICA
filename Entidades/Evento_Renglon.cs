using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class Evento_Renglon: EntidadBase
    {

        public int ever_id { get; set; }

        [Display(Name = "Invitación a Evento")]
        [Required(ErrorMessage = "Campo requerido")]
        public Entidades.Evento Evento { get; set; }

        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "Campo requerido")]
        public Entidades.App.SIS_Usuario Usuario { get; set; }

        [Display(Name = "Asistencia")]
        [Required(ErrorMessage = "Campo requerido")]
        public bool ever_asist { get; set; }

        public Evento_Renglon()
        {
            this.ever_id = 0;
            this.Evento = new Evento();
            this.Usuario = new App.SIS_Usuario();
        }

    }
}
