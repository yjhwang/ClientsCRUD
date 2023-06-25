using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ClientsCRUD.Pages.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientsCRUD.Pages.Act
{
	public class actDeleteModel : PageModel
    {
        public ActInfo cInfo = new();
        private readonly string connStr;
        private string tbName = "tbActivity";
        public string errorMessage = "";
        public string succMessage = "";

        public actDeleteModel(IConfiguration config1)
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
                string sqlStr = "select * from " + tbName + " WHERE actNo = @id";
                SqlCommand cmd = new(sqlStr, conn);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    cInfo.actNo = "" + rd.GetInt32(0);
                    cInfo.actName = rd.GetString(1);
                    cInfo.actType = rd.GetString(2);
                    cInfo.actDepart = rd.GetString(3);
                    cInfo.actLocation = rd.GetString(4);
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
                SqlConnection conn = new(connStr);
                conn.Open();
                string sqlStr = "DELETE from " + tbName + " WHERE actNo=@id";
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

            Response.Redirect("/Act/actList");

        }
    }
}
