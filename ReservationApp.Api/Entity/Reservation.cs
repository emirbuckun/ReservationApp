namespace ReservationApp.Api.Entity
{
    public class Reservation : BaseEntity
    {
        public DateTime ReservationDate { get; set; }
        public byte StartHour { get; set; }
        public byte Duration { get; set; }
        public string Name { get; set; }
        public Reservation() { Name = string.Empty; }
        public Reservation(DateTime reservationDate,
            byte startHour, byte duration, string name)
        {
            ReservationDate = reservationDate;
            StartHour = startHour;
            Duration = duration;
            Name = name;
        }
    }
}