using CFA.Areas.Common;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TestCFA
{
    public class Tests
    {
        BusinessValidator BV = new BusinessValidator();
        CFADBContext DB = new CFADBContext();
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task fishBoatExist_notExistBoat()
        {
            var result=await BV.isFishBoatExist(-1);
            Assert.IsFalse(result);
        }
        [Test]
        public async Task fishBoatExist_existBoat()
        {
            //get a boat 
            var boat = DB.FishBoats.First();
            var result = await BV.isFishBoatExist(boat.Id);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task fishTypeExist_notExistFishType()
        {
           
            var result = await BV.isFishTypeExist( -1);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task fishTypeExist_existFishType()
        {
            var fishType = DB.FishTypes.First();
            var result = await BV.isFishTypeExist(fishType.Id);
            Assert.IsTrue(result);
        }
        [Test]
        public async Task insertLoadDeliveryValidator_ExistBoatAndFishType()
        {
            //get a boat 
            var boat = DB.FishBoats.First();
            //get fishType
            var fishType = DB.FishTypes.First();
            var result = await BV.insertLoadDeliveryValidator(boat.Id, fishType.Id);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task createCFAAgentValidator_ExistAgentName()
        {
            var CFAAgent = DB.CFAAgents.First();
            var result =await BV.createCFAAgentValidator(CFAAgent.AgentName);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task createCFAAgentValidator_notExistAgentName()
        {
            var CFAAgent = DB.CFAAgents.First();
            
            var result = await BV.createCFAAgentValidator(CFAAgent.AgentName+DateTime.Now.ToString());
            Assert.IsTrue(result);
        }
        [Test]
        public async Task createFishBoatValidator_ExistBoatName()
        {
            //get boat
            var boat = DB.FishBoats.First();
            var result=await BV.createFishBoatValidator(boat.FishBoatName);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task createFishBoatValidator_notExistBoatName()
        {
            //get boat
            var boat = DB.FishBoats.First();
            var result = await BV.createFishBoatValidator(boat.FishBoatName+DateTime.Now);
            Assert.IsTrue(result);
        }
        //createFishBoatValidator
    }
}