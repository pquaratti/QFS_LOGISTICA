using Entidades.App;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.App
{
    public class SIS_Organizaciones : NegocioBase<Entidades.App.SIS_Organizacion>
    {
        public SIS_Organizaciones(Entidades.App.Token paramToken) : base("org_id", "org_activo", "SIS_Organizaciones", "org")
        {
            Token = paramToken;
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public override SIS_Organizacion Mapear(DataRow dr)
        {
            Entidades.App.SIS_Organizacion obj = MapearSimple(dr);
            return obj;
        }

        public override SIS_Organizacion MapearCompleto(DataRow dr)
        {
            Entidades.App.SIS_Organizacion obj = MapearSimple(dr);
            return obj;
        }

        public override SIS_Organizacion MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static SIS_Organizacion MapearStatic(DataRow dr)
        {
            Entidades.App.SIS_Organizacion obj = new SIS_Organizacion();
            obj.org_id = Resources.Validaciones.valNULLINT(dr["org_id"]);
            obj.org_nombre = Resources.Validaciones.valNULLString(dr["org_nombre"]);
            obj.org_abreviatura = Resources.Validaciones.valNULLString(dr["org_abreviatura"]);
            obj.org_mail = Resources.Validaciones.valNULLString(dr["org_mail"]);
            obj.org_activo = Resources.Validaciones.valNULLBool(dr["org_activo"]);
            return obj;
        }

        public override SIS_Organizacion ObjetoNuevo()
        {
            Entidades.App.SIS_Organizacion obj = new Entidades.App.SIS_Organizacion();
            return obj;
        }

        public override void DatosObligatorios(Entidades.App.SIS_Organizacion Obj)
        {
            if (Obj.org_nombre.Length.Equals(0))
                throw new Exception("Debe ingresar un nombre válido");

            if (Obj.org_mail.Length.Equals(0))
                throw new Exception("Debe ingresar un mail válido");

        }

        public override void PermiteGuardar(Entidades.App.SIS_Organizacion obj)
        {
          
        }

        public override ObjectMessage Save(SIS_Organizacion Obj)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                PermiteGuardar(Obj);
                DatosObligatorios(Obj);
                DataRow row = db.Estructura(nombreTablaPrincipal);
                row["org_nombre"] = Obj.org_nombre;
                row["org_abreviatura"] = Obj.org_abreviatura;
                row["org_mail"] = Obj.org_mail;
                row["org_activo"] = Obj.org_activo;
                
                if (Obj.org_id.Equals(0))
                {    
                    row["org_usu_id_alta"] = Convert.ToInt32(Token.UserID);
                    row["org_fec_alta"] = DateTime.Now;
                    Obj.org_id = db.SQLInsert(row, "org_id").Valor;
                    oM.Message = "Datos ingresados";
                }
                else
                {
                    row["org_usu_id_mod"] = Convert.ToInt32(Token.UserID);
                    row["org_fec_mod"] = DateTime.Now;
                    db.SQLUpdate(row, "org_id=@org_id", "org_id", new List<System.Data.SqlClient.SqlParameter>() {
                        new System.Data.SqlClient.SqlParameter("org_id",Obj.org_id)
                    });

                    oM.Message = "Datos actualizados";
                }

                oM.ObjectRelation = Obj.org_id;
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
            sQuery = "  Select * from SIS_Organizaciones ";
     
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

         public List<Entidades.App.SIS_Organizacion> ListarPorNombre(string texto, string cantidadRegistros = "")
        {
            List<Entidades.App.SIS_Organizacion> lst = new List<Entidades.App.SIS_Organizacion>();

            string sWhere = " org_nombre like @searchText ";

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
    }
}
