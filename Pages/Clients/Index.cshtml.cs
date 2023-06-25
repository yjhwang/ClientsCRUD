using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace ClientsCRUD.Pages.Clients
{
    public class ClientInfo
    {
        public string? id;
        public string? name;
        public string? email;
        public string? phone;
        public string? address;
        public string? created_at;
    }
    public class IndexModel : PageModel
    {
        //�إ�LIST�M�檫��
        public List<ClientInfo> listClients = new();
        public string  errorMessage="";
        public int records = 0;
        public string connStr;
        public IndexModel(IConfiguration config1)
        {
            connStr = config1.GetConnectionString("connDB");
            records = TbRecords();
        }
        public int TbRecords()
        {
            try
            {
                SqlConnection conn = new(connStr);
                conn.Open();
                string sqlStr = "select COUNT(*) from clients ";
                SqlCommand cmd = new(sqlStr, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read()) records = dr.GetInt32(0);
                conn.Close();
            }
            catch (Exception ex)
            { errorMessage = ex.Message; }

            return records;
        }
        public void OnGet()
        {
            string qryStr = Request.Query["Qry"];
            try {
                // string connStr = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=MyStore;Integrated Security=True";
                // string connStr = @"Data Source =(LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\yjhwa\OneDrive\Documents\MyStore.mdf; Integrated Security = True";
                SqlConnection conn = new(connStr);
                conn.Open();
                string sqlStr = "select top 100 * from clients ";
                sqlStr += " WHERE name Like @SearchTerm ";
                sqlStr += " OR email Like @SearchTerm";
                sqlStr += " OR address Like @SearchTerm";
                SqlCommand cmd = new(sqlStr, conn);
                // �w�� SQL Injection
                cmd.Parameters.AddWithValue("@SearchTerm", $"%{qryStr}%");
                
                SqlDataReader dr  = cmd.ExecuteReader();

                while (dr.Read())
                         {
                            ClientInfo cInfo = new();

                            cInfo.id = "" + dr.GetInt32(0);
                            cInfo.name = dr.GetString(1);
                            cInfo.email = dr.GetString(2);
                            cInfo.phone = dr.GetString(3);
                            cInfo.address = dr.GetString(4);
                            cInfo.created_at = dr.GetDateTime(5).ToString();
                            
                            listClients.Add(cInfo);
                         }
                conn.Close();
            }
            catch (Exception ex) {
                errorMessage = ex.Message + "\n Details: " + ex.ToString();
            }
        }
    }
}
