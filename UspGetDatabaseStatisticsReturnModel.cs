using System;
using System.Collections.Generic;
using System.Text;

namespace DVDStore.DAL.Models
{
	public partial class UspGetDatabaseStatisticsReturnModel
	{
		public string TableName { get; set; }
		public int? Rows { get; set; }
		public string SpaceReservedUsed { get; set; }
		public string DataSpaceUsed { get; set; }
		public string IndexSpaceUsed { get; set; }
		public string UnusedSpace { get; set; }
    }
}
