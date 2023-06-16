using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace WebAPICode.Helpers
{
	public class DBHelperPOS
	{
		private static string _connectionString;
		//private static readonly string connectionString = "data source=85.25.214.10;initial catalog=GarageCustomer_Live;persist security info=True;user id=garage;password=garage;";

		public DBHelperPOS(string connectionString)
		{
			_connectionString = connectionString;
		}

		public DataTable GetTableFromSP(string sp, Dictionary<string, object> parametersCollection)
		{

			SqlConnection connection = new SqlConnection(_connectionString);
			try
			{
				SqlCommand command = new SqlCommand(sp, connection) { CommandType = CommandType.StoredProcedure, CommandTimeout = connection.ConnectionTimeout };

				foreach (KeyValuePair<string, object> parameter in parametersCollection)
					command.Parameters.AddWithValue(parameter.Key, parameter.Value);

				DataSet dataSet = new DataSet();
				(new SqlDataAdapter(command)).Fill(dataSet);
				command.Parameters.Clear();

				if (dataSet.Tables.Count > 0)
				{
					return dataSet.Tables[0];
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{
				throw ex;
				//return null;
			}
			finally
			{
				connection.Close();

			}
		}

		public async Task<DataTable> GetTableFromSPAsync(string sp, Dictionary<string, object> parametersCollection)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				try
				{
					SqlCommand command = new SqlCommand(sp, connection)
					{
						CommandType = CommandType.StoredProcedure,
						CommandTimeout = connection.ConnectionTimeout
					};

					foreach (KeyValuePair<string, object> parameter in parametersCollection)
						command.Parameters.AddWithValue(parameter.Key, parameter.Value);
					await connection.OpenAsync();

					DataSet dataSet = new DataSet();
					SqlDataAdapter adapter = new SqlDataAdapter(command);

					await Task.Run(() => adapter.Fill(dataSet));

					command.Parameters.Clear();

					if (dataSet.Tables.Count > 0)
					{
						return dataSet.Tables[0];
					}
					else
					{
						return null;
					}
				}
				catch (Exception ex)
				{
					throw ex;
					//return null;
				}
				finally
				{
					connection.Close();
				}
			}
		}


		public DataTable GetTableFromSP(string sp, SqlParameter[] prms)
		{
			SqlConnection connection = new SqlConnection(_connectionString);
			try
			{
				SqlCommand command = new SqlCommand(sp, connection) { CommandType = CommandType.StoredProcedure, CommandTimeout = connection.ConnectionTimeout };
				connection.Open();

				command.Parameters.AddRange(prms);

				DataSet dataSet = new DataSet();
				(new SqlDataAdapter(command)).Fill(dataSet);
				command.Parameters.Clear();

				if (dataSet.Tables.Count > 0)
				{
					return dataSet.Tables[0];
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{
				throw ex;
				//return null;
			}
			finally
			{
				connection.Close();
			}
		}

		public async Task<DataTable> GetTableFromSPAsync(string sp, SqlParameter[] prms)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				try
				{
					SqlCommand command = new SqlCommand(sp, connection)
					{
						CommandType = CommandType.StoredProcedure,
						CommandTimeout = connection.ConnectionTimeout
					};

					command.Parameters.AddRange(prms);

					await connection.OpenAsync();

					DataSet dataSet = new DataSet();
					SqlDataAdapter adapter = new SqlDataAdapter(command);

					await Task.Run(() => adapter.Fill(dataSet));

					command.Parameters.Clear();

					if (dataSet.Tables.Count > 0)
					{
						return dataSet.Tables[0];
					}
					else
					{
						return null;
					}
				}
				catch (Exception ex)
				{
					throw ex;
					//return null;
				}
				finally
				{
					connection.Close();
				}
			}
		}

		public DataTable GetTableFromSP(string sp)
		{
			SqlConnection connection = new SqlConnection(_connectionString);
			SqlCommand command = new SqlCommand();
			try
			{
				command = new SqlCommand(sp, connection) { CommandType = CommandType.StoredProcedure, CommandTimeout = connection.ConnectionTimeout };
				connection.Open();

				DataSet dataSet = new DataSet();
				(new SqlDataAdapter(command)).Fill(dataSet);
				command.Parameters.Clear();

				if (dataSet.Tables.Count > 0)
				{
					return dataSet.Tables[0];
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{
				throw ex;
				//return null;
			}
			finally
			{
				connection.Close();
				command.Dispose();
			}
		}

		public async Task<DataTable> GetTableFromSPAsync(string sp)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = null;
				try
				{
					command = new SqlCommand(sp, connection)
					{
						CommandType = CommandType.StoredProcedure,
						CommandTimeout = connection.ConnectionTimeout
					};

					await connection.OpenAsync();

					DataSet dataSet = new DataSet();
					SqlDataAdapter adapter = new SqlDataAdapter(command);

					await Task.Run(() => adapter.Fill(dataSet));

					if (dataSet.Tables.Count > 0)
					{
						return dataSet.Tables[0];
					}
					else
					{
						return null;
					}
				}
				catch (Exception ex)
				{
					throw ex;
					//return null;
				}
				finally
				{
					connection.Close();
					command?.Dispose();
				}
			}
		}


		public void ExecuteNonQuery(string sp, SqlParameter[] prms)
		{

			SqlConnection connection = new SqlConnection(_connectionString);
			SqlCommand command = new SqlCommand();
			try
			{
				command = new SqlCommand(sp, connection) { CommandType = CommandType.StoredProcedure, CommandTimeout = connection.ConnectionTimeout };
				connection.Open();

				command.Parameters.AddRange(prms);

				command.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				throw ex;

			}
			finally
			{
				connection.Close();
				command.Dispose();
			}
		}

		public async Task ExecuteNonQueryAsync(string sp, SqlParameter[] prms)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = null;
				try
				{
					command = new SqlCommand(sp, connection) { CommandType = CommandType.StoredProcedure, CommandTimeout = connection.ConnectionTimeout };
					await connection.OpenAsync();

					command.Parameters.AddRange(prms);

					await command.ExecuteNonQueryAsync();
				}
				catch (Exception ex)
				{
					throw ex;
				}
				finally
				{
					connection.Close();
					command?.Dispose();
				}
			}
		}


		public void ExecuteNonQuery(string sp, SqlParameter prms)
		{

			SqlConnection connection = new SqlConnection(_connectionString);
			SqlCommand command = new SqlCommand();
			try
			{
				command = new SqlCommand(sp, connection) { CommandType = CommandType.StoredProcedure, CommandTimeout = connection.ConnectionTimeout };
				connection.Open();
				prms.SqlDbType = SqlDbType.Structured;
				command.Parameters.Add(prms);
				command.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				connection.Close();
				command.Dispose();
			}
		}

		public async Task ExecuteNonQueryAsync(string sp, SqlParameter prms)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = null;
				try
				{
					command = new SqlCommand(sp, connection)
					{
						CommandType = CommandType.StoredProcedure,
						CommandTimeout = connection.ConnectionTimeout
					};

					await connection.OpenAsync();

					prms.SqlDbType = SqlDbType.Structured;
					command.Parameters.Add(prms);

					await command.ExecuteNonQueryAsync();
				}
				catch (Exception ex)
				{
					throw ex;
				}
				finally
				{
					connection.Close();
					command?.Dispose();
				}
			}
		}


		public void ExecuteNonQuery(string sp, SqlParameter prm, SqlParameter[] prms)
		{

			SqlConnection connection = new SqlConnection(_connectionString);
			SqlCommand command = new SqlCommand();
			try
			{
				command = new SqlCommand(sp, connection) { CommandType = CommandType.StoredProcedure, CommandTimeout = connection.ConnectionTimeout };
				connection.Open();
				prm.SqlDbType = SqlDbType.Structured;
				command.Parameters.Add(prm);
				command.Parameters.AddRange(prms);
				command.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				connection.Close();
				command.Dispose();
			}
		}

		public async Task ExecuteNonQueryAsync(string sp, SqlParameter prm, SqlParameter[] prms)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = null;
				try
				{
					command = new SqlCommand(sp, connection)
					{
						CommandType = CommandType.StoredProcedure,
						CommandTimeout = connection.ConnectionTimeout
					};

					await connection.OpenAsync();

					prm.SqlDbType = SqlDbType.Structured;
					command.Parameters.Add(prm);
					command.Parameters.AddRange(prms);

					await command.ExecuteNonQueryAsync();
				}
				catch (Exception ex)
				{
					throw ex;
				}
				finally
				{
					connection.Close();
					command?.Dispose();
				}
			}
		}


		public DataTable GetTableRow(string sp, SqlParameter[] prms)
		{


			SqlConnection connection = new SqlConnection(_connectionString);
			try
			{
				SqlCommand command = new SqlCommand(sp, connection) { CommandType = CommandType.StoredProcedure, CommandTimeout = connection.ConnectionTimeout };
				command.Parameters.AddRange(prms);
				connection.Open();

				DataSet dataSet = new DataSet();
				(new SqlDataAdapter(command)).Fill(dataSet);
				command.Parameters.Clear();

				if (dataSet.Tables.Count > 0)
				{
					return dataSet.Tables[0];
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{
				throw ex;
				//return null;
			}
			finally
			{
				connection.Close();
			}
		}

		public async Task<DataTable> GetTableRowAsync(string sp, SqlParameter[] prms)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = null;
				try
				{
					command = new SqlCommand(sp, connection)
					{
						CommandType = CommandType.StoredProcedure,
						CommandTimeout = connection.ConnectionTimeout
					};

					command.Parameters.AddRange(prms);

					await connection.OpenAsync();

					DataSet dataSet = new DataSet();
					SqlDataAdapter adapter = new SqlDataAdapter(command);

					await Task.Run(() => adapter.Fill(dataSet));

					if (dataSet.Tables.Count > 0)
					{
						return dataSet.Tables[0];
					}
					else
					{
						return null;
					}
				}
				catch (Exception ex)
				{
					throw ex;
					//return null;
				}
				finally
				{
					connection.Close();
					command?.Dispose();
				}
			}
		}


		public DataSet GetDatasetFromSP(string sp, SqlParameter[] prms)
		{

			SqlConnection connection = new SqlConnection(_connectionString);
			try
			{
				SqlCommand command = new SqlCommand(sp, connection) { CommandType = CommandType.StoredProcedure, CommandTimeout = connection.ConnectionTimeout };
				connection.Open();

				command.Parameters.AddRange(prms);

				DataSet dataSet = new DataSet();
				(new SqlDataAdapter(command)).Fill(dataSet);
				command.Parameters.Clear();

				return dataSet;
			}
			catch (Exception ex)
			{
				throw ex;
				//return null;
			}
			finally
			{
				connection.Close();
			}
		}

		public async Task<DataSet> GetDatasetFromSPAsync(string sp, SqlParameter[] prms)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = null;
				try
				{
					command = new SqlCommand(sp, connection)
					{
						CommandType = CommandType.StoredProcedure,
						CommandTimeout = connection.ConnectionTimeout
					};

					await connection.OpenAsync();

					command.Parameters.AddRange(prms);

					DataSet dataSet = new DataSet();
					SqlDataAdapter adapter = new SqlDataAdapter(command);

					await Task.Run(() => adapter.Fill(dataSet));

					command.Parameters.Clear();

					return dataSet;
				}
				catch (Exception ex)
				{
					throw ex;
					//return null;
				}
				finally
				{
					connection.Close();
					command?.Dispose();
				}
			}
		}


		public int ExecuteNonQueryReturn(string sp, SqlParameter[] prms)
		{


			SqlConnection connection = new SqlConnection(_connectionString);
			SqlCommand command = new SqlCommand();
			try
			{
				command = new SqlCommand(sp, connection) { CommandType = CommandType.StoredProcedure, CommandTimeout = connection.ConnectionTimeout };
				connection.Open();
				command.Parameters.AddRange(prms);
				int result = command.ExecuteNonQuery();
				return result;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				connection.Close();
				command.Dispose();
			}
		}

		public async Task<int> ExecuteNonQueryReturnAsync(string sp, SqlParameter[] prms)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = null;
				try
				{
					command = new SqlCommand(sp, connection)
					{
						CommandType = CommandType.StoredProcedure,
						CommandTimeout = connection.ConnectionTimeout
					};

					await connection.OpenAsync();

					command.Parameters.AddRange(prms);

					int result = await command.ExecuteNonQueryAsync();

					return result;
				}
				catch (Exception ex)
				{
					throw ex;
				}
				finally
				{
					connection.Close();
					command?.Dispose();
				}
			}
		}


		public string ExecuteScalarFunction(string CommandText)
		{
			string Result = "";

			SqlConnection connection = new SqlConnection(_connectionString);
			SqlCommand command = new SqlCommand();
			try
			{
				connection.Open();
				command = new SqlCommand(CommandText, connection);
				SqlDataAdapter da = new SqlDataAdapter(command);
				DataTable dt = new DataTable();
				da.Fill(dt);

				Result = dt.Rows[0][0].ToString();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				connection.Close();
				command.Dispose();
			}

			return Result;

		}

		public async Task<string> ExecuteScalarFunctionAsync(string commandText)
		{
			string result = "";

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = null;
				try
				{
					command = new SqlCommand(commandText, connection);
					await connection.OpenAsync();

					SqlDataAdapter adapter = new SqlDataAdapter(command);
					DataTable dt = new DataTable();

					await Task.Run(() => adapter.Fill(dt));

					if (dt.Rows.Count > 0 && dt.Columns.Count > 0)
					{
						result = dt.Rows[0][0].ToString();
					}
				}
				catch (Exception ex)
				{
					throw ex;
				}
				finally
				{
					connection.Close();
					command?.Dispose();
				}
			}

			return result;
		}


		public void ExecuteMultipleDatatable(string sp, SqlParameter[] prms, DataSet ds)
		{
			SqlConnection connection = new SqlConnection(_connectionString);
			SqlCommand command = new SqlCommand();
			try
			{
				command = new SqlCommand(sp, connection) { CommandType = CommandType.StoredProcedure, CommandTimeout = connection.ConnectionTimeout };
				connection.Open();
				command.Parameters.AddRange(prms);
				if (null != ds)
				{
					foreach (DataTable dt in ds.Tables)
					{
						SqlParameter parameter = new SqlParameter();
						parameter.SqlDbType = SqlDbType.Structured;

						//DataTable.TableName is the parameter Name
						//e.g: @AppList
						parameter.ParameterName = dt.TableName;
						//DataTable.DisplayExpression is the equivalent SQLType Name. i.e. Name of the UserDefined Table type
						//e.g: AppCollectionType
						//parameter.TypeName = dt.DisplayExpression;
						parameter.TypeName = dt.Namespace;
						parameter.Value = dt;

						command.Parameters.Add(parameter);
					}
				}
				int result = command.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				connection.Close();
				command.Dispose();
			}
		}

		public async Task ExecuteMultipleDatatableAsync(string sp, SqlParameter[] prms, DataSet ds)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = null;
				try
				{
					command = new SqlCommand(sp, connection) { CommandType = CommandType.StoredProcedure, CommandTimeout = connection.ConnectionTimeout };
					await connection.OpenAsync();
					command.Parameters.AddRange(prms);
					if (ds != null)
					{
						foreach (DataTable dt in ds.Tables)
						{
							SqlParameter parameter = new SqlParameter
							{
								SqlDbType = SqlDbType.Structured,
								ParameterName = dt.TableName,
								TypeName = dt.Namespace,
								Value = dt
							};

							command.Parameters.Add(parameter);
						}
					}

					await command.ExecuteNonQueryAsync();
				}
				catch (Exception ex)
				{
					throw ex;
				}
				finally
				{
					connection.Close();
					command?.Dispose();
				}
			}
		}


	}
}
