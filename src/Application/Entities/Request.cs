namespace BlueBrown.BigBola.Application.Entities
{
    public class Request
    {
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public int Rows { get; set; }
        public int Page { get; set; }
    }
}
