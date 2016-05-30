using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{
    [Authorize(Roles = "QuanLySanPham")]
    public class QuanLySanPhamController : Controller
    {
        //
        // GET: /QuanLySanPham/
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index()
        {
            return View(db.SanPham.Where(n=>n.DaXoa==false));
        }

        [HttpGet]
        public ActionResult TaoMoi()
        {
            //load dll nhà cung cấp,loại sản phẩm...
            ViewBag.MaNCC = new SelectList(db.NhaCungCap.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPham.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuat.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX");
            return View();
        }


        [ValidateInput(false)]
        [HttpPost]
        public ActionResult TaoMoi(SanPham sp, HttpPostedFileBase HinhAnh)
        {

            //load dll nhà cung cấp,loại sản phẩm...
            ViewBag.MaNCC = new SelectList(db.NhaCungCap.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPham.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuat.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX");
            //kiểm tra hình ảnh có tồn tại trong csdl chưa...
            if (HinhAnh.ContentLength > 0)
            {
                //lấy tên hình ảnh....
                var fileName = Path.GetFileName(HinhAnh.FileName);
                //lấy hình ảnh chuyển vào thư mục hình ảnh ...
                var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP"), fileName);
                //nếu thư mục chứa hình ảnh đó rồi thì xuất ra thông báo....
                if (System.IO.File.Exists(path))
                {
                    ViewBag.upload = "Hình ảnh tồn tại...";
                }
                //lấy hình ảnh đưa vào thư mục hình ảnh sản phẩm...
                else
                {
                    HinhAnh.SaveAs(path);
                    sp.HinhAnh = fileName;
                    //Session["tenhinh"] = HinhAnh.FileName;
                    //ViewBag.tenhinh = "";
                }
            }
            db.SanPham.Add(sp);
            db.SaveChanges(); 
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ChinhSua(int? id)
        {
            //lấy sản phầm cần chỉnh sửa dựa vào id...

            //trang đường dẫn không hợp lệ...
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPham.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }


            //load dll nhà cung cấp,loại sản phẩm...
            ViewBag.MaNCC = new SelectList(db.NhaCungCap.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC",sp.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPham.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai",sp.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuat.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX",sp.MaNSX);
            return View(sp);
        }


        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSua(SanPham model)
        {

            //load dll nhà cung cấp,loại sản phẩm...
            ViewBag.MaNCC = new SelectList(db.NhaCungCap.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", model.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPham.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", model.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuat.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", model.MaNSX);
            //kiểm tra validation trong model nếu hợp thì thực hiện công việc{}
            //if (ModelState.IsValid)
            //{
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            //}
            //return View(model);
        }

        [HttpGet]
        public ActionResult Xoa(int? id)
        {
            //lấy sản phầm cần chỉnh sửa dựa vào id...

            //trang đường dẫn không hợp lệ...
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPham.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }


            //load dll nhà cung cấp,loại sản phẩm...
            ViewBag.MaNCC = new SelectList(db.NhaCungCap.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", sp.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPham.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuat.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", sp.MaNSX);
            return View(sp);
        }

        [HttpPost]
        public ActionResult Xoa(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham model = db.SanPham.SingleOrDefault(n => n.MaSP == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            db.SanPham.Remove(model);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

	}
}