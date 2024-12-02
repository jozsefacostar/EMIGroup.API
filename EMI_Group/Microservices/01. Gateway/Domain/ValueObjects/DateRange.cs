namespace Domain.ValueObjects
{
    public class DateRange
    {
        public DateTime StartDate { get; }
        public DateTime? EndDate { get; }

        public DateRange(DateTime startDate, DateTime? endDate)
        {
            if (endDate != null && (startDate > endDate))
                throw new ArgumentException("La fecha inicial no puede ser mayor que la fecha final.");

            StartDate = startDate;
            EndDate = endDate;
        }
        public override bool Equals(object obj)
        {
            var other = obj as DateRange;
            return other != null && StartDate == other.StartDate && EndDate == other.EndDate;
        }

        public override int GetHashCode() => HashCode.Combine(StartDate, EndDate);
    }
}
