using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class Encuesta_Respuesta: EntidadBase
    {
        public int encres_id { get; set; }

        [Display(Name = "Encuesta")]
        [Required(ErrorMessage = "Campo requerido")]
        public Entidades.Encuesta Encuesta { get; set; }

        [Display(Name = "Valor")]
        [Required(ErrorMessage = "Campo requerido")]
        public int encres_valor { get; set; }

        [Display(Name = "Contenido")]
        [Required(ErrorMessage = "Campo requerido")]
        public string encres_contenido { get; set; }

        public Encuesta_Respuesta()
        {
            this.Encuesta = new Encuesta();

        }

    }
}
