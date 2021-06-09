using CFA.Areas.Common;
using CFA.Areas.CoreServices;
using CFA.Areas.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CFA.Controllers
{
    public class CFAController : Controller
    {
        public ExceptionHandeler EXH = new ExceptionHandeler();
        public DBAdapter DB = new DBAdapter();
        public CoreServices CS = new CoreServices();
        public BusinessValidator BV = new BusinessValidator();
        public void Alert(string message,Constants.NotificationType notificationType)
        {
            var msg = "sweetAlert('" + notificationType.ToString().ToUpper() + "', '" + message + "','" + notificationType + "')" + "";
            TempData["notification"] = msg;
        }

    }
}
