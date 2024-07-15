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
    public class LoginController : Controller
    {
        readonly AuthDataModel adm = new AuthDataModel();
        readonly PasswordHelper ps = new PasswordHelper();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(LoginViewModel model)
        {
            try
            {
                if (model.email == null || model.email == "")
                {
                    this.SetMessage("Email harus di isi", false);
                    return View();
                }
                Users user = adm.LoginUsers(model.email);
                if (user != null)
                {

                    bool data = ps.PasswordVertify(model.password, user.Password, user.Salt);
                    if (data)
                    {
                        Session["isLogin"] = true;
                        Session["id"] = user.Id;
                        Session["email"] = user.Email;
                        Session["name"] = user.Name;
                        TempData["success"] = "berhasil login";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        this.SetMessage("Email dan Password tidak cocok !", false);
                        return View();

                    }
                }
                else
                {
                    this.SetMessage("User tidak ditemukan", false);
                    return View();
                }
            }
            catch (Exception e)
            {
                this.SetMessage(e.Message, false);
                return View();
            }
        }
        [HttpPost]
        public ActionResult ChangePassword(string newPassword)
        {
            try
            {
                string salt = ps.GenerateSalt();
                adm.ChangePassword(Guid.Parse(Session["id"].ToString()), ps.HashPassword(newPassword, salt), salt);
                return Json(new { success = true, message = "Password berhasil diubah" });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }
        public ActionResult logout()
        {
            Session.Abandon();
            this.SetMessage("Berhasil Logout!", true);
            return RedirectToAction("Index", "Login");

        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            try
            {
                string salt = ps.GenerateSalt();
                adm.RegisterUser(model.Name, model.Email, ps.HashPassword(model.Password, salt), salt);
                this.SetMessage("Berhasil Melakukan registrasi", true);
                return RedirectToAction("Index", "Login");
            }
            catch (Exception e)
            {
                this.SetMessage(e.Message, false);
                return View(model);
            }
        }
    }
}