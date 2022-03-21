using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.App
{
    public class SIS_Organizacion : EntidadBase
    {
        [Key]
        public int org_id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo requerido")]
        public string org_nombre { get; set; }

        [Display(Name = "Abreviatura")]
        [Required(ErrorMessage = "Campo requerido")]
        public string org_abreviatura { get; set; }

        [Display(Name = "Mail")]
        [Required(ErrorMessage = "Campo requerido")]
        public string org_mail { get; set; }

        [Display(Name = "Activo")]
        [Required(ErrorMessage = "Campo requerido")]
        public bool org_activo { get; set; }

        public SIS_Organizacion()
        {
            this.org_id = 0;
            this.org_nombre = "";
            this.org_mail = "";
            this.org_activo = true;
        }
    }
}
