﻿using DAL.DBEntities;
using DAL.DBEntities2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPICode.Helpers;

namespace DAL.Models
{
    class ViewModel
    {
    }

    public class Rsp
    {
        public string Description { get; set; }
        public int Status { get; set; }

    }
    public class RspBrandList : Rsp
    {
        public List<BrandsBLL> brands { get; set; }
    }
    public class ReviewRsp : Rsp
    {
        public List<ReviewsBLL> Reviews { get; set; }
    }
    public class CustomerUpdateRsp : Rsp
    {
        public Customers Customer { get; set; }
    }
    public class CarRsp : Rsp
    {
        public Cars cars { get; set; }
    }

    public class RspSetting : Rsp
    {
        public List<LocationsBLL> Settings { get; set; }
        public List<ServiceBLL> Services { get; set; }
    }
    public class AmenitiesBLL
    {
        public int? AmenitiesID { get; set; }
        public string Name { get; set; }
        public string ArabicName { get; set; }
        public string Image { get; set; }
        public int? LocationID { get; set; }
    }
    public class LandmarkBLL
    {
        public int? LandmarkID { get; set; }
        public string Name { get; set; }
        public string ArabicName { get; set; }
        public string Image { get; set; }

    }
    public class ReviewsUpdateBLL
    { }
    public class ReportReviewsBLL
    {
        public int? ReportReveiwID { get; set; }
        public int? ReviewID { get; set; }
        public int? CustomerID { get; set; }
        public int? StatusID { get; set; }
        public string Reason { get; set; }
        public int LikeStatus { get; set; }
        public DateTime? Date { get; set; }
        public int? LikeValue { get; set; } = 0;
        public int? DislikeValue { get; set; } = 0;
    }
    public class ReviewsBLL
    {
        public int? ReviewID { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public string Rate { get; set; }

        public double RateVal { get; set; }
        public string Date { get; set; }
        public string Image { get; set; }
        public int? LocationID { get; set; }
        public int? StatusID { get; set; }
        public int? LikeCount { get; set; }
        public int? DislikeCount { get; set; }
        public string ReportAbuse { get; set; }
        public int? CustomerID { get; set; }
        public List<ReportReviewsBLL> Customers { get; set; }
    }
    public class DiscountBLL
    {
        public string Name { get; set; }
        public string BrandImage { get; set; }
        public string Image { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int? LocationID { get; set; }
        public int? DiscountID { get; set; }
    }
    public class ServiceBLL
    {
        public int ServiceID { get; set; }
        public string ServiceTitle { get; set; }
        public string ArabicServiceTitle { get; set; }
        public string ServiceDescription { get; set; }
        public string ArabicServiceDescription { get; set; }
        public string Image { get; set; }
        public int? DisplayOrder { get; set; }
        public int? LocationID { get; set; }
        public bool? IsServices { get; set; }
    }


    public class RspAppSetting : Rsp
    {
        public AppInfoBLL AppInfo { get; set; }
        public List<AboutBLL> Timings { get; set; }
    }
    public class RspDeliveryAreaList : Rsp
    {
        public List<DeliveryAreaBLL> DeliveryArea { get; set; }
    }
    public class RspAdminLogin : Rsp
    {
        public int LocationID { get; set; }
        public string Name { get; set; }
        public string ContactNo { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }

    }
    public class Invoice
    {
        public string InvoiceNumber { get; set; }
        public int POSID { get; set; }
        public string USIN { get; set; }
        public DateTime DateTime { get; set; }
        public string BuyerName { get; set; }
        public string BuyerNTN { get; set; }
        public string BuyerCNIC { get; set; }
        public string BuyerPhoneNumber { get; set; }
        public double TotalSaleValue { get; set; }
        public double TotalTaxCharged { get; set; }
        public double TotalQuantity { get; set; }
        public double Discount { get; set; }
        public double FurtherTax { get; set; }
        public double TotalBillAmount { get; set; }
        public int PaymentMode { get; set; }

        public int InvoiceType { get; set; }
        public List<InvoiceItems> Items { get; set; }
    }
    public class InvoiceItems
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string PCTCode { get; set; }
        public double Quantity { get; set; }
        public float TaxRate { get; set; }
        public double SaleValue { get; set; }
        public double Discount { get; set; }
        public double FurtherTax { get; set; }
        public double TaxCharged { get; set; }
        public double TotalAmount { get; set; }
        public int InvoiceType { get; set; }
        public string RefUSIN { get; set; }

    }

    public class RspBanners
    {
        public int BannerID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<int> BrandID { get; set; }

    }
    public class RspOffers
    {
        public int OfferID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public Nullable<int> BrandID { get; set; }
        public Nullable<int> LocationID { get; set; }
        public string Image { get; set; }
        public string BrandLogo { get; set; }
        public string BrandName { get; set; }
        public string ItemName { get; set; }
        public string Calories { get; set; }
        public Nullable<int> ItemID { get; set; }
        public Nullable<int> StatusID { get; set; }

        public List<LocationsBLL> Locations { get; set; }

    }
    public class RspMenu : Rsp
    {
        public IEnumerable<CategoryBLL> categories { get; set; }
    }

    public class RspOrdersCustomer : Rsp
    {
        public IEnumerable<OrdersBLL> Orders { get; set; }
    }

    public class RspOrdersAdmin : Rsp
    {
        public IEnumerable<OrdersBLL> Orders { get; set; }
    }
    public class RspCustomerLogin : Rsp
    {
        public Customers customer { get; set; }
    }

    public class RspOrderPunch : Rsp
    {
        public int? OrderNo { get; set; }
        public int OrderID { get; set; }
    }
    public class RspCustomerSignup : Rsp
    {
        public int CustomerID { get; set; }
    }
    public class RspCustomerAddress : Rsp
    {
        public int AddressID { get; set; }
    }
    public class RspCustomerPayment : Rsp
    {
        public int PaymentID { get; set; }
    }

    public class DeliveryAreaBLL
    {
        public int? BrandID { get; set; }
        public int? DeliveryAreaID { get; set; }
        public string Name { get; set; }
        public double? Price { get; set; }
    }
    public class AboutBLL
    {

        public string BranchName { get; set; }
        public string BranchAddress { get; set; }
        public string BranchTiming { get; set; }
        public int? Discount { get; set; }
        public string DeliveryNo { get; set; }
        public string DiscountDecription { get; set; }
        public string WhatsappNo { get; set; }

    }
    public class AppInfoBLL
    {
        public string AppDescription { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
    }
    public class BrandsBLL
    {
        public int BrandID { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string CompanyURl { get; set; }
        public string Address { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string Currency { get; set; }
        public double? Tax { get; set; }
        public double? DeliveryCharges { get; set; }
        public Nullable<long> BusinessKey { get; set; }
        public string LastUpdateBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public double? DiscountApplied { get; set; }
        public List<LocationsBLL> Locations { get; set; }
        public List<DeliveryAreaBLL> DeliveryAreas { get; set; }
        //public List<AboutBLL> AppSettings { get; set; }
        //public List<AppInfoBLL> ApplicationInfo { get; set; }
    }

    public class LocationsBLL
    {
        public Nullable<double> Discounts { get; set; }
        public Nullable<double> Tax { get; set; }
        public int LocationID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public int? LicenseID { get; set; }
        public Nullable<bool> DeliveryServices { get; set; }
        public Nullable<double> DeliveryCharges { get; set; }
        public string DeliveryTime { get; set; }
        public Nullable<double> MinOrderAmount { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string ImageURL { get; set; }
        public Nullable<int> BrandID { get; set; }
        public int MyProperty { get; set; }
        public List<ServiceBLL> ServiceList { get; set; }
    }
    public class Locations
    {
        public int? SettingID { get; set; } = 0;
        public int LocationID { get; set; }
        public string BrandName { get; set; }
        public string BrandImage { get; set; }
        public string Name { get; set; }
        public string ArabicName { get; set; }
        public string Description { get; set; }
        public string ArabicDescription { get; set; }
        public string Address { get; set; }
        public string ArabicAddress { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string OpenTime { get; set; }
        public string CloseTime { get; set; }
        public string Rating { get; set; }
        public string Website { get; set; }
        public int? LandmarkID { get; set; }
        public string GMapLink { get; set; }
        public bool? IsFeatured { get; set; }
        //public float? ReviewAvgRating { get; set; }
        public int ReviewCount { get; set; }
        public int[] ReviewCountDetails { get; set; }
        public List<ServiceBLL> Services { get; set; }
        public List<LocationImages> LocationImages { get; set; }
        public List<AmenitiesBLL> Amenities { get; set; }
        public List<ReviewsBLL> Reviews { get; set; }
        public List<DiscountBLL> Discounts { get; set; }

        public List<WorkingHour> WorkingHours { get; set; }
    }
    public class  WorkingHour   {
        public string Name { get; set; }
        public string Time{ get; set; }
        public int? LocationID{ get; set; }
    }
    public class Services
    {
        public int ServiceID { get; set; }
        public string ServiceTitle { get; set; }
        public string ServiceDescription { get; set; }
        public string Image { get; set; }
        public Nullable<int> DisplayOrder { get; set; }
        public Nullable<int> LocationID { get; set; }
        public Nullable<int> StatusId { get; set; }
        public string Type { get; set; }
    }
    public class LocationImages
    {
        public int ImageID { get; set; }
        public string ImageURL { get; set; }
        public int? LocationID { get; set; }
    }
    public class CategoryBLL
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public string ArabicName { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int DisplayOrder { get; set; }
        public string LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<int> LocationID { get; set; }
        public Nullable<int> BrandID { get; set; }

        public List<ItemBLL> items { get; set; }
    }
    public class ItemBLL
    {
        public int ItemID { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public Nullable<int> UnitID { get; set; }
        public string Name { get; set; }
        public string ArabicName { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Barcode { get; set; }
        public string SKU { get; set; }
        public Nullable<int> DisplayOrder { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<double> Cost { get; set; }
        public string ItemType { get; set; }
        public string LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<double> Calories { get; set; }
        public Nullable<bool> IsApplyDiscount { get; set; }
        public Nullable<bool> IsSoldOut { get; set; }
        public List<ModifierBLL> modifiers { get; set; }
        public List<AddonsBLL> addons { get; set; }
    }
    public class ModifierBLL
    {
        public int ItemID { get; set; }
        public int ModifierID { get; set; }
        public string Name { get; set; }
        public string ArabicName { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public Nullable<double> Price { get; set; }
        public string LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<int> BrandID { get; set; }
    }
    public class AddonsBLL
    {
        public int ItemID { get; set; }
        public int AddonID { get; set; }
        public string Name { get; set; }
        public string ArabicName { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public string LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public int StatusID { get; set; }
        public Nullable<int> BrandID { get; set; }

    }
    public class CustomerBLLdelete
    {
        public float DiscountApplied { get; set; }
        public int CustomerID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Image { get; set; }
        public string Password { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string LastUpdatedBy { get; set; }
        public string Sex { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<int> LocationID { get; set; }
        public Nullable<int> BrandID { get; set; }

        public CustomerDetailBLL details { get; set; }

        public List<CustomerAddressBLL> address { get; set; }
        public List<CustomerPaymentBLL> payment { get; set; }
    }

    public class CustomerDetailBLL
    {
        public int CustomerDetailID { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string ZipCode { get; set; }
        public string LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
    }
    public class CustomerAddressBLL
    {

        public string placeID { get; set; }
        public int addressID { get; set; }
        public string houseNumber { get; set; }
        public string landmark { get; set; }

        public string locationAddress { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
        public string placeType { get; set; }
        public int customerID { get; set; }
        public Nullable<int> brandID { get; set; }
        public Nullable<int> statusID { get; set; }
    }
    public class CustomerPaymentBLL
    {
        public int PaymentID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CardExpire { get; set; }
        public string CVV { get; set; }
        public string CardTitle { get; set; }
        public Nullable<int> StatusID { get; set; }
        public int CustomerID { get; set; }
        public Nullable<int> BrandID { get; set; }
    }

    public class TokenBLL
    {
        public int Device { get; set; }
        public int TokenID { get; set; }
        public int? CustomerID { get; set; }
        public string Token { get; set; }
        //public Nullable<int> LocationID { get; set; }
        public Nullable<int> StatusID { get; set; }

    }
    public class TransferOrderBLL
    {
        public int FromLocationID { get; set; }
        public int ToLocationID { get; set; }
        public int OrderID { get; set; }

    }
    public class OrdersBLL
    {
        public int OrderID { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public Nullable<int> TransactionNo { get; set; }
        public Nullable<int> OrderNo { get; set; }
        public string OrderType { get; set; }
        public string OrderDate { get; set; }
        public string OrderDeliveryDate { get; set; }
        public Nullable<System.DateTime> OrderPreparedDate { get; set; }
        public Nullable<System.DateTime> OrderOFDDate { get; set; }
        public Nullable<System.DateTime> OrderDoneDate { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string SessionID { get; set; }
        public Nullable<int> OrderTakerID { get; set; }
        public Nullable<int> DeliverUserID { get; set; }
        public string LastUpdateBy { get; set; }
        public Nullable<System.DateTime> LastUpdateDT { get; set; }
        public Nullable<int> LocationID { get; set; }
        public Nullable<int> BrandID { get; set; }
        public string BrandName { get; set; }
        public string BrandLogo { get; set; }
        public string Remarks { get; set; }
        public OrderCheckoutBLL OrderCheckouts { get; set; }
        public OrderCustomerBLL CustomerOrders { get; set; }
        public List<OrderDetailBLL> OrderDetails { get; set; }
        public OrderStatusBLL OrderStatus { get; set; }
    }
    public class OrderStatusBLL
    {
        public OrderStatusChildBLL OrderConfirmed { get; set; }
        public OrderStatusChildBLL FoodPrepared { get; set; }
        public OrderStatusChildBLL DeliveryinProgress { get; set; }
    }
    public class OrderStatusChildBLL
    {
        public string Value { get; set; }
        public bool Status { get; set; }
        public string Description { get; set; }
        public string Time { get; set; }
    }
    public class OrderDetailBLL
    {
        public int OrderDetailID { get; set; }
        public Nullable<int> OrderID { get; set; }
        public Nullable<int> ItemID { get; set; }
        public string ItemName { get; set; }
        public Nullable<double> Quantity { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<double> Cost { get; set; }
        public string OrderMode { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string LastUpdateBy { get; set; }
        public Nullable<System.DateTime> LastUpdateDT { get; set; }

        public List<OrderModifiersBLL> OrderDetailModifiers { get; set; }

        public List<OrderAddonsBLL> OrderDetailAddons { get; set; }
    }
    public class OrderModifiersBLL
    {
        public int OrderDetailModifierID { get; set; }
        public Nullable<int> OrderDetailID { get; set; }
        public Nullable<int> ModifierID { get; set; }
        public string ModifierName { get; set; }
        public Nullable<double> Quantity { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<double> Cost { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string LastUpdateBy { get; set; }
        public Nullable<System.DateTime> LastUpdateDT { get; set; }
    }
    public class OrderAddonsBLL
    {
        public int OrderDetailAddonID { get; set; }
        public Nullable<int> OrderDetailID { get; set; }
        public Nullable<int> AddonID { get; set; }
        public string AddonName { get; set; }
        public Nullable<double> Quantity { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<double> Cost { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string LastUpdateBy { get; set; }
        public Nullable<System.DateTime> LastUpdateDT { get; set; }
    }
    public class OrderCheckoutBLL
    {
        public int OrderCheckoutID { get; set; }
        public Nullable<int> OrderID { get; set; }
        public Nullable<int> PaymentMode { get; set; }
        public Nullable<double> AmountPaid { get; set; }
        public Nullable<double> AmountTotal { get; set; }
        public Nullable<double> ServiceCharges { get; set; }
        public Nullable<double> GrandTotal { get; set; }
        public Nullable<double> Tax { get; set; }
        public Nullable<double> DiscountAmount { get; set; }
        public string CheckoutDate { get; set; }
        public string SessionID { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string LastUpdateBy { get; set; }

        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
    }
    public class OrderCustomerBLL
    {
        public int CustomerOrderID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string LocationURL { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<int> OrderID { get; set; }
        public string AddressNickName { get; set; }
        public string AddressType { get; set; }
        public string DeliveryArea { get; set; }
    }
    public class PushNoticationBLL
    {
        public string DeviceID { get; set; }
        public string Type { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }

    public class SettingBLL
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string ArabicTitle { get; set; }
        public string Description { get; set; }
        public string ArabicDescription { get; set; }
        public string Image { get; set; }
        public string AlternateImage { get; set; }
        public string PageName { get; set; }
        public string Type { get; set; }
        public Nullable<int> DisplayOrder { get; set; }
        public List<LocationJunc> Locations { get; set; }
        public List<Locations> SettingLocations { get; set; }
    }

    public class Customers
    {
        public int CustomerID { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Sex { get; set; }
        public string Mobile { get; set; }
        public string ImagePath { get; set; }
        public int? UserID { get; set; }
        public int? LocationID { get; set; }
        public bool? IsEmail { get; set; }
        public bool? IsSMS { get; set; }
        public string City { get; set; }
    }
    public class CustomerCarsResponse:Rsp
    {
        public List<Cars> CarList { get; set; }
    }
    public class CustomerNotificationResponse : Rsp
    {
        public List<NotificationBLL> Notifications { get; set; }
    }
    public class LoginResponse
    {
        public Customers Customer { get; set; }
        public List<Cars> CarList { get; set; }
        public List<CarSellList> CarFavourites { get; set; }
        public List<NotificationBLL> Notifications { get; set; }
        public List<CarSellList> MyAds { get; set; }
        public List<OrdersList> RecentOrders = new List<OrdersList>();
        public int Status { get; set; }
        public string Description { get; set; }
    }
    public class RspCustomerOrders : Rsp
    {
        public List<OrdersList> Orders = new List<OrdersList>();
    }
    public class CarFavouriteList
    {
        public int CarFavouriteID { get; set; }
        public Nullable<int> CarSellID { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
    }
    public class Settingv2Rsp
    {
        public List<ServiceBLL> Services { get; set; }
        public List<SettingBLL> Settings { get; set; }
        public List<AmenitiesBLL> Amenities { get; set; }
        public List<LandmarkBLL> Landmarks { get; set; }
        public List<UsersList> Brands { get; set; }
        public List<CityList> Cities { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public string AppstoreVersion { get; set; }
    }
    public class OffersRsp : Rsp
    { public List<DiscountBLL> Offers { get; set; } }
    public class SettingRsp
    {

        public List<Locations> Location { get; set; }
        public List<ServiceBLL> Services { get; set; }
        public List<SettingBLL> Settings { get; set; }
        public List<AmenitiesBLL> Amenities { get; set; }
        public List<LandmarkBLL> Landmarks { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
    }
    //public class UsersListResponse
    //{

    //    public List<UsersList> users = new List<UsersList>();

    //    public int Status { get; set; }
    //    public string Description { get; set; }
    //}
    public class UsersList
    {
        public int? UserID { get; set; }
        public string BrandName { get; set; }
        public string BrandImage { get; set; }
        public Nullable<int> StatusID { get; set; }
    }
    public class LoginVerificationResponse
    {
        public bool verified { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
    }
    public class Cars
    {
        public int? CarID { get; set; }
        public int? RowID { get; set; }
        public int CustomerID { get; set; }
        public string VinNo { get; set; }
        public Nullable<int> MakeID { get; set; }
        public string MakerImage { get; set; }
        public string MakerName { get; set; }
        public Nullable<int> ModelID { get; set; }
        public string ModelName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<int> Year { get; set; }
        public string CheckLitre { get; set; }
        public string RegistrationNo { get; set; }
        public string ImagePath { get; set; }
        public Nullable<int> LocationID { get; set; }

        public string SessionID { get; set; }
        public string RegistrationNoP1 { get; set; } = "";
        public string RegistrationNoP2 { get; set; } = "";
        public string RegistrationNoP3 { get; set; } = "";
        public string RegistrationNoP4 { get; set; } = "";
        public int? UserID { get; set; }
        public int? StatusID { get; set; }
        public bool? IsFavourite { get; set; }

        public List<OrdersList> Orders = new List<OrdersList>();
    }

    public class CarSellList
    {
        public int CarSellID { get; set; }
        public int? CustomerID { get; set; }
        public int? BodyTypeID { get; set; }
        public string Name { get; set; } = "";
        public string CustomerContact { get; set; } = "";
        public string Description { get; set; } = "";
        public string RegistrationNo { get; set; } = "";
        public string BodyType { get; set; } = "";
        public string FuelType { get; set; } = "";
        public string EngineType { get; set; } = "";
        public Double? Kilometer { get; set; }
        public string Year { get; set; } = "";
        public string MakeName { get; set; } = "";
        public string ModelName { get; set; } = "";
        public int? MakeID { get; set; }
        public int? ModelID { get; set; }
        public string Transmition { get; set; } = "";
        public double Price { get; set; }
        public bool IsInspected { get; set; }
        public int? CityID { get; set; }
        public string CityName { get; set; }
        public string Reason { get; set; }
        public int? CountryID { get; set; }
        public string Address { get; set; }
        public int? CarSellAddID { get; set; }
        public string BodyColor { get; set; } = "";
        public string Assembly { get; set; } = "";
        public int? StatusID { get; set; }
        public string EngineSize { get; set; } = "";
        public string CreatedDate { get; set; } = "";
        public string Image { get; set; } = "";
        public string CountryCode { get; set; } = "";
        public List<CarSellImageList> CarSellImages { get; set; }
        public List<CarSellFeatureList> CarSellFeatures { get; set; }

    }
    public class CarSellAds
    {
        public int? MakeID { get; set; }
        public int? ModelID { get; set; }
        public int? BodyTypeID { get; set; }
    }

    public class CarSellRsp : Rsp
    {
        public List<CarSellList> CarSellList { get; set; }
        public List<CountryList> CountryList { get; set; }
        public List<FeatureList> Features { get; set; }
        public List<BodyTypeList> BodyTypes { get; set; }
    }
    public class CarSellInsertRsp
    {
        public CarSellList CarSell { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
    }
    public class BodyTypeList
    {
        public int BodyTypeID { get; set; } = 0;
        public string Name { get; set; } = "";
        public string ArabicName { get; set; } = "";
        public string Image { get; set; } = "";
        public Nullable<int> StatusID { get; set; } = 0;
    }
    public class CarSellFeatureList
    {
        public int? CarSellID { get; set; }
        public int? FeatureID { get; set; }
        public string Name { get; set; }
        public string ArabicName { get; set; }
        public string Image { get; set; }
    }
    public class CountryList
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Continent { get; set; }
        public List<CityList> CityList { get; set; }
    }
    public class CityList
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string CountryCode { get; set; }
        public string District { get; set; }
    }
    public class FeatureList
    {
        public int FeatureID { get; set; }
        public string Name { get; set; }
        public string ArabicName { get; set; }
        public string Image { get; set; }
    }
    public class CarSellImageList
    {
        public int ID { get; set; }
        public int? CarSellID { get; set; }
        public string Image { get; set; }

        public int? StatusID { get; set; }
    }
    public class OrdersList
    {
        public string MakerName { get; set; }
        public Nullable<int> ModelID { get; set; }
        public string ModelName { get; set; }
        public string VATNo { get; set; }
        public string CompanyName { get; set; }
        public string LocationName { get; set; }
        public string LocationAddress { get; set; }
        public string CompanyImage { get; set; }
        public int? OrderCheckoutID { get; set; }
        public int OrderID { get; set; }
        public int? TransactionNo { get; set; }
        public int? OrderNo { get; set; }
        public int? CarID { get; set; }
        public string BayName { get; set; }
        public string RegistrationNo { get; set; }
        public int? CustomerID { get; set; } = 0;
        public Nullable<int> LocationID { get; set; }
        public string OrderPunchDate { get; set; }
        public string CheckoutDate { get; set; }
        public string MechanicName { get; set; }
        public int? Status { get; set; }
        public int? PaymentMode { get; set; }
        public double? AmountTotal { get; set; }
        public double? Tax { get; set; }
        public double? AmountDiscount { get; set; }
        public double? RefundedAmount { get; set; }
        public double? GrandTotal { get; set; }
        public double? PartialAmountReceived { get; set; }
        public bool? IsPartialPaid { get; set; }
        public string DiscountCode { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Twitter { get; set; }
        public string Snapchat { get; set; }
        public string NoPlateImage { get; set; }
        public double? CashAmount { get; set; }
        public double? CardAmount { get; set; }
        public float? NextKM { get; set; }
        public string CardType { get; set; }
        public string CustomerContact { get; set; }
        public string CustomerName { get; set; }
        public string Currency { get; set; }
        public int? CreditCustomerID { get; set; }
        public List<OItemsList> Items = new List<OItemsList>();
        public CreditCustomerA4 CreditCustomerInfo { get; set; }
    }
    public class CreditCustomerA4
    {
        public string SellerName { get; set; }
        public string SellerAddress { get; set; }
        public string SellerVAT { get; set; }
        public string SellerContact { get; set; }
        public string BuyerName { get; set; }
        public string BuyerAddress { get; set; }
        public string BuyerContact { get; set; }
        public string BuyerVAT { get; set; }
    }
    public partial class ReceiptBLL
    {
        public int ReceiptID { get; set; }
        public string ReceiptName { get; set; }
        public string CompanyTitle { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyPhones { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyWebsite { get; set; }
        public string Companytagline { get; set; }
        public string CompanyLogoURL { get; set; }
        public string Footer { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public string Snapchat { get; set; }
        public string LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsCompanyTagline { get; set; }
        public Nullable<int> StatusID { get; set; }
        public bool IsActive { get; set; }
        public int LocationID { get; set; }
        public Nullable<bool> IsA4Spacing { get; set; }

    }
    public class OItemsList
    {
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public Nullable<int> ItemID { get; set; }
        public Nullable<int> PackageID { get; set; }
        public string ItemName { get; set; }
        public string AlternateName { get; set; }
        public string ItemImage { get; set; }
        public Nullable<double> Quantity { get; set; }
        public Nullable<double> RefundQty { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<double> DiscountAmount { get; set; }
        public Nullable<double> RefundAmount { get; set; }
        public Nullable<double> Cost { get; set; }
        public Nullable<int> StatusID { get; set; }

        public List<OPackageDetailList> Packages = new List<OPackageDetailList>();
    }
    public class OPackageDetailList
    {
        public int OrderPkgDetailID { get; set; }
        public int OrderDetailID { get; set; }
        public Nullable<int> ItemID { get; set; }
        public Nullable<double> Quantity { get; set; }
        public Nullable<double> Discount { get; set; }
        public Nullable<double> Cost { get; set; }
        public string DiscoutType { get; set; }
        public string Name { get; set; }
        public string ItemName { get; set; }
        public string AlternateName { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<int> StatusID { get; set; }
    }
    public class CheckoutDetailsOrder
    {
        public int OrderCheckOutDetailID { get; set; }
        public int OrderCheckoutID { get; set; }
        public Nullable<int> PaymentMode { get; set; }
        public Nullable<double> AmountPaid { get; set; }
        public Nullable<double> AmountDiscount { get; set; }
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public string CardType { get; set; }

    }
    public class CarNotes
    {
        public int NotesID { get; set; }
        public string NotesComment { get; set; }
        public string NotesStatus { get; set; }
        public string Signature { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> CarID { get; set; }

        public List<CarNotesImagesList> NotesImages = new List<CarNotesImagesList>();
    }
    
    public class CarNotesImagesList
    {
        public int NotesImageID { get; set; }
        public int NotesID { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
    }
    public class NotificationBLL
    {
        public int NotificationID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string Image { get; set; }
        public string Date { get; set; }
        public string Type { get; set; }
        public bool? IsRead { get; set; }
        public Nullable<int> CustomerID { get; set; }
    }
    public class RspCarMake : Rsp
    { public List<CarMakeList> CarMake = new List<CarMakeList>(); }
    public class CarMakeList
    {
        public List<CarModelList> CarModels = new List<CarModelList>();
        public int MakeID { get; set; }
        public string Name { get; set; }
        public string ArabicName { get; set; }
        public string ImagePath { get; set; }

    }
    public class CarModelList
    {
        public int? ModelID { get; set; }
        public int? MakeID { get; set; }
        public string Name { get; set; }
        public string ArabicName { get; set; }
        public short? Year { get; set; }
        public string EngineNo { get; set; }
        public string RecommendedLitres { get; set; }
    }
    public class OrderLetterResponse
    {
        public int? OrderID { get; set; }
        public string Path { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
    }

    public class AIChat
    {
        public int? chatID { get; set; }
        public int? carID { get; set; }
        public int? customerID { get; set; }
        public string chatType { get; set; }
        public List<AIChatHistory> chatHistory { get; set; }
    }

    public class AIChatHistory
    {
        public string id { get; set; }
        public string sendImage { get; set; }
        public string sendText { get; set; }
        public string responseImage { get; set; }
        public string responseText { get; set; }
        public string likeState { get; set; }
    }

    public class AIChatModelRsp
    {
        public List<AIChat> chats { get; set; }
        public string description { get; set; }
        public int status { get; set; }
    }
}
