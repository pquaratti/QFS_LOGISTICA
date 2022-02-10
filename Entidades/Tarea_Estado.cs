using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Tarea_Estado
    {
        public int tarestado_id { get; set; }

        [Display(Name = "Título")]
        [Required(ErrorMessage = "Campo requerido")]
        public string tarestado_titulo { get; set; }

        public string tarestado_css_text { get; set; }
        public string tarestado_css_icon { get; set; }
        public string tarestado_css_background { get; set; }
        public Tarea_Estado() 
        {
            this.tarestado_id = 0;
        } 

    }
}
