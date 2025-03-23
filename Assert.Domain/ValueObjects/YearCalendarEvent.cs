namespace Assert.Domain.ValueObjects
{
    public class YearCalendarEvent
    {
        public List<MonthCalendarEvent> Months { get; set; }
        public int Year { get; set; }
    }
    public class MonthCalendarEvent
    {
        public int Month { get; set; }
        public List<int> Days { get; set; }
    }
}
