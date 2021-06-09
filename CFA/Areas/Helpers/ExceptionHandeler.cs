using CFA.Areas.Common;
using CFA.Areas.CoreServices;
using CFAEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CFA.Areas.Helpers
{
    public class ExceptionHandeler
    {
        CFADBContext DB = new CFADBContext();
        public async Task LogException(Exception exception,string className="",string methodName="")
        {
            try {
                ExceptionLog exceptionLog = new ExceptionLog
                {
                    ClassName = className,
                    MethodName = methodName,
                    Message = exception.Message,
                    DateTime = DateTime.Now,
                    InnerException = exception.InnerException?.Message,
                    StackTrace = exception.StackTrace
                };
                await DB.AddAsync<ExceptionLog>(exceptionLog);

            }catch(Exception ex)
            {

            }
        }
    }
}
