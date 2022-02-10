using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Tipo_Encuesta : EntidadBase
    {

        [KeyAttribute] 
        public int tenc_id { get; set; }

        [Display(Name = "Tipo de Encuesta")]
        [Required(ErrorMessage = "Campo requerido")]
        public string tenc_contenido { get; set; }

        public bool tenc_activo { get; set; }

        public Tipo_Encuesta()
        {
            this.tenc_id = 0;
        }

    }
}
