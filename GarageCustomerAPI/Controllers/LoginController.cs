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
    /// <summary>
    /// Login
    /// </summary>
    [RoutePrefix("api")]
    public class LoginController : ApiController
    {
        /// <summary>
        /// Login
        /// </summary>
        loginRepository loginRepo;
        settingRepository settingRepo;
        /// <summary>
        /// Login
        /// </summary>
        public LoginController()
        {
            loginRepo = new loginRepository(new Garage_Entities());
            settingRepo = new settingRepository(new GarageCustomer_Entities());
        }

        /// <summary>
        /// Old API
        /// Login user API
        /// </summary>
        /// <param name="Phone">Mandatory</param>
        /// <returns></returns>
        [Route("login/{Phone}")]
        [HttpGet]
        public LoginResponse Login(string Phone)
        {
            return loginRepo.CustomerLogin(Phone);
        }

        /// <summary>
        /// Login user API
        /// </summary>
        /// <remarks>
        /// - Login info
        /// - List if Notifications
        /// - My Ads
        /// - Favourite list
        /// - Recent Orders for Design V2
        /// - List Of cars (will obsolete soon)</remarks>
        /// <param name="Phone">Mandatory</param>
        /// <returns>
        ///</returns>
        [Route("login/v2/{Phone}")]
        [HttpGet]
        public LoginResponse Loginv2(string Phone)
        {
            return loginRepo.CustomerLoginV2(Phone);
        }

        /// <summary>
        /// Customer Profile Update
        /// </summary>
        /// <param name="obj">Mandatory</param>
        /// <returns></returns>
        [Route("customer/update")]
        public CustomerUpdateRsp PostUpdateCustomer(Customers obj)
        {
            return loginRepo.CustomerUpdate(obj);
        }

        /// <summary>
        /// Add Device Toke || Push Notification
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login/insert/token")]
        public Rsp PostInsertToken(TokenBLL obj)
        {
            return settingRepo.InsertToken(obj);
        }

        /// <summary>
        /// Update Device token
        /// </summary>
        /// <param name="obj">Mandatory</param>
        /// <returns></returns>
        [HttpPost]
        [Route("login/update/token")]
        public Rsp PostUpdateToken(TokenBLL obj)
        {
            return settingRepo.UpdateToken(obj);
        }

        /// <summary>
        /// List Of App Notifications
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("notification/update")]
        public Rsp PostUpdateToken(NotificationBLL obj)
        {
            return settingRepo.UpdateNotification(obj);
        }


        /// <summary>
        /// Get Notifications API
        /// </summary>
        /// <param name="customerid">Mandatory</param>
        /// <remarks>Get List Of Notifcations</remarks>
        /// <returns></returns>
        [Route("notifications/list/{customerid}")]
        [HttpGet]
        public CustomerNotificationResponse Login(int? customerid = 0)
        {
            return settingRepo.GetCustomerNotifications(customerid);
        }


    }
}
