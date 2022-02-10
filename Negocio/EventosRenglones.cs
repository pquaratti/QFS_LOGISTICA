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
    public class EventosRenglones : NegocioBase<Entidades.Evento_Renglon>
    {
        Negocio.Eventos negocioEVE;
        Negocio.App.SIS_Usuarios negocioUSU;

        public EventosRenglones(Entidades.App.Token paramToken) : base("ever_id", "sin", "Eventos_Renglones", "ever")
        {
            Token = paramToken;
            negocioUSU = new App.SIS_Usuarios(Token);
            negocioEVE = new Eventos(Token);
        }

        #region Maps
        public override Evento_Renglon Mapear(DataRow dr)
        {
            Entidades.Evento_Renglon obj = MapearSimple(dr);
            obj.Evento = negocioEVE.MapearSimple(dr);

            // Mapeaa Usuario con sus datos.
            obj.Usuario.usu_id = Resources.Validaciones.valNULLINT(dr["usu_id"]);
            obj.Usuario.usu_nickname = Resources.Validaciones.valNULLString(dr["usu_nickname"]);
            obj.Usuario.usu_nombre = Resources.Validaciones.valNULLString(dr["usu_nombre"]);
            obj.Usuario.usu_apellido = Resources.Validaciones.valNULLString(dr["usu_apellido"]);
            obj.Usuario.usu_documento = Resources.Validaciones.valNULLString(dr["usu_documento"]);
            obj.Usuario.usu_mail = Resources.Validaciones.valNULLString(dr["usu_mail"]);
         
            return obj;
        }

        public override Evento_Renglon MapearCompleto(DataRow dr)
        {
            Entidades.Evento_Renglon obj = Mapear(dr);
            return obj;
        }

        public override Evento_Renglon MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static Evento_Renglon MapearStatic(DataRow dr)
        {
            Entidades.Evento_Renglon obj = new Entidades.Evento_Renglon();
            obj.ever_id = Resources.Validaciones.valNULLINT(dr["ever_id"]);
            obj.Evento = new Entidades.Evento() { eve_id = Resources.Validaciones.valNULLINT(dr["ever_eve_id"]) };
            obj.Usuario = new Entidades.App.SIS_Usuario() { usu_id = Resources.Validaciones.valNULLINT(dr["ever_usu_id"]) };
            obj.ever_asist = Resources.Validaciones.valNULLBool(dr["ever_asist"]);

            return obj;
        }

        #endregion

        #region Save
        public override Evento_Renglon ObjetoNuevo()
        {
            Entidades.Evento_Renglon obj = new Entidades.Evento_Renglon();
            return obj;
        }

        public override void PermiteGuardar(Evento_Renglon Obj)
        {
            if (!(Obj.Usuario.usu_id > 0))
                throw new Exception("El usuario no es válido.");

        }

        public override void DatosObligatorios(Evento_Renglon Obj)
        {

        }

        public override ObjectMessage Save(Evento_Renglon Obj)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                PermiteGuardar(Obj);
                DataRow row = db.Estructura("Eventos_Renglones");
                row["ever_id"] = Obj.ever_id;
                row["ever_eve_id"] = Obj.Evento.eve_id;
                row["ever_usu_id"] = Obj.Usuario.usu_id;
                row["ever_asist"] = Obj.ever_asist;


                if (Obj.ever_id.Equals(0))
                {
                    Obj.ever_id = db.SQLInsert(row, "ever_id").Valor;
                    oM.Message = "Datos ingresados";
                }
                else
                {
                    db.SQLUpdate(row, "ever_id=@ever_id", "ever_id", new List<System.Data.SqlClient.SqlParameter>() {
                        new System.Data.SqlClient.SqlParameter("ever_id",Obj.ever_id)
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
        #endregion

        #region Listar

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public List<Entidades.Evento_Renglon> ListarRenglonesPorEvento(int eve_id)
        {

            sQuery = QueryDefault("", "", "");

            List<ObjectParameter> paramsFilter = new List<ObjectParameter>();

            if (eve_id > 0)
                paramsFilter.Add(new ObjectParameter() { Name = "ever_eve_id", Value = eve_id });

            List<Entidades.Evento_Renglon> renglones = new List<Entidades.Evento_Renglon>();

            renglones = ListarConFiltros(paramsFilter);

            return renglones;

        }

        #endregion

        #region Querys

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            string columns = " Eventos_Renglones.*, Eventos.*, " +
                "SIS_Usuarios.usu_id, SIS_Usuarios.usu_nickname, " +
                "SIS_Usuarios.usu_nombre, SIS_Usuarios.usu_apellido, " +
                "SIS_Usuarios.usu_documento, SIS_Usuarios.usu_mail, " +
                "SIS_Usuarios.usu_dis_id, SIS_Usuarios.usu_sud_id, " +
                "SIS_Usuarios.usu_fue_id, Distritos.dis_nombre, " +
                "Distritos.dis_abreviatura, Subdistritos.sud_abreviatura, " +
                "Subdistritos.sud_nombre, Fuerzas.fue_nombre  ";

            sQuery = "  SELECT " + columns + " FROM Eventos_Renglones ";
            sQuery += " LEFT JOIN Eventos ON eve_id=ever_eve_id ";
            sQuery += " LEFT JOIN SIS_Usuarios ON usu_id=ever_usu_id ";
            sQuery += " LEFT JOIN Distritos ON dis_id=usu_dis_id ";
            sQuery += " LEFT JOIN Subdistritos ON sud_id=usu_sud_id ";
            sQuery += " LEFT JOIN Fuerzas ON fue_id=usu_fue_id ";


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

        #region Funcionalidad

        public Entidades.App.ObjectMessage EventoAsistencia(List<Entidades.Evento_Renglon> lst)
        {
            Entidades.App.ObjectMessage oM = new ObjectMessage();

            try
            {
                foreach (Evento_Renglon renglon in lst)
                {
                    db.SQLExecuteNonQuery("UPDATE Eventos_Renglones SET ever_asist=@estado WHERE ever_id=@ever_id", new List<System.Data.SqlClient.SqlParameter>() {
                        new System.Data.SqlClient.SqlParameter("ever_id",renglon.ever_id),
                        new System.Data.SqlClient.SqlParameter("estado",renglon.ever_asist)
                    });
                }

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
    }
}
