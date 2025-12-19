using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Ink;

namespace DoAnTotNghiep_WebMinhChau.Areas.Admin.Controllers
{
    public class BackupController : Controller
    {
        private readonly string connectionString = "Server=LAPTOP-MK66GERQ;Database=master;Trusted_Connection=True;";
        private readonly string backupFolderPath = @"E:\DoAnTotNghiep-NhaThuocMinhChau\SaoLuu_PhucHoi"; // Thư mục chứa file backup

        // Tạo sao lưu cơ sở dữ liệu
        [HttpPost]
        public ActionResult CreateBackup()
        {
            // Cố định tên file backup
            string backupFileName = "DBQLT_Backup.bak";

            try
            {
                string backupFilePath = Path.Combine(backupFolderPath, backupFileName);

                // Kiểm tra thư mục backup có tồn tại hay không, nếu không thì tạo mới
                if (!Directory.Exists(backupFolderPath))
                {
                    Directory.CreateDirectory(backupFolderPath);
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Lệnh sao lưu cơ sở dữ liệu
                    string backupCommand = $@"
                        BACKUP DATABASE DB_QLNhaThuocMC
                        TO DISK = '{backupFilePath}'
                        WITH INIT, COMPRESSION;";

                    using (SqlCommand command = new SqlCommand(backupCommand, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                ViewBag.BackupStatus = "Sao lưu cơ sở dữ liệu thành công! File đã được lưu tại: " + backupFilePath;
            }
            catch (Exception ex)
            {
                ViewBag.BackupStatus = "Lỗi xảy ra trong quá trình sao lưu: " + ex.Message;
            }

            return View("Index");
        }

        // Phục hồi cơ sở dữ liệu
        [HttpPost]
        public ActionResult RestoreDatabase(HttpPostedFileBase backupFile)
        {
            if (backupFile == null || backupFile.ContentLength == 0)
            {
                ViewBag.RestoreStatus = "Vui lòng chọn file sao lưu để phục hồi!";
                return View("Index");
            }

            try
            {
                // Lưu file sao lưu được tải lên vào thư mục tạm thời
                string restoreFilePath = Path.Combine(backupFolderPath, backupFile.FileName);
                backupFile.SaveAs(restoreFilePath);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Đặt cơ sở dữ liệu về chế độ SINGLE_USER để ngắt các kết nối
                    string setSingleUserCommand = @"
                ALTER DATABASE DB_QLNhaThuocMC SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
            ";
                    using (SqlCommand singleUserCommand = new SqlCommand(setSingleUserCommand, connection))
                    {
                        singleUserCommand.ExecuteNonQuery();
                    }

                    // Lệnh phục hồi cơ sở dữ liệu
                    string restoreCommand = $@"
                RESTORE DATABASE DB_QLNhaThuocMC
                FROM DISK = '{restoreFilePath}'
                WITH REPLACE,
                     MOVE 'DBQLT_Primary' TO '{backupFolderPath}\DBQLT_primary.mdf',
                     MOVE 'DBQLT_Log' TO '{backupFolderPath}\DBQLT_Log.ldf';
            ";
                    using (SqlCommand command = new SqlCommand(restoreCommand, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Chuyển cơ sở dữ liệu về chế độ MULTI_USER sau khi phục hồi
                    string setMultiUserCommand = @"
                ALTER DATABASE DB_QLNhaThuocMC SET MULTI_USER;
            ";
                    using (SqlCommand multiUserCommand = new SqlCommand(setMultiUserCommand, connection))
                    {
                        multiUserCommand.ExecuteNonQuery();
                    }
                }

                ViewBag.RestoreStatus = "Phục hồi cơ sở dữ liệu thành công!";
            }
            catch (Exception ex)
            {
                ViewBag.RestoreStatus = "Lỗi xảy ra trong quá trình phục hồi: " + ex.Message;
            }

            return View("Index");
        }

        private bool RestoreDatabaseFromFile(string restoreFilePath)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Đặt cơ sở dữ liệu vào chế độ SINGLE_USER để ngắt các kết nối khác
                    string setSingleUserCommand = @"
                ALTER DATABASE DB_QLNhaThuocMC SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
            ";
                    using (SqlCommand singleUserCommand = new SqlCommand(setSingleUserCommand, connection))
                    {
                        singleUserCommand.ExecuteNonQuery();
                    }

                    // Lệnh RESTORE DATABASE
                    string restoreCommand = $@"
                RESTORE DATABASE DB_QLNhaThuocMC
                FROM DISK = '{restoreFilePath}'
                WITH REPLACE,
                     MOVE 'DBQLT_Primary' TO 'E:\DoAnTotNghiep-NhaThuocMinhChau\SaoLuu_PhucHoi\DBQLT_primary.mdf',
                     MOVE 'DBQLT_Log' TO 'E:\DoAnTotNghiep-NhaThuocMinhChau\SaoLuu_PhucHoi\DBQLT_Log.ldf';
            ";
                    using (SqlCommand restoreCmd = new SqlCommand(restoreCommand, connection))
                    {
                        restoreCmd.ExecuteNonQuery();
                    }

                    // Chuyển cơ sở dữ liệu về chế độ MULTI_USER
                    string setMultiUserCommand = @"
                ALTER DATABASE DB_QLNhaThuocMC SET MULTI_USER;
            ";
                    using (SqlCommand multiUserCommand = new SqlCommand(setMultiUserCommand, connection))
                    {
                        multiUserCommand.ExecuteNonQuery();
                    }
                }

                return true; // Khôi phục thành công
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during restore: " + ex.Message);
                return false; // Lỗi xảy ra
            }
        }


        // Trang Index để hiển thị giao diện sao lưu và phục hồi
        public ActionResult Index()
        {
            return View();
        }
    }
}
