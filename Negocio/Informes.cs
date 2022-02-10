using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades.Vistas.Graficos;
using Newtonsoft.Json;

namespace Negocio
{
    public class Informes
    {
        public Helpers.SQLDb db;
        public string sQuery = "";
        public Entidades.App.Token Token { get; set; }

        public Informes(Entidades.App.Token paramToken)
        {
            Token = paramToken;
            db = new Helpers.SQLDb();
        }
        public static decimal TimeSpan(System.DateTime input,int ejercicio)
        {
            System.DateTime span = new System.DateTime(ejercicio,1,1);
            TimeSpan difference = input - span;
            return Convert.ToDecimal(difference.TotalDays);
        }

    }
}
