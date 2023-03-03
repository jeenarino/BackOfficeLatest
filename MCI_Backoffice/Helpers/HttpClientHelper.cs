using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System;

namespace CheckinPortal.BackOffice.Helpers
{

    
    public class HttpClientHelper
    {
        public readonly HttpClient client;
        public HttpClientHelper(string baseURL)
        {
            client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(10);
            client.BaseAddress = new System.Uri(baseURL);
        }

        public async Task<HttpResponseMessage> PushDataToCloud(object requstObject,string functionName)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
            {
                return true;
            };

            if (ConfigurationHelper.Instance.GrabberConfig.isProxyEnableForCloudAPI)
            {
                HttpClientHandler handler = new HttpClientHandler();
                handler.UseDefaultCredentials = true;
                var proxy = new WebProxy
                {
                    Address = new System.Uri(ConfigurationHelper.Instance.GrabberConfig.CloudAPIProxyHost),
                    BypassProxyOnLocal = false,
                    UseDefaultCredentials = false,

                    Credentials = new NetworkCredential(
                    userName: ConfigurationHelper.Instance.GrabberConfig.CloudAPIProxyUN,
                    password: ConfigurationHelper.Instance.GrabberConfig.CloudAPIProxyPswd)
                };

                var httpClientHandler = new HttpClientHandler
                {
                    Proxy = proxy,
                };
                var temp = client.BaseAddress;
                HttpClient proxyClient = new HttpClient(handler: httpClientHandler, disposeHandler: true);
                proxyClient.BaseAddress = temp;
                string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(requstObject);
                proxyClient.DefaultRequestHeaders.Clear();
                proxyClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpContent requestContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                proxyClient.Timeout = TimeSpan.FromMinutes(10);
                

                return await proxyClient.PostAsync($"{functionName}", requestContent);
            }
            else
            {
                string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(requstObject);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpContent requestContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
               
                return await client.PostAsync($"{functionName}", requestContent);
            }
        }

        public async Task<HttpResponseMessage> GetDataFromCloud(string functionName)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return await client.GetAsync($"{functionName}");
            
        }

        public async Task<HttpResponseMessage> PushDataToSmartTapCloud(object requstObject, string functionName)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
            {
                return true;
            };

            if (ConfigurationHelper.Instance.smartTapConfig.isProxyEnableForSmartTapCloudAPI)
            {
                HttpClientHandler handler = new HttpClientHandler();
                handler.UseDefaultCredentials = true;
                var proxy = new WebProxy
                {
                    Address = new System.Uri(ConfigurationHelper.Instance.smartTapConfig.SmartTapCloudAPIProxyHost),
                    BypassProxyOnLocal = false,
                    UseDefaultCredentials = false,

                    Credentials = new NetworkCredential(
                    userName: ConfigurationHelper.Instance.smartTapConfig.SmartTapCloudAPIProxyUN,
                    password: ConfigurationHelper.Instance.smartTapConfig.SmartTapCloudAPIProxyPswd)
                };

                var httpClientHandler = new HttpClientHandler
                {
                    Proxy = proxy,
                };
                var temp = client.BaseAddress;
                HttpClient proxyClient = new HttpClient(handler: httpClientHandler, disposeHandler: true);
                proxyClient.BaseAddress = temp;
                string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(requstObject);
                proxyClient.DefaultRequestHeaders.Clear();
                proxyClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpContent requestContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                proxyClient.Timeout = TimeSpan.FromMinutes(10);


                return await proxyClient.PostAsync($"{functionName}", requestContent);
            }
            else
            {
                string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(requstObject);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpContent requestContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                return await client.PostAsync($"{functionName}", requestContent);
            }
        }

    }
}