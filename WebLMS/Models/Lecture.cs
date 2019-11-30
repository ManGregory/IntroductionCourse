using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebLMS.Models
{
    public class Lecture
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        [DataType("Date")]
        public DateTime? AvailableFrom { get; set; }
        [DataType("Date")]
        public DateTime? AvailableTo { get; set; }
    }
}
