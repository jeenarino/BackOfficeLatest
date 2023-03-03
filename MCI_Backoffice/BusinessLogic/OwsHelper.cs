using CheckinPortal.BackOffice.Helpers;
using CheckinPortal.BackOffice.Models;
using CheckinPortal.BackOffice.Models.OwsModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services.Description;

namespace CheckinPortal.BackOffice.BusinessLogic
{
    public class OwsHelper
    {
        public async Task<FetRoomResponseModel> GetOperaRoomListByRoomType(DateTime DepartureDate, string RoomType)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(ConfigurationHelper.Instance.OWSConfig.BaseURL);
                    var fetchRoomsList = new Models.FetchRoomList() { };

                    fetchRoomsList.departureDate = DepartureDate.ToString("yyyy-MM-dd");
                    fetchRoomsList.roomTypes = new string[1];
                    fetchRoomsList.roomTypes[0] = RoomType;

                    var fetchRoomListModel = new Models.fetchRoomList()
                    {
                        chainCode = ConfigurationHelper.Instance.OWSConfig.chainCode,
                        destinationEntityID = ConfigurationHelper.Instance.OWSConfig.destinationEntityID,
                        destinationSystemType = ConfigurationHelper.Instance.OWSConfig.destinationSystemType,
                        FetchRoomList = fetchRoomsList,
                        hotelDomain = ConfigurationHelper.Instance.OWSConfig.hotelDomain,
                        kioskID = ConfigurationHelper.Instance.OWSConfig.kioskID,
                        language = ConfigurationHelper.Instance.OWSConfig.language,
                        legNumber = ConfigurationHelper.Instance.OWSConfig.legNumber,
                        password = ConfigurationHelper.Instance.OWSConfig.password,
                        systemType = ConfigurationHelper.Instance.OWSConfig.systemType,
                        username = ConfigurationHelper.Instance.OWSConfig.username
                    };

                    string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(fetchRoomListModel);

                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpContent requestContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    httpClient.Timeout = TimeSpan.FromMinutes(10);
                    HttpResponseMessage response = await httpClient.PostAsync($"FetchRoomList", requestContent);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = await response.Content.ReadAsStringAsync();
                        var FetchRoomResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<FetRoomResponseModel>(responseString);
                        return FetchRoomResponse;
                    }
                    else
                    {
                        string FailureResponse = await response.Content.ReadAsStringAsync();
                        return new FetRoomResponseModel()
                        {
                            responseMessage = FailureResponse.Substring(0, Math.Min(FailureResponse.Length, 25))
                        };
                    }
                }
            }
            catch
            {

                return new FetRoomResponseModel()
                {
                    responseMessage = "unable to get room list"
                };
            }
        }

        public async Task<OWSResponseModel> CheckinIntoOpera(string ReservationNameID)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigurationHelper.Instance.OWSConfig.BaseURL);

                CheckinIntoOperaModel checkinIntoOperaModel = new Models.CheckinIntoOperaModel()
                {
                    chainCode = ConfigurationHelper.Instance.OWSConfig.chainCode,
                    destinationEntityID = ConfigurationHelper.Instance.OWSConfig.destinationEntityID,
                    destinationSystemType = ConfigurationHelper.Instance.OWSConfig.destinationSystemType,
                    OperaReservation = new OperaReservation()
                    {
                        ReservationNameID = ReservationNameID
                    },
                    hotelDomain = ConfigurationHelper.Instance.OWSConfig.hotelDomain,
                    kioskID = ConfigurationHelper.Instance.OWSConfig.kioskID,
                    language = ConfigurationHelper.Instance.OWSConfig.language,
                    legNumber = ConfigurationHelper.Instance.OWSConfig.legNumber,
                    password = ConfigurationHelper.Instance.OWSConfig.password,
                    systemType = ConfigurationHelper.Instance.OWSConfig.systemType,
                    username = ConfigurationHelper.Instance.OWSConfig.username
                };

                string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(checkinIntoOperaModel);
                File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} CheckinIntoOpera Request Json : {jsonString}" });
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpContent requestContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                httpClient.Timeout = TimeSpan.FromMinutes(10);
                HttpResponseMessage response = await httpClient.PostAsync($"GuestCheckIn", requestContent);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} CheckinIntoOpera Response Json : {responseString}" });
                    var FetchRoomResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<OWSResponseModel>(responseString);
                    return FetchRoomResponse;
                }
                else
                {
                    string FailureResponse = await response.Content.ReadAsStringAsync();
                    File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} CheckinIntoOpera Response Json : {FailureResponse}" });
                    return new OWSResponseModel()
                    {
                        responseMessage = FailureResponse.Substring(0, Math.Min(FailureResponse.Length, 25))
                    };
                }
            }
        }

        public async Task<OWSResponseModel> AssignRoom(string RoomNumber, string ReservationNameID)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigurationHelper.Instance.OWSConfig.BaseURL);

                AssignRoomRequestModel checkinIntoOperaModel = new Models.AssignRoomRequestModel()
                {
                    chainCode = ConfigurationHelper.Instance.OWSConfig.chainCode,
                    destinationEntityID = ConfigurationHelper.Instance.OWSConfig.destinationEntityID,
                    destinationSystemType = ConfigurationHelper.Instance.OWSConfig.destinationSystemType,
                    AssignRoomRequest = new AssignRoomAPIModel()
                    {
                        ReservationNameID = ReservationNameID,
                        RoomNumber = RoomNumber,
                        StationID = "Backoffice"
                    },
                    hotelDomain = ConfigurationHelper.Instance.OWSConfig.hotelDomain,
                    kioskID = ConfigurationHelper.Instance.OWSConfig.kioskID,
                    language = ConfigurationHelper.Instance.OWSConfig.language,
                    legNumber = ConfigurationHelper.Instance.OWSConfig.legNumber,
                    password = ConfigurationHelper.Instance.OWSConfig.password,
                    systemType = ConfigurationHelper.Instance.OWSConfig.systemType,
                    username = ConfigurationHelper.Instance.OWSConfig.username
                };

                string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(checkinIntoOperaModel);
                File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Assign Room Json : {jsonString}" });
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpContent requestContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                httpClient.Timeout = TimeSpan.FromMinutes(10);
                HttpResponseMessage response = await httpClient.PostAsync($"AssignRoom", requestContent);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Assign Room Response : {responseString}" });
                    var FetchRoomResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<OWSResponseModel>(responseString);
                    return FetchRoomResponse;
                }
                else
                {
                    string FailureResponse = await response.Content.ReadAsStringAsync();
                    File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Assign Room Response : {FailureResponse}" });
                    return new OWSResponseModel()
                    {
                        responseMessage = FailureResponse.Substring(0, Math.Min(FailureResponse.Length, 25))
                    };
                }
            }
        }

        public async Task<OwsFetchReservationResponseModel> FetchReservation(string ReservationNumber)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(ConfigurationHelper.Instance.OWSConfig.BaseURL);
                    CheckinIntoOperaModel fetchResevationModel = new Models.CheckinIntoOperaModel()
                    {
                        chainCode = ConfigurationHelper.Instance.OWSConfig.chainCode,
                        destinationEntityID = ConfigurationHelper.Instance.OWSConfig.destinationEntityID,
                        destinationSystemType = ConfigurationHelper.Instance.OWSConfig.destinationSystemType,
                        FetchBookingRequest = new FetchBookingRequest()
                        {
                            reservationNumber = ReservationNumber
                        },
                        hotelDomain = ConfigurationHelper.Instance.OWSConfig.hotelDomain,
                        kioskID = ConfigurationHelper.Instance.OWSConfig.kioskID,
                        language = ConfigurationHelper.Instance.OWSConfig.language,
                        legNumber = ConfigurationHelper.Instance.OWSConfig.legNumber,
                        password = ConfigurationHelper.Instance.OWSConfig.password,
                        systemType = ConfigurationHelper.Instance.OWSConfig.systemType,
                        username = ConfigurationHelper.Instance.OWSConfig.username
                    };

                    string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(fetchResevationModel);
                    File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Fetch Reservation Request : {jsonString}" });
                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpContent requestContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    httpClient.Timeout = TimeSpan.FromMinutes(10);
                    HttpResponseMessage response = await httpClient.PostAsync($"FetchReservation", requestContent);
                    
                    if (response.IsSuccessStatusCode)
                    {
                        File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Fetch Reservation Success" });
                        string responseString = await response.Content.ReadAsStringAsync();
                        
                        File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Fetch Reservation {responseString}" });
                        
                        var FetchRoomResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<OwsFetchReservationResponseModel>(responseString);
                        return FetchRoomResponse;
                    }
                    else
                    {
                        File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Fetch Reservation Failed" });
                        string FailureResponse = await response.Content.ReadAsStringAsync();
                        return new OwsFetchReservationResponseModel()
                        {
                             result =false,
                            responseMessage = FailureResponse.Substring(0, Math.Min(FailureResponse.Length, 25))
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Fetch Reservation Exception {ex.ToString()}" });

                return new OwsFetchReservationResponseModel()
                {
                    result = false,
                    responseMessage = ex.Message.Substring(0, Math.Min(ex.Message.Length, 25))
                };
            }
        }

        public async Task<OWSResponseModel> UpdateProfileEmail(Emails emails,string pmsProfileID)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(ConfigurationHelper.Instance.OWSConfig.BaseURL);
                    UpdateProileRequestModel fetchResevationModel = new Models.UpdateProileRequestModel()
                    {
                        chainCode = ConfigurationHelper.Instance.OWSConfig.chainCode,
                        destinationEntityID = ConfigurationHelper.Instance.OWSConfig.destinationEntityID,
                        destinationSystemType = ConfigurationHelper.Instance.OWSConfig.destinationSystemType,                       
                        hotelDomain = ConfigurationHelper.Instance.OWSConfig.hotelDomain,
                        kioskID = ConfigurationHelper.Instance.OWSConfig.kioskID,
                        language = ConfigurationHelper.Instance.OWSConfig.language,
                        legNumber = ConfigurationHelper.Instance.OWSConfig.legNumber,
                        password = ConfigurationHelper.Instance.OWSConfig.password,
                        systemType = ConfigurationHelper.Instance.OWSConfig.systemType,
                        username = ConfigurationHelper.Instance.OWSConfig.username
                    };
                    fetchResevationModel.updateProileRequest = new UpdateProileRequest()
                    {
                        ProfileID = pmsProfileID,
                        emails = new List<Emails>()
                        {
                            emails
                        }
                    };

                    string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(fetchResevationModel);
                    File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} UpdateEmailList Json : {jsonString}" });
                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpContent requestContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    httpClient.Timeout = TimeSpan.FromMinutes(10);
                    HttpResponseMessage response = await httpClient.PostAsync($"UpdateEmailList", requestContent);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = await response.Content.ReadAsStringAsync();
                        var FetchRoomResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<OWSResponseModel>(responseString);
                        return FetchRoomResponse;
                    }
                    else
                    {
                        string FailureResponse = await response.Content.ReadAsStringAsync();
                        return new OWSResponseModel()
                        {
                            responseMessage = FailureResponse.Substring(0, Math.Min(FailureResponse.Length, 25))
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new OWSResponseModel()
                {
                    responseMessage = ex.Message.Substring(0, Math.Min(ex.Message.Length, 25))
                };
            }
        }

        public async Task<OWSResponseModel> UpdateProfilePhone(Phones phones, string pmsProfileID)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(ConfigurationHelper.Instance.OWSConfig.BaseURL);
                    UpdateProileRequestModel fetchResevationModel = new Models.UpdateProileRequestModel()
                    {
                        chainCode = ConfigurationHelper.Instance.OWSConfig.chainCode,
                        destinationEntityID = ConfigurationHelper.Instance.OWSConfig.destinationEntityID,
                        destinationSystemType = ConfigurationHelper.Instance.OWSConfig.destinationSystemType,
                        hotelDomain = ConfigurationHelper.Instance.OWSConfig.hotelDomain,
                        kioskID = ConfigurationHelper.Instance.OWSConfig.kioskID,
                        language = ConfigurationHelper.Instance.OWSConfig.language,
                        legNumber = ConfigurationHelper.Instance.OWSConfig.legNumber,
                        password = ConfigurationHelper.Instance.OWSConfig.password,
                        systemType = ConfigurationHelper.Instance.OWSConfig.systemType,
                        username = ConfigurationHelper.Instance.OWSConfig.username
                    };
                    fetchResevationModel.updateProileRequest = new UpdateProileRequest()
                    {
                        ProfileID = pmsProfileID,
                        phones = new List<Phones>()
                        {
                            phones
                        }
                    };

                    string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(fetchResevationModel);
                    File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} UpdatePhoneList Json : {jsonString}" });
                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpContent requestContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    httpClient.Timeout = TimeSpan.FromMinutes(10);
                    HttpResponseMessage response = await httpClient.PostAsync($"UpdatePhoneList", requestContent);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = await response.Content.ReadAsStringAsync();
                        var FetchRoomResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<OWSResponseModel>(responseString);
                        return FetchRoomResponse;
                    }
                    else
                    {
                        string FailureResponse = await response.Content.ReadAsStringAsync();
                        return new OWSResponseModel()
                        {
                            responseMessage = FailureResponse.Substring(0, Math.Min(FailureResponse.Length, 25))
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new OWSResponseModel()
                {
                    responseMessage = ex.Message.Substring(0, Math.Min(ex.Message.Length, 25))
                };
            }
        }

        public async Task<OWSResponseModel> UpdateProfileAddress(Addresses addresses, string pmsProfileID)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(ConfigurationHelper.Instance.OWSConfig.BaseURL);
                    UpdateProileRequestModel fetchResevationModel = new Models.UpdateProileRequestModel()
                    {
                        chainCode = ConfigurationHelper.Instance.OWSConfig.chainCode,
                        destinationEntityID = ConfigurationHelper.Instance.OWSConfig.destinationEntityID,
                        destinationSystemType = ConfigurationHelper.Instance.OWSConfig.destinationSystemType,
                        hotelDomain = ConfigurationHelper.Instance.OWSConfig.hotelDomain,
                        kioskID = ConfigurationHelper.Instance.OWSConfig.kioskID,
                        language = ConfigurationHelper.Instance.OWSConfig.language,
                        legNumber = ConfigurationHelper.Instance.OWSConfig.legNumber,
                        password = ConfigurationHelper.Instance.OWSConfig.password,
                        systemType = ConfigurationHelper.Instance.OWSConfig.systemType,
                        username = ConfigurationHelper.Instance.OWSConfig.username
                    };

                    fetchResevationModel.updateProileRequest = new UpdateProileRequest()
                    {
                        ProfileID = pmsProfileID,
                        addresses = new List<Addresses>()
                        {
                            addresses
                        },
                    };


                    string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(fetchResevationModel);
                    File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} UpdateAddresList Json : {jsonString}" });
                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpContent requestContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    httpClient.Timeout = TimeSpan.FromMinutes(10);
                    HttpResponseMessage response = await httpClient.PostAsync($"UpdateAddresList", requestContent);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = await response.Content.ReadAsStringAsync();
                        var FetchRoomResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<OWSResponseModel>(responseString);
                        return FetchRoomResponse;
                    }
                    else
                    {
                        string FailureResponse = await response.Content.ReadAsStringAsync();
                        return new OWSResponseModel()
                        {
                            responseMessage = FailureResponse.Substring(0, Math.Min(FailureResponse.Length, 25))
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new OWSResponseModel()
                {
                    responseMessage = ex.Message.Substring(0, Math.Min(ex.Message.Length, 25))
                };
            }
        }

        public async Task<OWSResponseModel> UpdateProfilePassport(Emails emails, Phones phones, Addresses addresses, DocInformation docInformation, string pmsProfileID)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(ConfigurationHelper.Instance.OWSConfig.BaseURL);
                    UpdateProileRequestModel fetchResevationModel = new Models.UpdateProileRequestModel()
                    {
                        chainCode = ConfigurationHelper.Instance.OWSConfig.chainCode,
                        destinationEntityID = ConfigurationHelper.Instance.OWSConfig.destinationEntityID,
                        destinationSystemType = ConfigurationHelper.Instance.OWSConfig.destinationSystemType,
                        hotelDomain = ConfigurationHelper.Instance.OWSConfig.hotelDomain,
                        kioskID = ConfigurationHelper.Instance.OWSConfig.kioskID,
                        language = ConfigurationHelper.Instance.OWSConfig.language,
                        legNumber = ConfigurationHelper.Instance.OWSConfig.legNumber,
                        password = ConfigurationHelper.Instance.OWSConfig.password,
                        systemType = ConfigurationHelper.Instance.OWSConfig.systemType,
                        username = ConfigurationHelper.Instance.OWSConfig.username
                    };

                    fetchResevationModel.updateProileRequest = new UpdateProileRequest()
                    {
                        ProfileID = pmsProfileID,
                        documentNumber = docInformation.documentNumber,
                        documentType = docInformation.documentType,
                        dob = docInformation.dob,
                        gender = docInformation.gender,
                        issueCountry = docInformation.issueCountry,
                        issueDate = docInformation.issueDate,
                        nationality = docInformation.nationality,
                        phones = new List<Phones>()
                        {
                            phones
                        },
                            emails = new List<Emails>()
                        {
                            emails
                        },
                            addresses = new List<Addresses>()
                        {
                            addresses
                        }
                    };


                    string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(fetchResevationModel);
                    File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} UpdatePassport Json : {jsonString}" });
                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpContent requestContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    httpClient.Timeout = TimeSpan.FromMinutes(10);
                    HttpResponseMessage response = await httpClient.PostAsync($"UpdatePassport", requestContent);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = await response.Content.ReadAsStringAsync();
                        var FetchRoomResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<OWSResponseModel>(responseString);
                        return FetchRoomResponse;
                    }
                    else
                    {
                        string FailureResponse = await response.Content.ReadAsStringAsync();
                        return new OWSResponseModel()
                        {
                            responseMessage = FailureResponse.Substring(0, Math.Min(FailureResponse.Length, 25))
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new OWSResponseModel()
                {
                    responseMessage = ex.Message.Substring(0, Math.Min(ex.Message.Length, 25))
                };
            }
        }

        public async Task<OWSResponseModel> UpdateProfileName(Emails emails, Phones phones, Addresses addresses, DocInformation docInformation, string pmsProfileID)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(ConfigurationHelper.Instance.OWSConfig.BaseURL);
                    UpdateProileRequestModel fetchResevationModel = new Models.UpdateProileRequestModel()
                    {
                        chainCode = ConfigurationHelper.Instance.OWSConfig.chainCode,
                        destinationEntityID = ConfigurationHelper.Instance.OWSConfig.destinationEntityID,
                        destinationSystemType = ConfigurationHelper.Instance.OWSConfig.destinationSystemType,
                        hotelDomain = ConfigurationHelper.Instance.OWSConfig.hotelDomain,
                        kioskID = ConfigurationHelper.Instance.OWSConfig.kioskID,
                        language = ConfigurationHelper.Instance.OWSConfig.language,
                        legNumber = ConfigurationHelper.Instance.OWSConfig.legNumber,
                        password = ConfigurationHelper.Instance.OWSConfig.password,
                        systemType = ConfigurationHelper.Instance.OWSConfig.systemType,
                        username = ConfigurationHelper.Instance.OWSConfig.username
                    };

                    fetchResevationModel.updateProileRequest = new UpdateProileRequest()
                    {
                        ProfileID = pmsProfileID,
                        documentNumber = docInformation.documentNumber,
                        documentType = docInformation.documentType,
                        dob = docInformation.dob,
                        gender = docInformation.gender,
                        issueCountry = docInformation.issueCountry,
                        issueDate = docInformation.issueDate,
                        nationality = docInformation.nationality,
                        phones = new List<Phones>()
                        {
                            phones
                        },
                        emails = new List<Emails>()
                        {
                            emails
                        },
                        addresses = new List<Addresses>()
                        {
                            addresses
                        }
                    };

                    string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(fetchResevationModel);
                    File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} UpdateName Json : {jsonString}" });
                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpContent requestContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    httpClient.Timeout = TimeSpan.FromMinutes(10);
                    HttpResponseMessage response = await httpClient.PostAsync($"UpdateName", requestContent);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = await response.Content.ReadAsStringAsync();
                        var FetchRoomResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<OWSResponseModel>(responseString);
                        return FetchRoomResponse;
                    }
                    else
                    {
                        string FailureResponse = await response.Content.ReadAsStringAsync();
                        return new OWSResponseModel()
                        {
                            responseMessage = FailureResponse.Substring(0, Math.Min(FailureResponse.Length, 25))
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new OWSResponseModel()
                {
                    responseMessage = ex.Message.Substring(0, Math.Min(ex.Message.Length, 25))
                };
            }
        }

        public async Task<OWSResponseModel> GetOperaBusinessDate(string userName,string password)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(ConfigurationHelper.Instance.OWSConfig.BaseURL);
                    CheckinIntoOperaModel fetchResevationModel = new Models.CheckinIntoOperaModel()
                    {
                        chainCode = ConfigurationHelper.Instance.OWSConfig.chainCode,
                        destinationEntityID = ConfigurationHelper.Instance.OWSConfig.destinationEntityID,
                        destinationSystemType = ConfigurationHelper.Instance.OWSConfig.destinationSystemType,
                        hotelDomain = ConfigurationHelper.Instance.OWSConfig.hotelDomain,
                        kioskID = ConfigurationHelper.Instance.OWSConfig.kioskID,
                        language = ConfigurationHelper.Instance.OWSConfig.language,
                        legNumber = ConfigurationHelper.Instance.OWSConfig.legNumber,
                        password = password,
                        systemType = ConfigurationHelper.Instance.OWSConfig.systemType,
                        username = userName
                    };

                    string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(fetchResevationModel);
                    File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} GetOperaBusinessDate Request : {jsonString}" });
                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpContent requestContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    httpClient.Timeout = TimeSpan.FromMinutes(10);
                    HttpResponseMessage response = await httpClient.PostAsync($"GetBusinessDate", requestContent);

                    if (response.IsSuccessStatusCode)
                    {
                        File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} GetOperaBusinessDate Success" });
                        string responseString = await response.Content.ReadAsStringAsync();

                        File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} GetOperaBusinessDate {responseString}" });

                        var FetchRoomResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<OWSResponseModel>(responseString);
                        return FetchRoomResponse;
                    }
                    else
                    {
                        File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} GetOperaBusinessDate Failed" });
                        string FailureResponse = await response.Content.ReadAsStringAsync();
                        return new OWSResponseModel()
                        {
                            responseMessage = FailureResponse.Substring(0, Math.Min(FailureResponse.Length, 25)),
                            result = false
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                File.AppendAllLines(HttpContext.Current.Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} GetOperaBusinessDate Exception {ex.ToString()}" });

                return new OWSResponseModel()
                {
                    responseMessage = ex.Message.Substring(0, Math.Min(ex.Message.Length, 25)),
                    result = false
                };
            }
        }

    }
}