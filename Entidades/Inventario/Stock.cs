using System.ComponentModel.DataAnnotations;

namespace Entidades.Inventario
{
    public class Stock : EntidadBase
    {
        [Key]
        public int stock_id { get; set; }

        [KeyRelation]
        public Deposito Deposito { get; set; }

        [KeyRelation]
        public Producto Producto { get; set; }

        public decimal stock_actual { get; set; }
        public decimal stock_reservado { get; set; }
        public decimal stock_disponible { get; set; }
        public bool stock_activo { get; set; }

        public Stock()
        {
            stock_id = 0;
            stock_activo = true;
            Deposito = new Deposito();
            Producto = new Producto();
        }
    }
}
