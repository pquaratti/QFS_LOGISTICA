    using Entidades;
using Entidades.App;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class Proyectos : NegocioBase<Entidades.Proyecto>
    {
        public Proyectos(Entidades.App.Token paramToken) : base("proy_id", "proy_activo", "Proyectos", "proy")
        {
            Token = paramToken;
            TokenFilter = true;
        }

        #region Funcionalidad

        public override Proyecto ObjetoNuevo()
        {
            Entidades.Proyecto obj = new Entidades.Proyecto();
            return obj;
        }

        public override void PermiteGuardar(Proyecto obj)
        {
            if (obj.proy_fec_fin<DateTime.Now)
                throw new Exception("La fecha de finalización ingresada no es válida.");
            if (obj.proy_fec_ini > obj.proy_fec_fin)
                throw new Exception("La fecha de finalización y de inicio no son válidas.");
        }

        public override ObjectMessage Save(Proyecto Obj)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                PermiteGuardar(Obj);
                DataRow row = db.Estructura("Proyectos");
                row["proy_id"] = Obj.proy_id;
                row["proy_titulo"] = Obj.proy_titulo;
                row["proy_tproy_id"] = Obj.Tipo.tproy_id;
                row["proy_descripcion"] = Obj.proy_descripcion;
                row["proy_fec_ini"] = Obj.proy_fec_ini;
                row["proy_fec_fin"] = Obj.proy_fec_fin;
                row["proy_org_id"] = Token.OrganizacionID;
                row["proy_tproy_id"] = Obj.Tipo.tproy_id;
                row["proy_activo"] = Obj.proy_activo;
                row["proy_foto"] = Obj.proy_foto;
                row["proy_area_id"] = Obj.Area.area_id;

                if (Obj.proy_id.Equals(0)) 
                {
                    row["proy_activo"] = true;
                    row["proy_fec_alta"] = DateTime.Now;
                    row["proy_usu_id_alta"] = Token.UserID;
                    Obj.proy_id = db.SQLInsert(row, "proy_id").Valor;
                    oM.Message = "Datos ingresados";
                }
                else
                {
                    row["proy_fec_mod"] = DateTime.Now;
                    row["proy_usu_id_mod"] = Token.UserID;
                    db.SQLUpdate(row, "proy_id=@proy_id", "proy_id", new List<System.Data.SqlClient.SqlParameter>() {
                        new System.Data.SqlClient.SqlParameter("proy_id",Obj.proy_id)
                    });

                    oM.Message = "Datos actualizados.";
                }
                oM.ObjectRelation = Obj.proy_id;
                oM.Success = true;
            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.Message = ex.Message;
            }

            return oM;
        }

        public ObjectMessage ActualizarImagen(Entidades.Proyecto proy)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                if (Convert.ToInt32(proy.proy_id) > 0)
                {
                    db.SQLExecuteNonQuery("UPDATE Proyectos SET proy_foto=@pathFile WHERE proy_id=@proy_id", new List<System.Data.SqlClient.SqlParameter>() {
                    new System.Data.SqlClient.SqlParameter("pathFile",proy.proy_foto),
                    new System.Data.SqlClient.SqlParameter("proy_id",proy.proy_id)
                });
                    oM.Message = "Datos actualizados.";
                    oM.Success = true;
                }
            }
            catch (Exception ex)
            {
                oM.Message = ex.Message;
                oM.Success = false;
            }

            return oM;
        }

        public string ObtenerNombreFoto(string idEncriptado)
        {
            Proyecto proy = ObtenerPorIDEncriptado(idEncriptado);
            return proy.proy_foto;
        }

        public ObjectMessage BorrarFoto(string path)
        {
            ObjectMessage oM = new ObjectMessage();
            bool result = File.Exists(path);
            if (result == true)
            {
                try
                {
                    File.Delete(path);
                    oM.Message = "OK";
                    oM.Success = true;
                }
                catch (Exception ex)
                {
                    oM.Message = ex.Message;
                    oM.Success = false;
                }
            }

            return oM;
        }


        public ObjectMessage DesvincularFoto(string proy_id, string path)
        {
            ObjectMessage oM = new ObjectMessage();

            oM = BorrarFoto(path);
            if (oM.Success)
            {
                try
                {

                    if (Convert.ToInt32(proy_id) > 0)
                    {
                        db.SQLExecuteNonQuery("UPDATE Proyectos SET proy_foto=NULL WHERE proy_id=@proy_id", new List<System.Data.SqlClient.SqlParameter>() {
                    new System.Data.SqlClient.SqlParameter("proy_id",proy_id)
                });
                        oM.Message = "OK";
                        oM.Success = true;
                    }
                }
                catch (Exception ex)
                {
                    oM.Message = ex.Message;
                    oM.Success = false;
                }
            }
            

            return oM;
        }

        public ObjectMessage DeleteProyecto(int proy_id)
        {
            Entidades.App.ObjectMessage obj = new Entidades.App.ObjectMessage();

            Proyecto Proyecto = ObtenerPorID(Convert.ToString(proy_id));

            if (Proyecto.Finalizado)
            {
                obj.Success = false;
                obj.Message = "No se puede borrar un proyecto finalizado.";
                return obj;
            }

            if (Proyecto.Cerrado)
            {
                obj.Success = false;
                obj.Message = "No se puede borrar un proyecto cerrado.";
                return obj;
            }

            obj = Delete(proy_id);

            return obj;
        }

      

        public Entidades.App.ObjectMessage CerrarProyecto(string proy_id)
        {
            Entidades.App.ObjectMessage oM = new ObjectMessage();
            
            if (CantidadDeObjetivos(proy_id).Equals(0))
            {
                oM.Success = false;
                oM.Message = "No puede cerrar un Proyecto sin Objetivos";
                return oM;
            }

            if (CantidadDeIndicadores(proy_id).Equals(0))
            {
                oM.Success = false;
                oM.Message = "No puede cerrar un Proyecto sin Indicadores";
                return oM;
            }

            try
            {
                Entidades.Proyecto datosProyecto = ObtenerPorID(proy_id, true);

                db.SQLExecuteNonQuery("UPDATE Proyectos SET proy_fec_cerrado=GETDATE() WHERE proy_id=@proy_id", new List<System.Data.SqlClient.SqlParameter>() {
                    new System.Data.SqlClient.SqlParameter("proy_id",proy_id)
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

        public Entidades.App.ObjectMessage AbrirProyecto(string proy_id)
        {
            Entidades.App.ObjectMessage oM = new ObjectMessage();

            try
            {
                Entidades.Proyecto datosProyecto = ObtenerPorID(proy_id, true);

                db.SQLExecuteNonQuery("UPDATE Proyectos SET proy_fec_cerrado=NULL WHERE proy_id=@proy_id", new List<System.Data.SqlClient.SqlParameter>() {
                    new System.Data.SqlClient.SqlParameter("proy_id",proy_id)
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

        public Entidades.App.ObjectMessage FinalizarProyecto(string proy_id)
        {
            Entidades.App.ObjectMessage oM = new ObjectMessage();

            try
            {
                Entidades.Proyecto datosProyecto = ObtenerPorID(proy_id, true);

                db.SQLExecuteNonQuery("UPDATE Proyectos SET proy_fec_finalizado=GETDATE() WHERE proy_id=@proy_id", new List<System.Data.SqlClient.SqlParameter>() {
                    new System.Data.SqlClient.SqlParameter("proy_id",proy_id)
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

        public List<Proyecto> ListarPorTipoProyecto(int tproy_id)
        {
            sQuery = QueryDefault("", "", "");

            List<ObjectParameter> paramsFilter = new List<ObjectParameter>();

            if (tproy_id > 0)
                paramsFilter.Add(new ObjectParameter() { Name = "proy_tproy_id", Value = tproy_id });

            List<Entidades.Proyecto> Proyectos = new List<Entidades.Proyecto>();

            Proyectos = ListarConFiltros(paramsFilter);

            return Proyectos;
        }

        public List<Proyecto> ListarPorOrganizacion(int org_id)
        {
            sQuery = QueryDefault("", "", "");

            List<ObjectParameter> paramsFilter = new List<ObjectParameter>();

            if (org_id > 0)
                paramsFilter.Add(new ObjectParameter() { Name = "proy_org_id", Value = org_id });

            List<Entidades.Proyecto> Proyectos = new List<Entidades.Proyecto>();

            Proyectos = ListarConFiltros(paramsFilter);

            return Proyectos;
        }

        public List<Proyecto> ListarPorTipoProyectoYEstado(int tproy_id, string estado)
        {
            List<Entidades.Proyecto> lst = new List<Entidades.Proyecto>();
            lst = ListarPorTipoProyecto(Convert.ToInt32(tproy_id));
            if (estado == " Finalizado ")
                lst = lst.Where(m => m.Finalizado).ToList();
            if (estado == " A Tiempo ")
                lst = lst.Where(m => !m.Finalizado).ToList();
            if (estado == " Configurado ")
                lst = lst.Where(m => m.Cerrado).ToList();
            if (estado == " Sin Configurar ")
                lst = lst.Where(m => !m.Cerrado).ToList();
            return lst;
        }

        public List<Proyecto> ListarEnCurso()
        {
            List<Entidades.Proyecto> lst = new List<Entidades.Proyecto>();

            string _Where = GetFilterTokenQuery() + " and proy_fec_finalizado is null ";

            List<System.Data.SqlClient.SqlParameter> lstParams = new List<System.Data.SqlClient.SqlParameter>();
            lstParams.AddRange(GetFilterTokenParams());

            sQuery = QueryDefault("", _Where, "");

            DataTable dt_bus = db.SQLSelect(sQuery, lstParams);

            foreach (DataRow row in dt_bus.Rows)
            {
                lst.Add(Mapear(row));
            }

            SetObjetivos(lst);
          
            return lst;
        }

        private void SetObjetivos(List<Entidades.Proyecto> lst)
        {
            foreach (var itemProyecto in lst)
            {
                List<Entidades.Proyecto_Objetivo> lstObjetivos = new Negocio.ProyectosObjetivos(Token).ListarPorProyecto(itemProyecto.proy_id);

                itemProyecto.Objetivos = lstObjetivos;

                decimal _porcentajeTotal = Convert.ToDecimal(itemProyecto.Objetivos.Count) * Convert.ToDecimal(100);
                decimal _sumatoriaPorcEvolucion = itemProyecto.Objetivos.Sum(s => s.PorcentajeEvolucion);

                if (_sumatoriaPorcEvolucion > 0)
                    itemProyecto.PorcentajeEvolucion = (_sumatoriaPorcEvolucion / _porcentajeTotal) * Convert.ToDecimal(100);
                else
                    itemProyecto.PorcentajeEvolucion = 0;
            }
        }

        #endregion 

        #region Mappers
        public override Proyecto Mapear(DataRow dr)
        {
            Entidades.Proyecto obj = MapearSimple(dr);
            obj.Tipo = Negocio.Tipo_Proyectos.MapearStatic(dr);
            obj.Organizacion = Negocio.App.SIS_Organizaciones.MapearStatic(dr);
            return obj;
        }

        public override Proyecto MapearCompleto(DataRow dr)
        {
            Entidades.Proyecto obj = Mapear(dr);
            return obj;
        }

        public override Proyecto MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static Proyecto MapearStatic(DataRow dr)
        {
            Entidades.Proyecto obj = new Entidades.Proyecto();
            obj.proy_id = Resources.Validaciones.valNULLINT(dr["proy_id"]);
            obj.Tipo = new Entidades.Tipo_Proyecto() { tproy_id = Resources.Validaciones.valNULLINT(dr["proy_tproy_id"]) };
            obj.proy_titulo = Resources.Validaciones.valNULLString(dr["proy_titulo"]);
            obj.proy_descripcion = Resources.Validaciones.valNULLString(dr["proy_descripcion"]);
            obj.proy_fec_ini = Resources.Validaciones.valNULLDateTime(dr["proy_fec_ini"]);
            obj.proy_fec_fin = Resources.Validaciones.valNULLDateTime(dr["proy_fec_fin"]);
            obj.proy_foto = Resources.Validaciones.valNULLString(dr["proy_foto"]);
            if (dr["proy_fec_cerrado"] != DBNull.Value)
                obj.Cerrado = true;
            else
                obj.Cerrado = false;
            obj.proy_fec_cerrado = Resources.Validaciones.valNULLDateTime(dr["proy_fec_cerrado"]);
            obj.Organizacion = new Entidades.App.SIS_Organizacion() { org_id = Resources.Validaciones.valNULLINT(dr["proy_org_id"]) };
            obj.Organizacion.IdEncriptado = Negocio.App.Security.EncriptarID(obj.Organizacion.org_id.ToString());

            obj.proy_duracion = obj.proy_fec_fin - obj.proy_fec_ini;

            if (obj.proy_duracion.Ticks > 0)
            {
                obj.Finalizado = false;
            }
            else
                obj.Finalizado = true;

            obj.IdEncriptado = Negocio.App.Security.EncriptarID(obj.proy_id.ToString());
            obj.Area = new SIS_Area() { area_id = Resources.Validaciones.valNULLINT(dr["proy_area_id"]) };
            obj.Area.IdEncriptado = Negocio.App.Security.EncriptarID(obj.Area.area_id.ToString());

            return obj;
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM Proyectos ";
            sQuery += " LEFT JOIN Tipo_Proyecto on tproy_id = proy_tproy_id ";
            sQuery += " LEFT JOIN SIS_Organizaciones on org_id = proy_org_id ";

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

        #region Resumen

        public int CantidadDeIndicadores(string proy_id)
        {
            string sQuery = "SELECT COUNT(*) AS cantidad FROM Proyectos_Indicadores WHERE pryind_proy_id=@proy_id ";
            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("proy_id",proy_id)
            });
            return Resources.Validaciones.valNULLINT(dt_bus.Rows[0]["cantidad"]);
        }

        public int CantidadDeObjetivos(string proy_id)
        {
            string sQuery = "SELECT COUNT(*) AS cantidad FROM Proyectos_Objetivos WHERE pryobj_proy_id=@proy_id ";
            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("proy_id",proy_id)
            });
            return Resources.Validaciones.valNULLINT(dt_bus.Rows[0]["cantidad"]);
        }

        public int CantidadDeTareas(string proy_id)
        {
            string sQuery = "SELECT COUNT(*) AS cantidad FROM Proyectos_Tareas WHERE prytar_proy_id=@proy_id ";
            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("proy_id",proy_id)
            });
            return Resources.Validaciones.valNULLINT(dt_bus.Rows[0]["cantidad"]);
        }
        public int CantidadDeColaboradores(string proy_id)
        {
            string sQuery = "SELECT COUNT(*) AS cantidad FROM Proyectos_Colaboradores WHERE prycolab_proy_id=@proy_id ";
            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("proy_id",proy_id)
            });
            return Resources.Validaciones.valNULLINT(dt_bus.Rows[0]["cantidad"]);
        }

        public List<Entidades.Vistas.TarjetaCabecera> ResumenDetalleCabecera(string proy_id)
        {
            Negocio.Proyectos negocioPROY = new Proyectos(Token);
            List<Entidades.Vistas.TarjetaCabecera> lst = new List<Entidades.Vistas.TarjetaCabecera>();
            lst.Add(
                new Entidades.Vistas.TarjetaCabecera
                {
                    titulo = "Objetivos",
                    descripcion = "Cantidad de Objetivos",
                    valor = negocioPROY.CantidadDeObjetivos(proy_id)
                }
            );

            lst.Add(
                new Entidades.Vistas.TarjetaCabecera
                {
                    titulo = "Indicadores",
                    descripcion = "Cantidad de Indicadores",
                    valor = negocioPROY.CantidadDeIndicadores(proy_id)
                }
            );

            lst.Add(
                new Entidades.Vistas.TarjetaCabecera
                {
                    titulo = "Colaboradores",
                    descripcion = "Colaboradores trabajando en el proyecto",
                    valor = negocioPROY.CantidadDeColaboradores(proy_id)
                }
            );

            return lst;
        }

        public List<Entidades.Proyecto_Objetivo> ResumenDetalleObjetivos(string proy_id)
        {
            Negocio.ProyectosObjetivos negocioPRYOBJ = new Negocio.ProyectosObjetivos(Token);
            Negocio.ProyectosObjetivosIndicadores negocioPOI = new ProyectosObjetivosIndicadores(Token);

            List<Entidades.Proyecto_Objetivo> objetivos = negocioPRYOBJ.ListarPorProyecto(Convert.ToInt32(proy_id));

            foreach (Entidades.Proyecto_Objetivo objetivo in objetivos)
            {
                List<Entidades.Proyecto_Indicador> indicadores = negocioPOI.ListarIndicadoresPorObjetivo(objetivo.pryobj_id);

                objetivo.ValorIncial = negocioPOI.CalcularValorInicial(objetivo.pryobj_id);
                objetivo.ValorMeta = negocioPOI.CalcularValorMeta(objetivo.pryobj_id);
                objetivo.CantidadIndicadores = indicadores.Count;
            }
            return objetivos;
        }

        #endregion

        #region Seguimiento 

        public Entidades.Vistas.SeguimientoProyecto ConsultarSeguimiento(string proyecto)
        {
            Entidades.Vistas.SeguimientoProyecto datos = new Entidades.Vistas.SeguimientoProyecto();

            datos.DatosProyecto = ObtenerPorID(proyecto);
            datos.Objetivos = new Negocio.ProyectosObjetivos(Token).ListarPorProyecto(datos.DatosProyecto.proy_id);
            datos.Indicadores = new Negocio.ProyectosIndicadores(Token).ListarPorProyecto(datos.DatosProyecto.proy_id);

            datos.DatosProyecto.PorcentajeEvolucion = CalcularEvolucionProyecto(datos.Objetivos.Count, datos.Objetivos.Sum(s => s.PorcentajeEvolucion));

            return datos;
        }

        public decimal CalcularEvolucionProyecto(int cantidadObjetivos, decimal sumatoriaPorcEvolucion)
        {
            decimal _porcentajeEvolucion = 0;
            decimal _porcTotalObjetivos = Convert.ToDecimal(cantidadObjetivos * 100);
            
            if (sumatoriaPorcEvolucion > 0)
                _porcentajeEvolucion = (sumatoriaPorcEvolucion / _porcTotalObjetivos) * Convert.ToDecimal(100);

            return _porcentajeEvolucion;
        }
        #endregion
    }



}
