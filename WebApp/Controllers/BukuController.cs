using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using NPOI.XSSF.UserModel;
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
    public class BukuController : Controller
    {
        readonly KategoriBukuDataModel kbm = new KategoriBukuDataModel();
        readonly BukuDataModel bdm = new BukuDataModel();
        // GET: Buku
        public ActionResult Index()
        {
            List<Buku> buku = bdm.FindAll((Guid)Session["id"]);
            return View(buku);
        }
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Buku/Create
        public ActionResult Create()
        {
            ViewBag.Kategori = kbm.AllKategori();
            return View();
        }

        // POST: Buku/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Buku buku, HttpPostedFileBase file)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Kategori = kbm.AllKategori();
                    return View(buku);
                }

                if (file != null && file.ContentLength > 0)
                {
                    if (file.ContentLength > 1048576)
                    {
                        ModelState.AddModelError("", "Ukuran file Sampul tidak boleh lebih dari 1MB");
                        ViewBag.Kategori = kbm.AllKategori();
                        return View(buku);
                    }
                    string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                    buku.Sampul = fileName;
                    file.SaveAs(path);
                }
                else
                {
                    ModelState.AddModelError("", "File Sampul harus diunggah.");
                    ViewBag.Kategori = kbm.AllKategori();
                    return View(buku);
                }

                buku.created_at = DateTime.Now;
                buku.updated_at = DateTime.Now;
                buku.Id_user = (Guid)Session["id"];
                bdm.InsertBuku(buku);
                this.SetMessage("Buku berhasil ditambahkan", true);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                this.SetMessage(ex.Message, false);
                ViewBag.Kategori = kbm.AllKategori();
                return View(buku);
            }
        }
        public ActionResult Edit(Guid id)
        {
            ViewBag.Kategori = kbm.AllKategori().ToList();
            Buku buku = bdm.getBukuById(id);
            return View(buku);
        }

        // POST: Buku/Edit/5
        [HttpPost]
        public ActionResult Edit(Guid id, Buku model, HttpPostedFileBase file)
        {
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    if (!string.IsNullOrEmpty(model.Sampul))
                    {
                        string fullPath = Server.MapPath("~/Uploads/" + model.Sampul);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Uploads/"), fileName);
                    file.SaveAs(path);
                    model.Sampul = fileName;
                }
                model.id = id;
                model.Id_user = (Guid)Session["id"];
                bdm.UpdateBuku(model);
                this.SetMessage("Buku berhasil diupdate", true);
                return RedirectToAction("Index");

            }
            catch (Exception e)
            {
                this.SetMessage(e.Message, false);
                return View();
            }
        }

        // POST: Buku/Delete/5
        [HttpPost]
        public ActionResult Delete(Guid id, FormCollection collection)
        {
            try
            {
                Buku buku = bdm.getBukuById(id);
                string fullPath = Request.MapPath("~/Uploads/" + buku.Sampul);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                bdm.DeleteBuku(id);
                this.SetMessage("Buku berhasil dihapus !", true);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                this.SetMessage(ex.Message, false);
                return View();
            }
        }
        public ActionResult ReportBuku()
        {
            try
            {
                string id_user = Session["id"].ToString();
                string namaUser = Session["name"].ToString();
                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(Server.MapPath("~/Reports/BukuReport.rpt"));
                reportDocument.SetParameterValue("@id_user", id_user);
                reportDocument.SetParameterValue("@nama_user", namaUser);
                Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
                return File(stream, "application/pdf");
            }
            catch (Exception e)
            {
                this.SetMessage(e.Message, false);
                return RedirectToAction("Index");
            }
        }

        public ActionResult reportExcelBuku()
        {
            try
            {
                List<Buku> books = bdm.FindAll((Guid)Session["id"]);
                using (var workbook = new XSSFWorkbook())
                {
                    var sheet = workbook.CreateSheet("Daftar Buku");
                    var headerRow = sheet.CreateRow(0);
                    headerRow.CreateCell(0).SetCellValue("No");
                    headerRow.CreateCell(1).SetCellValue("Nama Buku");
                    headerRow.CreateCell(2).SetCellValue("Kategori");
                    headerRow.CreateCell(3).SetCellValue("Halaman");
                    headerRow.CreateCell(4).SetCellValue("Sampul");
                    headerRow.CreateCell(5).SetCellValue("Sinopsis");
                    headerRow.CreateCell(6).SetCellValue("Tanggal Dibuat");
                    headerRow.CreateCell(7).SetCellValue("Tanggal Diperbarui");

                    int rowIndex = 1;
                    foreach (var buku in books)
                    {
                        var row = sheet.CreateRow(rowIndex);
                        row.CreateCell(0).SetCellValue(rowIndex);
                        row.CreateCell(1).SetCellValue(buku.NamaBuku);
                        row.CreateCell(2).SetCellValue(buku.KategoriBuku.NamaKategori);
                        row.CreateCell(3).SetCellValue(buku.Halaman.ToString());
                        row.CreateCell(4).SetCellValue("https://localhost:44319/Uploads/" + buku.Sampul);
                        row.CreateCell(5).SetCellValue(buku.Sinopsis);
                        row.CreateCell(6).SetCellValue(buku.created_at.ToString("yyyy-MM-dd HH:mm:ss"));
                        row.CreateCell(7).SetCellValue(buku.updated_at.ToString("yyyy-MM-dd HH:mm:ss"));
                        rowIndex++;
                    }
                    byte[] fileContents;
                    using (var memoryStream = new MemoryStream())
                    {
                        workbook.Write(memoryStream);
                        fileContents = memoryStream.ToArray();
                    }
                    workbook.Close();
                    return File(
                        fileContents,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "DaftarBuku.xlsx"
                    );
                }
            }
            catch (Exception e)
            {
                this.SetMessage(e.Message, false);
                return RedirectToAction("Index");
            }
        }
    }
}