using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MCIGrabberService.Helper
{
    class UtilityHelper
    {
        public string GetDomainNameOfUrlString(string urlString)
        {
            if (!string.IsNullOrEmpty(urlString) && urlString.Contains("."))
            {
                var host = new Uri(urlString).Host;
                return host.Substring(host.LastIndexOf('.', host.LastIndexOf('.') - 1) + 1);
            }
            else if (!string.IsNullOrEmpty(urlString))
            {
                return new Uri(urlString).Host;
            }
            else
                return null;
        }

        public string EncryptString(string key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        
    }
}
