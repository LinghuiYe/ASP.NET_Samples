using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApi.Models
{
    public class StandardViewModel
    {
        public int StandardId { get; set; }
        public string StandardName { get; set; }

        public ICollection<StudentViewModel> Students { get; set; }
    }
}