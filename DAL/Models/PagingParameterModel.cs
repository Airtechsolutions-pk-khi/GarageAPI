namespace DAL.Models
{
	public class PagingParameterModel
	{
		//const int MaxPageSize = 20;

		public int PageNumber { get; set; } = 1;

		public int PageSize { get; set; } = 10;

		//public int PageSize
		//{

		//	get { return _PageSize; }
		//	set
		//	{
		//		//_PageSize = (value > MaxPageSize) ? MaxPageSize : value;
		//		_PageSize = value;
		//	}
		//}
	}
}
