﻿using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WebAPICode.Helpers;

namespace BAL.Repositories
{
	public class PaginationRepository
	{
		private readonly DBHelper _dbhelper;
		private readonly DBHelperPOS _dbhelperPOS;

		public PaginationRepository(DBHelper dbhelper, DBHelperPOS dbhelperPOS)
		{
			_dbhelper = dbhelper;
			_dbhelperPOS = dbhelperPOS;
		}

		public async Task<DataSet> GetPaginationData<U>(int pageNumber, int pageSize, string SP, U parametres)
		{
			try
			{
				SqlParameter[] p = new SqlParameter[3];
				p[0] = new SqlParameter("@PageNumber", pageNumber);
				p[1] = new SqlParameter("@PageSize", pageSize);
				p[2] = new SqlParameter("@ParamTable1", JsonConvert.SerializeObject(parametres));
				return await _dbhelper.GetDatasetFromSPAsync(SP, p);
			}
			catch (Exception ex)
			{
				return null;
			}
		}
		public async Task<DataSet> GetPaginationDataPos<U>(int pageNumber, int pageSize, string SP, U parametres)
		{
			try
			{
				SqlParameter[] p = new SqlParameter[4];
				p[0] = new SqlParameter("@PageNumber", pageNumber);
				p[1] = new SqlParameter("@PageSize", pageSize);
				p[2] = new SqlParameter("@ParamTable1", JsonConvert.SerializeObject(parametres));
				p[3] = new SqlParameter("@Date", DateTime.UtcNow.AddMinutes(180).Date);
				return await _dbhelperPOS.GetDatasetFromSPAsync(SP, p);
			}
			catch (Exception ex)
			{
				return null;
			}
		}
	}
}