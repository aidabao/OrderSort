using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderSort.Models
{
    public class ResponseDTO<T>:PageDTO
    {
        public int code { get; set; }
        public string msg { get; set; }
        public T data { get; set; }
    }
    public class PageDTO
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int count { get; set; }
    }
    public class Order
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Sku { get; set; }
        public string Size { get; set; }
        public int Num { get; set; }
        public int Total { get; set; }
    }
}