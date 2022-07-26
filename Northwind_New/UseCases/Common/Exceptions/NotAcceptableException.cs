using System;

namespace UseCases.Common.Exceptions
{
    public class NotAcceptableException : Exception
    {
        public NotAcceptableException(string message) : base(message)
        {

        }
    }
}
