using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckinPortal.BackOffice.Models
{


    public class DataTableParameters
    {
        public List<DataTableColumn> Columns { get; set; }
        public int draw { get; set; }
        public int Length { get; set; }
        public List<DataOrder> Order { get; set; }
        public Search search { get; set; }
        public int Start { get; set; }


    }

    public class Search
    {
        public bool Regex { get; set; }
        public string Value { get; set; }
    }

    public class DataTableColumn
    {
        public int Data { get; set; }
        public string Name { get; set; }
        public bool Orderable { get; set; }
        public bool Searchable { get; set; }
        public Search Search { get; set; }

    }

    public class DapperResult
    {
        public string Dapper { get; set; }
        public AdditionaliFileds AdditionaliFileds { get; set; }
    }

    public class AdditionaliFileds
    {
        public string FieldName  { get; set; }
        public string FieldValue  { get; set; }
    }

    public class DataOrder
    {
        public int Column { get; set; }
        public string Dir { get; set; }


    }


}