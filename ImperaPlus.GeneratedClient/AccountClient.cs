using ImperaPlus.DTO;
using ImperaPlus.DTO.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperaPlus.GeneratedClient
{
    public partial class AccountClient : ImperaHttpClient
    {
        public System.Threading.Tasks.Task<LoginResponseModel> LoginAsync(string grant_type, string username, string password, string scope, string refresh_token)
        {
            return LoginAsync(grant_type, username, password, scope, refresh_token, System.Threading.CancellationToken.None);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Success</returns>
        /// <exception cref="ImperaPlusException">A server side error occurred.</exception>
        public async System.Threading.Tasks.Task<LoginResponseModel> LoginAsync(string grant_type, string username, string password, string scope, string refresh_token, System.Threading.CancellationToken cancellationToken)
        {
            var urlBuilder_ = new System.Text.StringBuilder();
            urlBuilder_.Append(BaseUrl).Append("/api/Account/token");

            var client_ = await CreateHttpClientAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                using (var request_ = new System.Net.Http.HttpRequestMessage())
                {
                    PrepareRequest(client_, urlBuilder_);
                    var url_ = urlBuilder_.ToString();
                    PrepareRequest(client_, url_);

                    var content_ = new System.Net.Http.FormUrlEncodedContent(new []
                    {
                        new KeyValuePair<string, string>("grant_type", grant_type),
                        new KeyValuePair<string, string>("username", username),
                        new KeyValuePair<string, string>("password", password),
                        new KeyValuePair<string, string>("scope", scope),
                        new KeyValuePair<string, string>("refresh_token", refresh_token),
                    });
                    request_.Content = content_;
                    request_.Method = new System.Net.Http.HttpMethod("POST");
                    request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);
                    request_.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    try
                    {
                        var headers_ = System.Linq.Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                        foreach (var item_ in response_.Content.Headers)
                            headers_[item_.Key] = item_.Value;

                        ProcessResponse(client_, response_);

                        var status_ = ((int)response_.StatusCode).ToString();
                        if (status_ == "400")
                        {
                            var responseData_ = await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            var result_ = default(ErrorResponse);
                            try
                            {
                                result_ = Newtonsoft.Json.JsonConvert.DeserializeObject<ErrorResponse>(responseData_);
                            }
                            catch (System.Exception exception)
                            {
                                throw new ImperaPlusException("Could not deserialize the response body.", status_, responseData_, headers_, exception);
                            }
                            throw new ImperaPlusException<ErrorResponse>("Client Error", status_, responseData_, headers_, result_, null);
                        }
                        else
                        if (status_ == "200")
                        {
                            var responseData_ = await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            var result_ = default(LoginResponseModel);
                            try
                            {
                                result_ = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginResponseModel>(responseData_);
                                return result_;
                            }
                            catch (System.Exception exception)
                            {
                                throw new ImperaPlusException("Could not deserialize the response body.", status_, responseData_, headers_, exception);
                            }
                        }
                        else
                        if (status_ != "200" && status_ != "204")
                        {
                            var responseData_ = await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ImperaPlusException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", status_, responseData_, headers_, null);
                        }

                        return default(LoginResponseModel);
                    }
                    finally
                    {
                        if (response_ != null)
                            response_.Dispose();
                    }
                }
            }
            finally
            {
                if (client_ != null)
                    client_.Dispose();
            }
        }
    }
}
