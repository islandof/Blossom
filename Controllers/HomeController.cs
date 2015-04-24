using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Web.Mvc;
using BlossomWeb.Common;
using Microsoft.Ajax.Utilities;

namespace BlossomWeb.Controllers
{
    public class HomeController : Controller
    {
        public static readonly string ConnStr1 = ConfigurationManager.AppSettings["AccessConnString"] +
                                                 System.Web.HttpContext.Current.Server.MapPath(
                                                     ConfigurationManager.AppSettings["AccessDbPath"]) + ";";

        //public static readonly string connStr1 = "Provider = Microsoft.ACE.OLEDB.12.0 ;Data Source=/App_Data/Blossom.accdb";


        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult Products()
        {
            return View();
        }

        public ActionResult GetProducts()
        {
            var conn = new OleDbConnection(ConnStr1);

            conn.Open();
            var sql = "select * from Products";
            var myadapter = new OleDbDataAdapter(sql, conn);
            var dt = new DataTable();
            myadapter.Fill(dt);

            return this.JsonFormat(new EasyUIGrid
            {
                rows = dt,
                total = 0
            });
        }

        public ActionResult AddProduct()
        {
            ViewBag.CurrentID = Request["ID"];

            return View();
        }

        public ActionResult SaveProduct()
        {
            var sql = "";
            if (!Request.Form["Cnname"].IsNullOrWhiteSpace())
            {
                sql = Request.Form["Cnname"];
            }
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file.ContentLength == 0)
                {
                    //文件大小大（以字节为单位）为0时，做一些操作
                }
                else
                {
                    //文件大小不为0
                    //保存成自己的文件全路径,newfile就是你上传后保存的文件,
                    //服务器上的UpLoadFile文件夹必须有读写权限　　　　　　
                    file.SaveAs(Server.MapPath(@"\UploadFile\\newfile"));
                }
            }
            return this.JsonFormat("");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ProductList()
        {
            return View();
        }
    }
}