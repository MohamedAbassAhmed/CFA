using CFA.Areas.Common;
using CFA.Areas.CoreServices;
using CFA.Areas.Helpers;
using CFA.Models;
using CFAEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewEntities;

namespace CFA.Controllers
{
    public class PurchaseOrderController : CFAController
    {
        public async Task<IActionResult> Index()
        {
            List<ViewPurchaseOrder> purchaseOrders = new List<ViewPurchaseOrder>();
            try
            {
                var getOrdersResult = await DB.getList<ViewPurchaseOrder>();
                if (!getOrdersResult.result)
                {
                    ModelState.AddModelError("", SystemMessages.FailedToGetPurchaseOrders);
                    return View(purchaseOrders);
                }
                return View(getOrdersResult.List);
            }
            catch (Exception ex)
            {
                await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                ModelState.AddModelError("", SystemMessages.Exception);
                return View(purchaseOrders);
            }
        }
        public async Task<IActionResult> Create()
        {
            var fishTypesList = new List<FishType>();
            //var fishBoatsList = new List<FishBoat>();
            try
            {

                var getFishTypesResult = await DB.getList<FishType>();
                if (getFishTypesResult.result)
                {
                    fishTypesList = getFishTypesResult.List;
                }
                else
                {
                    ModelState.AddModelError("", SystemMessages.FailedToGetFishTypes);
                }

                ViewBag.FishTypesList = fishTypesList;
                List<CreatePurchaseOrderViewModel> viewModel = new List<CreatePurchaseOrderViewModel>();
                foreach (var item in fishTypesList)
                {
                    viewModel.Add(new CreatePurchaseOrderViewModel
                    {
                        FishTypeName = item.FishTypeName,
                        FishTypeNo = item.Id
                    });
                }
                return View(viewModel);
            }
            catch (Exception ex)
            {
                await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                ViewBag.FishTypesList = fishTypesList;
                return View();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(List<CreatePurchaseOrderViewModel> model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var agentNo = 1;
                model = model.Where(m => m.Quantity > 0).ToList();
                if (model.Count < 1)
                {
                    ModelState.AddModelError("", SystemMessages.PleaseSelectAtLeastOneType);
                    return RedirectToAction(nameof(Create));

                }
                Dictionary<int, int> types_quantities = model.ToDictionary(m => m.FishTypeNo, m => m.Quantity);
                var result = await CS.initlaizePurchaseOrder(agentNo, types_quantities);
                if (result)
                {
                    
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                ModelState.AddModelError("", SystemMessages.Exception);
                return RedirectToAction(nameof(Create));
            }
        }
        public async Task<IActionResult> ApproveOrder(int orderNo)
        {
            try
            {
                var approveResult=await CS.approvePerchaseOrder(orderNo);
                if (!approveResult.result)
                {
                    ModelState.AddModelError("", approveResult.message);
                    
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                ModelState.AddModelError("", SystemMessages.Exception);
                return RedirectToAction(nameof(Create));
            }
        }
    }
}
