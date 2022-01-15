using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Web14January.Models;

namespace Web14January.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string name, string email,string pass,string gender,string red,string foot, string cri, HttpPostedFileBase pic)
        {
            
            string StrConn = ConfigurationManager.ConnectionStrings["MyConn"].ToString();
            SqlConnection connection = new SqlConnection(StrConn);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            SqlCommand command = new SqlCommand("sp_RegisterStu", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@password", pass);
            command.Parameters.AddWithValue("@gender", gender);
            command.Parameters.AddWithValue("@profile", pic.FileName);
            string hobbie = "";
            if (red != null)
                hobbie += red.ToString() + ",";
            if (cri != null)
                hobbie += cri.ToString() + ",";
            if (foot != null)
                hobbie += foot.ToString();
            command.Parameters.AddWithValue("@hobbie", hobbie);
            int res=command.ExecuteNonQuery();
            if(res>0)
            {
                string filepath=System.IO.Path.Combine(Server.MapPath("/Content/Profile/") , pic.FileName);
                pic.SaveAs(filepath);
                Response.Write("<script>alert('Record Added SuccessFully')</script>");
            }
            else
            {
                Response.Write("<script>alert('Record does not Added')</script>");
            }
            connection.Close();
            return View();
        }
        public ActionResult SerarchStudent()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SerarchStudent(string email)
        {
            string StrConn = ConfigurationManager.ConnectionStrings["MyConn"].ToString();
            SqlConnection connection = new SqlConnection(StrConn);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            SqlCommand command = new SqlCommand("sp_readdata", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@email", email);
            DataTable dataTable = new DataTable();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
            if (connection.State == ConnectionState.Open)
            {
                sqlDataAdapter.Fill(dataTable);
            }
            connection.Close();
            student st = new student();
            if(dataTable.Rows.Count>0)
            {

                st.name = dataTable.Rows[0][1].ToString();
                st.gender = dataTable.Rows[0][2].ToString();
                st.email = dataTable.Rows[0][3].ToString();
                st.password = dataTable.Rows[0][4].ToString();
                st.hobbie = dataTable.Rows[0][5].ToString();
                st.picture = dataTable.Rows[0][6].ToString();
                st.regdate = dataTable.Rows[0][7].ToString();
                
            }
            return View(st);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}