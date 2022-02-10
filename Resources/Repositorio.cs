using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;

namespace Resources
{
    public class Repositorio
    {
        public static string JSONSerialize(object objeto)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(objeto);
            return JSONString;
        }

        public static string JSONProperty(string JSONStr, string properyName)
        {
            var myJsonString = JSONStr;
            var jo = JObject.Parse(myJsonString);
            return jo[properyName].ToString();
        }

        public static string TokenAleatorio(int longCadena)
        {
            int longitud = longCadena;
            Guid miGuid = Guid.NewGuid();
            string token = Convert.ToBase64String(miGuid.ToByteArray());
            token = token.Replace("=", "").Replace("+", "");
            return token.Substring(0, longitud);
        }

        public static string FormatoFecha(string fecha)
        {
            bool primerslap = false;
            bool segundoslap = false;
            int largo = fecha.Length;
            string dia;
            string mes;
            string ano;
            if ((largo >= 2) && (primerslap == false))
            {
                dia = fecha.Substring(0, 2);
                if ((IsNumeric(dia) == true) && (Int32.Parse(dia) <= 31) && (dia != "00")) { fecha = fecha.Substring(0, 2) + "/" + fecha.Substring(3, 7); primerslap = true; }
                else { fecha = ""; primerslap = false; }
            }
            else
            {
                dia = fecha.Substring(0, 1);
                if (IsNumeric(dia) == false) { fecha = ""; }
                if ((largo <= 2) && (primerslap = true)) { fecha = fecha.Substring(0, 1); primerslap = false; }
            }
            if ((largo >= 5) && (segundoslap == false))
            {
                mes = fecha.Substring(3, 2);
                if ((IsNumeric(mes) == true) && (Int32.Parse(mes) <= 12) && (mes != "00")) { fecha = fecha.Substring(0, 5) + "/" + fecha.Substring(6, 4); segundoslap = true; }
                else { fecha = fecha.Substring(0, 3); ; segundoslap = false; }
            }
            else { if ((largo <= 5) && (segundoslap = true)) { fecha = fecha.Substring(0, 4); segundoslap = false; } }
            if (largo >= 7)
            {
                ano = fecha.Substring(6, 4);
                if (IsNumeric(ano) == false) { fecha = fecha.Substring(0, 6); }
                else { if (largo == 10) { if ((Int32.Parse(ano) == 0) || (Int32.Parse(ano) < 1900) || (Int32.Parse(ano) > 2100)) { fecha = fecha.Substring(0, 6); } } }
            }
            if (largo >= 10)
            {
                fecha = fecha.Substring(0, 10);
                dia = fecha.Substring(0, 2);
                mes = fecha.Substring(3, 2);
                ano = fecha.Substring(6, 4);
                // Año no viciesto y es febrero y el dia es mayor a 28
                if ((Int32.Parse(ano) % 4 != 0) && (Int32.Parse(mes) == 02) && (Int32.Parse(dia) > 28)) { fecha = fecha.Substring(0, 2) + "/"; }
            }
            return (fecha);
        }

        public static Boolean IsNumeric(string valor)
        {
            int result;
            return int.TryParse(valor, out result);
        }

        public static string GenerarCuil(int dni, int sexo)
        {
            //Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            int x;
            string cuil;
            for (x = 0; (x <= 9); x++)
            {
                cuil = (((sexo == 1) ? "20" : "27") + (padl0(dni, 8) + x.ToString()));
                if ((Resources.Repositorio.validacuit(cuil, dni.ToString()) == true))
                {
                    return cuil;
                    //oM.ObjectRelation = cuil;
                    //oM.Success = true;
                    //return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
                }

            }
            for (x = 0; (x <= 9); x++)
            {
                cuil = ("23" + (padl0(dni, 8) + x.ToString()));
                if ((Resources.Repositorio.validacuit(cuil, dni.ToString()) == true))
                {
                    return cuil;
                    //oM.ObjectRelation = cuil;
                    //oM.Success = true;
                    //return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
                }


            }
            for (x = 0; (x <= 9); x++)
            {
                cuil = ("24" + (padl0(dni, 8).ToString() + x.ToString()));
                if ((Resources.Repositorio.validacuit(cuil, dni.ToString()) == true))
                {
                    return cuil;
                    //oM.ObjectRelation = cuil;
                    //oM.Success = true;
                    //return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
                }

            }
            //oM.ObjectRelation = "Error cuil";
            //oM.Success = false;
            //return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
            return "";
        }

        private static string padl0(Decimal texto, int longitud)
        {
            return ("0000000000000000" + texto.ToString()).Substring((("0000000000000000" + texto.ToString()).Length - longitud));
        }

        public static int Edad(DateTime fec)
        {
            DateTime now = DateTime.Today;
            int edad = DateTime.Today.Year - fec.Year;

            if (DateTime.Today < fec.AddYears(edad))
            {
                --edad;
            }
            return edad;
        }

        public static DateTime fecha_to_date(string fecha)
        {
            if ((fecha == "__/__/____"))
            {
                DateTime? fec = DateTime.MinValue;
                return (fec.Value);
            }
            else
            {
                try
                {
                    return DateTime.Parse(fecha_to_ddmm(fecha));
                }
                catch (Exception ex)
                {
                    try
                    {
                        return DateTime.Parse(fecha);
                    }
                    catch (Exception ex1)
                    {
                        DateTime? fec = DateTime.MinValue;
                        return (fec.Value);
                    }

                }

            }

        }

        public static string fecha_to_ddmm(string fecha)
        {
            int x;
            string fecha2;
            if ((fecha.Length == 10))
            {
                x = 4;
            }
            else
            {
                x = 10;
            }

            try
            {
                fecha2 = (fecha.Substring(3, 3) + (fecha.Substring(0, 3) + fecha.Substring(6, x)));
            }
            catch (Exception ex)
            {
                fecha2 = "";
            }

            return fecha2;
        }

        public static string formatocuit(string cuit)
        {
            cuit = cuit.Substring(0, 2) + "-" + cuit.Substring(2, 8) + "-" + cuit.Substring(10, 1);
            return cuit;
        }

        public static bool validacuit(string cuil, string dni)
        {
            int mk_suma;
            string nro =
            cuil = cuil.Replace("-", "");

            if (Repositorio.IsNumeric(dni))
            {
                if ((cuil.Length != 11))
                {
                    return false;
                }
                else
                {
                    mk_suma = 0;
                    mk_suma = (mk_suma
                                + (int.Parse(cuil.Substring(0, 1)) * 5));
                    mk_suma = (mk_suma
                                + (int.Parse(cuil.Substring(1, 1)) * 4));
                    mk_suma = (mk_suma
                                + (int.Parse(cuil.Substring(2, 1)) * 3));
                    mk_suma = (mk_suma
                                + (int.Parse(cuil.Substring(3, 1)) * 2));
                    mk_suma = (mk_suma
                                + (int.Parse(cuil.Substring(4, 1)) * 7));
                    mk_suma = (mk_suma
                                + (int.Parse(cuil.Substring(5, 1)) * 6));
                    mk_suma = (mk_suma
                                + (int.Parse(cuil.Substring(6, 1)) * 5));
                    mk_suma = (mk_suma
                                + (int.Parse(cuil.Substring(7, 1)) * 4));
                    mk_suma = (mk_suma
                                + (int.Parse(cuil.Substring(8, 1)) * 3));
                    mk_suma = (mk_suma
                                + (int.Parse(cuil.Substring(9, 1)) * 2));
                    mk_suma = (mk_suma
                                + (int.Parse(cuil.Substring(10, 1)) * 1));
                }

                decimal m = mk_suma;
                if (Math.Round(m / 11, 0) == (mk_suma / 11))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }


        }

        public static bool ValidaMail(String email)
        {
            String expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static DateTime Fecha_To_Date_YYYY_MM_DD(String dd_mm_yyyy, Boolean mm_dd = false)
        {
            string dia = "";
            string mes = "";
            string ano = "";
            string fecha = "";
            string fec = dd_mm_yyyy;
            if (fec.Length == 22)
            {
                dia = fec.Substring(0, 2);
                mes = fec.Substring(3, 2);
                ano = fec.Substring(fec.Length - 16, 4);
            }
            else
            {
                if (fec.Substring(1, 1) == "/" && fec.Substring(3, 1) == "/")
                {
                    dia = fec.Substring(0, 1);
                    mes = fec.Substring(2, 1);
                    ano = fec.Substring(4, 4);
                }
                else
                {
                    if (fec.Substring(1, 1) == "/" && fec.Substring(4, 1) == "/")
                    {
                        dia = fec.Substring(0, 1);
                        mes = fec.Substring(2, 2);
                        ano = fec.Substring(5, 4);
                    }
                    else
                    {
                        if (fec.Substring(2, 1) == "/" && fec.Substring(4, 1) == "/")
                        {
                            dia = fec.Substring(0, 2);
                            mes = fec.Substring(3, 1);
                            ano = fec.Substring(5, 4);
                        }
                    }
                }
            }
            if (mm_dd)
            {
                fecha = ano + "-" + dia + "-" + mes;
            }
            else
            {
                fecha = ano + "-" + mes + "-" + dia;
            }
            DateTime fecha_date = Convert.ToDateTime(fecha);
            return fecha_date;
        }

        public static String Fecha_To_String_YYYY_MM_DD(String dd_mm_yyyy, Boolean mm_dd = false)
        {
            string dia = "";
            string mes = "";
            string ano = "";
            string fecha = "";
            string fec = dd_mm_yyyy;
            if (fec.Length == 22)
            {
                dia = fec.Substring(0, 2);
                mes = fec.Substring(3, 2);
                ano = fec.Substring(fec.Length - 16, 4);
            }
            else
            {
                if (fec.Substring(1, 1) == "/" && fec.Substring(3, 1) == "/")
                {
                    dia = "0" + fec.Substring(0, 1);
                    mes = "0" + fec.Substring(2, 1);
                    ano = fec.Substring(4, 4);
                }
                else
                {
                    if (fec.Substring(1, 1) == "/" && fec.Substring(4, 1) == "/")
                    {
                        dia = "0" + fec.Substring(0, 1);
                        mes = fec.Substring(2, 2);
                        ano = fec.Substring(5, 4);
                    }
                    else
                    {
                        if (fec.Substring(2, 1) == "/" && fec.Substring(4, 1) == "/")
                        {
                            dia = fec.Substring(0, 2);
                            mes = "0" + fec.Substring(3, 1);
                            ano = fec.Substring(5, 4);
                        }
                    }
                }
            }
            if (mm_dd)
            {
                fecha = ano + "-" + dia + "-" + mes;
            }
            else
            {
                fecha = ano + "-" + mes + "-" + dia;
            }
            //DateTime fecha_date = Convert.ToDateTime(fecha);
            return fecha;
        }

        public Boolean IsDate(string fec)
        {
            DateTime fromDateValue;
            var formats = new[] { "dd/MM/yyyy", "yyyy-MM-dd" };
            if (DateTime.TryParseExact(fec, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out fromDateValue))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static DateTime FechaARGToDate(string fechaARG)
        {
            return DateTime.ParseExact(fechaARG, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        public static bool FechaARGValida(string fechaARG)
        {
            DateTime fecha;

            if (DateTime.TryParseExact(fechaARG + " 12:00", "dd/MM/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out fecha) == false)
                return false;

            if (FechaARGToDate(fechaARG).Year < 2000)
                return false;

            return true;
        }

        public static int GetDaysDifference(DateTime startDate, DateTime endDate)
        {
            int days = (endDate.Date - startDate.Date).Days;
            return days;
        }

        public static int GetMonthDifference(DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            return Math.Abs(monthsApart);
        }

        public static int GetYearDifference(DateTime startDate, DateTime endDate)
        {
            try
            {
                return (startDate.Year - endDate.Year);
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public static int FechaToPeriodo(DateTime fechaDate)
        {
            string valPer = "0";

            valPer = fechaDate.Year.ToString();

            if (fechaDate.Month.ToString().Length == 1)
            {
                valPer += "0" + fechaDate.Month.ToString();
            }
            else
            {
                valPer += fechaDate.Date.Month.ToString();
            }

            return Convert.ToInt32(valPer);
        }

        public static int FechaToPeriodo(int anio, int mes)
        {
            DateTime oDate;

            if (anio == 0 || mes == 0)
            {
                oDate = DateTime.Now.Date;
            }
            else
            {
                oDate = new DateTime(anio, mes, 1);
            }

            return FechaToPeriodo(oDate);
        }

        public static string FechaToAAMMDD(DateTime fecha)
        {
            var strAnio = fecha.Year.ToString().Substring(2, 2);
            var strMes = fecha.Month.ToString();
            var strDia = fecha.Day.ToString();

            if (strMes.Length == 1)
            {
                strMes = "0" + strMes;
            }

            if (strDia.Length == 1)
            {
                strDia = "0" + strDia;
            }

            return strAnio + strMes + strDia;
        }

        public static string FechaToMMAA(DateTime fecha)
        {
            var strAnio = fecha.Year.ToString().Substring(2, 2);
            var strMes = fecha.Month.ToString();

            if (strMes.Length == 1)
                strMes = "0" + strMes;

            return strMes + strAnio;
        }

        public static string FechaToDDMMAAAA(DateTime fecha)
        {
            var strAnio = fecha.Year.ToString();
            var strMes = fecha.Month.ToString();
            var strDia = fecha.Day.ToString();

            if (strMes.Length == 1)
            {
                strMes = "0" + strMes;
            }

            if (strDia.Length == 1)
            {
                strDia = "0" + strDia;
            }

            return strDia + strMes + strAnio;
        }

        public static string FechaToDDMM(DateTime fecha)
        {
            var strMes = fecha.Month.ToString();
            var strDia = fecha.Day.ToString();

            if (strMes.Length == 1)
            {
                strMes = "0" + strMes;
            }

            if (strDia.Length == 1)
            {
                strDia = "0" + strDia;
            }

            return strDia + strMes;
        }

        public static string FechaToDDMMAA(DateTime fecha)
        {
            var strMes = fecha.Month.ToString();
            var strDia = fecha.Day.ToString();
            var strAnio = fecha.Year.ToString().Substring(2, 2);

            if (strMes.Length == 1)
            {
                strMes = "0" + strMes;
            }

            if (strDia.Length == 1)
            {
                strDia = "0" + strDia;
            }

            return strDia + strMes + strAnio;
        }

        public static string FechaToAAAAMMDD(DateTime fecha)
        {
            var strAnio = fecha.Year.ToString();
            var strMes = fecha.Month.ToString();
            var strDia = fecha.Day.ToString();

            if (strMes.Length == 1)
            {
                strMes = "0" + strMes;
            }

            if (strDia.Length == 1)
            {
                strDia = "0" + strDia;
            }

            return strAnio + strMes + strDia;
        }

        public static string AddCeros(int cantidad)
        {
            string resultado = "";

            for (int i = 0; i < cantidad; i++)
            {
                resultado += "0";
            }

            return resultado;
        }

        public static string AddEspacio(int cantidad)
        {
            string resultado = "";

            for (int i = 0; i < cantidad; i++)
            {
                resultado += " ";
            }

            return resultado;
        }

        public static string PadLeftCeros(string cadena, int longitudMaxima)
        {
            int lcadena = cadena.Length;
            int cantCeros = longitudMaxima - lcadena;
            return AddCeros(cantCeros) + cadena;
        }

        public static string PadRightCeros(string cadena, int longitudMaxima)
        {
            int lcadena = cadena.Length;
            int cantCeros = longitudMaxima - lcadena;
            return cadena + AddCeros(cantCeros);
        }

        public static string PadLeftVacios(string cadena, int longitudMaxima)
        {
            int lcadena = cadena.Length;
            int cantCeros = longitudMaxima - lcadena;
            return AddEspacio(cantCeros) + cadena;
        }

        public static string PadRightVacios(string cadena, int longitudMaxima)
        {
            int lcadena = cadena.Length;
            int cantCeros = longitudMaxima - lcadena;
            return cadena + AddEspacio(cantCeros);
        }

        public static int MesPeriodo(int periodo)
        {
            return Convert.ToInt32(periodo.ToString().Substring(4, 2));
        }

        public static string MesPeriodoV2(int periodo)
        {
            var mes = Convert.ToInt32(periodo.ToString().Substring(4, 2));

            if (mes.ToString().Trim().Length == 1)
            {
                return "0" + mes.ToString();
            }
            else
            {
                return mes.ToString();
            }
        }

        public static int AnioPeriodo(int periodo)
        {
            return Convert.ToInt32(periodo.ToString().Substring(0, 4));
        }

        public static string Periodo(int mes, int anio)
        {
            string sPer = "";

            sPer = anio.ToString();

            if (mes.ToString().Trim().Length == 1)
            {
                sPer += "0" + mes.ToString();
            }
            else
            {
                sPer += mes.ToString();
            }

            return sPer;
        }

        public static DateTime PeriodoToFecha(int periodo, int dia, bool ultimoDiaMes = false)
        {
            int pAnio = AnioPeriodo(periodo);
            int pMes = MesPeriodo(periodo);
            int pDia = dia;

            if (ultimoDiaMes == true)
            {
                pDia = new DateTime(pAnio, pMes, 1).AddMonths(1).AddDays(-1).Day;
            }

            DateTime oDate = new DateTime(pAnio, pMes, pDia);

            return oDate;
        }

        public static string PeriodoToYYMM(int periodo)
        {
            return periodo.ToString().Substring(2, periodo.ToString().Trim().Length - 2);
        }

        public static string PrefijoCBU(string nroCBU)
        {
            if (nroCBU.Trim().Length >= 3)
            {
                return nroCBU.Substring(0, 3);
            }
            else
            {
                return "000";
            }
        }

        public static bool Validate_CUIL_CUIT(string cuit)
        {
            if (string.IsNullOrEmpty(cuit)) throw new ArgumentNullException(nameof(cuit));
            if (cuit.Length != 13) throw new ArgumentException(nameof(cuit));
            bool rv = false;
            int verificador;
            int resultado = 0;
            string cuit_nro = cuit.Replace("-", string.Empty);
            string codes = "6789456789";
            long cuit_long = 0;
            if (long.TryParse(cuit_nro, out cuit_long))
            {
                verificador = int.Parse(cuit_nro[cuit_nro.Length - 1].ToString());
                int x = 0;
                while (x < 10)
                {
                    int digitoValidador = int.Parse(codes.Substring((x), 1));
                    int digito = int.Parse(cuit_nro.Substring((x), 1));
                    int digitoValidacion = digitoValidador * digito;
                    resultado += digitoValidacion;
                    x++;
                }
                resultado = resultado % 11;
                rv = (resultado == verificador);
            }
            return rv;
        }

        public static bool Validate_CBU(string cbu)
        {
            try
            {
                if (cbu.Length != 22)
                    return false;

                char[] cbuArray = cbu.ToCharArray();

                for (int i = 0; i < cbuArray.Length; i++)
                {
                    if (IsNumeric(cbuArray[i].ToString()) == false)
                        throw new Exception();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }

        public static string PathTemplatesMailHTML()
        {
            return System.Configuration.ConfigurationManager.AppSettings["mail_path_templates"];
        }

        public static string PathArchivosMail()
        {
            return System.Configuration.ConfigurationManager.AppSettings["mail_path_files"];
        }

        public static string MailOrigen()
        {
            return System.Configuration.ConfigurationManager.AppSettings["mail_origen"];
        }

        public static bool MailSSL()
        {
            var valorSSL = System.Configuration.ConfigurationManager.AppSettings["mail_ssl"];

            if (valorSSL == "0")
                return false;
            else
                return true;
        }

        public static string MailCredentialsUser()
        {
            return System.Configuration.ConfigurationManager.AppSettings["mail_credentials_user"];
        }

        public static string MailCredentialsPassword()
        {
            return System.Configuration.ConfigurationManager.AppSettings["mail_credentials_pass"];
        }

        public static string MailHost()
        {
            return System.Configuration.ConfigurationManager.AppSettings["mail_host"];
        }

        public static string MailPort()
        {
            return System.Configuration.ConfigurationManager.AppSettings["mail_port"];
        }


        public static string RandomCode(int min, int max)
        {
            Random random = new Random(); return random.Next(min, max).ToString();
        }

        public static int ParteEntera(decimal obj)
        {
            try
            {
                return int.Parse(obj.ToString().Split(',')[0]);
            }
            catch (Exception ex)
            {
                return -1;
            }

        }

        public static int ParteDecimal(decimal obj)
        {
            try
            {
                return int.Parse(obj.ToString().Split(',')[1]);
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public static string IncludeFilePaths()
        {
            if (System.Configuration.ConfigurationManager.AppSettings["enviroment_execute"] == "DEV")
                return HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority;
            else
                return System.Configuration.ConfigurationManager.AppSettings["application_path"];
        }

        public static int ExpirationTokenMinutes()
        {
            return Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["expiration_token_minutes"]);
        }

        public static string APP_Title()
        {
            return System.Configuration.ConfigurationManager.AppSettings["APP_title"].ToString();
        }


        public static int LoginMaxAttemps()
        {
            return Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["login_max_attemps"]);
        }

    }
}