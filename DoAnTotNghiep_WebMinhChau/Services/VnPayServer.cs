using DoAnTotNghiep_WebMinhChau.Helper;
using DoAnTotNghiep_WebMinhChau.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Web;

namespace DoAnTotNghiep_WebMinhChau.Server
{
    public class VnPayServer : IVnPayServers
    {
        private readonly NameValueCollection _someConfigValue;

        public static object LogManager { get; private set; }

        //internal static ILog Log { get; } = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public VnPayServer()
        {
            // Lấy giá trị cấu hình từ web.config
            _someConfigValue = ConfigurationManager.AppSettings;
        }

        public string CreatePaymentUrl(HttpContextBase context, VnPaymentRequestModel model)
        {
            var tick = DateTime.Now.Ticks.ToString();
            var vnpay = new VnPayLibrary();

            //Thêm dữ liệu yêu cầu vào vnpay
            vnpay.AddRequestData("vnp_Version", _someConfigValue["VnPay:Version"]);
            vnpay.AddRequestData("vnp_Command", _someConfigValue["VnPay:Command"]);
            vnpay.AddRequestData("vnp_TmnCode", _someConfigValue["VnPay:TmnCode"]);
            vnpay.AddRequestData("vnp_Amount", (model.Amount * 100).ToString());
            vnpay.AddRequestData("vnp_CreateDate", model.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", _someConfigValue["VnPay:CurrCode"]);
            vnpay.AddRequestData("vnp_IpAddr", GetIpAddress(context)); // Sử dụng HttpContextBase

            vnpay.AddRequestData("vnp_Locale", _someConfigValue["VnPay:Locale"]);
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán cho đơn hàng:" + model.OrderId);
            vnpay.AddRequestData("vnp_OrderType", "other");
            vnpay.AddRequestData("vnp_ReturnUrl", _someConfigValue["VnPay:PaymentCallBack"]);
            vnpay.AddRequestData("vnp_TxnRef", tick);

            // Tạo URL thanh toán
            var paymentUrl = vnpay.CreateRequestUrl(_someConfigValue["VnPay:BaseUrl"], _someConfigValue["VnPay:HashSecret"]);
            return paymentUrl;
        }


        private string GetIpAddress(HttpContextBase context)
        {
            var ipAddress = string.Empty;

            try
            {
                // Lấy địa chỉ IP từ các biến máy chủ
                ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(ipAddress))
                {
                    ipAddress = context.Request.ServerVariables["REMOTE_ADDR"];
                }

                // Nếu có danh sách IP trong "HTTP_X_FORWARDED_FOR", lấy địa chỉ IP đầu tiên
                if (!string.IsNullOrEmpty(ipAddress) && ipAddress.Contains(","))
                {
                    ipAddress = ipAddress.Split(',').First();
                }
                return ipAddress;
            }
            catch (Exception ex)
            {
                return "Invalid IP: " + ex.Message;
            }

            //return string.IsNullOrEmpty(ipAddress) ? "127.0.0.1" : ipAddress;
        }
        public VnPaymentResponseModel PaymentExecute(NameValueCollection queryString, string hashSecret)
        {
            var vnpay = new VnPayLibrary();

            // Populate the response data from the query string
            foreach (string key in queryString.AllKeys)
            {
                var value = queryString[key];
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }

            // Retrieve specific VNPay response data
            var vnp_orderId = (vnpay.GetResponseData("vnp_TxnRef"));
            var vnp_TransactionId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");
            var vnp_SecureHash = queryString["vnp_SecureHash"];

            // Validate the signature
            bool isSignatureValid = vnpay.ValidateSignature(vnp_SecureHash, _someConfigValue["VnPay:HashSecret"]);
            if (isSignatureValid)
            {

                return new VnPaymentResponseModel
                {
                    Success = false
                };
                // Return null or a failure response if the signature is invalid
            }

            // Return the payment response model if the signature is valid
            return new VnPaymentResponseModel
            {
                Success = true,
                PaymentMethod = "VnPay",
                OrderDescription = vnp_OrderInfo,
                OrderId = vnp_orderId.ToString(),
                TransactionId = vnp_TransactionId.ToString(),
                Token = vnp_SecureHash,
                VnPayResponseCode = vnp_ResponseCode.ToString(),
            };
        }
    }
}
