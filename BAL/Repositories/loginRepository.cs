
using DAL.DBEntities;
using DAL.DBEntities2;
using DAL.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using WebAPICode.Helpers;

namespace BAL.Repositories
{

    public class loginRepository : BaseRepository2
    {
        public Customers customer;
        public loginRepository()
            : base()
        {
            DBContext2 = new Garage_UATEntities();

        }
        public loginRepository(Garage_UATEntities contextDB2)
            : base(contextDB2)
        {
            DBContext2 = contextDB2;
        }

        public LoginResponse CustomerLogin(string Phone)
        {
            var rsp = new LoginResponse();
            try
            {
                Phone = Phone.StartsWith("966") ? "+" + Phone : Phone;
                var ds = GetLoginInfo(Phone);
                var _dsCustomerInfo = ds.Tables[0] == null ? new Customers() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0])).ToObject<List<Customers>>().FirstOrDefault();
                var _dsCarInfo = ds.Tables[1] == null ? new List<Cars>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[1])).ToObject<List<Cars>>();
                var _dsOrders = ds.Tables[2] == null ? new List<OrdersList>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[2])).ToObject<List<OrdersList>>();
                var _dsOrderdetail = ds.Tables[3] == null ? new List<OItemsList>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[3])).ToObject<List<OItemsList>>();
                var _dsOrderdetailPkg = ds.Tables[4] == null ? new List<OPackageDetailList>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[4])).ToObject<List<OPackageDetailList>>();
                var _dsordercheckoutdetail = ds.Tables[5] == null ? new List<CheckoutDetailsOrder>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[5])).ToObject<List<CheckoutDetailsOrder>>();
                var _dsNotifications = ds.Tables[6] == null ? new List<NotificationBLL>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[6])).ToObject<List<NotificationBLL>>();
                var _dtCarSellInfo = ds.Tables[7] == null ? new List<CarSellList>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[7])).ToObject<List<CarSellList>>().ToList();
                var _dtCSImageInfo = ds.Tables[8] == null ? new List<CarSellImageList>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[8])).ToObject<List<CarSellImageList>>().ToList();
                var _dtFeatureInfo = ds.Tables[9] == null ? new List<DAL.Models.CarSellFeatureList>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[9])).ToObject<List<CarSellFeatureList>>().ToList();
                var _dtMyAds = ds.Tables[10] == null ? new List<DAL.Models.CarSellList>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[10])).ToObject<List<CarSellList>>().ToList();
                var _dtCSMyAdsImageInfo = ds.Tables[11] == null ? new List<CarSellImageList>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[11])).ToObject<List<CarSellImageList>>().ToList();

                rsp.Notifications = _dsNotifications;
                rsp.Customer = _dsCustomerInfo;

                rsp.CarList = rsp.CarList ?? new List<Cars>();
                foreach (var i in _dsCarInfo)
                {
                    if (rsp.CarList.Where(x => x.RegistrationNo == i.RegistrationNo).Count() == 0)
                        rsp.CarList.Add(i);
                }
                //rsp.CarList = _dsCarInfo;
                foreach (var j in rsp.Notifications)
                {
                    j.IsRead = j.IsRead ?? true;
                    j.Type = "Orders";
                    j.Date = DateParse(j.Date);
                    j.Image = (j.Image == null || j.Image == "") ? null : ConfigurationSettings.AppSettings["AdminURL"].ToString() + j.Image;
                }
                foreach (var i in rsp.CarList)
                {
                    i.MakerImage = i.MakerImage == null ? null : ConfigurationSettings.AppSettings["CpAdminURL"].ToString() + i.MakerImage;
                    i.ImagePath = i.ImagePath == null ? null : ConfigurationSettings.AppSettings["ApiURL"].ToString() + i.ImagePath;
                    try { i.RegistrationNoP1 = i.RegistrationNo.Split('-')[0]; } catch { i.RegistrationNoP1 = ""; }
                    try { i.RegistrationNoP2 = i.RegistrationNo.Split('-')[1]; } catch { i.RegistrationNoP2 = ""; }
                    try { i.RegistrationNoP3 = TranslateToArabic(i.RegistrationNoP1, 1); } catch { i.RegistrationNoP3 = ""; }
                    try { i.RegistrationNoP4 = TranslateToArabic(i.RegistrationNoP2, 2); } catch { i.RegistrationNoP4 = ""; }
                    i.Orders = _dsOrders.Where(x => x.RegistrationNo == i.RegistrationNo).ToList();
                    foreach (var j in i.Orders)
                    {
                        j.CompanyImage = j.CompanyImage == null ? null : ConfigurationSettings.AppSettings["AdminURL"].ToString() + j.CompanyImage;
                        j.NoPlateImage = ConfigurationSettings.AppSettings["ApiURL"].ToString() + "/assets/images/EmptyNoplate.png";
                        j.OrderPunchDate = DateParse(j.OrderPunchDate);
                        j.CheckoutDate = DateParse(j.CheckoutDate);
                        j.Items = _dsOrderdetail.Where(x => x.OrderID == j.OrderID).ToList();

                        foreach (var k in j.Items)
                        {
                            k.Packages = _dsOrderdetailPkg.Where(x => x.OrderDetailID == k.OrderDetailID).ToList();
                        }
                        //checkoutdetails
                        var checkoutDetails = _dsordercheckoutdetail.Where(x => x.OrderCheckoutID == j.OrderCheckoutID).ToList();
                        if (checkoutDetails != null)
                        {
                            j.CardAmount = checkoutDetails.Where(x => x.PaymentMode == 2).FirstOrDefault() == null ? 0 : checkoutDetails.Where(x => x.PaymentMode == 2).FirstOrDefault().AmountPaid;
                            j.CashAmount = checkoutDetails.Where(x => x.PaymentMode == 1).FirstOrDefault() == null ? 0 : checkoutDetails.Where(x => x.PaymentMode == 1).FirstOrDefault().AmountPaid;
                            j.CardType = checkoutDetails.Where(x => x.PaymentMode == 2).FirstOrDefault() == null ? "" : checkoutDetails.Where(x => x.PaymentMode == 2).FirstOrDefault().CardType;
                        }
                        else
                        {
                            j.CardAmount = 0;
                            j.CashAmount = 0;
                            j.CardType = "";
                        }
                    }
                }
                foreach (var i in _dtCarSellInfo)
                {
                    i.CreatedDate = DateParse(i.CreatedDate.ToString());
                    i.CarSellImages = _dtCSImageInfo.Where(x => x.CarSellID == i.CarSellID).ToList();
                    foreach (var j in i.CarSellImages)
                    {
                        j.StatusID = 1;
                        j.Image = j.Image == null ? null : ConfigurationSettings.AppSettings["ApiURL"].ToString() + j.Image;
                    }
                    i.Image = i.CarSellImages.Count > 0 ? i.CarSellImages[0].Image : null;
                    i.CarSellFeatures = _dtFeatureInfo.Where(x => x.CarSellID == i.CarSellID).ToList();
                    foreach (var j in i.CarSellFeatures)
                    {
                        j.Image = j.Image == null ? null : ConfigurationSettings.AppSettings["CAdminURL"].ToString() + j.Image;
                    }
                }
                foreach (var i in _dtMyAds)
                {
                    i.CreatedDate = DateParse(i.CreatedDate.ToString());
                    i.CarSellImages = _dtCSMyAdsImageInfo.Where(x => x.CarSellID == i.CarSellID).ToList();
                    foreach (var j in i.CarSellImages)
                    {
                        j.StatusID = 1;
                        j.Image = j.Image == null ? null : ConfigurationSettings.AppSettings["ApiURL"].ToString() + j.Image;
                    }
                    i.Image = i.CarSellImages.Count > 0 ? i.CarSellImages[0].Image : null;
                    i.CarSellFeatures = _dtFeatureInfo.Where(x => x.CarSellID == i.CarSellID).ToList();
                    foreach (var j in i.CarSellFeatures)
                    {
                        j.Image = j.Image == null ? null : ConfigurationSettings.AppSettings["CAdminURL"].ToString() + j.Image;
                    }
                }
                rsp.CarFavourites = _dtCarSellInfo;
                rsp.MyAds = _dtMyAds;
                rsp.Status = 1;
                rsp.Description = "Login Successfully";
            }
            catch (Exception ex)
            {
                rsp.Status = 0;
                rsp.Description = "Failed";
            }
            return rsp;
        }
        public LoginResponse CustomerLoginV2(string Phone)
        {
            var rsp = new LoginResponse();
            try
            {
                Phone = Phone.StartsWith("966") ? "+" + Phone : Phone;
                var ds = GetLoginInfov2(Phone);
                var _dsCustomerInfo = ds.Tables[0] == null ? new Customers() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0])).ToObject<List<Customers>>().FirstOrDefault();
                var _dsCarInfo = ds.Tables[1] == null ? new List<Cars>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[1])).ToObject<List<Cars>>();
               var _dsNotifications = ds.Tables[2] == null ? new List<NotificationBLL>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[2])).ToObject<List<NotificationBLL>>();
                var _dtCarSellInfo = ds.Tables[3] == null ? new List<CarSellList>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[3])).ToObject<List<CarSellList>>().ToList();
                var _dtCSImageInfo = ds.Tables[4] == null ? new List<CarSellImageList>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[4])).ToObject<List<CarSellImageList>>().ToList();
                var _dtFeatureInfo = ds.Tables[5] == null ? new List<CarSellFeatureList>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[5])).ToObject<List<CarSellFeatureList>>().ToList();
                var _dsRecentOrders = ds.Tables[6] == null ? new List<OrdersList>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[6])).ToObject<List<OrdersList>>();

                rsp.Notifications = _dsNotifications;
                rsp.Customer = _dsCustomerInfo;
                rsp.Customer.ImagePath = rsp.Customer.ImagePath == null ? null : ConfigurationSettings.AppSettings["ApiURL"].ToString() + rsp.Customer.ImagePath;

                rsp.CarList = rsp.CarList ?? new List<Cars>();
                foreach (var i in _dsCarInfo)
                {
                    if (rsp.CarList.Where(x => x.RegistrationNo == i.RegistrationNo).Count() == 0)
                        rsp.CarList.Add(i);
                }
                //rsp.CarList = _dsCarInfo;
                foreach (var j in rsp.Notifications)
                {
                    j.IsRead = j.IsRead ?? true;
                    j.Type = "Orders";
                    j.Date = DateParse(j.Date);
                    j.Image = (j.Image == null || j.Image == "") ? null : ConfigurationSettings.AppSettings["AdminURL"].ToString() + j.Image;
                }
                foreach (var i in rsp.CarList)
                {
                    i.MakerImage = i.MakerImage == null ? null : ConfigurationSettings.AppSettings["CpAdminURL"].ToString() + i.MakerImage;
                    i.ImagePath = i.ImagePath == null ? null : ConfigurationSettings.AppSettings["ApiURL"].ToString() + i.ImagePath;
                    try { i.RegistrationNoP1 = i.RegistrationNo.Split('-')[0]; } catch { i.RegistrationNoP1 = ""; }
                    try { i.RegistrationNoP2 = i.RegistrationNo.Split('-')[1]; } catch { i.RegistrationNoP2 = ""; }
                    try { i.RegistrationNoP3 = TranslateToArabic(i.RegistrationNoP1, 1); } catch { i.RegistrationNoP3 = ""; }
                    try { i.RegistrationNoP4 = TranslateToArabic(i.RegistrationNoP2, 2); } catch { i.RegistrationNoP4 = ""; }

                }
                foreach (var i in _dtCarSellInfo)
                {
                    i.CreatedDate = DateParse(i.CreatedDate.ToString());
                    i.CarSellImages = _dtCSImageInfo.Where(x => x.CarSellID == i.CarSellID).ToList();
                    foreach (var j in i.CarSellImages)
                    {
                        j.StatusID = 1;
                        j.Image = j.Image == null ? null : ConfigurationSettings.AppSettings["ApiURL"].ToString() + j.Image;
                    }
                    i.Image = i.CarSellImages.Count > 0 ? i.CarSellImages[0].Image : null;
                    i.CarSellFeatures = _dtFeatureInfo.Where(x => x.CarSellID == i.CarSellID).ToList();
                    foreach (var j in i.CarSellFeatures)
                    {
                        j.Image = j.Image == null ? null : ConfigurationSettings.AppSettings["CAdminURL"].ToString() + j.Image;
                    }
                }

                foreach (var j in _dsRecentOrders)
                {
                    j.CompanyImage = j.CompanyImage == null ? null : ConfigurationSettings.AppSettings["AdminURL"].ToString() + j.CompanyImage;

                }
                rsp.RecentOrders = _dsRecentOrders;
                rsp.CarFavourites = _dtCarSellInfo;
                rsp.MyAds = new List<CarSellList>();
                rsp.Status = 1;
                rsp.Description = "Login Successfully";
            }
            catch (Exception ex)
            {
                rsp.Status = 0;
                rsp.Description = "Failed";
            }
            return rsp;
        }

        public DataSet GetLoginInfo(string Mobile)
        {
            try
            {
                SqlParameter[] p = new SqlParameter[1];
                p[0] = new SqlParameter("@Phone", Mobile);
                return (new DBHelperPOS().GetDatasetFromSP)("sp_login_CAPI", p);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetLoginInfov2(string Mobile)
        {
            try
            {
                SqlParameter[] p = new SqlParameter[1];
                p[0] = new SqlParameter("@Phone", Mobile);
                return (new DBHelperPOS().GetDatasetFromSP)("sp_loginv2_CAPI", p);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public CustomerUpdateRsp CustomerUpdate(Customers obj)
        {
            CustomerUpdateRsp rsp = new CustomerUpdateRsp();
            try
            {
                var chkImagePath = false;
                try
                {
                    chkImagePath = IsBase64Encoded(obj.ImagePath
                       .Replace("data:image/png;base64,", "")
                       .Replace("data:image/jpg;base64,", "")
                       .Replace("data:image/jpeg;base64,", ""));

                    if (chkImagePath)
                    {
                        if (obj.ImagePath != null && obj.ImagePath != "")
                        {
                            obj.ImagePath = uploadFiles(obj.ImagePath, "UserProfile");
                        }
                    }
                }
                catch { }
                var dt = UpdateCustomer(obj);

                rsp.Customer = dt == null ? new Customers() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(dt)).ToObject<List<Customers>>().FirstOrDefault();
                rsp.Customer.ImagePath = rsp.Customer.ImagePath == null ? null : ConfigurationSettings.AppSettings["ApiURL"].ToString() + rsp.Customer.ImagePath;

                if (rsp.Customer == null)
                {
                    rsp.Customer = new Customers();
                    rsp.Status = 0;
                    rsp.Description = "Failed To Update Customer";
                }
                else
                {
                    rsp.Status = 1;
                    rsp.Description = "Customer Updated Successfully";
                }
            }
            catch (Exception ex)
            {
                rsp.Customer = new Customers();
                rsp.Status = 0;
                rsp.Description = "Failed To Update Customer";
            }
            return rsp;
        }
        public DataTable UpdateCustomer(Customers obj)
        {
            try
            {

                SqlParameter[] p = new SqlParameter[15];
                p[0] = new SqlParameter("@FullName", obj.FullName);
                p[1] = new SqlParameter("@Password", obj.Password);
                p[2] = new SqlParameter("@Email", obj.Email);
                p[3] = new SqlParameter("@Sex", obj.Sex);
                p[4] = new SqlParameter("@Mobile", obj.Mobile);
                p[5] = new SqlParameter("@LastUpdatedBy", "CustomerAPP");
                p[6] = new SqlParameter("@LastUpdatedDate", DateTime.UtcNow);
                p[7] = new SqlParameter("@Points", 0);
                p[8] = new SqlParameter("@ImagePath", obj.ImagePath.Replace(ConfigurationSettings.AppSettings["ApiURL"].ToString(),""));
                p[9] = new SqlParameter("@StatusID", 1);
                p[10] = new SqlParameter("@UserID", obj.UserID);
                p[11] = new SqlParameter("@CreatedOn", DateTime.UtcNow);
                p[12] = new SqlParameter("@CreatedBy", "CustomerAPP");
                p[13] = new SqlParameter("@CustomerID", obj.CustomerID);
                p[14] = new SqlParameter("@City", obj.City);
                return (new DBHelperPOS().GetTableFromSP)("sp_UpdateCustomer_CAPI", p);

            }
            catch (Exception ex)
            {
                return null;
            }
        }

       
    }
}
