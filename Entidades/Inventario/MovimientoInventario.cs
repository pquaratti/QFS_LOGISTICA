using System;
using System.ComponentModel.DataAnnotations;

namespace Entidades.Inventario
{
    public class MovimientoInventario : EntidadBase
    {
        [Key]
        public int movinv_id { get; set; }

        [KeyRelation]
        [Required(ErrorMessage = "Campo requerido")]
        public TipoMovimientoInventario TipoMovimientoInventario { get; set; }

        [KeyRelation]
        [Required(ErrorMessage = "Campo requerido")]
        public Deposito DepositoOrigen { get; set; }

        [KeyRelation]
        public Deposito DepositoDestino { get; set; }

        [KeyRelation]
        [Required(ErrorMessage = "Campo requerido")]
        public Producto Producto { get; set; }

        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "Campo requerido")]
        public decimal movinv_cantidad { get; set; }

        public DateTime movinv_fecha { get; set; }
        public string movinv_lote { get; set; }
        public DateTime? movinv_vencimiento { get; set; }
        public string movinv_serie { get; set; }
        public string movinv_motivo { get; set; }
        public string movinv_observaciones { get; set; }
        public string movinv_estado { get; set; }
        public bool movinv_activo { get; set; }

        public MovimientoInventario()
        {
            movinv_id = 0;
            movinv_activo = true;
            movinv_fecha = DateTime.Now;
            movinv_estado = "CONFIRMADO";
            TipoMovimientoInventario = new TipoMovimientoInventario();
            DepositoOrigen = new Deposito();
            DepositoDestino = new Deposito();
            Producto = new Producto();
        }
    }
}
