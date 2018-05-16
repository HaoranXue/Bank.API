using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Bank.API.Modles;

namespace Bank.API
{
    public class BankAPI: Disposable
    {

        #region Constant
        private const string ClientID = "kodin-64yu";
        private const string ClientSecret = "dga1x3b62j5q7anzfqqrip";

        private const string HeaderUserAgent = "data/v1/";
        private const string Metadata = "me";
        private const string Identity = "info";
        private const string Offline_access = "refresh_token";
        private const string CustomersAccounts = "accounts";
        private const string Cards = "cards";
        private const string Transactions = "transactions";

        private const string BankAPIRequestType = "application/json";
        private const string BankAPIRefreshType = "REFRESH-TOKEN-HERE";

        private const string AuthLink = "https://auth.truelayer.com/?response_type=code&client_id=kodin-64yu&nonce=655915437&scope=info%20accounts%20balance%20transactions%20cards%20offline_access&redirect_uri=https://console.truelayer.com/redirect-page&enable_mock=true";
        
        private const string DataAPIEndPoint = "https://api.truelayer.com";

        private static readonly Encoding _encoding = Encoding.UTF8;

        #endregion

        #region Properties

        private readonly string _ClientID;
        private readonly string _ClientSecret;
        private readonly string _refresh;
        private readonly HttpClient _client;
        private readonly IDictionary<string, string> _tokenCache;

        #endregion

        #region Constructors 

        public BankAPI(string ID, IDictionary<string, string> tokenCache)
        {

            _ClientID = ClientID;
            _ClientSecret = ClientSecret;

            _client = new HttpClient();

            _tokenCache = tokenCache ?? new Dictionary<string, string>();

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(BankAPIRequestType));
            _client.DefaultRequestHeaders.UserAgent.Clear();
            _client.DefaultRequestHeaders.UserAgent.ParseAdd(HeaderUserAgent);

        }
        #endregion

        #region CreatURL

        public static string CreatMetaURL()
        {
            return string.Format("{0}/{1}{2}",DataAPIEndPoint,HeaderUserAgent,Metadata);
        }

        public static string CreatIdentityURL()
        {
            return string.Format("{0}/{1}{2}",DataAPIEndPoint,HeaderUserAgent,Identity);
        }

        public static string CreatAccountsURL()
        {
            return string.Format("{0}/{1}{2}",DataAPIEndPoint,HeaderUserAgent,CustomersAccounts);
        }

        public static string CreatAccountURL(string account_id)
        {
            return string.Format("{0}/{1}{2}/{3}",DataAPIEndPoint,HeaderUserAgent,CustomersAccounts,account_id);

        }

        public static string CreatAccountBalanceURL(string account_id)
        {
            return string.Format("{0}/{1}{2}/{3}/balance", DataAPIEndPoint, HeaderUserAgent,CustomersAccounts,account_id);
        }

        public static string CreatCardsTransactionsURL(string account_id)
        {
            return string.Format("{0}/{1}{2}/{3}/transactions", DataAPIEndPoint,HeaderUserAgent,Cards,account_id);
        }
        #endregion

        #region Authentication

        private async Task EnsureAccessToken()
        {
            if (!_tokenCache.TryGetValue(_refresh, out string accessToken))
            {
                var postData = string.Format("grant_type=refresh_token&refresh_token={0}", _refresh);
                var content = new StringContent(postData, _encoding, BankAPIRefreshType);
                var endPoint = DataAPIEndPoint + BankAPIRequestType;
                var authBytes = _encoding.GetBytes(string.Format("{0}:{1}", ClientID, ClientSecret));
                var basicToken = Convert.ToBase64String(authBytes);

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicToken);

                var response = await _client.PostAsync(endPoint, content);
                var jsonContent = await response.Content.ReadAsStringAsync();
                var accessTokens = JsonConvert.DeserializeObject<RefreshTokenModel>(jsonContent);

                accessToken = accessTokens.AccessToken;

                _tokenCache.Add(_refresh, accessToken);
            }

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        #endregion

        #region Disposable

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    _client.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        #endregion

        #region Helpers

        private async Task<T> GetObject<T>(string endPoint, NameValueCollection parameters = null)
        {
            var builder = new UriBuilder(endPoint);

            if (parameters != null)
            {
                var collection = HttpUtility.ParseQueryString(string.Empty);

                foreach (var key in parameters.AllKeys)
                {
                    collection.Add(key, parameters[key]);
                }

                builder.Query = collection.ToString();
            }

            return await SendObject<T>(HttpMethod.Get, endPoint + builder.Query.ToString(), endPoint);
        }

        private async Task<T> PutObject<T>(string endPoint, string name, T value)
        {
            var requestContent = CreateContent(name, value);

            return await SendObject<T>(HttpMethod.Put, endPoint, name, requestContent);
        }

        private async Task<T> PostObject<T>(string endPoint, string name, T value)
        {
            var requestContent = CreateContent(name, value);

            return await SendObject<T>(HttpMethod.Post, endPoint, name, requestContent);
        }

        private async Task<T> SendObject<T>(HttpMethod method, string endPoint, string responseWrapper, HttpContent requestContent = null)
        {
            await EnsureAccessToken();

            var requestMessage = new HttpRequestMessage(method, DataAPIEndPoint + endPoint)
            {
                Content = requestContent,
            };
            var responseMessage = await _client.SendAsync(requestMessage);
            var responseContent = await responseMessage.Content.ReadAsStringAsync();

            try
            {
                var responseObject = JsonConvert.DeserializeObject<IDictionary<string, T>>(responseContent);

                if (responseObject.TryGetValue(responseWrapper, out T value))
                {
                    return value;
                }

                return default(T);
            }
            catch (Exception ex)
            {
                var logger = DependencyResolver.Current.GetService<ILoggerService>();

                logger.Error<BankAPI>("Could not deserialise response from FreeAgent", ex);

                return default(T);
            }
        }

        private static HttpContent CreateContent(string name, object value)
        {
            var requestData = new Dictionary<string, object>()
            {
                { name, value },
            };

            var requestJson = JsonConvert.SerializeObject(requestData, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
            });

            return new StringContent(requestJson, _encoding, "application/json");
        }

        private static string GenerateGetUrl(string endPoint, int? id)
        {
            return string.Format("{0}/{1}", endPoint, id);
        }

        private static string GenerateUpdateUrl(string endPoint, AbstractModel model)
        {
            return string.Format("{0}/{1}", endPoint, model.GetID());
        }

        private static string GenerateCreateUrl(string endPoint)
        {
            return string.Format("{0}/create", endPoint);
        }

        #endregion

    }
}
