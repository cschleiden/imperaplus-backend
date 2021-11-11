using ImperaPlus.Domain.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;

namespace ImperaPlus.Domain.Tests.Helper
{
    public class ExpectedDomainExceptionAttribute : ExpectedExceptionBaseAttribute
    {
        private readonly ErrorCode? errorCode;

        public ExpectedDomainExceptionAttribute()
        {
        }

        public ExpectedDomainExceptionAttribute(ErrorCode errorCode)
        {
            this.errorCode = errorCode;
        }

        protected override void Verify(Exception exception)
        {
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(exception, typeof(DomainException));

            var domainException = (DomainException)exception;

            if (errorCode.HasValue)
            {
                Assert.AreEqual(errorCode, domainException.ErrorCode,
                    string.Format(CultureInfo.InvariantCulture, "ErrorCode {0} expected, but received {1}", errorCode,
                        domainException.ErrorCode));
            }
        }
    }
}
