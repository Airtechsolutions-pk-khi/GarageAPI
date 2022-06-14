using DAL.DBEntities;
using DAL.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace BAL.Repositories
{

    public class settingRepository : BaseRepository
    {
        public settingRepository()
            : base()
        {
            DBContext = new GarageCustomer_UATEntities();

        }
        public settingRepository(GarageCustomer_UATEntities contextDB)
            : base(contextDB)
        {
            DBContext = contextDB;
        }
        public RspSetting GetSettings()
        {
            var serviceLst = new List<ServiceBLL>();

            var lst = new List<SettingBLL>();
            var rsp = new RspSetting();
            try
            {
                var list = DBContext.Settings.ToList();

                foreach (var i in list)
                {
                    lst.Add(new SettingBLL
                    {
                        ID = i.ID,
                        Title = i.Tittle,
                        Description = i.Description,
                        Image = i.Image,
                        PageName = i.PageName,
                        Type = i.Type,
                        DisplayOrder = i.DisplayOrder,
                    });
                }
                var serviceList = DBContext.sp_GetServices().ToList();
                foreach (var item in serviceList)
                {
                    serviceLst.Add(new ServiceBLL
                    {
                        ServiceID = item.ServiceID,
                        ServiceTitle = item.ServiceTitle,
                        ServiceDescription = item.ServiceDescription,
                        Image = item.Image,
                        DisplayOrder = item.DisplayOrder,
                        StatusId = item.StatusId,
                    });
                }
                rsp.Services = serviceLst;
                rsp.Settings = lst;
                rsp.status = 1;
                rsp.description = "Success";

                return rsp;
            }
            catch (Exception ex)
            {
                rsp.Settings = lst;
                rsp.status = 0;
                rsp.description = "Failed";
                return rsp;
            }
        }
       
    }
}
