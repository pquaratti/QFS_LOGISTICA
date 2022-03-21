using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using Entidades;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public abstract class NegocioBase<T>
    {
        public Helpers.SQLDb db;
        public string fieldID;
        public string sQuery = "";
        public string nombreTablaPrincipal = "";
        public string fieldActivo = "";
        public string nombrePrefijoCampo = "";
        public Entidades.App.Token Token { get; set; }
        public bool TokenFilter = false;

        protected NegocioBase(string _fieldID = "", string _fieldActivo = "", string _nombreTablaPrincipal = "", string _nombrePrefijoCampo = "")
        {
            fieldID = _fieldID;
            fieldActivo = _fieldActivo;
            nombreTablaPrincipal = _nombreTablaPrincipal;
            nombrePrefijoCampo = _nombrePrefijoCampo;
            db = new Helpers.SQLDb();
        }

        protected NegocioBase(Helpers.SQLDb dbInject, string _fieldID = "", string _fieldActivo = "", string _nombreTablaPrincipal = "", string _nombrePrefijoCampo = "")
        {
            fieldID = _fieldID;
            fieldActivo = _fieldActivo;
            nombreTablaPrincipal = _nombreTablaPrincipal;
            nombrePrefijoCampo = _nombrePrefijoCampo;
            db = dbInject;
        }

        public abstract Entidades.App.ObjectMessage Save(T Obj);

        public T ObtenerPorID(string id, bool objetoCompleto = false)
        {
            var _id = 0;
            if (Resources.Repositorio.IsNumeric(id))
                _id = Convert.ToInt32(id);
            else
            if (id != "")
                _id = Convert.ToInt32(Negocio.App.Security.DesencriptarID(id));
            
            List<System.Data.SqlClient.SqlParameter> lstParam = new List<System.Data.SqlClient.SqlParameter>();
            lstParam.Add(new System.Data.SqlClient.SqlParameter("value", _id));

            var _QueryFilter = GetFilterTokenQuery();
            var _CustomWhere = fieldID + "=@value ";

            if (_QueryFilter.Trim().Length > 0)
            {
                _CustomWhere += " and " +_QueryFilter;
                lstParam.AddRange(GetFilterTokenParams());
            }

            sQuery = QueryDefault("", _CustomWhere, "");

            DataTable dt = db.SQLSelect(sQuery,lstParam);

            if (dt.Rows.Count > 0)
            {
                if (objetoCompleto)
                    return MapearCompleto(dt.Rows[0]);
                else
                    return Mapear(dt.Rows[0]);
            }
            else
            {
                return ObjetoNuevo();
                //throw new Exception("No se encuentra el objeto");
            }
        }

        public T ObtenerPorIDEncriptado(string id, bool objetoCompleto = false)
        {
            id = Negocio.App.Security.DesencriptarID(id);

            List<System.Data.SqlClient.SqlParameter> lstParam = new List<System.Data.SqlClient.SqlParameter>();
            lstParam.Add(new System.Data.SqlClient.SqlParameter("value", id));

            var _QueryFilter = GetFilterTokenQuery();
            var _CustomWhere = fieldID + "=@value ";

            if (_QueryFilter.Trim().Length > 0)
            {
                _CustomWhere += " and " + _QueryFilter;
                lstParam.AddRange(GetFilterTokenParams());
            }

            sQuery = QueryDefault("", _CustomWhere, "");

            DataTable dt = db.SQLSelect(sQuery,lstParam);

            if (dt.Rows.Count > 0)
            {
                if (objetoCompleto)
                    return MapearCompleto(dt.Rows[0]);
                else
                    return Mapear(dt.Rows[0]);
            }
            else
            {
                return ObjetoNuevo();
            }
        }

        public List<T> Listar(string sTOP = "", string sWHERE = "", string sOrderBy = "")
        {
            List<T> lst = new List<T>();

            List<System.Data.SqlClient.SqlParameter> lstParam = new List<System.Data.SqlClient.SqlParameter>();

            var _sWhere = sWHERE;
            var _queryFilter = GetFilterTokenQuery();

            if (_queryFilter.Trim().Length > 0)
            {
                if (_sWhere.Trim().Length > 0)
                    _sWhere += " and ";

                _sWhere += _queryFilter;
                lstParam.AddRange(GetFilterTokenParams());
            }

            sQuery = QueryDefault(sTOP, _sWhere, sOrderBy);

            DataTable dt = db.SQLSelect(sQuery, lstParam);

            foreach (DataRow row in dt.Rows)
            {
                lst.Add(Mapear(row));
            }

            return lst;
        }


        public abstract T MapearSimple(DataRow dr);
        public abstract T Mapear(DataRow dr);
        public abstract T MapearCompleto(DataRow dr);
        public abstract T ObjetoNuevo();

        public virtual int MaximoID()
        {
            return 0;
        }

        protected abstract string QueryDefault(string sTOP, string sWHERE, string sOrderBy);

        public Entidades.App.ObjectMessage Delete(int valorId)
        {
            List<System.Data.SqlClient.SqlParameter> lstParam = new List<System.Data.SqlClient.SqlParameter>();
            Entidades.App.ObjectMessage obj = new Entidades.App.ObjectMessage();

            var _QueryFilter = GetFilterTokenQuery();

            lstParam.Add(new System.Data.SqlClient.SqlParameter("id", valorId));
            
            if (_QueryFilter.Trim().Length > 0)
            {
                sQuery = "  DELETE " + nombreTablaPrincipal + " WHERE " + fieldID + " = @id and " + _QueryFilter;
                lstParam.AddRange(GetFilterTokenParams());
            }
            else
                sQuery = "  DELETE " + nombreTablaPrincipal + " WHERE " + fieldID + " = @id ";

            try
            {
                db.SQLExecuteNonQuery(sQuery,lstParam);

                obj.Success = true;

            }
            catch (Exception Ex)
            {
                obj.Success = false;
                obj.Message = Ex.Message;
            }
            return obj;
        }
        public Entidades.App.ObjectMessage DeleteLogico(string valorId)
        {
            var _id = 0;

            if (Resources.Repositorio.IsNumeric(valorId))
                _id = Convert.ToInt32(valorId);
            else
                _id = Convert.ToInt32(Negocio.App.Security.DesencriptarID(valorId));


            var _usuarioID = Convert.ToInt32(Token.UserID);
            return DeleteLogico(_usuarioID, _id);
        }
        public Entidades.App.ObjectMessage DeleteLogico(int usuario, int valorId)
        {
            Entidades.App.ObjectMessage obj = new Entidades.App.ObjectMessage();

            List<System.Data.SqlClient.SqlParameter> lstParam = new List<System.Data.SqlClient.SqlParameter>();
            
            var _QueryFilter = GetFilterTokenQuery();

            lstParam.Add(new System.Data.SqlClient.SqlParameter("id", valorId));
            lstParam.Add(new System.Data.SqlClient.SqlParameter("valor", int.Parse("0")));
            lstParam.Add(new System.Data.SqlClient.SqlParameter("date", DateTime.Now));
            lstParam.Add(new System.Data.SqlClient.SqlParameter("usu", usuario));

            sQuery = "UPDATE " + nombreTablaPrincipal + " SET " + fieldActivo + " = @valor, " + nombrePrefijoCampo + "_usu_id_baja = @usu, " + nombrePrefijoCampo + "_fec_baja = @date  WHERE " + fieldID + " = @id ";

            if (_QueryFilter.Trim().Length > 0)
            {
                sQuery += " and " + _QueryFilter;
                lstParam.AddRange(GetFilterTokenParams());
            }
            
            try
            {
                db.SQLExecuteNonQuery(sQuery,lstParam);

                obj.Success = true;

            }
            catch (Exception Ex)
            {
                obj.Success = false;
                obj.Message = Ex.Message;
            }
            return obj;
        }

        public Entidades.App.ObjectMessage RecuperarLogico(string valorId)
        {
            var _id = 0;

            if (Resources.Repositorio.IsNumeric(valorId))
                _id = Convert.ToInt32(valorId);
            else
                _id = Convert.ToInt32(Negocio.App.Security.DesencriptarID(valorId));

            var _usuarioID = Convert.ToInt32(Token.ID);
            return RecuperarLogico(_usuarioID, _id);
        }

        public Entidades.App.ObjectMessage RecuperarLogico(int usuario, int valorId)
        {
            Entidades.App.ObjectMessage obj = new Entidades.App.ObjectMessage();

            List<System.Data.SqlClient.SqlParameter> lstParam = new List<System.Data.SqlClient.SqlParameter>();

            var _QueryFilter = GetFilterTokenQuery();

            lstParam.Add(new System.Data.SqlClient.SqlParameter("id", valorId));
            lstParam.Add(new System.Data.SqlClient.SqlParameter("valor", int.Parse("0")));
            lstParam.Add(new System.Data.SqlClient.SqlParameter("date", DateTime.Now));
            lstParam.Add(new System.Data.SqlClient.SqlParameter("usu", usuario));

            sQuery = "  UPDATE " + nombreTablaPrincipal + " SET " + fieldActivo + " = @valor, " + nombrePrefijoCampo + "_usu_id_mod=@usu," + nombrePrefijoCampo + "_fec_mod=@date," + nombrePrefijoCampo + "_usu_id_baja = NULL, " + nombrePrefijoCampo + "_fec_baja = NULL  WHERE " + fieldID + " = @id ";

            if (_QueryFilter.Trim().Length > 0)
            {
                sQuery = " and " + _QueryFilter;
                lstParam.AddRange(GetFilterTokenParams());
            }

            try
            {
                db.SQLExecuteNonQuery(sQuery, lstParam);
                obj.Success = true;
            }
            catch (Exception Ex)
            {
                obj.Success = false;
                obj.Message = Ex.Message;
            }
            return obj;
        }

        public List<T> ListarActivos(bool objetoCompleto = false)
        {
            List<T> lst = new List<T>();
            
            List<System.Data.SqlClient.SqlParameter> lstParam = new List<System.Data.SqlClient.SqlParameter>();

            var _sWhere = fieldActivo + "= 1 or " + fieldActivo + " is null ";
            var _queryFilter = GetFilterTokenQuery();

            if (_queryFilter.Trim().Length > 0)
            {
                _sWhere += " and " + _queryFilter;
                lstParam.AddRange(GetFilterTokenParams());
            }
                
            sQuery = QueryDefault("",_sWhere, "");

            DataTable dt = db.SQLSelect(sQuery, lstParam);

            foreach (DataRow row in dt.Rows)
            {
                if (objetoCompleto == true)
                    lst.Add(MapearCompleto(row));
                else
                    lst.Add(Mapear(row));
            }

            return lst;
        }

        public List<T> ListarSimple()
        {
            List<T> lst = new List<T>();

            List<System.Data.SqlClient.SqlParameter> lstParam = new List<System.Data.SqlClient.SqlParameter>();

            var _sWhere = "";
            var _queryFilter = GetFilterTokenQuery();

            if (_queryFilter.Trim().Length > 0)
            {
                _sWhere +=  _queryFilter;
                lstParam.AddRange(GetFilterTokenParams());
            }

            sQuery = QueryDefault("", _sWhere, "");

            DataTable dt = db.SQLSelect(sQuery, lstParam);

            foreach (DataRow row in dt.Rows)
            {
                lst.Add(MapearSimple(row));
            }
            return lst;
        }

        public List<T> ListarConFiltros(List<Entidades.App.ObjectParameter> listaFiltros, string cantRegistros = "")
        {
            List<T> lst = new List<T>();

            var _queryFilter = GetFilterTokenQuery();
            string sWhere = "";
            
            foreach (var itemFiltro in listaFiltros)
            {
                sWhere += itemFiltro.Name + "=@" + itemFiltro.Name + " and ";
            }

            sWhere += " 1=1 ";

            List<System.Data.SqlClient.SqlParameter> lstParamsSQL = new List<System.Data.SqlClient.SqlParameter>();

            foreach (var itemParam in listaFiltros)
            {
                lstParamsSQL.Add(new System.Data.SqlClient.SqlParameter(itemParam.Name, itemParam.Value));
            }

            
            if (_queryFilter.Trim().Length > 0)
            {
                sWhere += " and " + _queryFilter;
                lstParamsSQL.AddRange(GetFilterTokenParams());
            }

            sQuery = QueryDefault(cantRegistros, sWhere, "");

            DataTable dt = db.SQLSelect(sQuery, lstParamsSQL);

            foreach (DataRow row in dt.Rows)
            {
                lst.Add(Mapear(row));
            }

            return lst;
        }

        public bool ExisteRegistro(List<Entidades.App.ObjectParameter> listaFiltros)
        {
            string sWhere = "";
            foreach (var itemFiltro in listaFiltros)
            {
                sWhere += itemFiltro.Name + "=@" + itemFiltro.Name + " and ";
            }
            sWhere += " 1=1 ";

            List<System.Data.SqlClient.SqlParameter> lstParamsSQL = new List<System.Data.SqlClient.SqlParameter>();

            foreach (var itemParam in listaFiltros)
            {
                lstParamsSQL.Add(new System.Data.SqlClient.SqlParameter(itemParam.Name, itemParam.Value));
            }

            var _queryFilter = GetFilterTokenQuery();
            if (_queryFilter.Trim().Length > 0)
            {
                sWhere += " and " + _queryFilter;
                lstParamsSQL.AddRange(GetFilterTokenParams());
            }

            sQuery = QueryDefault("", sWhere, "");

            DataTable dt = db.SQLSelect(sQuery, lstParamsSQL);

            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool ExisteRegistroSigno(List<Entidades.App.ObjectParameterSigno> listaFiltros)
        {
            string sWhere = "";
            foreach (var itemFiltro in listaFiltros)
            {
                sWhere += itemFiltro.Name + itemFiltro.Signo + "@" + itemFiltro.Name + " and ";
            }
            sWhere += " 1=1 ";

            List<System.Data.SqlClient.SqlParameter> lstParamsSQL = new List<System.Data.SqlClient.SqlParameter>();

            foreach (var itemParam in listaFiltros)
            {
                lstParamsSQL.Add(new System.Data.SqlClient.SqlParameter(itemParam.Name, itemParam.Value));
            }

            var _queryFilter = GetFilterTokenQuery();
            if (_queryFilter.Trim().Length > 0)
            {
                sWhere += " and " + _queryFilter;
                lstParamsSQL.AddRange(GetFilterTokenParams());
            }

            sQuery = QueryDefault("", sWhere, "");

            DataTable dt = db.SQLSelect(sQuery, lstParamsSQL);

            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public abstract List<Entidades.App.DLLObject> ListarDLL(bool agregaDefault = false);

        public virtual List<T> ListarParaTableAjax(Entidades.App.DatatableJS datatableFilters)
        {
            return null;
        }

        public virtual void DatosObligatorios(T obj)
        {

        }

        public virtual void PermiteGuardar(T obj)
        {

        }

        public T ObtenerPorIDSimple(string id)
        {
            var _id = 0;
            if (Resources.Repositorio.IsNumeric(id))
                _id = Convert.ToInt32(id);
            else
            if (id != "")
            {
                _id = Convert.ToInt32(Negocio.App.Security.DesencriptarID(id));
            }

            List<System.Data.SqlClient.SqlParameter> lstParams = new List<System.Data.SqlClient.SqlParameter>();
            lstParams.Add(new System.Data.SqlClient.SqlParameter("value", _id));

            var sWhere = fieldID + "=@value ";
            var _queryFilter = GetFilterTokenQuery();
            if (_queryFilter.Trim().Length > 0)
            {
                sWhere += " and " + _queryFilter;
                lstParams.AddRange(GetFilterTokenParams());
            }

            sQuery = QueryDefault("", sWhere, "");

            DataTable dt = db.SQLSelect(sQuery, lstParams);

            if (dt.Rows.Count > 0)
            {
                return MapearSimple(dt.Rows[0]);
            }
            else
            {
                return ObjetoNuevo();
                //throw new Exception("No se encuentra el objeto");
            }
        }

        #region REFLECTION
        public Entidades.App.ObjectMessage SaveReflection(T item, DataRow row, bool conSeguimiento = false)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            var pList = item.GetType().GetProperties();
            var fkpList = pList.Where(x => x.CustomAttributes.Any(attr => attr.AttributeType == typeof(KeyRelationAttribute))).ToList();
            var itemID = pList.Where(x => x.CustomAttributes.Any(attr => attr.AttributeType == typeof(KeyAttribute))).FirstOrDefault();
            var idEncriptado = pList.Where(x => x.Name == "IdEncriptado").FirstOrDefault();

            foreach (DataColumn c in row.Table.Columns)
            {
                System.Reflection.PropertyInfo p = pList.Where(x => x.Name == c.ColumnName).FirstOrDefault();

                if (p != null)
                    if (p.GetValue(item) != null)
                        row[c] = p.GetValue(item);
            }

            foreach (System.Reflection.PropertyInfo prop in fkpList)
            {
                var obj = prop.GetValue(item);
                var pId = obj.GetType().GetProperties().Where(x =>
                   x.CustomAttributes.Any(
                       attr => attr.AttributeType == typeof(KeyAttribute))).FirstOrDefault();
                if (pId != null)
                    row[nombrePrefijoCampo + "_" + pId.Name] = pId.GetValue(obj);
            }

            if (itemID.GetValue(item).Equals(0))
            {
                if (conSeguimiento)
                {
                    row[nombrePrefijoCampo + "_" + "fec_alta"] = DateTime.Now;
                    row[nombrePrefijoCampo + "_" + "fec_mod"] = DateTime.Now;
                    row[nombrePrefijoCampo + "_" + "usu_id_alta"] = Token.UserID;
                }

                ///CAMPOS DEL TOKEN ASOCIADOS A LA ENTIDAD//////////////////////////////////
                row = SaveTokenFilters(row);
                ////////////////////////////////////////////////////////////////////////////

                int idvalue = db.SQLInsert(row, fieldID).Valor;
                itemID.SetValue(item, idvalue, null);
                idEncriptado.SetValue(item, Negocio.App.Security.EncriptarID(Convert.ToString(idvalue)), null);
                oM.Message = "Datos ingresados";
            }
            else
            {
                if (conSeguimiento)
                {
                    row[nombrePrefijoCampo + "_" + "fec_mod"] = DateTime.Now;
                    row[nombrePrefijoCampo + "_" + "usu_id_mod"] = Token.UserID;
                }

                db.SQLUpdate(row, fieldID + "=@" + fieldID, fieldID, new List<System.Data.SqlClient.SqlParameter>() {
                        new System.Data.SqlClient.SqlParameter(fieldID,itemID.GetValue(item))
                    });

                oM.Message = "Datos actualizados";
            }
            oM.ObjectRelation = idEncriptado.GetValue(item);
            oM.Success = true;

            return oM;
        }

        public static T MapearReflection(T item, DataRow row)
        {
            var pList = item.GetType().GetProperties();
            string idEncriptado = "";
            var propID = pList.Where(x => x.CustomAttributes.Any(attr => attr.AttributeType == typeof(KeyAttribute))).FirstOrDefault();
            string propIDName = propID.Name;
            string prefixID = propID.Name.Split('_')[0];
            idEncriptado = Negocio.App.Security.EncriptarID(
                        Resources.Validaciones.valNULLString(row[propIDName]));
            var fkpList = pList.Where(x => x.CustomAttributes.Any(attr => attr.AttributeType == typeof(KeyRelationAttribute))).ToList();
            System.Reflection.PropertyInfo p;

            foreach (DataColumn c in row.Table.Columns)
            {
                p = pList.Where(x => x.Name == c.ColumnName).FirstOrDefault();

                if (p != null & row[c] != DBNull.Value)
                    p.SetValue(item, row[c], null);
            }
            pList.Where(x => x.Name == "IdEncriptado").FirstOrDefault()
                    .SetValue(item, idEncriptado, null);


            foreach (System.Reflection.PropertyInfo prop in fkpList)
            {
                var obj = Activator.CreateInstance(prop.PropertyType);
                var pId = obj.GetType().GetProperties().Where(x =>
                   x.CustomAttributes.Any(
                       attr => attr.AttributeType == typeof(KeyAttribute))).FirstOrDefault();
                var pIdEncriptado = item.GetType().GetProperties().Where(x => x.Name == "IdEncriptado").FirstOrDefault();

                if (pId != null & pIdEncriptado != null & row[prefixID + "_" + pId.Name] != DBNull.Value)
                {
                    pId.SetValue(obj, row[prefixID + "_" + pId.Name], null);
                    pIdEncriptado.SetValue(obj,
                        Negocio.App.Security.EncriptarID(
                               Resources.Validaciones.valNULLString(row[prefixID + "_" + pId.Name])), null);
                }
                prop.SetValue(item, obj, null);
            }
            return item;
        }




        public static T MapearReflectionID(T item, DataRow row)
        {
            var pId = item.GetType().GetProperties().Where(x =>
                   x.CustomAttributes.Any(
                       attr => attr.AttributeType == typeof(KeyAttribute))).FirstOrDefault();
            var pIdEncriptado = item.GetType().GetProperties().Where(x => x.Name == "IdEncriptado").FirstOrDefault();

            if (pId != null && pIdEncriptado != null && row[pId.Name] != DBNull.Value)
            {
                pId.SetValue(item, row[pId.Name], null);
                pIdEncriptado.SetValue(item,
                    Negocio.App.Security.EncriptarID(
                           Resources.Validaciones.valNULLString(row[pId.Name])), null);
            }

            return item;
        }

        #endregion

        public string GetFilterTokenQuery()
        {
            string sQueryParams = "";

            if (TokenFilter)
                sQueryParams = nombrePrefijoCampo + "_org_id=@org_id ";

            return sQueryParams;
        }

        public List<System.Data.SqlClient.SqlParameter> GetFilterTokenParams()
        {
            List<System.Data.SqlClient.SqlParameter> lst = new List<System.Data.SqlClient.SqlParameter>();

            if (TokenFilter)
            {
                lst.Add(new System.Data.SqlClient.SqlParameter("org_id", Token.OrganizacionID));
            }

            return lst;
        }

        private DataRow SaveTokenFilters(DataRow row)
        {
            if (TokenFilter)
                foreach (var filter in GetFilterTokenParams())
                {
                    if (row.Table.Columns.Contains(nombrePrefijoCampo + "_" + filter.ParameterName))
                        row[nombrePrefijoCampo + "_" + filter.ParameterName] = filter.Value;
                }
            return row;
        }

    }
}
