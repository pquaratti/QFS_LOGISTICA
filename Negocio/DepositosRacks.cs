using Entidades;
using Entidades.App;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Negocio
{
    public class DepositosRacks : NegocioBase<Entidades.DepositoRack>
    {
        public DepositosRacks(Entidades.App.Token paramToken) : base("deprack_id", "deprack_activo", "Depositos_Racks", "deprack")
        {
            Token = paramToken;
            TokenFilter = true;
        }

        public override DepositoRack ObjetoNuevo()
        {
            DepositoRack obj = new DepositoRack();
            obj.deprack_id = 0;
            obj.IdEncriptado = App.Security.EncriptarID(Convert.ToString(obj.deprack_id));
            return obj;
        }

        public override void PermiteGuardar(DepositoRack obj)
        {
            if (obj.Deposito == null || obj.Deposito.depo_id <= 0)
                throw new Exception("Debe seleccionar un depósito.");

            if (string.IsNullOrWhiteSpace(obj.deprack_codigo))
                throw new Exception("Debe ingresar el código del rack.");

            if (string.IsNullOrWhiteSpace(obj.deprack_nombre))
                obj.deprack_nombre = obj.deprack_codigo;

            if (obj.deprack_cantidad_columnas <= 0)
                throw new Exception("La cantidad de columnas debe ser mayor a cero.");

            if (obj.deprack_cantidad_niveles <= 0)
                throw new Exception("La cantidad de niveles debe ser mayor a cero.");

            if (obj.deprack_largo <= 0)
                obj.deprack_largo = 200;

            if (obj.deprack_ancho <= 0)
                obj.deprack_ancho = 40;

            if (string.IsNullOrWhiteSpace(obj.deprack_orientacion))
                obj.deprack_orientacion = "H";
        }

        public override ObjectMessage Save(DepositoRack Obj)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                PermiteGuardar(Obj);
                DataRow row = db.Estructura(nombreTablaPrincipal);
                oM = SaveReflection(Obj, row, true);
            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.Message = ex.Message;
            }

            return oM;
        }

        public ObjectMessage SaveYGenerarUbicaciones(DepositoRack obj)
        {
            ObjectMessage oM = Save(obj);

            if (oM.Success)
            {
                if (obj.deprack_id <= 0 && oM.ObjectRelation != null && !string.IsNullOrWhiteSpace(Convert.ToString(oM.ObjectRelation)))
                    obj.deprack_id = Convert.ToInt32(App.Security.DesencriptarID(Convert.ToString(oM.ObjectRelation)));

                if (obj.deprack_id <= 0)
                {
                    oM.Success = false;
                    oM.Message = "No se pudo obtener el identificador del rack guardado.";
                    return oM;
                }

                string idEncriptado = App.Security.EncriptarID(Convert.ToString(obj.deprack_id));

                string codigoDeposito = ObtenerCodigoDeposito(obj.Deposito != null ? obj.Deposito.depo_id : 0);
                string codigoZona = ObtenerCodigoZona(obj.deprack_zonlog_id ?? 0);
                string codigoPasillo = ObtenerCodigoPasillo(obj.deprack_pasillo_id ?? 0);

                oM = new UbicacionesLogisticas(Token).RegenerarDesdeRack(obj, codigoDeposito, codigoZona, codigoPasillo);
                oM.ObjectRelation = idEncriptado;
            }

            return oM;
        }

        public ObjectMessage DeleteLogicoConUbicaciones(string valorId)
        {
            int rackID = ObtenerID(valorId);
            new UbicacionesLogisticas(Token).DesactivarUbicacionesRack(rackID);
            return DeleteLogico(valorId);
        }

        public List<DepositoRack> ListarPorDeposito(int depoID)
        {
            List<ObjectParameter> filtros = new List<ObjectParameter>();
            filtros.Add(new ObjectParameter() { Name = "deprack_depo_id", Value = depoID });
            filtros.Add(new ObjectParameter() { Name = "deprack_activo", Value = 1 });
            return ListarConFiltros(filtros, "", "deprack_codigo");
        }

        public List<DepositoRack> ListarConFiltros(List<ObjectParameter> listaFiltros, string cantRegistros, string orden)
        {
            List<DepositoRack> lst = new List<DepositoRack>();
            string sWhere = "";
            List<SqlParameter> lstParamsSQL = new List<SqlParameter>();

            foreach (var itemFiltro in listaFiltros)
            {
                sWhere += itemFiltro.Name + "=@" + itemFiltro.Name + " and ";
                lstParamsSQL.Add(new SqlParameter(itemFiltro.Name, itemFiltro.Value));
            }

            sWhere += " 1=1 ";

            var _queryFilter = GetFilterTokenQuery();
            if (_queryFilter.Trim().Length > 0)
            {
                sWhere += " and " + _queryFilter;
                lstParamsSQL.AddRange(GetFilterTokenParams());
            }

            sQuery = QueryDefault(cantRegistros, sWhere, orden);
            DataTable dt = db.SQLSelect(sQuery, lstParamsSQL);

            foreach (DataRow row in dt.Rows)
                lst.Add(Mapear(row));

            return lst;
        }

        public void ActualizarGeometria(int id, decimal x, decimal y, decimal largo, decimal ancho)
        {
            string query = "UPDATE Depositos_Racks SET deprack_x=@x, deprack_y=@y, deprack_largo=@largo, deprack_ancho=@ancho, deprack_fec_mod=@fecha, deprack_usu_id_mod=@usuario WHERE deprack_id=@id";
            List<SqlParameter> p = new List<SqlParameter>();
            p.Add(new SqlParameter("x", x));
            p.Add(new SqlParameter("y", y));
            p.Add(new SqlParameter("largo", largo));
            p.Add(new SqlParameter("ancho", ancho));
            p.Add(new SqlParameter("fecha", DateTime.Now));
            p.Add(new SqlParameter("usuario", Token.UserID));
            p.Add(new SqlParameter("id", id));
            db.SQLExecuteNonQuery(query, p);
        }

        public int ObtenerID(string id)
        {
            if (Resources.Repositorio.IsNumeric(id))
                return Convert.ToInt32(id);

            if (!string.IsNullOrWhiteSpace(id))
                return Convert.ToInt32(App.Security.DesencriptarID(id));

            return 0;
        }

        private string ObtenerCodigoDeposito(int depoID)
        {
            if (depoID <= 0) return "";
            Deposito d = new Depositos(Token).ObtenerPorID(depoID.ToString());
            return d != null ? d.depo_codigo : "";
        }

        private string ObtenerCodigoZona(int zonaID)
        {
            if (zonaID <= 0) return "";
            ZonaLogistica z = new ZonasLogisticas(Token).ObtenerPorID(zonaID.ToString());
            return z != null ? z.zonlog_codigo : "";
        }

        private string ObtenerCodigoPasillo(int pasilloID)
        {
            if (pasilloID <= 0) return "";
            DepositoPasillo p = new DepositosPasillos(Token).ObtenerPorID(pasilloID.ToString());
            return p != null ? p.depopas_codigo : "";
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            List<DLLObject> lst = new List<DLLObject>();
            if (agregaDefault)
                lst.Add(new DLLObject() { Value = "0", Text = "Seleccione" });

            foreach (var item in ListarActivos())
                lst.Add(new DLLObject() { Value = item.deprack_id.ToString(), Text = item.deprack_codigo + " - " + item.deprack_nombre });

            return lst;
        }

        public override DepositoRack Mapear(DataRow dr)
        {
            DepositoRack obj = MapearSimple(dr);
            obj.Deposito = Depositos.MapearStatic(dr);
            return obj;
        }

        public override DepositoRack MapearCompleto(DataRow dr)
        {
            return Mapear(dr);
        }

        public override DepositoRack MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static DepositoRack MapearStatic(DataRow dr)
        {
            DepositoRack obj = new DepositoRack();
            obj = MapearReflection(obj, dr);
            return obj;
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT " + sTOP + " * FROM Depositos_Racks ";
            sQuery += " LEFT JOIN Depositos ON depo_id = deprack_depo_id ";

            if ((sWHERE != ""))
                sQuery += " WHERE " + sWHERE;

            if ((sOrderBy != ""))
                sQuery += " ORDER BY " + sOrderBy;

            return sQuery;
        }
    }
}
