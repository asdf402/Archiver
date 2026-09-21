namespace Archiver
{
    public class ParsedArguments
    {
        public ParsedArguments(
            ICompression compress,
            byte retryAmount,
            TimeSpan waitTime,
            string source,
            string destination,
            ICommand mainCommand)
        {
            this.Compress = compress;
            this.RetryAmount = retryAmount;
            this.WaitTime = waitTime;
            this.Source = source;
            this.Destination = destination;
            this.MainCommand = mainCommand;
        }

        public ICompression Compress
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

        public ICommand MainCommand
        {
            get;
            private set;
        }
    }
}