using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using WebApp.Helper;
using WebApp.Models;
using WebApp.Models.DataModel;

namespace WebApp.Controllers
{
    public class ProductController : Controller
    {
        readonly ProductDataModel pdm = new ProductDataModel();

        public ActionResult ReportProduct()
        {
            try
            {
                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(Server.MapPath("~/Reports/ProductReport.rpt"));
                Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
                return File(stream, "application/pdf");
            }
            catch (Exception e)
            {
                this.SetMessage(e.Message, false);
                return RedirectToAction("Index");
            }
        }
        // GET: Product
        public ActionResult Index()
        {
            ViewBag.Products = pdm.GetProducts();
            return View();
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(Product model)
        {
            try
            {
                if (model.Name.IsNullOrWhiteSpace() || model.Price == 0)
                {
                    return View();
                }
                pdm.InsertProduct(model.Name, model.Price);
                this.SetMessage("Addded Successfully !", true);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                this.SetMessage(ex.Message, false);
                return View();
            }
        }

        // GET: Product/Edit/5
        public ActionResult Edit(Guid id)
        {
            Product product = pdm.getProductById(id);
            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(Guid id, Product model)
        {
            try
            {
                pdm.UpdateProduct(id, model.Name, model.Price);
                this.SetMessage("Updated Successfully !", true);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                this.SetMessage(ex.Message, false);
                return View();
            }
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                pdm.DeleteProduct(id);
                this.SetMessage("Deleted Successfully !", true);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                this.SetMessage(ex.Message, false);
                return View();
            }
        }
    }
}
