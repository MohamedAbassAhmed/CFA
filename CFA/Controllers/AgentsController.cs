using CFA.Areas.Common;
using CFA.Areas.CoreServices;
using CFA.Areas.Helpers;
using CFAEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CFA.Controllers
{
    public class AgentsController : CFAController
    {
        // GET: AgentsController
        public  async Task<ActionResult> Index()
        {
            var CFAAgents = new List<CFAAgent>();
            try {
                var getAgentsResult = DB.getList<CFAAgent>(a=>!a.IsDeleted);
                if (!getAgentsResult.result)
                {
                    ModelState.AddModelError("", SystemMessages.FailedToGetAgents);
                    return View(CFAAgents);
                }
                return View(getAgentsResult.List);
            }

            catch (Exception ex)
            {
                await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                ModelState.AddModelError("", SystemMessages.Exception);
                return View(CFAAgents);

            }
            
        }

        // GET: AgentsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AgentsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AgentsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  async Task<ActionResult> Create(CFAAgent model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var vaidatorResult = await BV.createCFAAgentValidator(model.AgentName);
                    if (!vaidatorResult)
                    {
                        ModelState.AddModelError("", SystemMessages.AgentNameAlreadyExist);
                        return View(model);
                    }
                    var insertResult=await CS.insertCFAAgent(model);
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

        // GET: AgentsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AgentsController/Edit/5
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

        // GET: AgentsController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // GET: AgentsController/Delete/5
        [HttpGet]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var deleteResult=await CS.DeleteCFAAgent(id);
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
