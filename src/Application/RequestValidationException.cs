namespace BlueBrown.BigBola.Application
{
    public class RequestValidationException : Exception
    {
        public RequestValidationException()
        {
        }

        public RequestValidationException(string message)
            : base(message)
        {
        }

        public RequestValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
