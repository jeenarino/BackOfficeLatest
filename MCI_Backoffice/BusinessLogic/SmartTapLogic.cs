using CheckinPortal.BackOffice.Models;
using CheckinPortal.BackOffice.Models.SmartTap;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CheckinPortal.BackOffice.BusinessLogic
{
    public class SmartTapLogic
    {
        public Helpers.HttpClientHelper httpHelper { get; set; }
        private string baseURL;
        public SmartTapLogic()
        {
            baseURL = ConfigurationManager.AppSettings["SmartTapURL"].ToString();
            httpHelper = new Helpers.HttpClientHelper(baseURL);
        }
        public  async Task<Models.SmartTapResponseModel> GetOutlets(PagingRequestModel RequestObject)
        {
            var responseMessage = await httpHelper.PushDataToSmartTapCloud(RequestObject, baseURL + "/fetchOutletPageList");
            if (responseMessage != null)
            {
                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    var SmartTapResponseModel = JsonConvert.DeserializeObject<SmartTapResponseModel>(responseString);
                    if (SmartTapResponseModel != null)
                    {
                        if(SmartTapResponseModel.Data!=null)
                        SmartTapResponseModel.Data = JsonConvert.DeserializeObject<List<Outlet>>(SmartTapResponseModel.Data.ToString());
                        return SmartTapResponseModel;
                    }
                }
            }
            return new SmartTapResponseModel();
        }
        public  async Task<Models.SmartTapResponseModel> GetOutletUsers(PagingRequestModel RequestObject)
        {
            var responseMessage = await httpHelper.PushDataToSmartTapCloud(RequestObject, baseURL + "/fetchOutletUserPageList");
            if (responseMessage != null)
            {
                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    var SmartTapResponseModel = JsonConvert.DeserializeObject<SmartTapResponseModel>(responseString);
                    if (SmartTapResponseModel != null )
                    {
                        if(SmartTapResponseModel.Data != null)
                        SmartTapResponseModel.Data = JsonConvert.DeserializeObject<List<OutletUser>>(SmartTapResponseModel.Data.ToString());
                        return SmartTapResponseModel;
                    }
                }
            }
            return null;
        }
        public  async Task<Models.SmartTapResponseModel> GetPackages(PagingRequestModel RequestObject)
        {
            var responseMessage = await httpHelper.PushDataToSmartTapCloud(RequestObject, baseURL + "/fetchPackagePageList");
            if (responseMessage != null)
            {
                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    var SmartTapResponseModel = JsonConvert.DeserializeObject<SmartTapResponseModel>(responseString);
                    if (SmartTapResponseModel != null)
                    {
                        if(SmartTapResponseModel.Data!=null)
                        SmartTapResponseModel.Data = JsonConvert.DeserializeObject<List<GeneralPackages>>(SmartTapResponseModel.Data.ToString());
                        return SmartTapResponseModel;
                    }
                }
            }
            return null;
        }
        public  async Task<Models.SmartTapResponseModel> GetGeneralSetings()
        {
            var responseMessage = await httpHelper.GetDataFromCloud(baseURL + "/fetchGeneralSetting");
            if (responseMessage != null)
            {
                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    var SmartTapResponseModel = JsonConvert.DeserializeObject<SmartTapResponseModel>(responseString);
                    if (SmartTapResponseModel != null)
                    {
                        if(SmartTapResponseModel.Data!=null)
                            SmartTapResponseModel.Data = JsonConvert.DeserializeObject<List<GeneralSetting>>(SmartTapResponseModel.Data.ToString()).FirstOrDefault();
                        return SmartTapResponseModel;
                    }
                }
            }
            return null;
        }
        public async Task<Models.SmartTapResponseModel> InsertOulet(Outlet RequestObject)
        {
            var responseMessage = await httpHelper.PushDataToSmartTapCloud(RequestObject, baseURL + "/InsertOutlet");
            if (responseMessage != null)
            {
                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    var SmartTapResponseModel = JsonConvert.DeserializeObject<SmartTapResponseModel>(responseString);
                    if (SmartTapResponseModel != null)
                    {
                        return SmartTapResponseModel;
                    }
                }
            }
            return null;
        }
        public async Task<Models.SmartTapResponseModel> InsertOuletUser(OutletUser RequestObject)
        {
            var responseMessage = await httpHelper.PushDataToSmartTapCloud(RequestObject, baseURL + "/InsertOutletUser");
            if (responseMessage != null)
            {
                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    var SmartTapResponseModel = JsonConvert.DeserializeObject<SmartTapResponseModel>(responseString);
                    if (SmartTapResponseModel != null)
                    {
                        return SmartTapResponseModel;
                    }
                }
            }
            return null;
        }
        public async Task<Models.SmartTapResponseModel> InsertGeneralPackage(GeneralPackages RequestObject)
        {
            var responseMessage = await httpHelper.PushDataToSmartTapCloud(RequestObject, baseURL + "/InsertGeneralPackage");
            if (responseMessage != null)
            {
                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    var SmartTapResponseModel = JsonConvert.DeserializeObject<SmartTapResponseModel>(responseString);
                    if (SmartTapResponseModel != null)
                    {
                        return SmartTapResponseModel;
                    }
                }
            }
            return null;
        }
        public async Task<Models.SmartTapResponseModel> InsertGeneralSettings(GeneralSetting RequestObject)
        {
            var responseMessage = await httpHelper.PushDataToSmartTapCloud(RequestObject, baseURL + "/InsertGeneralSettings");
            if (responseMessage != null)
            {
                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    var SmartTapResponseModel = JsonConvert.DeserializeObject<SmartTapResponseModel>(responseString);
                    if (SmartTapResponseModel != null)
                    {
                        return SmartTapResponseModel;
                    }
                }
            }
            return null;
        }
        public async Task<Models.SmartTapResponseModel> InActiveOutlet(DeleteRequestModel RequestObject)
        {
            var responseMessage = await httpHelper.PushDataToSmartTapCloud(RequestObject, baseURL + "/InActiveOutlet");
            if (responseMessage != null)
            {
                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    var SmartTapResponseModel = JsonConvert.DeserializeObject<SmartTapResponseModel>(responseString);
                    if (SmartTapResponseModel != null)
                    {
                        return SmartTapResponseModel;
                    }
                }
            }
            return null;
        }
        public async Task<Models.SmartTapResponseModel> InActiveOutletUsers(DeleteRequestModel RequestObject)
        {
            var responseMessage = await httpHelper.PushDataToSmartTapCloud(RequestObject, baseURL + "/InActiveOutletUsers");
            if (responseMessage != null)
            {
                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    var SmartTapResponseModel = JsonConvert.DeserializeObject<SmartTapResponseModel>(responseString);
                    if (SmartTapResponseModel != null)
                    {
                        return SmartTapResponseModel;
                    }
                }
            }
            return null;
        }

        public async Task<Models.SmartTapResponseModel> InActivePackages(DeleteRequestModel RequestObject)
        {
            var responseMessage = await httpHelper.PushDataToSmartTapCloud(RequestObject, baseURL + "/InActivePackages");
            if (responseMessage != null)
            {
                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    var SmartTapResponseModel = JsonConvert.DeserializeObject<SmartTapResponseModel>(responseString);
                    if (SmartTapResponseModel != null)
                    {
                        return SmartTapResponseModel;
                    }
                }
            }
            return null;
        }

        public async Task<Models.SmartTapResponseModel> GetCumulativeReport(CumulativeReportRequestModel RequestObject)
        {
            var responseMessage = await httpHelper.PushDataToSmartTapCloud(RequestObject, baseURL + "/GetCumulativeReport");
            if (responseMessage != null)
            {
                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    var SmartTapResponseModel = JsonConvert.DeserializeObject<SmartTapResponseModel>(responseString);
                    if (SmartTapResponseModel != null)
                    {
                        if (SmartTapResponseModel.Data != null)
                        {
                            SmartTapResponseModel.Data = JsonConvert.DeserializeObject<List<CumulativeReponseModel>>(SmartTapResponseModel.Data.ToString());
                        }
                        return SmartTapResponseModel;
                    }
                }
            }
            return null;
        }
        public async Task<Models.SmartTapResponseModel> GetOutletById(GetRequestModel RequestObject)
        {
            var responseMessage = await httpHelper.GetDataFromCloud(baseURL + "/fetchOutletById?Id=" + RequestObject.id);
            if (responseMessage != null)
            {
                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    var SmartTapResponseModel = JsonConvert.DeserializeObject<SmartTapResponseModel>(responseString);
                    if (SmartTapResponseModel != null)
                    {
                        SmartTapResponseModel.Data = JsonConvert.DeserializeObject<List<Outlet>>(SmartTapResponseModel.Data.ToString()).FirstOrDefault();
                        return SmartTapResponseModel;
                    }
                }
            }
            return new SmartTapResponseModel();
        }
        public async Task<Models.SmartTapResponseModel> GetOutlets(GetRequestModel RequestObject)
        {
            var responseMessage = await httpHelper.GetDataFromCloud(baseURL + "/fetchOutletById?Id=" + RequestObject.id);
            if (responseMessage != null)
            {
                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    var SmartTapResponseModel = JsonConvert.DeserializeObject<SmartTapResponseModel>(responseString);
                    if (SmartTapResponseModel != null)
                    {
                        SmartTapResponseModel.Data = JsonConvert.DeserializeObject<List<Outlet>>(SmartTapResponseModel.Data.ToString());
                        return SmartTapResponseModel;
                    }
                }
            }
            return new SmartTapResponseModel();
        }
        public async Task<Models.SmartTapResponseModel> GetPackageById(GetRequestModel RequestObject)
        {
            var responseMessage = await httpHelper.GetDataFromCloud(baseURL + "/fetchPackageById?Id=" + RequestObject.id);
            if (responseMessage != null)
            {
                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    var SmartTapResponseModel = JsonConvert.DeserializeObject<SmartTapResponseModel>(responseString);
                    if (SmartTapResponseModel != null)
                    {
                        SmartTapResponseModel.Data = JsonConvert.DeserializeObject<List<GeneralPackages>>(SmartTapResponseModel.Data.ToString()).FirstOrDefault();
                        return SmartTapResponseModel;
                    }
                }
            }
            return new SmartTapResponseModel();
        }
        public async Task<Models.SmartTapResponseModel> GetOutletUserById(GetRequestModel RequestObject)
        {
            var responseMessage = await httpHelper.GetDataFromCloud(baseURL + "/fetchOutletUserById?Id=" + RequestObject.id);
            if (responseMessage != null)
            {
                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    var SmartTapResponseModel = JsonConvert.DeserializeObject<SmartTapResponseModel>(responseString);
                    if (SmartTapResponseModel != null)
                    {
                        SmartTapResponseModel.Data = JsonConvert.DeserializeObject<List<OutletUser>>(SmartTapResponseModel.Data.ToString()).FirstOrDefault();
                        return SmartTapResponseModel;
                    }
                }
            }
            return new SmartTapResponseModel();
        }
        public async Task<Models.SmartTapResponseModel> GetOutletsByUser(int Id)
        {
            var responseMessage = await httpHelper.GetDataFromCloud(baseURL + "/fetchOutletByUserId?Id=" + Id);
            if (responseMessage != null)
            {
                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    var SmartTapResponseModel = JsonConvert.DeserializeObject<SmartTapResponseModel>(responseString);
                    if (SmartTapResponseModel != null)
                    {
                        SmartTapResponseModel.Data = JsonConvert.DeserializeObject<List<Outlet>>(SmartTapResponseModel.Data.ToString());
                        return SmartTapResponseModel;
                    }
                }
            }
            return new SmartTapResponseModel();
        }


    }
}