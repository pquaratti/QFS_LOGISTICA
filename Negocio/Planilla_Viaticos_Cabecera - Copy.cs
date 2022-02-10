using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using Entidades.App;

namespace Negocio
{
    public class Planilla_Viaticos_Cabecera : NegocioBase<Entidades.Planilla_Viaticos_Cabecera>
    {
        Negocio.Distritos  negocioDIS;
        Negocio.Localidades negocioLOC;
        Negocio.Ordenes_Pago negocioOPA;
      
        public Planilla_Viaticos_Cabecera(Entidades.App.Token paramToken) : base("pvc_id", "pvc_activo", "Planilla_Viaticos_Cabecera", "pvc")
        {
            Token = paramToken;
            negocioDIS = new Distritos(Token);
            negocioLOC = new Localidades(Token);
            negocioOPA = new Ordenes_Pago(Token); 
        }

        #region Funcionalidad
        public override Entidades.Planilla_Viaticos_Cabecera ObjetoNuevo()
        {
            Entidades.Planilla_Viaticos_Cabecera obj = new Entidades.Planilla_Viaticos_Cabecera();
            return obj;
        }

        public override void PermiteGuardar(string pNombreTabla, string id_nombre, Int32 id, string nom_nombre, string nom, string activo)
        {

        }

        public override ObjectMessage Save(Entidades.Planilla_Viaticos_Cabecera Obj)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                DataRow row = db.Estructura("Planilla_Viaticos_Cabecera");
                row["pvc_dis_id"] = Obj.Distritos.dis_id;
                row["pvc_loc_id"] = Obj.Localidades.loc_id;
                row["pvc_fec_desde"] = Obj.pvc_fec_desde;
                row["pvc_fec_hasta"] = Obj.pvc_fec_hasta;
                row["pvc_tipo_personal"] = Obj.pvc_tipo_personal;
                row["pvc_opa_id"] = Obj.Ordenes_Pago.opa_id;
                row["pvc_nombre"] = Obj.pvc_nombre;
                row["pvc_observaciones"] = Obj.pvc_observaciones;

                if (Obj.pvc_id.Equals(0))
                {
                    row["pvc_activo"] = 1;
                    row["pvc_usu_id_alta"] = Convert.ToInt32(Token.UserID);
                    row["pvc_fec_alta"] = DateTime.Now;
                    Obj.pvc_id = db.SQLInsert(row, "pvc_id").Valor;
                    oM.Message = "Datos ingresados";
                }
                else
                {
                    row["pvc_usu_id_mod"] = Convert.ToInt32(Token.UserID);
                    row["pvc_fec_mod"] = DateTime.Now;
                    db.SQLUpdate(row, "pvc_id=@pvc_id", "pvc_id", new List<System.Data.SqlClient.SqlParameter>() {
                        new System.Data.SqlClient.SqlParameter("pvc_id",Obj.pvc_id)
                    });

                    oM.Message = "Datos actualizados";
                }

                oM.Success = true;
            }
            catch (Exception ex)
            {
                oM.Success = false;
                if (ex.HResult.ToString() == "-2146232016")
                {
                    oM.Message = "Debe Seleccionar una fecha valida!!!";
                }
                else
                {
                    oM.Message = ex.Message;
                }

            }

            return oM;
        }

        #endregion

        #region Consultas y Listados

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public Entidades.Planilla_Viaticos_Cabecera ObtenerPorID(int pvc_id)
        {
            Entidades.Planilla_Viaticos_Cabecera obj = new Entidades.Planilla_Viaticos_Cabecera();
            sQuery = QueryDefault("", " pvc_id = @pvc_id", "");
            DataTable dt_movimiento = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter ("pvc_id", pvc_id)
            });

            if (dt_movimiento.Rows.Count > 0)
            {
                obj = Mapear(dt_movimiento.Rows[0]);
            }
            else
            {
                obj = null;
            }
            return obj;
        }

        public List<Entidades.Planilla_Viaticos_Cabecera> Listar(int distritoID, int unidadID, int dependenciaID)
        {
            List<Entidades.Planilla_Viaticos_Cabecera> lst = new List<Entidades.Planilla_Viaticos_Cabecera>();

            sQuery = QueryDefault("", " WHERE pvc_dis_id=@dis_id ", "");

            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("dis_id",distritoID)
            });

            foreach (DataRow row in dt_bus.Rows)
            {
                lst.Add(MapearSimple(row));
            }

            return lst;
        }

        #endregion

        #region Mappers

        public override Entidades.Planilla_Viaticos_Cabecera Mapear(DataRow dr)
        {
            Entidades.Planilla_Viaticos_Cabecera obj = MapearSimple(dr);
            obj.Distritos = negocioDIS.MapearSimple(dr);
            obj.Localidades = negocioLOC.MapearSimple(dr);
            return obj;
        }

        public override Entidades.Planilla_Viaticos_Cabecera MapearCompleto(DataRow dr)
        {
            Entidades.Planilla_Viaticos_Cabecera obj = Mapear(dr);
            return obj;
        }

        public override Entidades.Planilla_Viaticos_Cabecera MapearSimple(DataRow dr)
        {
            Entidades.Planilla_Viaticos_Cabecera obj = new Entidades.Planilla_Viaticos_Cabecera();
            obj.pvc_id = Resources.Validaciones.valNULLINT(dr["pvc_id"]);
            obj.pvc_fec_desde = Resources.Validaciones.valNULLDateTime(dr["pvc_fec_desde"]);
            obj.pvc_fec_hasta = Resources.Validaciones.valNULLDateTime(dr["pvc_fec_hasta"]);
            obj.pvc_tipo_personal = Resources.Validaciones.valNULLString(dr["pvc_tipo_personal"]);
            obj.pvc_nombre = Resources.Validaciones.valNULLString(dr["pvc_nombre"]);
            obj.pvc_activo = Resources.Validaciones.valNULLBool(dr["pvc_activo"]);
            obj.pvc_observaciones = Resources.Validaciones.valNULLString(dr["pvc_observaciones"]);
            return obj;
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = " Select * from Planilla_Viaticos_Cabecera";
            sQuery += " LEFT JOIN Distritos on dis_id = pvc_dis_id";
            sQuery += " LEFT JOIN Localidades on loc_id = pvc_loc_id";
            sQuery += sWHERE;
            sQuery += sOrderBy;
            return sQuery;
        }

        #endregion

    }
}
