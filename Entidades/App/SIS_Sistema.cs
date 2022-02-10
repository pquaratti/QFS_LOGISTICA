using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.App
{
    public class SIS_Sistema
    {
        public List<Entidades.App.SIS_Modulo> Modulos { get; set; }
        public Entidades.App.Token Token { get; set; }
        public Entidades.App.SIS_Usuario UsuarioActual { get; set; }
        
    }
}
