using System;

namespace mvHub
{
    public abstract class ImvException : Exception
    {
        private readonly string _message;

        protected ImvException()
        { Init(); }

        protected ImvException(string message)
            : base(message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            _message = message;
            Init();
        }

        protected ImvException(string message, Exception interException) : base(message, interException) { Init(); }

        public int SuggestedHttpCode { get; set; }
        protected string Comment { get; set; }
        private void Init()
        {
            SuggestedHttpCode = 500;
            Comment = "";
        }
    }

    [Serializable]
    public class DelimitSetException : ImvException
    {
        private readonly string _message;
        public DelimitSetException()
        { }
        public DelimitSetException(string message)
            : base(message)
        {
            _message = message;
        }

        public DelimitSetException(string message, Exception interException) : base(message, interException) { }
    }

    [Serializable]
    public class ConnectionFailedException : Exception
    {
        public ConnectionFailedException(string message, string connectionString)
            : base(message)
        {
            this.ConnectionString = connectionString;
        }

        public string ConnectionString { get; private set; }

    }

    [Serializable]
    public class SubroutineException : Exception
    {
        private readonly string _message;
        public SubroutineException()
        { }
        public SubroutineException(string message)
            : base(message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            _message = message;
        }

        public SubroutineException(string message, Exception interException) : base(message, interException) { }
    }

    [Serializable]
    public class HeaderParseException : ImvException
    {
        private readonly string _message;
        public HeaderParseException()
        { }

        public HeaderParseException(string message) : base(message) { }
        public HeaderParseException(string message, Exception interException)
            : base(message, interException)
        {
            _message = message;
        }
    }

    [Serializable]
    public class SessionException : ImvException
    {
        private readonly string _message;
        public SessionException()
        { }
        public SessionException(string message)
            : base(message)
        {
            _message = message;
        }

        public SessionException(string message, Exception interException)
            : base(message, interException)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            _message = message;
        }
    }

    [Serializable]
    public class RecordException : ImvException
    {
        private readonly string _message;
        public RecordException()
        { }
        public RecordException(string message)
            : base(message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            _message = message;
        }

        public RecordException(string message, Exception interException)
            : base(message, interException)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            _message = message;
        }
    }

    [Serializable]
    public class MvHubException : ImvException
    {
        public MvHubException()
        {
        }

        public MvHubException(string message)
        : base(message)
        {
        }

        public MvHubException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }

    [Serializable]
    public class MvHubDataConnectorException : ImvException
    {
        public MvHubDataConnectorException()
        {
        }

        public MvHubDataConnectorException(string message)
        : base(message)
        {
        }

        public MvHubDataConnectorException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }

    [Serializable]
    public class MvHubSubroutineException : ImvException
    {
        public MvHubSubroutineException()
        {
        }

        public MvHubSubroutineException(string message)
        : base(message)
        {
        }

        public MvHubSubroutineException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
