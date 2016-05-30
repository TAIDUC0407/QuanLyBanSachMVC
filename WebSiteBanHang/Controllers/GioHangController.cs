using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{
    public class GioHangController : Controller
    {
        //
        // GET: /GioHang/
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        //Lấy giỏ hàng...
        public List<ItemGioHang> LayGioHang()
        {
            //giỏ hàng đã tồn tại...
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                //Nếu session giỏ hàng chưa tồn tại...
                lstGioHang= new List<ItemGioHang>();
                Session["GioHang"] = lstGioHang;
                    
            }
            return lstGioHang;
        }

        //Thêm giỏ hàng không ajax...
        public ActionResult ThemGioHang(int MaSP, string strURL)
        {
            //Kiểm tra sp có tồn tại trong csdl hay không...
            SanPham sp = db.SanPham.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy giỏ hàng...
            List<ItemGioHang> lstGioHang = LayGioHang();

            //trường hợp 1: sp đã tồn tại trong giỏ hang...
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck != null)
            {
                //kiểm tra trong kho trước khi khách đặt hàng...
                if (sp.SoLuongTon < spCheck.SoLuong)
                {
                    return View("ThongBao");
                }
                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                return Redirect(strURL);
            }

            ItemGioHang itemGH = new ItemGioHang(MaSP);
            if (sp.SoLuongTon < itemGH.SoLuong)
            {
                return View("ThongBao");
            }
            lstGioHang.Add(itemGH);
            return Redirect(strURL);

        }


        //Tính tổng tiền...
        public decimal TinhTongTien()
        {
            //lấy giỏ hàng...
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.ThanhTien);
        }


        //Tính tổng số lượng...
        public double TinhTongSoLuong()
        {
            //lấy giỏ hàng...
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if(lstGioHang==null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.SoLuong);
        }

        public ActionResult GioHangPartial()
        {
            if (TinhTongSoLuong() == 0)
            {
                ViewBag.TongSoLuong = 0;
                ViewBag.TongTien = 0;
                return PartialView();
            }
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            return PartialView();
        }




        public ActionResult XemGioHang()
        {
            //Lấy giở hàng...
            List<ItemGioHang> lstGioHang = LayGioHang();
            return View(lstGioHang);
        }

        //chỉnh sửa giỏ hàng...
        public ActionResult SuaGioHang(int MaSP)
        {
            //kiểm tra giỏ hàng có tồn tại hay chưa...
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            //Kiểm tra sp có tồn tại trong csdl hay không...
            SanPham sp = db.SanPham.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //lấy list giỏ hàng từ session...
            List<ItemGioHang> lstGioHang = LayGioHang();
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }

            //lấy list giỏ hàng tạo giao diện...
            ViewBag.GioHang = lstGioHang;
            //nếu tồn tại rồi...
            return View(spCheck);
        }

        //xử lý cập nhật giỏ hàng...
        [HttpPost]
        public ActionResult CapNhatGioHang(ItemGioHang itemGH)
        {
            //Kiểm tra số lượng tồn trong kho...
            SanPham spCheck = db.SanPham.SingleOrDefault(n => n.MaSP == itemGH.MaSP);
            if (spCheck.SoLuongTon < itemGH.SoLuong)
            {
                return View("ThongBao");
            }
            //cập nhật số lượng trong session
            List<ItemGioHang> lstGH = LayGioHang();
            ItemGioHang itemGHUpdate = lstGH.Find(n => n.MaSP == itemGH.MaSP);
            itemGHUpdate.SoLuong = itemGH.SoLuong;

            itemGHUpdate.ThanhTien = itemGHUpdate.DonGia * itemGH.SoLuong;
            return RedirectToAction("XemGioHang");
        }

        public ActionResult XoaGioHang(int MaSP)
        {
            //kiểm tra giỏ hàng có tồn tại hay chưa...
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            //Kiểm tra sp có tồn tại trong csdl hay không...
            SanPham sp = db.SanPham.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //lấy list giỏ hàng từ session...
            List<ItemGioHang> lstGioHang = LayGioHang();
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }

            //Xoa item gio hang...
            lstGioHang.Remove(spCheck);
            return RedirectToAction("XemGioHang");
        }

        //xây dựng chức năng đặt hàng...
        public ActionResult DatHang(KhachHang kh)
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");

            }

            //thiết kế form lưu thông tin khách hàng đối với khách vãng lai(khách hàng chưa có tài khoản...)
            KhachHang khang = new KhachHang();
            if (Session["TaiKhoan"] == null)
            {
                khang = kh;
                db.KhachHang.Add(khang);
                db.SaveChanges();
            }
            else
            {
                //đối với khách hàng là thành viên...
                ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
                khang.TenKH = tv.HoTen;
                khang.DiaChi = tv.DiaChi;
                khang.Email = tv.Email;
                khang.SoDienThoai = tv.SoDienThoai;
                khang.MaThanhVien = tv.MaLoaiTV;
                db.KhachHang.Add(khang);
                db.SaveChanges();
            }
            //thêm đơn đặt hàng...
            DonDatHang ddh = new DonDatHang();
            ddh.MaKH = khang.MaKH;
            ddh.NgayDat = DateTime.Now;
            ddh.TinhTrangGiaoHang = false;
            ddh.DaThanhToan = false;
            ddh.UuDai = 0;
            db.DonDatHang.Add(ddh);
            db.SaveChanges();
            //Thêm chi tiết đơn đặt hàng...
            List<ItemGioHang> lstGH = LayGioHang();
            foreach (var item in lstGH)
            {
                ChiTietDonDatHang ctdh = new ChiTietDonDatHang();
                ctdh.MaDDH = ddh.MaDDH;
                ctdh.MaSP = item.MaSP;
                ctdh.TenSP = item.TenSP;
                ctdh.SoLuong = item.SoLuong;
                ctdh.DonGia = item.DonGia;
                db.ChiTietDonDatHang.Add(ctdh);
            }
            db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XemGioHang");

        }

        //thêm giỏ hàng bằng ajax...
        public ActionResult ThemGioHangAJax(int MaSP, string strURL)
        {
            //Kiểm tra sp có tồn tại trong csdl hay không...
            SanPham sp = db.SanPham.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy giỏ hàng...
            List<ItemGioHang> lstGioHang = LayGioHang();

            //trường hợp 1: sp đã tồn tại trong giỏ hang...
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck != null)
            {
                //kiểm tra trong kho trước khi khách đặt hàng...
                if (sp.SoLuongTon < spCheck.SoLuong)
                {
                    return Content("<script> alert(\"Sản phẩm đã hết hàng!\")</script>");
                }
                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                ViewBag.TongSoLuong = TinhTongSoLuong();
                ViewBag.TongTien = TinhTongTien();
                return PartialView("GioHangPartial");
            }

            ItemGioHang itemGH = new ItemGioHang(MaSP);
            if (sp.SoLuongTon < itemGH.SoLuong)
            {
                return Content("<script> alert(\"Sản phẩm đã hết hàng!\")</script>");
            }
            lstGioHang.Add(itemGH);
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            return PartialView("GioHangPartial");

        }
	}
}