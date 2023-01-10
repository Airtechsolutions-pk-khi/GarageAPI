using BAL.Repositories;
using DAL.DBEntities;
using DAL.DBEntities2;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace GarageCustomerAPI.Controllers
{
    [RoutePrefix("api")]
    public class LoginController : ApiController
    {
        loginRepository loginRepo;
        settingRepository settingRepo;
        public LoginController()
        {
            loginRepo = new loginRepository(new Garage_Entities());
            settingRepo = new settingRepository(new GarageCustomer_Entities());
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

        [HttpPost]
        [Route("login/insert/token")]
        public Rsp PostInsertToken(TokenBLL obj)
        {
            return settingRepo.InsertToken(obj);
        }

        [HttpPost]
        [Route("login/update/token")]
        public Rsp PostUpdateToken(TokenBLL obj)
        {
            return settingRepo.UpdateToken(obj);
        }

        [HttpPost]
        [Route("notification/update")]
        public Rsp PostUpdateToken(NotificationBLL obj)
        {
            return settingRepo.UpdateNotification(obj);
        }
    }
}
