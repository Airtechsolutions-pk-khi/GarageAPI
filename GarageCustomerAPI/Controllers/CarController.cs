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
        public OrderLetterResponse PrintLetter(string orderid = "0")
        {
            return carRepo.OrderPrintLetter(orderid);
        }

        /// <summary>
        /// Add Review to Locations
        /// </summary>
        /// <param name="obj">(Status 1 for Active,3 for Delete)</param>
        /// <returns></returns>
        [Route("review/customer")]
        public ReviewRsp PostReview(ReviewsBLL obj)
        {
            return carRepo.AddReview(obj);
        }

        /// <summary>
        /// Update Review to Locations
        /// </summary>
        /// <param name="obj">(Status 1 for Active,3 for Delete) \n Add Like/Dislike Count to existing value</param>
        /// <returns></returns>
        [Route("review/customer/update")]
        public ReviewRsp PostReviewUpdate(ReviewsBLL obj)
        {
            return carRepo.UpdateReview(obj);
        }
    }
}
