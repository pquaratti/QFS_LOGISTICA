using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.App
{
    public class SIS_Modulo : EntidadBase
    {
        public int mod_id { get; set; }

        [Required(ErrorMessage = "* Completar este campo")]
        [Display(Name = "Nombre")]
        public string mod_nombre { get; set; }

        [Required(ErrorMessage = "* Completar este campo")]
        [Display(Name = "Descripcion")]
        public string mod_descripcion { get; set; }
        public bool mod_activo { get; set; }

        public List<Entidades.App.SIS_Perfil> Perfiles { get; set; }

        public SIS_Modulo()
        {
            this.Perfiles = new List<SIS_Perfil>();
        }
        
    }
}
