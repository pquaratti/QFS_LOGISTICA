using System;
using System.ComponentModel.DataAnnotations;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Sexo : EntidadBase 
    {
        public int sex_id { get; set; }

        [Display(Name = "Sexo")]
        [Required(ErrorMessage = "Campo requerido")]
        public string sex_nombre { get; set; }
        
        [Display(Name = "Abreviatura")]
        [Required(ErrorMessage = "Campo requerido")]
        public string sex_abreviatura { get; set; }
        
        public Sexo()
        {
            this.sex_id = 0;
        }
    }
}
