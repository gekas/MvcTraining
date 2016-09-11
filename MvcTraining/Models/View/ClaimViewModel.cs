using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcTraining.Models.View
{
    public class ClaimViewModel
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; }
        public string Issuer { get; set; }
    }
}