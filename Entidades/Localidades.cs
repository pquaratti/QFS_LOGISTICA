using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Localidades : EntidadBase
    {
        public int loc_id { get; set; }

        [Display(Name = "Localidad")]
        [Required(ErrorMessage = "Campo requerido")]
        public string loc_nombre { get; set; }

        [Display(Name = "Código")]
        [Required(ErrorMessage = "Campo requerido")]
        public string loc_codigo { get; set; }

        [Display(Name = "Activa")]
        [Required(ErrorMessage = "Campo requerido")]
        public Boolean loc_activo { get; set; }


        [Display(Name = "Departamento")]
        [Required(ErrorMessage = "Campo requerido")]
        public Entidades.Departamento Departamento { get; set; }

        [Display(Name = "Abreviatura")]
        [Required(ErrorMessage = "Campo requerido")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "La longitud debe ser entre (1-10) caracteres")]
        public string loc_nombre_abreviado { get; set; }



        public Localidades()
        {
           
            this.Departamento = new Departamento();
            this.loc_activo = true;
            //this.Centro_Medico = new Centro_Medico(); 
        }

    }
}
