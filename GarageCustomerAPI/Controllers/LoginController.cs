using BAL.Repositories;
using DAL.DBEntities;
using DAL.DBEntities2;
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
    public class LoginController : ApiController
    {           
        loginRepository loginRepo;      
        public LoginController()
        {
            loginRepo = new loginRepository(new Garage_UATEntities2());
        }

        [Route("login/{Phone}")]
        [HttpGet]
        public LoginResponse Login(string Phone)
        {
            return loginRepo.CustomerLogin(Phone);
        }

        [Route("customer/update")]
        public CustomerUpdateRsp PostUpdateCustomer(Customers obj)
        {
            return loginRepo.CustomerUpdate(obj);
        }

    }
}
