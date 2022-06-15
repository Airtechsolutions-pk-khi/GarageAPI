
using DAL.DBEntities;
using DAL.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using WebAPICode.Helpers;

namespace BAL.Repositories
{

    public class settingRepository : BaseRepository
    {
        public settingRepository()
            : base()
        {
            DBContext = new GarageCustomer_UATEntities();

        }
        public settingRepository(GarageCustomer_UATEntities contextDB)
            : base(contextDB)
        {
            DBContext = contextDB;
        }
        public SettingRsp GetSettings()
        {
            
            var rsp = new SettingRsp();
            try
            {
                var ds = GetInfo();
                var _dtLocationInfo = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0])).ToObject<List<Locations>>().ToList();
                var _dtServiceInfo = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[1])).ToObject<List<ServiceBLL>>().ToList();
                var _dtLocImageInfo = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[2])).ToObject<List<LocationImage>>().ToList();
                var _dtSettingInfo = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[3])).ToObject<List<SettingBLL>>().ToList();

                rsp.Location = _dtLocationInfo;
                rsp.Services = _dtServiceInfo;
                rsp.Settings = _dtSettingInfo;

                foreach (var i in _dtLocationInfo)
                {
                    i.Services = _dtServiceInfo.Where(x => x.LocationID == i.LocationID).ToList();
                    i.LocationImages = _dtLocImageInfo.Where(x => x.LocationID == i.LocationID).ToList();
                }

                rsp.Status = 1;
                rsp.Description = "Successful";
            }
            catch (Exception ex)
            {
                rsp.Status = 0;
                rsp.Description = "Failed";
            }
            return rsp;

        }
        public DataSet GetInfo()
        {
            try
            {
                SqlParameter[] p = new SqlParameter[0];                
                return (new DBHelperPOS().GetDatasetFromSP)("sp_GetLocationServices", p);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
