﻿
using DAL.DBEntities2;
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
            DBContext = new GarageCustomer_Entities();

        }
        public settingRepository(GarageCustomer_Entities contextDB)
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
                var _dtAminitiesInfoAll = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[8])).ToObject<List<AmenitiesBLL>>().ToList();
                var _dtLandmarks = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[9])).ToObject<List<LandmarkBLL>>().ToList();

                rsp.Location = _dtLocationInfo;
                rsp.Services = _dtServiceInfoAll;
                rsp.Settings = _dtSettingInfo;
                rsp.Amenities = _dtAminitiesInfoAll;
                rsp.Landmarks = _dtLandmarks;

                foreach (var j in rsp.Settings)
                {
                    j.Image = j.Image == null ? null : ConfigurationSettings.AppSettings["CAdminURL"].ToString() + j.Image;
                }
                foreach (var j in rsp.Landmarks)
                {
                    j.Image = j.Image == null ? null : ConfigurationSettings.AppSettings["CAdminURL"].ToString() + j.Image;
                }
                foreach (var j in rsp.Amenities)
                {
                    j.Image = j.Image == null ? null : ConfigurationSettings.AppSettings["CAdminURL"].ToString() + j.Image;
                }
                foreach (var j in rsp.Services)
                {
                    j.Image = j.Image == null ? null : ConfigurationSettings.AppSettings["CAdminURL"].ToString() + j.Image;
                }
                foreach (var i in _dtLocationInfo)
                {
                    var opening = TimespanToDecimal(TimeSpan.Parse(i.OpenTime));
                    var closing = TimespanToDecimal(TimeSpan.Parse(i.CloseTime));

                    if (opening>closing)
                    {
                        i.OpenTime = DateParse(DateTime.UtcNow.AddMinutes(180).ToString("MM/dd/yyyy") + ' ' + i.OpenTime);
                        i.CloseTime = DateParse(DateTime.UtcNow.AddDays(1).AddMinutes(180).ToString("MM/dd/yyyy") + ' ' + i.CloseTime);
                    }
                    else
                    {
                        i.OpenTime = DateParse(DateTime.UtcNow.AddMinutes(180).ToString("MM/dd/yyyy") + ' ' + i.OpenTime);
                        i.CloseTime = DateParse(DateTime.UtcNow.AddMinutes(180).ToString("MM/dd/yyyy") + ' ' + i.CloseTime);
                    }
                    
                    i.BrandImage = i.BrandImage == null ? null : ConfigurationSettings.AppSettings["AdminURL"].ToString() + i.BrandImage;
                    i.Services = _dtServiceInfo.Where(x => x.LocationID == i.LocationID).ToList();
                    i.LocationImages = _dtLocImageInfo.Where(x => x.LocationID == i.LocationID).ToList();
                    i.Amenities = _dtAmenitiesInfo.Where(x => x.LocationID == i.LocationID).ToList();
                    i.Discounts = _dtDiscountInfo.Where(x => x.LocationID == i.LocationID).ToList();
                    foreach (var j in i.LocationImages)
                    {
                        j.ImageURL = j.ImageURL == null ? null : ConfigurationSettings.AppSettings["CAdminURL"].ToString() + j.ImageURL;
                    }
                    foreach (var j in i.Amenities)
                    {
                        j.Image = j.Image == null ? null : ConfigurationSettings.AppSettings["CAdminURL"].ToString() + j.Image;
                    }
                    foreach (var j in i.Services)
                    {
                        j.Image = j.Image == null ? null : ConfigurationSettings.AppSettings["CAdminURL"].ToString() + j.Image;
                    }

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
                    i.ImagePath = i.ImagePath == null ? null : ConfigurationSettings.AppSettings["CpAdminURL"].ToString() + i.ImagePath;
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

        public Rsp InsertToken(TokenBLL obj)
        {
            Rsp rsp;
            try
            {
                PushToken token = JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(obj)).ToObject<PushToken>();
                token.StatusID = 1;
                var chk = DBContext.PushTokens.Where(x => x.Token == obj.Token && x.StatusID==1).Count();
                if (chk == 0)
                {
                    PushToken data = DBContext.PushTokens.Add(token);
                    DBContext.SaveChanges();
                }

                rsp = new Rsp();
                rsp.Status = (int)eStatus.Success;
                rsp.Description = "Token Added";
            }
            catch (Exception ex)
            {
                rsp = new Rsp();
                rsp.Status = (int)eStatus.Exception;
                rsp.Description = "Failed to add token";
            }
            return rsp;
        }
        public Rsp UpdateToken(TokenBLL obj)
        {
            Rsp rsp;
            try
            {
                //PushToken token = JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(obj)).ToObject<PushToken>();

                var chk = DBContext.PushTokens.Where(x => x.Token == obj.Token && x.StatusID==1).FirstOrDefault();
              
                if (chk!=null)
                {
                    chk.StatusID = obj.StatusID;
                    DBContext.PushTokens.Attach(chk);
                    DBContext.UpdateOnly<PushToken>(
                    chk, x => x.StatusID);

                    DBContext.SaveChanges();
                }


                rsp = new Rsp();
                rsp.Status = (int)eStatus.Success;
                rsp.Description = "Token Updated";
            }
            catch (Exception ex)
            {
                rsp = new Rsp();
                rsp.Status = (int)eStatus.Exception;
                rsp.Description = "Failed to update token";
            }
            return rsp;
        }
        public Rsp UpdateNotification(NotificationBLL obj)
        {
            Rsp rsp;
            try
            {
                var chk = DBContext.Notifications.Where(x => x.NotificationID== obj.NotificationID).FirstOrDefault();

                if (chk != null)
                {
                    chk.IsRead = obj.IsRead;
                    DBContext.Notifications.Attach(chk);
                    DBContext.UpdateOnly<Notification>(
                    chk, x => x.IsRead);

                    DBContext.SaveChanges();
                }
                rsp = new Rsp();
                rsp.Status = (int)eStatus.Success;
                rsp.Description = "Notification Updated";
            }
            catch (Exception ex)
            {
                rsp = new Rsp();
                rsp.Status = (int)eStatus.Exception;
                rsp.Description = "Failed to update notification";
            }
            return rsp;
        }
    }
}
