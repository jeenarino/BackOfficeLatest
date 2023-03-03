using CheckinPortal.BackOffice.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace CheckinPortal.BackOffice.BusinessLogic
{
    public class CloudMastersLogic
    {
        public Helpers.HttpClientHelper httpHelper { get; set; }
        private string baseURL;
        public CloudMastersLogic()
        {
            baseURL = ConfigurationManager.AppSettings["CloudAPIURL"].ToString();
            httpHelper = new Helpers.HttpClientHelper(baseURL);
        }

        #region Package Master calls
     
        public async Task<List<CloudPackageMasterModel>> GetPackageMasterList(int? PackageID)
        {
            Models.CloudPackageRequsetModel cloudPackageRequsetModel = new CloudPackageRequsetModel()
            {
                IsFromCloud = true,
                RequestObject = PackageID != null ? PackageID.Value : 0
            };

            var responseMessage = await httpHelper.PushDataToCloud(cloudPackageRequsetModel, baseURL + "/cloud/FetchPackageMaster");

            if (responseMessage != null)
            {
                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    var cloudResponseModel = JsonConvert.DeserializeObject<CloudResponseModel>(responseString);
                    if(cloudResponseModel != null && cloudResponseModel.responseData != null)
                    {
                        var roomList = JsonConvert.DeserializeObject<List<CloudPackageMasterModel>>(cloudResponseModel.responseData.ToString());
                        return roomList;
                    }
                }
            }
            return new List<CloudPackageMasterModel>();
        }

        public async Task<CloudPackageMasterModel> GetPackageMasterByID(int id)
        {
            var responseMessage = await httpHelper.GetDataFromCloud(baseURL + $"/cloud/GetPackageMasterList?PackageID={id}");
            if (responseMessage != null)
            {
                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    var cloudResponseModel = JsonConvert.DeserializeObject<CloudResponseModel>(responseString);
                    if (cloudResponseModel != null && cloudResponseModel.responseData != null)
                    {
                        var package = JsonConvert.DeserializeObject<CloudPackageMasterModel>(cloudResponseModel.responseData.ToString());
                        return package;
                    }
                }
            }
            return new CloudPackageMasterModel();
        }

        public async Task<bool> InsetUpdatePackageMaster(CloudPackageRequsetModel cloudPackageMasterModel)
        {
            var responseMessage = await httpHelper.PushDataToCloud(cloudPackageMasterModel, baseURL + "/cloud/InsertPackageMaster");
            if (responseMessage != null)
            {
                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    var cloudResponseModel = JsonConvert.DeserializeObject<CloudResponseModel>(responseString);
                    if (cloudResponseModel != null )
                    {
                        return cloudResponseModel.result;
                    }
                }
            }
            return false;
        }

        public async Task<bool> DeactivatePackage(CloudPackageRequsetModel cloudPackageMasterModel)
        {
            var responseMessage = await httpHelper.PushDataToCloud(cloudPackageMasterModel, baseURL + "/cloud/InsertPackageMaster");
            if (responseMessage != null)
            {
                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    var cloudResponseModel = JsonConvert.DeserializeObject<CloudResponseModel>(responseString);
                    if (cloudResponseModel != null)
                    {
                        return cloudResponseModel.result;
                    }
                }
            }
            return false;
        }

        #endregion
        public async Task<List<RoomTypeMasterModel>> GetRoomTypeMaster()
        {
            //var roomList1 = new List<RoomTypeMasterModel>();
            //roomList1.Add(new RoomTypeMasterModel() { RoomTypeCode = "BKO", RoomTypeDescription = "BKO", RoomTypeID = 1 });
            //roomList1.Add(new RoomTypeMasterModel() { RoomTypeCode = "BTO", RoomTypeDescription = "BTO", RoomTypeID = 1 });
            //roomList1.Add(new RoomTypeMasterModel() { RoomTypeCode = "LKO", RoomTypeDescription = "LKO", RoomTypeID = 1 });
            //return roomList1;

            var responseMessage = await httpHelper.PushDataToCloud("", baseURL + "/cloud/FetchRoomTypeMaster");
            if (responseMessage != null)
            {
                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    var cloudResponseModel = JsonConvert.DeserializeObject<CloudResponseModel>(responseString);
                    if (cloudResponseModel != null && cloudResponseModel.responseData != null)
                    {
                        var roomList = JsonConvert.DeserializeObject<List<RoomTypeMasterModel>>(cloudResponseModel.responseData.ToString());
                        return roomList;
                    }
                }
            }
            return new List<RoomTypeMasterModel>();
        }

        public async Task<List<RoomTypeMasterModel>> InsetUpdateRoomTypeMaster()
        {
            var responseMessage = await httpHelper.GetDataFromCloud(baseURL + "/cloud/InsetUpdateRoomTypeMaster");
            if (responseMessage != null)
            {
                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    var cloudResponseModel = JsonConvert.DeserializeObject<CloudResponseModel>(responseString);
                    if (cloudResponseModel != null && cloudResponseModel.responseData != null)
                    {
                        var roomList = JsonConvert.DeserializeObject<List<RoomTypeMasterModel>>(cloudResponseModel.responseData.ToString());
                        return roomList;
                    }
                }
            }
            return new List<RoomTypeMasterModel>();
        }

        public async Task<List<RoomTypeMasterModel>> DeleteRoomTypeMaster()
        {

            var responseMessage = await httpHelper.GetDataFromCloud(baseURL + "/cloud/DeleteRoomTypeMaster");
            if (responseMessage != null)
            {
                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    var cloudResponseModel = JsonConvert.DeserializeObject<CloudResponseModel>(responseString);
                    if (cloudResponseModel != null && cloudResponseModel.responseData != null)
                    {
                        var roomList = JsonConvert.DeserializeObject<List<RoomTypeMasterModel>>(cloudResponseModel.responseData.ToString());
                        return roomList;
                    }
                }
            }
            return new List<RoomTypeMasterModel>();
        }

        public async Task<CloudResponseModel> FetchPrecheckinReservationStatus(string confirmationNumber)
        {
            var cloudResponseModel = new CloudResponseModel()
            {
                result = false
            };
            var responseMessage = await httpHelper.PushDataToCloud(new { RequestObject = confirmationNumber }, baseURL + $"/cloud/FetchReservationStatusInCloud");

            if (responseMessage != null)
            {
                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    cloudResponseModel = JsonConvert.DeserializeObject<CloudResponseModel>(responseString);
                    if (cloudResponseModel != null && cloudResponseModel.responseData != null)
                    {
                        var package = JsonConvert.DeserializeObject<List<FetchResrvationStatusModel>>(cloudResponseModel.responseData.ToString());
                        cloudResponseModel.responseData = package;
                    }
                }
            }
            return cloudResponseModel;
        }


    }

}