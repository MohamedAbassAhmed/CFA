using CFA.Areas.Common;
using CFAEntities;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestCFA
{
    public class CoreServicesTest
    {
        BusinessValidator BV = new BusinessValidator();


        CFADBContext DB;
        [SetUp]
        public async Task SetUp()
        {
            DbContextOptions<CFADBContext> options = new DbContextOptionsBuilder<CFADBContext>()
  .UseSqlServer("Server=.;Database=CFADBTest;User Id=sa;Password=123456;MultipleActiveResultSets=true")
  .Options;
            DB = new CFADBContext(options);
            DB = new CFADBContext();
            //insert fish boat 
            FishBoat fishBoat = new FishBoat { FishBoatName = "fishBoatName1" };
            await DB.FishBoats.AddAsync(fishBoat);
            await DB.SaveChangesAsync();
            //insert fish type 
            FishType fishType1 = new FishType { FishTypeName = "fishTypeName1" };
            FishType fishType2 = new FishType { FishTypeName = "fishTypeName2" };
            await DB.AddRangeAsync(fishType1, fishType2);
            await DB.SaveChangesAsync();

        }

        [Test]
        public async Task t()
        {

        }
    }
}
