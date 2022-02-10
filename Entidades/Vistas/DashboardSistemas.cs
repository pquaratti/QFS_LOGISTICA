using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Vistas
{
    public class DashboardSistemas
    {
        public int CantidadUsuarios { get; set; }
        public int CantidadDistritos { get; set; }
        public int CantidadUsuariosAdministradores { get; set; }
        public int CantidadUsuariosBloqueados { get; set; }
        public List<Entidades.Vistas.ClaveValor> DistritosTotalesUsuarios { get; set; }
        public DashboardSistemas()
        {
            this.CantidadUsuarios = 0;
            this.CantidadDistritos = 0;
            this.CantidadUsuariosAdministradores = 0;
            this.CantidadUsuariosBloqueados = 0;
            this.DistritosTotalesUsuarios = new List<ClaveValor>();
        }
    }
}
