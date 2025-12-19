using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnTotNghiep_WebMinhChau.Models
{
    public class VnPayConfig
    {
        public VnPay VnPay { get; set; }
    }

    public class VnPay
    {
        public string vnp_TmnCode { get; set; }
        public string vnp_HashSecret { get; set; }
        public string Base_Url { get; set; }
        public string Version { get; set; }
        public string Command { get; set; }
        public string CurrCode { get; set; }
        public string Locale { get; set; }
        public string PaymentBackReturnUrl { get; set; }
    }
}