using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.App
{
    public class SIS_Provincia : EntidadBase
    {
        public int prv_id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo requerido")]
        public string prv_nombre { get; set; }



        public SIS_Provincia()
        {
            this.prv_id = 0;
        }

    }
}
