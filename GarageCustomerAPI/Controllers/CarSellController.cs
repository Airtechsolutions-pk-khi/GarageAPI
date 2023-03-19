using BAL.Repositories;
using DAL.DBEntities;
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
    /// <summary>
    /// Sell Car
    /// </summary>
    [RoutePrefix("api")]
    public class CarSellController : ApiController
    {
        /// <summary>
        /// Sell Car
        /// </summary>
        carSellRepository carSellRepo;
        /// <summary>
        /// Sell Car
        /// </summary>
        public CarSellController()
        {
            carSellRepo = new carSellRepository(new Garage_Entities());
        }

        /// <summary>
        /// Settings for Carsell
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("carsellsetting/all")]
        public async Task<CarSellRsp> CarSellList()
        {
            return await carSellRepo.GetCarSellList();
        }

        /// <summary>
        /// Add Car Sell
        /// </summary>
        /// <param name="carSell"></param>
        /// <returns></returns>
        [Route("sell/car")]
        public CarSellInsertRsp AddCar(CarSellList carSell)
        {
            return carSellRepo.InsertCarSell(carSell);
        }

        /// <summary>
        /// Add Car to Favourite
        /// </summary>
        /// <param name="obj">CarSellID for the Identity || Status Id (1 Add and 2 for Remove)</param>
        /// <returns></returns>
        [Route("favourite/car")]
        public Rsp FavouriteCar(CarFavouriteList obj)
        {
            return carSellRepo.InsertCarFavourite(obj);
        }

    }
}
