using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace FinalExamService.Helper
{
    public static class Util
    {
        public static string EncryptPassword(string password)
        {
            SHA1CryptoServiceProvider hash = new SHA1CryptoServiceProvider();
            byte[] buffer = Encoding.Default.GetBytes(password);
            return BitConverter.ToString(hash.ComputeHash(buffer)).Replace("-", "");
        }

        public static string generateRandomString()
        {
            return Guid.NewGuid().ToString();
        }

        public static Boolean passwordChecker(String password)
        {
            Regex rgx = new Regex(@"^(?=.*[A-Z])(?=.*[0-9])(?=.*[a-z]).{8,15}$");
            return rgx.IsMatch(password);
        }
        public static MvcHtmlString EncodeUrl(string actionName, string controllerName, object routeValues, String tipe)
        {
            string queryString = string.Empty;
            if (routeValues != null)
            {
                RouteValueDictionary d = new RouteValueDictionary(routeValues);
                for (int i = 0; i < d.Keys.Count; i++)
                {
                    if (i > 0)
                    {
                        queryString += "?";
                    }
                    queryString += d.Keys.ElementAt(i) + "=" + d.Values.ElementAt(i);
                }
            }

            StringBuilder ancor = new StringBuilder();

            if (controllerName != string.Empty)
            {
                ancor.Append(controllerName);
            }

            if (actionName != "Index")
            {
                ancor.Append("/" + actionName);
            }
            if (queryString != string.Empty)
            {
                if (tipe == "string")
                {
                    ancor.Append("?s=" + Encrypt(queryString));
                }
                else if (tipe == "int")
                {
                    ancor.Append("?q=" + Encrypt(queryString));
                }
            }
            return MvcHtmlString.Create(ancor.ToString());
        }

        public static string Encrypt(string plainText)
        {
            string key = "adsg432387#";
            byte[] EncryptKey = { };
            byte[] IV = { 55, 34, 87, 64, 87, 195, 54, 21 };
            EncryptKey = System.Text.Encoding.UTF8.GetBytes(key.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByte = Encoding.UTF8.GetBytes(plainText);
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, des.CreateEncryptor(EncryptKey, IV), CryptoStreamMode.Write);
            cStream.Write(inputByte, 0, inputByte.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
        }


     
    }
}
