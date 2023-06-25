using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClientsCRUD.Pages.Member;
using System.Runtime;
using System.Security.Cryptography.Xml;

namespace ClientsCRUD.Pages.Act
{
    public class MemActInfo
    {
        public string? maNo; // 自動
        public string? maActNo;
        public string? maMemID;
        public string? maDTime;
        public string? maRemark;
        public string? mUserID;
    }

    public class actMemRegModel : PageModel
    {
        public MemActInfo maInfo = new();
        public ActInfo cInfo = new();
        public MemInfo memInfo = new();
        
        public string errorMessage = "";
        public string succMessage = "";
        string connStr = "";
        public string id="";

        public actMemRegModel(IConfiguration config1)
        {
            connStr = config1.GetConnectionString("connDB");
        }
        // 顯示活動摘要
        public void OnGet(string id)
        {
            // id = Request.Query["id"];
            try
            {
                SqlConnection conn = new(connStr);
                conn.Open();
                string sqlStr = "select * from tbActivity WHERE actNo=@aid" ;
                SqlCommand cmd = new(sqlStr, conn);
                cmd.Parameters.AddWithValue("@aid", id);
                SqlDataReader rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    cInfo.actNo = "" + rd.GetInt32(0);
                    cInfo.actName = rd.GetString(1);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message + "\n Details: " + ex.ToString();
            }

        }

        public IActionResult OnPost()
        {
            maInfo.maActNo = Request.Form["actNo"];
            maInfo.maMemID = Request.Form["memID"];
            memInfo.memName = "";
            DateTime now_time = DateTime.Now;
            maInfo.maDTime = now_time.ToString();
            maInfo.maRemark = Request.Form["maRemark"];

            // Find the name of member by memID
            try
            {
                SqlConnection conn = new(connStr);
                conn.Open();
                string sqlStr = "SELECT * FROM tbMembers Where memID = @MemID";
                SqlCommand cmd = new(sqlStr, conn);
                cmd.Parameters.AddWithValue("@MemID", maInfo.maMemID);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    memInfo.memID = "" + dr.GetInt32(0);
                    memInfo.memName = dr.GetString(3);
                    memInfo.memPhone = dr.GetString(4);
                    memInfo.memEmail = dr.GetString(5);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return RedirectToPage(new { id = maInfo.maActNo });
            }

            if (maInfo.maMemID.Length == 0 || memInfo.memName.Length == 0 )
            {
                errorMessage = "找不到Member ID，請先確認會員編號!";
                return RedirectToPage(new { id = maInfo.maActNo });
            }

            // Save to Database
            try
            {
                SqlConnection conn = new(connStr);
                conn.Open();
                string sqlStr = "INSERT INTO tbMemAct " +
                       "(maActNo, maMemID, maRemark) VALUES " +
                       "(@ActNo, @MemID, @Remark);";

                SqlCommand cmd = new(sqlStr, conn);
                cmd.Parameters.AddWithValue("@ActNo", maInfo.maActNo);
                cmd.Parameters.AddWithValue("@MemID", maInfo.maMemID);
                cmd.Parameters.AddWithValue("@Remark", maInfo.maRemark);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return RedirectToPage(new { id = maInfo.maActNo });
            }

            succMessage = "新增資料完成!";

            return RedirectToPage("/Act/actRegList");
        }
        

        // 2. 非會員，輸入會員資料，勾選註冊 --> 確定報名及完成註冊（若勾選註冊，傳驗證碼）
    }
}
