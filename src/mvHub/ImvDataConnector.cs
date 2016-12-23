using System;
using System.Diagnostics;
using System.Web.Configuration;

namespace mvHub
{
    public abstract class ImvDataConnector
    {
        #region Constructors

        protected ImvDataConnector()
        {
            Configuration = new HandlerConfiguration(this);
        }

        #endregion

        #region Public Methods

        public abstract MvSession GetSession(string subroutine = null,
            string usernameOverride = null,
            string passwordOverride = null);

        public HandlerConfiguration Configuration { get; protected set; }

        public string DefaultDatabase => Configuration.Database;
        public string DefaultAccount => Configuration.Account;
        public string DefaultUsername => Configuration.Username;
        public string DefaultPassword => Configuration.Password;
        public string DefaultAuthPath => Configuration.AuthPath;
        public string DefaultSubroutine => Configuration.Subroutine;

        #endregion
    }

    public abstract class MvSession : IDisposable
    {
        protected MvSession(ImvDataConnector assignParentConnector,
            string subroutineOverride,
            string usernameOverride,
            string passwordOverride)
        {
            if (assignParentConnector == null)
            {
                throw new ArgumentNullException(nameof(assignParentConnector));
            }
            ParentConnector = assignParentConnector;
            Hostname = assignParentConnector.Configuration.Database;
            Account = assignParentConnector.Configuration.Account;
            User = usernameOverride ?? assignParentConnector.Configuration.Username;
            Password = passwordOverride ?? assignParentConnector.Configuration.Password;
            Subroutine = subroutineOverride ?? assignParentConnector.Configuration.Subroutine;
            RequestHeader = "";
            RequestBody = "";
            ReplyHeader = "";
            ReplyBody = "";
        }

        public string ReplyBody { get; protected set; }

        public string ReplyHeader { get; protected set; }

        public string RequestBody { protected get; set; }

        public string RequestHeader { protected get; set; }

        public ImvDataConnector ParentConnector { get; protected set; }

        protected string Hostname { get; set; }
        protected string Account { get; set; }
        protected string User { get; set; }
        protected string Password { get; set; }
        protected string Subroutine { get; set; }
        public abstract void Dispose();


        public abstract void Close();
        public abstract bool Call();
    }

    public class HandlerConfiguration
    {
        public void ReloadConfiguration()
        {
            Account = WebConfigurationManager.AppSettings["DBAccount"];
            Database = WebConfigurationManager.AppSettings["DBHost"];
            Password = WebConfigurationManager.AppSettings["DBDefaultPassword"];
            Username = WebConfigurationManager.AppSettings["DBDefaultUser"];
            Subroutine = WebConfigurationManager.AppSettings["DBDefaultSubroutine"];
            AuthPath = WebConfigurationManager.AppSettings["mvHubAuth"];
            HubPath = WebConfigurationManager.AppSettings["mvHubPath"];
        }


        public static void Log(EventLogEntryType error, int errorNumber, string section, string errorMessage)
        {
            Console.WriteLine(error + ":" + errorNumber + "/" + section + ":" + errorMessage);
        }

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the GraphServerConfiguration class.
        /// </summary>
        public HandlerConfiguration(ImvDataConnector dataConnector)
        {
            DataConnector = dataConnector;
            ReloadConfiguration();
        }

        public string Database { get; set; }
        public string Account { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Subroutine { get; set; }
        public string AuthPath { get; set; }
        public string HubPath { get; set; }
        public ImvDataConnector DataConnector { get; private set; }

        #endregion
    }
}