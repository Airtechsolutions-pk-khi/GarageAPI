using DAL.DBEntities2;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
        
}
