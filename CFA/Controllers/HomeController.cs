using CFA.Areas.CoreServices;
using CFA.Models;
using CFAEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CFA.Controllers
{
    public class HomeController : Controller
    {
        private DBAdapter DB = new DBAdapter();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                return RedirectToAction("Index", "LoadDelivery");
                //FishBoat fishBoat = new FishBoat
                //{
                //    FishBoatName = "name",

                //};
                //await DB.addRecord(fishBoat);
                //fishBoat.FishBoatName = "name2";
                //await DB.update(fishBoat);
                //await DB.deleteRecord<FishBoat>(fishBoat.Id);
                //var loggs =await DB.getList<DatabaseLog>();
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
