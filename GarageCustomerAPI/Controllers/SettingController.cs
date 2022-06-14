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
    public class SettingController : ApiController
    {        
        settingRepository settingRepo;      
        public SettingController()
        {            
            settingRepo = new settingRepository(new GarageCustomer_UATEntities());
        }
        
        [Route("setting/all")]
        public RspSetting GetAll()
        {
            return settingRepo.GetSettings();
        }

    }
}
