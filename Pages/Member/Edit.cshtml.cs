using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace ClientsCRUD.Pages.Member
{
    public class EditModel : PageModel
    {
        public MemInfo mInfo = new();
        public string errorMessage = "";
        public string succMessage = "";
        string connStr = "";

        public EditModel(IConfiguration config1)
        {
            connStr = config1.GetConnectionString("connDB");
        }
        public void OnGet()
        {
            string id = Request.Query["id"];
            try
            {
                SqlConnection conn = new(connStr);
                conn.Open();
                string sqlStr = "select * from tbMembers WHERE memID = @id";   
                SqlCommand cmd = new(sqlStr, conn);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    mInfo.memID = "" + rd.GetInt32(0);
                    mInfo.memUname = rd.GetString(1);
                    mInfo.memPwd = rd.GetString(2);
                    mInfo.memName = rd.GetString(3);
                    mInfo.memPhone = rd.GetString(4);
                    mInfo.memEmail = rd.GetString(5);
                    mInfo.memBirth = rd.GetDateTime(6).ToString();
                    mInfo.memRemark = rd.GetString(7);

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message + "\n Details: " + ex.ToString();
            }

        }
        public void OnPost()
        {
            mInfo.memID = Request.Form["id"];
            mInfo.memName = Request.Form["name"];
            mInfo.memEmail = Request.Form["email"];
            mInfo.memPhone = Request.Form["phone"];
            mInfo.memBirth = Request.Form["birth"];
            mInfo.memRemark = Request.Form["remark"];

            if (mInfo.memName.Length == 0 || mInfo.memEmail.Length == 0 ||
                mInfo.memPhone.Length == 0 || mInfo.memBirth.Length == 0 || mInfo.memRemark.Length == 0 )
            {
                errorMessage = "All the fields are required!";
                return;
            }

            // Update to Database
            try
            {
                SqlConnection conn = new(connStr);
                conn.Open();
                string sqlStr = "UPDATE tbMembers " +
                    " SET memName=@name, memEmail=@email, memPhone=@phone, memBirth=@birth, memRemark=@remark " +
                    " WHERE memID = @id";
                SqlCommand cmd = new(sqlStr, conn);
                cmd.Parameters.AddWithValue("@id", mInfo.memID);
                cmd.Parameters.AddWithValue("@name", mInfo.memName);
                cmd.Parameters.AddWithValue("@email", mInfo.memEmail);
                cmd.Parameters.AddWithValue("@phone", mInfo.memPhone);
                cmd.Parameters.AddWithValue("@birth", mInfo.memBirth);
                cmd.Parameters.AddWithValue("@remark", mInfo.memRemark);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message + "\n" + ex.ToString();
                return;
            }


            mInfo.memID = ""; mInfo.memEmail = ""; mInfo.memPhone = ""; mInfo.memBirth = ""; mInfo.memRemark = "";
            succMessage = "Update client corrected!";

            Response.Redirect("/Member/Index");
        }
    }
}
