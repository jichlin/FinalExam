using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Web.Routing;
using System.Security.Cryptography;
using System.IO;

namespace FinalExamService.Helper
{
    public static class MyExtensions
    {
        public static MvcHtmlString EncodedActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes,String tipe)
        {
            string queryString = string.Empty;
            string htmlAttributesString = string.Empty;
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

            //if (htmlAttributes != null)
            //{
            //    RouteValueDictionary d = new RouteValueDictionary(htmlAttributes);
            //    for (int i = 0; i < d.Keys.Count; i++)
            //    {
            //        htmlAttributesString += " " + d.Keys.ElementAt(i) + "=" + d.Values.ElementAt(i);
            //    }
            //}

            //What is Entity Framework??
            StringBuilder ancor = new StringBuilder();
            ancor.Append("<a ");
            //if (htmlAttributesString != string.Empty)
            //{
            //    ancor.Append(htmlAttributesString);
            //}
            if (htmlAttributes != null)
            {
                ancor.Append(htmlAttributes);
            }
            ancor.Append(" href='");
            if (controllerName != string.Empty)
            {
                ancor.Append("/" + controllerName);
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
            ancor.Append("'");
            ancor.Append(">");
            ancor.Append(linkText);
            ancor.Append("</a>");
            return MvcHtmlString.Create(ancor.ToString());
        }
        
        public static MvcHtmlString EncodedURLActionLink(this HtmlHelper htmlHelper, string actionName, string controllerName, object routeValues,String tipe)
        {
            string queryString = string.Empty;
            string htmlAttributesString = string.Empty;
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
                ancor.Append("/" + controllerName);
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

        private static string Encrypt(string plainText)
        {
            //string key = "jdsg432387#";
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