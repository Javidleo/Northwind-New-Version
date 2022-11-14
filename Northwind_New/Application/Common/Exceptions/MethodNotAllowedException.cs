namespace Application.Common.Exceptions
{
    public class MethodNotAllowedException : Exception
    {
        public MethodNotAllowedException(string message) : base(message)
        { }
    }
}
