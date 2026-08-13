namespace Archiver
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

        public ICompress Compress
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

        public MainCommands MainCommand
        {
            get
            {
                return MainCommands.None;
            }
        }
    }
}