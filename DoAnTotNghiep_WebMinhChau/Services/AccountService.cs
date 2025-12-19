using DoAnTotNghiep_WebMinhChau.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Data.SqlClient;

namespace DoAnTotNghiep_WebMinhChau.Services
{
    public class AccountService
    {
        // Kiểm tra email có tồn tại trong bảng TaiKhoan hay không
        public bool IsEmailExist(string email)
        {
            using (var db = new DBContext())
            {
                return db.TaiKhoan.Any(tk => tk.EmailKH == email);
            }
        }

        // Tạo mật khẩu ngẫu nhiên
        public string GenerateRandomPassword(int length = 10)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // Mã hóa mật khẩu bằng MD5
        public string HashPassword(string password)
        {
            string hashedPassword = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(password))).Replace("-", "");
            return hashedPassword;
        }

        // Cập nhật mật khẩu mới vào bảng TaiKhoan
        public void UpdatePassword(string email, string newPassword)
        {
            // Cập nhật mật khẩu trong bảng TaiKhoan
            using (var db = new DBContext())
            {
                var user = db.TaiKhoan.FirstOrDefault(tk => tk.EmailKH == email);
                if (user != null)
                {
                    // Cập nhật mật khẩu trong ứng dụng
                    user.MatKhau = newPassword;
                    db.SaveChanges();
                }
            }

            // Đổi mật khẩu trong SQL Server (đổi mật khẩu cho login SQL Server)
            try
            {
                string connectionString = "Server=LAPTOP-MK66GERQ;Database=DB_QLNhaThuocMC;Integrated Security=True;";
                string query = $"ALTER LOGIN [{email.Trim()}] WITH PASSWORD = '{newPassword}'";

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (nếu có)
                Console.WriteLine($"Lỗi khi đổi mật khẩu SQL Server: {ex.Message}");
            }
        }

        // Gửi email chứa mật khẩu mới
        public void SendNewPasswordEmail(string email, string newPassword)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("nhathuocminhchauk12@gmail.com", "vqnh wxvn bpsh owar"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("nhathuocminhchauk12@gmail.com"),
                Subject = "Your New Password",
                Body = $"Your new password is: {newPassword}",
                IsBodyHtml = false,
            };
            mailMessage.To.Add(email);

            smtpClient.Send(mailMessage);
        }

        // Phương thức ResetPassword tổng hợp
        public void ResetPassword(string userEmail)
        {
            if (IsEmailExist(userEmail))
            {
                // Tạo mật khẩu ngẫu nhiên và mã hóa mật khẩu mới
                string newPassword = GenerateRandomPassword();
                string hashedPassword = HashPassword(newPassword);

                // Cập nhật mật khẩu mới vào SQL và gửi email
                UpdatePassword(userEmail, hashedPassword);
                SendNewPasswordEmail(userEmail, newPassword);
            }
            else
            {
                Console.WriteLine("Email không tồn tại trong hệ thống.");
            }
        }
    }
}