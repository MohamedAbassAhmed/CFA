using CFA.Areas.Common;
using CFAEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CFA.Controllers
{
    public class FishBoatController : CFAController
    {
        // GET: FishBoatController
        public async Task<ActionResult> Index()
        {
            List<FishBoat> fishBoats = new List<FishBoat>();
            try
            {
                var getFishBoatsResult = await DB.getList<FishBoat>();
                if (!getFishBoatsResult.result)
                {
                    ModelState.AddModelError("", SystemMessages.FailedToGetFishBoats);
                    return View(fishBoats);
                }
                return View(getFishBoatsResult.List.Where(fb=>!fb.IsDeleted).ToList());
            }
            catch (Exception ex)
            {
                await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                ModelState.AddModelError("", SystemMessages.Exception);
                return View(fishBoats);
            }
        }

        // GET: FishBoatController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FishBoatController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FishBoatController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(FishBoat model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var vaidatorResult = await BV.createFishBoatValidator(model.FishBoatName);
                    if (!vaidatorResult)
                    {
                        ModelState.AddModelError("", SystemMessages.AgentNameAlreadyExist);
                        return View(model);
                    }
                    var insertResult = await CS.insertFishBoat(model);
                    if (!insertResult.result)
                    {
                        ModelState.AddModelError("", insertResult.message);
                    }
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View(model);
                }
            }
            catch(Exception ex)
            {
                await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                ModelState.AddModelError("", SystemMessages.Exception);
                return View(model);
            }
        }

        // GET: FishBoatController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FishBoatController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FishBoatController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // GET: FishBoatController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var deleteResult = await CS.DeleteFishBoat(id);
                if (!deleteResult.result)
                {
                    ModelState.AddModelError("", deleteResult.message);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                ModelState.AddModelError("", SystemMessages.Exception);
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
