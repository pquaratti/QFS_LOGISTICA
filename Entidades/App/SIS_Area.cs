using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.App
{
    public class SIS_Area : EntidadBase
    {

        [KeyAttribute]
        public int area_id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo requerido")]
        public string area_nombre { get; set; }

        [Display(Name = "Abreviatura")]
        [Required(ErrorMessage = "Campo requerido")]
        public string area_abreviatura { get; set; }
        public Entidades.App.SIS_Area Padre { get; set; }
        public Entidades.App.SIS_Organizacion Organizacion { get; set; }

        [Display(Name = "Activa")]
        [Required(ErrorMessage = "Campo requerido")]
        public bool area_activo { get; set; }

        public SIS_Area()
        {
            this.area_id = 0;
        }

    }
}
