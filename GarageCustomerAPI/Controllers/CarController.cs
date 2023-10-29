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
        /// Customer's Car Orders
        /// </summary>
        /// <remarks>
        /// - List of order against any car
        /// </remarks>
        /// <param name="carid">Mandatory</param>
        /// <param name="customerid">Mandatory</param>
        /// <returns>A4 receipt</returns>
        [HttpGet]
        [Route("order/{carid}/{customerid}")]
        public RspCustomerOrders GetOrders(int? carid = 0, int? customerid = 0)
        {
            return carRepo.GetCustomerOrders(carid, customerid);
        }

        /// <summary>
        /// Get Cars API
        /// </summary>
        /// <param name="customerid">Mandatory</param>
        /// <remarks>Get List Of Customer cars</remarks>
        /// <returns></returns>
        [Route("customer/cars/{customerid}")]
        [HttpGet]
        public CustomerCarsResponse GetCustomerCars(int? customerid = 0)
        {
            return carRepo.GetCustomerCars(customerid);
        }
        /// <summary>
        /// Get Car Order API
        /// </summary>
        /// <param name="customerid">Mandatory</param>
        /// <remarks>Get Single Car w.r.t OrderID</remarks>
        /// <returns></returns>
        [Route("car/order/{carid}/{orderid}")]
        [HttpGet]
        public CustomerCarsResponse GetCarOrder(int? carid= 0,int? orderid=0)
        {
            return carRepo.GetCarOrder(carid, orderid);
        }

    }
}
