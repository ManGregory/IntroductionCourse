using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebLMS.Models
{
    public class Lecture
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        [DataType("Date")]
        public DateTime? AvailableFrom { get; set; }
        [DataType("Date")]
        public DateTime? AvailableTo { get; set; }

        [NotMapped]
        public bool IsAvailable 
        { 
            get
            {
                return AvailableFrom.HasValue && AvailableFrom.Value >= DateTime.Now && (!AvailableTo.HasValue || AvailableTo.Value <= DateTime.Now);
            } 
        }

        [NotMapped]
        public string Title
        {
            get
            {
                return $"Лекция {Number} - {Subject}";
            }
        }
    }
}
