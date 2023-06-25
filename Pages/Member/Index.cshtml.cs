using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientsCRUD.Pages.Member
{

    public class MemInfo
    {
        public string? memID;
        public string? memUname; // Login Name
        public string? memPwd; 
        public string? memName;
        public string? memPhone;
        public string? memEmail;
        public string? memBirth;
        public string? memRemark;
        public string? cDate;  // create_at datetime
        public string? mDate;  // modified datetime
    }

    public class IndexModel : PageModel
    {
        public List<MemInfo> listMem = new();
        public string errorMessage = "";
        public int records = 0;
        public string connStr;

        public IndexModel(IConfiguration config1)
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
                string sqlStr = "select COUNT(*) from tbMembers ";
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
                string sqlStr = "select top 100 * from tbMembers ";
                sqlStr += " WHERE memName Like @SearchTerm ";
                sqlStr += " OR memPhone Like @SearchTerm";
                sqlStr += " OR memEmail Like @SearchTerm";
                SqlCommand cmd = new(sqlStr, conn);
                //預防 SQL Injection
                cmd.Parameters.AddWithValue("@SearchTerm", $"%{qryStr}%");

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    MemInfo cInfo = new();

                    cInfo.memID = "" + dr.GetInt32(0);
                    cInfo.memName = dr.GetString(3);
                    cInfo.memPhone = dr.GetString(4);
                    cInfo.memEmail = dr.GetString(5);
                    cInfo.memBirth = dr.GetDateTime(6).ToString();
                    cInfo.memRemark = dr.GetString(7);
                    cInfo.cDate = dr.GetDateTime(8).ToString();
                    cInfo.mDate = dr.GetDateTime(9).ToString();

                    listMem.Add(cInfo);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message + "\n 詳細錯誤訊息: " + ex.ToString();
            }
        }
    }
}
