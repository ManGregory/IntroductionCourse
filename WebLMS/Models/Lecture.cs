using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebLMS.Models
{
    public class Lecture
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime? AvailableFrom { get; set; }
        public DateTime? AvailableTo { get; set; }
    }
}
