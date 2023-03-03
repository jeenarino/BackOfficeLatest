using CheckinPortal.BackOffice.Helpers;
using CheckinPortal.BackOffice.Models;
using DigiDoc.DataAccess.Models;
using DlibDotNet;
using DlibDotNet.Extensions;
using MCIGrabberService.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CheckinPortal.BackOffice.Controllers
{
    public class EVAController : Controller
    {
        // GET: EVA
        public ActionResult Index(string Status = "All")
        {
            DateTime startDate = DateTime.Now;

            ViewBag.StartDate = startDate.ToString("MM/dd/yyyy");
            ViewBag.EndDate = startDate.ToString("MM/dd/yyyy");
            ViewBag.Status = Status;
            return View();
        }
        public ActionResult GetEVAListAjax(DocumentDataTableModel model, CheckinPortal.BackOffice.Models.Search search, string StartDate, string EndDate, string Status = "All")
        {
            DateTime StartDateDT;
            DateTime EndDateDT;

            DateTime startDate = DateTime.Now;
            if (string.IsNullOrEmpty(StartDate))
                StartDateDT = startDate;
            else
            {
                StartDateDT = DateTime.ParseExact(StartDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            }

            if (string.IsNullOrEmpty(EndDate))
                EndDateDT = startDate;
            else
            {
                EndDateDT = DateTime.ParseExact(EndDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            }
            int start = 0;

            if (model.Start > 0)
            {
                start = model.Start / model.Length;
            }

            start += 1;

            string filterby = string.Empty;
            string soryOrder = "DEC";
            string sortBy = "";
            string sortColumn = "";

            if (Request.Params["search[value]"] != null)
            {
                filterby = Request.Params["search[value]"].ToString();
            }

            if (Request.Params["order[0][column]"] != null)
            {
                sortBy = Request.Params["order[0][column]"].ToString();
            }

            if (sortBy == "0")
            {
                sortColumn = "Confirmation No";
            }
            else if (sortBy == "1")
            {
                sortColumn = "Arrival Date";
            }
            else if (sortBy == "2")
            {
                sortColumn = "Departure Date";
            }
            else if (sortBy == "3")
            {
                sortColumn = "Document Type";
            }
            else if (sortBy == "4")
            {
                sortColumn = "MannualAutherize";
            }
            else if (sortBy == "5")
            {
                sortColumn = "Status";
            }
            else if (sortBy == "6")
            {
                sortColumn = "Details";
            }


            if (Request.Params["order[0][dir]"] != null)
            {
                soryOrder = Request.Params["order[0][dir]"].ToString();
            }

            var spResponse = new EVAHelper().fetchEVADetails(start, model.Length, StartDateDT.ToString("yyyyMMdd"), EndDateDT.ToString("yyyyMMdd"), filterby, sortColumn, soryOrder, Status);
            if (spResponse != null && spResponse.result)
            {
                var evaHeaders = new List<EVAModel>();
                try
                {
                    evaHeaders = (List<EVAModel>)spResponse.ResponseData;


                    var TotalCount = evaHeaders[0].TotalRecords;

                    var response = new
                    {
                        draw = model.draw,
                        data = evaHeaders,
                        recordsFiltered = TotalCount,
                        recordsTotal = TotalCount
                    };

                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {

                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var response = new
                {
                    draw = model.draw,
                    data = spResponse,
                    recordsFiltered = 0,
                    recordsTotal = 0
                };
                return Json(response, JsonRequestBehavior.AllowGet);
            }





        }
        public async Task<ActionResult> Details(string ResponseId)
        { 
            EVAImageVerification eVA=new EVAImageVerification();
            try
            {
                var ShowButton = false;
                var spResponse = new EVAHelper().fetchEVADById(ResponseId);
                if (spResponse != null && spResponse.result)
                {
                    var evaHeaders = (List<EVAImageVerification>)spResponse.ResponseData;
                    if(evaHeaders != null)
                    {
                        eVA=evaHeaders.FirstOrDefault();
                        int GuestIndex = 0;
                        string path1 = Server.MapPath($"~/temp/{GuestIndex}_1_{eVA.ReservationNumber}.jpeg");
                        string path2 = Server.MapPath($"~/temp/{GuestIndex}_2_{eVA.ReservationNumber}.jpeg");
                        string path3 = Server.MapPath($"~/temp/{GuestIndex}_3_{eVA.ReservationNumber}.jpeg");
                        string path4 = Server.MapPath($"~/temp/{GuestIndex}_live_{eVA.ReservationNumber}.jpeg");

                      
                        if (eVA.DocumentImage1 != null)
                        {
                            System.IO.File.WriteAllBytes(path1, eVA.DocumentImage1);
                           
                        }
                     
                        if (eVA.DocumentImage2 != null)
                        {
                            System.IO.File.WriteAllBytes(path2, eVA.DocumentImage2);
                           
                        }

                        if (eVA.DocumentImage3 != null)
                        {
                            System.IO.File.WriteAllBytes(path3, eVA.DocumentImage3);
                           
                        }
                        if (eVA.LivePhoto != null)
                        {
                            byte[] data = Convert.FromBase64String(eVA.LivePhoto);
                            System.IO.File.WriteAllBytes(path4, data);
                        }
                        ShowButton = true;
                        if (eVA.StatusCode=="201")
                        {
                            ShowButton = false;
                        }
                       
                        ViewBag.ShowButton = ShowButton;
                        ViewBag.DocumentPath1 = $"{GuestIndex}_1_{eVA.ReservationNumber}.jpeg";
                        ViewBag.DocumentPath2 = $"{GuestIndex}_2_{eVA.ReservationNumber}.jpeg";
                        ViewBag.DocumentPath3 = $"{GuestIndex}_3_{eVA.ReservationNumber}.jpeg";
                        ViewBag.LivePhoto = $"{GuestIndex}_live_{eVA.ReservationNumber}.jpeg";
                    }
                }
                ViewBag.ShowButton = ShowButton;
                return View(eVA);
            }
            catch (Exception ex)
            {

            }
            return View(eVA);
        }

        public async Task<ActionResult> SendEVA(string ResponseId)
        {
            EVAImageVerification eVA = new EVAImageVerification();
            var spResponse = new EVAHelper().fetchEVADById(ResponseId);
            if (spResponse != null && spResponse.result)
            {
                var evaHeaders = (List<EVAImageVerification>)spResponse.ResponseData;
                if (evaHeaders != null)
                {
                    eVA = evaHeaders.FirstOrDefault();
                }
            }
            var s=SendToSTB(ResponseId, eVA);
            if(s!=null)
            {
                SaveResponseToDB(eVA.ReservationNameID,eVA,s);
            }
            return View("Index");
        }
        private bool SaveResponseToDB(string reservationNameID, EVAImageVerification doc,VisitorCheckInResponse response)
        {
            var request = new tbSTBResponse()
            {
                ReservationNameID = reservationNameID,
                DocumentType = doc.DocumentType,
                DocumentNumber = doc.DocumentNumber,
                IsMannualAutherized = doc.IsMannualAutherized
            };

            if (response != null)
            {
                request.ResponseJSON = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                request.CreatedAt = response.CreatedAt;
                request.ResponseDateTime = response.Data?.ResponseDateTime ?? null;
                request.TransactionId = response.Data?.TransactionId ?? null;
                request.StatusCode = response.Status?.Code ?? null;
                if (response.Status?.ErrorCodeList != null)
                {
                    request.ErrorCodes = string.Join(",", response.Status.ErrorCodeList);
                }
                request.Message = response.Status?.Message ?? null;
            }

            return InsertUpdateSTBResponse(request);
        }

        public bool InsertUpdateSTBResponse(tbSTBResponse stbResponse)
        {
            string functionName = "InsertUpdateSTBResponse";
            string applicationName = "Backoffice";
            string description = "Save STB Response to local database tbSTBResponse.";
            try
            {
                using (var httpClient = new HttpClient())
                {

                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    HttpContent requestContent = new StringContent(JsonConvert.SerializeObject(new { RequestObject = stbResponse }), Encoding.UTF8, "application/json");
                    httpClient.Timeout = TimeSpan.FromMinutes(2);
                    HttpResponseMessage httpResponse = httpClient.PostAsync(new Uri($"{ConfigurationHelper.Instance.GrabberConfig.LocalAPIURL}/Local/InsertUpdateSTBResponse"), requestContent).Result;
                    if (httpResponse != null && httpResponse.IsSuccessStatusCode)
                    {
                        var responseMessage = httpResponse.Content.ReadAsStringAsync().Result;
                        if (!string.IsNullOrEmpty(responseMessage))
                        {
                            var apiResponse = JsonConvert.DeserializeObject<LocalResponseModel>(responseMessage);
                            if (apiResponse != null && apiResponse.result)
                            {
                                return apiResponse.result;
                            }
                            else if (apiResponse != null)
                            {
                                LogHelper.Instance.Debug($"Failed to Save STB Response to local DB with the below response from API :- {apiResponse.responseMessage}","",
                               functionName, applicationName, description);
                                return false;
                            }
                            else
                            {
                                LogHelper.Instance.Debug($"Failed to Save STB Response to local DB, since API returned NULL","",
                                functionName, applicationName, description);
                                return false;
                            }
                        }
                        else
                        {
                            LogHelper.Instance.Debug("Failed to Save STB Response to local DB with the below response from API, since API returned NULL","",
                                functionName, applicationName, description);
                            return false;
                        }
                    }
                    else
                    {
                        LogHelper.Instance.Debug($"Failed to Save STB Response to local DB with the below response from API, {httpResponse.ReasonPhrase} code:- {httpResponse.StatusCode}","",
                            functionName, applicationName, description);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Error(ex,"", functionName, applicationName, description);
                return false;
            }
        }
        private VisitorCheckInResponse SendToSTB(string ResponseId, EVAImageVerification eVA)
        {

          
                    EVARequestModel request = new EVARequestModel
            {
                webUrl = System.Configuration.ConfigurationManager.AppSettings["TransactionwebUrl"].ToString()
                    };
            var imgphoto = 0;
            byte[] docimage = eVA.DocumentImage1;
            if (eVA.DocumentImage1 != null)
            {
              
                if (imgphoto == 0)
                {
                    using (var faceDetector = Dlib.GetFrontalFaceDetector())
                    using (var ms = new MemoryStream(eVA.DocumentImage1))
                    using (var bitmap = (Bitmap)Image.FromStream(ms))
                    {
                        using (var image = bitmap.ToArray2D<RgbPixel>())
                        {
                            var dets = faceDetector.Operator(image);
                            if (dets.Count() > 0)
                            {
                                imgphoto = 1;
                            }

                        }

                    }
                }
            }

            if (eVA.DocumentImage2 != null)
            {
               
                if (imgphoto == 0)
                {
                    using (var faceDetector = Dlib.GetFrontalFaceDetector())
                    using (var ms = new MemoryStream(eVA.DocumentImage2))
                    using (var bitmap = (Bitmap)Image.FromStream(ms))
                    {
                        using (var image = bitmap.ToArray2D<RgbPixel>())
                        {
                            var dets = faceDetector.Operator(image);
                            if (dets.Count() > 0)
                            {
                                imgphoto = 2;
                                docimage = eVA.DocumentImage2;
                            }

                        }

                    }
                }
            }

            if (eVA.DocumentImage3 != null)
            {
              
                if (imgphoto == 0)
                {
                    using (var faceDetector = Dlib.GetFrontalFaceDetector())
                    using (var ms = new MemoryStream(eVA.DocumentImage2))
                    using (var bitmap = (Bitmap)Image.FromStream(ms))
                    {
                        using (var image = bitmap.ToArray2D<RgbPixel>())
                        {
                            var dets = faceDetector.Operator(image);
                            if (dets.Count() > 0)
                            {
                                imgphoto = 3;
                                docimage = eVA.DocumentImage3;
                            }

                        }

                    }
                }
            }
            string accessToken = GenerateToken();
            if (!string.IsNullOrEmpty(accessToken))
            {
                request.accessToken = accessToken;
                request.RequestObject = new VisitorCheckInRequest()
                {
                    Visitor = new Visitor()
                    {
                        Info = new VisitorInfo()
                        {
                            Name = eVA.FirstName+eVA.MiddleName+eVA.LastName,
                            Nationality = eVA.Nationality,
                            PassportNumber = eVA.DocumentNumber,
                            DateOfBirth = eVA.BirthDate,
                            Gender = eVA.Gender,
                            ManuallyInputted = false,
                            PassportType = "P",
                            CheckIn = eVA.ArrivalDateTime,
                            CheckOut = eVA.DepartureDateTime 
                        },
                        Hotel = new HotelDetails()
                        {
                            BookingId = eVA.ReservationNumber,
                            HotelCode = "L0098",
                            StaffId = Session["UserID"].ToString(),
                            StaffName = Session["UserName"].ToString()
                          
                        },
                        Images = new List<DocImage>()
                        {
                            GetImage("_imgScannedTd",Convert.ToBase64String(docimage)),
                            GetImage("_imgRef",CompressImage(Convert.ToBase64String(eVA.FaceImage))),
                            GetImage("_imgPhoto",CompressImage(eVA.LivePhoto))
                        }
                    }
                };

                return SendVisitorCheckin(request);
            }

            return null;
        }
        public static string CompressImage(string imageString)
        {
            try
            {
                return !string.IsNullOrEmpty(imageString)
                        ? Convert.ToBase64String(CompressImage(Convert.FromBase64String(imageString)))
                        : imageString;
            }
            catch
            {

                return imageString;
            }
        }

        public static byte[] CompressImage(byte[] imageByte)
        {
            try
            {
                if (imageByte != null && imageByte.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        var imageIn = CompressImage(Image.FromStream(new MemoryStream(imageByte)), 90);
                        imageIn.Save(ms, ImageFormat.Jpeg);
                        return ms.ToArray();
                    }
                }
            }
            catch { }
            return imageByte;
        }

        private static Image CompressImage(Image imgPhoto, int Percent)
        {
            try
            {
                float nPercent = (float)Percent / 100;

                int sourceWidth = imgPhoto.Width;
                int sourceHeight = imgPhoto.Height;

                int destWidth = (int)(sourceWidth * nPercent);
                int destHeight = (int)(sourceHeight * nPercent);

                Bitmap bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);

                bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

                using (Graphics grPhoto = Graphics.FromImage(bmPhoto))
                {
                    grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    
                    grPhoto.DrawImage(imgPhoto,
                        new System.Drawing.Rectangle(0, 0, destWidth, destHeight),
                        new System.Drawing.Rectangle(0, 0, sourceWidth, sourceHeight),
                        GraphicsUnit.Pixel);
                }

                return bmPhoto;
            }
            catch (Exception)
            {
                return null;
            }
        }
        private DocImage GetImage(string prefix, string image)
        {
            return new DocImage() { PrefixName = prefix, Base64Data = $"data:image/jpg;base64,{image}" };
        }
        public AccessTokenResponse GetAccessToken(EVARequestModel requestModel)
        {
            string functionName = "GetAccessToken";
            string applicationName = "Backoffice";
            string description = "get access token for STB EVA Interface.";
            try
            {
                using (var httpClient = new HttpClient())
                {

                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    HttpContent requestContent = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");
                    httpClient.Timeout = TimeSpan.FromMinutes(2);
                    HttpResponseMessage httpResponse = httpClient.PostAsync(new Uri($"{ConfigurationHelper.Instance.GrabberConfig.LocalAPIURL}/STBEva/GetAccessToken"), requestContent).Result;
                    if (httpResponse != null && httpResponse.IsSuccessStatusCode)
                    {
                        var responseMessage = httpResponse.Content.ReadAsStringAsync().Result;
                        if (!string.IsNullOrEmpty(responseMessage))
                        {
                            var apiResponse = JsonConvert.DeserializeObject<EVAResponseModel>(responseMessage);
                            if (apiResponse != null && apiResponse.Response != null && apiResponse.result)
                            {
                                var b= JsonConvert.SerializeObject(apiResponse.Response);
                                return JsonConvert.DeserializeObject<AccessTokenResponse>(b);
                            }
                            else if (apiResponse != null)
                            {
                                LogHelper.Instance.Debug($"Failed to generate STB access token with the below response from API :- {apiResponse.responseMessage}","",
                               functionName, applicationName, description);
                                return null;
                            }
                            else
                            {
                                LogHelper.Instance.Debug($"Failed to generate STB access token, since API returned NULL","",
                                functionName, applicationName, description);
                                return null;
                            }
                        }
                        else
                        {
                            LogHelper.Instance.Debug("Failed to generate STB access token with the below response from API, since API returned NULL","",
                                functionName, applicationName, description);
                            return null;
                        }
                    }
                    else
                    {
                        LogHelper.Instance.Debug($"Failed to generate STB access token with the below response from API, {httpResponse.ReasonPhrase} code:- {httpResponse.StatusCode}","",
                            functionName, applicationName, description);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Error(ex,"", functionName, applicationName, description);
                return null;
            }
        }
        private string GenerateToken()
        {
            string accessToken = string.Empty;
            var result = GetAccessToken(new EVARequestModel
            {
                webUrl = System.Configuration.ConfigurationManager.AppSettings["AccessTokenwebUrl"].ToString(),
                ClientId = System.Configuration.ConfigurationManager.AppSettings["ClientId"].ToString(),
                ClientSecert = System.Configuration.ConfigurationManager.AppSettings["ClientSecert"].ToString()
            });

            if (result != null && !string.IsNullOrEmpty(result.AccessToken))
            {
                accessToken = result.AccessToken;
            }

            return accessToken;
        }

        public VisitorCheckInResponse SendVisitorCheckin(EVARequestModel requestModel)
        {
            string functionName = "SendVisitorCheckin";
            string applicationName = "Backoffice";
            string description = "send checkin profile to STB EVA Interface.";
            try
            {
                using (var httpClient = new HttpClient())
                {

                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    HttpContent requestContent = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");
                    var jsondata = JsonConvert.SerializeObject(requestModel);
                    //For testing.
                   LogHelper.Instance.Debug("RequestJson : " + JsonConvert.SerializeObject(requestModel),"", functionName, applicationName, description);

                    httpClient.Timeout = TimeSpan.FromMinutes(2);
                    HttpResponseMessage httpResponse = httpClient.PostAsync(new Uri($"{ConfigurationHelper.Instance.GrabberConfig.LocalAPIURL}/STBEva/VisitorCheckin"), requestContent).Result;
                    if (httpResponse != null && httpResponse.IsSuccessStatusCode)
                    {
                        var responseMessage = httpResponse.Content.ReadAsStringAsync().Result;
                        if (!string.IsNullOrEmpty(responseMessage))
                        {
                            var apiResponse = JsonConvert.DeserializeObject<EVAResponseModel>(responseMessage);
                            if (apiResponse != null && apiResponse.Response != null && apiResponse.result)
                            {
                                return JsonConvert.DeserializeObject<VisitorCheckInResponse>(apiResponse.Response.ToString());
                            }
                            else if (apiResponse != null)
                            {
                                LogHelper.Instance.Debug($"Failed to send checkin profile with the below response from API :- {apiResponse.responseMessage}","",
                               functionName, applicationName, description);
                                return null;
                            }
                            else
                            {
                                LogHelper.Instance.Debug($"Failed to send checkin profile, since API returned NULL","",
                                functionName, applicationName, description);
                                return null;
                            }
                        }
                        else
                        {
                            LogHelper.Instance.Debug("Failed to send checkin profile with the below response from API, since API returned NULL",
                             "",   functionName, applicationName, description);
                            return null;
                        }
                    }
                    else
                    {
                        LogHelper.Instance.Debug($"Failed to send checkin profile with the below response from API, {httpResponse.ReasonPhrase} code:- {httpResponse.StatusCode}","",
                            functionName, applicationName, description);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Error(ex,"", functionName, applicationName, description);
                return null;
            }
        }

    }
}