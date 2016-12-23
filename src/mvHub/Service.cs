using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace mvHub
{
    public class Service : IHttpHandler
    {
        static readonly string MvHubPath;
        static readonly ImvDataConnector DataConnector;

        static Service()
        {
            MvHubPath = WebConfigurationManager.AppSettings["mvHubPath"];
            if (MvHubPath.Length < 1) MvHubPath = "Service";
            var mvHubDataConnectorAssembly = WebConfigurationManager.AppSettings["mvHubDataConnectorAssembly"];
            if (string.IsNullOrEmpty(mvHubDataConnectorAssembly))
            {
                throw new MvHubDataConnectorException("mvHub Data Connector Assembly Not Defined");
            }
            mvHubDataConnectorAssembly = Path.Combine(HttpRuntime.AppDomainAppPath,
                       "bin\\" + mvHubDataConnectorAssembly);
            var assembly = Assembly.LoadFrom(mvHubDataConnectorAssembly);

            var mvHubDataConnectorClass = WebConfigurationManager.AppSettings["mvHubDataConnectorClass"];
            if (string.IsNullOrEmpty(mvHubDataConnectorClass))
            {
                throw new MvHubDataConnectorException("mvHub Data Connector Class Not Defined");
            }
            try
            {
                var type = assembly.GetType(mvHubDataConnectorClass);
                DataConnector = Activator.CreateInstance(type) as ImvDataConnector;
                if (DataConnector == null)
                {
                    throw new MvHubDataConnectorException("Null Connector");
                }
            }
            catch (Exception ex)
            {
                throw new MvHubDataConnectorException("mvHub Unable To Create Connector", ex);
            }

        }

        public void ProcessRequest(HttpContext context)
        {

            byte[] msg;
           
            try
            {
                var param = Helper.ParseSegments(MvHubPath, context);
                if (param.Length < 1)
                {
                    throw new MvHubSubroutineException("Must Supply Routine Name");
                }

                var hubRoutine = param[0];

                var requestHeader = ParseHeader(context);

                var session = DataConnector.GetSession(hubRoutine);
                session.RequestHeader = requestHeader.ToString();
                session.RequestBody = ParseBody(context, requestHeader);
                session.Call();
                session.Close();
                
                ApplyHeader(context,new Mvdom(session.ReplyHeader));



                msg = Encoding.UTF8.GetBytes(session.ReplyBody);


                if (msg.Length > 0)
                {
                    context.Response.OutputStream.Write(msg, 0, msg.Length);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "text/plain";
                    context.Response.AddHeader("Cache-Control", "no-cache");
                    msg = Encoding.UTF8.GetBytes(ex.Message + Environment.NewLine
                        + ex.StackTrace);
                    context.Response.OutputStream.Write(msg, 0, msg.Length);
                }
                catch (Exception)
                {
                    context.Response.Close();
                }

            }

        }

        private static void ApplyHeader(HttpContext context, Mvdom replyHeader)
        {
            context.Response.StatusCode = MvConversion.ToInt(replyHeader.Extract(1));
            context.Response.ContentType = replyHeader.Extract(3);
            var cnt = replyHeader.DCount(5);
            for (var pos = 1; pos <= cnt; pos++)
            {
                context.Response.AddHeader(replyHeader.Extract(5,pos), replyHeader.Extract(6, pos));
            }
                
        }
        private static Mvdom ParseHeader(HttpContext context)
        {
            var reqHeader = new Mvdom();
            var accessMethod = "GET";
            var urlReferrer = "";
            try
            {
                accessMethod = context.Request.HttpMethod.ToUpper();
                if (context.Request.UrlReferrer != null)
                {
                    urlReferrer = context.Request.UrlReferrer.AbsolutePath;
                }
            }
            catch (Exception)
            {
                urlReferrer = "";
            }
            var url = HttpUtility.UrlDecode(context.Request.Url.AbsolutePath);
            reqHeader[1] = url;
            reqHeader[2] = accessMethod;
            var contentType = context.Request.ContentType.ToLower();
            reqHeader[3] = contentType;


            reqHeader[4] = context.Request.ContentEncoding.EncodingName;
            reqHeader[5] = context.Request.FilePath;
            reqHeader[6] = context.Request.Path;
            reqHeader[7] = urlReferrer;

            reqHeader[10] = context.Request.IsSecureConnection.ToString();
            reqHeader[11] = context.Request.UserHostAddress;
            reqHeader[12] = context.Request.UserHostName;
            if (context.Request.UserAgent == null)
            {
                reqHeader[13] = "Not Provided";
            }
            else
            {
                reqHeader[13] = context.Request.UserAgent;
            }
            reqHeader[14] = context.Request.IsAuthenticated.ToString();



            if (!context.Request.RawUrl.Contains("?")) return reqHeader;
            var bufferarray =
                context.Request.RawUrl.Substring(context.Request.RawUrl.IndexOf("?", StringComparison.Ordinal) + 1)
                    .Split('&');

            var pos = 0;
            foreach (var seg in bufferarray)
            {
                var postTable = HttpUtility.ParseQueryString(seg);
                foreach (var qName in postTable.Cast<string>().Where(qName => qName != null))
                {
                    pos += 1;
                    reqHeader[31, pos] = qName;
                    reqHeader[32, pos] = postTable[qName];
                    reqHeader[33, pos] = "Q";
                    reqHeader[34, pos] = "text";
                }
            }

            try
            {
                pos = 0;
                foreach (Cookie cookie in context.Request.Cookies)
                {
                    pos += 1;
                    reqHeader[41, pos] = cookie.Name;
                    reqHeader[42, pos] = cookie.Domain;
                    reqHeader[43, pos] = cookie.Path;
                    reqHeader[44, pos] = HttpUtility.UrlDecode(cookie.Value);
                    reqHeader[45, pos] = cookie.TimeStamp.ToString(CultureInfo.InvariantCulture);
                }
            }
            catch
            {
                // ignored
            }

            return reqHeader;
        }
        private static string ParseBody(HttpContext context, DomSupport.MvNode header)
        {
            var requestBody = "";
            var action = header.Extract(2);
            switch (action)
            {
                case "GET":
                    break;
                case "DELETE":
                    break;
                case "POST":
                    var contentType = header.Extract(3).ToLower();
                    var postBuffer = Helper.GetPostBuffer(context);

                    switch (contentType)
                    {

                        case "text/csv":
                            requestBody = MvCsv.ToMvString(postBuffer);
                            break;
                        default:
                            requestBody = postBuffer;
                            break;
                    }

                    break;
            }
            return requestBody;

        }
        public bool IsReusable => true;
    }
}
