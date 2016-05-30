using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;
using CaptchaMvc.HtmlHelpers;
using CaptchaMvc;
using System.Web.Security;

namespace WebSiteBanHang.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();

        
        public ActionResult Index()
        {
            var lstDTM = db.SanPham.Where(n => n.MaLoaiSP == 1 && n.Moi == 1 && n.DaXoa == false);
            ViewBag.lstDTM = lstDTM;

            var lstLTM = db.SanPham.Where(n => n.MaLoaiSP == 3 && n.Moi == 1 && n.DaXoa == false);
            ViewBag.lstLTM = lstLTM;

            var lstMTBM = db.SanPham.Where(n => n.MaLoaiSP == 2 && n.Moi == 1 && n.DaXoa == false);
            ViewBag.lstMTBM = lstMTBM;
            return View();
        }

        public ActionResult MenuPartial()
        {
            var lstSP = db.SanPham;
            return PartialView(lstSP);
        }

        [HttpGet]
        public ActionResult DangKy()
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(ThanhVien tv)
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            //Kiem tra captcha hop le...
            if (this.IsCaptchaValid("Mã không hợp lệ..."))
            {
                ViewBag.ThongBao = "Thêm thành công";
                db.ThanhVien.Add(tv);
                db.SaveChanges();
                return View();
            }
            ViewBag.ThongBao = "Mã không hợp lệ";
            return View();
        }

        public List<string> LoadCauHoi()
        {
            List<string> lstCauHoi = new List<string>();
            lstCauHoi.Add("Con vật mà bạn yêu thích?");
            lstCauHoi.Add("Ca sĩ mà bạn yêu thích?");
            lstCauHoi.Add("công việc mà bạn thích?");
            return lstCauHoi;
        }


        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            //string sTaiKhoan = fc["txtTenDangNhap"].ToString();
            //string sMatKhau = fc["txtMatKhau"].ToString();
            //ThanhVien tv = db.ThanhVien.SingleOrDefault(n => n.TaiKhoan == sTaiKhoan && n.MatKhau == sMatKhau);
            //if (tv != null)
            //{
            //    Session["TaiKhoan"] = tv;
            //    return Content("<script>window.location.reload();</script>");
            //}

            //return Content("Tên tài khoản hoặc mật khẩu không đúng!");

            string taikhoan = f["txtTenDangNhap"].ToString();
            string matkhau = f["txtMatKhau"].ToString();
            //Truy vấn kiểm tra đăng nhập lấy thông tin thành viên
            ThanhVien tv = db.ThanhVien.SingleOrDefault(n => n.TaiKhoan == taikhoan && n.MatKhau == matkhau);
            if (tv != null)
            {
                //Lấy ra list quyền của thành viên tương ứng với loại thành viên
                var lstQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == tv.MaLoaiTV);
                //Duyệt list quyền
                string Quyen = "";
                foreach (var item in lstQuyen)
                {
                    Quyen += item.Quyen.MaQuyen + ",";
                }
                Quyen = Quyen.Substring(0, Quyen.Length - 1); //Cắt dấu ","
                PhanQuyen(tv.TaiKhoan.ToString(), Quyen);
                Session["TaiKhoan"] = tv;
                return Content("<script>window.location.reload();</script>");
            }
            return Content("Tài khoản hoặc mật khẩu không đúng!");

        }

        public void PhanQuyen(string TaiKhoan, string Quyen)
        {
            FormsAuthentication.Initialize();
            var ticket = new FormsAuthenticationTicket(1,
                                          TaiKhoan, //user
                                          DateTime.Now, //Thời gian bắt đầu
                                          DateTime.Now.AddHours(3), //Thời gian kết thúc
                                          false, //Ghi nhớ?
                                          Quyen, // "DangKy,QuanLyDonHang,QuanLySanPham"
                                          FormsAuthentication.FormsCookiePath);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;
            Response.Cookies.Add(cookie);
        }
        //Tạo trang ngăn chặn quyền truy cập
        public ActionResult LoiPhanQuyen()
        {

            return View();
        }

        public ActionResult DangXuat()
        {
            //Session["TaiKhoan"] = null;
            //return RedirectToAction("Index");

            Session["TaiKhoan"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");

        }
	}
}