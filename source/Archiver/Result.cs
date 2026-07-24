namespace Archiver
{
    public delegate void ErrorMessage(string wrongArgument);

    public class Result
    {
        public Result(bool errorOccured, ErrorMessage errorMessage, string wrongArgument)
        {
            this.ErrorOccured = errorOccured;
            this.ErrorMessage = errorMessage;
            this.WrongArgument = wrongArgument;
        }

        public Result(bool errorOccured)
        {
            this.ErrorOccured = errorOccured;
        }

        public bool ErrorOccured
        {
            get;
            private set;
        }

        public ErrorMessage ErrorMessage
        {
            get;
            private set;
        }

        public string WrongArgument
        {
            get;
            private set;
        }
    }
}