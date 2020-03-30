using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Http;

namespace ImperaPlus.IntegrationTests.Support
{
    public static class HttpResponseMessageExtensions
    {
        public static void AssertIsSuccessful(this HttpResponseMessage httpResponseMessage)
        {
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                var content = httpResponseMessage.Content.ReadAsStringAsync().Result;

                Assert.Fail("Request not successful: {0} {1}", httpResponseMessage.StatusCode, content);
            }
        }
    }
}
