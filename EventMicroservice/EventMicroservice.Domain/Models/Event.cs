using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventMicroservice.Domain.Models
{
    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Author { get; set; }
        public string Guest { get; set; }
        public long EpochStart { get;set;}
        public long EpochEnd { get; set; }
        public string Subject { get; set; }
        public bool Status { get;set;}
    }
}
