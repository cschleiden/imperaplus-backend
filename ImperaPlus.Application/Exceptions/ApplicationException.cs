using System;

namespace ImperaPlus.Application.Exceptions
{
    public class ApplicationException : Exception
    {
        public ApplicationException(string message, ErrorCode errorCode)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        public ErrorCode ErrorCode { get; private set; }
    }
}
