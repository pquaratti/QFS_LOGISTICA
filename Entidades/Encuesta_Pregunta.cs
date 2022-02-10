using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class Encuesta_Pregunta :EntidadBase
    {
        public int encpreg_id { get; set; }

        [Display(Name = "Cabecera")]
        [Required(ErrorMessage = "Campo requerido")]
        public Entidades.Encuesta Encuesta { get; set; }

        [Display(Name = "Pregunta")]
        [Required(ErrorMessage = "Campo requerido")]
        public string encpreg_contenido { get; set; }

        [Display(Name = "Número")]
        [Required(ErrorMessage = "Campo requerido")]
        public int encpreg_numero { get; set; }

        public Encuesta_Pregunta()
        {
            this.Encuesta = new Encuesta();
        }

    }
}
