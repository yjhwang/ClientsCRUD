using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace ClientsCRUD.Pages.Clients
{
    public class DeleteModel : PageModel
    {
        public ClientInfo cInfo = new();
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
                string sqlStr = "select * from clients WHERE id = @id";
                SqlCommand cmd = new(sqlStr, conn);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    cInfo.id = "" + rd.GetInt32(0);
                    cInfo.name = rd.GetString(1);
                    cInfo.email = rd.GetString(2);
                    cInfo.phone = rd.GetString(3);
                    cInfo.address = rd.GetString(4);
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
                string sqlStr = "DELETE from clients WHERE id=@id";
                SqlCommand cmd = new(sqlStr, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message + "\n" + ex.ToString();
                return;
            }

            Response.Redirect("/Clients/Index");

        }
    }
}
