using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Categoria_Colaborador_Evolucion : EntidadBase
    {
        public int cce_id { get; set; }


        [Display(Name = "Documento")]
        [Required(ErrorMessage = "Campo requerido")]
        public string cce_doc { get; set; }

        [Display(Name = "Categoría")]
        public Categoria Categoria { get; set; }
        
        [Display(Name = "Colaborador")]
        public Entidades.App.SIS_Usuario Colaborador { get; set; }
        
        [Display(Name = "Fecha")]
        [Required(ErrorMessage = "Campo requerido")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime cce_fecha { get; set; }

        public Categoria_Colaborador_Evolucion()
        {
            this.Categoria = new Categoria();
            this.Colaborador = new App.SIS_Usuario();
            this.cce_id = 0;
        }
    }
}
