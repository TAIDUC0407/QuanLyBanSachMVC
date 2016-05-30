using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{
    //[Authorize(Roles = "quantri")]
    [Authorize(Roles = "QuanLyKhachHang")]
    public class KhachHangController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        //[Authorize(Roles = "QuanLySanPham")]
        public ActionResult Index()
        {
            //Truy vấn dữ liệu thông qua câu lệnh
            //Đối lstKH sẽ lấy toàn bộ dữ liệu từ bản khách hàng
            //Cách 1: Lấy dữ liệu là 1 danh sách khách hàng
            //var lstKH = from KH in db.KhachHangs select KH;
            //Cách 2: Dùng phương thức hổ trợ sẵn
            var lstKH = db.KhachHang;
            return View(lstKH);

        }
        //[Authorize(Roles = "QuanTri")]
        public ActionResult Index1()
        {
            var lstKH = from KH in db.KhachHang select KH;
            return View(lstKH);
        }
        public ActionResult TruyVan1DoiTuong()
        {
            //Cách 1: Truy vấn 1 đối tượng bằng câu lệnh truy vấn
            //Bước1: Lấy ra danh sách khách hàng 
            var lstKH = from kh in db.KhachHang where kh.MaKH == 2 select kh;
            //Buoc2: 
            //KhachHang khang = lstKH.FirstOrDefault();
            //Lấy đối tượng khách hàng dựa trên phương thức hổ trợ
            KhachHang khang = db.KhachHang.SingleOrDefault(n => n.MaKH == 2);
            return View(khang);
        }
        public ActionResult SortDuLieu()
        {
            //Phương thức sắp xếp dữ liệu
            List<KhachHang> lstKH = db.KhachHang.OrderByDescending(n => n.TenKH).ToList();
            return View(lstKH);

        }
        public ActionResult GroupDuLieu()
        {
            //Group dữ liệu
            List<ThanhVien> lstKH = db.ThanhVien.OrderByDescending(n => n.TaiKhoan).ToList();
            return View(lstKH);
        }
	}
}