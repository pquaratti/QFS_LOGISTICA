using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.App
{
    public class SIS_Localidad : EntidadBase
    {
        [Key]
        public int loc_id { get; set; }

        [Display(Name = "Localidad")]
        [Required(ErrorMessage = "Campo requerido")]
        public string loc_nombre { get; set; }


        [Display(Name = "Provincia")]
        [Required(ErrorMessage = "Campo requerido")]
        public Entidades.App.SIS_Provincia Provincia { get; set; }

        public SIS_Localidad()
        {
            this.Provincia = new SIS_Provincia();

        }

    }
}
