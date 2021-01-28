using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace OrderSort.Common
{
    /// <summary>
    /// DES加密/解密类。
    /// </summary>
    public class DESEncrypt
    {
        #region ========加密========
        /// <summary>
        /// 随机生成KEY
        /// </summary>
        /// <returns></returns>
        public static string GenerateKey()
        {
            int _len = 32;
            Random random = new Random(DateTime.Now.Millisecond);
            byte[] keybyte = new byte[_len];
            for (int i = 0; i < _len; i++)
            {
                keybyte[i] = (byte)random.Next(65, 122);
            }
            return ASCIIEncoding.ASCII.GetString(keybyte);
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Encrypt(string Text)
        {
            return Encrypt(Text, "banchenfileweb20170531");
        }
        /// <summary> 
        /// 加密数据
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Encrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray;
            inputByteArray = Encoding.Default.GetBytes(Text);
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        #endregion

        #region ========解密========


        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Decrypt(string Text)
        {
            return Decrypt(Text, "banchenfileweb20170531");
        }
        /// <summary> 
        /// 解密数据
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Decrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            int len;
            len = Text.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }

        /// <summary>
        /// 解密(兼容 .netCore)
        /// </summary>
        /// <param name="decryptString">解密内容</param>
        /// <param name="key">秘钥</param>
        /// <returns></returns>
        public static string DecryptLegal(string decryptString, string key)
        {
            try
            {
                int len;
                len = decryptString.Length / 2;
                byte[] inputByteArray = new byte[len];
                int x, i;
                for (x = 0; x < len; x++)
                {
                    i = Convert.ToInt32(decryptString.Substring(x * 2, 2), 16);
                    inputByteArray[x] = (byte)i;
                }

                if (decryptString == null)
                {
                    return "";
                }
                byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 8));
                byte[] keyIV = keyBytes;
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, provider.CreateDecryptor(keyBytes, keyIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// DES加密(兼容 .netCore)
        /// </summary>
        /// <param name="encryptString">要加密的字符串</param>
        /// <returns></returns>
        public static string EncryptLegal(string encryptString, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 8));
            byte[] keyIV = keyBytes;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, provider.CreateEncryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in mStream.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        #endregion

        #region ========MD5========
        public static string Md5(string value)
        {
            var result = string.Empty;
            if (string.IsNullOrEmpty(value)) return result;
            using (var md5 = MD5.Create())
            {
                result = GetMd5Hash(md5, value);
            }
            return result;
        }


        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sBuilder = new StringBuilder();
            foreach (byte t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }
            return sBuilder.ToString();
        }

        static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            var hashOfInput = GetMd5Hash(md5Hash, input);
            var comparer = StringComparer.OrdinalIgnoreCase;
            return 0 == comparer.Compare(hashOfInput, hash);
        }
        #endregion

        #region ========SHA1========
        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="content">待加密的字符串</param>
        /// <param name="encode">编码方式</param>
        /// <returns></returns>
        public static String Sha1Sign(String content, Encoding encode)
        {
            try
            {
                SHA1 sha1 = new SHA1CryptoServiceProvider();//创建SHA1对象
                byte[] bytes_in = encode.GetBytes(content);//将待加密字符串转为byte类型
                byte[] bytes_out = sha1.ComputeHash(bytes_in);//Hash运算
                sha1.Dispose();//释放当前实例使用的所有资源
                String result = BitConverter.ToString(bytes_out);//将运算结果转为string类型
                result = result.Replace("-", "").ToLower();//替换并转为大写
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion
    }
}
