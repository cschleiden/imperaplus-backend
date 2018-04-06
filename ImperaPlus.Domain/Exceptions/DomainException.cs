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

        public DomainException(ErrorCode errorCode, string format, params object[] args)
            : this(errorCode, string.Format(format, args))
        {
        }

        public ErrorCode ErrorCode
        {
            get { return this.errorCode; }
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", this.errorCode, this.Message);
        }
    }
}
