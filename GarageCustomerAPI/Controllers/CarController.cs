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
        carRepository carRepo;
        public CarController()
        {
            carRepo = new carRepository(new Garage_Entities());
        }

        [Route("add/car")]
        public CarRsp AddCar(Cars cars)
        {
            return carRepo.AddCar(cars);
        }

        [Route("edit/car")]
        public CarRsp EditCar(Cars cars)
        {
            return carRepo.EditCar(cars);
        }

        [HttpGet]
        [Route("order/receipt/{orderid}")]
        public OrderLetterResponse PrintLetter(string orderid)
        {
            return carRepo.OrderPrintLetter(orderid);
        }

        [Route("review/customer")]
        public ReviewRsp PostReview(ReviewsBLL obj)
        {
            return carRepo.AddReview(obj);
        }
    }
}
