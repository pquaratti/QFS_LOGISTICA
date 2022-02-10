using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.App
{
    public class SIS_Usuario_Login : EntidadBase
    {
        public int usl_id { get; set; }

        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "Campo requerido")]
        public Entidades.App.SIS_Usuario Usuario;

        [Display(Name = "Inicio del Log")]
        public DateTime usl_fec_ini { get; set; }

        [Display(Name = "Fin del Log")]
        public DateTime usl_fec_fin { get; set; }

        [Display(Name = "IPv4")]
        [Required(ErrorMessage = "Campo requerido")]
        public string usl_ipv4 { get; set; }

        [Display(Name = "Navigator")]
        [Required(ErrorMessage = "Campo requerido")]
        public string usl_navigator { get; set; }
  
        public SIS_Usuario_Login()
        {
            this.usl_id = 0;
            this.Usuario = new SIS_Usuario(); 
        }
        
    }
}
