using Entidades.App;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.App
{
    public class SIS_Areas : NegocioBase<Entidades.App.SIS_Area>
    {
        public SIS_Areas(Entidades.App.Token paramToken) : base("area_id", "area_activo", "SIS_Areas", "area")
        {
            Token = paramToken;
            TokenFilter = true;
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public override SIS_Area Mapear(DataRow dr)
        {
            Entidades.App.SIS_Area obj = MapearSimple(dr);
            return obj;
        }

        public override SIS_Area MapearCompleto(DataRow dr)
        {
            Entidades.App.SIS_Area obj = MapearSimple(dr);
            return obj;
        }

        public override SIS_Area MapearSimple(DataRow dr)
        {
            Entidades.App.SIS_Area obj = MapearStatic(dr);
            return obj;
        }

        public static SIS_Area MapearStatic(DataRow dr)
        {
            Entidades.App.SIS_Area obj = new SIS_Area();
            obj.area_id = Resources.Validaciones.valNULLINT(dr["area_id"]);
            obj.area_nombre = Resources.Validaciones.valNULLString(dr["area_nombre"]);
            obj.area_abreviatura = Resources.Validaciones.valNULLString(dr["area_abreviatura"]);
            obj.Padre = new SIS_Area() { area_id = Resources.Validaciones.valNULLINT(dr["area_area_id"]) };
            obj.Organizacion = new SIS_Organizacion() { org_id = Resources.Validaciones.valNULLINT(dr["area_org_id"]) };
            obj.area_activo = Resources.Validaciones.valNULLBool(dr["area_activo"]);
            obj.descripcion_combo = obj.area_nombre;
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(obj.area_id.ToString());
            return obj;
        }

        public override SIS_Area ObjetoNuevo()
        {
            Entidades.App.SIS_Area obj = new Entidades.App.SIS_Area();
            return obj;
        }

        public override void DatosObligatorios(Entidades.App.SIS_Area Obj)
        {
            if (Obj.area_nombre.Length.Equals(0))
                throw new Exception("Debe ingresar un nombre válido");

            if (Obj.area_abreviatura.Length.Equals(0))
                throw new Exception("Debe ingresar un mail válido");
        }

        public override void PermiteGuardar(Entidades.App.SIS_Area obj)
        {
          
        }

        public override ObjectMessage Save(SIS_Area Obj)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                PermiteGuardar(Obj);
                DatosObligatorios(Obj);
                DataRow row = db.Estructura(nombreTablaPrincipal);
                row["area_nombre"] = Obj.area_nombre;
                row["area_abreviatura"] = Obj.area_abreviatura;
                row["area_area_id"] = Obj.Padre.area_id;
                row["area_org_id"] = Obj.Organizacion.org_id;
                row["area_activo"] = Obj.area_activo;
                
                if (Obj.area_id.Equals(0))
                {    
                    row["area_usu_id_alta"] = Convert.ToInt32(Token.UserID);
                    row["area_fec_alta"] = DateTime.Now;
                    Obj.area_id = db.SQLInsert(row, "area_id").Valor;
                    oM.Message = "Datos ingresados";
                }
                else
                {
                    row["area_usu_id_mod"] = Convert.ToInt32(Token.UserID);
                    row["area_fec_mod"] = DateTime.Now;
                    db.SQLUpdate(row, "area_id=@area_id", "area_id", new List<System.Data.SqlClient.SqlParameter>() {
                        new System.Data.SqlClient.SqlParameter("area_id",Obj.area_id)
                    });

                    oM.Message = "Datos actualizados";
                }

                oM.ObjectRelation = Obj.area_id;
                oM.Success = true;
            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.Message = ex.Message;
            }

            return oM;
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  Select * from SIS_Areas ";
            sQuery += " inner join SIS_Organizaciones on org_id=area_org_id";
     
            if ((sWHERE != ""))
            {
                sQuery += " WHERE " + sWHERE;
            }

            if ((sOrderBy != ""))
            {
                sQuery += " ORDER BY " + sOrderBy;
            }

            return sQuery;
        }

         public List<Entidades.App.SIS_Area> ListarPorNombre(string texto, string cantidadRegistros = "")
        {
            List<Entidades.App.SIS_Area> lst = new List<Entidades.App.SIS_Area>();

            string sWhere = " (area_nombre like @searchText or area_abreviatura like @searchText) ";

            sQuery = QueryDefault(cantidadRegistros, sWhere, "");

            DataTable dt = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>() {
                new System.Data.SqlClient.SqlParameter("searchText", "%" + texto + "%"),
            });
            foreach (DataRow row in dt.Rows)
            {
                lst.Add(Mapear(row));
            }
            return lst;
        }

        public List<Entidades.App.SIS_Area> ListarPadres()
        {
            List<Entidades.App.SIS_Area> lst = new List<SIS_Area>();
            lst = ListarConFiltros(new List<ObjectParameter>() { new ObjectParameter("area_area_id", "0") });
            return lst;
        }
    }
}
