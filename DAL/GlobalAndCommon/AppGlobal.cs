using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.GlobalAndCommon
{
	public static class AppGlobal
	{
		public static readonly string connectionStringUAT = "data source=85.25.214.10;initial catalog=Garage_UAT;persist security info=True;user id=garage;password=garage;";
		public static readonly string connectionStringLive = "data source=85.25.214.10;initial catalog=GarageCustomer_Live;persist security info=True;user id=garage;password=garage;";
	}
}
