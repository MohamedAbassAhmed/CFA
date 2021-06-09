using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;

namespace CFA.Areas.Common
{
    public class Constants
    {
        public const string SectionName = "Settings";
        public static IConfiguration _config;
        public Constants(IConfiguration config)
        {
            _config = config;
        }
        
        public static string ConnectionString = "Server=.;Database=CFADB;User Id=sa;Password=123456;MultipleActiveResultSets=true";
        public const string PurchaseOrderStatusNEW = "New";
        public const string PurchaseOrderStatusPENDDING = "Pendding";
        public const string PurchaseOrderStatusApproved = "Approved";
        public const string PurchaseOrderStatusDELIVERD = "Deliverd";

        public enum NotificationType
        {
            error,
            success,
            warning,
            info
        }
    }
}
