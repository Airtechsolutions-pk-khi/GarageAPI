using DAL.DBEntities2;
using DAL.GlobalAndCommon;
using DAL.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebAPICode.Helpers;

namespace BAL.Repositories
{

    public class settingRepository : BaseRepository
    {
		private readonly PaginationRepository _pageRepo;
		public settingRepository()
            : base()
        {
            DBContext = new GarageCustomer_Entities();
			_pageRepo = new PaginationRepository(
                new DBHelper(), 
                new DBHelperPOS(AppGlobal.connectionStringUAT));

		}
        public settingRepository(GarageCustomer_Entities contextDB, PaginationRepository pageRepo)
            : base(contextDB)
        {
            DBContext = contextDB;
            _pageRepo = pageRepo;

		}
        public async Task<SettingRsp> GetSettings(int pageNumber, int pageSize, int? LocationID)
        {

            var rsp = new SettingRsp();
            try
            {
                //var ds = GetInfo(LocationID);
                LocationID = LocationID == 0 ? null : LocationID;
				var ds = await _pageRepo.GetPaginationDataPos<dynamic>(pageNumber, pageSize, "sp_GetLocationsV3_CAPI", new { LocationID });
				var _dtLocationInfo = JArray.Parse(JsonConvert.SerializeObject(ds.Tables[0])).ToObject<List<Locations>>().ToList();
                var _dtServiceInfo = JArray.Parse(JsonConvert.SerializeObject(ds.Tables[1])).ToObject<List<ServiceBLL>>().ToList();
                var _dtLocImageInfo = JArray.Parse(JsonConvert.SerializeObject(ds.Tables[2])).ToObject<List<LocationImages>>().ToList();
                var _dtSettingInfo = JArray.Parse(JsonConvert.SerializeObject(ds.Tables[3])).ToObject<List<SettingBLL>>().ToList();
                var _dtAmenitiesInfo = JArray.Parse(JsonConvert.SerializeObject(ds.Tables[4])).ToObject<List<AmenitiesBLL>>().ToList();
                var _dtReviewsInfo = JArray.Parse(JsonConvert.SerializeObject(ds.Tables[5])).ToObject<List<ReviewsBLL>>().ToList();
                var _dtDiscountInfo = JArray.Parse(JsonConvert.SerializeObject(ds.Tables[6])).ToObject<List<DiscountBLL>>().ToList();
                var _dtServiceInfoAll = JArray.Parse(JsonConvert.SerializeObject(ds.Tables[7])).ToObject<List<ServiceBLL>>().ToList();
                var _dtAminitiesInfoAll = JArray.Parse(JsonConvert.SerializeObject(ds.Tables[8])).ToObject<List<AmenitiesBLL>>().ToList();
                var _dtLandmarks = JArray.Parse(JsonConvert.SerializeObject(ds.Tables[9])).ToObject<List<LandmarkBLL>>().ToList();
                var _dtSettingLocation = JArray.Parse(JsonConvert.SerializeObject(ds.Tables[10])).ToObject<List<LocationJunc>>().ToList();
                var _dtReviewCustomer = ds.Tables[11] == null?new List<ReportReviewsBLL>(): JArray.Parse(JsonConvert.SerializeObject(ds.Tables[11])).ToObject<List<ReportReviewsBLL>>().ToList();
				var _nextPage = JArray.Parse(JsonConvert.SerializeObject(ds.Tables[12]));

				rsp.Location = _dtLocationInfo;
                rsp.Services = _dtServiceInfoAll;
                rsp.Settings = _dtSettingInfo;
                rsp.Amenities = _dtAminitiesInfoAll;
                rsp.Landmarks = _dtLandmarks;
                rsp.PageInfo = _nextPage;

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
                        j.Customers =_dtReviewCustomer.Where(x => x.ReviewID == j.ReviewID).ToList();
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
        public async Task<SettingRsp> GetSettings(int LocationID)
        {

            var rsp = new SettingRsp();
            try
            {
				var ds = GetInfo(LocationID);
				//var ds = await _pageRepo.GetPaginationDataPos<dynamic>(pageNumber, pageSize, "sp_GetLocationsV3_CAPI", new { LocationID });
				var _dtLocationInfo = JArray.Parse(JsonConvert.SerializeObject(ds.Tables[0])).ToObject<List<Locations>>().ToList();
                var _dtServiceInfo = JArray.Parse(JsonConvert.SerializeObject(ds.Tables[1])).ToObject<List<ServiceBLL>>().ToList();
                var _dtLocImageInfo = JArray.Parse(JsonConvert.SerializeObject(ds.Tables[2])).ToObject<List<LocationImages>>().ToList();
                var _dtSettingInfo = JArray.Parse(JsonConvert.SerializeObject(ds.Tables[3])).ToObject<List<SettingBLL>>().ToList();
                var _dtAmenitiesInfo = JArray.Parse(JsonConvert.SerializeObject(ds.Tables[4])).ToObject<List<AmenitiesBLL>>().ToList();
                var _dtReviewsInfo = JArray.Parse(JsonConvert.SerializeObject(ds.Tables[5])).ToObject<List<ReviewsBLL>>().ToList();
                var _dtDiscountInfo = JArray.Parse(JsonConvert.SerializeObject(ds.Tables[6])).ToObject<List<DiscountBLL>>().ToList();
                var _dtServiceInfoAll = JArray.Parse(JsonConvert.SerializeObject(ds.Tables[7])).ToObject<List<ServiceBLL>>().ToList();
                var _dtAminitiesInfoAll = JArray.Parse(JsonConvert.SerializeObject(ds.Tables[8])).ToObject<List<AmenitiesBLL>>().ToList();
                var _dtLandmarks = JArray.Parse(JsonConvert.SerializeObject(ds.Tables[9])).ToObject<List<LandmarkBLL>>().ToList();
                var _dtSettingLocation = JArray.Parse(JsonConvert.SerializeObject(ds.Tables[10])).ToObject<List<LocationJunc>>().ToList();
                var _dtReviewCustomer = ds.Tables[11] == null?new List<ReportReviewsBLL>(): JArray.Parse(JsonConvert.SerializeObject(ds.Tables[11])).ToObject<List<ReportReviewsBLL>>().ToList();
				//var _nextPage = JArray.Parse(JsonConvert.SerializeObject(ds.Tables[12]));

				rsp.Location = _dtLocationInfo;
                rsp.Services = _dtServiceInfoAll;
                rsp.Settings = _dtSettingInfo;
                rsp.Amenities = _dtAminitiesInfoAll;
                rsp.Landmarks = _dtLandmarks;
                //rsp.PageInfo = _nextPage;

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
                        j.Customers =_dtReviewCustomer.Where(x => x.ReviewID == j.ReviewID).ToList();
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
                rsp.CarMake = JArray.Parse(JsonConvert.SerializeObject(ds.Tables[0])).ToObject<List<CarMakeList>>().ToList();
                var _dtCarModels = JArray.Parse(JsonConvert.SerializeObject(ds.Tables[1])).ToObject<List<CarModelList>>().ToList();

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
                return (new DBHelperPOS(AppGlobal.connectionStringUAT).GetDatasetFromSP)("sp_GetLocationsV2_CAPI", p);
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
                return (new DBHelperPOS(AppGlobal.connectionStringUAT).GetDatasetFromSP)("sp_GetCarMake_CAPI", p);
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
                PushToken token = JObject.Parse(JsonConvert.SerializeObject(obj)).ToObject<PushToken>();
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

                rsp.Reviews = dt == null ? new List<ReviewsBLL>() : JArray.Parse(JsonConvert.SerializeObject(dt)).ToObject<List<ReviewsBLL>>(); ;
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
                rsp.Reviews = dt == null ? new List<ReviewsBLL>() : JArray.Parse(JsonConvert.SerializeObject(dt)).ToObject<List<ReviewsBLL>>(); ;
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
    }
}
