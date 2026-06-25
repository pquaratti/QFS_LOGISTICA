using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class UbicacionLogistica : EntidadBase
    {
        [KeyAttribute]
        public int ubilog_id { get; set; }

        public string ubilog_codigo { get; set; }

        // Claves foráneas como enteros (los nombres coinciden con las columnas de la tabla)
        public int? ubilog_tubilog_id { get; set; }
        public int? ubilog_tmanilog_id { get; set; }
        public int? ubilog_trotlog_id { get; set; }
        public int? ubilog_teubilog_id { get; set; }
        public int? ubilog_zonlog_id { get; set; }
        public int? ubilog_planta_id { get; set; }
        public int? ubilog_depo_id { get; set; }
        public int? ubilog_pasillo_id { get; set; }
        public int? ubilog_deprack_id { get; set; }

        public string ubilog_secuencia_ruta { get; set; }

        // Propiedades de navegación (sin [KeyRelation]): usadas sólo para enlace en vistas.
        // La persistencia de claves foráneas se hace mediante los enteros de arriba.
        public Entidades.Planta Planta { get; set; }
        public Entidades.Deposito Deposito { get; set; }
        public Entidades.ZonaLogistica Zona { get; set; }
        public Entidades.TipoUbicacionLogistica TipoUbicacion { get; set; }

        public int ubilog_posicion { get; set; }

        public int ubilog_columna { get; set; }

        public string ubilog_nivel { get; set; }

        public decimal ubilog_coord_x { get; set; }

        public decimal ubilog_coord_y { get; set; }

        public decimal ubilog_coord_z { get; set; }

        public decimal ubilog_altura { get; set; }

        public decimal ubilog_longitud { get; set; }

        public decimal ubilog_anchura { get; set; }

        public decimal ubilog_capacidad_cubica { get; set; }

        public decimal ubilog_capacidad_maxima { get; set; }

        public decimal ubilog_volumen_maximo { get; set; }

        public decimal ubilog_peso_maximo { get; set; }

        public string ubilog_tipo_producto_permitido { get; set; }

        public bool ubilog_multiples_articulos { get; set; }
        public bool ubilog_multiples_lotes { get; set; }

        public bool ubilog_activo { get; set; }

        public UbicacionLogistica()
        {
            this.ubilog_id = 0;
            this.ubilog_activo = true;
            this.Planta = new Planta();
            this.Deposito = new Deposito();
            this.Zona = new ZonaLogistica();
            this.TipoUbicacion = new TipoUbicacionLogistica();
        }
    }
}
