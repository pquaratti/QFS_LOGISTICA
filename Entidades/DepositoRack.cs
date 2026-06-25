using System;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class DepositoRack : EntidadBase
    {
        [KeyAttribute]
        public int deprack_id { get; set; }

        [KeyRelation]
        [Display(Name = "Depósito")]
        [Required(ErrorMessage = "Campo requerido")]
        public Deposito Deposito { get; set; }

        // FKs como enteros (los nombres coinciden con las columnas de la tabla)
        [Display(Name = "Zona")]
        public int? deprack_zonlog_id { get; set; }

        [Display(Name = "Pasillo")]
        public int? deprack_pasillo_id { get; set; }

        [Display(Name = "Código")]
        [Required(ErrorMessage = "Campo requerido")]
        public string deprack_codigo { get; set; }

        [Display(Name = "Nombre")]
        public string deprack_nombre { get; set; }

        [Display(Name = "Descripción")]
        public string deprack_descripcion { get; set; }

        [Display(Name = "Posición X")]
        public decimal deprack_x { get; set; }

        [Display(Name = "Posición Y")]
        public decimal deprack_y { get; set; }

        [Display(Name = "Largo")]
        public decimal deprack_largo { get; set; }

        [Display(Name = "Ancho")]
        public decimal deprack_ancho { get; set; }

        [Display(Name = "Orientación")]
        public string deprack_orientacion { get; set; }

        [Display(Name = "Cantidad de columnas")]
        public int deprack_cantidad_columnas { get; set; }

        [Display(Name = "Cantidad de niveles")]
        public int deprack_cantidad_niveles { get; set; }

        [Display(Name = "Altura por nivel")]
        public decimal deprack_altura_nivel { get; set; }

        [Display(Name = "Peso máximo")]
        public decimal deprack_peso_maximo { get; set; }

        [Display(Name = "Color")]
        public string deprack_color { get; set; }

        [Display(Name = "Activo")]
        public bool deprack_activo { get; set; }

        public DepositoRack()
        {
            deprack_id = 0;
            Deposito = new Deposito();
            deprack_orientacion = "H";
            deprack_cantidad_columnas = 4;
            deprack_cantidad_niveles = 3;
            deprack_largo = 200;
            deprack_ancho = 40;
            deprack_altura_nivel = 100;
            deprack_color = "#f59e0b";
            deprack_activo = true;
        }
    }
}
