using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaMicroservice.Domain.Models
{
    public class Media
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Data { get; set; }
    }
}
