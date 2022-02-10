using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class Encuesta_Usuario : EntidadBase
    {
        public int encusu_id { get; set; }

        [Display(Name = "Encuesta")]
        [Required(ErrorMessage = "Campo requerido")]
        public Entidades.Encuesta Encuesta { get; set; }

        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "Campo requerido")]
        public Entidades.App.SIS_Usuario Usuario { get; set; }

        [Display(Name = "Fec.Ini")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Campo requerido")]
        public DateTime encusu_fec_ini { get; set; }

        [Display(Name = "Fec.Fin")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Campo requerido")]
        public DateTime encusu_fec_fin { get; set; }
        public int EstadoActual { get; set; }

        public Encuesta_Usuario()
        {
            this.Encuesta = new Encuesta();
            this.Usuario = new App.SIS_Usuario();
        }

    }
}
