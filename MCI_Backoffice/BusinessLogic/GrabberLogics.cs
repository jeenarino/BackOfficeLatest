using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CheckinPortal.BackOffice.Helpers;
using MCIGrabberService;
using Newtonsoft.Json;

namespace CheckinPortal.BackOffice.BusinessLogic
{
    public class GrabberLogics
    {
        public static async Task<Models.CloudResponseModel> FetchPreCheckionReservationStatus(string ConfirmationNumber)
        {
            
            CloudMastersLogic localHelper = new CloudMastersLogic();
            return  await localHelper.FetchPrecheckinReservationStatus(ConfirmationNumber);
        }

        public static async Task<Models.LocalResponseModel> FetchPreCheckinReservation(string ConfirmationNumber)
        {
            LocalAPICallsHelper localHelper = new LocalAPICallsHelper();
            return await localHelper.FetchPreCheckedinReservationByReservationNumber(ConfirmationNumber);
        }

        public static async Task<Models.LocalResponseModel> FetchPreCheckoutReservation(string ConfirmationNumber)
        {
            LocalAPICallsHelper localHelper = new LocalAPICallsHelper();
            return await localHelper.FetchPreCheckedoutReservationByReservationNumber(ConfirmationNumber);
        }

        public static async Task<Models.LocalResponseModel> PushDueOutReservation(string ConfirmationNumber)
        {
            LocalAPICallsHelper localHelper = new LocalAPICallsHelper();
            return await localHelper.PushDueOutReservation(ConfirmationNumber);
        }

        public static async Task<Models.LocalResponseModel> PushDueInReservation(string ConfirmationNumber)
        {
            LocalAPICallsHelper localHelper = new LocalAPICallsHelper();
            return await localHelper.PushDueIntReservation(ConfirmationNumber);
        }
    }
}