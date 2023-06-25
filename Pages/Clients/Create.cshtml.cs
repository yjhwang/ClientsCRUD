using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace ClientsCRUD.Pages.Clients
{
    public class CreateModel : PageModel
    {
        public ClientInfo cInfo = new ClientInfo();
        private readonly string _connectStr;
        
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

                // Save to Database
                try
                {
                // string connStr = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=MyStore;Integrated Security=True";
                // string connStr = @"Data Source =(LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\yjhwa\OneDrive\Documents\MyStore.mdf; Integrated Security = True";
                string connStr = _connectStr;
                SqlConnection conn = new(connStr);
                    conn.Open();
                    string sqlStr = "INSERT INTO clients " +
                           "(name, email, phone, address) VALUES " +
                           "(@name, @email, @phone, @address);";

                    SqlCommand cmd = new(sqlStr, conn);
                    cmd.Parameters.AddWithValue("name", cInfo.name);
                    cmd.Parameters.AddWithValue("email", cInfo.email);
                    cmd.Parameters.AddWithValue("phone", cInfo.phone);
                    cmd.Parameters.AddWithValue("address", cInfo.address);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message;
                    return;
                }


                cInfo.name = ""; cInfo.email = ""; cInfo.phone = ""; cInfo.address = "";
                succMessage = "New client add corrected!";

                Response.Redirect("/Clients/Index");
            }
        
    }
}
