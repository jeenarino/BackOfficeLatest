using MCIGrabberService.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace CheckinPortal.BackOffice.BusinessLogic
{
    public class ReservationLogics
    {
        public async Task<bool> syncReservationFromOpera(string ConfirmationNumber)
        {
            try
            {

                OwsHelper owsHelper = new OwsHelper();
                LocalAPICallsHelper localAPICallsHelper = new LocalAPICallsHelper();

                var ReservationResponse = await owsHelper.FetchReservation(ConfirmationNumber);
                if (ReservationResponse != null && ReservationResponse.result)
                {
                    var syncResult = await localAPICallsHelper.PushReservationDetails(ReservationResponse);
                    if (syncResult != null && syncResult.result)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return false;
        }

        public async Task<Models.OwsModels.ResponseData> FetchReservationFromOpera(string ConfirmationNumber)
        {
            OwsHelper owsHelper = new OwsHelper();
            LocalAPICallsHelper localAPICallsHelper = new LocalAPICallsHelper();

            var ReservationResponse = await owsHelper.FetchReservation(ConfirmationNumber);
            if (ReservationResponse != null && ReservationResponse.responseData != null)
            {
                var operaReservation = ReservationResponse.responseData;// Newtonsoft.Json.JsonConvert.DeserializeObject<Models.OperaReservation>(ReservationResponse.responseData.ToString());
                return operaReservation[0];
            }
            else
            {
                return new  Models.OwsModels.ResponseData()
                {
                     
                };
            }
        }

        public async Task<bool> GetOperaBusinessDate(string userName,string password)
        {
            try
            {

                OwsHelper owsHelper = new OwsHelper();
                LocalAPICallsHelper localAPICallsHelper = new LocalAPICallsHelper();

                var ReservationResponse = await owsHelper.GetOperaBusinessDate(userName,password);
                if (ReservationResponse != null)
                {
                    return ReservationResponse.result;
                }
                return false;
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllLines(System.Web.HttpContext.Current.Server.MapPath("~/log22.txt"), new string[] { $"GetOperaBusinessDate exception {ex.ToString()}" });
                return false;
            }
        }

      

    }
}