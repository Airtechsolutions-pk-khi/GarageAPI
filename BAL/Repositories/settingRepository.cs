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
                        Tittle = i.Tittle,
                        Description = i.Description,
                        Image = i.Image,
                        PageName = i.PageName,
                        //Type = i.Type,
                        DisplayOrder = i.DisplayOrder,
                        //URL = i.URL
                    }) ;
                }
                rsp.settings = lst;
                rsp.status = 1;
                rsp.description = "Success";

                return rsp;
            }
            catch (Exception ex)
            {
                rsp.settings = lst;
                rsp.status = 0;
                rsp.description = "Failed";
                return rsp;
            }
        }
        
    }
}
