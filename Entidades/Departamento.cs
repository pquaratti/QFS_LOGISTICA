using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class Departamento : EntidadBase
    {

        public int dto_id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo requerido")]
        public string dto_nombre { get; set; }

        [Display(Name = "Provincia")]
        [Required(ErrorMessage = "Campo requerido")]
        public SIS_Provincia Provincia { get; set; }

        [Display(Name = "Código de Departamento")]
        [Required(ErrorMessage = "Campo requerido")]
        public string dto_codigo { get; set; }

        public bool dto_activo { get; set; }

        public Departamento()
        {
            this.dto_id = 0;
            this.dto_activo = true;
            Provincia = new SIS_Provincia();
        }
    }
      
}