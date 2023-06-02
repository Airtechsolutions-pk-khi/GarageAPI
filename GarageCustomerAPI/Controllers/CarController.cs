using BAL.Repositories;
using DAL.DBEntities;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GarageCustomerAPI.Controllers
{
    [RoutePrefix("api")]
    public class CarController : ApiController
    {
        /// <summary>
        /// Cars
        /// </summary>
        carRepository carRepo;
        /// <summary>
        /// Cars
        /// </summary>
        public CarController()
        {
            carRepo = new carRepository(new Garage_Entities());
        }

        /// <summary>
        /// Add New Cars
        /// </summary>
        /// <param name="cars"></param>
        /// <returns></returns>
        [Route("add/car")]
        [Authorize]
        public CarRsp AddCar(Cars cars)
        {
            return carRepo.AddCar(cars);
        }

        /// <summary>
        /// Edit Cars
        /// </summary>
        /// <param name="cars"></param>
        /// <returns></returns>
        [Route("edit/car")]
		[Authorize]
		public CarRsp EditCar(Cars cars)
        {
            return carRepo.EditCar(cars);
        }

        /// <summary>
        /// A4 Order receipt
        /// </summary>
        /// <param name="orderid">Mandatory</param>
        /// <returns>A4 receipt</returns>
        [HttpGet]
        [Route("order/receipt/{orderid}")]
		[Authorize]
		public OrderLetterResponse PrintLetter(string orderid = "0")
        {
            return carRepo.OrderPrintLetter(orderid);
        }

      
    }
}
