using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ReportManager.Entitites
{
    public class ReportParameterCustom
    {
        public string DataSourceName { get; set; }
        public object DataSourceObject { get; set; }

        public ReportParameterCustom(string name, object objetoDatos)
        {
            this.DataSourceName = name;
            this.DataSourceObject = objetoDatos;
        }


    }
}
