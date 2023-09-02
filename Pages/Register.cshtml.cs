using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using ClientsCRUD.Pages.Member;

namespace ClientsCRUD.Pages
{
    public class RegisterModel : PageModel
    {
        public MemInfo mInfo = new MemInfo();
        private readonly string _connectStr;  //不了解這行待研究 private string connStr;

        public RegisterModel(IConfiguration config1)
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
            mInfo.memUname = Request.Form["uname"];
            mInfo.memPwd = Request.Form["pwd"];
            mInfo.memName = Request.Form["name"];
            mInfo.memEmail = Request.Form["email"];

            if (mInfo.memUname.Length == 0 || mInfo.memPwd.Length == 0 ||
                mInfo.memName.Length == 0 || mInfo.memEmail.Length == 0)
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
                       "(memUname, memPwd, memName, memPhone, memEmail, memBirth, memRemark) VALUES " +
                       "(@uname, @pwd, 'name', 'phone', @email, 'birth', 'remark');";
                // 沒有給值， UI會有錯誤訊息 NULL

                SqlCommand cmd = new(sqlStr, conn);
                cmd.Parameters.AddWithValue("uname", mInfo.memUname);
                cmd.Parameters.AddWithValue("pwd", mInfo.memPwd);
                cmd.Parameters.AddWithValue("name", mInfo.memName);
                cmd.Parameters.AddWithValue("email", mInfo.memEmail);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            mInfo.memUname = "";  mInfo.memPwd = ""; mInfo.memName = ""; mInfo.memEmail = ""; mInfo.memPhone = ""; mInfo.memBirth = ""; mInfo.memRemark = "";
            succMessage = "新增資料完成!";

            Response.Redirect("/Member/Index");
        }
    }
}
