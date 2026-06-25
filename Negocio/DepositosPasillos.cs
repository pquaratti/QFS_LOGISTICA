using Entidades;
using Entidades.App;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Negocio
{
    public class DepositosPasillos : NegocioBase<Entidades.DepositoPasillo>
    {
        public DepositosPasillos(Entidades.App.Token paramToken) : base("depopas_id", "depopas_activo", "Depositos_Pasillos", "depopas")
        {
            Token = paramToken;
            TokenFilter = true;
        }

        public override DepositoPasillo ObjetoNuevo()
        {
            DepositoPasillo obj = new DepositoPasillo();
            obj.depopas_id = 0;
            obj.IdEncriptado = App.Security.EncriptarID(Convert.ToString(obj.depopas_id));
            return obj;
        }

        public override void PermiteGuardar(DepositoPasillo obj)
        {
            if (obj.Deposito == null || obj.Deposito.depo_id <= 0)
                throw new Exception("Debe seleccionar un depósito.");

            if (string.IsNullOrWhiteSpace(obj.depopas_codigo))
                throw new Exception("Debe ingresar el código del pasillo.");

            if (string.IsNullOrWhiteSpace(obj.depopas_nombre))
                obj.depopas_nombre = obj.depopas_codigo;

            if (obj.depopas_cantidad_posiciones <= 0)
                throw new Exception("La cantidad de posiciones debe ser mayor a cero.");

            if (obj.depopas_cantidad_alturas <= 0)
                throw new Exception("La cantidad de alturas debe ser mayor a cero.");

            if (obj.depopas_largo <= 0)
                obj.depopas_largo = 240;

            if (obj.depopas_ancho <= 0)
                obj.depopas_ancho = 60;

            if (string.IsNullOrWhiteSpace(obj.depopas_orientacion))
                obj.depopas_orientacion = "H";
        }

        public override ObjectMessage Save(DepositoPasillo Obj)
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

        public ObjectMessage SaveYGenerarUbicaciones(DepositoPasillo obj)
        {
            ObjectMessage oM = Save(obj);

            if (oM.Success)
            {
                if (obj.depopas_id <= 0 && oM.ObjectRelation != null && !string.IsNullOrWhiteSpace(Convert.ToString(oM.ObjectRelation)))
                    obj.depopas_id = Convert.ToInt32(App.Security.DesencriptarID(Convert.ToString(oM.ObjectRelation)));

                if (obj.depopas_id <= 0)
                {
                    oM.Success = false;
                    oM.Message = "No se pudo obtener el identificador del pasillo guardado.";
                    return oM;
                }

                oM = new UbicacionesLogisticas(Token).RegenerarDesdePasillo(obj);
            }

            return oM;
        }


        public ObjectMessage DeleteLogicoConUbicaciones(string valorId)
        {
            int pasilloID = ObtenerID(valorId);
            new UbicacionesLogisticas(Token).DesactivarUbicacionesPasillo(pasilloID);
            return DeleteLogico(valorId);
        }

        public List<DepositoPasillo> ListarPorDeposito(string depositoID)
        {
            int depoID = ObtenerID(depositoID);
            List<ObjectParameter> filtros = new List<ObjectParameter>();
            filtros.Add(new ObjectParameter() { Name = "depopas_depo_id", Value = depoID });
            filtros.Add(new ObjectParameter() { Name = "depopas_activo", Value = 1 });
            return ListarConFiltros(filtros, "", "depopas_codigo");
        }

        public List<DepositoPasillo> ListarConFiltros(List<ObjectParameter> listaFiltros, string cantRegistros, string orden)
        {
            List<DepositoPasillo> lst = new List<DepositoPasillo>();
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

        public int ObtenerID(string id)
        {
            if (Resources.Repositorio.IsNumeric(id))
                return Convert.ToInt32(id);

            if (!string.IsNullOrWhiteSpace(id))
                return Convert.ToInt32(App.Security.DesencriptarID(id));

            return 0;
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            List<DLLObject> lst = new List<DLLObject>();
            if (agregaDefault)
                lst.Add(new DLLObject() { Value = "0", Text = "Seleccione" });

            foreach (var item in ListarActivos())
                lst.Add(new DLLObject() { Value = item.depopas_id.ToString(), Text = item.depopas_codigo + " - " + item.depopas_nombre });

            return lst;
        }

        public override DepositoPasillo Mapear(DataRow dr)
        {
            DepositoPasillo obj = MapearSimple(dr);
            obj.Deposito = Depositos.MapearStatic(dr);
            return obj;
        }

        public override DepositoPasillo MapearCompleto(DataRow dr)
        {
            return Mapear(dr);
        }

        public override DepositoPasillo MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static DepositoPasillo MapearStatic(DataRow dr)
        {
            DepositoPasillo obj = new DepositoPasillo();
            obj = MapearReflection(obj, dr);
            return obj;
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT " + sTOP + " * FROM Depositos_Pasillos ";
            sQuery += " LEFT JOIN Depositos ON depo_id = depopas_depo_id ";

            if ((sWHERE != ""))
                sQuery += " WHERE " + sWHERE;

            if ((sOrderBy != ""))
                sQuery += " ORDER BY " + sOrderBy;

            return sQuery;
        }
    }
}