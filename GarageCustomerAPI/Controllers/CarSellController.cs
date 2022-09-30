using BAL.Repositories;
using DAL.DBEntities2;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GarageCustomerAPI.Controllers
{
    [RoutePrefix("api")]
    public class CarSellController : ApiController
    {
        
        carSellRepository carSellRepo;
        public CarSellController()
        {
           carSellRepo = new carSellRepository(new Garage_UATEntities2());
        }
        [HttpGet]
        [Route("carsellsetting/all")]
        public async Task<CarSellRsp> CarSellList()
        {            
            return await carSellRepo.GetCarSellList();
        }

        [Route("sell/car")]
        public CarSellInsertRsp AddCar(CarSell carSell)
        {
            return carSellRepo.InsertCarSell(carSell);
        }


    }
}
