using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Proyecto_Objetivo : EntidadBase
    {
        public int pryobj_id { get; set; }

        [Display(Name = "Proyecto")]
        [Required(ErrorMessage = "Campo requerido")]
        public Entidades.Proyecto ProyectoVinculado { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo requerido")]
        public string pryobj_nombre { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "Campo requerido")]
        public string pryobj_descripcion { get; set; }

        [Display(Name = "Foto")]
        [Required(ErrorMessage = "Campo requerido")]
        public string pryobj_foto { get; set; }

        [Display(Name = "Prioridad")]
        [Required(ErrorMessage = "Campo requerido")]
        public Entidades.Tipo_Prioridad Prioridad { get; set; }

        [Display(Name = "Fec.Ini")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Campo requerido")]
        public DateTime pryobj_fec_ini { get; set; }

        [Display(Name = "Fec.Ven")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Campo requerido")]
        public DateTime pryobj_fec_ven { get; set; }

        [Display(Name = "Código")]
        [Required(ErrorMessage = "Campo requerido")]
        public string pryobj_codigo { get; set; }

        [Display(Name = "Valor Inicial")]
        public decimal ValorIncial { get; set; }

        [Display(Name = "Valor Meta")]
        public decimal ValorMeta { get; set; }

        [Display(Name = "Cantidad de Indicadores")]
        public int CantidadIndicadores { get; set; }
        public decimal PorcentajeEvolucion { get; set; }
        
        public Proyecto_Objetivo()
        {
            this.pryobj_id = 0;
        }
    }
}
