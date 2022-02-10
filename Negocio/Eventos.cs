using Entidades;
using Entidades.App;
using Entidades.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class Eventos : NegocioBase<Entidades.Evento> , Negocio.App.INegocioAgendable
    {
        Negocio.Tipo_Eventos negocioEVET;
        public Eventos(Entidades.App.Token paramToken) : base("eve_id", "eve_activo", "Eventos", "eve")
        {
            Token = paramToken;
            negocioEVET = new Negocio.Tipo_Eventos(Token);
        }

        #region Funcionalidad

        public override Evento ObjetoNuevo()
        {
            Entidades.Evento obj = new Entidades.Evento();
            return obj;
        }

        public override void PermiteGuardar(Evento obj)
        {
            if (obj.eve_fecha<DateTime.Now)
                throw new Exception("La fecha ingresada no es válida.");
        }

        public override ObjectMessage Save(Evento Obj)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                PermiteGuardar(Obj);
                DataRow row = db.Estructura("Eventos");
                row["eve_id"] = Obj.eve_id;
                row["eve_titulo"] = Obj.eve_titulo;
                row["eve_evet_id"] = Obj.Tipo.evet_id;
                row["eve_objetivo"] = Obj.eve_objetivo;
                row["eve_link"] = Obj.eve_link;
                row["eve_pass"] = Obj.eve_pass;
                row["eve_duracion"] = Obj.eve_duracion;
                row["eve_fecha"] = Obj.eve_fecha;

                if (Obj.eve_id.Equals(0)) 
                {
                    row["eve_fec_creador"] = DateTime.Now;
                    row["eve_usu_creador"] = Token.UserID;
                    Obj.eve_id = db.SQLInsert(row, "eve_id").Valor;
                    oM.Message = "Datos ingresados";
                }
                else
                {
                    db.SQLUpdate(row, "eve_id=@eve_id", "eve_id", new List<System.Data.SqlClient.SqlParameter>() {
                        new System.Data.SqlClient.SqlParameter("eve_id",Obj.eve_id)
                    });

                    oM.Message = "Datos actualizados";
                }

                oM.Success = true;
            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.Message = ex.Message;
            }

            return oM;
        }

        public ObjectMessage DeleteEvento(int eve_id)
        {
            Entidades.App.ObjectMessage obj = new Entidades.App.ObjectMessage();

            Evento evento = ObtenerPorID(Convert.ToString(eve_id));

            if (evento.Cerrado | evento.Finalizado)
            {
                obj.Success = false;
                obj.Message = "No se puede borrar un evento cerrado.";
                return obj;
            }

            obj = Delete(eve_id);

            if (!obj.Success)
                return obj;

            sQuery = "DELETE Eventos_Renglones WHERE ever_eve_id=@eve_id";
            try
            {
                db.SQLExecuteNonQuery(sQuery, new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("eve_id", eve_id)
                });

                obj.Success = true;

            }
            catch (Exception Ex)
            {
                obj.Success = false;
                obj.Message = Ex.Message;
            }

            return obj;
        }

        public List<Evento> ListarPorTipoEvento(int evet_id)
        {
                sQuery = QueryDefault("", "", "");

                List<ObjectParameter> paramsFilter = new List<ObjectParameter>();

                if (evet_id > 0)
                    paramsFilter.Add(new ObjectParameter() { Name = "eve_evet_id", Value = evet_id });

                List<Entidades.Evento> eventos = new List<Entidades.Evento>();

                eventos = ListarConFiltros(paramsFilter);

                return eventos;
        }

           
        public Entidades.App.ObjectMessage CerrarEvento(string eve_id)
        {
            Entidades.App.ObjectMessage oM = new ObjectMessage();
            Negocio.EventosRenglones n = new Negocio.EventosRenglones(Token);
            
            //if (n.ListarRenglonesPorEvento(Convert.ToInt32(eve_id)).Count.Equals(0))
            //{
            //    oM.Success = false;
            //    oM.Message = "No puede cerrar un evento sin invitados";
            //    return oM;
            //}

            try
            {
                Entidades.Evento datosEvento = ObtenerPorID(eve_id, true);

                db.SQLExecuteNonQuery("UPDATE Eventos SET eve_fec_cerrado=GETDATE() WHERE eve_id=@eve_id", new List<System.Data.SqlClient.SqlParameter>() {
                    new System.Data.SqlClient.SqlParameter("eve_id",eve_id)
                });

                oM.Success = true;
                oM.Message = "OK";
            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.Message = ex.Message;
            }

            return oM;
        }

        #endregion

        #region Consultas y Listados

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public Entidades.Evento ObtenerPorID(int eve_id)
        {
            Entidades.Evento obj = new Entidades.Evento();
            sQuery = QueryDefault("", " eve_id = @eve_id", "");
            DataTable dt_movimiento = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter ("eve_id", eve_id)
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

        public List<Entidades.Evento> ListarEventosPorUsuario(int usu_id)
        {
            List<Entidades.Evento> lst = new List<Evento>();

            string _WHERE = "eve_id in (select ever_eve_id from Eventos_Renglones where ever_usu_id=@usu_id) ";

            sQuery = QueryDefault("", _WHERE, "");

            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("usu_id", usu_id)
            });

            foreach (DataRow row in dt_bus.Rows)
            {
                lst.Add(Mapear(row));
            }

            return lst;
        }


        #endregion 

        #region Mappers
        public override Evento Mapear(DataRow dr)
        {
            Entidades.Evento obj = MapearSimple(dr);
            obj.Tipo = negocioEVET.MapearSimple(dr);
            return obj;
        }

        public override Evento MapearCompleto(DataRow dr)
        {
            Entidades.Evento obj = Mapear(dr);
            return obj;
        }

        public override Evento MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static Evento MapearStatic(DataRow dr)
        {
            Entidades.Evento obj = new Entidades.Evento();
            obj.eve_id = Resources.Validaciones.valNULLINT(dr["eve_id"]);
            obj.Tipo = new Entidades.Tipo_Evento() { evet_id = Resources.Validaciones.valNULLINT(dr["eve_evet_id"]) };
            obj.eve_titulo = Resources.Validaciones.valNULLString(dr["eve_titulo"]);
            obj.eve_objetivo = Resources.Validaciones.valNULLString(dr["eve_objetivo"]);
            obj.eve_link = Resources.Validaciones.valNULLString(dr["eve_link"]);
            obj.eve_pass = Resources.Validaciones.valNULLString(dr["eve_pass"]);
            obj.eve_duracion = Resources.Validaciones.valNULLINT(dr["eve_duracion"]);
            obj.eve_fecha = Resources.Validaciones.valNULLDateTime(dr["eve_fecha"]);
            obj.eve_fec_cerrado = Resources.Validaciones.valNULLDate(dr["eve_fec_cerrado"]);
            obj.eve_fecha_fin = obj.eve_fecha.AddHours(obj.eve_duracion);

            if (dr["eve_fec_cerrado"] != DBNull.Value)
            {
                obj.Cerrado = true;
            }
            else
                obj.Cerrado = false;
            if (obj.eve_fecha < DateTime.Now.AddHours(obj.eve_duracion))
            {
                obj.Finalizado = true;
            }
            else
                obj.Finalizado = false;

            return obj;
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM Eventos ";
            sQuery += " LEFT JOIN Tipo_Evento on evet_id = eve_evet_id ";

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

        #endregion

        #region Implementa Agendable

        public fullCalendar CrearCalendario(string id)
        {
            throw new NotImplementedException();
        }

        public eventCalendar MaquetarAgendable(IAgendable item, eventCalendar oEvento)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
