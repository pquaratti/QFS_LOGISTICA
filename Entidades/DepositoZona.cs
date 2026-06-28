using System;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class DepositoZona : EntidadBase
    {
        [KeyAttribute]
        public int depzon_id { get; set; }

        [KeyRelation]
        [Display(Name = "Depósito")]
        [Required(ErrorMessage = "Campo requerido")]
        public Deposito Deposito { get; set; }

        [Display(Name = "Código")]
        [Required(ErrorMessage = "Campo requerido")]
        public string depzon_codigo { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo requerido")]
        public string depzon_nombre { get; set; }

        [Display(Name = "Descripción")]
        public string depzon_descripcion { get; set; }

        [Display(Name = "Posición X")]
        public decimal depzon_x { get; set; }

        [Display(Name = "Posición Y")]
        public decimal depzon_y { get; set; }

        [Display(Name = "Largo")]
        public decimal depzon_largo { get; set; }

        [Display(Name = "Ancho")]
        public decimal depzon_ancho { get; set; }

        [Display(Name = "Activo")]
        public bool depzon_activo { get; set; }

        public DepositoZona()
        {
            depzon_id = 0;
            Deposito = new Deposito();
            depzon_largo = 520;
            depzon_ancho = 260;
            depzon_activo = true;
        }
    }
}