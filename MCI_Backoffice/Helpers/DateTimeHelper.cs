using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckinPortal.BackOffice.Helpers
{
    public class DateTimeHelper
    {
        public static string ConvertFromUTC(string Time)
        {
            string ExpectedTimeofArrival = "";
            if (!string.IsNullOrEmpty(Time))
            {
                string[] ETaStringArray = Time.Split(':');
                if (ETaStringArray.Length > 1)
                {
                    DateTime timeUtc = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day,
                        Convert.ToInt32(ETaStringArray[0]),
                        Convert.ToInt32(ETaStringArray[1]), 0);
                    string HotelTimeZone = System.Configuration.ConfigurationManager.AppSettings["HotelTimeZone"].ToString();
                    TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById(HotelTimeZone);
                    DateTime sgtTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);
                    ExpectedTimeofArrival = sgtTime.ToString("HH:mm");
                }
            }
            return ExpectedTimeofArrival;
        }
    }
}