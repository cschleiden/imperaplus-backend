using System;

namespace ImperaPlus.Domain.Exceptions
{
    public class DomainException : Exception
    {
        private readonly ErrorCode errorCode;

        public DomainException(ErrorCode errorCode, string message)
            : base(message)
        {
            this.errorCode = errorCode;
        }

        public ErrorCode ErrorCode
        {
            get { return this.errorCode; }
        }
    }
}
