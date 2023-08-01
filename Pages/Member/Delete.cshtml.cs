using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace ClientsCRUD.Pages.Member
{
    public class DeleteModel : PageModel
    {
        public MemInfo mInfo = new();
        private readonly string _connectStr;
        public string errorMessage = "";
        public string succMessage = "";

        public DeleteModel(IConfiguration config1)
        {
            _connectStr = config1.GetConnectionString("connDB");
        }
        public void OnGet()
        {
            string id = Request.Query["id"];
            try
            {
                string connStr = _connectStr;
                SqlConnection conn = new(connStr);
                conn.Open();
                string sqlStr = "select * from tbMembers WHERE memID = @id";
                SqlCommand cmd = new(sqlStr, conn);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    mInfo.memID = "" + rd.GetInt32(0);
                    mInfo.memName = rd.GetString(3);
                    mInfo.memPhone = rd.GetString(4);
                    mInfo.memEmail = rd.GetString(5);
                    mInfo.memBirth = rd.GetString(6).ToString();
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
            try
            {
                string id = Request.Query["id"];

                string connStr = _connectStr;
                SqlConnection conn = new(connStr);
                conn.Open();
                string sqlStr = "DELETE from tbMember WHERE memID = @id";
                SqlCommand cmd = new(sqlStr, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message + "\n" + ex.ToString();
                return;
            }

            Response.Redirect("/Member/Index");

        }
    }
}
