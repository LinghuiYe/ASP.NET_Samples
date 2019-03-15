using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.DTO
{
    public class CustomerDTO
    {
        public long Id { get; set; }
        public string CustName { get; set; }
        public string CustEmail { get; set; }
    }
}