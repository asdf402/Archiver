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

        public bool RleCompress
        {
            get
            {
                return false;
            }
        }
    }
}