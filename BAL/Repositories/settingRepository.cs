
using DAL.DBEntities;
using DAL.DBEntities2;
using DAL.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Helpers;
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
        public SettingRsp GetSettings(int LocationID)
        {

            var rsp = new SettingRsp();
            try
            {
                var ds = GetInfo(LocationID);
                var _dtLocationInfo = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0])).ToObject<List<Locations>>().ToList();
                var _dtServiceInfo = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[1])).ToObject<List<ServiceBLL>>().ToList();
                var _dtLocImageInfo = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[2])).ToObject<List<LocationImages>>().ToList();
                var _dtSettingInfo = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[3])).ToObject<List<SettingBLL>>().ToList();
                var _dtAmenitiesInfo = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[4])).ToObject<List<AmenitiesBLL>>().ToList();
                var _dtReviewsInfo = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[5])).ToObject<List<ReviewsBLL>>().ToList();
                var _dtDiscountInfo = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[6])).ToObject<List<DiscountBLL>>().ToList();
                var _dtServiceInfoAll = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[7])).ToObject<List<ServiceBLL>>().ToList();
                var _dtAminitiesInfoAll = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[8])).ToObject<List<AmenitiesBLL>>().ToList();
                var _dtLandmarks = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[9])).ToObject<List<LandmarkBLL>>().ToList();
                var _dtSettingLocation = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[10])).ToObject<List<LocationJunc>>().ToList();
                var _dtReviewCustomer = ds.Tables[11] == null ? new List<ReportReviewsBLL>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[11])).ToObject<List<ReportReviewsBLL>>().ToList();

                rsp.Location = _dtLocationInfo;
                rsp.Services = _dtServiceInfoAll;
                rsp.Settings = _dtSettingInfo;
                rsp.Amenities = _dtAminitiesInfoAll;
                rsp.Landmarks = _dtLandmarks;

                foreach (var j in rsp.Settings)
                {
                    j.Locations = _dtSettingLocation.Where(x => x.SettingID == j.ID).ToList();
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

                    if (opening > closing)
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

                    var rating1 = i.Reviews.Where(x => x.RateVal > 4 && x.RateVal <= 5).Count();
                    var rating2 = i.Reviews.Where(x => x.RateVal > 3 && x.RateVal <= 4).Count();
                    var rating3 = i.Reviews.Where(x => x.RateVal > 2 && x.RateVal <= 3).Count();
                    var rating4 = i.Reviews.Where(x => x.RateVal > 1 && x.RateVal <= 2).Count();
                    var rating5 = i.Reviews.Where(x => x.RateVal >= 0 && x.RateVal <= 1).Count();
                    i.ReviewCountDetails = new int[5] { rating1, rating2, rating3, rating4, rating5 };
                    foreach (var j in i.Reviews)
                    {
                        j.Customers = _dtReviewCustomer.Where(x => x.ReviewID == j.ReviewID).ToList();
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
        public SettingRsp GetSeachLocations(string Search)
        {

            var rsp = new SettingRsp();
            try
            {
                var ds = SeachLocations_ADO(Search);
                var _dtLocationInfo = ds.Tables[0] == null ? new List<Locations>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0])).ToObject<List<Locations>>().ToList();
                rsp.Location = _dtLocationInfo;
                rsp.Settings = new List<SettingBLL>();
                rsp.Amenities = new List<AmenitiesBLL>();
                rsp.Landmarks = new List<LandmarkBLL>();
                rsp.Services = new List<ServiceBLL>();  
                foreach (var i in rsp.Location)
                {
                    var opening = TimespanToDecimal(TimeSpan.Parse(i.OpenTime ?? "00:00:00"));
                    var closing = TimespanToDecimal(TimeSpan.Parse(i.CloseTime ?? "23:59:00"));
                    i.OpenTime = DateParse(DateTime.UtcNow.AddMinutes(180).ToString("MM/dd/yyyy") + ' ' + i.OpenTime ?? "00:00:00");
                    i.CloseTime = DateParse(DateTime.UtcNow.AddDays(opening > closing ? 1 : 0).AddMinutes(180).ToString("MM/dd/yyyy") + ' ' + i.CloseTime ?? "23:59:00");
                    i.BrandImage = i.BrandImage == null ? null : ConfigurationSettings.AppSettings["AdminURL"].ToString() + i.BrandImage;

                }

                rsp.Status = 1;
                rsp.Description = "Success";
            }
            catch (Exception ex)
            {
                rsp.Status = 0;
                rsp.Description = "Failed";
            }
            return rsp;

        }
        public SettingRsp GetServiceLocations(int ServiceID, int LocationID, int? UserID)
        {

            var rsp = new SettingRsp();
            try
            {
                var ds = UserID == 0 ? GetLocationADOV1(LocationID, ServiceID) : GetLocationADO(LocationID, ServiceID, UserID);
                var _dtLocationInfo = ds.Tables[0] == null ? new List<Locations>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0])).ToObject<List<Locations>>().ToList();
                var _dtServiceInfo = ds.Tables[6] == null ? new List<ServiceBLL>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[6])).ToObject<List<ServiceBLL>>().ToList();
                var _dtLocImageInfo = ds.Tables[1] == null ? new List<LocationImages>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[1])).ToObject<List<LocationImages>>().ToList();
                var _dtAmenitiesInfo = ds.Tables[2] == null ? new List<AmenitiesBLL>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[2])).ToObject<List<AmenitiesBLL>>().ToList();
                var _dtReviewsInfo = ds.Tables[3] == null ? new List<ReviewsBLL>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[3])).ToObject<List<ReviewsBLL>>().ToList();
                var _dtDiscountInfo = ds.Tables[4] == null ? new List<DiscountBLL>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[4])).ToObject<List<DiscountBLL>>().ToList();
                var _dtReviewCustomer = ds.Tables[5] == null ? new List<ReportReviewsBLL>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[5])).ToObject<List<ReportReviewsBLL>>().ToList();
                var _dtWorkingHours = UserID == 0 ? null : ds.Tables[7] == null ? new List<WorkingHour>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[7])).ToObject<List<WorkingHour>>().ToList();

                rsp.Location = _dtLocationInfo;
                rsp.Services = new List<ServiceBLL>();// _dtServiceInfoAll;
                rsp.Settings = new List<SettingBLL>();// _dtSettingInfo;
                rsp.Amenities = new List<AmenitiesBLL>();// _dtAminitiesInfoAll;
                rsp.Landmarks = new List<LandmarkBLL>();// _dtLandmarks;

                foreach (var i in rsp.Location)
                {
                    if (i.OpenTime is null)
                    {

                    }
                    var opening = TimespanToDecimal(TimeSpan.Parse(i.OpenTime ?? "00:00:00"));
                    var closing = TimespanToDecimal(TimeSpan.Parse(i.CloseTime ?? "23:59:00"));
                    i.OpenTime = DateParse(DateTime.UtcNow.AddMinutes(180).ToString("MM/dd/yyyy") + ' ' + i.OpenTime ?? "00:00:00");
                    i.CloseTime = DateParse(DateTime.UtcNow.AddDays(opening > closing ? 1 : 0).AddMinutes(180).ToString("MM/dd/yyyy") + ' ' + i.CloseTime ?? "23:59:00");
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

                    var rating1 = i.Reviews.Where(x => x.RateVal > 4 && x.RateVal <= 5).Count();
                    var rating2 = i.Reviews.Where(x => x.RateVal > 3 && x.RateVal <= 4).Count();
                    var rating3 = i.Reviews.Where(x => x.RateVal > 2 && x.RateVal <= 3).Count();
                    var rating4 = i.Reviews.Where(x => x.RateVal > 1 && x.RateVal <= 2).Count();
                    var rating5 = i.Reviews.Where(x => x.RateVal >= 0 && x.RateVal <= 1).Count();
                    i.ReviewCountDetails = new int[5] { rating1, rating2, rating3, rating4, rating5 };
                    foreach (var j in i.Reviews)
                    {
                        j.Customers = _dtReviewCustomer.Where(x => x.ReviewID == j.ReviewID).ToList();
                        j.Date = DateParse(j.Date);
                    }
                    i.WorkingHours = _dtWorkingHours == null ? new List<WorkingHour>() : _dtWorkingHours.Where(x => x.LocationID == i.LocationID).ToList();
                }

                rsp.Status = 1;
                rsp.Description = "Success";
            }
            catch (Exception ex)
            {
                rsp.Status = 0;
                rsp.Description = "Failed";
            }
            return rsp;

        }

        //old
        public SettingRsp GetSettingsV2(int LocationID)
        {

            var rsp = new SettingRsp();
            try
            {
                var ds = GetSettingsInfo(LocationID);
                var _dtLocationInfo = ds.Tables[0] == null ? new List<Locations>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0])).ToObject<List<Locations>>().ToList();
                //var _dtServiceInfo = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[1])).ToObject<List<ServiceBLL>>().ToList();
                //var _dtLocImageInfo = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[2])).ToObject<List<LocationImages>>().ToList();
                var _dtSettingInfo = ds.Tables[1] == null ? new List<SettingBLL>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[1])).ToObject<List<SettingBLL>>().ToList();
                //var _dtAmenitiesInfo = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[4])).ToObject<List<AmenitiesBLL>>().ToList();
                //var _dtReviewsInfo = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[5])).ToObject<List<ReviewsBLL>>().ToList();
                //var _dtDiscountInfo = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[6])).ToObject<List<DiscountBLL>>().ToList();
                var _dtServiceInfoAll = ds.Tables[2] == null ? new List<ServiceBLL>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[2])).ToObject<List<ServiceBLL>>().ToList();
                var _dtAminitiesInfoAll = ds.Tables[3] == null ? new List<AmenitiesBLL>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[3])).ToObject<List<AmenitiesBLL>>().ToList();
                var _dtLandmarks = ds.Tables[4] == null ? new List<LandmarkBLL>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[4])).ToObject<List<LandmarkBLL>>().ToList();
                var _dtSettingLocation = ds.Tables[5] == null ? new List<LocationJunc>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[5])).ToObject<List<LocationJunc>>().ToList();
                //var _dtReviewCustomer = ds.Tables[11] == null ? new List<ReportReviewsBLL>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[11])).ToObject<List<ReportReviewsBLL>>().ToList();

                rsp.Location = _dtLocationInfo;
                rsp.Services = _dtServiceInfoAll;
                rsp.Settings = _dtSettingInfo;
                rsp.Amenities = _dtAminitiesInfoAll;
                rsp.Landmarks = _dtLandmarks;

                foreach (var j in rsp.Settings)
                {
                    j.Locations = _dtSettingLocation.Where(x => x.SettingID == j.ID).ToList();
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
                    i.OpenTime = DateParse(DateTime.UtcNow.AddMinutes(180).ToString("MM/dd/yyyy") + ' ' + i.OpenTime);
                    i.CloseTime = DateParse(DateTime.UtcNow.AddDays(opening > closing ? 1 : 0).AddMinutes(180).ToString("MM/dd/yyyy") + ' ' + i.CloseTime);
                    i.BrandImage = i.BrandImage == null ? null : ConfigurationSettings.AppSettings["AdminURL"].ToString() + i.BrandImage;
                }

                rsp.Status = 1;
                rsp.Description = "Success";
            }
            catch (Exception ex)
            {
                rsp.Status = 0;
                rsp.Description = "Failed";
            }
            return rsp;

        }
        public Settingv2Rsp GetAppSettingsV2()
        {
            var rsp = new Settingv2Rsp();
            try
            {
                var ds = GetSettings_ADO();
                rsp.AppstoreVersion = "1.0.10";
                rsp.Settings = ds.Tables[0] == null ? new List<SettingBLL>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0])).ToObject<List<SettingBLL>>().ToList();
                rsp.Services = ds.Tables[1] == null ? new List<ServiceBLL>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[1])).ToObject<List<ServiceBLL>>().ToList();
                rsp.Amenities = ds.Tables[2] == null ? new List<AmenitiesBLL>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[2])).ToObject<List<AmenitiesBLL>>().ToList();
                rsp.Landmarks = ds.Tables[3] == null ? new List<LandmarkBLL>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[3])).ToObject<List<LandmarkBLL>>().ToList();
                rsp.Brands = ds.Tables[4] == null ? new List<UsersList>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[4])).ToObject<List<UsersList>>().ToList();
                rsp.Cities = ds.Tables[5] == null ? new List<CityList>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[5])).ToObject<List<CityList>>().ToList();
                var _dtSettingLocation = ds.Tables[6] == null ? new List<Locations>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[6])).ToObject<List<Locations>>().ToList();

                foreach (var j in rsp.Brands)
                {
                    j.BrandImage = j.BrandImage == null ? null : ConfigurationSettings.AppSettings["AdminURL"].ToString() + j.BrandImage;
                }
                foreach (var j in rsp.Settings)
                {
                    j.SettingLocations = _dtSettingLocation.Where(x => x.SettingID == j.ID).ToList();
                    j.Image = j.Image == null ? null : ConfigurationSettings.AppSettings["CAdminURL"].ToString() + j.Image;
                    foreach (var k in j.SettingLocations)
                    {
                        var opening = TimespanToDecimal(TimeSpan.Parse(k.OpenTime));
                        var closing = TimespanToDecimal(TimeSpan.Parse(k.CloseTime));
                        k.OpenTime = DateParse(DateTime.UtcNow.AddMinutes(180).ToString("MM/dd/yyyy") + ' ' + k.OpenTime);
                        k.CloseTime = DateParse(DateTime.UtcNow.AddDays(opening > closing ? 1 : 0).AddMinutes(180).ToString("MM/dd/yyyy") + ' ' + k.CloseTime);
                        k.BrandImage = k.BrandImage == null ? null : ConfigurationSettings.AppSettings["AdminURL"].ToString() + k.BrandImage;

                        k.LocationImages = new List<LocationImages>();
                        k.Services = new List<ServiceBLL>();
                        k.Amenities = new List<AmenitiesBLL>();
                        k.Discounts = new List<DiscountBLL>();
                        k.Reviews = new List<ReviewsBLL>();
                        foreach (var l in k.LocationImages)
                        { l.ImageURL = l.ImageURL == null ? null : ConfigurationSettings.AppSettings["CAdminURL"].ToString() + l.ImageURL; }

                    }
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
                rsp.Status = 1;
                rsp.Description = "Success";
            }
            catch (Exception ex)
            {
                rsp.Status = 0;
                rsp.Description = "Failed";
            }
            return rsp;
        }
        public Settingv2Rsp GetAppSettings()
        {
            var rsp = new Settingv2Rsp();
            try
            {
                var ds = GetSettings_ADOV1();
                rsp.AppstoreVersion = "1.0.10";
                rsp.Settings = ds.Tables[0] == null ? new List<SettingBLL>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0])).ToObject<List<SettingBLL>>().ToList();
                rsp.Services = ds.Tables[1] == null ? new List<ServiceBLL>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[1])).ToObject<List<ServiceBLL>>().ToList();
                rsp.Amenities = ds.Tables[2] == null ? new List<AmenitiesBLL>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[2])).ToObject<List<AmenitiesBLL>>().ToList();
                rsp.Landmarks = ds.Tables[3] == null ? new List<LandmarkBLL>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[3])).ToObject<List<LandmarkBLL>>().ToList();
                var _dtSettingLocation = ds.Tables[4] == null ? new List<Locations>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[4])).ToObject<List<Locations>>().ToList();
                var _dtSettingLocationImgs = ds.Tables[5] == null ? new List<LocationImages>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[5])).ToObject<List<LocationImages>>().ToList();

                foreach (var j in rsp.Settings)
                {
                    j.SettingLocations = _dtSettingLocation.Where(x => x.SettingID == j.ID).ToList();
                    foreach (var k in j.SettingLocations)
                    {
                        var opening = TimespanToDecimal(TimeSpan.Parse(k.OpenTime));
                        var closing = TimespanToDecimal(TimeSpan.Parse(k.CloseTime));
                        k.OpenTime = DateParse(DateTime.UtcNow.AddMinutes(180).ToString("MM/dd/yyyy") + ' ' + k.OpenTime);
                        k.CloseTime = DateParse(DateTime.UtcNow.AddDays(opening > closing ? 1 : 0).AddMinutes(180).ToString("MM/dd/yyyy") + ' ' + k.CloseTime);
                        k.BrandImage = k.BrandImage == null ? null : ConfigurationSettings.AppSettings["AdminURL"].ToString() + k.BrandImage;

                        k.LocationImages = _dtSettingLocationImgs.Where(x => x.LocationID == k.LocationID).ToList();
                        k.Services = new List<ServiceBLL>();
                        k.Amenities = new List<AmenitiesBLL>();
                        k.Discounts = new List<DiscountBLL>();
                        k.Reviews = new List<ReviewsBLL>();
                        foreach (var l in k.LocationImages)
                        { l.ImageURL = l.ImageURL == null ? null : ConfigurationSettings.AppSettings["CAdminURL"].ToString() + l.ImageURL; }
                    }
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
                rsp.Status = 1;
                rsp.Description = "Success";
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
        public DataSet GetInfo(int LocationID)
        {
            try
            {
                SqlParameter[] p = new SqlParameter[2];
                p[0] = new SqlParameter("@LocationID", LocationID == 0 ? null : LocationID.ToString());
                p[1] = new SqlParameter("@Date", DateTime.UtcNow.AddMinutes(180).Date);
                return (new DBHelperPOS().GetDatasetFromSP)("sp_GetLocationsV2_CAPI", p);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet SeachLocations_ADO(string Search)
        {
            try
            {
                SqlParameter[] p = new SqlParameter[1];
                p[0] = new SqlParameter("@Search", Search);
                return (new DBHelperPOS().GetDatasetFromSP)("sp_SearchLocation_CAPI", p);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet GetLocationADOV1(int LocationID, int? ServiceID)
        {
            try
            {
                SqlParameter[] p = new SqlParameter[3];
                p[0] = new SqlParameter("@LocationID", LocationID == 0 ? null : LocationID.ToString());
                p[1] = new SqlParameter("@Date", DateTime.UtcNow.AddMinutes(180).Date);
                p[2] = new SqlParameter("@ServiceID", ServiceID == 0 ? null : ServiceID.ToString());
                return (new DBHelperPOS().GetDatasetFromSP)("sp_GetLocation_CAPI", p);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetLocationADO(int LocationID, int? ServiceID, int? UserID)
        {
            try
            {
                SqlParameter[] p = new SqlParameter[4];
                p[0] = new SqlParameter("@LocationID", LocationID == 0 ? null : LocationID.ToString());
                p[1] = new SqlParameter("@Date", DateTime.UtcNow.AddMinutes(180).Date);
                p[2] = new SqlParameter("@ServiceID", ServiceID == 0 ? null : ServiceID.ToString());
                p[3] = new SqlParameter("@UserID", UserID == 0 ? null : UserID.ToString());
                return (new DBHelperPOS().GetDatasetFromSP)("sp_GetLocationV2_CAPI", p);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetSettingsInfo(int LocationID)
        {
            try
            {
                SqlParameter[] p = new SqlParameter[2];
                p[0] = new SqlParameter("@LocationID", LocationID == 0 ? null : LocationID.ToString());
                p[1] = new SqlParameter("@Date", DateTime.UtcNow.AddMinutes(180).Date);
                return (new DBHelperPOS().GetDatasetFromSP)("sp_GetLocationsV3_CAPI", p);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetSettings_ADOV1()
        {
            try
            {
                SqlParameter[] p = new SqlParameter[0];
                return (new DBHelperPOS().GetDatasetFromSP)("sp_GetSettings_CAPI", p);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetSettings_ADO()
        {
            try
            {
                SqlParameter[] p = new SqlParameter[0];
                return (new DBHelper().GetDatasetFromSP)("sp_GetSettings_CAPI", p);
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
                var chk = DBContext.PushTokens.Where(x => x.Token == obj.Token && x.StatusID == 1).Count();
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

                var chk = DBContext.PushTokens.Where(x => x.Token == obj.Token && x.StatusID == 1).FirstOrDefault();

                if (chk != null)
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
                var chk = DBContext.Notifications.Where(x => x.NotificationID == obj.NotificationID).FirstOrDefault();

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
        public Rsp AddFeedback(Feedback obj)
        {
            Rsp rsp;
            try
            {
                //var chk = DBContext.Feedbacks.Where(x => x.FeedbackID == obj.FeedbackID).FirstOrDefault();

                DBContext.Feedbacks.AddOrUpdate(obj);
                DBContext.SaveChanges();

                rsp = new Rsp();
                rsp.Status = (int)eStatus.Success;
                rsp.Description = "Feedback Added";
            }
            catch (Exception ex)
            {
                rsp = new Rsp();
                rsp.Status = (int)eStatus.Exception;
                rsp.Description = "Failed to add Feedback";
            }
            return rsp;
        }
        public Rsp AddReportReview(ReportReviewsBLL obj)
        {
            Rsp rsp = new Rsp();
            try
            {
                var dt = AddReportReviewADO(obj);

                if (dt == null)
                {
                    rsp.Status = 0;
                    rsp.Description = "Failed To Report Review";
                }
                else
                {
                    rsp.Status = 1;
                    rsp.Description = "Review Reported Successfully";
                }
            }
            catch (Exception ex)
            {
                rsp.Status = 0;
                rsp.Description = "Failed To Report Review";
            }
            return rsp;
        }

        public AIChatModelRsp GetListAIBOT(int carID, int customerID, int chatID)
        {

            var rsp = new AIChatModelRsp();
            try
            {
                string content = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/Template") + "\\" + "aibot.txt");
                JObject jsonResponse = JObject.Parse(content);
                rsp = JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(jsonResponse)).ToObject<AIChatModelRsp>();
                rsp.chats =
                    carID > 0 ? rsp.chats.Where(x => x.carID == carID).ToList() :
                    customerID > 0 ? rsp.chats.Where(x => x.customerID == customerID).ToList() :
                    chatID > 0 ? rsp.chats.Where(x => x.chatID == chatID).ToList() : rsp.chats.ToList();
                rsp.status = 1;
                rsp.description = "Successful";
            }
            catch (Exception ex)
            {
                rsp.status = 0;
                rsp.description = "Failed";
            }
            return rsp;

        }
        public Rsp AddAIChat(AIChat obj)
        {
            Rsp rsp;
            try
            {
                //var chk = DBContext.Feedbacks.Where(x => x.FeedbackID == obj.FeedbackID).FirstOrDefault();
                //string baseJson = "[{\"id\":\"123\",\"name\":\"carl\"}]";

                string content = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/Template") + "\\" + "aibot.txt");
                JObject jsonResponse = JObject.Parse(content);
                var jsonfile = JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(jsonResponse)).ToObject<AIChatModelRsp>();
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(jsonfile.chats);
                jsonfile.chats.Add(obj);
                //var baseJson = JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(json)).ToObject<AIChat>();
                //List<AIChat> personsToAdd = new List<AIChat>() { obj };

                //string updatedJson = AddObjectsToJson(json, personsToAdd);

                // jsonfile.chats = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0])).ToObject<List<AIChat>>().ToList() JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(updatedJson)).ToObject<List<AIChat>>().ToList();
                string jsonwrite = Newtonsoft.Json.JsonConvert.SerializeObject(jsonfile);

                File.WriteAllText(System.Web.HttpContext.Current.Server.MapPath("~/Template") + "\\" + "aibot.txt", jsonwrite);
                rsp = new Rsp();
                rsp.Status = (int)eStatus.Success;
                rsp.Description = "Chat Added";
            }
            catch (Exception ex)
            {
                rsp = new Rsp();
                rsp.Status = (int)eStatus.Exception;
                rsp.Description = "Failed";
            }
            return rsp;
        }
        public string AddObjectsToJson<T>(string json, List<T> objects)
        {
            List<T> list = JsonConvert.DeserializeObject<List<T>>(json);
            list.AddRange(objects);
            return JsonConvert.SerializeObject(list);
        }
        public DataTable AddReportReviewADO(ReportReviewsBLL obj)
        {
            try
            {
                SqlParameter[] p = new SqlParameter[9];
                p[0] = new SqlParameter("@ReviewID", obj.ReviewID);
                p[1] = new SqlParameter("@CustomerID", obj.CustomerID);
                p[2] = new SqlParameter("@Reason", obj.Reason);
                p[3] = new SqlParameter("@LikeStatus", obj.LikeStatus);
                p[4] = new SqlParameter("@StatusID", 1);
                p[5] = new SqlParameter("@Date", DateTime.UtcNow.AddMinutes(180));
                p[6] = new SqlParameter("@ReportReveiwID", obj.ReportReveiwID);
                p[7] = new SqlParameter("@LikeValue", obj.LikeValue);
                p[8] = new SqlParameter("@DisLikeValue", obj.DislikeValue);

                return (new DBHelper().GetDatasetFromSP)("sp_InsertReportReview", p).Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable AddReviewADO(ReviewsBLL obj)
        {
            try
            {
                SqlParameter[] p = new SqlParameter[8];
                p[0] = new SqlParameter("@Name", obj.Name);
                p[1] = new SqlParameter("@Message", obj.Message);
                p[2] = new SqlParameter("@Rate", obj.Rate);
                p[3] = new SqlParameter("@StatusID", 1);
                p[4] = new SqlParameter("@LastUpdatedDate", DateTime.UtcNow.AddMinutes(180));
                p[5] = new SqlParameter("@LocationID", obj.LocationID);
                p[6] = new SqlParameter("@Date", DateTime.UtcNow.AddMinutes(180));
                p[7] = new SqlParameter("@CustomerID", obj.CustomerID);

                return (new DBHelper().GetDatasetFromSP)("sp_InsertReview_CAPI", p).Tables[0];
                // DateTime.ParseExact(obj.Date, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture)
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable UpdateReviewADO(ReviewsBLL obj)
        {
            try
            {
                SqlParameter[] p = new SqlParameter[11];
                p[0] = new SqlParameter("@Name", obj.Name);
                p[1] = new SqlParameter("@Message", obj.Message);
                p[2] = new SqlParameter("@Rate", obj.Rate);
                p[3] = new SqlParameter("@StatusID", obj.StatusID);
                p[4] = new SqlParameter("@LastUpdatedDate", DateTime.UtcNow.AddMinutes(180));
                p[5] = new SqlParameter("@LocationID", obj.LocationID);
                p[6] = new SqlParameter("@Date", DateTime.UtcNow.AddMinutes(180));
                p[7] = new SqlParameter("@LikeCount", obj.LikeCount);
                p[8] = new SqlParameter("@DislikeCount", obj.DislikeCount);
                p[9] = new SqlParameter("@ReportAbuse", obj.ReportAbuse);
                p[10] = new SqlParameter("@ReviewID", obj.ReviewID);
                return (new DBHelper().GetDatasetFromSP)("sp_UpdateReview_CAPI", p).Tables[0];
                // DateTime.ParseExact(obj.Date, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture)
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ReviewRsp AddReview(ReviewsBLL obj)
        {
            ReviewRsp rsp = new ReviewRsp();
            try
            {
                var dt = AddReviewADO(obj);

                rsp.Reviews = dt == null ? new List<ReviewsBLL>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(dt)).ToObject<List<ReviewsBLL>>(); ;
                if (dt == null)
                {
                    rsp.Reviews = new List<ReviewsBLL>();
                    rsp.Status = 0;
                    rsp.Description = "Failed To Add Review";
                }
                else
                {
                    foreach (var item in rsp.Reviews)
                    {
                        item.Date = DateParse(item.Date);
                    }
                    rsp.Status = 1;
                    rsp.Description = "Review Added Successfully";
                }
            }
            catch (Exception ex)
            {
                rsp.Reviews = new List<ReviewsBLL>();
                rsp.Status = 0;
                rsp.Description = "Failed To Add Review";
            }
            return rsp;
        }
        public ReviewRsp UpdateReview(ReviewsBLL obj)
        {
            ReviewRsp rsp = new ReviewRsp();
            try
            {
                var dt = UpdateReviewADO(obj);
                rsp.Reviews = dt == null ? new List<ReviewsBLL>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(dt)).ToObject<List<ReviewsBLL>>(); ;
                if (dt == null)
                {
                    rsp.Reviews = new List<ReviewsBLL>();
                    rsp.Status = 0;
                    rsp.Description = "Failed To Add Review";
                }
                else
                {
                    foreach (var item in rsp.Reviews)
                    {
                        item.Date = DateParse(item.Date);
                    }
                    rsp.Status = 1;
                    rsp.Description = "Review Updated Successfully";
                }
            }
            catch (Exception ex)
            {
                rsp.Reviews = new List<ReviewsBLL>();
                rsp.Status = 0;
                rsp.Description = "Failed To Update Review";
            }
            return rsp;
        }

        public OffersRsp GetOffers()
        {
            var rsp = new OffersRsp();
            try
            {
                var ds = GetOffers_ADO();
                rsp.Offers = ds.Tables[0] == null ? new List<DiscountBLL>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0])).ToObject<List<DiscountBLL>>().ToList();
                foreach (var j in rsp.Offers)
                {
                    j.FromDate = DateParse(j.FromDate);
                    j.ToDate = DateParse(j.ToDate);
                    j.Image = j.Image == null ? null : ConfigurationSettings.AppSettings["CAdminURL"].ToString() + j.Image;
                }

                rsp.Status = 1;
                rsp.Description = "Success";
            }
            catch (Exception ex)
            {
                rsp.Status = 0;
                rsp.Description = "Failed";
            }
            return rsp;
        }
        public DataSet GetOffers_ADO()
        {
            try
            {
                SqlParameter[] p = new SqlParameter[2];
                p[0] = new SqlParameter("@LocationID", null);
                p[1] = new SqlParameter("@Date", DateTime.UtcNow.AddMinutes(180).Date);
                return (new DBHelperPOS().GetDatasetFromSP)("sp_GetOffers_CAPI", p);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public CustomerNotificationResponse GetCustomerNotifications(int? CustomerID)
        {
            var rsp = new CustomerNotificationResponse();
            try
            {
                var ds = GetCustomerNotificationsADO(CustomerID);
                rsp.Notifications = ds.Tables[0] == null ? new List<NotificationBLL>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0])).ToObject<List<NotificationBLL>>();

                foreach (var i in rsp.Notifications)
                {
                    i.Image = i.Image == null ? null : ConfigurationSettings.AppSettings["ApiURL"].ToString() + i.Image;

                }
                rsp.Status = 1;
                rsp.Description = "Success";
            }
            catch (Exception ex)
            {
                rsp.Status = 0;
                rsp.Description = "Failed";
            }
            return rsp;
        }
        public DataSet GetCustomerNotificationsADO(int? CustomerID)
        {
            try
            {
                SqlParameter[] p = new SqlParameter[1];
                p[0] = new SqlParameter("@CustomerID", CustomerID);
                return (new DBHelper().GetDatasetFromSP)("sp_CustomerNotifications_CAPI", p);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
