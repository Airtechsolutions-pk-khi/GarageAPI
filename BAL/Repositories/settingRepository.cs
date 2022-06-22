
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
                var _dtAmenitiesInfo = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[4])).ToObject<List<AmenitiesBLL>>().ToList();
                var _dtReviewsInfo = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[5])).ToObject<List<ReviewsBLL>>().ToList();
                var _dtDiscountInfo = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[6])).ToObject<List<DiscountBLL>>().ToList();
                

                rsp.Location = _dtLocationInfo;
                rsp.Services = _dtServiceInfo;
                rsp.Settings = _dtSettingInfo;

                foreach (var i in _dtLocationInfo)
                {
                    i.BrandImage = i.BrandImage == null ? null : ConfigurationSettings.AppSettings["AdminURL"].ToString() + i.BrandImage;

                    i.Services = _dtServiceInfo.Where(x => x.LocationID == i.LocationID).ToList();
                    i.LocationImages = _dtLocImageInfo.Where(x => x.LocationID == i.LocationID).ToList();
                    i.Amenities = _dtAmenitiesInfo.Where(x => x.LocationID == i.LocationID).ToList();
                    i.Discounts = _dtDiscountInfo.Where(x => x.LocationID == i.LocationID).ToList();
                    i.Reviews = _dtReviewsInfo.Where(x => x.LocationID == i.LocationID).ToList();
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
                SqlParameter[] p = new SqlParameter[1];
                p[0] = new SqlParameter("@Date", DateTime.UtcNow.AddMinutes(180).Date);
                return (new DBHelperPOS().GetDatasetFromSP)("sp_GetLocations_CAPI", p);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
