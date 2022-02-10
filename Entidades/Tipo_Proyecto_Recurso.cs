using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Tipo_Proyecto_Recurso
    {
        public int tproyrecurso_id { get; set; }

        [Display(Name = "Tipo de Recurso")]
        [Required(ErrorMessage = "Campo requerido")]
        public string tproyrecurso_nombre { get; set; }

        public Tipo_Proyecto_Recurso()
        {
            this.tproyrecurso_id = 0;
        }

    }
}
