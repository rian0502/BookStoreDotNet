using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Helper;
using WebApp.Models;
using WebApp.Models.DataModel;

namespace WebApp.Controllers
{
    public class KategoriBukuController : Controller
    {
        readonly KategoriBukuDataModel kbm = new KategoriBukuDataModel();
        // GET: KategoriBuku
        public ActionResult Index()
        {
            List<KategoriBuku> data = kbm.AllKategori();
            return View(data);
        }

        // GET: KategoriBuku/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: KategoriBuku/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: KategoriBuku/Create
        [HttpPost]
        public ActionResult Create(KategoriBuku model)
        {
            try
            {
                bool insert = kbm.InsertKategori(model.NamaKategori);
                this.SetMessage("Kategori berhasil ditambahkan!", true);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                this.SetMessage(ex.Message, false);
                return View();
            }
        }

        // GET: KategoriBuku/Edit/5
        public ActionResult Edit(Guid id)
        {
            KategoriBuku kb = kbm.GetKategoriById(id);
            return View(kb);
        }

        // POST: KategoriBuku/Edit/5
        [HttpPost]
        public ActionResult Edit(Guid id, KategoriBuku model)
        {
            try
            {

                kbm.UpdateKategori(id, model.NamaKategori);
                this.SetMessage("Kategori berhasil diubah!", true);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                this.SetMessage(ex.Message, false);
                return View();
            }
        }

        // POST: KategoriBuku/Delete/5
        [HttpPost]
        public ActionResult Delete(Guid id, FormCollection collection)
        {
            try
            {
                kbm.DeleteKategori(id);
                this.SetMessage("Berhasil Menghapus Data !", true);
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                this.SetMessage(e.Message, false);
                return View();
            }
        }
    }
}
