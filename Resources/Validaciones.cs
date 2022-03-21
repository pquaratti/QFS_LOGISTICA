﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources
{
    public class Validaciones
    {
        private static Validaciones _instancia = null;
        protected Validaciones() { }
             
        public static Validaciones Instancia
        {
            get
            {
                if (_instancia == null)
                    _instancia = new Validaciones();

                return _instancia;
            }
        }

        public static int valNULLINT(object obj, int defaultValue = 0)
        {

            if (obj is DBNull)
            {
                return defaultValue;
            }
            else
            {
                return (int)(obj);
            }  
        }

        public static string valNULLString(object obj, string defaultValue = "")
        {
            if (obj is DBNull)
                return defaultValue;
            else
                return (string)(obj);
                //return obj.ToString();
        }

        public static Decimal valNULLDecimal(object obj)
        {
            if (obj is DBNull)
                return new decimal(0.00);
            else
                return (decimal)(obj);
            //return obj.ToString();
        }

        public static Boolean valNULLBool(object obj)
        {
            if (obj is DBNull)
                return false;
            else
                return (bool)(obj)  ;
        }

        public static DateTime valNULLDateTime(object obj)
        {
            DateTime s;
            s = DateTime.MinValue;
            if (obj is DBNull)
                return s;
            else
            {
                if (((string)obj).Trim().Length.Equals(0))
                {
                    return s;
                }
                else
                {
                    return Convert.ToDateTime(obj);
                }
            }
                
        }

        public static DateTime valNULLDate(object obj)
        {
            DateTime s;
            s = DateTime.MinValue;
            if (obj is DBNull)
                return s.Date;
            else
                return Convert.ToDateTime(obj).Date;
        }

        public static DateTime valNULLDateTimeMax(object obj)
        {
            DateTime s;
            s = DateTime.MaxValue;
            if (obj is DBNull)
                return s;
            else
                return Convert.ToDateTime(obj);
        }

        public static Byte valNULLBinary(object obj)
        {
            Byte  s;
            s = Byte.MaxValue;
            if (obj is DBNull)
                return s;
            else
                return Convert.ToByte(obj);
        }



    }
}
