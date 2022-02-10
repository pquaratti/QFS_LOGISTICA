using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.App
{
    public class SIS_Usuario : EntidadBase
    {
        public int usu_id { get; set; }

        [Display(Name = "Nickname")]
        [Required(ErrorMessage = "Campo requerido")]
        public string usu_nickname { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Campo requerido")]
        public string usu_password { get; set; }
        
        [Display(Name = "Last Log")]
        public DateTime usu_fec_last_logon { get; set; }

        [Display(Name = "Eliminado")]
         public DateTime usu_fec_eliminado { get; set; }

        [Display(Name = "Cambio Pass")]
        public DateTime usu_fec_pass_changed { get; set; }

        [Display(Name = "Bloqueado")]
        public DateTime usu_fec_bloqueado { get; set; }
        
        [Display(Name = "Administrador")]
        [Required(ErrorMessage = "Campo requerido")]
        public bool usu_administrador { get; set; }

        public int usu_intentos { get; set; }

        //public Entidades.Organismo Organismo { get; set; }
        
        [Display(Name = "Eliminado")]
        [Required(ErrorMessage = "Campo requerido")]
        public bool Eliminado { get; set; }

        [Display(Name = "Bloqueado")]
        [Required(ErrorMessage = "Campo requerido")]
        public bool Bloqueado { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo requerido")]
        public string usu_nombre { get; set; }

        [Display(Name = "Apellido")]
        [Required(ErrorMessage = "Campo requerido")]
        public string usu_apellido { get; set; }

        [Display(Name = "Documento")]
        [Required(ErrorMessage = "Campo requerido")]
        public string usu_documento { get; set; }

        [Display(Name = "Mail")]
        [Required(ErrorMessage = "Campo requerido")]
        public string usu_mail { get; set; }

        public DateTime usu_terminos_y_condiciones { get; set; }
        public bool AceptaTerminosYCondiciones { get; set; }

        public Entidades.App.SIS_Organizacion Organizacion { get; set; }

        [Display(Name = "Categoría")]
        public Entidades.Categoria Categoria { get; set; }

        public Entidades.App.SIS_Area Area { get; set; }

        [Display(Name = "Nro.Legajo")]
        public string usu_legajo { get; set; }

        public SIS_Usuario()
        {
            this.Categoria = new Categoria();
            this.usu_intentos = 0;
            this.Eliminado = true;
        }
        
    }
}
