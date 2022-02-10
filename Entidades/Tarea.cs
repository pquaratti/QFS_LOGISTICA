using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Tarea : EntidadBase, Controls.IAgendable
    {
        public int tar_id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo requerido")]
        public string tar_nombre { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "Campo requerido")]
        public string tar_descripcion { get; set; }

        [Display(Name = "Prioridad")]
        [Required(ErrorMessage = "Campo requerido")]
        public Entidades.Tipo_Prioridad Prioridad { get; set; }

        [Display(Name = "Fec.Ini")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Campo requerido")]
        public DateTime tar_fec_ini { get; set; }

        [Display(Name = "Fec.Fin")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Campo requerido")]
        public DateTime tar_fec_fin { get; set; }

        [Display(Name = "Fec.Mod")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Campo requerido")]
        public DateTime tar_fec_mod { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "Campo requerido")]
        public Tarea_Estado EstadoTarea { get; set; }

        [Display(Name = "Tiempo Estimado")]
        [Required(ErrorMessage = "Campo requerido")]
        public decimal tar_tiempo { get; set; }

        [Display(Name = "Resolución")]
        [Required(ErrorMessage = "Campo requerido")]
        public int tar_porcentaje { get; set; }

        public DateTime tar_fec_fin_real { get; set; }

        [Display(Name = "Nro")]
        public int tar_numero { get; set; }

        public Entidades.App.SIS_Organizacion Organizacion { get; set; }

        public Entidades.App.SIS_Area Area { get; set; }

        public string CodigoReferencia { get; set; }

        #region Implementacion Agendable
        public DateTime agendable_fecha { get { return tar_fec_ini; } set { } }
        public string agendable_titulo { get { return tar_nombre; } set { } }
        public int agendable_id { get { return tar_id; } set { } }
        public DateTime agendable_fecha_hasta { get { return tar_fec_fin; } set { } }
        #endregion

        public Tarea()
        {
            this.tar_id = 0;
            this.EstadoTarea = new Tarea_Estado();
            this.Organizacion = new App.SIS_Organizacion();
            this.Area = new App.SIS_Area();
        }

    }
}