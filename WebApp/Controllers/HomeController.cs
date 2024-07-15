using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Highsoft.Web.Mvc.Charts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.Models.DataModel;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        readonly ProductDataModel pdm = new ProductDataModel();
        readonly KategoriBukuDataModel kbm = new KategoriBukuDataModel();
        readonly BukuDataModel bdm = new BukuDataModel();
        readonly AuthDataModel adm = new AuthDataModel();
        public ActionResult Index()
        {
            if (Session["id"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.Product = pdm.GetProducts().Count;
            ViewBag.Kategori = kbm.AllKategori().Count;
            ViewBag.Buku = bdm.FindAll((Guid)Session["id"]).Count;
            ViewBag.Users = adm.AllUser().Count;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult GetPenggunaanKategori()
        {
            return Json(new
            {
                success = true,
                data = kbm.PenggunaanKategori()
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetProduct()
        {
            return Json(
                new
                {
                 success = true,
                 data = pdm.GetProducts()
                }, JsonRequestBehavior.AllowGet);
        }
    }


}