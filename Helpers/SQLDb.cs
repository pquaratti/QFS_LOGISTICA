using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace Helpers
{
    public class SQLDb
    {
        public string DBConnectionString { get; set; }

        public SQLDb(string stringSQL)
        {
            DBConnectionString = stringSQL;
        }

        public SQLDb()
        {
            DBConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
        }
        
        public DataTable SQLSelect(string pQuery, List<SqlParameter> pSQLParameters = null)
        {
            SqlConnection objConn = MyConnection();

            using (var connection = objConn)
            {
                DataSet oDS = new DataSet();

                using (var oDA = new SqlDataAdapter(pQuery, connection))
                {
                    if (pSQLParameters != null)
                    {
                        foreach (SqlParameter itemParam in pSQLParameters)
                        {
                            oDA.SelectCommand.Parameters.Add(itemParam);
                        }
                    }
                    oDA.Fill(oDS);
                }
                return oDS.Tables[0];
            }

        }

        public DataSet SQLSelectDS(string pQuery, List<SqlParameter> pSQLParameters = null)
        {
            SqlConnection objConn = MyConnection();

            using (var connection = objConn)
            {
                DataSet oDS = new DataSet();

                using (var oDA = new SqlDataAdapter(pQuery, connection))
                {
                    if (pSQLParameters != null)
                    {
                        foreach (SqlParameter itemParam in pSQLParameters)
                        {
                            oDA.SelectCommand.Parameters.Add(itemParam);
                        }
                    }
                    oDA.Fill(oDS);
                }

                return oDS;
            }

        }

        public DataRow Estructura(string pTableName)
        {
            try
            {
                DataTable dtDatos = SQLSelect("SELECT TOP 0 * FROM " + pTableName);
                dtDatos.TableName = pTableName;
                dtDatos.Rows.Add();
                return dtDatos.Rows[0];
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public QFSOMessage SQLInsert(DataRow pDR, string pPK = "")
        {
            QFSOMessage oMessage = new QFSOMessage();

            SqlConnection objConn = MyConnection();

            try
            {
                string sSQL = "";

                // Comenzamos a escribir la consulta Insert into
                sSQL = " INSERT INTO " + pDR.Table.TableName.ToString() + " (";

                //Por cada columna del datarow sigue construyendo la consulta sql con los nombre de las columnas
                //menos el id porque daria error de sql.

                foreach (DataColumn iCOL in pDR.Table.Columns)
                {
                    if (pPK != iCOL.ColumnName)
                    {
                        if (pDR[iCOL.ColumnName] != DBNull.Value)
                        {
                            sSQL += iCOL.ColumnName + ",";
                        }
                    }
                }

                // Sacamos la última coma(,) que viene de la creación de campos.
                sSQL = sSQL.Substring(0, sSQL.Length - 1) + ") VALUES (";

                // Creamos un objeto command que nos permitirá ejecutar la consulta.
                SqlCommand oCommand = new SqlCommand(sSQL, objConn);

                // Recorro de vuelta el datarow y voy creando los campos a insertar.
                foreach (DataColumn iCOL in pDR.Table.Columns)
                {
                    if (pPK != iCOL.ColumnName)
                    {
                        if (pDR[iCOL.ColumnName] != DBNull.Value)
                        {
                            SqlParameter oParam = new SqlParameter();
                            sSQL += "@" + iCOL.ColumnName + ",";
                            oParam.ParameterName = iCOL.ColumnName;
                            oParam.Value = pDR[iCOL.ColumnName];
                            oCommand.Parameters.Add(oParam);
                        }
                    }

                }

                // Sacamos la última coma(,) que viene de la creación de valores
                sSQL = sSQL.Substring(0, sSQL.Length - 1) + ")";

                oCommand.CommandType = CommandType.Text;
                oCommand.CommandText = sSQL;
                oCommand.ExecuteNonQuery();

                if (pPK.Trim().Length > 0)
                {
                    oCommand.CommandText = "SELECT @@IDENTITY as maximo";
                    oMessage.Valor = Convert.ToInt32(oCommand.ExecuteScalar().ToString());
                    oMessage.State = QFSEnum.StateMessage.Success;
                }
                else
                {
                    oMessage.Valor = 0;
                    oMessage.State = QFSEnum.StateMessage.NotSet;
                }

                CerrarConexion(ref objConn);

            }
            catch (Exception ex)
            {
                CerrarConexion(ref objConn);
                oMessage.TextMessage = ex.Message;
                oMessage.ObjectResponse = null;
                oMessage.Valor = -1;
                oMessage.State = QFSEnum.StateMessage.Exception;
                throw;
            }

            return oMessage;
        }

        public QFSOMessage SQLUpdate(DataRow pDR, string pCondicion, string pPK = "", List<SqlParameter> paramCondicion = null)
        {
            QFSOMessage oMessage = new QFSOMessage();

            string sSQL = "";
            SqlConnection objConn = MyConnection();

            try
            {
                sSQL = "UPDATE " + pDR.Table.TableName.ToString().Trim() + " SET ";

                foreach (DataColumn iCOL in pDR.Table.Columns)
                {
                    if (pPK != iCOL.ColumnName & pDR[iCOL.ColumnName] != DBNull.Value)
                    {
                        sSQL += iCOL.ColumnName + "=@SET_" + iCOL.ColumnName + ",";
                    }
                }

                sSQL = sSQL.Substring(0, sSQL.Length - 1) + " WHERE " + pCondicion;

                // Creamos un objeto command que nos permitirá ejecutar la consulta.
                SqlCommand oCommand = new SqlCommand(sSQL, objConn);

                foreach (DataColumn iCOL in pDR.Table.Columns)
                {
                    if (pPK != iCOL.ColumnName & pDR[iCOL.ColumnName] != DBNull.Value)
                    {
                        SqlParameter oParam = new SqlParameter();
                        oParam.ParameterName = "SET_" + iCOL.ColumnName;
                        oParam.Value = pDR[iCOL.ColumnName];

                        // Agrego el parámetro al command
                        oCommand.Parameters.Add(oParam);
                    }
                }

                if (paramCondicion != null)
                {
                    foreach (SqlParameter itemParam in paramCondicion)
                    {
                        oCommand.Parameters.Add(itemParam);
                    }
                }

                oMessage.Valor = oCommand.ExecuteNonQuery();

                if (oMessage.Valor > 0)
                {
                    oMessage.State = QFSEnum.StateMessage.Success;
                    oMessage.TextMessage = "Actualizado Exitosamente";
                }
                else
                {
                    oMessage.State = QFSEnum.StateMessage.NotSet;
                    oMessage.TextMessage = "No se actualizó ningun datos";
                }

                CerrarConexion(ref objConn);

            }
            catch (Exception ex)
            {
                CerrarConexion(ref objConn);
                oMessage.TextMessage = ex.Message;
                oMessage.ObjectResponse = null;
                oMessage.Valor = -1;
                oMessage.State = QFSEnum.StateMessage.Exception;

                throw;
            }

            return oMessage;
        }

        public QFSOMessage SQLExecuteNonQuery(string pQuery, List<SqlParameter> pSQLParameters = null)
        {
            string strSQLADD = "";
            strSQLADD += "SET ROWCOUNT 0 ";
            strSQLADD += "SET TEXTSIZE 2147483647 ";
            strSQLADD += "SET NOCOUNT OFF ";
            strSQLADD += "SET CONCAT_NULL_YIELDS_NULL ON ";
            strSQLADD += "SET ARITHABORT On ";
            strSQLADD += "SET LOCK_TIMEOUT -1 ";
            strSQLADD += "SET QUERY_GOVERNOR_COST_LIMIT 0 ";
            strSQLADD += "SET DEADLOCK_PRIORITY NORMAL ";
            strSQLADD += "SET TRANSACTION ISOLATION LEVEL READ COMMITTED ";
            strSQLADD += "SET ANSI_NULLS ON ";
            strSQLADD += "SET ANSI_NULL_DFLT_ON On ";
            strSQLADD += "SET ANSI_PADDING ON ";
            strSQLADD += "SET ANSI_WARNINGS On ";
            strSQLADD += "SET CURSOR_CLOSE_ON_COMMIT OFF ";
            strSQLADD += "SET IMPLICIT_TRANSACTIONS OFF ";
            strSQLADD += "SET QUOTED_IDENTIFIER ON ";
            strSQLADD += "SET NOEXEC, PARSEONLY, FMTONLY OFF ";

            pQuery = strSQLADD + pQuery;
            
            QFSOMessage oMessage = new QFSOMessage();

            SqlConnection objConn = MyConnection();

            try
            {
                // Creamos un objeto command que nos permitirá ejecutar la consulta.
                SqlCommand oCommand = new SqlCommand(pQuery, objConn);

                if (pSQLParameters != null)
                {
                    foreach (SqlParameter itemParam in pSQLParameters)
                    {
                        oCommand.Parameters.Add(itemParam);
                    }

                }

                oMessage.Valor = oCommand.ExecuteNonQuery();

                if (oMessage.Valor > 0)
                {
                    oMessage.State = QFSEnum.StateMessage.Success;
                    oMessage.TextMessage = "Actualización realizada Exitosamente";
                }
                else
                {
                    oMessage.State = QFSEnum.StateMessage.NotSet;
                    oMessage.TextMessage = "No se actualizó ningun dato";
                }

                CerrarConexion(ref objConn);

            }
            catch (Exception ex)
            {
                CerrarConexion(ref objConn);
                oMessage.TextMessage = ex.Message;
                oMessage.ObjectResponse = null;
                oMessage.Valor = -1;
                oMessage.State = QFSEnum.StateMessage.Exception;

                throw;
            }

            return oMessage;

        }

        private SqlConnection MyConnection()
        {
            try
            {
                SqlConnection objConn;

                objConn = new SqlConnection(this.DBConnectionString);
                objConn.Open();

                return objConn;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        private void CerrarConexion(ref SqlConnection oConn)
        {
            try
            {
                oConn.Close();
                oConn.Dispose();
            }
            catch (Exception)
            {
                throw;
            }
        }
        
    }
}




