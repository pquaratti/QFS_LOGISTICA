using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades.App;

namespace Negocio.App
{
    //public class SIS_Sistemas : NegocioBase<Entidades.App.SIS_Sistema>
    //{
    //    //public SIS_Sistemas() : base("sis_id") { }

    //    //public override SIS_Sistema Mapear(DataRow dr)
    //    //{
    //    //    Entidades.App.SIS_Sistema obj = new SIS_Sistema();
    //    //    obj.sis_id = Resources.Validaciones.valNULLString(dr["sis_id"]);
    //    //    obj.sis_nombre = Resources.Validaciones.valNULLString(dr["sis_nombre"]);
    //    //    obj.sis_descripcion = Resources.Validaciones.valNULLString(dr["sis_descripcion"]);
    //    //    obj.sis_activo = Resources.Validaciones.valNULLBool(dr["sis_activo"]);
    //    //    return obj;
    //    //}

    //    //public override SIS_Sistema MapearCompleto(DataRow dr)
    //    //{
    //    //    Entidades.App.SIS_Sistema obj = Mapear(dr);

    //    //    // Propiedades específicas

    //    //    return obj;
    //    //}

    //    //public override bool Save(SIS_Sistema Obj)
    //    //{
    //    //    DataRow row = db.Estructura("SIS_SISTEMAS");
    //    //    row["sis_nombre"] = Obj.sis_nombre.Trim();
    //    //    row["sis_descripcion"] = Obj.sis_descripcion.Trim();
    //    //    row["sis_activo"] = Obj.sis_activo;
            
    //    //    DataTable dt_bus = db.SQLSelect("SELECT * FROM SIS_SISTEMAS WHERE sis_id=@sistema", new List<System.Data.SqlClient.SqlParameter>()
    //    //    {
    //    //        new System.Data.SqlClient.SqlParameter("sistema",Obj.sis_id)
    //    //    });

    //    //    if (Obj.sis_id.Equals(0)) {
             
    //    //        if (dt_bus.Rows.Count > 0)
    //    //        {
    //    //            return false;
    //    //        }
    //    //        row["sis_id"] = Obj.sis_id;
    //    //        db.SQLInsert(row);
    //    //    }
    //    //    else
    //    //    {
    //    //        db.SQLUpdate(row, "sis_id=@id", "sis_id", new List<System.Data.SqlClient.SqlParameter>()
    //    //        {
    //    //            new System.Data.SqlClient.SqlParameter("id",Obj.sis_id)
    //    //        });
    //    //      //return false;
    //    //    }
    //    //    return true;
    //    //}

    //    //public List<Entidades.App.SIS_Sistema> ListarActivos(string orden = "")
    //    //{
    //    //    List<Entidades.App.SIS_Sistema> lst = new List<Entidades.App.SIS_Sistema>();

    //    //    sQuery = QueryDefault("", "sis_activo = 1 or sis_activo is null", orden);

    //    //    DataTable dt = db.SQLSelect(sQuery);

    //    //    foreach (DataRow row in dt.Rows)
    //    //    {
    //    //        lst.Add(Mapear(row));
    //    //    }
    //    //    return lst;
    //    //}



    //    //protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
    //    //{
    //    //    sQuery = "  SELECT * FROM SIS_SISTEMAS ";
    //    //    if ((sWHERE != ""))
    //    //    {
    //    //        sQuery += " WHERE " + sWHERE;
    //    //    }

    //    //    if ((sOrderBy != ""))
    //    //    {
    //    //        sQuery += " ORDER BY " + sOrderBy;
    //    //    }
    //    //    return sQuery;
    //    //}

    //    //public override void PermiteGuardar(string pNombreTabla, SIS_Sistema Obj)
    //    //{
    //    //    throw new NotImplementedException();
    //    //}

    //    //public override List<DLLObject> ListarDLL(bool agregaDefault = false)
    //    //{
    //    //    throw new NotImplementedException();
    //    //}
    //}
}
