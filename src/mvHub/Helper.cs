using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace mvHub
{
    public static class Helper
    {
        public static string Md5Hash(string input)
        {
            var md5Hash = MD5.Create();

            var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sBuilder = new StringBuilder();

            foreach (var t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }

            return sBuilder.ToString();
        }

        public static string RemoveWhitespace(string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !char.IsWhiteSpace(c))
                .ToArray());
        }

        public static void ExceptionDump(Exception ex, StringBuilder s)
        {
            if (ex.InnerException != null)
            {
                ExceptionDump(ex.InnerException, s);
            }

            s.AppendLine(ex.Message);
            s.AppendLine(ex.StackTrace);
        }

        public static string[] ParseSegments(string handlerName, HttpContext context)
        {
            var parm = new List<string>();

            var mode = 0;

            var segments = context.Request.Url.Segments;

            var cSeg = handlerName.ToLower();

            foreach (var seg in segments)
            {
                var oSeg = seg;
                if (oSeg.EndsWith("/"))
                {
                    oSeg = oSeg.Substring(0, oSeg.Length - 1);
                }
                var uSeg = oSeg.ToLower();

                switch (mode)
                {
                    case 0:
                        if (uSeg == cSeg) mode = 1;
                        break;
                    case 1:
                        parm.Add(oSeg);
                        break;
                }
            }
            return parm.ToArray();
        }

        public static string GetPostBuffer(HttpContext context)
        {
            string postBuffer;
            try
            {
                postBuffer = new StreamReader(context.Request.InputStream).ReadToEnd();
            }
            catch
            {
                postBuffer = "";
            }
            return postBuffer;
        }
    }
}