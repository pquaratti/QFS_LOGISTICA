using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Tipo_Evento
    {
        public int evet_id { get; set; }

        [Display(Name = "Tipo de Evento")]
        [Required(ErrorMessage = "Campo requerido")]
        public string evet_contenido { get; set; }

        public bool evet_activo { get; set; }

        public Tipo_Evento()
        {
            this.evet_id = 0;
        }

    }
}
