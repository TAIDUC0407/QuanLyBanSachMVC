using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{
    public class AngularJSController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        //
        // GET: /AngularJS/
        public ActionResult exam1()
        {
            return View();
        }
        //get list json...
        
        [HttpGet]
        public ActionResult getJson()
        {
            //var items = new List<object>();
            //JavaScriptSerializer js = new JavaScriptSerializer();
            //List<KhachHang> listKH = (List<KhachHang>)db.NhaSanXuat.ToList();
            //string json = js.Serialize(listKH);
            return View();
        }

        public ActionResult getJson1()
        {
            var items = new List<object>();
            //JavaScriptSerializer js = new JavaScriptSerializer();
            var listKH = db.KhachHang.ToList();
            //string json = js.Serialize(db.NhaSanXuat.ToList());
            //NhaSanXuat nsx = new NhaSanXuat(); ;
            foreach (var item in listKH)
            {
                items.Add(new { item.TenKH, item.DiaChi, item.Email, item.SoDienThoai });
            }

            return Json(items, JsonRequestBehavior.AllowGet);
            //return View(Json(items, JsonRequestBehavior.AllowGet));
        }
	}
}