using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ImperaPlus.GeneratedClient
{
    public class ImperaHttpClient
    {
        public string AuthToken { get; set; }

        public HttpMessageHandler MessageHandler { get; set; }

        protected Task<HttpClient> CreateHttpClientAsync(CancellationToken cancellationToken)
        {
            HttpClient httpClient;

            if (this.MessageHandler != null)
            {
                httpClient = new HttpClient(this.MessageHandler);
            }
            else
            {
                httpClient = new HttpClient();
            }

            if (!string.IsNullOrEmpty(this.AuthToken))
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", this.AuthToken);
            }

            return Task.FromResult(httpClient);
        }
    }

    public static class ImperaClientFactory
    {
        public static TClientType GetClient<TClientType>(string baseUri, string authToken = null, HttpMessageHandler messageHandler = null) where TClientType : ImperaHttpClient
        {
            var client = (TClientType)Activator.CreateInstance(typeof(TClientType));

            var type = client.GetType();
            var propertyInfo = type.GetProperty("BaseUrl");
            propertyInfo.SetValue(client, baseUri);

            if (!string.IsNullOrEmpty(authToken))
            {
                client.AuthToken = authToken;
            }

            if (messageHandler != null)
            {
                client.MessageHandler = messageHandler;
            }

            return client;
        }
    }
}
