using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace ClientsCRUD.Pages.Act
{
	public class actRegListModel : PageModel
    {
        public List<ActInfo> listAct = new();
        public string errorMessage = "";
        public int records = 0;
        public string connStr="";
        public actRegListModel(IConfiguration config1)
        {
            connStr = config1.GetConnectionString("connDB");
            records = TbRecords();
        }
        // 回傳資料表數量
        public int TbRecords()
        {
            try
            {
                SqlConnection conn = new(connStr);
                conn.Open();
                string sqlStr = "select COUNT(*) from tbActivity ";
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
            try
            {
                SqlConnection conn = new(connStr);
                conn.Open();
                string sqlStr = "select top 100 * from tbActivity ";
                sqlStr += " WHERE actName Like @SearchTerm ";
                sqlStr += " OR actType Like @SearchTerm";
                sqlStr += " OR actDepart Like @SearchTerm";
                sqlStr += " OR actLocation Like @SearchTerm";
                SqlCommand cmd = new(sqlStr, conn);
                //預防 SQL Injection
                cmd.Parameters.AddWithValue("@SearchTerm", $"%{qryStr}%");

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ActInfo cInfo = new();

                    cInfo.actNo = "" + dr.GetInt32(0);
                    cInfo.actName = dr.GetString(1);
                    cInfo.actType = dr.GetString(2);
                    cInfo.actDepart = dr.GetString(3);
                    cInfo.actLocation = dr.GetString(4);
                    cInfo.actStart = dr.GetDateTime(5).ToString();
                    cInfo.actEnd = dr.GetDateTime(6).ToString();


                    listAct.Add(cInfo);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                // errorMessage += "\n 詳細錯誤訊息: " + ex.ToString();
            }
        }
    }
}
