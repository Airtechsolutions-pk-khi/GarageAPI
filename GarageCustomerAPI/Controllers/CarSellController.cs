using BAL.Repositories;
using DAL.DBEntities;
using DAL.DBEntities2;
using DAL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
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
		[Authorize]
		public async Task<CarSellRsp> CarSellList([FromUri] PagingParameterModel pagingparametermodel)
        {
            var result = await carSellRepo.GetCarSellList(null);
			// Get's No of Rows Count   
			int count = result.CountryList.Count();

			// Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
			int CurrentPage = pagingparametermodel.PageNumber;

			// Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
			int PageSize = pagingparametermodel.PageSize;

			// Display TotalCount to Records to User  
			int TotalCount = count;

			// Calculating Totalpage by Dividing (No of Records / Pagesize)  
			int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

			// Returns List of Customer after applying Paging   
			var pagresult = result.CountryList.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

			// if CurrentPage is greater than 1 means it has previousPage  
			var previousPage = CurrentPage > 1 ? "Yes" : "No";

			// if TotalPages is greater than CurrentPage means it has nextPage  
			var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

			// Object which we are going to send in header   
			var paginationMetadata = new
			{
				totalCount = TotalCount,
				pageSize = PageSize,
				currentPage = CurrentPage,
				totalPages = TotalPages,
				previousPage,
				nextPage
			};

			// Setting Header  
			HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));
			// Returing List of Customers Collections  
			result.CountryList = pagresult;
			return result;
        }

        /// <summary>
        /// Add Car Sell
        /// </summary>
        /// <param name="carSell"></param>
        /// <returns></returns>
        [Route("sell/car")]
		[Authorize]
		public async Task<CarSellInsertRsp> AddCarSell(CarSellList carSell)
        {
            var rsp = carSellRepo.InsertCarSell(carSell);
            var rspcarsell = await carSellRepo.GetCarSellList(carSell.CarSellID);
            rsp.CarSell = rspcarsell.CarSellList.FirstOrDefault();
            return rsp;
        }

        /// <summary>
        /// Edit Car Sell 
        /// </summary>
        /// <param name="carSell">Images: Delete Images will be sent with Status ID 2, and for new Images Base64 Image object is required</param>
        /// <returns></returns>
        [Route("sell/car/edit")]
        [HttpPut]
		[Authorize]
		public async Task<CarSellInsertRsp> EditCarSell(CarSellList carSell)
        {
            var rsp = carSellRepo.InsertCarSell(carSell);
            var rspcarsell = await carSellRepo.GetCarSellList(carSell.CarSellID);
            rsp.CarSell = rspcarsell.CarSellList.FirstOrDefault();
            return rsp;
        }

        /// <summary>
        /// Add Car to Favourite
        /// </summary>
        /// <param name="obj">CarSellID for the Identity || Status Id (1 Add and 2 for Remove)</param>
        /// <returns></returns>
        [Route("favourite/car")]
		[Authorize]
		public Rsp FavouriteCar(CarFavouriteList obj)
        {
            return carSellRepo.InsertCarFavourite(obj);
        }

        /// <summary>
        /// Ads Car Sell
        /// </summary>
        /// <param name="carSell"></param>
        /// <returns></returns>
        [Route("sell/car/ads")]
		[Authorize]
		public async Task<CarSellRsp> AdsCarSell(CarSellAds carSell)
        {
            return await carSellRepo.GetCarSellAdsList(carSell);
        }
    }
}
