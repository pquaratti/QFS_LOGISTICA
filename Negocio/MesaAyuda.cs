using Entidades;
using Entidades.App;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class MesaAyuda : NegocioBase<Entidades.Mesa_Ayuda>
    {
        Negocio.Tipo_Consulta_Ayuda negocioTCA;
        public MesaAyuda(Entidades.App.Token paramToken) : base("mesa_id", "", "Mesa_Ayuda", "mesa")
        {
            Token = paramToken;
            negocioTCA = new Tipo_Consulta_Ayuda(Token);
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public override Mesa_Ayuda Mapear(DataRow dr)
        {
            Entidades.Mesa_Ayuda obj = MapearSimple(dr);
            obj.UsuarioSolicita.usu_nombre = Resources.Validaciones.valNULLString(dr["usuariosolicita.usu_nombre"]);
            obj.UsuarioSolicita.usu_apellido = Resources.Validaciones.valNULLString(dr["usuariosolicita.usu_apellido"]);
            obj.UsuarioResponsable.usu_nombre = Resources.Validaciones.valNULLString(dr["usuarioresponsable.usu_nombre"]);
            obj.UsuarioResponsable.usu_apellido = Resources.Validaciones.valNULLString(dr["usuarioresponsable.usu_apellido"]);
            obj.TipoConsulta = negocioTCA.MapearSimple(dr);

            return obj;
        }

        public override Mesa_Ayuda MapearCompleto(DataRow dr)
        {
            Entidades.Mesa_Ayuda obj = Mapear(dr);
            return obj;
        }

        public override Mesa_Ayuda MapearSimple(DataRow dr)
        {
            Entidades.Mesa_Ayuda obj = new Entidades.Mesa_Ayuda();
            obj.mesa_id = Resources.Validaciones.valNULLINT(dr["mesa_id"]);
            obj.mesa_fecha = Resources.Validaciones.valNULLDateTime(dr["mesa_fecha"]);
            obj.mesa_problema = Resources.Validaciones.valNULLString(dr["mesa_problema"]);
            obj.mesa_solucion = Resources.Validaciones.valNULLString(dr["mesa_solucion"]);
            obj.TipoConsulta = new Entidades.Tipo_Consulta_Ayuda() { tipoconsulta_id = Resources.Validaciones.valNULLINT(dr["mesa_tipoconsulta_id"]) };

            if (dr["mesa_fec_cerrada"] != DBNull.Value)
            {
                obj.mesa_fec_cerrada = Convert.ToDateTime(dr["mesa_fec_cerrada"]);
                obj.Cerrada = true;
            }
            else
                obj.Cerrada = false;

            obj.UsuarioSolicita = new SIS_Usuario() { usu_id = Resources.Validaciones.valNULLINT(dr["mesa_usu_id_solicita"]) };
            obj.UsuarioResponsable = new SIS_Usuario() { usu_id = Resources.Validaciones.valNULLINT(dr["mesa_usu_id_responsable"]) };

            return obj;
        }

        public override Mesa_Ayuda ObjetoNuevo()
        {
            throw new NotImplementedException();
        }

        public override void PermiteGuardar(Mesa_Ayuda obj)
        {
            
        }

        public override ObjectMessage Save(Mesa_Ayuda Obj)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                PermiteGuardar(Obj);

                DataRow row = db.Estructura("Mesa_Ayuda");
                row["mesa_tipoconsulta_id"] = Obj.TipoConsulta.tipoconsulta_id;
                row["mesa_problema"] = Obj.mesa_problema;

                if (Obj.mesa_id.Equals(0))
                {
                    row["mesa_fecha"] = DateTime.Now;
                    row["mesa_usu_id_solicita"] = Convert.ToInt32(Token.UserID);
                    row["mesa_usu_id_alta"] = Convert.ToInt32(Token.UserID);
                    row["mesa_fec_alta"] = DateTime.Now;
                    Obj.mesa_id = db.SQLInsert(row, "mesa_id").Valor;
                    oM.Message = "Datos ingresados";
                }
                else
                {
                    row["mesa_usu_id_mod"] = Convert.ToInt32(Token.UserID);
                    row["mesa_fec_mod"] = DateTime.Now;
                    db.SQLUpdate(row, "mesa_id=@mesa_id", "mesa_id", new List<System.Data.SqlClient.SqlParameter>() {
                        new System.Data.SqlClient.SqlParameter("mesa_id",Obj.mesa_id)
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

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  Select Mesa_Ayuda.*, Tipo_Consultas_Ayuda.* ";
            sQuery += " ,usuariosolicita.usu_nombre 'usuariosolicita.usu_nombre', usuariosolicita.usu_apellido 'usuariosolicita.usu_apellido' ";
            sQuery += " ,usuarioresponsable.usu_nombre 'usuarioresponsable.usu_nombre', usuarioresponsable.usu_apellido 'usuarioresponsable.usu_apellido' ";
            sQuery += " from Mesa_Ayuda";
            sQuery += "  INNER JOIN SIS_Usuarios as usuariosolicita on usuariosolicita.usu_id = mesa_usu_id_solicita ";
            sQuery += "  LEFT JOIN SIS_Usuarios as usuarioresponsable on usuarioresponsable.usu_id = mesa_usu_id_responsable ";
            sQuery += "  INNER JOIN Tipo_Consultas_Ayuda on tipoconsulta_id = mesa_tipoconsulta_id ";
            
            if (sWHERE.Length > 0)
                sQuery += " WHERE " + sWHERE;

            sQuery += sOrderBy;
            return sQuery;
        }


        #region Consultas específicas 

        public List<Entidades.Mesa_Ayuda> ListarPorUsuarioSolicitante(int usu_id)
        {
            List<Entidades.Mesa_Ayuda> lst = new List<Mesa_Ayuda>();

            sQuery = QueryDefault("", "mesa_usu_id_solicita=@usu_id", "");

            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("usu_id",usu_id)
            });

            foreach (DataRow row in dt_bus.Rows)
            {
                lst.Add(Mapear(row));
            }

            return lst;
        }

        public List<Entidades.Mesa_Ayuda> ListarPorUsuarioResponsable(int usu_id = 0, int estadoID = 0)
        {
            List<Entidades.Mesa_Ayuda> lst = new List<Mesa_Ayuda>();

            string _WHERE = "1 = 1";

            if (usu_id > 0)
                _WHERE += "and mesa_usu_id_responsable=@usu_id";

            // Abiertos
            if (estadoID == 1)
                _WHERE += " and mesa_fec_cerrada is null ";

            // Cerrados
            if (estadoID == 2)
                _WHERE += " and mesa_fec_cerrada is not null ";

            sQuery = QueryDefault("", _WHERE, "");

            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("usu_id",usu_id)
            });

            foreach (DataRow row in dt_bus.Rows)
            {
                lst.Add(Mapear(row));
            }

            return lst;
        }

        #endregion


        #region Funcionalidad Específica

        public ObjectMessage CerrarAyuda(Entidades.Mesa_Ayuda unTicket)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                if (unTicket.mesa_solucion.Length.Equals(0))
                    throw new Exception("Debe especificar una solución valida para cerrar el Ticket de mesa de ayuda");

                db.SQLExecuteNonQuery("UPDATE Mesa_Ayuda SET mesa_fec_cerrada=@fecha, mesa_solucion=@mesa_solucion, mesa_usu_id_responsable=@usu_id WHERE mesa_id=@mesa_id", new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("fecha", DateTime.Now),
                    new System.Data.SqlClient.SqlParameter("mesa_id",unTicket.mesa_id),
                    new System.Data.SqlClient.SqlParameter("mesa_solucion",unTicket.mesa_solucion),
                    new System.Data.SqlClient.SqlParameter("usu_id",Token.UserID)
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

        public ObjectMessage AbrirAyuda(int mesa_id)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                db.SQLExecuteNonQuery("UPDATE Mesa_Ayuda SET mesa_fec_cerrada=NULL WHERE mesa_id=@mesa_id", new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("mesa_id",mesa_id)
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

        public ObjectMessage GrabarInteraccion(Entidades.Mesa_Ayuda_Interaccion unaInteraccion)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                if (unaInteraccion.mesainteraccion_mensaje.Length.Equals(0))
                    throw new Exception("El texto a enviar no puede estar vacío");

                DataRow row = db.Estructura("Mesa_Ayuda_Interaccion");
                row["mesainteraccion_mesa_id"] = unaInteraccion.DatosAyuda.mesa_id;
                row["mesainteraccion_usu_id"] = Convert.ToInt32(Token.UserID);
                row["mesainteraccion_fecha"] = DateTime.Now;
                row["mesainteraccion_mensaje"] = unaInteraccion.mesainteraccion_mensaje;

                unaInteraccion.mesainteraccion_id = db.SQLInsert(row, "mesainteraccion_id").Valor;
             
                oM.Message = "OK";
                oM.Success = true;
            }
            catch (Exception ex)
            {
                oM.Message = ex.Message;
                oM.Success = false;
            }

            return oM;
        }

        public List<Entidades.Mesa_Ayuda_Interaccion> ListarInteracciones(int mesa_id)
        {
            List<Entidades.Mesa_Ayuda_Interaccion> lst = new List<Mesa_Ayuda_Interaccion>();

            sQuery = "  select * from Mesa_Ayuda_Interaccion";
            sQuery += " inner join SIS_Usuarios on usu_id=mesainteraccion_usu_id ";
            sQuery += " inner join Mesa_Ayuda on mesa_id=mesainteraccion_mesa_id ";
            sQuery += " where mesainteraccion_mesa_id = @mesa_id ";
            sQuery += " order by mesainteraccion_fecha ";

            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("mesa_id", mesa_id)
            });

            foreach (DataRow dr in dt_bus.Rows)
            {
                Entidades.Mesa_Ayuda_Interaccion interaccion = new Mesa_Ayuda_Interaccion();
                interaccion.mesainteraccion_id = Resources.Validaciones.valNULLINT(dr["mesainteraccion_id"]);
                interaccion.mesainteraccion_fecha = Resources.Validaciones.valNULLDateTime(dr["mesainteraccion_fecha"]);
                interaccion.mesainteraccion_mensaje = Resources.Validaciones.valNULLString(dr["mesainteraccion_mensaje"]);
                interaccion.UsuarioInteraccion = new SIS_Usuario()
                {
                    usu_nombre = Resources.Validaciones.valNULLString(dr["usu_nombre"]),
                    usu_apellido = Resources.Validaciones.valNULLString(dr["usu_apellido"]),
                    usu_id = Resources.Validaciones.valNULLINT(dr["usu_id"])
                };

                lst.Add(interaccion);
            }

            return lst;
        }

        #endregion
    }
}
