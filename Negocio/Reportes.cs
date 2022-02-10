using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Newtonsoft.Json;

namespace Negocio
{
    public class Reportes
    {
        private Entidades.App.Token _currentToken;
        public Reportes(Entidades.App.Token token)
        {
            _currentToken = token;
        }

        public List<Entidades.Reportes.datosHeader> DatosHeader()
        {
            List<Entidades.Reportes.datosHeader> lst = new List<Entidades.Reportes.datosHeader>();

            var _subtituloReporte = "";

            lst.Add(new Entidades.Reportes.datosHeader() { Titulo = "NOS", Subtitulo = _subtituloReporte });

            return lst;
        }


    }
}
