using CheckinPortal.BackOffice.Helpers;
using CheckinPortal.BackOffice.Models;
using CheckinPortal.BackOffice.Models.OwsModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CheckinPortal.BackOffice.BusinessLogic
{
    public enum EmailType
    {
        Precheckedin,
        PreCheckedout,
        CheckinConfirmation,
        GuestFolio,
        AcceptRequest,
        RejectRequest
    }

    public class LocalAPICallsHelper
    {
        public async Task<LocalResponseModel> Encodekey(Models.KeyEncodeRequestModel keyEncodeRequestModel)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigurationHelper.Instance.OWSConfig.LocalAPIURL);

                Models.LocalAPIRequestModel encodeKeyModel = new Models.LocalAPIRequestModel()
                {
                    IsFromCloud = false,
                    RequestObject = keyEncodeRequestModel
                };

                string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(encodeKeyModel);

                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpContent requestContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                httpClient.Timeout = TimeSpan.FromMinutes(10);

                HttpResponseMessage response = await httpClient.PostAsync($"local/EncodeKey", requestContent);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    var FetchRoomResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<LocalResponseModel>(responseString);
                    return FetchRoomResponse;
                }
                else
                {
                    string FailureResponse = await response.Content.ReadAsStringAsync();
                    return new LocalResponseModel()
                    {
                        responseMessage = FailureResponse.Substring(0, Math.Min(FailureResponse.Length, 25))
                    };
                }
            }
        }

        public async Task<LocalResponseModel> SendEmail(SendEmailRequestModel sendEmailRequestModel)
        {
            //File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Send Email started" });
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
            {
                return true;
            };
            if (ConfigurationHelper.Instance.GrabberConfig.isProxyEnableForEmailAPI)
            {
                HttpClientHandler handler = new HttpClientHandler();
                handler.UseDefaultCredentials = true;
                var proxy = new WebProxy
                {
                    Address = new Uri(ConfigurationHelper.Instance.GrabberConfig.EmailAPIProxyHost),
                    BypassProxyOnLocal = false,
                    UseDefaultCredentials = false,

                    Credentials = new NetworkCredential(
                    userName: ConfigurationHelper.Instance.GrabberConfig.EmailAPIProxyUN,
                    password: ConfigurationHelper.Instance.GrabberConfig.EmailAPIProxyPswd)
                };

                var httpClientHandler = new HttpClientHandler
                {
                    Proxy = proxy,
                };


                using (var httpClient = new HttpClient(handler: httpClientHandler, disposeHandler: true))
                {

                    httpClient.BaseAddress = new Uri(ConfigurationHelper.Instance.OWSConfig.EmailAPIURL);

                    //LocalAPIRequestModel encodeKeyModel = new LocalAPIRequestModel()
                    //{
                    //    IsFromCloud = false,
                    //    RequestObject =  sendEmailRequestModel
                    //};

                    string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(sendEmailRequestModel);

                    File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Send Email Json : {jsonString}" });

                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpContent requestContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    httpClient.Timeout = TimeSpan.FromMinutes(10);
                   

                    HttpResponseMessage response = await httpClient.PostAsync(ConfigurationHelper.Instance.OWSConfig.EmailAPIURL + $"SendEmail", requestContent);

                    if (response.IsSuccessStatusCode)
                    {
                        File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Send Email Success" });

                        string responseString = await response.Content.ReadAsStringAsync();
                        var sendEmailResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<LocalResponseModel>(responseString);
                        return sendEmailResponse;
                    }
                    else
                    {
                        string FailureResponse = await response.Content.ReadAsStringAsync();
                        File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Send Email Failure  {FailureResponse}" });
                        return new LocalResponseModel()
                        {
                            responseMessage = FailureResponse.Substring(0, Math.Min(FailureResponse.Length, 25))
                        };
                    }
                }
            }
            else
            {
                using (var httpClient = new HttpClient())
                {

                    httpClient.BaseAddress = new Uri(ConfigurationHelper.Instance.OWSConfig.EmailAPIURL);

                    //LocalAPIRequestModel encodeKeyModel = new LocalAPIRequestModel()
                    //{
                    //    IsFromCloud = false,
                    //    RequestObject =  sendEmailRequestModel
                    //};

                    string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(sendEmailRequestModel);

                    File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Send Email Json : {jsonString}" });

                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpContent requestContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    httpClient.Timeout = TimeSpan.FromMinutes(10);

                    HttpResponseMessage response = await httpClient.PostAsync(ConfigurationHelper.Instance.OWSConfig.EmailAPIURL + $"SendEmail", requestContent);

                    if (response.IsSuccessStatusCode)
                    {
                        File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Send Email Success" });

                        string responseString = await response.Content.ReadAsStringAsync();
                        var sendEmailResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<LocalResponseModel>(responseString);
                        return sendEmailResponse;
                    }
                    else
                    {
                        string FailureResponse = await response.Content.ReadAsStringAsync();
                        File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Send Email Failure  {FailureResponse}" });
                        return new LocalResponseModel()
                        {
                            responseMessage = FailureResponse.Substring(0, Math.Min(FailureResponse.Length, 25))
                        };
                    }
                }
            }
        }

        public async Task<LocalResponseModel> PushReservationDetails(OwsFetchReservationResponseModel RequestObject)
        {
            //File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Pushing Reservation Details." });

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigurationHelper.Instance.OWSConfig.LocalAPIURL);


                RequestObject.responseData[0].GuestProfiles = null;
                Models.LocalAPIRequestModel  pushReservationRequest = new LocalAPIRequestModel()
                {
                    RequestObject = RequestObject.responseData
                };

                string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(pushReservationRequest);



                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpContent requestContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                httpClient.Timeout = TimeSpan.FromMinutes(10);

                HttpResponseMessage response = await httpClient.PostAsync(ConfigurationHelper.Instance.OWSConfig.LocalAPIURL + "/local/PushReservationDetails", requestContent);
                
                string responseString = await response.Content.ReadAsStringAsync();

                File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Push Reservation Response: {responseString}" });

                if (response.IsSuccessStatusCode)
                {
                    
                    File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Success Response from server {responseString}" });
                    var pushReservationResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<LocalResponseModel>(responseString);
                    return pushReservationResponse;
                }
                else
                {
                    string FailureResponse = await response.Content.ReadAsStringAsync();
                    File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Failure Response from server {FailureResponse}" });

                    return new LocalResponseModel()
                    {
                         result = false,
                        responseMessage = FailureResponse.Substring(0, Math.Min(FailureResponse.Length, 25))
                    };
                }
            }
        }

        public async Task<LocalResponseModel> SendPrecheckinLink(string  ConfirmationNo)
        {
            //File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Sending precheckin link for confirmation no : {ConfirmationNo}" });
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigurationHelper.Instance.OWSConfig.LocalAPIURL);
                LocalAPIRequestModel sendPrecheckinRequest = new LocalAPIRequestModel()
                {
                    RequestObject = ConfirmationNo
                };
                string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(sendPrecheckinRequest);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpContent requestContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                httpClient.Timeout = TimeSpan.FromMinutes(10);

                HttpResponseMessage response = await httpClient.PostAsync($"local/SendPreCheckin", requestContent);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    var pushReservationResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<LocalResponseModel>(responseString);
                    return pushReservationResponse;
                }
                else
                {
                    string FailureResponse = await response.Content.ReadAsStringAsync();
                    //File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Send Precheckin link failed with response: {FailureResponse}" });
                    return new LocalResponseModel()
                    {
                        result = false,
                        responseMessage = FailureResponse.Substring(0, Math.Min(FailureResponse.Length, 25))
                    };
                }
            }
        }

        public async Task<LocalResponseModel> FetchPreCheckedinReservationByReservationNumber(string  ConfirmationNo)
        {
            //File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Fetch Precheckin" });
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigurationHelper.Instance.OWSConfig.LocalAPIURL);
                try
                {
                    var sendPrecheckinRequest = new
                    {
                        RequestObject = new
                        {
                            ReservationNumber = ConfirmationNo,
                            isForceFetch = true,
                            ServiceParameters = new
                            {
                                isProxyEnableForCloudAPI = ConfigurationHelper.Instance.GrabberConfig.isProxyEnableForCloudAPI,// true,
                                cloudAPIProxyHost = ConfigurationHelper.Instance.GrabberConfig.CloudAPIProxyHost,// "http://rtputm01.myfairmont.com:8080",
                                cloudAPIProxyUN = ConfigurationHelper.Instance.GrabberConfig.CloudAPIProxyUN,// "CPH\\_rtpfps",
                                cloudAPIProxyPswd = ConfigurationHelper.Instance.GrabberConfig.CloudAPIProxyPswd,// "IT$upp0rt",
                                cloudAPIURL = ConfigurationHelper.Instance.GrabberConfig.CloudAPIURL,// "https://fairmontsingapore.h-butler.com/WebCheckin/api",
                                isProxyEnableForLocalAPI = ConfigurationHelper.Instance.GrabberConfig.isProxyEnableForLocalAPI,// false,
                                localAPIProxyHost = ConfigurationHelper.Instance.GrabberConfig.LocalAPIProxyHost,// null,
                                localAPIProxyUN = ConfigurationHelper.Instance.GrabberConfig.LocalAPIProxyUN,// null,
                                localAPIProxyPswd = ConfigurationHelper.Instance.GrabberConfig.LocalAPIProxyPswd,// null,
                                localAPIURL = ConfigurationHelper.Instance.GrabberConfig.LocalAPIURL,// "http://localhost:8081/api",
                                isProxyEnableForEmailAPI = ConfigurationHelper.Instance.GrabberConfig.isProxyEnableForEmailAPI,// false,
                                emailAPIProxyHost = ConfigurationHelper.Instance.GrabberConfig.EmailAPIProxyHost,// null,
                                emailAPIProxyUN = ConfigurationHelper.Instance.GrabberConfig.EmailAPIProxyUN,// null,
                                emailAPIProxyPswd = ConfigurationHelper.Instance.GrabberConfig.EmailAPIProxyPswd,// null,
                                emailURL = ConfigurationHelper.Instance.GrabberConfig.EmailURL,// "http://10.193.19.29:8087/api",
                                preArrivalConfirmationEmail = ConfigurationHelper.Instance.GrabberConfig.PreArrivalConfirmationEmail,// null,
                                preArrivalConfirmationEmailSubject = ConfigurationHelper.Instance.GrabberConfig.PreArrivalConfirmationEmailSubject,// null,
                                emailDisplayName = ConfigurationHelper.Instance.GrabberConfig.EmailDisplayName,// "Fairmont Singapore",
                                chainCode = ConfigurationHelper.Instance.OWSConfig.chainCode,// "CHA",
                                destinationEntityID = ConfigurationHelper.Instance.OWSConfig.destinationEntityID,// "TI",
                                hotelDomain = ConfigurationHelper.Instance.OWSConfig.hotelDomain,// "RTP",
                                kioskID = ConfigurationHelper.Instance.OWSConfig.kioskID,// "KIOSK",
                                language = ConfigurationHelper.Instance.OWSConfig.language,// "EN",
                                legnumber = ConfigurationHelper.Instance.OWSConfig.legNumber,// "1",
                                password = ConfigurationHelper.Instance.OWSConfig.password,// "$$$KIOSK$$",
                                systemType = ConfigurationHelper.Instance.OWSConfig.systemType,// "KIOSK",
                                username = ConfigurationHelper.Instance.OWSConfig.username,//"KIOSK",
                                clientID = "BO",
                                preAuthUDF = ConfigurationHelper.Instance.OWSConfig.preAuthUDF,// "Appr_Code",
                                preAuthAmntUDF = ConfigurationHelper.Instance.OWSConfig.preAuthAmntUDF,// "Appr_Code"
                                GarunteeTypeCode= ConfigurationHelper.Instance.OWSConfig.GarunteeTypeCode,// "Appr_Code"
                                DueInBufferDays = ConfigurationHelper.Instance.OWSConfig.DueInBufferDays,
                                IsBreakFastValidationWithUDF = ConfigurationHelper.Instance.OWSConfig.IsBreakFastValidationWithUDF,
                                IsBreakFastValidationWithPackage = ConfigurationHelper.Instance.OWSConfig.IsBreakFastValidationWithPackage,
                                PackageCodes = ConfigurationHelper.Instance.OWSConfig.PackageCodes,
                            }
                        },

                    };


                    string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(sendPrecheckinRequest);

                    //File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Fetch Precheckin Request : {jsonString}" });
                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpContent requestContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    httpClient.Timeout = TimeSpan.FromMinutes(10);
                    

                    HttpResponseMessage response = await httpClient.PostAsync($"{ConfigurationHelper.Instance.OWSConfig.LocalAPIURL}/LocalService/FetchPreCheckedInReservation", requestContent);



                    //File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Fetch Precheckin Client Address {response.RequestMessage.RequestUri.ToString()}" });
                    if (response.IsSuccessStatusCode)
                    {
                        File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Fetch Precheckin Success" });
                        string responseString = await response.Content.ReadAsStringAsync();
                        var pushReservationResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<LocalResponseModel>(responseString);
                        return pushReservationResponse;
                    }
                    else
                    {
                        string FailureResponse = await response.Content.ReadAsStringAsync();
                        //File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Fetch Precheckin FailureResponse {FailureResponse}" });
                        // File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Send Precheckin link failed with response: {FailureResponse}" });
                        return new LocalResponseModel()
                        {
                            result = false,
                            responseMessage = FailureResponse.Substring(0, Math.Min(FailureResponse.Length, 25))
                        };
                    }
                }
                catch (Exception ex)
                {

                    
                    //File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Fetch Precheckin Exception {ex}" });
                    return new LocalResponseModel()
                    {
                        result = false,
                        responseMessage = ex.Message
                    };
                }
            }
        }

        public async Task<LocalResponseModel> FetchPreCheckedoutReservationByReservationNumber(string ConfirmationNo)
        {
            //File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Fetch Precheckout" });
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigurationHelper.Instance.OWSConfig.LocalAPIURL);
                try
                {
                    var sendPrecheckinRequest = new
                    {
                        RequestObject = new
                        {
                            ReservationNumber = ConfirmationNo,
                            isForceFetch = true,
                            ServiceParameters = new
                            {
                                isProxyEnableForCloudAPI = ConfigurationHelper.Instance.GrabberConfig.isProxyEnableForCloudAPI,// true,
                                cloudAPIProxyHost = ConfigurationHelper.Instance.GrabberConfig.CloudAPIProxyHost,// "http://rtputm01.myfairmont.com:8080",
                                cloudAPIProxyUN = ConfigurationHelper.Instance.GrabberConfig.CloudAPIProxyUN,// "CPH\\_rtpfps",
                                cloudAPIProxyPswd = ConfigurationHelper.Instance.GrabberConfig.CloudAPIProxyPswd,// "IT$upp0rt",
                                cloudAPIURL = ConfigurationHelper.Instance.GrabberConfig.CloudAPIURL,// "https://fairmontsingapore.h-butler.com/WebCheckin/api",
                                isProxyEnableForLocalAPI = ConfigurationHelper.Instance.GrabberConfig.isProxyEnableForLocalAPI,// false,
                                localAPIProxyHost = ConfigurationHelper.Instance.GrabberConfig.LocalAPIProxyHost,// null,
                                localAPIProxyUN = ConfigurationHelper.Instance.GrabberConfig.LocalAPIProxyUN,// null,
                                localAPIProxyPswd = ConfigurationHelper.Instance.GrabberConfig.LocalAPIProxyPswd,// null,
                                localAPIURL = ConfigurationHelper.Instance.GrabberConfig.LocalAPIURL,// "http://localhost:8081/api",
                                isProxyEnableForEmailAPI = ConfigurationHelper.Instance.GrabberConfig.isProxyEnableForEmailAPI,// false,
                                emailAPIProxyHost = ConfigurationHelper.Instance.GrabberConfig.EmailAPIProxyHost,// null,
                                emailAPIProxyUN = ConfigurationHelper.Instance.GrabberConfig.EmailAPIProxyUN,// null,
                                emailAPIProxyPswd = ConfigurationHelper.Instance.GrabberConfig.EmailAPIProxyPswd,// null,
                                emailURL = ConfigurationHelper.Instance.GrabberConfig.EmailURL,// "http://10.193.19.29:8087/api",
                                preArrivalConfirmationEmail = ConfigurationHelper.Instance.GrabberConfig.PreArrivalConfirmationEmail,// null,
                                preArrivalConfirmationEmailSubject = ConfigurationHelper.Instance.GrabberConfig.PreArrivalConfirmationEmailSubject,// null,
                                emailDisplayName = ConfigurationHelper.Instance.GrabberConfig.EmailDisplayName,// "Fairmont Singapore",
                                chainCode = ConfigurationHelper.Instance.OWSConfig.chainCode,// "CHA",
                                destinationEntityID = ConfigurationHelper.Instance.OWSConfig.destinationEntityID,// "TI",
                                hotelDomain = ConfigurationHelper.Instance.OWSConfig.hotelDomain,// "RTP",
                                kioskID = ConfigurationHelper.Instance.OWSConfig.kioskID,// "KIOSK",
                                language = ConfigurationHelper.Instance.OWSConfig.language,// "EN",
                                legnumber = ConfigurationHelper.Instance.OWSConfig.legNumber,// "1",
                                password = ConfigurationHelper.Instance.OWSConfig.password,// "$$$KIOSK$$",
                                systemType = ConfigurationHelper.Instance.OWSConfig.systemType,// "KIOSK",
                                username = ConfigurationHelper.Instance.OWSConfig.username,//"KIOSK",
                                clientID = "BO",
                                preAuthUDF = ConfigurationHelper.Instance.OWSConfig.preAuthUDF,// "Appr_Code",
                                preAuthAmntUDF = ConfigurationHelper.Instance.OWSConfig.preAuthAmntUDF,// "Appr_Code"
                                GarunteeTypeCode = ConfigurationHelper.Instance.OWSConfig.GarunteeTypeCode,// "Appr_Code"
                                DueInBufferDays = ConfigurationHelper.Instance.OWSConfig.DueInBufferDays,
                                IsBreakFastValidationWithUDF = ConfigurationHelper.Instance.OWSConfig.IsBreakFastValidationWithUDF,
                                IsBreakFastValidationWithPackage = ConfigurationHelper.Instance.OWSConfig.IsBreakFastValidationWithPackage,
                                PackageCodes = ConfigurationHelper.Instance.OWSConfig.PackageCodes,
                                PreCheckoutFolioEmail = ConfigurationHelper.Instance.GrabberConfig.PreCheckoutFolioEmail,
                                PreCheckoutFolioEmailSubject = ConfigurationHelper.Instance.GrabberConfig.PreCheckoutFolioEmailSubject,
                                isAutoCheckOutEnabled = ConfigurationHelper.Instance.GrabberConfig.isAutoCheckOutEnabled,
                                sendFolioFromOpera = ConfigurationHelper.Instance.GrabberConfig.sendFolioFromOpera
                            }
                        },

                    };


                    string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(sendPrecheckinRequest);

                    //File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Fetch Precheckin Request : {jsonString}" });
                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpContent requestContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    httpClient.Timeout = TimeSpan.FromMinutes(10);

                    HttpResponseMessage response = await httpClient.PostAsync($"{ConfigurationHelper.Instance.OWSConfig.LocalAPIURL}/LocalService/FetchPreCheckedOutReservation", requestContent);



                    //File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Fetch Precheckin Client Address {response.RequestMessage.RequestUri.ToString()}" });
                    if (response.IsSuccessStatusCode)
                    {
                        File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Fetch Precheckout Success" });
                        string responseString = await response.Content.ReadAsStringAsync();
                        var pushReservationResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<LocalResponseModel>(responseString);
                        return pushReservationResponse;
                    }
                    else
                    {
                        string FailureResponse = await response.Content.ReadAsStringAsync();
                        //File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Fetch Precheckin FailureResponse {FailureResponse}" });
                        // File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Send Precheckin link failed with response: {FailureResponse}" });
                        return new LocalResponseModel()
                        {
                            result = false,
                            responseMessage = FailureResponse.Substring(0, Math.Min(FailureResponse.Length, 25))
                        };
                    }
                }
                catch (Exception ex)
                {


                    //File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Fetch Precheckin Exception {ex}" });
                    return new LocalResponseModel()
                    {
                        result = false,
                        responseMessage = ex.Message
                    };
                }
            }
        }

        public async Task<LocalResponseModel> PushDueOutReservation(string ConfirmationNo)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigurationHelper.Instance.OWSConfig.LocalAPIURL);
                try
                {
                    var sendPrecheckinRequest = new
                    {
                        RequestObject = new
                        {
                            ReservationNumber = ConfirmationNo,
                            isForceFetch = true,
                            ServiceParameters = new
                            {
                                isProxyEnableForCloudAPI = ConfigurationHelper.Instance.GrabberConfig.isProxyEnableForCloudAPI,// true,
                                cloudAPIProxyHost = ConfigurationHelper.Instance.GrabberConfig.CloudAPIProxyHost,// "http://rtputm01.myfairmont.com:8080",
                                cloudAPIProxyUN = ConfigurationHelper.Instance.GrabberConfig.CloudAPIProxyUN,// "CPH\\_rtpfps",
                                cloudAPIProxyPswd = ConfigurationHelper.Instance.GrabberConfig.CloudAPIProxyPswd,// "IT$upp0rt",
                                cloudAPIURL = ConfigurationHelper.Instance.GrabberConfig.CloudAPIURL,// "https://fairmontsingapore.h-butler.com/WebCheckin/api",
                                isProxyEnableForLocalAPI = ConfigurationHelper.Instance.GrabberConfig.isProxyEnableForLocalAPI,// false,
                                localAPIProxyHost = ConfigurationHelper.Instance.GrabberConfig.LocalAPIProxyHost,// null,
                                localAPIProxyUN = ConfigurationHelper.Instance.GrabberConfig.LocalAPIProxyUN,// null,
                                localAPIProxyPswd = ConfigurationHelper.Instance.GrabberConfig.LocalAPIProxyPswd,// null,
                                localAPIURL = ConfigurationHelper.Instance.GrabberConfig.LocalAPIURL,// "http://localhost:8081/api",
                                isProxyEnableForEmailAPI = ConfigurationHelper.Instance.GrabberConfig.isProxyEnableForEmailAPI,// false,
                                emailAPIProxyHost = ConfigurationHelper.Instance.GrabberConfig.EmailAPIProxyHost,// null,
                                emailAPIProxyUN = ConfigurationHelper.Instance.GrabberConfig.EmailAPIProxyUN,// null,
                                emailAPIProxyPswd = ConfigurationHelper.Instance.GrabberConfig.EmailAPIProxyPswd,// null,
                                emailURL = ConfigurationHelper.Instance.GrabberConfig.EmailURL,// "http://10.193.19.29:8087/api",
                                preArrivalConfirmationEmail = ConfigurationHelper.Instance.GrabberConfig.PreCheckoutFromEmail,// null,
                                preArrivalConfirmationEmailSubject = ConfigurationHelper.Instance.GrabberConfig.PreCheckoutEmailSubject,// null,

                                PreCheckoutFromEmail = ConfigurationHelper.Instance.GrabberConfig.PreCheckoutFromEmail,// null,
                                PreCheckoutEmailSubject = ConfigurationHelper.Instance.GrabberConfig.PreCheckoutEmailSubject,// null,

                                emailDisplayName = ConfigurationHelper.Instance.GrabberConfig.EmailDisplayName,// "Fairmont Singapore",
                                chainCode = ConfigurationHelper.Instance.OWSConfig.chainCode,// "CHA",
                                destinationEntityID = ConfigurationHelper.Instance.OWSConfig.destinationEntityID,// "TI",
                                hotelDomain = ConfigurationHelper.Instance.OWSConfig.hotelDomain,// "RTP",
                                kioskID = ConfigurationHelper.Instance.OWSConfig.kioskID,// "KIOSK",
                                language = ConfigurationHelper.Instance.OWSConfig.language,// "EN",
                                legnumber = ConfigurationHelper.Instance.OWSConfig.legNumber,// "1",
                                password = ConfigurationHelper.Instance.OWSConfig.password,// "$$$KIOSK$$",
                                systemType = ConfigurationHelper.Instance.OWSConfig.systemType,// "KIOSK",
                                username = ConfigurationHelper.Instance.OWSConfig.username,//"KIOSK",
                                clientID = "BO",
                                preAuthUDF = ConfigurationHelper.Instance.OWSConfig.preAuthUDF,// "Appr_Code",
                                preAuthAmntUDF = ConfigurationHelper.Instance.OWSConfig.preAuthAmntUDF,// "Appr_Code"
                                GarunteeTypeCode = ConfigurationHelper.Instance.OWSConfig.GarunteeTypeCode,
                                DueInBufferDays = ConfigurationHelper.Instance.OWSConfig.DueInBufferDays,
                                IsBreakFastValidationWithUDF = ConfigurationHelper.Instance.OWSConfig.IsBreakFastValidationWithUDF,
                                IsBreakFastValidationWithPackage = ConfigurationHelper.Instance.OWSConfig.IsBreakFastValidationWithPackage,
                                PackageCodes = ConfigurationHelper.Instance.OWSConfig.PackageCodes,
                                MealPlanFieldName = ConfigurationHelper.Instance.OWSConfig.MealPlanFieldName,
                            }
                        },

                    };


                    string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(sendPrecheckinRequest);

                    File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\Push_Due-out_log.txt"), new string[] { $"{DateTime.Now.ToString()} Request Json : {jsonString}" });

                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpContent requestContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    httpClient.Timeout = TimeSpan.FromMinutes(10);

                    HttpResponseMessage response = await httpClient.PostAsync($"{ConfigurationHelper.Instance.OWSConfig.LocalAPIURL}/LocalService/PushDueOutReservation", requestContent);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = await response.Content.ReadAsStringAsync();

                        File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\Push_Due-out_log.txt"), new string[] { $"{DateTime.Now.ToString()} Response Json : {responseString}" });


                        var pushReservationResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<LocalResponseModel>(responseString);
                        return pushReservationResponse;
                    }
                    else
                    {
                        string FailureResponse = await response.Content.ReadAsStringAsync();

                        File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\Push_Due-out_log.txt"), new string[] { $"{DateTime.Now.ToString()} Response Json : {FailureResponse}" });

                        return new LocalResponseModel()
                        {
                            result = false,
                            responseMessage = FailureResponse.Substring(0, Math.Min(FailureResponse.Length, 25))
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new LocalResponseModel()
                    {
                        result = false,
                        responseMessage = ex.Message
                    };
                }
            }
        }

        public async Task<LocalResponseModel> PushDueIntReservation(string ConfirmationNo)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigurationHelper.Instance.OWSConfig.LocalAPIURL);
                try
                {
                    var sendPrecheckinRequest = new
                    {
                        RequestObject = new
                        {
                            ReservationNumber = ConfirmationNo,
                            isForceFetch = true,
                            ServiceParameters = new
                            {
                                isProxyEnableForCloudAPI = ConfigurationHelper.Instance.GrabberConfig.isProxyEnableForCloudAPI,// true,
                                cloudAPIProxyHost = ConfigurationHelper.Instance.GrabberConfig.CloudAPIProxyHost,// "http://rtputm01.myfairmont.com:8080",
                                cloudAPIProxyUN = ConfigurationHelper.Instance.GrabberConfig.CloudAPIProxyUN,// "CPH\\_rtpfps",
                                cloudAPIProxyPswd = ConfigurationHelper.Instance.GrabberConfig.CloudAPIProxyPswd,// "IT$upp0rt",
                                cloudAPIURL = ConfigurationHelper.Instance.GrabberConfig.CloudAPIURL,// "https://fairmontsingapore.h-butler.com/WebCheckin/api",
                                isProxyEnableForLocalAPI = ConfigurationHelper.Instance.GrabberConfig.isProxyEnableForLocalAPI,// false,
                                localAPIProxyHost = ConfigurationHelper.Instance.GrabberConfig.LocalAPIProxyHost,// null,
                                localAPIProxyUN = ConfigurationHelper.Instance.GrabberConfig.LocalAPIProxyUN,// null,
                                localAPIProxyPswd = ConfigurationHelper.Instance.GrabberConfig.LocalAPIProxyPswd,// null,
                                localAPIURL = ConfigurationHelper.Instance.GrabberConfig.LocalAPIURL,// "http://localhost:8081/api",
                                isProxyEnableForEmailAPI = ConfigurationHelper.Instance.GrabberConfig.isProxyEnableForEmailAPI,// false,
                                emailAPIProxyHost = ConfigurationHelper.Instance.GrabberConfig.EmailAPIProxyHost,// null,
                                emailAPIProxyUN = ConfigurationHelper.Instance.GrabberConfig.EmailAPIProxyUN,// null,
                                emailAPIProxyPswd = ConfigurationHelper.Instance.GrabberConfig.EmailAPIProxyPswd,// null,
                                emailURL = ConfigurationHelper.Instance.GrabberConfig.EmailURL,// "http://10.193.19.29:8087/api",

                                preArrivalConfirmationEmail = ConfigurationHelper.Instance.GrabberConfig.PreArrivalFromEmail,// null,
                                preArrivalConfirmationEmailSubject = ConfigurationHelper.Instance.GrabberConfig.PreArrivalEmailSubject,// null,

                                PreArrivalFromEmail = ConfigurationHelper.Instance.GrabberConfig.PreArrivalFromEmail,// null,
                                PreArrivalEmailSubject = ConfigurationHelper.Instance.GrabberConfig.PreArrivalEmailSubject,// null,

                                emailDisplayName = ConfigurationHelper.Instance.GrabberConfig.EmailDisplayName,// "Fairmont Singapore",
                                chainCode = ConfigurationHelper.Instance.OWSConfig.chainCode,// "CHA",
                                destinationEntityID = ConfigurationHelper.Instance.OWSConfig.destinationEntityID,// "TI",
                                hotelDomain = ConfigurationHelper.Instance.OWSConfig.hotelDomain,// "RTP",
                                kioskID = ConfigurationHelper.Instance.OWSConfig.kioskID,// "KIOSK",
                                language = ConfigurationHelper.Instance.OWSConfig.language,// "EN",
                                legnumber = ConfigurationHelper.Instance.OWSConfig.legNumber,// "1",
                                password = ConfigurationHelper.Instance.OWSConfig.password,// "$$$KIOSK$$",
                                systemType = ConfigurationHelper.Instance.OWSConfig.systemType,// "KIOSK",
                                username = ConfigurationHelper.Instance.OWSConfig.username,//"KIOSK",
                                clientID = "BO",
                                preAuthUDF = ConfigurationHelper.Instance.OWSConfig.preAuthUDF,// "Appr_Code",
                                preAuthAmntUDF = ConfigurationHelper.Instance.OWSConfig.preAuthAmntUDF,// "Appr_Code"
                                GarunteeTypeCode = ConfigurationHelper.Instance.OWSConfig.GarunteeTypeCode,
                                DueInBufferDays = ConfigurationHelper.Instance.OWSConfig.DueInBufferDays,
                                IsBreakFastValidationWithUDF = ConfigurationHelper.Instance.OWSConfig.IsBreakFastValidationWithUDF,
                                IsBreakFastValidationWithPackage = ConfigurationHelper.Instance.OWSConfig.IsBreakFastValidationWithPackage,
                                PackageCodes = ConfigurationHelper.Instance.OWSConfig.PackageCodes,
                                MealPlanFieldName = ConfigurationHelper.Instance.OWSConfig.MealPlanFieldName,
                                IsETADefault = ConfigurationHelper.Instance.GrabberConfig.IsETADefault,
                                IsPaymentDisabled= ConfigurationHelper.Instance.GrabberConfig.IsPaymentDisabled,
                            }
                        },

                    };


                    string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(sendPrecheckinRequest);

                    File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\Push_Due-in_log.txt"), new string[] { $"{DateTime.Now.ToString()} Request Json : {jsonString}" });

                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpContent requestContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    httpClient.Timeout = TimeSpan.FromMinutes(10);

                    HttpResponseMessage response = await httpClient.PostAsync($"{ConfigurationHelper.Instance.OWSConfig.LocalAPIURL}/LocalService/PushDueInReservation", requestContent);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = await response.Content.ReadAsStringAsync();
                        var pushReservationResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<LocalResponseModel>(responseString);
                        return pushReservationResponse;
                    }
                    else
                    {
                        string FailureResponse = await response.Content.ReadAsStringAsync();
                        return new LocalResponseModel()
                        {
                            result = false,
                            responseMessage = FailureResponse.Substring(0, Math.Min(FailureResponse.Length, 25))
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new LocalResponseModel()
                    {
                        result = false,
                        responseMessage = ex.Message
                    };
                }
            }
        }
        public async Task<LocalResponseModel> PushLocalReservation(string ConfirmationNo)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigurationHelper.Instance.OWSConfig.LocalAPIURL);
                try
                {
                    var sendPrecheckinRequest = new
                    {
                        RequestObject = new
                        {
                            ReservationNumber = ConfirmationNo,
                            isForceFetch = true,
                            ServiceParameters = new
                            {
                                isProxyEnableForCloudAPI = ConfigurationHelper.Instance.GrabberConfig.isProxyEnableForCloudAPI,// true,
                                cloudAPIProxyHost = ConfigurationHelper.Instance.GrabberConfig.CloudAPIProxyHost,// "http://rtputm01.myfairmont.com:8080",
                                cloudAPIProxyUN = ConfigurationHelper.Instance.GrabberConfig.CloudAPIProxyUN,// "CPH\\_rtpfps",
                                cloudAPIProxyPswd = ConfigurationHelper.Instance.GrabberConfig.CloudAPIProxyPswd,// "IT$upp0rt",
                                cloudAPIURL = ConfigurationHelper.Instance.GrabberConfig.CloudAPIURL,// "https://fairmontsingapore.h-butler.com/WebCheckin/api",
                                isProxyEnableForLocalAPI = ConfigurationHelper.Instance.GrabberConfig.isProxyEnableForLocalAPI,// false,
                                localAPIProxyHost = ConfigurationHelper.Instance.GrabberConfig.LocalAPIProxyHost,// null,
                                localAPIProxyUN = ConfigurationHelper.Instance.GrabberConfig.LocalAPIProxyUN,// null,
                                localAPIProxyPswd = ConfigurationHelper.Instance.GrabberConfig.LocalAPIProxyPswd,// null,
                                localAPIURL = ConfigurationHelper.Instance.GrabberConfig.LocalAPIURL,// ",
                                isProxyEnableForEmailAPI = ConfigurationHelper.Instance.GrabberConfig.isProxyEnableForEmailAPI,// false,
                                emailAPIProxyHost = ConfigurationHelper.Instance.GrabberConfig.EmailAPIProxyHost,// null,
                                emailAPIProxyUN = ConfigurationHelper.Instance.GrabberConfig.EmailAPIProxyUN,// null,
                                emailAPIProxyPswd = ConfigurationHelper.Instance.GrabberConfig.EmailAPIProxyPswd,// null,
                                emailURL = ConfigurationHelper.Instance.GrabberConfig.EmailURL,// "http://10.193.19.29:8087/api",

                                preArrivalConfirmationEmail = ConfigurationHelper.Instance.GrabberConfig.PreArrivalFromEmail,// null,
                                preArrivalConfirmationEmailSubject = ConfigurationHelper.Instance.GrabberConfig.PreArrivalEmailSubject,// null,

                                PreArrivalFromEmail = ConfigurationHelper.Instance.GrabberConfig.PreArrivalFromEmail,// null,
                                PreArrivalEmailSubject = ConfigurationHelper.Instance.GrabberConfig.PreArrivalEmailSubject,// null,

                                emailDisplayName = ConfigurationHelper.Instance.GrabberConfig.EmailDisplayName,// "Fairmont Singapore",
                                chainCode = ConfigurationHelper.Instance.OWSConfig.chainCode,// "CHA",
                                destinationEntityID = ConfigurationHelper.Instance.OWSConfig.destinationEntityID,// "TI",
                                hotelDomain = ConfigurationHelper.Instance.OWSConfig.hotelDomain,// "RTP",
                                kioskID = ConfigurationHelper.Instance.OWSConfig.kioskID,// "KIOSK",
                                language = ConfigurationHelper.Instance.OWSConfig.language,// "EN",
                                legnumber = ConfigurationHelper.Instance.OWSConfig.legNumber,// "1",
                                password = ConfigurationHelper.Instance.OWSConfig.password,// "$$$KIOSK$$",
                                systemType = ConfigurationHelper.Instance.OWSConfig.systemType,// "KIOSK",
                                username = ConfigurationHelper.Instance.OWSConfig.username,//"KIOSK",
                                clientID = "BO",
                                preAuthUDF = ConfigurationHelper.Instance.OWSConfig.preAuthUDF,// "Appr_Code",
                                preAuthAmntUDF = ConfigurationHelper.Instance.OWSConfig.preAuthAmntUDF,// "Appr_Code"
                                GarunteeTypeCode = ConfigurationHelper.Instance.OWSConfig.GarunteeTypeCode,
                                DueInBufferDays = ConfigurationHelper.Instance.OWSConfig.DueInBufferDays,
                                IsBreakFastValidationWithUDF = ConfigurationHelper.Instance.OWSConfig.IsBreakFastValidationWithUDF,
                                IsBreakFastValidationWithPackage = ConfigurationHelper.Instance.OWSConfig.IsBreakFastValidationWithPackage,
                                PackageCodes = ConfigurationHelper.Instance.OWSConfig.PackageCodes,
                                MealPlanFieldName = ConfigurationHelper.Instance.OWSConfig.MealPlanFieldName,
                                IsETADefault = ConfigurationHelper.Instance.GrabberConfig.IsETADefault,
                                IsPaymentDisabled = ConfigurationHelper.Instance.GrabberConfig.IsPaymentDisabled,
                            }
                        },

                    };


                    string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(sendPrecheckinRequest);

                    File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\Push_Due-in_log.txt"), new string[] { $"{DateTime.Now.ToString()} Request Json : {jsonString}" });

                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpContent requestContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    httpClient.Timeout = TimeSpan.FromMinutes(10);

                    HttpResponseMessage response = await httpClient.PostAsync($"{ConfigurationHelper.Instance.OWSConfig.LocalAPIURL}/Local/PushReservationLocally", requestContent);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = await response.Content.ReadAsStringAsync();
                        var pushReservationResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<LocalResponseModel>(responseString);
                        return pushReservationResponse;
                    }
                    else
                    {
                        string FailureResponse = await response.Content.ReadAsStringAsync();
                        return new LocalResponseModel()
                        {
                            result = false,
                            responseMessage = FailureResponse.Substring(0, Math.Min(FailureResponse.Length, 25))
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new LocalResponseModel()
                    {
                        result = false,
                        responseMessage = ex.Message
                    };
                }
            }
        }
    }
}