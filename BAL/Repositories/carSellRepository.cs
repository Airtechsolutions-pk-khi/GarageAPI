
using DAL.DBEntities;
using DAL.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WebAPICode.Helpers;
using DAL.DBEntities2;
using System.Security.Cryptography;

namespace BAL.Repositories
{
    public class carSellRepository : BaseRepository2
    {
        //public Cars cars;
        public carSellRepository()
            : base()
        {
            DBContext2 = new Garage_Entities();

        }
        public carSellRepository(Garage_Entities contextDB2)
            : base(contextDB2)
        {
            DBContext2 = contextDB2;
        }
        public async Task<CarSellRsp> GetCarSellList()
        {

            var rsp = new CarSellRsp();
            try
            {
                var ds = await GetInfo();
                var _dtCarSellInfo = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0])).ToObject<List<CarSellList>>().ToList();

                var _dtFeatureInfo = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[1])).ToObject<List<CarSellFeatureList>>().ToList();

                var _dtCSImageInfo = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[2])).ToObject<List<CarSellImageList>>().ToList();

                var _dtCityInfo = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[3])).ToObject<List<CityList>>().ToList();

                var _dtCountryInfo = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[4])).ToObject<List<CountryList>>().ToList();

                //var _dtFeatureJuncInfo = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[5])).ToObject<List<Feature>>().ToList();

                var _dtFeatureInfoALL = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[5])).ToObject<List<FeatureList>>().ToList();

                foreach (var i in _dtCarSellInfo)
                {
                    i.CreatedDate = DateParse(i.CreatedDate.ToString());
                    i.CarSellImages = _dtCSImageInfo.Where(x => x.CarSellID == i.CarSellID).ToList();
                    foreach (var j in i.CarSellImages)
                    {
                        j.Image = j.Image == null ? null : ConfigurationSettings.AppSettings["ApiURL"].ToString() + j.Image;
                    }
                    i.Image = i.CarSellImages.Count > 0 ? i.CarSellImages[0].Image : null;
                    i.CarSellFeatures = _dtFeatureInfo.Where(x => x.CarSellID == i.CarSellID).ToList();
                    foreach (var j in i.CarSellFeatures)
                    {
                        j.Image = j.Image == null ? null : ConfigurationSettings.AppSettings["CAdminURL"].ToString() + j.Image;
                    }
                }
                foreach (var item in _dtCountryInfo)
                {
                    item.CityList = _dtCityInfo.Where(x => x.CountryCode == item.Code).ToList();
                }

                rsp.Features = _dtFeatureInfoALL;
                rsp.CarSellList = _dtCarSellInfo;
                rsp.CountryList = _dtCountryInfo;
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
        public async Task<DataSet> GetInfo()
        {
            try
            {
                SqlParameter[] p = new SqlParameter[0];
                return (new DBHelper().GetDatasetFromSP)("sp_GetCarSell_CAPI", p);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public HttpResponseMessage Post()
        {
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                var docfiles = new List<string>();
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var filePath = HttpContext.Current.Server.MapPath("~/" + postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    docfiles.Add(filePath);
                }
                result = new HttpResponseMessage(HttpStatusCode.Created);
            }
            else
            {
                result = new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            return result;
        }
        public CarSellInsertRsp InsertCarSell(CarSellList carSell)
        {
            var dsc = 0;
            CarSellImage carimg = new CarSellImage();
            CarSellInsertRsp rsp = new CarSellInsertRsp();
            try
            {
                try
                {
                    dsc = InsertCar(carSell);
                    foreach (var item in carSell.CarSellImages)
                    {
                        string im = item.Image;
                        if (im != null && im != "")
                        {
                            try
                            {
                                var chkImagePath = IsBase64Encoded(im
                                               .Replace("data:image/png;base64,", "")
                                               .Replace("data:image/jpg;base64,", "")
                                               .Replace("data:image/jpeg;base64,", ""));

                                if (chkImagePath)
                                {
                                    if (im != null && im != "")
                                    {
                                        carimg.Image = uploadFiles(im, "CarSell");

                                        SqlParameter[] p1 = new SqlParameter[1];

                                        p1[0] = new SqlParameter("@Image", carimg.Image);
                                        (new DBHelper().ExecuteNonQueryReturn)("sp_insertCarSellImages_CAPI", p1);
                                    }
                                }
                            }
                            catch { }
                        }

                    }
                }
                catch (Exception ex)
                { }
                if (dsc > 0)
                {
                    //rsp.CarSell = carSell;
                    rsp.Status = 1;
                    rsp.Description = "Car has been added successfully";
                }
                else
                {
                    //rsp.CarSell = carSell;
                    rsp.Status = 0;
                    rsp.Description = "Failed to add car";
                }
                //SqlParameter[] p = new SqlParameter[3];
                //p[0] = new SqlParameter("@RegistrationNo", carSell.RegistrationNo);
                //p[1] = new SqlParameter("@StatusID", carSell.StatusID);
                //p[2] = new SqlParameter("@CustomerID", carSell.CustomerID);
                //var check = (new DBHelperPOS().GetDatasetFromSP)("sp_CheckCarSellNoPlate", p);

                //if (check.Tables[0].Rows.Count == 0)
                //{

                //}
                //else
                //{
                //    //rsp.CarSell = carSell;
                //    rsp.Status = 0;
                //    rsp.Description = "Car Already Listed For Selling";
                //}
            }
            catch (Exception e)
            {
                //rsp.CarSell = carSell;
                rsp.Status = 0;
                rsp.Description = e.Message;
            }
            return rsp;
        }
        public Rsp InsertCarFavourite(CarFavouriteList obj)
        {
            var rsp = new Rsp();
            try
            {
              var  dsc = InsertCarFav(obj);
                return new Rsp
                {
                    Status = 1,
                    Description = "Success"
                };

            }
            catch { }

            return  new Rsp
            {
                Status = 0,
                Description="Failed"
            };

        }
        public string UploadImage(string image, string folderName)
        {
            try
            {
                var chkImage = IsBase64Encoded(image
                    .Replace("data:image/png;base64,", "")
                    .Replace("data:image/jpg;base64,", "")
                    .Replace("data:image/jpeg;base64,", ""));

                if (chkImage)
                {
                    if (image != null && image != "")
                    {
                        image = uploadFiles(image, folderName);
                    }
                }
            }
            catch { }

            return image;
        }
        public CarRsp EditCar(Cars cars)
        {
            CarRsp rsp = new CarRsp();
            try
            {
                var chkImagePath = false;
                try
                {
                    chkImagePath = IsBase64Encoded(cars.ImagePath
                       .Replace("data:image/png;base64,", "")
                       .Replace("data:image/jpg;base64,", "")
                       .Replace("data:image/jpeg;base64,", ""));

                    if (chkImagePath)
                    {
                        if (cars.ImagePath != null && cars.ImagePath != "")
                        {
                            cars.ImagePath = uploadFiles(cars.ImagePath, "Cars");
                        }
                    }
                }
                catch { }

                var ds = ap_EditCar(cars, chkImagePath);
                rsp.cars = cars;
                rsp.Status = 1;
                rsp.Description = "Car has been Updated successfully";
            }
            catch (Exception ex)
            {
                rsp.cars = cars;
                rsp.Status = 0;
                rsp.Description = "Car can not Updated successfully";
            }
            return rsp;
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

        public int InsertCar(CarSellList carSell)
        {
            try
            {
                SqlParameter[] p = new SqlParameter[22];
                p[0] = new SqlParameter("@Name", carSell.Name);
                p[1] = new SqlParameter("@Description", carSell.Description);
                p[2] = new SqlParameter("@RegistrationNo", carSell.RegistrationNo);
                p[3] = new SqlParameter("@BodyType", carSell.BodyType);
                p[4] = new SqlParameter("@FuelType", carSell.FuelType);
                p[5] = new SqlParameter("@EngineType", carSell.EngineType);
                p[6] = new SqlParameter("@Year", carSell.Year);
                p[7] = new SqlParameter("@CustomerID", carSell.CustomerID);
                p[8] = new SqlParameter("@MakeID", carSell.MakeID);
                p[9] = new SqlParameter("@ModelID", carSell.ModelID);
                p[10] = new SqlParameter("@Transmition", carSell.Transmition);
                p[11] = new SqlParameter("@Kilometer", carSell.Kilometer);
                p[12] = new SqlParameter("@Price", carSell.Price);
                p[13] = new SqlParameter("@IsInspected", carSell.IsInspected);
                p[14] = new SqlParameter("@CityID", carSell.CityID);
                p[15] = new SqlParameter("@Address", carSell.Address);
                p[16] = new SqlParameter("@CarSellAddID", carSell.CarSellAddID);
                p[17] = new SqlParameter("@BodyColor", carSell.BodyColor);
                p[18] = new SqlParameter("@Assembly", carSell.Assembly);
                p[19] = new SqlParameter("@StatusID", carSell.StatusID);
                p[20] = new SqlParameter("@CreatedDate", DateTime.UtcNow.AddMinutes(180));
                p[21] = new SqlParameter("@CreatedBy", carSell.CustomerID);
                //p[22] = new SqlParameter("@UpdatedBy", carSell.CustomerID);
                //p[23] = new SqlParameter("@UpdatedDate", DateTime.UtcNow.AddMinutes(180));

                carSell.CarSellID = int.Parse((new DBHelper().GetDatasetFromSP)("sp_InsertCarSell", p).Tables[0].Rows[0][0].ToString());
                //carSell.Image = carSell.Image != null ? ConfigurationSettings.AppSettings["ApiURL"].ToString() + carSell.Image : null;

                foreach (var i in carSell.CarSellFeatures)
                {
                    i.FeatureID = i.FeatureID;
                    i.CarSellID = carSell.CarSellID;
                    SqlParameter[] p1 = new SqlParameter[2];
                    p1[0] = new SqlParameter("@CarSellID", i.CarSellID);
                    p1[1] = new SqlParameter("@FeatureID", i.FeatureID);
                    (new DBHelper().ExecuteNonQueryReturn)("sp_insertCarSellFeature_CAPI", p1);
                }
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public int InsertCarFav(CarFavouriteList obj)
        {
            try
            {
                SqlParameter[] p = new SqlParameter[5];
                p[0] = new SqlParameter("@CarSellID", obj.CarSellID);
                p[1] = new SqlParameter("@CustomerID", obj.CustomerID);
                p[2] = new SqlParameter("@StatusID", obj.StatusID);
                p[3] = new SqlParameter("@LastUpdatedBy", "Admin");
                p[4] = new SqlParameter("@LastUpdatedDate", DateTime.UtcNow.AddMinutes(180));

                (new DBHelper().ExecuteNonQueryReturn)("sp_insertCarFav_CAPI", p);
               
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public int ap_EditCar(Cars cars, bool iseditimage)
        {
            try
            {
                SqlParameter[] p = new SqlParameter[13];
                p[0] = new SqlParameter("@CarID", cars.CarID);
                p[1] = new SqlParameter("@RowID", cars.RowID);
                p[2] = new SqlParameter("@CustomerID", cars.CustomerID);
                p[3] = new SqlParameter("@MakeID", cars.MakeID);
                p[4] = new SqlParameter("@Name", cars.Name);
                p[5] = new SqlParameter("@ModelID", cars.ModelID);
                p[6] = new SqlParameter("@Description", cars.Description);
                p[7] = new SqlParameter("@Year", cars.Year);
                p[8] = new SqlParameter("@RegistrationNo", cars.RegistrationNo);
                p[9] = new SqlParameter("@ImagePath", cars.ImagePath);
                p[10] = new SqlParameter("@LocationID", cars.LocationID);
                p[11] = new SqlParameter("@UserID", cars.UserID);
                p[12] = new SqlParameter("@StatusID", cars.StatusID);
                if (iseditimage)
                {
                    cars.ImagePath = cars.ImagePath != null ? ConfigurationSettings.AppSettings["ApiURL"].ToString() + cars.ImagePath : null;
                }
                return (new DBHelperPOS().ExecuteNonQueryReturn)("sp_UpdateCars", p);

            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public DataTable AddReviewADO(ReviewsBLL obj)
        {
            try
            {
                SqlParameter[] p = new SqlParameter[7];
                p[0] = new SqlParameter("@Name", obj.Name);
                p[1] = new SqlParameter("@Message", obj.Message);
                p[2] = new SqlParameter("@Rate", obj.Rate);
                p[3] = new SqlParameter("@StatusID", 1);
                p[4] = new SqlParameter("@LastUpdatedDate", DateTime.UtcNow);
                p[5] = new SqlParameter("@LocationID", obj.LocationID);
                p[6] = new SqlParameter("@Date", DateTime.ParseExact(obj.Date, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture));
                return (new DBHelperPOS().GetDatasetFromSP)("sp_InsertReview_CAPI", p).Tables[0];

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public OrderLetterResponse OrderPrintLetter(string OrderID)
        {

            OrderLetterResponse rsp = new OrderLetterResponse();
            int orderid = 0;
            try
            {
                orderid = int.Parse(OrderID);
            }
            catch
            {
                rsp.OrderID = 0;
                rsp.Status = (int)eStatus.Failed;
                rsp.Description = "Invalid OrderID";
                return rsp;
            }
            try
            {

                var data = DBContext2.Orders.Where(x =>
                  x.OrderID == orderid).FirstOrDefault();

                if (data != null)
                {
                    string checklist = "";
                    string orderdetails = "";
                    string content = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/Template") + "\\" + "CustomerLetterV3.txt");

                    #region CarNotes
                    var carImagesHtml = "";
                    var CarNotes = data.CarNotes.FirstOrDefault();
                    var NotesDesc = data.CarNotes.FirstOrDefault() != null ? data.CarNotes.FirstOrDefault().NotesComment : "";
                    if (CarNotes != null)
                    {
                        var CarImages = CarNotes.CarNotesImages.ToList();
                        if (CarImages.Count > 0)
                        {
                            carImagesHtml += "<h4>صور السيارة</h4>";
                            foreach (var item in CarImages)
                            {
                                item.ImagePath = item.ImagePath == null ? "" : ConfigurationSettings.AppSettings["ApiURL"].ToString() + item.ImagePath;
                                carImagesHtml += "<img src='" + item.ImagePath + "' style='margin:2px;'  width='150px' />";
                            }
                            carImagesHtml += "<h5>" + NotesDesc + "</h4>";
                        }
                    }
                    var signatureNotes = data.CarNotes.FirstOrDefault() != null ? ConfigurationSettings.AppSettings["ApiURL"].ToString() + data.CarNotes.FirstOrDefault().Signature : "";

                    #endregion CarNotes

                    #region checklist

                    var datatChecklist = data.OrdersChecklists.Where(x => x.InspectionDetailID != 0).ToList();
                    if (datatChecklist.Count > 0)
                    {
                        var _count = datatChecklist.Count % 2;
                        _count = _count == 0 ? datatChecklist.Count : (datatChecklist.Count + 1);

                        for (int i = 0; i < _count; i++)
                        {
                            checklist += "<tr width='100%' style='border-top:0px solid  #ffffff !important;'>                                                                                     ";
                            checklist += "    <td dir='rtl' width='50%'>                                                                                 ";
                            checklist += "        <table class='table table1'>                                                           ";
                            checklist += "            <tbody>                                                                      ";
                            checklist += "                <tr class='sec2-row'>                                                                     ";
                            checklist += "                    <td class='text-right'><h5>" + datatChecklist[i].AlternateName + "</h5><h5>" + datatChecklist[i].Name + "</h5></td>      ";
                            checklist += "                    <td style='padding:2px;' class='text-left'>                                            ";
                            checklist += "                        <h4><strong>" + datatChecklist[i].Value + "</strong></h4>                                   ";
                            checklist += "                    </td>                                                                ";
                            checklist += "                </tr>                                                                    ";
                            checklist += "            </tbody>                                                                     ";
                            checklist += "        </table>                                                                         ";
                            checklist += "    </td>                                                                                ";
                            try
                            {
                                if (datatChecklist.Count > i + 1)
                                {
                                    checklist += "<td dir='rtl' width='50%'>                                                                                 ";
                                    checklist += "    <table class='table table1'>                                                           ";
                                    checklist += "        <tbody>                                                                      ";
                                    checklist += "            <tr class='sec2-row'>                                                                     ";
                                    checklist += "                <td class='text-right'><h5>" + datatChecklist[i + 1].AlternateName + "</h5><h5>" + datatChecklist[i + 1].Name + "</h5></td>      ";
                                    checklist += "                <td class='text-left'>                                            ";
                                    checklist += "                    <h4><strong>" + datatChecklist[i + 1].Value + "</strong></h4>                                   ";
                                    checklist += "                </td>                                                                ";
                                    checklist += "            </tr>                                                                    ";
                                    checklist += "        </tbody>                                                                     ";
                                    checklist += "    </table>                                                                         ";
                                    checklist += "</td>          ";
                                }
                            }
                            catch (Exception)
                            {
                                checklist += "<td dir='rtl' width='50%'>                                                                                 ";
                                checklist += "    <table class='table table1'>                                                           ";
                                checklist += "        <tbody>                                                                      ";
                                checklist += "            <tr class='sec2-row'>                                                                     ";
                                checklist += "                <td style='padding:2px;' class='text-right'><h4></h4></td>      ";
                                checklist += "                <td style='padding:2px;' class='text-left'>                                            ";
                                checklist += "                    <h4><strong></strong></h4>                                   ";
                                checklist += "                </td>                                                                ";
                                checklist += "            </tr>                                                                    ";
                                checklist += "        </tbody>                                                                     ";
                                checklist += "    </table>                                                                         ";
                                checklist += "</td>                                                                                ";
                            }
                            checklist += "</tr>    ";
                            i += 1;
                        }
                        content = content.Replace("#checklist#", checklist);
                    }
                    else
                    {
                        checklist += "<tr width='100%' style='border-top:0px solid #ffffff !important;'>                                                                                     ";
                        checklist += "    <td colspan='2' dir='rtl' >                                                                                 ";
                        checklist += "        <table class='table table1'>                                                           ";
                        checklist += "            <tbody>                                                                      ";
                        checklist += "                <tr class='sec2-row'>                                                                     ";
                        checklist += "                    <td style='padding:2px;' class='text-center'><h4>لاتوجد بيانات</h4></td>      ";
                        checklist += "                    <td style='padding:2px;' class='text-left'>                                            ";
                        checklist += "                    </td>                                                                ";
                        checklist += "                </tr>                                                                    ";
                        checklist += "            </tbody>                                                                     ";
                        checklist += "        </table>                                                                         ";
                        checklist += "    </td>                                                                             ";
                        checklist += "</tr>    ";
                        content = content.Replace("#checklist#", checklist);
                    }
                    #endregion checklist

                    #region orderdetail
                    var isReturnBar = false;
                    if (data.OrderDetails.Count > 0)
                    {
                        foreach (var item in data.OrderDetails.Where(x => x.StatusID != 203))
                        {
                            //for letter refund bar
                            item.RefundQty = item.RefundQty ?? 0;
                            isReturnBar = isReturnBar == true ? true : item.StatusID == 204 ? true : Double.Parse(item.RefundQty.ToString()) > 0 ? true : false;

                            orderdetails += "<tr>";
                            var returnQty = isReturnBar == true ? "<br/><strong>(" + item.RefundQty + " - Return/مسترجع)</strong></h4>" : "";
                            if (item.ItemID != 0 && item.ItemID != null)
                            {
                                orderdetails += " <td style='padding:7px;'>" +
                                    "<h4><strong>" + item.Item.Name + "</strong></h4>" +
                                    "<h4><strong>" + item.Item.NameOnReceipt + "</strong></h4>" +
                                    "<h5>" + item.Item.Description + "</h5>" +
                                    returnQty +
                                    "</td>";
                            }
                            else if (item.PackageID != 0 && item.PackageID != null)
                            {
                                orderdetails += " <td style='padding:7px; 7px; 3px; 7px;'> " +
                                    "<h4><strong>" + item.Package.Name + "</strong></h4>" +
                                    "<h4><strong>" + item.Package.ArabicName + "</strong></h4>" +
                                    returnQty +
                                    "<br/>";
                                foreach (var odp in item.OrderDetailPackages.Where(x => x.StatusID == 1))
                                {
                                    orderdetails += "<h5>--->   " + odp.Quantity + "X " + odp.Item.Name + " || " + odp.Item.NameOnReceipt + "</h5>";
                                }
                                orderdetails += "</td>";
                            }
                            orderdetails += " <td style='padding:7px;' class='text-center'><h4>" + item.Quantity + "</h4></td>";
                            if (item.DiscountAmount > 0 && item.DiscountAmount != null)
                            {
                                var discountAmount = Math.Round(float.Parse((item.Price - item.DiscountAmount).ToString()), 2);
                                orderdetails += "<td style='padding:7px;' class='text-left'><h4><s>" + Math.Round(float.Parse(item.Price.ToString()), 2) + "</s>" + discountAmount + "</h4></td>";
                            }
                            else
                                orderdetails += "<td style='padding:7px;' class='text-left'><h4>" + Math.Round(float.Parse(item.Price.ToString()), 2) + "</h4></td>";

                            orderdetails += "</tr>";
                        }
                        content = content.Replace("#items#", orderdetails);
                        // rsp.Items = Items;
                    }
                    else
                    {
                        //rsp.Items = new List<OrderItems>();
                        content = content.Replace("#items#", orderdetails);
                    }
                    #endregion orderdetail

                    var carinfo = data.Car;
                    var ordercheckout = data.OrderCheckouts.FirstOrDefault();
                    var companyinfo = DBContext2.Users.Where(x => x.UserID == data.Location.UserID).FirstOrDefault();
                    #region logo
                    var logoHtml = "<div style='padding: 25px 0 40px 0; '><img src=" + ConfigurationSettings.AppSettings["AdminURL"].ToString() + companyinfo.ImagePath + " height='100px' /></div>";
                    #endregion logo

                    #region socialmedia
                    var receipt = data.Location.Receipts.Where(x => x.IsActive == true).FirstOrDefault();
                    try
                    {
                        if (receipt != null)
                        {
                            var contentSM = "";
                            if (receipt.CompanyWebsite != null && receipt.CompanyWebsite != "")
                            {
                                contentSM += "<span style='margin-right:14px'><img src='" + ConfigurationSettings.AppSettings["ApiURL"].ToString() + "/Template/web.png' width='35px' />  " + receipt.CompanyWebsite + "</span>";
                            }
                            if (receipt.Instagram != null && receipt.Instagram != "")
                            {
                                contentSM += "<span style='margin-right:14px'><img src='" + ConfigurationSettings.AppSettings["ApiURL"].ToString() + "/Template/ig.png' width='35px' />  " + receipt.Instagram + "</span>";
                            }
                            if (receipt.Snapchat != null && receipt.Snapchat != "")
                            {
                                contentSM += "<span style='margin-right:14px'><img src='" + ConfigurationSettings.AppSettings["ApiURL"].ToString() + "/Template/sc.png' width='35px' />  " + receipt.Snapchat + "</span>";
                            }
                            if (receipt.Facebook != null && receipt.Facebook != "")
                            {
                                contentSM += "<span style='margin-right:14px'><img src='" + ConfigurationSettings.AppSettings["ApiURL"].ToString() + "/Template/fb.png' width='35px' />  " + receipt.Facebook + "</span>";
                            }
                            if (receipt.Twitter != null && receipt.Twitter != "")
                            {
                                contentSM += "<span style='margin-right:14px'><img src='" + ConfigurationSettings.AppSettings["ApiURL"].ToString() + "/Template/tw.png' width='35px' />  " + receipt.Twitter + "</span>";
                            }
                            if (receipt.CompanyEmail != null && receipt.CompanyEmail != "")
                            {
                                contentSM += "<span style='margin-right:14px'><img src='" + ConfigurationSettings.AppSettings["ApiURL"].ToString() + "/Template/email.png' width='35px' />  " + receipt.CompanyEmail + "</span>";
                            }
                            content = content.Replace("#socialmedia#", contentSM);
                        }
                        else
                        {
                            content = content.Replace("#socialmedia#", "");
                        }
                    }
                    catch (Exception)
                    {
                        content = content.Replace("#socialmedia#", "");
                    }
                    #endregion socialmedia


                    var qrlink = "";
                    try
                    {
                        qrlink = new Zatca().ZatcaInvoiceQR(companyinfo.Company, companyinfo.VATNO, ordercheckout.Tax.ToString(), Convert.ToDateTime(data.OrderPunchDt).ToString("yyyy-MM-dd HH:mm:ss"), ordercheckout.GrandTotal.ToString());
                    }
                    catch { qrlink = ""; }
                    var cname = "-";
                    try
                    {
                        cname = data.Car.Customer.FullName;
                    }
                    catch { cname = "-"; }

                    isReturnBar = isReturnBar == true ? true : data.StatusID == 106 ? true : false;
                    var refundAmountHTML = "";
                    if (isReturnBar)
                    {

                        refundAmountHTML += "  <div class='row' style='background-color:#eaeaea; padding:0 20px;'>";
                        refundAmountHTML += "     <div class='col-4' style='padding:10px 0;'>                     ";
                        refundAmountHTML += "         <h4> <strong>" + ordercheckout.RefundedAmount + " SR</strong></h4>                    ";
                        refundAmountHTML += "     </div>                                                          ";
                        refundAmountHTML += "     <div class='col-8 text-right' style='padding:10px 0;'>          ";
                        refundAmountHTML += "         <h4>(Refund Amount) القيمة المسترجعة</h4>                          ";
                        refundAmountHTML += "     </div>                                                          ";
                        refundAmountHTML += " </div>                                                              ";
                        content = content.Replace("#refundbar#", "  <div class='row' style='background:#e8acac;margin-bottom:5px;'><div class='col-12 text-center'><h2 style='padding:10px 0 10px 0;font-size:26px'>اشعار دائن للفاتورة الضريبية المبسطة</h2></div></div>");
                    }
                    else
                        content = content.Replace("#refundbar#", "<div class='row' style='background:#eee;margin-bottom:5px;'><div class='col-12 text-center'><h2 style='padding:10px 0 10px 0;font-size:26px'>فاتورة ضريبية</h2></div></div>");

                    content = content.Replace("#logo#", logoHtml);
                    content = content.Replace("#qrlink#", qrlink);
                    //ConfigurationSettings.AppSettings["AdminAPIURL"].ToString() + "/QR-Receipt/receipt.html?id=" + data.OrderID);
                    content = content.Replace("#notes#", NotesDesc);
                    content = content.Replace("#signature#", signatureNotes);
                    content = content.Replace("#carmake#", carinfo.Make.Name);
                    content = content.Replace("#carmodel#", carinfo.Model.Name);
                    content = content.Replace("#year#", carinfo.Year.ToString());
                    content = content.Replace("#locationname#", data.Location.Name);
                    content = content.Replace("#customername#", cname);
                    content = content.Replace("#locationaddress#", data.Location.Address);
                    content = content.Replace("#locationcontact#", data.Location.ContactNo);
                    content = content.Replace("#mobile#", carinfo.Customer.Mobile.ToString().Replace("+", ""));
                    content = content.Replace("#checkliters#", data.CheckLiters.ToString());
                    content = content.Replace("#noplate#", carinfo.RegistrationNo);
                    content = content.Replace("#vinno#", carinfo.VinNo);
                    content = content.Replace("#transactionno#", data.TransactionNo.ToString());
                    content = content.Replace("#orderdate#", Convert.ToDateTime(data.OrderPunchDt).ToString("dd/MM/yyyy"));
                    content = content.Replace("#worker#", ordercheckout.SubUser.FirstName + " " + ordercheckout.SubUser.LastName);
                    content = content.Replace("#total#", ordercheckout.AmountTotal.ToString());
                    content = content.Replace("#tax#", ordercheckout.Tax.ToString());
                    content = content.Replace("#discount#", ordercheckout.AmountDiscount.ToString());
                    content = content.Replace("#subtotal#", ordercheckout.GrandTotal.ToString());
                    content = content.Replace("#refundtotal#", refundAmountHTML);
                    content = content.Replace("#vatno#", companyinfo == null ? "" : companyinfo.VATNO);
                    content = content.Replace("#carimages#", carImagesHtml);
                    ;
                    content = content.Replace("#footernotes#", receipt.Footer);
                    content = content.Replace("#showchecklist#", datatChecklist.Count > 0 ? "block" : "none");
                    content = content.Replace("#partialreceived#", ordercheckout.PartialAmountReceived.ToString());
                    content = content.Replace("#ispartialreceived#", ordercheckout.PartialAmountReceived > 0 ? "block" : "none");
                    var path = ConfigurationSettings.AppSettings["ApiURL"].ToString() + GetPdf(content, orderid);
                    rsp.Path = path.Replace("~", "");
                    rsp.OrderID = orderid;
                    rsp.Status = (int)eStatus.Success;
                    rsp.Description = "Success";
                }
            }
            catch (Exception ex)
            {
                rsp.OrderID = orderid;
                rsp.Status = (int)eStatus.Exception;
                rsp.Description = ex.Message;
            }
            return rsp;
        }
        private string GetPdf(string html, int orderid)
        {

            var htmlContent = html.ToString();
            var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
            var pdfBytes = htmlToPdf.GeneratePdf(htmlContent);
            var filename = orderid + "-" + DateTime.Now.Ticks.ToString();
            string folderPath = "~/OrderLetters/";   // set folder

            if (!Directory.Exists(HttpContext.Current.Server.MapPath(folderPath)))
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(folderPath));
            string FilePath = HttpContext.Current.Server.MapPath(folderPath + filename + ".pdf");
            var bw = new BinaryWriter(System.IO.File.Open(FilePath, FileMode.OpenOrCreate));
            bw.Write(pdfBytes);
            bw.Close();



            return folderPath + filename + ".pdf";
        }
    }

}
