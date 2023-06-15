namespace BlueBrown.BigBola.Application.Entities
{
    public record Request
    {
        public string startDate { get; private set; } = string.Empty;
        public string endDate { get; private set; } = string.Empty;
        public int rows { get; init; }
        public int page { get; init; }

        public Request(string startDate, string endDate, int rows, int page)
        {
            this.startDate = startDate;
            this.endDate = endDate;
            this.rows = rows;
            this.page = page;
        }

        public void UpdateStartDate()
        {
            var date = DateTime.Parse(startDate);
            startDate = date.ToString("yyyy-MM-dd");
        }

        public void UpdateEndDate()
        {
            var date = DateTime.Parse(endDate);
            endDate = date.ToString("yyyy-MM-dd");
        }
    }
}
