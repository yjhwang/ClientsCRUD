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
	public class actEditModel : PageModel
    {
        public ActInfo cInfo = new();
        public string errorMessage = "";
        public string succMessage = "";
        string connStr = "";

        public actEditModel(IConfiguration config1)
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
                string sqlStr = "select * from tbActivity WHERE actNo=@id";
                SqlCommand cmd = new(sqlStr, conn);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    cInfo.actNo = ""+ rd.GetInt32(0);
                    cInfo.actName = rd.GetString(1);
                    cInfo.actType = rd.GetString(2);
                    cInfo.actDepart = rd.GetString(3);
                    cInfo.actLocation = rd.GetString(4);
                    cInfo.actStart = rd.GetDateTime(5).ToString("yyyy-MM-dd");   //頁面帶出原來日期
                    cInfo.actEnd = rd.GetDateTime(6).ToString("yyyy-MM-dd");  //頁面帶出原來日期
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
            cInfo.actNo = Request.Form["actNo"];
            cInfo.actName = Request.Form["actName"];
            cInfo.actType = Request.Form["actType"];
            cInfo.actDepart = Request.Form["actDepart"];
            cInfo.actLocation = Request.Form["actLocation"];
            cInfo.actStart = Request.Form["actStart"];    //表單傳送資料
            cInfo.actEnd = Request.Form["actEnd"];      //表單傳送資料

            if (cInfo.actName.Length == 0 || cInfo.actType.Length == 0 ||
                cInfo.actDepart.Length == 0 || cInfo.actLocation.Length == 0)
            {
                errorMessage = "All the fields are required!";
                return;
            }

            // Update to Database
            try
            {
                SqlConnection conn = new(connStr);
                conn.Open();
                string sqlStr = "UPDATE tbActivity " +
                    " SET actName=@name, actType=@type, actDepart=@depart, actLocation=@location, actStart=@start, actEnd=@end  " +
                    " WHERE actNo = @id";    //存入DB
                SqlCommand cmd = new(sqlStr, conn);
                cmd.Parameters.AddWithValue("@id", cInfo.actNo);
                cmd.Parameters.AddWithValue("@name", cInfo.actName);
                cmd.Parameters.AddWithValue("@type", cInfo.actType);
                cmd.Parameters.AddWithValue("@depart", cInfo.actDepart);
                cmd.Parameters.AddWithValue("@location", cInfo.actLocation);
                cmd.Parameters.AddWithValue("@start",cInfo.actStart);
                cmd.Parameters.AddWithValue("@end", cInfo.actEnd);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message + "\n" + ex.ToString();
                return;
            }


            cInfo.actName = ""; cInfo.actType = ""; cInfo.actDepart = ""; cInfo.actLocation = ""; cInfo.actStart = ""; cInfo.actEnd = "";
            succMessage = "Update client corrected!";

            Response.Redirect("/Act/actList");
        }
    }
}
