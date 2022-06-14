
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
        //public LoginResponse Login(string Phone)
        //{
        //    List<Cars> carLst = new List<Cars>();
        //    var rsp = new LoginResponse();

        //    if (Phone.StartsWith("966"))
        //    {
        //        Phone = "+" + Phone;
        //    }
        //    try
        //    {
        //        var list = DBContext2.usp_Login(Phone).FirstOrDefault();

        //        if (list == null)
        //        {
        //            //rsp.customer = new Customers();
        //            rsp.Status = 2;
        //            rsp.Description = "Username or Password is not correct.";
        //            return rsp;
        //        }
        //        else
        //        {
        //            customer = new Customers
        //            {
        //                FullName = list.FullName,
        //                UserName = list.UserName,
        //                CustomerID = list.CustomerID,
        //                Email = list.Email,
        //                ImagePath = list.ImagePath,
        //                IsEmail = list.IsEmail,
        //                IsSMS = list.IsSMS,
        //                LocationID = list.LocationID == null ? 0 : int.Parse(list.LocationID.ToString()),
        //                Mobile = list.Mobile,
        //                Password = list.Password,
        //                Points = list.Points == null ? 0 : float.Parse(list.Points.ToString()),
        //                Sex = list.Sex,
        //                UserID = list.UserID == null ? 0 : int.Parse(list.UserID.ToString())

        //            };
        //            var carList = DBContext2.sp_GetCarsBy_CustomerID(customer.CustomerID).ToList();
        //            if (carList == null)
        //            {
        //                rsp.Status = 2;
        //                rsp.Description = "Cars Not Found.";
        //                return rsp;
        //            }
        //            foreach (var item in carList)
        //            {
        //                carLst.Add(new Cars
        //                {
        //                    CarID = item.CarID,
        //                    CustomerID = item.CustomerID,
        //                    VinNo = item.VinNo,
        //                    MakeID = item.MakeID,
        //                    //MakerName = item.MakeName,
        //                    ModelID = item.ModelID,
        //                    //ModelName = item.ModelName,
        //                    CarName = item.Name,
        //                    CarDescription = item.Description,
        //                    Year = item.Year,
        //                    Color = item.Color,
        //                    RegistrationNo = item.RegistrationNo,
        //                    ImagePath = item.ImagePath,
        //                    //MakeImage = item.MakeImage,
        //                    LocationID = item.LocationID,
        //                    RecommendedAmount = item.RecommendedAmount,
        //                    //CustomerContact = item.Mobile,
        //                    CheckLitre = item.CheckLitre.ToString(),
        //                    EngineType = item.EngineType,
        //                    //CustomerName = item.FullName,
        //                });
        //            }

        //            rsp.CarList = carLst;
        //            rsp.customer = customer;
        //            rsp.Status = 1;
        //            rsp.Description = "Login Successfully";

        //            return rsp;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        rsp.customer = new Customers();
        //        rsp.Status = 0;
        //        rsp.Description = "Failed";
        //        return rsp;
        //    }
        //}
        public LoginResponse CustomerLogin(string Phone)
        {
            var rsp = new LoginResponse();
            try
            {
                Phone = Phone.StartsWith("966") ? "+" + Phone : Phone;
                var ds = GetLoginInfo(Phone);
                var _dsCustomerInfo = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0])).ToObject<List<Customers>>().FirstOrDefault();
                var _dsCarInfo = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[1])).ToObject<List<Cars>>();
                var _dsOrders = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[2])).ToObject<List<OrdersList>>();
                var _dsOrderdetail = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[3])).ToObject<List<OItemsList>>();

                rsp.Customer = _dsCustomerInfo;
                rsp.CarList = _dsCarInfo;
                foreach (var i in rsp.CarList)
                {
                    try {i.RegistrationNoP1 = i.RegistrationNo.Split('-')[0]; } catch { i.RegistrationNoP1 = ""; }
                    try {i.RegistrationNoP2 = i.RegistrationNo.Split('-')[1]; } catch { i.RegistrationNoP2 = ""; }
                    try {i.RegistrationNoP3 = TranslateToArabic(i.RegistrationNoP1, 1); } catch { i.RegistrationNoP3 = ""; }
                    try {i.RegistrationNoP4 = TranslateToArabic(i.RegistrationNoP2, 2); } catch { i.RegistrationNoP4 = ""; }
                    i.Orders = _dsOrders.Where(x => x.CarID == i.CarID).ToList();
                    foreach (var j in i.Orders)
                    {
                        j.Items = _dsOrderdetail.Where(x => x.OrderID == j.OrderID).ToList();
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
