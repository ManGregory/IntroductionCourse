﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebLMS.Models
{
    public class CodingHomeworkRun
    {
        public int Id { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual CodingHomework CodingHomework { get; set; }
        public string SourceCode { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
