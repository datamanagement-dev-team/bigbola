namespace BlueBrown.BigBola.Application.Entities
{
	public record Request
	{
		private readonly string _databaseDateTimeFormat = "yyyy-MM-dd";

		public string StartDate { get; private set; } = string.Empty;
		public string EndDate { get; private set; } = string.Empty;
		public int Rows { get; init; }
		public int Page { get; init; }

		public Request(string startDate, string endDate, int rows, int page)
		{
			StartDate = startDate;
			EndDate = endDate;
			Rows = rows;
			Page = page;
		}

		public void UpdateStartDate()
		{
			var date = DateTime.Parse(StartDate);
			StartDate = date.ToString(_databaseDateTimeFormat);
		}

		public void UpdateEndDate()
		{
			var date = DateTime.Parse(EndDate);
			EndDate = date.ToString(_databaseDateTimeFormat);
		}
	}
}
