using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.App
{
    public class Security
    {
        public enum CryptoProvider
        {
            DES,
            TripleDES,
            RC2,
            Rijndael
        }

        public enum CryptoAction
        {
            Encrypt,
            Desencrypt
        }

        public static string EncriptarBasico(string cadena)
        {
            string result = string.Empty;
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(cadena);
            result = Convert.ToBase64String(encryted);
            return result;
        }

        public static string DesencriptarBasico(string cadena)
        {
            string result = string.Empty;

            try
            {
                byte[] decryted = Convert.FromBase64String(cadena);
                result = System.Text.Encoding.Unicode.GetString(decryted);
            }
            catch (Exception ex)
            {
                result = cadena;
            }
            
            return result;
        }

        public static string Encriptar(string cadena)
        {
            CryptoClass oCrypto = new CryptoClass(CryptoProvider.TripleDES);
            string encriptVal = oCrypto.CryptString(cadena);
            oCrypto = null;
            return encriptVal;
        }

        public static string Desencriptar(string cadena)
        {
            CryptoClass oCrypto = new CryptoClass(CryptoProvider.TripleDES);
            string desencriptVal = oCrypto.DecryptString(cadena);
            oCrypto = null;
            return desencriptVal;
        }

        public static string EncriptarID(string cadena)
        {
            //CryptoClass oCrypto = new CryptoClass(CryptoProvider.DES);
            //string encriptVal = oCrypto.CryptString(cadena);
            //oCrypto = null;
            //return encriptVal;
            string result = string.Empty;
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(cadena);
            result = SpecialChars_Change(Convert.ToBase64String(encryted));
            return result;
        }

        public static string DesencriptarID(string cadena)
        {
            //CryptoClass oCrypto = new CryptoClass(CryptoProvider.DES);
            //string desencriptVal = oCrypto.DecryptString(cadena);
            //oCrypto = null;
            //return desencriptVal;

            string result = string.Empty;
            byte[] decryted = Convert.FromBase64String(SpecialChars_Recreate(cadena));
            //result = System.Text.Encoding.Unicode.GetString(decryted, 0, decryted.ToArray().Length);
            result = System.Text.Encoding.Unicode.GetString(decryted);
            return result;
        }

        public static string SpecialChars_Change(string cadena)
        {
            cadena = cadena.Replace("=", "SC1")
                           .Replace("/", "SC2")
                           .Replace("?", "SC3")
                           .Replace("%", "SC4");

            return cadena;
        }

        public static string SpecialChars_Recreate(string cadena)
        {
            cadena = cadena.Replace("SC1", "=")
                           .Replace("SC2", "/")
                           .Replace("SC3", "?")
                           .Replace("SC4", "%");

            return cadena;
        }

        public static int GetID(string id)
        {
            if (Resources.Repositorio.IsNumeric(id))
                return Convert.ToInt32(id);
            else
                return Convert.ToInt32(DesencriptarID(id));
        }

        public static Entidades.App.IntegritySecurityRequest IntegritySecurityObject(string cadenaEncriptada)
        {
            string[] _cadenaSplit = DesencriptarBasico(cadenaEncriptada).Split('_');

            Entidades.App.IntegritySecurityRequest obj = new Entidades.App.IntegritySecurityRequest();
            obj.DefaultPrimary = _cadenaSplit[0];
            obj.DefaultSecondary = _cadenaSplit[1];
            obj.ID = _cadenaSplit[2];

            return obj;
        }

        public static string GenerateTrackID(string distritoID, string prefijoTabla, string idregistro)
        {
            return EncriptarBasico(distritoID + "/" + "/" + prefijoTabla + "/" + idregistro);
        }

        public static Entidades.App.IntegritySecurityRequest ReadGenerateTrackID(string cadenaEncriptada)
        {
            string[] _cadenaSplit = DesencriptarBasico(cadenaEncriptada).Split('/');

            Entidades.App.IntegritySecurityRequest obj = new Entidades.App.IntegritySecurityRequest();
            obj.DefaultPrimary = _cadenaSplit[0];
            obj.DefaultSecondary = _cadenaSplit[1];
            obj.ID = _cadenaSplit[2];

            return obj;
        }

        public static string CookieSessionName()
        {
            return "VSTKID";
        }

        public static string DefaultPassword()
        {
            return "viasano";
        }

        public static Entidades.App.Token DesencriptarToken(string tokenEncrypt)
        {
            try
            {
                string tokenDesencriptado = Negocio.App.Security.Desencriptar(tokenEncrypt);
                return JsonConvert.DeserializeObject<Entidades.App.Token>(tokenDesencriptado);
            }
            catch (Exception)
            {

                throw new Exception("No se pudo desencriptar el token");
            }
        }

        public static Entidades.App.Token JsonToToken(string jSONtoken)
        {
            try
            {
                return JsonConvert.DeserializeObject<Entidades.App.Token>(jSONtoken);
            }
            catch (Exception)
            {

                throw new Exception("No se pudo desencriptar el token");
            }
        }

        public static string FilterSelectQuery(string pQuery)
        {
            pQuery = pQuery.Replace("UPDATE", "");
            pQuery = pQuery.Replace("INSERT", "");
            pQuery = pQuery.Replace("DELETE", "");
            pQuery = pQuery.Replace("DROP", "");
            pQuery = pQuery.Replace("ALTER", "");

            return pQuery;
        }

        public static string StopSQLInjection(string pQuery)
        {

            pQuery = pQuery.Replace("'", "");
            pQuery = pQuery.Replace("+", "");
            pQuery = pQuery.Replace("-", "");
            // pQuery = GetSafeHtmlFragment(pCadena);

            return pQuery;
        }

        public static bool ExisteSesion(string token, string browser_id)
        {
            return true;
        }


        #region ### CrytoClass ###

        public sealed class CryptoClass
        {
          
            private string stringKey = "QFS";
            private string stringIV = "QFS";
            private CryptoProvider algorithm;

            #region .Properties.

            public string Key
            {
                get { return stringKey; }
                set { stringKey = value; }
            }

            public string IV
            {
                get { return stringIV; }
                set { stringIV = value; }
            }

            #endregion

            #region .Constructors.

            public CryptoClass(CryptoProvider alg)
            {
                algorithm = alg;

            }

            public CryptoClass(CryptoProvider alg, string key, string IV)
            {
                algorithm = alg;
                stringKey = key;
                stringIV = IV;
            }

            #endregion

            #region .Functions & Methods.

            private byte[] MakeKeyByteArray()
            {
                switch (this.algorithm)
                {
                    case CryptoProvider.RC2:
                    case CryptoProvider.DES:
                        if ((stringKey.Length < 8))
                        {
                            stringKey = stringKey.PadRight(8);
                        }
                        else if ((stringKey.Length > 8))
                        {
                            stringKey = stringKey.Substring(0, 8);
                        }
                        break; // TODO: might not be correct. Was : Exit Select
                    case CryptoProvider.Rijndael:
                    case CryptoProvider.TripleDES:
                        if ((stringKey.Length < 16))
                        {
                            stringKey = stringKey.PadRight(16);
                        }
                        else if ((stringKey.Length > 16))
                        {
                            stringKey = stringKey.Substring(0, 16);
                        }
                        break; // TODO: might not be correct. Was : Exit Select
                }

                return Encoding.UTF8.GetBytes(stringKey);
            }

            private byte[] MakeIVByteArray()
            {

                switch (this.algorithm)
                {
                    case CryptoProvider.RC2:
                    case CryptoProvider.DES:
                        if (stringIV.Length < 8)
                        {
                            stringIV = stringIV.PadRight(8);
                        }
                        else if (stringKey.Length > 8)
                        {
                            stringIV = stringIV.Substring(0, 8);
                        }
                        break;
                    case CryptoProvider.Rijndael:
                    case CryptoProvider.TripleDES:
                        {
                            if (stringIV.Length < 16)
                            {
                                stringIV = stringIV.PadRight(16);
                            }
                            else if (stringIV.Length > 16)
                            {
                                stringIV = stringIV.Substring(0, 16);
                            }
                            break;
                        }
                }

                return Encoding.UTF8.GetBytes(stringIV);
            }

            public string CryptString(string CadenaOriginal)
            {
                MemoryStream memStream = null;

                try
                {
                    if (stringKey.Length > 0 & stringIV.Length > 0)
                    {
                        byte[] key = MakeKeyByteArray();
                        byte[] IV = MakeIVByteArray();
                        byte[] textoPlano = Encoding.UTF8.GetBytes(CadenaOriginal);

                        memStream = new MemoryStream(CadenaOriginal.Length * 2);

                        CryptoServiceProvider cryptoProvider = new CryptoServiceProvider(algorithm, CryptoAction.Encrypt);
                        ICryptoTransform transform = cryptoProvider.GetServiceProvider(key, IV);
                        CryptoStream cs = new CryptoStream(memStream, transform, CryptoStreamMode.Write);
                        cs.Write(textoPlano, 0, textoPlano.Length);
                        cs.Close();
                    }
                    else
                    {
                        throw new Exception("Error al inicializar la clave y el vector");
                    }

                }
                catch (Exception)
                {
                    throw;
                }

                return Convert.ToBase64String(memStream.ToArray());

            }

            public string DecryptString(string CadenaCifrada)
            {
                MemoryStream memStream = null;

                try
                {
                    if (stringKey.Length > 0 & stringIV.Length > 0)
                    {
                        byte[] key = MakeKeyByteArray();
                        byte[] IV = MakeIVByteArray();
                        byte[] textoCifrado = Convert.FromBase64String(CadenaCifrada);

                        memStream = new MemoryStream(CadenaCifrada.Length);

                        CryptoServiceProvider cryptoProvider = new CryptoServiceProvider(algorithm, CryptoAction.Desencrypt);
                        ICryptoTransform transform = cryptoProvider.GetServiceProvider(key, IV);
                        CryptoStream cs = new CryptoStream(memStream, transform, CryptoStreamMode.Write);
                        cs.Write(textoCifrado, 0, textoCifrado.Length);
                        cs.Close();
                    }
                    else
                    {
                        throw new Exception("Error al inicializar la clave y el vector");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return Encoding.UTF8.GetString(memStream.ToArray());
            }

            public bool EDFile(string InFileName, string OutFileName, CryptoAction Action)
            {
                bool functionReturnValue = false;
                functionReturnValue = false;
                if (!File.Exists(InFileName))
                {
                    functionReturnValue = false;
                    return false;
                }
                try
                {
                    if (stringKey.Length > 0 & stringIV.Length > 0)
                    {
                        FileStream fsIn = new FileStream(InFileName, FileMode.Open, FileAccess.Read);
                        FileStream fsOut = new FileStream(OutFileName, FileMode.OpenOrCreate, FileAccess.Write);
                        fsOut.SetLength(0);
                        byte[] key = MakeKeyByteArray();
                        byte[] IV = MakeIVByteArray();
                        byte[] byteBuffer = new byte[4097];
                        long largoArchivo = fsIn.Length;
                        long bytesProcesados = 0;
                        int bloqueBytes = 0;
                        CryptoServiceProvider cryptoProvider = new CryptoServiceProvider(algorithm, Action);
                        ICryptoTransform transform = cryptoProvider.GetServiceProvider(key, IV);
                        CryptoStream cryptoStream = null;
                        switch (Action)
                        {
                            case CryptoAction.Encrypt:
                                cryptoStream = new CryptoStream(fsOut, transform, CryptoStreamMode.Write);
                                break;
                            case CryptoAction.Desencrypt:
                                cryptoStream = new CryptoStream(fsOut, transform, CryptoStreamMode.Write);
                                break;
                        }
                        while ((bytesProcesados < largoArchivo))
                        {
                            bloqueBytes = fsIn.Read(byteBuffer, 0, 4096);
                            cryptoStream.Write(byteBuffer, 0, bloqueBytes);
                            bytesProcesados += Convert.ToInt64(bloqueBytes);
                        }
                        if ((cryptoStream != null))
                            cryptoStream.Close();
                        fsIn.Close();
                        fsOut.Close();
                        functionReturnValue = true;
                    }
                    else
                    {
                        functionReturnValue = false;
                    }
                }
                catch
                {
                }
                return functionReturnValue;

            }

            #endregion

            #region .Interfaz CryptoTransform.

            private class CryptoServiceProvider
            {
                CryptoProvider algorithm;
                CryptoAction cAction;

                // Sub new friend
                internal CryptoServiceProvider(CryptoProvider alg, CryptoAction action)
                {
                    algorithm = alg;
                    cAction = action;
                }

                // Funcion Friend

                internal ICryptoTransform GetServiceProvider(byte[] Key, byte[] IV)
                {
                    ICryptoTransform transform = null;

                    switch (algorithm)
                    {
                        case CryptoProvider.DES:
                            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                            switch (cAction)
                            {
                                case CryptoAction.Encrypt:
                                    transform = des.CreateEncryptor(Key, IV);
                                    break;
                                case CryptoAction.Desencrypt:
                                    transform = des.CreateDecryptor(Key, IV);
                                    break;
                            }
                            return transform;
                        case CryptoProvider.RC2:
                            RC2CryptoServiceProvider rc2 = new RC2CryptoServiceProvider();

                            switch (cAction)
                            {
                                case CryptoAction.Encrypt:
                                    transform = rc2.CreateEncryptor(Key, IV);
                                    break;
                                case CryptoAction.Desencrypt:
                                    transform = rc2.CreateDecryptor(Key, IV);
                                    break;
                            }

                            return transform;
                        case CryptoProvider.Rijndael:
                            Rijndael oRijndael = new RijndaelManaged();

                            switch (cAction)
                            {
                                case CryptoAction.Encrypt:
                                    transform = oRijndael.CreateEncryptor(Key, IV);
                                    break;
                                case CryptoAction.Desencrypt:
                                    transform = oRijndael.CreateDecryptor(Key, IV);
                                    break;
                            }

                            return transform;
                        case CryptoProvider.TripleDES:
                            TripleDESCryptoServiceProvider oDes = new TripleDESCryptoServiceProvider();

                            switch (cAction)
                            {
                                case CryptoAction.Encrypt:
                                    transform = oDes.CreateEncryptor(Key, IV);
                                    break;
                                case CryptoAction.Desencrypt:
                                    transform = oDes.CreateDecryptor(Key, IV);
                                    break;
                            }

                            return transform;
                    }

                    return null;
                }



            }

            #endregion

        }

        #endregion

        public class SHA1
        {
            public static string Encode(string value)
            {
                var hash = System.Security.Cryptography.SHA1.Create();
                var encoder = new System.Text.ASCIIEncoding();
                var combined = encoder.GetBytes(value ?? "");
                return BitConverter.ToString(hash.ComputeHash(combined)).ToLower().Replace("-", "");
            }
        }




    }
}
