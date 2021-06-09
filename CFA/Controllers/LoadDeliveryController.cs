using CFA.Areas.Common;
using CFA.Areas.CoreServices;
using CFA.Areas.Helpers;
using CFAEntities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewEntities;

namespace CFA.Controllers
{
    public class LoadDeliveryController : CFAController
    {
        public async Task<ActionResult> Index()
        {
            var loadDeliveries = new List<ViewLoadDelivery>();
            try
            {

                var getResult =  DB.getList<ViewLoadDelivery>(ld=>!ld.IsDeleted);
                if (!getResult.result)
                {
                    ModelState.AddModelError("", SystemMessages.FailedToGetLoadDelivery);
                    return View(loadDeliveries);
                }
                return View(getResult.List);
            }
            catch (Exception ex)
            {
                await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                ModelState.AddModelError("",SystemMessages.Exception);
                return View(loadDeliveries);

            }
        }

        public async Task<ActionResult> Create()
        {
            var fishTypesList = new List<FishType>();
            var fishBoatsList = new List<FishBoat>();
            try
            {
                
                var getFishTypesResult = await DB.getList<FishType>();
                if (getFishTypesResult.result)
                {
                    fishTypesList = getFishTypesResult.List;
                }
                var getfishBoatsResult = await DB.getList<FishBoat>();
                if (getFishTypesResult.result)
                {
                    fishBoatsList = getfishBoatsResult.List;
                }
                ViewBag.FishTypesList = fishTypesList;
                ViewBag.FishBoatsList = fishBoatsList;
                return View();
            }
            catch (Exception ex)
            {
                await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                ViewBag.FishTypesList = fishTypesList;
                ViewBag.FishBoatsList = fishBoatsList;
                return View();
            }
        }
        [HttpPost]
        public async Task<ActionResult> Create(LoadDelivery loadDelivery)
        {
            //loadDelivery = new LoadDelivery();
            try
            {
                var vaildatorResult = await BV.insertLoadDeliveryValidator(loadDelivery.FishBoatNo, loadDelivery.FishTypeNo);
                if (!vaildatorResult)
                {
                    ModelState.AddModelError("", "Entered data not Vailed.");
                    return View(loadDelivery);
                }
                var insertResult=await CS.insertLoadDelivery(loadDelivery.FishBoatNo, loadDelivery.FishTypeNo, loadDelivery.Quantity);
                if (insertResult)
                {
                    return RedirectToAction("index", "LoadDelivery");
                }
                else
                {
                    ModelState.AddModelError("", "Could Not Insert Load Delivery");
                    return View(loadDelivery);
                }
            }
            catch (Exception ex)
            {
                await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                ModelState.AddModelError("", ex.Message);
                return View(loadDelivery);
            }
        }

        public async Task<ActionResult> Delete(int loadNo)
        {
            try
            {
                var deleteResult = await CS.deleteLoadDelivery(loadNo);
                if (!deleteResult.result)
                {
                    Alert(deleteResult.message, Constants.NotificationType.warning);

                }
                else
                {
                    Alert(SystemMessages.DeletedSucssfully, Constants.NotificationType.success);
                }
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                //ModelState.AddModelError("", ex.Message);
                Alert(SystemMessages.Exception, Constants.NotificationType.error);
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
