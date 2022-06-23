
using DAL.DBEntities2;
using DAL.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPICode.Helpers;

namespace BAL.Repositories
{
    public class carRepository : BaseRepository2
    {
        public Cars cars;
        public carRepository()
            : base()
        {
            DBContext2 = new Garage_UATEntities2();

        }
        public carRepository(Garage_UATEntities2 contextDB2)
            : base(contextDB2)
        {
            DBContext2 = contextDB2;
        }

        public CarRsp AddCar(Cars cars)
        {
            CarRsp rsp = new CarRsp();

            SqlParameter[] p = new SqlParameter[3];
            p[0] = new SqlParameter("@RegistrationNo", cars.RegistrationNo);
            p[1] = new SqlParameter("@StatusID", cars.StatusID);
            p[2] = new SqlParameter("@CustomerID", cars.CustomerID);
            var check =  (new DBHelperPOS().GetDatasetFromSP)("sp_CheckNoPlate", p);

            if (check.Tables[0].Rows.Count == 0 )
            {
                var ds = InsertCar(cars);
                rsp.cars = cars;
                rsp.status = 200;
                rsp.description = "Car has been added successfully";
            }
            else
            {
                rsp.cars = cars;
                 rsp.status = 409;
                rsp.description = "Car Already Exist";
            }


            return rsp;
        }
        public CarRsp EditCar(Cars cars)
        {
            CarRsp rsp = new CarRsp();
            try
            {              
                var ds = ap_EditCar(cars);
                rsp.cars = cars;
                rsp.status = 200;
                rsp.description = "Car has been Updated successfully";
            }
            catch (Exception ex)
            {
                rsp.cars = cars;
                rsp.status = 500;
                rsp.description = "Car can not Updated successfully";
            }          
            return rsp;
        }

        public int InsertCar(Cars cars)
        {
            try
            {
                SqlParameter[] p = new SqlParameter[12];
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

                return (new DBHelperPOS().ExecuteNonQueryReturn)("sp_AddCars", p);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int ap_EditCar(Cars cars)
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

                return (new DBHelperPOS().ExecuteNonQueryReturn)("sp_UpdateCars", p);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

    }
        
}
