using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace App2
{
    public class Encryption
    {

        private static string GetKey()
        {
            return "KPN12862ZD79943QP6789GLD";
        }



        public static string EncryptX(string inputText)
        {
            if (inputText != "")
            {
                RijndaelManaged rijndaelCipher = new RijndaelManaged();
                byte[] plainText = System.Text.Encoding.UTF8.GetBytes(inputText);

                rijndaelCipher.Mode = System.Security.Cryptography.CipherMode.ECB;
                rijndaelCipher.KeySize = 128;
                rijndaelCipher.BlockSize = 128;
                rijndaelCipher.Padding = PaddingMode.PKCS7;
                rijndaelCipher.Key = System.Text.Encoding.UTF8.GetBytes(GetKey());

                using (ICryptoTransform encryptor = rijndaelCipher.CreateEncryptor())
                {


                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(plainText, 0, plainText.Length);
                            cryptoStream.FlushFinalBlock();
                            string base64 = Convert.ToBase64String(memoryStream.ToArray());
                            return base64;
                        }

                    }

                }

            }
            else
                return "";


        }



        public static string DecryptX(string inputText)
        {
            if (inputText != "")
            {
                RijndaelManaged rijndaelCipher = new RijndaelManaged();
                byte[] encryptedData = Convert.FromBase64String(inputText);
                rijndaelCipher.Mode = System.Security.Cryptography.CipherMode.ECB;
                rijndaelCipher.KeySize = 128;
                rijndaelCipher.BlockSize = 128;
                rijndaelCipher.Padding = PaddingMode.PKCS7;
                rijndaelCipher.Key = System.Text.Encoding.UTF8.GetBytes(GetKey());

                using (ICryptoTransform decryptor = rijndaelCipher.CreateDecryptor())
                {
                    using (MemoryStream memoryStream = new MemoryStream(encryptedData))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            byte[] plainText = new byte[encryptedData.Length - 1 + 1];
                            int decryptedCount = cryptoStream.Read(plainText, 0, plainText.Length);
                            return System.Text.Encoding.UTF8.GetString(plainText, 0, decryptedCount);
                        }
                    }
                }
            }

            else
                return "";
        }




    }
}
