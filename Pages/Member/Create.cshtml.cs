using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace ClientsCRUD.Pages.Member
{
    public class CreateModel : PageModel
    {
        public MemInfo mInfo = new MemInfo();
        private readonly string _connectStr;  //不了解這行待研究 private string connStr;

        public CreateModel(IConfiguration config1) 
        {
            _connectStr = config1.GetConnectionString("connDB");
        }

        public void OnGet()
        {
        }

         public string errorMessage = "";
         public string succMessage = "";
         public void OnPost()
            {
                mInfo.memName = Request.Form["name"];
                mInfo.memPhone = Request.Form["phone"];
                mInfo.memEmail = Request.Form["email"];
                mInfo.memBirth = Request.Form["birth"];
                mInfo.memRemark = Request.Form["remark"];

                if (mInfo.memName.Length == 0 || mInfo.memEmail.Length == 0 ||
                    mInfo.memPhone.Length == 0 || mInfo.memRemark.Length == 0)
                {
                    errorMessage = "請填完整(*)必填的資料內容!";
                    return;
                }
                

                // Save to Database
                try
                {
                string connStr = _connectStr;   //不了解這行待研究 
                SqlConnection conn = new(connStr);
                    conn.Open();
                    string sqlStr = "INSERT INTO tbMembers " +
                           "(memName, memPhone, memEmail, memBirth, memRemark) VALUES " +
                           "(@name, @phone, @email, @birth, @remark);";

                    SqlCommand cmd = new(sqlStr, conn);
                    cmd.Parameters.AddWithValue("name", mInfo.memName);
                    cmd.Parameters.AddWithValue("phone", mInfo.memPhone);
                    cmd.Parameters.AddWithValue("email", mInfo.memEmail);
                    cmd.Parameters.AddWithValue("birth", mInfo.memBirth);
                    cmd.Parameters.AddWithValue("remark", mInfo.memRemark);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message;
                    return;
                }


                mInfo.memName = ""; mInfo.memEmail = ""; mInfo.memPhone = ""; mInfo.memBirth = ""; mInfo.memRemark = "";
                succMessage = "新增資料完成!";

                Response.Redirect("/Member/Index");
            }
        
    }
}
