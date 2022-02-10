using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entidades.App
{
    public class SIS_Perfil : EntidadBase
    {
        public int prf_id { get; set; }
        
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo requerido")]
        public string prf_nombre { get; set; }
        
        [Display(Name = "Accion")]
        [Required(ErrorMessage = "Campo requerido")]
        public List<SIS_Accion> Acciones { get; set;}

        [Display(Name = "Activo")]
        [Required(ErrorMessage = "Campo requerido")]
        public bool prf_activo { get; set; }

        public int prf_mod_id { get; set; }

        public SIS_Perfil()
        {
            this.Acciones = new List<SIS_Accion>();
            this.prf_activo = true;
        }
        
    }
}
