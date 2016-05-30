using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using WebSiteBanHang.Models;



namespace WebSiteBanHang.Controllers
{
    //[Authorize(Roles = "SanPham")]
    public class SanPhamController : Controller
    {
        //
        // GET: /SanPham/
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        //public ActionResult SanPham1()
        //{
        //    var lstSanPhamLTM = db.SanPham.Where(n => n.MaLoaiSP == 2 && n.Moi == 1);
        //    ViewBag.ListSP = lstSanPhamLTM;
        //    return View();
        //}
        //public ActionResult SanPham2()
        //{
        //    var lstSanPhamDT = db.SanPham.Where(n => n.MaLoaiSP == 1);
        //    ViewBag.ListDT = lstSanPhamDT;

        //    var lstSanPhamLTM = db.SanPham.Where(n => n.MaLoaiSP == 2 && n.Moi == 1);
        //    ViewBag.ListSP = lstSanPhamLTM;
        //    return View();
        //}

        //[ChildActionOnly]
        //public ActionResult SanPhamPartial()
        //{

        //    return PartialView();
        //}


        [ChildActionOnly]
        public ActionResult SanPhamStyle1Partial()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult SanPhamStyle2Partial()
        {
            return PartialView();
        }

        //xây dựng trang xem chi tiết...
        public ActionResult XemChiTiet(int? id)
        {
            if (id == null)
            {
                //kiểm tra tham số truyền vào có rỗng hay không...
                //Response.StatusCode = 404;
                //return null;
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //nếu không truy xuất vào cơ sở dữ liệu lấy sp tương ứng...
            SanPham sp = db.SanPham.SingleOrDefault(n => n.MaSP == id && n.DaXoa == false);

            if (sp == null)
            {
               // thông báo nếu như không có sản phẩm...
                return HttpNotFound();
            }

            return View(sp);
        }

        public ActionResult SanPham(int? MaLoaiSP, int? MaNSX, int? page)
        {
            if (MaLoaiSP == null || MaNSX ==null)
            {
                //kiểm tra tham số truyền vào có rỗng hay không...
                //Response.StatusCode = 404;
                //return null;
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var sp = db.SanPham.Where(n => n.MaLoaiSP == MaLoaiSP && n.MaNSX == MaNSX);
            if (sp == null)
            {
                // thông báo nếu như không có sản phẩm...
                return HttpNotFound();
            }
            //thực hiện thức năng phân trang...
            //tạo số sản phẩm trên trang...
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            ViewBag.MaLoaiSP = MaLoaiSP;
            ViewBag.MaNSX = MaNSX;
            return View(sp.OrderBy(n=>n.MaSP).ToPagedList(pageNumber,pageSize));

        }
	}
}