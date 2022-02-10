using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Pregunta_Frecuente_Categoria
    {
        public int pgfc_id { get; set; }

        [Display(Name = "Rótulo de categoría")]
        [Required(ErrorMessage = "Campo requerido")]
        public string pgfc_nombre { get; set; }

        public bool pgfc_activo { get; set; }

        public Pregunta_Frecuente_Categoria()
        {
            this.pgfc_id = 0;
        }

    }
}
