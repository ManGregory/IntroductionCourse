﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebLMS.Models
{
    public class CodingHomeworkTestRun
    {
        public int Id { get; set; }
        public int HomeworkId { get; set; }
        public int CodingTestId { get; set; }
        public virtual IdentityUser User { get; set; }
        public string SourceCode { get; set; }
        public string Result { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}