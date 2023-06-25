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
	public class actAddModel : PageModel
    {
        public ActInfo cInfo = new();
        private string connStr;

        public actAddModel(IConfiguration config1)
        {
            connStr = config1.GetConnectionString("connDB");
        }
        public void OnGet()
        {

        }
        public string errorMessage = "";
        public string succMessage = "";
        public void OnPost()
        {
            cInfo.actName = Request.Form["actName"];
            cInfo.actType = Request.Form["actType"];
            cInfo.actDepart = Request.Form["actDepart"];
            cInfo.actLocation = Request.Form["actLocation"];

            if (cInfo.actName.Length == 0 || cInfo.actType.Length == 0 ||
                cInfo.actDepart.Length == 0 || cInfo.actLocation.Length == 0)
            {
                errorMessage = "請填完整(*)必填的資料內容!";
                return;
            }

            // Save to Database
            try
            {
                SqlConnection conn = new(connStr);
                conn.Open();
                string sqlStr = "INSERT INTO tbActivity " +
                       "(actName, actType, actDepart, actLocation) VALUES " +
                       "(@name, @type, @depart, @address);";

                SqlCommand cmd = new(sqlStr, conn);
                cmd.Parameters.AddWithValue("@name", cInfo.actName);
                cmd.Parameters.AddWithValue("@type", cInfo.actType);
                cmd.Parameters.AddWithValue("@depart", cInfo.actDepart);
                cmd.Parameters.AddWithValue("@address", cInfo.actLocation);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }


            cInfo.actName = ""; cInfo.actType = ""; cInfo.actDepart = ""; cInfo.actLocation = "";
            succMessage = "新增資料完成!";

            Response.Redirect("/Act/actList");
        }
    }
}
