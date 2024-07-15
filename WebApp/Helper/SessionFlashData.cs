using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Helper
{
    public static class SessionFlashData
    {
        public static void SetMessage(this Controller controller, string message, bool status)
        {
            if (status)
            {
                controller.TempData["success"] = message;
            }
            else
            {
                controller.TempData["error"] = message;
            }
        }
    }
}