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
    public class TareasColaboradores : NegocioBase<Entidades.Tarea_Colaborador>, App.INegocioAgendable
    {
        public TareasColaboradores(Entidades.App.Token paramToken) : base("tarcol_id", "tarcol_activo", "Tareas_Colaboradores", "tarcol")
        {
            Token = paramToken;
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public override Tarea_Colaborador Mapear(DataRow dr)
        {
            Entidades.Tarea_Colaborador obj = MapearSimple(dr);
            obj.Legajo = Negocio.App.SIS_Usuarios.MapearStatic(dr);
            obj.Tarea = Negocio.Tareas.MapearStatic(dr);

            return obj;
        }

        public override Tarea_Colaborador MapearCompleto(DataRow dr)
        {
            Entidades.Tarea_Colaborador obj = Mapear(dr);
            return obj;
        }

        public override Tarea_Colaborador MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static Tarea_Colaborador MapearStatic(DataRow dr)
        {
            Entidades.Tarea_Colaborador obj = new Tarea_Colaborador();
            obj.tarcol_id = Resources.Validaciones.valNULLINT(dr["tarcol_id"]);
            obj.Tarea = new Tarea() { tar_id = Resources.Validaciones.valNULLINT(dr["tarcol_tar_id"]) };
            obj.Tarea.tar_id = Resources.Validaciones.valNULLINT(dr["tarcol_tar_id"]);
            obj.Tarea.IdEncriptado = Negocio.App.Security.EncriptarID(obj.Tarea.tar_id.ToString());
            obj.Legajo = new Entidades.App.SIS_Usuario() { usu_id = Resources.Validaciones.valNULLINT(dr["tarcol_usu_id"]) };
            obj.Legajo.usu_id = Resources.Validaciones.valNULLINT(dr["tarcol_usu_id"]);
            obj.Legajo.IdEncriptado = Negocio.App.Security.EncriptarID(obj.Legajo.usu_id.ToString());
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(obj.tarcol_id.ToString());
            return obj;
        }

        public override Tarea_Colaborador ObjetoNuevo()
        {
            Entidades.Tarea_Colaborador obj = new Tarea_Colaborador();
            return obj;
        }

        public bool TareaPreviamenteAsignada(Tarea_Colaborador obj)
        {

            string sQuery = QueryDefault("", " tarcol_usu_id = @usu_id and tarcol_tar_id = @tar_id ", "");
            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("usu_id",obj.Legajo.usu_id),
                    new System.Data.SqlClient.SqlParameter("tar_id",obj.Tarea.tar_id)
                });
            return (dt_bus.Rows.Count > 0);
        }


        public override void PermiteGuardar(Tarea_Colaborador obj)
        {
            if (TareaPreviamenteAsignada(obj))
                throw new Exception("La persona fue asignada a la tarea previamente.");
        }

        public override ObjectMessage Save(Tarea_Colaborador Obj)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                PermiteGuardar(Obj);
                DataRow row = db.Estructura("Tareas_Colaboradores");
                row["tarcol_usu_id"] = Obj.Legajo.usu_id;
                row["tarcol_tar_id"] = Obj.Tarea.tar_id;

                if (Obj.tarcol_id.Equals(0))
                {
                    row["tarcol_fec_alta"] = DateTime.Now;
                    row["tarcol_usu_id_alta"] = Token.UserID;
                    Obj.tarcol_id = db.SQLInsert(row, "tarcol_id").Valor;
                    oM.Message = "Datos ingresados";
                }
                else
                {
                    db.SQLUpdate(row, "tarcol_id=@tarcol_id", "tarcol_id", new List<System.Data.SqlClient.SqlParameter>() {
                        new System.Data.SqlClient.SqlParameter("tarcol_id",Obj.tarcol_id)
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
            sQuery = "  SELECT * FROM Tareas_Colaboradores ";
            sQuery += " LEFT JOIN Tareas on tar_id=tarcol_tar_id ";
            sQuery += " LEFT JOIN SIS_Usuarios ON usu_id=tarcol_usu_id ";

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

        #region Funcionalidad Particular

        public List<Entidades.Tarea_Colaborador> ListarPorColaborador(int usu_id)
        {
            List<Entidades.Tarea_Colaborador> lst = new List<Tarea_Colaborador>();

            sQuery = QueryDefault("", "usu_id=@usu_id", "");

            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("usu_id", usu_id)
            });

            foreach (DataRow row in dt_bus.Rows)
            {
                Entidades.Tarea_Colaborador obj = Mapear(row);


                lst.Add(obj);
            }


            return lst;
        }

        public List<Entidades.Tarea> ListarTareasPorColaborador(int usu_id)
        {
            List<Entidades.Tarea> lst = new List<Tarea>();

            sQuery = QueryDefault("", "usu_id=@usu_id", "");

            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("usu_id", usu_id)
            });

            foreach (DataRow row in dt_bus.Rows)
            {
                Entidades.Tarea_Colaborador obj = Mapear(row);


                lst.Add(obj.Tarea);
            }


            return lst;
        }



        public List<Entidades.Tarea_Colaborador> ListarPorTarea(int tar_id)
        {
            List<Entidades.Tarea_Colaborador> lst = new List<Tarea_Colaborador>();

            sQuery = QueryDefault("", "tar_id=@tar_id", "");

            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("tar_id", tar_id)
            });

            foreach (DataRow row in dt_bus.Rows)
            {
                Entidades.Tarea_Colaborador obj = Mapear(row);


                lst.Add(obj);
            }


            return lst;
        }

        #endregion

        #region Implementa Agendable
        public fullCalendar CrearCalendario(string usu_id)
        {
            Negocio.App.Calendario calendario = new App.Calendario();
            List<Entidades.Tarea> lst = ListarTareasPorColaborador(Convert.ToInt32(usu_id));
            fullCalendar calendar = new fullCalendar();
            calendar = calendario.CargarParametros(lst, this, calendar);
            return calendar;
        }

        public eventCalendar MaquetarAgendable(IAgendable item, eventCalendar oEvento)
        {
            oEvento.UrlRedirect = "";
            oEvento.UrlController = "";
            oEvento.UrlAction = "";

            string contentHTMLPreview = "";
            Entidades.Tarea tarea = (Tarea)item;
            contentHTMLPreview += "<b>Detalle: </b>" + tarea.agendable_titulo.Replace("\r\n", "<br/>");
            //if (item.ent_confirmado == false)
            //    contentHTMLPreview += $"<br/> <span class=\"text-danger\"> Pendiente de confirmación </span> ";

            //if (item.Entregado == true)
            //    contentHTMLPreview += $"<br/> <span class=\"text-success\">Entregado </span> ";
            oEvento.ContentHtmlPopup = contentHTMLPreview;
            if (tarea.agendable_fecha.ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy"))
            {
                oEvento.ClassNameBackground = Entidades.Controls.eventCalendar.Azul();
            }
            else
            {
                if (item.agendable_fecha < DateTime.Now)
                    oEvento.ClassNameBackground = Entidades.Controls.eventCalendar.Default();

                //if (item.agendable_fecha > DateTime.Now)
                //{
                //    if (item.ent_confirmado == false)
                //        oEvento.ClassNameBackground = Entidades.Controls.eventCalendar.Rojo2();
                //    else
                //        oEvento.ClassNameBackground = Entidades.Controls.eventCalendar.Verde2();
                //}
            }
            //if (item.Entregado)
            //    oEvento.ClassNameBackground = Entidades.Controls.eventCalendar.Verde();

            return oEvento;
        }


        #endregion

    }
}
