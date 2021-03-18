using System;
using System.Runtime.Serialization;

namespace BlazorEntre2Ages.Models
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Subject { get; set; }
        public bool Status { get; set; }
        public string GuestEmail { get; set; }

        public Appointment()
        {
            
        }

        public Appointment(Event @event)
        {
            Id = @event.Id;
            Start = DateTimeOffset.FromFileTime(@event.EpochStart).Date;
            End = DateTimeOffset.FromFileTime(@event.EpochEnd).Date;
            Subject = @event.Subject;
            Status = @event.Status;
            GuestEmail = @event.Guest;
        }

        public Event ToEvent()
        {
            return new Event()
            {
                Id = Id,
                Guest = GuestEmail,
                Subject = Subject,
                EpochEnd = End.ToFileTimeUtc(),
                EpochStart = Start.ToFileTimeUtc(),
                Status = Status
            };
        }
    }
}