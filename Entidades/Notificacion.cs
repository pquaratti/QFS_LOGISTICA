using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Notificacion
    {
        public int not_id { get; set; }

        [Display(Name = "Texto")]
        [Required(ErrorMessage = "Campo requerido")]
        public string not_texto { get; set; }

        public bool not_visto { get; set; }

    }
}
