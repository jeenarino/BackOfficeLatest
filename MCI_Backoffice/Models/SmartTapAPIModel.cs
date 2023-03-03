using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckinPortal.BackOffice.Models
{
    public class SmartTapResponseModel
    {

        
            public string Code { get; set; }
            public object Data { get; set; }
            public string Exception { get; set; }
            public string Message { get; set; }
            public bool Result { get; set; }
        

    }
    public class PagingRequestModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string search { get; set; }
        public string Sort { get; set; }
        public string SortBy { get; set; }
        //public int Id { get; set; }
    }
    public class SmartTapAPIRequestModel
    {
      
        public object RequestObject { get; set; }

    }
    public class DeleteRequestModel
    {
        public int id { get; set; }
    }
    public class CumulativeReportRequestModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string search { get; set; }
        public string Sort { get; set; }
        public string SortBy { get; set; }
        public int? OutletId { get; set; }
        public string FunctionalityId { get; set; }
        public bool IsPDF { get; set; }
    }
    public class GetRequestModel
    {
        public int? id { get; set; }
    }
}