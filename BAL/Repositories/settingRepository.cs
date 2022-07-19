
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
                var _dtServiceInfoAll = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[7])).ToObject<List<ServiceBLL>>().ToList();

                rsp.Location = _dtLocationInfo;
                rsp.Services = _dtServiceInfoAll;
                rsp.Settings = _dtSettingInfo;

                foreach (var i in _dtLocationInfo)
                {
                    i.BrandImage = "http://apicustomer-uat.garage.sa/assets/images/logo-brand.svg";
                    //i.BrandImage == null ? null : ConfigurationSettings.AppSettings["AdminURL"].ToString() + i.BrandImage;

                    i.Services = _dtServiceInfo.Where(x => x.LocationID == i.LocationID).ToList();
                    i.LocationImages = _dtLocImageInfo.Where(x => x.LocationID == i.LocationID).ToList();
                    i.Amenities = _dtAmenitiesInfo.Where(x => x.LocationID == i.LocationID).ToList();
                    i.Discounts = _dtDiscountInfo.Where(x => x.LocationID == i.LocationID).ToList();
                    foreach (var j in i.Discounts)
                    {
                        j.FromDate = DateParse(j.FromDate);
                        j.ToDate = DateParse(j.ToDate);
                    }
                    i.Reviews = _dtReviewsInfo.Where(x => x.LocationID == i.LocationID).ToList();
                    foreach (var j in i.Reviews)
                    {
                        j.Date = DateParse(j.Date);
                    }
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
        public RspCarMake GetCarMake()
        {
            var rsp = new RspCarMake();
            try
            {
                var ds = GetCarMakeInfo();
                rsp.CarMake = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0])).ToObject<List<CarMakeList>>().ToList();
                var _dtCarModels = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[1])).ToObject<List<CarModelList>>().ToList();

                foreach (var i in rsp.CarMake)
                {
                    i.ImagePath = i.ImagePath == null ? null : ConfigurationSettings.AppSettings["AdminURL"].ToString() + i.ImagePath;
                    i.CarModels = _dtCarModels.Where(x => x.MakeID == i.MakeID).ToList();
                }

                rsp.Status = 1;
                rsp.Description = "Successful";
            }
            catch (Exception ex)
            {
                rsp.CarMake = new List<CarMakeList>();
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
        public DataSet GetCarMakeInfo()
        {
            try
            {
                SqlParameter[] p = new SqlParameter[0];
                return (new DBHelperPOS().GetDatasetFromSP)("sp_GetCarMake_CAPI", p);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
