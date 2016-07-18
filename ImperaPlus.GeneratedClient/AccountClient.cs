using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ImperaPlus.GeneratedClient
{
    public partial class AccountClient
    {
        public async Task<string> LoginAsync(string userName, string password)
        {
            var url_ = string.Format("{0}/{1}", BaseUrl, "Token");


            var client_ = await CreateHttpClientAsync(CancellationToken.None).ConfigureAwait(false);
            PrepareRequest(client_, ref url_);

            var content_ = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", userName),
                new KeyValuePair<string, string>("password", password)
            });

            var response_ = await client_.PostAsync(url_, content_, CancellationToken.None).ConfigureAwait(false);
            ProcessResponse(client_, response_);

            var responseData_ = await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
            var status_ = ((int)response_.StatusCode).ToString();

            if (status_ == "200")
            {
                var ticket = JObject.Parse(responseData_);
                var token = ticket["access_token"].ToString();

                return token;
            }
            else
            {
            }

            throw new SwaggerException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", status_, null, null);
        }
    }
}
