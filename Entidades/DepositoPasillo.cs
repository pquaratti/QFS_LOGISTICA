using System;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class DepositoPasillo : EntidadBase
    {
        [KeyAttribute]
        public int depopas_id { get; set; }

        [KeyRelation]
        [Display(Name = "Depósito")]
        [Required(ErrorMessage = "Campo requerido")]
        public Deposito Deposito { get; set; }

        [Display(Name = "Código")]
        [Required(ErrorMessage = "Campo requerido")]
        public string depopas_codigo { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo requerido")]
        public string depopas_nombre { get; set; }

        [Display(Name = "Descripción")]
        public string depopas_descripcion { get; set; }

        [Display(Name = "Posición X")]
        public decimal depopas_x { get; set; }

        [Display(Name = "Posición Y")]
        public decimal depopas_y { get; set; }

        [Display(Name = "Largo")]
        public decimal depopas_largo { get; set; }

        [Display(Name = "Ancho")]
        public decimal depopas_ancho { get; set; }

        [Display(Name = "Orientación")]
        public string depopas_orientacion { get; set; }

        [Display(Name = "Cantidad de posiciones")]
        public int depopas_cantidad_posiciones { get; set; }

        [Display(Name = "Cantidad de alturas")]
        public int depopas_cantidad_alturas { get; set; }

        [Display(Name = "Altura por nivel")]
        public decimal depopas_altura_nivel { get; set; }

        [Display(Name = "Peso máximo")]
        public decimal depopas_peso_maximo { get; set; }

        [Display(Name = "Activo")]
        public bool depopas_activo { get; set; }

        public DepositoPasillo()
        {
            depopas_id = 0;
            Deposito = new Deposito();
            depopas_orientacion = "H";
            depopas_cantidad_posiciones = 1;
            depopas_cantidad_alturas = 1;
            depopas_largo = 240;
            depopas_ancho = 60;
            depopas_altura_nivel = 100;
            depopas_activo = true;
        }
    }
}