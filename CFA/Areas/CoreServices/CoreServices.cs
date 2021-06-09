using CFA.Areas.Common;
using CFA.Areas.Helpers;
using CFA.Models;
using CFAEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewEntities;

namespace CFA.Areas.CoreServices
{
    public class CoreServices
    {
        ExceptionHandeler EXH = new ExceptionHandeler();
        DBAdapter DB = new DBAdapter();
        public async Task<bool> insertLoadDelivery(int boatNo, int fishTypeNo, int quantity)
        {
            try
            {
                LoadDelivery loadDelivery = new LoadDelivery
                {
                    FishBoatNo = boatNo,
                    FishTypeNo = fishTypeNo,
                    Quantity = quantity,
                    BatchNo = ""
                };

                var insertResult = await DB.addRecord(loadDelivery);
                if (!insertResult.result)
                {
                    return false;
                }
                loadDelivery.BatchNo = await genarateBatchNo(boatNo, fishTypeNo, quantity, loadDelivery.Id);
                var updateResult = await DB.update<LoadDelivery>(loadDelivery);
                if (updateResult.result)
                {
                    var insertStockResult = await insertLoadDeliveryStock(loadDelivery);
                    return insertStockResult.result;
                }
                return updateResult.result;
            }
            catch (Exception ex)
            {
                await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        public async Task<(bool result, string message)> insertLoadDeliveryStock(LoadDelivery loadDelivery)
        {
            try
            {
                LoadDeliveryStock loadDeliveryStock = new LoadDeliveryStock
                {
                    LoadDeliveryNo = loadDelivery.Id,
                    Quantity = loadDelivery.Quantity
                };
                return await DB.addRecord<LoadDeliveryStock>(loadDeliveryStock);
            }
            catch (Exception ex)
            {
                await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return (false, SystemMessages.Exception);
            }
        }
        public async Task<string> genarateBatchNo(int boat, int fishType, double quantity, int deliveryLoadNo)
        {
            try
            {
                string batchNo = "";
                var todayPreCode = DateTime.Today.ToString("yy-MM-dd").Replace("-", "").ToString();
                batchNo += todayPreCode + "-" + fishType + "-" + boat + "-" + deliveryLoadNo;
                return batchNo;
            }
            catch (Exception ex)
            {
                await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return "";

            }
        }
        public async Task<(bool result, string message)> approvePerchaseOrder(int purshaseNo)
        {
            try
            {
                var findResult = await DB.findRecord<PurchaseOrder>(purshaseNo);
                PurchaseOrder purchaseOrder = findResult.Model;
                var supplyResult = await supplyPurchaseOrder(purshaseNo);
                if (!supplyResult.result)
                {
                    purchaseOrder.PurchaseOrderStatus = Constants.PurchaseOrderStatusPENDDING;
                    await DB.update<PurchaseOrder>(purchaseOrder);
                    return (false, supplyResult.message);
                }
                purchaseOrder.PurchaseOrderStatus = Constants.PurchaseOrderStatusApproved;
                await DB.update<PurchaseOrder>(purchaseOrder);
                return (true, "");
            }
            catch (Exception ex)
            {

                await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return (false, ex.Message);
            }
        }
        public async Task<bool> initlaizePurchaseOrder(int agentNo, Dictionary<int, int> types_quantites)
        {
            try
            {
                PurchaseOrder purchaseOrder = new PurchaseOrder
                {
                    AgentNo = agentNo,
                    PurchaseOrderStatus = Constants.PurchaseOrderStatusNEW,
                };
                await DB.addRecord(purchaseOrder);
                await addPurchaseOrderDetails(purchaseOrder.Id, types_quantites);
                await approvePerchaseOrder(purchaseOrder.Id);
                return true;
            }
            catch (Exception ex)
            {
                await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return false;
            }

        }
       public async Task<(bool result ,string message)> deleteLoadDelivery(int loadDeliveryNo)
        {
            try
            {
                var getLoadDeliveryResult =await DB.findRecord<LoadDelivery>(loadDeliveryNo);
                if (!getLoadDeliveryResult.result)
                {
                    return (false, SystemMessages.FailedToGetLoadDelivery);
                }
                getLoadDeliveryResult.Model.IsDeleted = true;
                return await DB.update<LoadDelivery>(getLoadDeliveryResult.Model);
            }
            catch (Exception ex)
            {
                await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return (false, ex.Message);
            }
        }
        public async Task<bool> addPurchaseOrderDetails(int purchaseOrderNo, Dictionary<int, int> types_quantites)
        {
            try
            {
                List<PurchaseOrderDetails> purchaseOrderDetails = new List<PurchaseOrderDetails>();
                foreach (var detail in types_quantites)
                {
                    PurchaseOrderDetails purchaseOrderDetail = new PurchaseOrderDetails
                    {
                        FishTypeNo = detail.Key,
                        PurchaseOrderNo = purchaseOrderNo,
                        Quantity = detail.Value
                    };
                    purchaseOrderDetails.Add(purchaseOrderDetail);
                }
                await DB.addRange(purchaseOrderDetails);
                return true;
            }
            catch (Exception ex)
            {
                await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return false;
            }

        }
        public async Task<(bool result, List<FishTypeStockViewModel> List)> getFishTypesAvailableQuantities()
        {
            try
            {
                var fishTypesList = await DB.getList<FishType>();
                var fishTypeStockList = new List<FishTypeStockViewModel>();
                if (!fishTypesList.result)
                    return (false, null);
                foreach (var fishType in fishTypesList.List)
                {
                    var fishTypeStock = new FishTypeStockViewModel()
                    {
                        FishTypeName = fishType.FishTypeName,
                        FishTypeNo = fishType.Id,
                        Quantity = await getFishTypeStockQuantity(fishType.Id)
                    };
                    fishTypeStockList.Add(fishTypeStock);

                }
                return (true, fishTypeStockList);
            }
            catch (Exception ex)
            {
                await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return (false, null);
            }
        }
        public async Task<int> getFishTypeStockQuantity(int fishTypeNo)
        {
            try
            {
                var fishTypeStock = await DB.getList<ViewFishTypeStock>();
                if (!fishTypeStock.result)
                    return 0;
                var quantity = fishTypeStock.List.Where(fts => fts.FishTypeNo == fishTypeNo).Select(e => e.Quantity).Sum();
                return quantity;

            }
            catch (Exception ex)
            {
                await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return 0;
            }
        }
        /// <summary>
        /// This method role is supply the purchase orders from the stock 
        /// How It Work:-
        /// -First : it checks if all types of fish that are on the order are exeist in the sock with there quantities
        /// -Second : it order the stock base on the load Delivery Time (using First In First Out)
        /// -Third : it save that it used the specific stock row to supply specific Quantity for specific orderDetail
        /// -4th : if the Third did not supply all quantity the order need it try to use the next load delivery
        /// -5th :it repeat the "Third" and "4th"until the quantity found 
        /// -6th : it repeat the "Third","4th" and "5th" for all the fish types needed on the order
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns>
        /// the base return sign is the result flag it will be true only if all order details are supplyed otherwise it returns false with readable message 
        /// </returns>
        public async Task<(bool result, string message)> supplyPurchaseOrder(int orderId)
        {
            try
            {
                var getPurchaseOrderDetailsResult = DB.getList<PurchaseOrderDetails>(pod => pod.PurchaseOrderNo == orderId);
                if (!getPurchaseOrderDetailsResult.result)
                {
                    return (false, SystemMessages.FailedToGetPurchaseOrderDetails);
                }
                var purchaseOrderDetails = getPurchaseOrderDetailsResult.List;
                foreach (var POD in purchaseOrderDetails)
                {
                    if (POD.Quantity > await getFishTypeStockQuantity(POD.FishTypeNo))
                    {
                        var getFishTypeResult = await DB.findRecord<FishType>(POD.FishTypeNo);
                        if (getFishTypeResult.result && getFishTypeResult.Model != null)
                        {
                            return (false, SystemMessages.FishTypeIsOutOfStock + " :" + getFishTypeResult.Model.FishTypeName);
                        }
                        else
                            return (false, SystemMessages.FishTypeIsOutOfStock);
                    }
                }
                var loadDeliveryStockResult = await DB.getList<ViewLoadDeliveryStock>();
                if (!loadDeliveryStockResult.result)
                {
                    return (false, SystemMessages.FailedToGetLoadDeliveryStock);
                }
                foreach (var POD in purchaseOrderDetails)
                {
                    var loadDeliveryStockForOneFishType = loadDeliveryStockResult.List.Where(lds => lds.FishTypeNo == POD.FishTypeNo && lds.Quantity > 0).OrderBy(lds => lds.LoadDeliveryDate).ToList();
                    await supplyPurchaseOrderDetail(POD, loadDeliveryStockForOneFishType);
                }
                return (true, null);
            }
            catch (Exception ex)
            {
                await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return (false, "Exception");
            }
        }
        public async Task supplyPurchaseOrderDetail(PurchaseOrderDetails POD, List<ViewLoadDeliveryStock> loadDeliveryStocks)
        {
            try
            {
                PurchaseOrderSupply purchaseOrderSupply = new PurchaseOrderSupply();
                if (loadDeliveryStocks.First().Quantity >= POD.Quantity)
                {
                    purchaseOrderSupply.LoadDeliveryNo = loadDeliveryStocks.First().LoadDeliveryNo;
                    purchaseOrderSupply.PurchaseOrderNo = POD.PurchaseOrderNo;
                    purchaseOrderSupply.Quantity = POD.Quantity;

                    await DB.addRecord<PurchaseOrderSupply>(purchaseOrderSupply);
                    //LDS.Quantity -= POD.Quantity;
                    var getLoadDeliveryStockResult = await DB.findRecord<LoadDeliveryStock>(loadDeliveryStocks.First().Id);
                    if (!getLoadDeliveryStockResult.result)
                    {

                    }
                    LoadDeliveryStock loadDeliveryStock = getLoadDeliveryStockResult.Model;
                    loadDeliveryStock.Quantity -= POD.Quantity;
                    await DB.update<LoadDeliveryStock>(loadDeliveryStock);
                    return;
                }
                foreach (var LDS in loadDeliveryStocks)
                {
                    if (POD.Quantity > 0)
                    {
                        purchaseOrderSupply = new PurchaseOrderSupply
                        {
                            LoadDeliveryNo = LDS.LoadDeliveryNo,
                            PurchaseOrderNo = POD.PurchaseOrderNo,
                            Quantity = Math.Min(LDS.Quantity, POD.Quantity)
                        };
                       
                        await DB.addRecord<PurchaseOrderSupply>(purchaseOrderSupply);
                        LoadDeliveryStock loadDeliveryStock = (await DB.findRecord<LoadDeliveryStock>(LDS.Id)).Model;
                        loadDeliveryStock.Quantity -= Math.Min(LDS.Quantity, POD.Quantity);
                        await DB.update<LoadDeliveryStock>(loadDeliveryStock);
                        POD.Quantity -= LDS.Quantity;
                    }
                    else
                    {
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        public async Task<(bool result, string message)> insertCFAAgent(CFAAgent model)
        {
            try
            {
                return await DB.addRecord<CFAAgent>(model);
            }
            catch (Exception ex)
            {
                await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return (false, ex.Message);
            }
        }
        public async Task<(bool result, string message)> insertFishBoat(FishBoat model)
        {
            try
            {
                return await DB.addRecord<FishBoat>(model);
            }
            catch (Exception ex)
            {
                await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return (false, ex.Message);
            }
        }

        public async Task<(bool result, string message)> DeleteCFAAgent(int AgentNo)
        {
            try
            {
                var getAgentResult = await DB.findRecord<CFAAgent>(AgentNo);
                if (!getAgentResult.result)
                {
                    return (false, SystemMessages.FailedToGetAgent);
                }
                getAgentResult.Model.IsDeleted = true;
                return await DB.update<CFAAgent>(getAgentResult.Model);
            }
            catch (Exception ex)
            {
                await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return (false, ex.Message);
            }
        }
        public async Task<(bool result, string message)> DeleteFishBoat(int AgentNo)
        {
            try
            {
                var getBoatResult = await DB.findRecord<FishBoat>(AgentNo);
                if (!getBoatResult.result)
                {
                    return (false, SystemMessages.FailedToGetAgent);
                }
                getBoatResult.Model.IsDeleted = true;
                return await DB.update<FishBoat>(getBoatResult.Model);
            }
            catch (Exception ex)
            {
                await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return (false, ex.Message);
            }
        }
    }
}
