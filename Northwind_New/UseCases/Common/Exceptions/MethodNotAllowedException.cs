using System;

namespace UseCases.Common.Exceptions
{
    public class MethodNotAllowedException : Exception
    {
        public MethodNotAllowedException(string message) : base(message)
        { }
    }
}
