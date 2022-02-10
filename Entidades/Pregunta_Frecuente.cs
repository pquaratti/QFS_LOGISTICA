using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Pregunta_Frecuente
    {
        public int pgf_id { get; set; }

        [Display(Name = "Título")]
        [Required(ErrorMessage = "Campo requerido")]
        public string pgf_titulo { get; set; }

        [Display(Name = "Contenido")]
        [Required(ErrorMessage = "Campo requerido")]
        public string pgf_contenido { get; set; }

        [Display(Name = "Categoría")]
        [Required(ErrorMessage = "Campo requerido")]
        public Entidades.Pregunta_Frecuente_Categoria Categoria { get; set; }

        [Display(Name = "Usuario Creador")]
        [Required(ErrorMessage = "Campo requerido")]
        public int pgf_usu_id_creador { get; set; }

        [Display(Name = "Usuario Modificador")]
        [Required(ErrorMessage = "Campo requerido")]
        public int pgf_usu_id_modificador { get; set; }

        [Display(Name = "Fecha de Creación")]
        [Required(ErrorMessage = "Campo requerido")]
        public DateTime pgf_fec_creador { get; set; }

        [Display(Name = "Fecha de Modificación")]
        [Required(ErrorMessage = "Campo requerido")]
        public DateTime pgf_fec_modificador { get; set; }

        [Display(Name = "Usuario que da la baja")]
        [Required(ErrorMessage = "Campo requerido")]
        public int pgf_usu_id_baja { get; set; }

        [Display(Name = "Fecha de Baja")]
        [Required(ErrorMessage = "Campo requerido")]
        public DateTime pgf_fec_baja { get; set; }

        public bool pgf_activo { get; set; }

        public Pregunta_Frecuente()
        {
            this.Categoria = new Pregunta_Frecuente_Categoria();
            this.pgf_id = 0;
        }

    }
}
