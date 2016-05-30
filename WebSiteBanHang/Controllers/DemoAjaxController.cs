using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSiteBanHang.Controllers
{
    public class DemoAjaxController : Controller
    {
        //
        // GET: /DemoAjax/
        public ActionResult DemoAjax()
        {
            return View();
        }

        //xử lý ajax actionlnik
        public ActionResult LoadAjaxActionLink()
        {
            System.Threading.Thread.Sleep(2000);
            return Content("hello ajax...");
        }

        //xử lý LoadAjaxBeginForm
        public ActionResult LoadAjaxBeginForm( FormCollection fc)
        {
            System.Threading.Thread.Sleep(2000);
            string kq = fc["txtKetQua"].ToString();
            return Content(kq);
        }

        //xử lý LoadAjaxJquery...
        public ActionResult LoadAjaxJquery(int a, int b)
        {
            System.Threading.Thread.Sleep(2000);
            return Content((a + b).ToString());
        }
	}
}