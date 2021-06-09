using CFA.Areas.Common;
using CFA.Areas.Helpers;
using CFAEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CFA.Areas.CoreServices
{
    public class DBAdapter
    {
        //private HttpContext _httpContext ;
        private IHttpContextAccessor _httpContext { get; set; }
        public DBAdapter()
        {
            _httpContext = new HttpContextAccessor();
        }
        ExceptionHandeler EXH = new ExceptionHandeler();
        public CFADBContext DB = new CFADBContext();





        public async Task<(bool result, string message)> addRecord<T>(T model) where T : class
        {
            try
            {

                DB.Add<T>(model);
                await DB.SaveChangesAsync();
                await saveDatabaseLog<T>(model, "Add " + model.ToString());
                return (result: true, message: "");
            }
            catch (Exception ex)
            {

                return (result: false, message: ex.Message);
            }
        }
        public async Task<(bool result, string message)> addRange<T>(List<T> Models) where T : class
        {
            try
            {
                await DB.Set<T>().AddRangeAsync(Models);
                await DB.SaveChangesAsync();
                await saveDatabaseLog<T>(Models, "Add Range " + Models.ToString());

                return (result: true, message: "");
            }
            catch (Exception ex)
            {
                await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return (result: false, message: ex.Message);
            }
        }

        public async Task<(bool result, string message)> update<T>(T Model) where T : class
        {
            try
            {

                DB.Entry(Model).State = EntityState.Detached;
                DB.Entry(Model).State = EntityState.Modified;
                await saveDatabaseLog(Model, "Update ");
                await DB.SaveChangesAsync();

                return (result: true, message: "");
            }
            catch (Exception ex)
            {
                await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return (result: false, message: ex.Message);
            }
        }
        public async Task<(bool result, string message)> deleteRecord<T>(dynamic Id) where T : class
        {
            try
            {
                var findResult = await findRecord<T>(Id);

                DB.Entry(findResult.Item2).State = EntityState.Deleted;
                await saveDatabaseLog(findResult.Item2, "Delete " + findResult.Item2.ToString());
                await DB.SaveChangesAsync();

                return (result: true, message: "");
            }
            catch (Exception ex)
            {
                await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return (result: false, message: ex.Message);
            }
        }

        public async Task<(bool result, T Model)> findRecord<T>(dynamic Id) where T : class
        {
            try
            {
                T Record = null;
                
                    Record = await DB.Set<T>().FindAsync(Id);
                return (result: true, Model: Record);
            }
            catch (Exception ex)
            {

                await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return (result: false, Model: null);
            }

        }

        public async Task<(bool result, T Model)> findRecord<T>(Func<T, bool> condition) where T : class
        {
            try
            {
                T Record = null;
                await Task.Factory.StartNew(() =>
                {
                    Record = DB.Set<T>().FirstOrDefault<T>(condition);
                });
                return (result: true, Model: Record);
            }
            catch (Exception ex)
            {
                await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return (result: false, Model: null);
            }

        }

        public async Task<(bool result, List<T> List)> getList<T>() where T : class
        {
            try
            {
                List<T> Records = null;
                Records = await DB.Set<T>().ToListAsync();

                return (result: true, List: Records);
            }
            catch (Exception ex)
            {
                await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return (result: false, List: null);
            }

        }

        public (bool result, List<T> List) getList<T>(Func<T, bool> condition) where T : class
        {
            try
            {
                List<T> list =  DB.Set<T>().Where(condition).ToList();
                return (true, list);
            }
            catch (Exception)
            {
                return (false,null);
            }
        }

        public async Task<bool> saveDatabaseLog<T>(T Model, string operationName) where T : class
        {
            try
            {
                JavaScriptSerializer JSS = new JavaScriptSerializer();
                DatabaseLog databaseLog = new DatabaseLog
                {
                    Operation = operationName + " " + JSS.Serialize(Model),
                    PageName = getPageName(),
                    UserName = _httpContext.HttpContext.User.Identity.Name

                };
                DB.Add<DatabaseLog>(databaseLog);
                await DB.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }
        public async Task<bool> saveDatabaseLog<T>(List<T> Model, string operationName) where T : class
        {
            try
            {
                JavaScriptSerializer JSS = new JavaScriptSerializer();
                DatabaseLog databaseLog = new DatabaseLog
                {
                    Operation = operationName + " " + JSS.Serialize(Model),
                    PageName = getPageName(),
                    UserName = _httpContext.HttpContext.User.Identity.Name

                };
                DB.Add<DatabaseLog>(databaseLog);
                await DB.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                await EXH.LogException(ex, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }
        public string getPageName()
        {
            //var httpContext= this HttpContext;
            if (_httpContext == null)
                throw new ArgumentNullException(nameof(_httpContext));

            Endpoint endpoint = _httpContext.HttpContext.GetEndpoint();
            var CAD = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
            return CAD.ControllerName + "/" + CAD.ActionName;
        }

    }
}
