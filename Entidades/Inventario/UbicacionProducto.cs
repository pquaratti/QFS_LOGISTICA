using System;
using System.ComponentModel.DataAnnotations;

namespace Entidades.Inventario
{
    public class UbicacionProducto : EntidadBase
    {
        [Key]
        public int ubipro_id { get; set; }

        [KeyRelation]
        public UbicacionLogistica UbicacionLogistica { get; set; }

        [KeyRelation]
        public Producto Producto { get; set; }

        public decimal ubipro_cantidad { get; set; }
        public int ubipro_cantidad_maxima { get; set; }
        public string ubipro_lote { get; set; }
        public string ubipro_nro_serie { get; set; }
        public DateTime? ubipro_fec_vencimiento { get; set; }
        public bool ubipro_activo { get; set; }

        public UbicacionProducto()
        {
            ubipro_id = 0;
            ubipro_activo = true;
            UbicacionLogistica = new UbicacionLogistica();
            Producto = new Producto();
        }
    }
}