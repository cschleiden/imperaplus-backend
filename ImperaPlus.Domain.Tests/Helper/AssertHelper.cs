using System;
using ImperaPlus.Domain.Exceptions;

namespace ImperaPlus.Domain.Tests.Helper
{
    public static class AssertHelper
    {
        public static void VerifyThrows<TException>(params Action[] actions)
        {
            foreach (var action in actions)
            {
                bool thrown = false;

                try
                {
                    action();
                }
                catch (Exception ex) when (ex is TException)
                {
                    thrown = true;
                }

                if (!thrown)
                {
                    throw new InvalidOperationException("Expected exception not raised");
                }
            }
        }

        public static void VerifyThrowsDomain(ErrorCode errorCode, params Action[] actions)
        {
            foreach (var action in actions)
            {
                bool thrown = false;

                try
                {
                    action();
                }
                catch (DomainException ex) when (ex.ErrorCode == errorCode)
                {
                    thrown = true;
                }

                if (!thrown)
                {
                    throw new InvalidOperationException("Expected exception not raised");
                }
            }
        }
    }
}
