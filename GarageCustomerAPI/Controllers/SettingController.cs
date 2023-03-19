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

        /// <summary>
        /// - List Of Locations
        /// - List Of Amenities
        /// - List Of Services
        /// - List Of Landmarks
        /// </summary>
        /// <returns></returns>
        [Route("setting/all/{LocationID}")]
        public SettingRsp GetLocation(int LocationID)
        {
            return settingRepo.GetSettings(LocationID);
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
        public void PushNotification()
        {
            try
            {
                var applicationID = "AAAAxl51rZs:APA91bHiGrGKga3ZYLPrzQmzYClRynk3448-mKPg-3IL8q6RJ3fE35OeV4yM2ohv6wjbsfe6LyolpwMD4kq1sp_jc7Swybi0f7jRshFdJj_5-DItwg9zGRpXK1JImoStU3mAO25CXZNG";
                var senderId = "851988295067";
                string deviceId = "cwb3TAzRgUVRoWGFrw4ubR:APA91bER9bKB-cO32ab792isUsiJO-xdc21ltPxonb9BP_TH00Fa6dOUWi74X7ikst97L60gtE5WuBxT7YKHUTnBlhuzh8IY8JCbGkxT2nfnUx4j7UHkqkWNaYRnX_s5jEYACehZt74G";
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
    }
}
