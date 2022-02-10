using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Categoria : EntidadBase
    {
        public int cat_id { get; set; }

        [Display(Name = "Organizacion")]
        [Required(ErrorMessage = "Campo requerido")]
        public Entidades.App.SIS_Organizacion Organizacion { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo requerido")]
        public string cat_nombre { get; set; }

        [Display(Name = "Descripcion")]
        [Required(ErrorMessage = "Campo requerido")]
        public string cat_descripcion { get; set; }

        [Display(Name = "Estado")]
        public bool cat_activo { get; set; }

        public Categoria()
        {
            this.cat_id = 0;
        }
    }
}
