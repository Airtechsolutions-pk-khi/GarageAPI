
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
            DBContext2 = new Garage_UATEntities2();

        }
        public loginRepository(Garage_UATEntities2 contextDB2)
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
                var _dsCustomerInfo = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0])).ToObject<List<Customers>>().FirstOrDefault();
                var _dsCarInfo = ds.Tables[1] == null ? new List<Cars>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[1])).ToObject<List<Cars>>();
                var _dsOrders = ds.Tables[2] == null ? new List<OrdersList>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[2])).ToObject<List<OrdersList>>();
                var _dsOrderdetail = ds.Tables[3] == null ? new List<OItemsList>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[3])).ToObject<List<OItemsList>>();
                var _dsOrderdetailPkg = ds.Tables[4] == null ? new List<OPackageDetailList>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[4])).ToObject<List<OPackageDetailList>>();
                var _dsordercheckoutdetail = ds.Tables[5] == null ? new List<CheckoutDetailsOrder>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[5])).ToObject<List<CheckoutDetailsOrder>>();
                
                
                rsp.Customer = _dsCustomerInfo;
                rsp.CarList = _dsCarInfo;
                foreach (var i in rsp.CarList)
                {
                    i.ImagePath = i.ImagePath == null ? null : ConfigurationSettings.AppSettings["ApiURL"].ToString() + i.ImagePath;
                    try { i.RegistrationNoP1 = i.RegistrationNo.Split('-')[0]; } catch { i.RegistrationNoP1 = ""; }
                    try { i.RegistrationNoP2 = i.RegistrationNo.Split('-')[1]; } catch { i.RegistrationNoP2 = ""; }
                    try { i.RegistrationNoP3 = TranslateToArabic(i.RegistrationNoP1, 1); } catch { i.RegistrationNoP3 = ""; }
                    try { i.RegistrationNoP4 = TranslateToArabic(i.RegistrationNoP2, 2); } catch { i.RegistrationNoP4 = ""; }
                    i.Orders = _dsOrders.Where(x => x.CarID == i.CarID).ToList();
                    foreach (var j in i.Orders)
                    {
                        j.NoPlateImage = "http://apicustomer-uat.garage.sa/assets/images/EmptyNoplate.png";
                        j.OrderPunchDate = DateParse(j.OrderPunchDate);
                        j.CheckoutDate = DateParse(j.CheckoutDate);
                        j.Items = _dsOrderdetail.Where(x => x.OrderID == j.OrderID).ToList();

                        foreach (var k in j.Items)
                        {
                           k.Packages= _dsOrderdetailPkg.Where(x => x.OrderDetailID == k.OrderDetailID).ToList();
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
    }
}
