using BAL.Repositories;
using DAL.DBEntities;
using DAL.DBEntities2;
using DAL.Models;
using System.Web.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System;

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
        /// Login user API
        /// </summary>
        /// <param name="Phone">Mandatory</param>
        /// <returns></returns>
        [Route("login/{Phone}")]
        [HttpGet]
        public LoginResponse Login(string Phone)
        {
            var result = loginRepo.CustomerLogin(Phone);
            var token = GenerateJwtToken(result.Customer.CustomerID.ToString(), result.Customer.Mobile?? "", result.Customer.Email?? "");
            result.Token = token;
            return result;
        }

        /// <summary>
        /// Customer Profile Update
        /// </summary>
        /// <param name="obj">Mandatory</param>
        /// <returns></returns>
        [Route("customer/update")]
        [Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
		public Rsp PostUpdateToken(NotificationBLL obj)
        {
            return settingRepo.UpdateNotification(obj);
        }

		internal string GenerateJwtToken(string userId, string mobileNo, string Email)
		{
			var issuer = WebConfigurationManager.AppSettings["Issuer"];
			var audience = WebConfigurationManager.AppSettings["Audience"];
			var secretKey = WebConfigurationManager.AppSettings["SecretKey"];

			var claims = new List<Claim>
		{
			new Claim(ClaimTypes.NameIdentifier, userId),
			new Claim(ClaimTypes.MobilePhone, mobileNo),
			new Claim(ClaimTypes.Email, Email)
		};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var token = new JwtSecurityToken(
				issuer: issuer,
				audience: audience,
				claims: claims,
				expires: DateTime.Now.AddHours(10), // Token expiration time
				signingCredentials: credentials
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
