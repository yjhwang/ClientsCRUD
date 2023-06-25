using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace ClientsCRUD.Pages.Clients
{
    public class EditModel : PageModel
    {
        public ClientInfo cInfo = new();
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
            cInfo.id = Request.Form["id"];
            cInfo.name = Request.Form["name"];
            cInfo.email = Request.Form["email"];
            cInfo.phone = Request.Form["phone"];
            cInfo.address = Request.Form["address"];

            if (cInfo.name.Length == 0 || cInfo.email.Length == 0 ||
                cInfo.phone.Length == 0 || cInfo.address.Length == 0)
            {
                errorMessage = "All the fields are required!";
                return;
            }

            // Update to Database
            try
            {
                SqlConnection conn = new(connStr);
                conn.Open();
                string sqlStr = "UPDATE clients " +
                    " SET name=@name, email=@email, phone=@phone, address=@address " +
                    " WHERE id = @id";
                SqlCommand cmd = new(sqlStr, conn);
                cmd.Parameters.AddWithValue("@id", cInfo.id);
                cmd.Parameters.AddWithValue("@name", cInfo.name);
                cmd.Parameters.AddWithValue("@email", cInfo.email);
                cmd.Parameters.AddWithValue("@phone", cInfo.phone);
                cmd.Parameters.AddWithValue("@address", cInfo.address);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message + "\n" + ex.ToString();
                return;
            }


            cInfo.name = ""; cInfo.email = ""; cInfo.phone = ""; cInfo.address = "";
            succMessage = "Update client corrected!";

            Response.Redirect("/Clients/Index");
        }
    }
}
