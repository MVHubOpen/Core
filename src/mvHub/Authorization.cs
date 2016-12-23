using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Web;

namespace mvHub
{
    public class Authorization
    {
        public static HttpCredentials GetCredentials(HttpRequest request)
        {
            var authorizationHeader = request.Headers["Authorization"];
            var authMethod = "Basic";
            if (authorizationHeader == null) return new HttpCredentials(authMethod, null, null, null);

            if (!authorizationHeader.StartsWith("Basic"))
            {
                if (!authorizationHeader.StartsWith("DBUser")) return new HttpCredentials(authMethod, null, null, null);
                authMethod = "DBUser";
            }


            var encodedUsernamePassword =
                authorizationHeader.Substring(authorizationHeader.IndexOf(" ", StringComparison.Ordinal)).Trim();
            var encoding = Encoding.GetEncoding("iso-8859-1");
            var usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

            var separatorIndex = usernamePassword.IndexOf(':');

            var username = usernamePassword.Substring(0, separatorIndex);
            if (username == string.Empty)
            {
                username = null;
            }
            var password = usernamePassword.Substring(separatorIndex + 1);
            if (password == string.Empty)
            {
                password = null;
            }
            var sessionId = request.UserHostAddress != null
                ? Convert.ToBase64String(Encoding.ASCII.GetBytes(request.UserHostAddress))
                : "UNKNOWNLOCATION";

            return new HttpCredentials(authMethod, username, password, sessionId);
        }

        public class HttpCredentials
        {
            private readonly Timer _timer;

            public HttpCredentials(string authMethod, string username, string password, string sessionId,
                int timeoutMinutes = 1, Dictionary<string, HttpCredentials> cacheDictionary = null)
            {
                if (authMethod == null) authMethod = "Basic";
                AuthMethod = authMethod;

                if (username == null) username = "";
                Username = username;
                if (Username == string.Empty)
                {
                    Username = null;
                }
                Password = password;
                if (Password == string.Empty)
                {
                    Password = null;
                }

                SessionId = sessionId;
                TimeoutMinutes = timeoutMinutes;
                ParentCache = cacheDictionary;
                Expired = false;

                _timer = new Timer(timeoutMinutes*60*1000) {AutoReset = false};
                _timer.Elapsed += OnTimedEvent;
                _timer.Enabled = true;
                Expired = false;
            }

            public string Username { get; }
            public string Password { get; }
            public string SessionId { get; }
            public string AuthMethod { get; }
            public bool Expired { get; private set; }
            public double TimeoutMinutes { get; private set; }
            public Dictionary<string, HttpCredentials> ParentCache { get; set; }


            private void RemoveByValue()
            {
                if (ParentCache == null) return;
                var itemsToRemove = (from pair in ParentCache
                    where pair.Value != null && pair.Value == this
                    select pair.Key).ToList();

                foreach (var item in itemsToRemove)
                {
                    ParentCache.Remove(item);
                }
            }

            private void OnTimedEvent(object source, ElapsedEventArgs e)
            {
                Expired = true;
                _timer.Enabled = false;
                RemoveByValue();
            }

            public void Touch()
            {
                _timer.Stop();
                _timer.Start();
                Expired = false;
            }

            public string Hashkey(string prefix = "")
            {
                if (string.IsNullOrEmpty(Username)) return string.Empty;
                if (string.IsNullOrEmpty(Password)) return string.Empty;
                if (string.IsNullOrEmpty(SessionId)) return string.Empty;

                return Expired ? string.Empty : prefix + Helper.Md5Hash(prefix + Username + Password + SessionId);
            }
        }
    }
}