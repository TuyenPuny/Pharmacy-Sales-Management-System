using DoAnTotNghiep_WebMinhChau.Models;
using System.Collections.Specialized;
using System.Web;

public interface IVnPayServers
{
    VnPaymentResponseModel PaymentExecute(NameValueCollection collections, string hashSecret);
    string CreatePaymentUrl(HttpContextBase context, VnPaymentRequestModel model);
}

