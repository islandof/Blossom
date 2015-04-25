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
            conn.Close();

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

            string[] imglist = new string[8];
            for(var i =0;i<Request.Files.Count;i++)
            {
                var file = Request.Files[i];
                var imgname = "";
                if (file.ContentLength != 0)
                {

                    imgname = System.Guid.NewGuid().ToString().Substring(0, 8) + file.FileName;
                    file.SaveAs(Server.MapPath(@"\UploadFile\\" + imgname));
                }
                imglist[i] = imgname;
            }

            if (!Request.Form["ID"].IsNullOrWhiteSpace())
            {
                sql = "update products set Cnname = '" + Request.Form["Cnname"] + "', Enname = '" +
                      Request.Form["Enname"] + "', _parentId = '" + Request.Form["_parentId"] + "', Icon = '" +
                      (imglist[0].IsNullOrWhiteSpace() ? Request.Form["Icon"] : imglist[0]) + 
                      "', Firstimg = '" + (imglist[1].IsNullOrWhiteSpace() ? Request.Form["Firstimg"] : imglist[1]) + 
                      "', Secondimg = '" + (imglist[2].IsNullOrWhiteSpace() ? Request.Form["Secondimg"] : imglist[2]) + 
                      "', Thirdimg = '" + (imglist[3].IsNullOrWhiteSpace() ? Request.Form["Thirdimg"] : imglist[3]) +
                      "', Forthimg = '" + (imglist[4].IsNullOrWhiteSpace() ? Request.Form["Forthimg"] : imglist[4]) +
                      "', Fifthimg = '" + (imglist[5].IsNullOrWhiteSpace() ? Request.Form["Fifthimg"] : imglist[5]) +
                      "', Sixthimg = '" + (imglist[6].IsNullOrWhiteSpace() ? Request.Form["Sixthimg"] : imglist[6]) +
                      "', Seventhimg = '" + (imglist[7].IsNullOrWhiteSpace() ? Request.Form["Seventhimg"] : imglist[7]) + 
                      "', Type = '" +Request.Form["Type"] + "' where ID = " + Request["ID"];
            }
            else
            {
                sql = "insert into Products (Cnname,Enname,_parentId,Icon,Firstimg,Secondimg,Thirdimg,Forthimg,Fifthimg,Sixthimg,Seventhimg,Type) values( '" + Request.Form["Cnname"] + "', '" +
                  Request.Form["Enname"] + "', '" + Request.Form["_parentId"] + "', '" + imglist[0] + "', '" + imglist[1] + "', '" + imglist[2] + "', '" + imglist[3] + "', '" + imglist[4] + 
                  "', '" + imglist[5] + "', '" + imglist[6] + "', '" + imglist[7] + "', '" + Request.Form["Type"] + "' )";    
            }
            
            var conn = new OleDbConnection(ConnStr1);

            conn.Open();

            var cmd = new OleDbCommand(sql, conn);
            cmd.ExecuteNonQuery();
            conn.Close();


            return this.JsonFormat("");
        }

        public ActionResult GetProduct()
        {
            var conn = new OleDbConnection(ConnStr1);

            conn.Open();
            var sql = "select * from Products where ID = " + Request["ID"];
            var myadapter = new OleDbDataAdapter(sql, conn);
            var dt = new DataTable();
            myadapter.Fill(dt);
            conn.Close();
            return this.JsonFormat(dt);
        }

        public ActionResult DeleteProduct()
        {
            var conn = new OleDbConnection(ConnStr1);
            conn.Open();

            var sql = "select * from Products where [_parentId] = '" + Request["ID"] + "' ";
            var myadapter = new OleDbDataAdapter(sql, conn);
            var dt = new DataTable();
            myadapter.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                sql = "Delete from  Products where ID = " + Request["ID"];
                var cmd = new OleDbCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            else
            {
                return this.JsonFormat("不能删除，此项包含子元素，要删除前，请先删除子元素");
            }
            
            return this.JsonFormat("");
        }

        public ActionResult ProductDetail()
        {
            ViewBag.Firstimg = Request["Firstimg"];
            ViewBag.Secondimg = Request["Secondimg"];
            ViewBag.Thirdimg = Request["Thirdimg"];
            ViewBag.Forthimg = Request["Forthimg"];
            ViewBag.Fifthimg = Request["Fifthimg"];
            ViewBag.Sixthimg = Request["Sixthimg"];
            ViewBag.Seventhimg = Request["Seventhimg"];

            return View();
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
        
        public ActionResult LoginSumit()
        {
            if (Request["username"] == "admin" && Request["pwd"] == "111111")
            {
                return this.JsonFormat("../home/ProductList");
            }
            else
            {
                return this.JsonFormat("用户名和密码错误，请重新输入");
            }
                 
        }
    }
}