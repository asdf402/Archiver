namespace Archiver.Commands
{
    public class CommandDefaults
    {
        public byte RetryAmount
        {
            get
            {
                return 1;
            }
        }

        public TimeSpan WaitTime
        {
            get
            {
                return new TimeSpan(hours: 0, minutes: 0, seconds: 1);
            }
        }

        public ICompression Compress
        {
            get
            {
                return new NoCompress();
            }
        }

        public string Path
        {
            get
            {
                return string.Empty;
            }
        }

        public ICommand MainCommand
        {
            get
            {
                return new None();
            }
        }
    }
}