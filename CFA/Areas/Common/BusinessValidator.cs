using CFA.Areas.CoreServices;
using CFA.Areas.Helpers;
using CFAEntities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CFA.Areas.Common
{
    public class BusinessValidator
    {
        ExceptionHandeler EXH = new ExceptionHandeler();
        DBAdapter DB = new DBAdapter();
        public async Task<bool> insertLoadDeliveryValidator(int boatNo, int fishTypeNo)
        {
            try
            {
                //check the boat
                if (!await isFishBoatExist(boatNo))
                {
                    return false;
                }
                //check the fishType
                
                return await isFishTypeExist(fishTypeNo);
            }
            catch (Exception ex)
            {
                await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return false;
            }

        }
        public async Task<bool> isFishBoatExist(int boatNo)
        {
            var fishBoat = await DB.findRecord<FishBoat>(boatNo);
            if (!fishBoat.result || fishBoat.Model == null)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> isFishTypeExist(int fishTypeNo)
        {
            var fishType = await DB.findRecord<FishType>(fishTypeNo);
            if (!fishType.result || fishType.Model == null)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> createCFAAgentValidator(string agentName)
        {
            try
            {
                var getAgentWithNameResult = await DB.findRecord<CFAAgent>(a => a.AgentName == agentName);
                if (!getAgentWithNameResult.result || getAgentWithNameResult.Model != null)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }
        public async Task<bool> createFishBoatValidator(string boatName)
        {
            try
            {
                var getBoatWithNameResult = await DB.findRecord<FishBoat>(a => a.FishBoatName == boatName);
                if (!getBoatWithNameResult.result || getBoatWithNameResult.Model != null)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }
        //public async Task<bool> selectFishTypesValidator( List<int> fishTypesOrdered)
        //{
        //    try
        //    {
        //        foreach (var fishTypeNo in fishTypesOrdered)
        //        {
        //            var getResult = await DB.findRecord<FishType>(fishTypeNo);
        //            if (!getResult.result || getResult.Model != null)
        //                return false;
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
        //        return false;
        //    }
        //}
        //public async Task<bool> selectAgencyValidator(int agencyNo)
        //{
        //    try
        //    {
        //        var getAgencyResult = await DB.findRecord<CFAAgent>(agencyNo);
        //        if (!getAgencyResult.result || getAgencyResult.Model != null)
        //            return false;
        //        else
        //            return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
        //        return false;
        //    }
        //}
    }
}
