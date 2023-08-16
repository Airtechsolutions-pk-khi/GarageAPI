using BAL.Repositories;
using DAL.DBEntities2;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace GarageCustomerAPI.Controllers
{
    [RoutePrefix("api")]
    public class SettingController : ApiController
    {
        /// <summary>
        /// Settings
        /// </summary>
        settingRepository settingRepo;
        /// <summary>
        /// Settings
        /// </summary>
        public SettingController()
        {
            settingRepo = new settingRepository(new GarageCustomer_Entities());
        }

        /// <summary>
        /// - List Of Amenities
        /// - List Of Services
        /// - List Of Landmarks
        /// </summary>
        /// <returns></returns>
        [Route("setting")]
        public Settingv2Rsp GetAllSettings()
        {
            return settingRepo.GetSettingsV3();
        }

        //old with all data
        /// <summary>
        /// - List Of Locations
        /// - List Of Amenities
        /// - List Of Services
        /// - List Of Landmarks
        /// </summary>
        /// <returns></returns>
        [Route("setting/all")]
        public SettingRsp GetAll()
        {
            return settingRepo.GetSettings(0);
        }

        //old with location list
        /// <summary>
        /// - List Of Locations
        /// - List Of Amenities
        /// - List Of Services
        /// - List Of Landmarks
        /// </summary>
        /// <returns></returns>
        [Route("setting/v2/all")]
        public SettingRsp GetAllSettingsv2()
        {
            return settingRepo.GetSettingsV2(0);
        }

        /// <summary>
        /// Get Locations by service
        /// - List Of Locations
        /// - List Of Amenities
        /// - List Of Services
        /// - List Of Landmarks
        /// </summary>
        /// <returns></returns>
        [Route("locations/{ServiceID}")]
        public SettingRsp GetLocation(int ServiceID)
        {
            return settingRepo.GetLocation(ServiceID);
        }

        /// <summary>
        /// List of CarMake
        /// </summary>
        /// <returns></returns>
        [Route("setting/carmake")]
        public RspCarMake GetCarmake()
        {
            return settingRepo.GetCarMake();
        }

        /// <summary>
        /// Test Push Notification API
        /// </summary>
        [HttpGet]
        [Route("push/android")]
        public void PushNotification(string deviceid)
        {
            try
            {
                var applicationID = "AAAAxl51rZs:APA91bHiGrGKga3ZYLPrzQmzYClRynk3448-mKPg-3IL8q6RJ3fE35OeV4yM2ohv6wjbsfe6LyolpwMD4kq1sp_jc7Swybi0f7jRshFdJj_5-DItwg9zGRpXK1JImoStU3mAO25CXZNG";
                var senderId = "851988295067";
                string deviceId = deviceid;// "cwb3TAzRgUVRoWGFrw4ubR:APA91bER9bKB-cO32ab792isUsiJO-xdc21ltPxonb9BP_TH00Fa6dOUWi74X7ikst97L60gtE5WuBxT7YKHUTnBlhuzh8IY8JCbGkxT2nfnUx4j7UHkqkWNaYRnX_s5jEYACehZt74G";
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var data = new
                {
                    to = deviceId,
                    notification = new
                    {
                        body = "test",
                        title = "teest",
                        icon = "myicon",
                        sound = "default"

                    }
                };
                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(data);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                string str = sResponseFromServer;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
        }

        /// <summary>
        /// FeedBack Add
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("setting/feedback/add")]
        public Rsp PostFeedback(Feedback obj)
        {
            return settingRepo.AddFeedback(obj);
        }

        /// <summary>
        /// Add Review to Locations
        /// </summary>
        /// <param name="obj">(Status 1 for Active,3 for Delete)</param>
        /// <returns></returns>
        [Route("review/customer")]
        public ReviewRsp PostReview(ReviewsBLL obj)
        {
            return settingRepo.AddReview(obj);
        }

        /// <summary>
        /// Update Review to Locations
        /// </summary>
        /// <param name="obj">- Status 1 for Active,3 for Delete \n Add Like/Dislike Count to existing value</param>
        /// <returns></returns>
        [Route("review/customer/update")]
        public ReviewRsp PostReviewUpdate(ReviewsBLL obj)
        {
            return settingRepo.UpdateReview(obj);
        }

        /// <summary>
        /// Report Review
        /// </summary>
        /// <param name="obj">- Status 1 for Active,3 for Delete \n - LikeValue\DisLikeValue (0 for no action, 1 new action and -1 for update action) on like dislike action </param>
        /// <returns></returns>
        [Route("review/customer/action")]
        public Rsp PostReportReview(ReportReviewsBLL obj)
        {
            return settingRepo.AddReportReview(obj);
        }


        /// <summary>
        /// - List of AI BOT
        /// </summary>
        /// <returns></returns>
        [Route("ai/chat/all/{carID}/{customerID}/{chatID}")]
        public AIChatModelRsp GetAllListAIBOT(int carID, int customerID, int chatID)
        {
            return settingRepo.GetListAIBOT(carID, customerID, chatID);
        }

        /// <summary>
        /// - Add AI CHAT
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ai/chat/add")]
        public Rsp PostAIChat(AIChat obj)
        {
            return settingRepo.AddAIChat(obj);
        }

        [HttpPost]
        [Route("pushnotication")]
        public Rsp PushNotification(PushNoticationBLL obj)
        {
            settingRepo.PushNotificationAndroid(obj);
            return new Rsp { 
            Status=1
            };
        }
    }
}
