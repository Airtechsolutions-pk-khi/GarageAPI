
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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using WebAPICode.Helpers;

namespace BAL.Repositories
{
    public class carRepository : BaseRepository2
    {
        public Cars cars;
        public carRepository()
            : base()
        {
            DBContext2 = new Garage_Entities();

        }
        public carRepository(Garage_Entities contextDB2)
            : base(contextDB2)
        {
            DBContext2 = contextDB2;
        }

        public CarRsp AddCar(Cars cars)
        {
            CarRsp rsp = new CarRsp();
            try
            {

                SqlParameter[] p = new SqlParameter[3];
                p[0] = new SqlParameter("@RegistrationNo", cars.RegistrationNo);
                p[1] = new SqlParameter("@StatusID", cars.StatusID);
                p[2] = new SqlParameter("@CustomerID", cars.CustomerID);
                var check = (new DBHelperPOS().GetDatasetFromSP)("sp_CheckNoPlate", p);

                if (check.Tables[0].Rows.Count == 0)
                {
                    try
                    {
                        var isbase64 = IsBase64String(cars.ImagePath);
                        var chkImagePath = IsBase64Encoded(cars.ImagePath
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

                    var ds = InsertCar(cars);
                    if (ds > 0)
                    {
                        rsp.cars = cars;
                        rsp.Status = 1;
                        rsp.Description = "Car has been added successfully";
                    }
                    else
                    {
                        rsp.cars = cars;
                        rsp.Status = 0;
                        rsp.Description = "Failed to add car";
                    }
                }
                else
                {
                    rsp.cars = cars;
                    rsp.Status = 0;
                    rsp.Description = "Car Already Exist";
                }
            }
            catch (Exception e)
            {
                rsp.cars = cars;
                rsp.Status = 0;
                rsp.Description = e.Message;
            }
            return rsp;
        }
        public bool IsBase64String(string base64)
        {
            base64 = base64.Trim();
            return (base64.Length % 4 == 0) && Regex.IsMatch(base64, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
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
                    else
                        cars.ImagePath = null;
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
        public  bool? IsBase64( string base64String)
        {
            // Credit: oybek https://stackoverflow.com/users/794764/oybek
            if (string.IsNullOrEmpty(base64String) || base64String.Length % 4 != 0
               || base64String.Contains(" ") || base64String.Contains("\t") || base64String.Contains("\r") || base64String.Contains("\n"))
                return false;

            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch (Exception exception)
            {
                // Handle the exception
            }
            return false;
        }
        public int InsertCar(Cars cars)
        {
            try
            {
                SqlParameter[] p = new SqlParameter[14];
                p[0] = new SqlParameter("@RowID", cars.RowID);
                p[1] = new SqlParameter("@CustomerID", cars.CustomerID);
                p[2] = new SqlParameter("@MakeID", cars.MakeID);
                p[3] = new SqlParameter("@Name", cars.Name);
                p[4] = new SqlParameter("@ModelID", cars.ModelID);
                p[5] = new SqlParameter("@Description", cars.Description);
                p[6] = new SqlParameter("@Year", cars.Year);
                p[7] = new SqlParameter("@RegistrationNo", cars.RegistrationNo);
                p[8] = new SqlParameter("@ImagePath", cars.ImagePath);
                p[9] = new SqlParameter("@LocationID", cars.LocationID);
                p[10] = new SqlParameter("@UserID", cars.UserID);
                p[11] = new SqlParameter("@StatusID", cars.StatusID);
                p[12] = new SqlParameter("@CheckLitre", cars.CheckLitre);
                p[13] = new SqlParameter("@EngineType", cars.EngineType);

                cars.CarID = int.Parse((new DBHelperPOS().GetDatasetFromSP)("sp_AddCars", p).Tables[0].Rows[0][0].ToString());
                cars.ImagePath = cars.ImagePath != null ? ConfigurationSettings.AppSettings["ApiURL"].ToString() + cars.ImagePath : null;
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
                SqlParameter[] p = new SqlParameter[15];
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
                p[13] = new SqlParameter("@CheckLitre", cars.CheckLitre);
                p[14] = new SqlParameter("@EngineType", cars.EngineType);
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

        public RspCustomerOrders GetCustomerOrders(int? CarID, int? CustomerID)
        {
            var rsp = new RspCustomerOrders();
            try
            {
                var ds = GetCustomerOrders_ADO(CarID, CustomerID);
                var _dsOrders = ds.Tables[0] == null ? new List<OrdersList>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0])).ToObject<List<OrdersList>>();
                var _dsOrderdetail = ds.Tables[1] == null ? new List<OItemsList>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[1])).ToObject<List<OItemsList>>();
                var _dsOrderdetailPkg = ds.Tables[2] == null ? new List<OPackageDetailList>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[2])).ToObject<List<OPackageDetailList>>();
                var _dsordercheckoutdetail = ds.Tables[3] == null ? new List<CheckoutDetailsOrder>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[3])).ToObject<List<CheckoutDetailsOrder>>();
                foreach (var j in _dsOrders)
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
                    j.CheckoutDetails = checkoutDetails;
                }

                rsp.Orders = _dsOrders;
                rsp.Status = 1;
                rsp.Description = "Success";
            }
            catch (Exception ex)
            {
                rsp.Orders = new List<OrdersList>();
                rsp.Status = 0;
                rsp.Description = "Failed";
            }
            return rsp;
        }
        public DataSet GetCustomerOrders_ADO(int? CarID, int? CustomerID)
        {
            try
            {
                SqlParameter[] p = new SqlParameter[2];
                p[0] = new SqlParameter("@CustomerID", CustomerID);
                p[1] = new SqlParameter("@CarID", CarID);
                return (new DBHelperPOS().GetDatasetFromSP)("sp_CustomerOrders_CAPI", p);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public OrderLetterResponse OrderPrintLetter(string OrderID)
        {
            var rsp = new OrderLetterResponse();

            try
            {

                var ds = GetOrderDetailADO(int.Parse(OrderID), "order", 0);
                var _dsOrders = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0])).ToObject<List<OrdersList>>();
                var _dsorderdetail = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[1])).ToObject<List<OItemsList>>();
                var _dsordercheckoutdetail = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[2])).ToObject<List<CheckoutDetailsOrder>>();
                var _dsorderpkgdetail = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[3])).ToObject<List<OPackageDetailList>>();
                var _dsCarNotes = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[6])).ToObject<List<CarNotes>>().FirstOrDefault();
                var _dsCarNotesImages = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[7])).ToObject<List<CarNotesImagesList>>();
                var _dsChecklist = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[8])).ToObject<List<OrdersChecklist>>();
                var _dsCar = ds.Tables[0] == null ? new List<Cars>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0])).ToObject<List<Cars>>();
                var _dsCompany = ds.Tables[5] == null ? null : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[5])).ToObject<List<User>>().FirstOrDefault();
                var _dtReceipt = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(GetReceiptADO(_dsOrders.FirstOrDefault().LocationID).Tables[0])).ToObject<List<ReceiptBLL>>();
                var _dtCreditCustomer = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[9])).ToObject<List<CreditCustomerA4>>().FirstOrDefault();

                var carinfo = _dsCar.FirstOrDefault();
                var ordercheckout = _dsOrders.FirstOrDefault();
                ordercheckout.CreditCustomerInfo = _dtCreditCustomer;
                var companyinfo = _dsCompany;

                if (ds != null)
                {
                    string checklist = "";
                    string orderdetails = "";
                    string content = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/Template") + "\\" + "CustomerLetterV3.txt");

                    #region CarNotes
                    var carImagesHtml = "";
                    var CarNotes = _dsCarNotes;
                    if (CarNotes != null)
                    {
                        var CarImages = _dsCarNotesImages.ToList();
                        if (CarImages.Count > 0)
                        {
                            carImagesHtml += "<h4>صور السيارة</h4>";
                            foreach (var item in CarImages)
                            {
                                item.ImagePath = item.ImagePath == null ? "" : ConfigurationSettings.AppSettings["ApiURL"].ToString() + item.ImagePath;
                                carImagesHtml += "<img src='" + item.ImagePath + "' style='margin:2px;'  width='150px' />";
                            }
                            carImagesHtml += "<h5>" + CarNotes.NotesComment + "</h4>";
                        }
                    }
                    var signatureNotes = CarNotes != null ? (CarNotes.Signature != null ? ConfigurationSettings.AppSettings["ApiURL"].ToString() + CarNotes.Signature : "") : "";

                    #endregion CarNotes

                    #region checklist

                    var datatChecklist = _dsChecklist.Where(x => x.InspectionDetailID != 0).ToList();
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
                    if (_dsorderdetail.Count > 0)
                    {
                        foreach (var item in _dsorderdetail.Where(x => x.StatusID != 203))
                        {
                            //for letter refund bar
                            item.RefundQty = item.RefundQty ?? 0;
                            isReturnBar = isReturnBar == true ? true : item.StatusID == 204 ? true : Double.Parse(item.RefundQty.ToString()) > 0 ? true : false;

                            orderdetails += "<tr>";
                            var returnQty = isReturnBar == true ? "<br/><strong>(" + item.RefundQty + " - Return/مسترجع)</strong></h4>" : "";
                            if (item.ItemID != 0 && item.ItemID != null)
                            {
                                orderdetails += " <td style='padding:7px;'>" +
                                    "<h4><strong>" + item.ItemName + "</strong></h4>" +
                                    "<h4><strong>" + item.AlternateName + "</strong></h4>" +
                                    //"<h5>" + item.Description + "</h5>" +
                                    returnQty +
                                    "</td>";
                            }
                            else if (item.PackageID != 0 && item.PackageID != null)
                            {
                                orderdetails += " <td style='padding:7px; 7px; 3px; 7px;'> " +
                                    "<h4><strong>" + item.ItemName + "</strong></h4>" +
                                    "<h4><strong>" + item.AlternateName + "</strong></h4>" +
                                    returnQty +
                                    "<br/>";
                                foreach (var odp in _dsorderpkgdetail.Where(x => x.StatusID == 1 && x.OrderDetailID == item.OrderDetailID))
                                {
                                    orderdetails += "<h5>--->   " + odp.Quantity + "X " + odp.ItemName + " || " + odp.AlternateName + "</h5>";
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

                    #region logo
                    var logoHtml = "<div style='padding: 25px 0 40px 0; '><img src=" + ConfigurationSettings.AppSettings["AdminURL"].ToString() + companyinfo.ImagePath + " height='100px' /></div>";
                    #endregion logo

                    #region socialmedia
                    var receipt = _dtReceipt.FirstOrDefault();
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


                    var payment = "";
                    if (_dsordercheckoutdetail != null)
                    {
                        var Payby = "Payment Type(نوع الدفع) | ";
                        switch (ordercheckout.PaymentMode)
                        {
                            case 3:
                                Payby += "Multi Payment(دفع متعدد)";
                                break;
                            case 4:
                                Payby += "CreditCustomer (عمبل آجل)";
                                break;
                            default:
                                Payby = "";
                                break;
                        }
                        if (Payby != "")
                        {
                            payment += "  <div class='row' style='background-color:#eaeaea; padding:0 20px;'>";
                            payment += "     <div class='col-12 text-right' style='padding:10px 0;'>          ";
                            payment += "         <h5>" + Payby + "</h5>                          ";
                            payment += "     </div>                                                          ";
                            payment += " </div>  ";
                        }
                        var PMLabel = "";


                        foreach (var item in _dsordercheckoutdetail)
                        {
                            switch (item.PaymentMode)
                            {
                                case 1:
                                    PMLabel = "Cash نقدا";
                                    break;
                                case 2:
                                    PMLabel = "Card بطاقة";
                                    break;
                                case 3:
                                    PMLabel = "Credit";
                                    break;
                                case 5:
                                    PMLabel = "Tabby" + (ordercheckout.Remarks != null && ordercheckout.Remarks != "" ? "(" + ordercheckout.Remarks + ")" : "");
                                    break;
                                case 6:
                                    PMLabel = "Tamara" + (ordercheckout.Remarks != null && ordercheckout.Remarks != "" ? "(" + ordercheckout.Remarks + ")" : "");
                                    break;
                                case 7:
                                    PMLabel = "StcPay" + (ordercheckout.Remarks != null && ordercheckout.Remarks != "" ? "(" + ordercheckout.Remarks + ")" : "");
                                    break;
                                case 8:
                                    PMLabel = "Bank Transfer" + (ordercheckout.Remarks != null && ordercheckout.Remarks != "" ? "(" + ordercheckout.Remarks + ")" : "");
                                    break;
                                default:
                                    PMLabel = "";
                                    break;
                            }
                            payment += " <div class='row' style=' padding:0 20px;'>                      ";
                            payment += "                                                                 ";
                            payment += "     <div class='col-4' style='padding:10px 0;'>                 ";
                            payment += "         <h4> <strong>" + item.AmountPaid + "</strong></h4>                   ";
                            payment += "     </div>                                                      ";
                            payment += "     <div class='col-8 text-right' style='padding:10px 0;'>      ";
                            payment += "         <h4>" + PMLabel + "</h4>                                ";
                            payment += "     </div>                                                      ";
                            payment += " </div>";
                        }
                    }
                    var qrlink = "";
                    try
                    {
                        qrlink = new Zatca().ZatcaInvoiceQR(companyinfo.Company, companyinfo.VATNO, ordercheckout.Tax.ToString(), Convert.ToDateTime(ordercheckout.CheckoutDate).ToString("yyyy-MM-dd HH:mm:ss"), ordercheckout.GrandTotal.ToString());
                    }
                    catch { qrlink = ""; }

                    #region HeaderText
                    isReturnBar = isReturnBar == true ? true : ordercheckout.Status == 106 ? true : false;
                    var refundAmountHTML = "";

                    if (isReturnBar)
                    {

                        refundAmountHTML += "  <div class='row' style='background-color:#eaeaea; padding:0 20px;'>";
                        refundAmountHTML += "     <div class='col-4' style='padding:10px 0;'>                     ";
                        refundAmountHTML += "         <h4> <strong>" + ordercheckout.RefundedAmount + " </strong></h4>                    ";
                        refundAmountHTML += "     </div>                                                          ";
                        refundAmountHTML += "     <div class='col-8 text-right' style='padding:10px 0;'>          ";
                        refundAmountHTML += "         <h4>(Refund Amount) القيمة المسترجعة</h4>                          ";
                        refundAmountHTML += "     </div>                                                          ";
                        refundAmountHTML += " </div>                                                              ";
                        content = content.Replace("#refundbar#", "  <div class='row' style='background:#e8acac;margin-bottom:5px;'><div class='col-12 text-center'><h2 style='padding:10px 0 10px 0;font-size:26px'>اشعار دائن للفاتورة الضريبية المبسطة</h2></div></div>");
                        if (ordercheckout.CreditCustomerID > 0 && ordercheckout.CreditCustomerInfo != null)
                        {
                            if (ordercheckout.CreditCustomerInfo.BuyerVAT != null && ordercheckout.CreditCustomerInfo.BuyerVAT != "")
                            {
                                content = content
                                        .Replace("#buyername#", ordercheckout.CreditCustomerInfo.BuyerName)
                                        .Replace("#buyeraddress#", ordercheckout.CreditCustomerInfo.BuyerAddress)
                                        .Replace("#buyercontact#", ordercheckout.CreditCustomerInfo.BuyerContact)
                                        .Replace("#buyervatno#", ordercheckout.CreditCustomerInfo.BuyerVAT)
                                        .Replace("#sellername#", ordercheckout.CreditCustomerInfo.SellerName)
                                        .Replace("#selleraddress#", ordercheckout.CreditCustomerInfo.SellerAddress)
                                        .Replace("#sellercontact#", ordercheckout.CreditCustomerInfo.SellerContact)
                                        .Replace("#sellervatno#", ordercheckout.CreditCustomerInfo.SellerVAT);

                                content = content.Replace("#showcredcustsection#", "block");
                            }
                            else
                                content = content.Replace("#showcredcustsection#", "none");
                        }
                        else
                            content = content.Replace("#showcredcustsection#", "none");
                    }
                    else
                    {
                        if (ordercheckout.CreditCustomerID > 0 && ordercheckout.CreditCustomerInfo != null)
                        {
                            if (ordercheckout.CreditCustomerInfo.BuyerVAT != null && ordercheckout.CreditCustomerInfo.BuyerVAT != "")
                            {
                                content = content.Replace("#refundbar#", "<div class='row' style='background:#eee;margin-bottom:5px;'>" +
                                           "<div class='col-12 text-center'><h2 style='padding:10px 0 10px 0;font-size:26px'>فاتورة ضريبية</h2>" +
                                           "</div></div>");

                                content = content
                                        .Replace("#buyername#", ordercheckout.CreditCustomerInfo.BuyerName)
                                        .Replace("#buyeraddress#", ordercheckout.CreditCustomerInfo.BuyerAddress)
                                        .Replace("#buyercontact#", ordercheckout.CreditCustomerInfo.BuyerContact)
                                        .Replace("#buyervatno#", ordercheckout.CreditCustomerInfo.BuyerVAT)
                                        .Replace("#sellername#", ordercheckout.CreditCustomerInfo.SellerName)
                                        .Replace("#selleraddress#", ordercheckout.CreditCustomerInfo.SellerAddress)
                                        .Replace("#sellercontact#", ordercheckout.CreditCustomerInfo.SellerContact)
                                        .Replace("#sellervatno#", ordercheckout.CreditCustomerInfo.SellerVAT);

                                content = content.Replace("#showcredcustsection#", "block");
                            }
                            else
                            {
                                content = content.Replace("#showcredcustsection#", "none");
                                content = content.Replace("#refundbar#", "<div class='row' style='background:#eee;margin-bottom:5px;'>" +
                                                  "<div class='col-12 text-center'><h2 style='padding:10px 0 10px 0;font-size:26px'>فاتورة ضريبية المبسطة</h2>" +
                                                  "</div></div>");
                            }
                        }
                        else
                        {
                            content = content.Replace("#showcredcustsection#", "none");
                            content = content.Replace("#refundbar#", "<div class='row' style='background:#eee;margin-bottom:5px;'>" +
                                              "<div class='col-12 text-center'><h2 style='padding:10px 0 10px 0;font-size:26px'>فاتورة ضريبية المبسطة</h2>" +
                                              "</div></div>");
                        }
                    }
                    #endregion HeaderText
                    var cname = "-";
                    try
                    {
                        cname = ordercheckout.CustomerName != null ? ordercheckout.CustomerName : "";
                        cname += ordercheckout.CreditCustomerID > 0 ? " | " + ordercheckout.CreditCustomerInfo.BuyerName : "";
                    }
                    catch { cname = "-"; }

                    content = content.Replace("#logo#", logoHtml);
                    content = content.Replace("#qrlink#", qrlink);
                    content = content.Replace("#notes#", CarNotes != null ? CarNotes.NotesComment : "");
                    content = content.Replace("#signature#", CarNotes != null ? signatureNotes : "");
                    content = content.Replace("#carmake#", ordercheckout.MakerName);
                    content = content.Replace("#carmodel#", ordercheckout.ModelName);
                    content = content.Replace("#year#", carinfo.Year.ToString());
                    content = content.Replace("#locationname#", receipt.CompanyTitle);
                    content = content.Replace("#customername#", cname);
                    content = content.Replace("#locationaddress#", receipt.CompanyAddress);
                    content = content.Replace("#locationcontact#", receipt.CompanyPhones);
                    content = content.Replace("#mobile#", ordercheckout.CustomerContact);
                    content = content.Replace("#checkliters#", carinfo.CheckLitre ?? "0");
                    content = content.Replace("#noplate#", carinfo.RegistrationNo);
                    content = content.Replace("#vinno#", carinfo.VinNo);
                    content = content.Replace("#transactionno#", ordercheckout.TransactionNo.ToString());
                    content = content.Replace("#orderdate#", Convert.ToDateTime(ordercheckout.OrderPunchDate).ToString("dd/MM/yyyy"));
                    content = content.Replace("#worker#", ordercheckout.MechanicName);
                    content = content.Replace("#total#", ordercheckout.AmountTotal.ToString());
                    content = content.Replace("#tax#", ordercheckout.Tax.ToString());
                    content = content.Replace("#discount#", Math.Round(ordercheckout.AmountDiscount ?? 0, 2).ToString());
                    content = content.Replace("#subtotal#", ordercheckout.GrandTotal.ToString() + " " + ordercheckout.Currency.ToString());
                    content = content.Replace("#refundtotal#", refundAmountHTML);
                    content = content.Replace("#vatno#", companyinfo == null ? "" : companyinfo.VATNO);
                    content = content.Replace("#carimages#", carImagesHtml);
                    content = content.Replace("#footernotes#", receipt.Footer);
                    content = content.Replace("#showchecklist#", datatChecklist.Count > 0 ? "block" : "none");
                    content = content.Replace("#partialreceived#", ordercheckout.PartialAmountReceived.ToString());
                    content = content.Replace("#ispartialreceived#", ordercheckout.PartialAmountReceived > 0 ? "block" : "none");
                    var path = ConfigurationSettings.AppSettings["ApiURL"].ToString() + GetPdf(content, int.Parse(OrderID));
                    rsp.Path = path.Replace("~", "");
                    rsp.OrderID = int.Parse(OrderID);
                    rsp.Status = (int)eStatus.Success;
                    rsp.Description = "Success";
                }
            }
            catch (Exception ex)
            {
                rsp.OrderID = int.Parse(OrderID);
                rsp.Status = (int)eStatus.Exception;
                rsp.Description = ex.Message;
            }
            return rsp;
        }

        public DataSet GetOrderDetailADO(int id, string type, int LocationID)
        {
            try
            {
                SqlParameter[] p = new SqlParameter[3];
                p[0] = new SqlParameter("@ID", id);//or transactionno
                p[1] = new SqlParameter("@LocationID", LocationID);
                p[2] = new SqlParameter("@Type", type);
                return (new DBHelperPOS().GetDatasetFromSP)("sp_GetOrderDetailv2_APP", p);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetReceiptADO(int? LocationID)
        {
            try
            {
                SqlParameter[] p = new SqlParameter[1];
                p[0] = new SqlParameter("@LocationID", LocationID);
                return (new DBHelperPOS().GetDatasetFromSP)("sp_GetReceiptInfo_APP", p);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private string GetPdf(string html, int orderid)
        {

            var htmlContent = html.ToString();
            var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
            var pdfBytes = htmlToPdf.GeneratePdf(htmlContent);
            var filename = orderid + "-" + DateTime.Now.Ticks.ToString();
            string folderPath = "~/Upload/OrderLetters/";   // set folder

            if (!Directory.Exists(HttpContext.Current.Server.MapPath(folderPath)))
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(folderPath));
            string FilePath = HttpContext.Current.Server.MapPath(folderPath + filename + ".pdf");
            var bw = new BinaryWriter(System.IO.File.Open(FilePath, FileMode.OpenOrCreate));
            bw.Write(pdfBytes);
            bw.Close();



            return folderPath + filename + ".pdf";
        }

        public CustomerCarsResponse GetCustomerCars(int? CustomerID)
        {
            var rsp = new CustomerCarsResponse();
            try
            {
                var ds = GetCustomerCarsADO(CustomerID);
                var _dsCarInfo = ds.Tables[0] == null ? new List<Cars>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0])).ToObject<List<Cars>>();

                rsp.CarList = rsp.CarList ?? new List<Cars>();
                foreach (var i in _dsCarInfo)
                {
                    if (rsp.CarList.Where(x => x.RegistrationNo == i.RegistrationNo).Count() == 0)
                        rsp.CarList.Add(i);
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
        public DataSet GetCustomerCarsADO(int? CustomerID)
        {
            try
            {
                SqlParameter[] p = new SqlParameter[1];
                p[0] = new SqlParameter("@CustomerID", CustomerID);
                return (new DBHelperPOS().GetDatasetFromSP)("sp_CustomerCars_CAPI", p);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public CustomerCarsResponse GetCarOrder(int? carid, int? orderid)
        {
            var rsp = new CustomerCarsResponse();
            try
            {
                var ds = GetCarOrder_ADO(carid, orderid);
                var _dsCarInfo = ds.Tables[0] == null ? new List<Cars>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0])).ToObject<List<Cars>>();
                var _dsOrders = ds.Tables[1] == null ? new List<OrdersList>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[1])).ToObject<List<OrdersList>>();
                var _dsOrderdetail = ds.Tables[2] == null ? new List<OItemsList>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[2])).ToObject<List<OItemsList>>();
                var _dsOrderdetailPkg = ds.Tables[3] == null ? new List<OPackageDetailList>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[3])).ToObject<List<OPackageDetailList>>();
                var _dsordercheckoutdetail = ds.Tables[4] == null ? new List<CheckoutDetailsOrder>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[4])).ToObject<List<CheckoutDetailsOrder>>();
                rsp.CarList = _dsCarInfo;
                foreach (var i in rsp.CarList)
                {
                    i.MakerImage = i.MakerImage == null ? null : ConfigurationSettings.AppSettings["CpAdminURL"].ToString() + i.MakerImage;
                    i.ImagePath = i.ImagePath == null ? null : ConfigurationSettings.AppSettings["ApiURL"].ToString() + i.ImagePath;
                    try { i.RegistrationNoP1 = i.RegistrationNo.Split('-')[0]; } catch { i.RegistrationNoP1 = ""; }
                    try { i.RegistrationNoP2 = i.RegistrationNo.Split('-')[1]; } catch { i.RegistrationNoP2 = ""; }
                    try { i.RegistrationNoP3 = TranslateToArabic(i.RegistrationNoP1, 1); } catch { i.RegistrationNoP3 = ""; }
                    try { i.RegistrationNoP4 = TranslateToArabic(i.RegistrationNoP2, 2); } catch { i.RegistrationNoP4 = ""; }
                    i.Orders = _dsOrders.Where(x => x.CarID == i.CarID).ToList();
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
                        j.CheckoutDetails = checkoutDetails;
                    }
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

        public RecentOrdersResponse GetRecentOrders(int? customerid)
        {
            var rsp = new RecentOrdersResponse();
            try
            {
                var ds = GetRecentOrders_ADO(customerid);
                var _dsRecentOrders = ds.Tables[0] == null ? new List<OrdersList>() : JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0])).ToObject<List<OrdersList>>();

                foreach (var j in _dsRecentOrders)
                {
                    j.CompanyImage = j.CompanyImage == null ? null : ConfigurationSettings.AppSettings["AdminURL"].ToString() + j.CompanyImage;

                }
                rsp.RecentOrders = _dsRecentOrders;
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
        public DataSet GetCarOrder_ADO(int? CarID, int? OrderID)
        {
            try
            {
                SqlParameter[] p = new SqlParameter[2];
                p[0] = new SqlParameter("@CarID", CarID);
                p[1] = new SqlParameter("@OrderID", OrderID);
                return (new DBHelperPOS().GetDatasetFromSP)("sp_GetCarOrder_CAPI", p);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetRecentOrders_ADO(int? CustomerID)
        {
            try
            {
                SqlParameter[] p = new SqlParameter[1];
                p[0] = new SqlParameter("@CustomerID", CustomerID);
                return (new DBHelperPOS().GetDatasetFromSP)("sp_GetRecentOrders_CAPI", p);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

}
