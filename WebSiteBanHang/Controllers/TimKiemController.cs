using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;
using PagedList;
using PagedList.Mvc;
namespace WebSiteBanHang.Controllers
{
    public class TimKiemController : Controller
    {
        //
        // GET: /TimKiem/
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        [HttpGet]
        public ActionResult KQTimKiem(string sTuKhoa, int? page)
        {
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            var lstKQ = db.SanPham.Where(n => n.TenSP.Contains(sTuKhoa));
            //thực hiện thức năng phân trang...
            //tạo số sản phẩm trên trang...
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            ViewBag.TuKhoa = sTuKhoa;

            return View(lstKQ.OrderBy(n => n.TenSP).ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult KQTimKiem(string sTuKhoa)
        {
            //gội về hàm get tìm kiếm
            return RedirectToAction("KQTimKiem", new { @sTuKhoa = sTuKhoa }); 
            //if (Request.HttpMethod != "GET")
            //{
            //    page = 1;
            //}
            //var lstKQ = db.SanPham.Where(n => n.TenSP.Contains(sTuKhoa));
            ////thực hiện thức năng phân trang...
            ////tạo số sản phẩm trên trang...
            //int pageSize = 6;
            //int pageNumber = (page ?? 1);
            //ViewBag.TuKhoa = sTuKhoa;
            //return View(lstKQ.OrderBy(n => n.TenSP).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult KQTimKiemPartial(string sTuKhoa)
        {
            //tìm kiếm theo tên sản phẩm....
            var lstSP = db.SanPham.Where(n => n.TenSP.Contains(sTuKhoa));
            ViewBag.TuKhoa = sTuKhoa;
            return PartialView(lstSP.OrderBy(n=>n.DonGia));
        }
	
	}
}