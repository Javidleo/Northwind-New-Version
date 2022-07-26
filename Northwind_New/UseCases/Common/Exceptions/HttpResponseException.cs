using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UseCases.Common.Exceptions
{
    public class HttpResponseException : Exception
    {
        public int Status { get; set; } = 500;
        public object Value { get; set; }

        public HttpResponseException(int status, object value)
        {
            Status = status;
            Value = value;
        }
    }
}
