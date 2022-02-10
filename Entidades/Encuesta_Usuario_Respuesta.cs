using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class Encuesta_Usuario_Respuesta: EntidadBase
    {
        public int eur_id { get; set; }

        [Display(Name = "Pregunta")]
        [Required(ErrorMessage = "Campo requerido")]
        public Entidades.Encuesta_Pregunta Pregunta { get; set; }

        [Display(Name = "Respuesta")]
        [Required(ErrorMessage = "Campo requerido")]
        public Entidades.Encuesta_Respuesta Respuesta { get; set; }

        [Display(Name = "Intento")]
        [Required(ErrorMessage = "Campo requerido")]
        public Entidades.Encuesta_Usuario Intento { get; set; }


        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Campo requerido")]
        public DateTime eur_fecha { get; set; }

        public Encuesta_Usuario_Respuesta()
        {
            this.eur_id = 0;
            this.Pregunta = new Encuesta_Pregunta();
            this.Respuesta = new Encuesta_Respuesta();
            this.Intento = new Encuesta_Usuario();
        }

    }
}
