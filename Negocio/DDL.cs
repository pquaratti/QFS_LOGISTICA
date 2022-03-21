using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Entidades;
using System.IO;
using Entidades.App;
using System.Reflection.Emit;

namespace Negocio
{
    public class DDL
    {
        public enum OrdenColeccion
        {
            Texto = 1,
            Id = 2,
            Sin = 0
        }

        public static Helpers.SQLDb dbStatic = new Helpers.SQLDb();

        public static List<Entidades.App.DLLObject> ListarBoolean()
        {
            List<Entidades.App.DLLObject> lst = new List<Entidades.App.DLLObject>();

            lst.Add(new Entidades.App.DLLObject()
            {
                Value = "0",
                Text = "NO"
            });

            lst.Add(new Entidades.App.DLLObject()
            {
                Value = "1",
                Text = "SI"
            });

            return lst;
        }
        /// <summary>
        /// 0 a 100 múltiplos de 5. - 
        /// EJ: 85%
        /// </summary>
        /// <returns></returns>
        public static List<Entidades.App.DLLObject> ListarPorcentajes()
        {
            List<Entidades.App.DLLObject> lst = new List<Entidades.App.DLLObject>();
            for (int i = 0; i <= 20; i++)
            {
                lst.Add(new Entidades.App.DLLObject()
                {
                    Value = (i*5).ToString(),
                    Text = (i*5).ToString() + "%"
                });
            }

            return lst;
        }

        public static List<Entidades.App.DLLObject> ListarProvincias(Entidades.App.Token token, bool agregarDefault = false)
        {
            List<Entidades.App.DLLObject> lst = new List<Entidades.App.DLLObject>();
            Negocio.Provincias negocio = new Provincias(token);
            List<Entidades.SIS_Provincia> lstDatos = negocio.Listar();

            lstDatos.Add(new Entidades.SIS_Provincia()
            {
                prv_id = 0,
                prv_nombre = "-"
            });

            foreach (Entidades.SIS_Provincia item in lstDatos)
            {
                lst.Add(new DLLObject()
                {
                    Value = item.prv_id.ToString(),
                    Text = item.prv_nombre
                });
            }

            lst = lst.OrderBy(o => o.Value).ToList();
            return lst;
        }

        public static List<Entidades.App.DLLObject> ListarDepartamentos(Entidades.App.Token token, bool agregarDefault = false)
        {
            List<Entidades.App.DLLObject> lst = new List<Entidades.App.DLLObject>();
            Negocio.Provincias negocio = new Provincias(token);
            List<Entidades.SIS_Provincia> lstDatos = negocio.Listar();

            lstDatos.Add(new Entidades.SIS_Provincia()
            {
                prv_id = 0,
                prv_nombre = "-"
            });

            foreach (Entidades.SIS_Provincia item in lstDatos)
            {
                lst.Add(new DLLObject()
                {
                    Value = item.prv_id.ToString(),
                    Text = item.prv_nombre
                });
            }

            lst = lst.OrderBy(o => o.Value).ToList();
            return lst;
        }

        public static List<Entidades.App.DLLObject> ListarTiposDocumento(Entidades.App.Token token, bool agregarDefault = false)
        {
            List<Entidades.App.DLLObject> lst = new List<Entidades.App.DLLObject>();
            Negocio.Tipo_Documento negocio = new Tipo_Documento(token);
            List<Entidades.Tipo_Documento> lstDatos = negocio.Listar();

            lstDatos.Add(new Entidades.Tipo_Documento()
            {
                tid_id = 0,
                tid_nombre = "-"
            });

            foreach (Entidades.Tipo_Documento item in lstDatos)
            {
                lst.Add(new DLLObject()
                {
                    Value = item.tid_id.ToString(),
                    Text = item.tid_nombre
                });
            }

            lst = lst.OrderBy(o => o.Value).ToList();
            return lst;
        }

        public static List<Entidades.App.DLLObject> ListarTiposSexos(Entidades.App.Token token, bool agregarDefault = false)
        {
            List<Entidades.App.DLLObject> lst = new List<Entidades.App.DLLObject>();
            Negocio.Sexos negocio = new Sexos(token);
            List<Entidades.Sexo> lstDatos = negocio.Listar();

            lstDatos.Add(new Entidades.Sexo()
            {
                sex_id = 0,
                sex_nombre = "-"
            });

            foreach (Entidades.Sexo item in lstDatos)
            {
                lst.Add(new DLLObject()
                {
                    Value = item.sex_id.ToString(),
                    Text = item.sex_nombre
                });
            }

            lst = lst.OrderBy(o => o.Value).ToList();
            return lst;
        }

        public static int CalcularDigitoCuit(string cuit)
        {
            int[] mult = new[] { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };
            char[] nums = cuit.ToCharArray();
            int total = 0;
            for (int i = 0; i < mult.Length; i++)
            {
                total += int.Parse(nums[i].ToString()) * mult[i];
            }
            var resto = total % 11;
            return resto == 0 ? 0 : resto == 1 ? 9 : 11 - resto;
        }

        public static bool ValidaCuit(string cuit)
        {
            if (cuit == null)
            {
                return false;
            }
            //Quito los guiones, el cuit resultante debe tener 11 caracteres.
            cuit = cuit.Replace("-", string.Empty);
            if (cuit.Length != 11)
            {
                return false;
            }
            else
            {
                int calculado = CalcularDigitoCuit(cuit);
                int digito = int.Parse(cuit.Substring(10));
                return calculado == digito;
            }
        }

        public static int CalcularDigitoCuitNew(string cuit)
        {
            int[] mult = new[] { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };
            char[] nums = cuit.ToCharArray();
            int total = 0;
            for (int i = 0; i < mult.Length; i++)
            {
                total += int.Parse(nums[i].ToString()) * mult[i];
            }
            var resto = total % 11;
            return resto == 0 ? 0 : resto == 1 ? 9 : 11 - resto;
        }

        public static bool ValidaCuitNew(string cuit)
        {
            if (cuit == null)
            {
                return false;
            }
            //Quito los guiones, el cuit resultante debe tener 11 caracteres.
            cuit = cuit.Replace("-", string.Empty);
            if (cuit.Length != 11)
            {
                return false;
            }
            else
            {
                int calculado = CalcularDigitoCuitNew(cuit);
                int digito = int.Parse(cuit.Substring(10));
                return calculado == digito;
            }
        }

        public string ObtieneParametroGeneral(string dato)
        {
            string sQuery = "select pag_valor from Parametros_Generales where pag_dato = @dato and pag_activo = 1 and GETDATE() between pag_desde and pag_hasta";
            DataTable dt_par = dbStatic.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("dato", dato)
                });
            if (dt_par.Rows.Count > 0)
            {
                DataRow row = dt_par.Rows[0];
                string cp = row["pag_valor"].ToString();
                return cp;
            }
            else
            {
                return "";
            }
        }

        public class Select2
        {

            public static List<Entidades.App.DLLObject> OptionSeleccionada(string pValue, string pText)
            {
                List<Entidades.App.DLLObject> lst = new List<DLLObject>();

                lst.Add(new DLLObject()
                {
                    Value = pValue,
                    Text = pText
                });

                return lst;
            }

        }

        public Boolean ComparaFechas(DateTime fec_ini, DateTime fec_fin)
        {
            if (fec_ini.Date > fec_fin.Date)
            {
                return false;
            }
            else
            {
                return true;
            }


        }

        public static List<Entidades.App.DLLObject> TiposMovimientosImportes()
        {
            List<Entidades.App.DLLObject> lst = new List<Entidades.App.DLLObject>();

            lst.Add(new Entidades.App.DLLObject()
            {
                Text = "Seleccione",
                Value = ""
            });

            lst.Add(new Entidades.App.DLLObject()
            {
                Text = "INGRESO",
                Value = "+"
            });

            lst.Add(new Entidades.App.DLLObject()
            {
                Text = "EGRESO",
                Value = "-"
            });

            return lst;
        }

        public static List<Entidades.App.DLLObject> TiposPersonal()
        {
            List<Entidades.App.DLLObject> lst = new List<Entidades.App.DLLObject>();

            lst.Add(new Entidades.App.DLLObject()
            {
                Text = "Seleccione",
                Value = "0"
            });

            lst.Add(new Entidades.App.DLLObject()
            {
                Text = "Militar",
                Value = "1"
            });

            lst.Add(new Entidades.App.DLLObject()
            {
                Text = "Civil",
                Value = "2"
            });

            return lst;
        }



        public Decimal ObtieneSaldo(int tarjeta)
        {
            Decimal saldo = 0;
            string sQuery = "select sum(Saldo) as Saldo from (";
            sQuery += " Select Case when mvf_mov_id = 1 then sum(mvf_importe) else sum(mvf_importe * -1) end as Saldo from Movimientos_Visa_Flota";
            sQuery += " where mvf_tar_id = @tarjeta and mvf_fec_baja is null and mvf_activo = 1";
            sQuery += " group by mvf_tar_id, mvf_mov_id) as aux";

            DataTable dt_tarjeta = dbStatic.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("tarjeta", tarjeta)
                });
            if (dt_tarjeta.Rows.Count > 0)
            {
                DataRow row = dt_tarjeta.Rows[0];
                saldo = Decimal.Parse(row["Saldo"].ToString());
                return saldo;
            }
            else
            {
                return saldo;
            }
        }

        public bool NoPermiteConsumo(int tarjeta, decimal consumo)
        {
            Decimal saldo = ObtieneSaldo(tarjeta);
            if (saldo > consumo)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public bool ExisteSaldo(string Query, List<Entidades.App.ObjectParameterSigno> listaFiltros)
        {
            string sWhere = "";
            string sQuery = "";
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

            sQuery = Query + " " + sWhere;

            DataTable dt = dbStatic.SQLSelect(sQuery, lstParamsSQL);

            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static List<Entidades.App.DLLObject> ListarCategoriaDePreguntas(Entidades.App.Token token, bool mostrarActivos = true, bool agregarDefault = false)
        {
            List<Entidades.App.DLLObject> lst = new List<Entidades.App.DLLObject>();
            Negocio.PreguntasFrecuentesCategorias negocio = new PreguntasFrecuentesCategorias(token);
            List<Entidades.Pregunta_Frecuente_Categoria> lstDatos = negocio.Listar();
            if (mostrarActivos)
                lstDatos = lstDatos.Where(w => w.pgfc_activo == true).ToList();
            foreach (Entidades.Pregunta_Frecuente_Categoria item in lstDatos)
            {
                lst.Add(new DLLObject()
                {
                    Value = item.pgfc_id.ToString(),
                    Text = item.pgfc_nombre
                });
            }
            if (agregarDefault == true)
            {
                lst.Add(new DLLObject()
                {
                    Value = "0",
                    Text = " Todas las Categorías "
                });
            }
            lst = lst.OrderBy(o => o.Value).ToList();
            return lst;
        }

        public static List<Entidades.App.DLLObject> ListarAccionesPadre()
        {
            List<Entidades.App.DLLObject> lst = new List<Entidades.App.DLLObject>();

            DataTable dt = dbStatic.SQLSelect("select acc_nombre,acc_id from SIS_Acciones where acc_menu=1 and acc_id_padre=0 and acc_activo = 1 order by acc_nombre");

            lst.Add(new Entidades.App.DLLObject()
            {
                Text = "-",
                Value = "0"
            });

            foreach (DataRow row in dt.Rows)
            {
                lst.Add(new Entidades.App.DLLObject()
                {
                    Text = row["acc_nombre"].ToString(),
                    Value = row["acc_id"].ToString()
                });
            }

            return lst;
        }


        public static List<Entidades.App.DLLObject> ListarAccionesPadre(Entidades.App.Token token, bool mostrarActivos = true, bool agregarDefault = true)
        {
            List<Entidades.App.DLLObject> lst = new List<DLLObject>();
            lst = ListarAccionesPadre();

            if (agregarDefault == true)
            {
                lst.Add(new DLLObject()
                {
                    Value = "0",
                    Text = " No se vincula acción "
                });
            }

            return lst;
        }

        public static List<Entidades.App.DLLObject> ListarTipoEvento(Entidades.App.Token token, bool mostrarActivos = false, bool agregarDefault = true)
        {
            List<Entidades.App.DLLObject> lst = new List<Entidades.App.DLLObject>();
            Negocio.Tipo_Eventos negocio = new Tipo_Eventos(token);
            List<Entidades.Tipo_Evento> lstDatos = negocio.Listar();
            if (mostrarActivos)
                lstDatos = lstDatos.Where(w => w.evet_activo == true).ToList();
            foreach (Entidades.Tipo_Evento item in lstDatos)
            {
                lst.Add(new DLLObject()
                {
                    Value = item.evet_id.ToString(),
                    Text = item.evet_contenido
                });
            }
            if (agregarDefault == true)
            {
                lst.Add(new DLLObject()
                {
                    Value = "0",
                    Text = " No vincula "
                });
            }
            lst = lst.OrderBy(o => o.Value).ToList();
            return lst;
        }



        public static List<Entidades.App.DLLObject> ListarIconos()
        {
            List<Entidades.App.DLLObject> lst = new List<DLLObject>();

            lst.Add(new DLLObject() { Value = " ", Text = "Sin ícono" });
            lst.Add(new DLLObject() { Value = "fa fa-home", Text = "fa fa-home" });
            lst.Add(new DLLObject() { Value = "fa fa-list", Text = "fa fa-list" });
            lst.Add(new DLLObject() { Value = "fa fa-edit", Text = "fa fa-edit" });
            lst.Add(new DLLObject() { Value = "fa fa-camera-polaroid", Text = "fa fa-camera-polaroid" });
            lst.Add(new DLLObject() { Value = "fa fa-fax", Text = "fa fa-fax" });



            return lst;
        }

        public static List<Entidades.App.DLLObject> ListarEventoPorEstado(Entidades.App.Token token)
        {
            List<Entidades.App.DLLObject> lst = new List<Entidades.App.DLLObject>();

            List<string> lstDatos = new List<string>() { " Finalizado ", " A Tiempo " };
            foreach (string item in lstDatos)
            {
                lst.Add(new DLLObject()
                {
                    Value = item,
                    Text = item
                }); ;
            }
            lst.Add(new DLLObject()
            {
                Value = "0",
                Text = " Todos "
            });
            lst = lst.OrderBy(o => o.Value).ToList();
            return lst;
        }

        public static List<Entidades.App.DLLObject> ListarProyectoPorEstado(Entidades.App.Token token)
        {
            List<Entidades.App.DLLObject> lst = new List<Entidades.App.DLLObject>();

            List<string> lstDatos = new List<string>() { " Finalizado ", " En Curso " , " Configurado " , " Sin Configurar " };
            foreach (string item in lstDatos)
            {
                lst.Add(new DLLObject()
                {
                    Value = item,
                    Text = item
                }); ;
            }
            lst.Add(new DLLObject()
            {
                Value = "0",
                Text = " Todos "
            });
            lst = lst.OrderBy(o => o.Value).ToList();
            return lst;
        }

        public static List<Entidades.App.DLLObject> ListarTipoProyecto(Entidades.App.Token token, bool agregarDefault = true)
        {
            List<Entidades.App.DLLObject> lst = new List<Entidades.App.DLLObject>();
            Negocio.Tipo_Proyectos negocio = new Tipo_Proyectos(token);
            List<Entidades.Tipo_Proyecto> lstDatos = negocio.Listar();

            foreach (Entidades.Tipo_Proyecto item in lstDatos)
            {
                lst.Add(new DLLObject()
                {
                    Value = item.tproy_id.ToString(),
                    Text = item.tproy_nombre
                });
            }
            if (agregarDefault == true)
            {
                lst.Add(new DLLObject()
                {
                    Value = "0",
                    Text = " No vincula "
                });
            }
            lst = lst.OrderBy(o => o.Value).ToList();
            return lst;
        }

        public static List<Entidades.App.DLLObject> ListarPorcentajesDefault()
        {
            List<Entidades.App.DLLObject> lst = new List<Entidades.App.DLLObject>();

            lst.Add(new Entidades.App.DLLObject()
            {
                Text = "100%",
                Value = "100"
            });

            lst.Add(new Entidades.App.DLLObject()
            {
                Text = "50%",
                Value = "50"
            });

            lst.Add(new Entidades.App.DLLObject()
            {
                Text = "0%",
                Value = "0"
            });

            return lst;
        }

        public static List<Entidades.App.DLLObject> ListarTiposConsultaMesaAyuda(Entidades.App.Token token, bool agregarDefault = false, string textoDefault = "-")
        {
            List<Entidades.App.DLLObject> lst = new List<Entidades.App.DLLObject>();
            Negocio.Tipo_Consulta_Ayuda negocio = new Tipo_Consulta_Ayuda(token);
            List<Entidades.Tipo_Consulta_Ayuda> lstDatos = negocio.Listar();

            if (agregarDefault)
            {
                lstDatos.Add(new Entidades.Tipo_Consulta_Ayuda()
                {
                    tipoconsulta_id = 0,
                    tipoconsulta_nombre = textoDefault
                });
            }

            foreach (Entidades.Tipo_Consulta_Ayuda item in lstDatos)
            {
                lst.Add(new DLLObject()
                {
                    Value = item.tipoconsulta_id.ToString(),
                    Text = item.tipoconsulta_nombre
                });
            }

            lst = lst.OrderBy(o => o.Value).ToList();
            return lst;
        }

        public static List<Entidades.App.DLLObject> ListarEstadosRegistrosDefault()
        {
            List<Entidades.App.DLLObject> lst = new List<Entidades.App.DLLObject>();

            lst.Add(new Entidades.App.DLLObject()
            {
                Text = "Todos",
                Value = "0"
            });

            lst.Add(new Entidades.App.DLLObject()
            {
                Text = "Abiertos",
                Value = "1"
            });

            lst.Add(new Entidades.App.DLLObject()
            {
                Text = "Cerrados",
                Value = "2"
            });

            return lst;
        }

        public static List<Entidades.App.DLLObject> ListarDepartamentosPorProvincias(Entidades.App.Token token, bool agregaDefault = false, int prv_id = 0, string textoDefault = "Seleccione")
        {
            Negocio.Departamentos negocio = new Departamentos(token);
            List<Entidades.App.DLLObject> lst = new List<Entidades.App.DLLObject>();
            List<Entidades.Departamento> objetos = new List<Entidades.Departamento>();

            if (agregaDefault)
            {
                lst.Add(new Entidades.App.DLLObject()
                {
                    Value = "0",
                    Text = textoDefault
                });
            }

            if (prv_id > 0)
            {
                objetos.AddRange(negocio.ListarConFiltros(new List<ObjectParameter>() { new ObjectParameter() { Name = "prv_id", Value = prv_id } }));

                foreach (Entidades.Departamento item in objetos.OrderBy(o => o.Provincia.prv_nombre))
                {
                    lst.Add(new Entidades.App.DLLObject()
                    {
                        Value = item.dto_id.ToString(),
                        Text = item.dto_nombre
                    });
                }
            }
            return lst;
        }

        public static List<Entidades.App.DLLObject> ListarProyectosPorOrganizacion(Entidades.App.Token token, bool agregarDefault = false)
        {
            List<Entidades.App.DLLObject> lst = new List<Entidades.App.DLLObject>();
            Negocio.Proyectos negocio = new Proyectos(token);
            List<Entidades.Proyecto> lstDatos = negocio.ListarPorOrganizacion(Convert.ToInt32(token.OrganizacionID));

            lstDatos.Add(new Entidades.Proyecto()
            {
                proy_id = 0,
                proy_titulo = "-"
            });

            foreach (Entidades.Proyecto item in lstDatos)
            {
                lst.Add(new DLLObject()
                {
                    Value = item.proy_id.ToString(),
                    Text = item.proy_titulo
                });
            }

            lst = lst.OrderBy(o => o.Value).ToList();
            return lst;
        }

        public static List<Entidades.App.DLLObject> ListarObjetivosPorProyecto(Entidades.App.Token token, bool agregaDefault = true, int proy_id = 0, string textoDefault = "Seleccione")
        {
            Negocio.ProyectosObjetivos negocio = new ProyectosObjetivos(token);
            List<Entidades.App.DLLObject> lst = new List<Entidades.App.DLLObject>();
            List<Entidades.Proyecto_Objetivo> objetos = new List<Entidades.Proyecto_Objetivo>();

            if (agregaDefault)
            {
                lst.Add(new Entidades.App.DLLObject()
                {
                    Value = "0",
                    Text = textoDefault
                });
            }

            if (proy_id > 0)
            {
                objetos.AddRange(negocio.ListarConFiltros(new List<ObjectParameter>() { new ObjectParameter() { Name = "proy_id", Value = proy_id } }));

                foreach (Entidades.Proyecto_Objetivo item in objetos.OrderBy(o => o.pryobj_nombre))
                {
                    lst.Add(new Entidades.App.DLLObject()
                    {
                        Value = item.pryobj_id.ToString(),
                        Text = item.pryobj_nombre
                    });
                }
            }
            return lst;
        }

        public static List<Entidades.App.DLLObject> ListarTareasPorProyectoObjetivos(Entidades.App.Token token, bool agregaDefault = true, int proy_id = 0, int pryobj_id = 0, string textoDefault = "Seleccione")
        {
            Negocio.Tareas negocio = new Tareas(token);
            List<Entidades.App.DLLObject> lst = new List<Entidades.App.DLLObject>();
            List<Entidades.Tarea> objetos = new List<Entidades.Tarea>();
            List<ObjectParameter> filtros = new List<ObjectParameter>();

            if (agregaDefault)
            {
                lst.Add(new Entidades.App.DLLObject()
                {
                    Value = "0",
                    Text = textoDefault
                });
            }

            if (pryobj_id > 0)
                filtros.Add(new ObjectParameter() { Name = "pryobj_id", Value = pryobj_id });
            if (proy_id > 0)
                filtros.Add(new ObjectParameter() { Name = "proy_id", Value = proy_id });

            objetos.AddRange(negocio.ListarConFiltros(filtros));

            foreach (Entidades.Tarea item in objetos.OrderBy(o => o.tar_nombre))
            {
                lst.Add(new Entidades.App.DLLObject()
                {
                    Value = item.tar_id.ToString(),
                    Text = item.tar_nombre
                });
            }

            return lst;
        }


        public static List<Entidades.App.DLLObject> ListarLocalidadesPorDepartamentos(Entidades.App.Token token, bool agregaDefault = false, int dto_id = 0, string textoDefault = "Seleccione")
        {
            Negocio.Localidades negocio = new Localidades(token);
            List<Entidades.App.DLLObject> lst = new List<Entidades.App.DLLObject>();
            List<Entidades.SIS_Localidad> objetos = new List<Entidades.SIS_Localidad>();

            if (agregaDefault)
            {
                lst.Add(new Entidades.App.DLLObject()
                {
                    Value = "0",
                    Text = textoDefault
                });
            }

            if (dto_id > 0)
            {
                objetos.AddRange(negocio.ListarConFiltros(new List<ObjectParameter>() { new ObjectParameter() { Name = "dto_id", Value = dto_id } }));

                foreach (Entidades.SIS_Localidad item in objetos.OrderBy(o => o.loc_nombre))
                {
                    lst.Add(new Entidades.App.DLLObject()
                    {
                        Value = item.loc_id.ToString(),
                        Text = item.loc_nombre
                    });
                }
            }
            return lst;
        }

        public static List<Entidades.App.DLLObject> ListarTareasPorProyecto(Entidades.App.Token token, bool agregaDefault = false, int proy_id = 0, string textoDefault = "Seleccione")
        {
            Negocio.Tareas negocio = new Tareas(token);
            List<Entidades.App.DLLObject> lst = new List<Entidades.App.DLLObject>();
            List<Entidades.Tarea> objetos = new List<Entidades.Tarea>();

            if (agregaDefault)
            {
                lst.Add(new Entidades.App.DLLObject()
                {
                    Value = "0",
                    Text = textoDefault
                });
            }

            if (proy_id > 0)
            {
                objetos.AddRange(negocio.ListarConFiltros(new List<ObjectParameter>() { new ObjectParameter() { Name = "proy_id", Value = proy_id } }));

                foreach (Entidades.Tarea item in objetos.OrderBy(o => o.tar_nombre))
                {
                    lst.Add(new Entidades.App.DLLObject()
                    {
                        Value = item.tar_id.ToString(),
                        Text = item.tar_nombre
                    });
                }
            }

            if (lst.Count.Equals(0))
            {
                lst.Add(new Entidades.App.DLLObject()
                {
                    Value = "0",
                    Text = "No se encontraron tareas para vincular."
                });
            }
            return lst;
        }

        public static List<Entidades.App.DLLObject> ListarIndicadoresPorProyecto(Entidades.App.Token token, bool agregaDefault = false, string proy_id = "0", string textoDefault = "Sin Filtro")
        {
            Negocio.ProyectosIndicadores negocio = new ProyectosIndicadores(token);
            List<Entidades.App.DLLObject> lst = new List<Entidades.App.DLLObject>();
            List<Entidades.Proyecto_Indicador> objetos = new List<Entidades.Proyecto_Indicador>();

            if (agregaDefault)
            {
                lst.Add(new Entidades.App.DLLObject()
                {
                    Value = "0",
                    Text = textoDefault
                });
            }

            if (proy_id != "0")
            {
                int id = Convert.ToInt32(proy_id);
                objetos.AddRange(negocio.ListarConFiltros(new List<ObjectParameter>() { new ObjectParameter() { Name = "proy_id", Value = id } }));
            }
            else
                objetos = negocio.Listar();

            foreach (Entidades.Proyecto_Indicador item in objetos.OrderBy(o => o.pryind_nombre))
            {
                lst.Add(new Entidades.App.DLLObject()
                {
                    Value = item.pryind_id.ToString(),
                    Text = item.pryind_nombre
                });
            }

            if (lst.Count.Equals(0))
            {
                lst.Add(new Entidades.App.DLLObject()
                {
                    Value = "0",
                    Text = "No se encontraron Indicadores" +
                    "."
                });
            }
            return lst;
        }

        public static List<Entidades.App.DLLObject> ListarTipoPrioridades(Entidades.App.Token token, bool agregarDefault = true)
        {
            List<Entidades.App.DLLObject> lst = new List<Entidades.App.DLLObject>();
            Negocio.TipoPrioridades negocio = new TipoPrioridades(token);
            List<Entidades.Tipo_Prioridad> lstDatos = negocio.Listar();

            foreach (Entidades.Tipo_Prioridad item in lstDatos)
            {
                lst.Add(new DLLObject()
                {
                    Value = item.tprioridad_id.ToString(),
                    Text = item.tprioridad_nombre
                });
            }
            
            if (agregarDefault == true)
            {
                lst.Add(new DLLObject()
                {
                    Value = "0",
                    Text = " Seleccione "
                });
            }

            lst = lst.OrderBy(o => o.Value).ToList();
            return lst;
        }

        public static List<Entidades.App.DLLObject> ListarProyectos(Entidades.App.Token token, bool agregarDefault = true)
        {
            List<Entidades.App.DLLObject> lst = new List<Entidades.App.DLLObject>();
            Negocio.Proyectos negocio = new Proyectos(token);
            List<Entidades.Proyecto> lstDatos = negocio.Listar();

            foreach (Entidades.Proyecto item in lstDatos)
            {
                lst.Add(new DLLObject()
                {
                    Value = item.proy_id.ToString(),
                    Text = item.proy_titulo
                });
            }

            if (agregarDefault == true)
            {
                lst.Add(new DLLObject()
                {
                    Value = "0",
                    Text = " Seleccione "
                });
            }

            lst = lst.OrderBy(o => o.Value).ToList();
            return lst;
        }

        public static List<Entidades.App.DLLObject> ListarOrganizaciones(Entidades.App.Token token, bool agregaDefault = true,string textoDefault = "Seleccione")
        {
            Negocio.App.SIS_Organizaciones negocio = new App.SIS_Organizaciones(token);
            List<Entidades.App.DLLObject> lst = new List<Entidades.App.DLLObject>();
            
            if (agregaDefault)
            {
                lst.Add(new Entidades.App.DLLObject()
                {
                    Value = "0",
                    Text = textoDefault
                });
            }

            foreach (Entidades.App.SIS_Organizacion item in negocio.ListarSimple().OrderBy(o=>o.org_nombre))
            {
                lst.Add(new Entidades.App.DLLObject()
                {
                    Value = item.org_id.ToString(),
                    Text = item.org_nombre
                });
            }

            return lst;
        }

        public static List<Entidades.App.DLLObject> ListarAreasPadre(Entidades.App.Token oToken, string textoDefault = "")
        {
            List<Entidades.App.DLLObject> lst = new List<Entidades.App.DLLObject>();

            List<Entidades.App.SIS_Area> lstAreas = new Negocio.App.SIS_Areas(oToken).ListarPadres();

            lst.Add(new Entidades.App.DLLObject()
            {
                Text = "-",
                Value = "0"
            });

            foreach (Entidades.App.SIS_Area item in lstAreas)
            {
                lst.Add(new Entidades.App.DLLObject()
                {
                    Text = item.area_nombre,
                    Value = item.area_id.ToString()
                });
            }

            return lst;
        }

        public static List<Entidades.App.DLLObject> ListarEstadosTarea()
        {
            List<Entidades.App.DLLObject> lst = new List<Entidades.App.DLLObject>();

            lst.Add(new Entidades.App.DLLObject()
            {
                Text = "Sin Filtro",
                Value = "0"
            });

            lst.Add(new Entidades.App.DLLObject()
            {
                Text = "Edición",
                Value = Convert.ToString((int)Negocio.TareasEstados.Estados.Edicion)
            });

            lst.Add(new Entidades.App.DLLObject()
            {
                Text = "En Progreso",
                Value = Convert.ToString((int)Negocio.TareasEstados.Estados.EnProgreso)
            });

            lst.Add(new Entidades.App.DLLObject()
            {
                Text = "Pendiente",
                Value = Convert.ToString((int)Negocio.TareasEstados.Estados.Pendiente)
            });
            lst.Add(new Entidades.App.DLLObject()
            {
                Text = "Finalizada",
                Value = Convert.ToString((int)Negocio.TareasEstados.Estados.Finalizada)
            });

            return lst;
        }


        public static List<Entidades.App.DLLObject> ListarCategorias(Entidades.App.Token token, bool agregarDefault = false)
        {
            List<Entidades.App.DLLObject> lst = new List<Entidades.App.DLLObject>();
            Negocio.Categorias negocio = new Categorias(token);
            List<Entidades.Categoria> lstDatos = negocio.Listar();

            lstDatos.Add(new Entidades.Categoria()
            {
                cat_id = 0,
                cat_nombre = "Seleccione una Categoría"
            });

            foreach (Entidades.Categoria item in lstDatos)
            {
                lst.Add(new DLLObject()
                {
                    Value = item.cat_id.ToString(),
                    Text = item.cat_nombre
                });
            }

            lst = lst.OrderBy(o => o.Value).ToList();
            return lst;
        }


        public static List<Entidades.App.DLLObject> ListarSkills(Entidades.App.Token token, bool agregarDefault = false)
        {
            List<Entidades.App.DLLObject> lst = new List<Entidades.App.DLLObject>();
            Negocio.Skills negocio = new Skills(token);
            List<Entidades.Skill> lstDatos = negocio.Listar();

            lstDatos.Add(new Entidades.Skill()
            {
                skill_id = 0,
                skill_nombre = "Seleccione un Skill"
            });

            foreach (Entidades.Skill item in lstDatos)
            {
                lst.Add(new DLLObject()
                {
                    Value = item.skill_id.ToString(),
                    Text = item.skill_nombre
                });
            }

            lst = lst.OrderBy(o => o.Value).ToList();
            return lst;
        }

        public static List<Entidades.App.DLLObject> ListarAreas(Entidades.App.Token token, bool agregarDefault = false)
        {
            List<Entidades.App.DLLObject> lst = new List<Entidades.App.DLLObject>();
            Negocio.App.SIS_Areas negocio = new App.SIS_Areas(token);
            List<Entidades.App.SIS_Area> lstDatos = negocio.Listar();

            lstDatos.Add(new Entidades.App.SIS_Area()
            {
                area_id = 0,
                area_nombre = "Seleccione un Área"
            });

            foreach (Entidades.App.SIS_Area item in lstDatos)
            {
                lst.Add(new DLLObject()
                {
                    Value = item.area_id.ToString(),
                    Text = item.area_nombre
                });
            }

            lst = lst.OrderBy(o => o.Value).ToList();
            return lst;
        }


        public static List<Entidades.App.DLLObject> ListarTipoEncuesta(Entidades.App.Token token, bool agregarDefault = false)
        {
            List<Entidades.App.DLLObject> lst = new List<Entidades.App.DLLObject>();
            Negocio.Tipo_Encuestas negocio = new Tipo_Encuestas(token);
            List<Entidades.Tipo_Encuesta> lstDatos = negocio.Listar();

            lstDatos.Add(new Tipo_Encuesta()
            {
                tenc_id = 0,
                tenc_contenido = "Seleccione un Tipo de Encuesta"
            });

            foreach (Entidades.Tipo_Encuesta item in lstDatos)
            {
                lst.Add(new DLLObject()
                {
                    Value = item.tenc_id.ToString(),
                    Text = item.tenc_contenido
                });
            }

            lst = lst.OrderBy(o => o.Value).ToList();
            return lst;
        }

        public static List<Entidades.App.DLLObject> ListarEstadosEncuesta()
        {
            List<Entidades.App.DLLObject> lst = new List<Entidades.App.DLLObject>();

            lst.Add(new Entidades.App.DLLObject()
            {
                Text = "Sin Filtro",
                Value = "0"
            });

            lst.Add(new Entidades.App.DLLObject()
            {
                Text = "Disponibles",
                Value = Convert.ToString((int)Negocio.EncuestasUsuarios.Estados.Disponibles)
            });

            lst.Add(new Entidades.App.DLLObject()
            {
                Text = "Realizadas",
                Value = Convert.ToString((int)Negocio.EncuestasUsuarios.Estados.Realizadas)
            });

            return lst;
        }

    }

}

