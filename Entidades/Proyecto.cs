using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class Proyecto : EntidadBase
    {
        public int proy_id { get; set; }

        [Display(Name = "Título")]
        [Required(ErrorMessage = "Campo requerido")]
        public string proy_titulo { get; set; }

        [Display(Name = "Tipo de Proyecto")]
        [Required(ErrorMessage = "Campo requerido")]
        public Entidades.Tipo_Proyecto Tipo { get; set; }

        [Display(Name = "Organización")]
        [Required(ErrorMessage = "Campo requerido")]
        public Entidades.App.SIS_Organizacion Organizacion { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "Campo requerido")]
        public string proy_descripcion { get; set; }

        [Display(Name = "Foto")]
        [Required(ErrorMessage = "Campo requerido")]
        public string proy_foto { get; set; }

        [Display(Name = "Duración")]
        [Required(ErrorMessage = "Campo requerido")]
        public TimeSpan proy_duracion { get; set; }

        [Display(Name = "Activo")]
        [Required(ErrorMessage = "Campo requerido")]
        public bool proy_activo { get; set; }
        public bool Finalizado { get; set; }
        public bool EnCurso { get; set; }
        public bool Cerrado { get; set; }

        [Display(Name = "Fecha de Cierre")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime proy_fec_cerrado { get; set; }

        [Display(Name = "Fecha de Finalización")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Campo requerido")]
        public DateTime proy_fec_fin { get; set; }

        [Display(Name = "Fecha de Inicio")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Campo requerido")]
        public DateTime proy_fec_ini { get; set; }

        public Entidades.App.SIS_Area Area { get; set; }

        public decimal PorcentajeEvolucion { get; set; }
        
        public List<Entidades.Proyecto_Objetivo> Objetivos { get; set; }

        public Proyecto()
        {
            this.proy_id = 0;
            this.Tipo = new Tipo_Proyecto();
            this.Organizacion = new App.SIS_Organizacion();
        }

    }


}
