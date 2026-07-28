namespace Archiver
{
    public class ParsedArguments
    {
        public ParsedArguments(
            bool rleCompress,
            byte retryAmount,
            TimeSpan waitTime,
            string source,
            string destination,
            MainCommands mainCommand)
        {
            this.RleCompress = rleCompress;
            this.RetryAmount = retryAmount;
            this.WaitTime = waitTime;
            this.Source = source;
            this.Destination = destination;
            this.MainCommand = mainCommand;
        }

        public bool RleCompress
        {
            get;
            private set;
        }

        public byte RetryAmount
        {
            get;
            private set;
        }

        public TimeSpan WaitTime
        {
            get;
            private set;
        }

        public string Source
        {
            get;
            private set;
        }

        public string Destination
        {
            get;
            private set;
        }

        public MainCommands MainCommand
        {
            get;
            private set;
        }
    }
}