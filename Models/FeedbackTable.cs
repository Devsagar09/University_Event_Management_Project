using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventsMVC.Models
{
    public partial class FeedbackTable
    {
        public int FeedbackId { get; set; }
        public int UserId { get; set; } 
		public string? Message { get; set; }

        public virtual UserMaster User { get; set; } = null!;
    }
}
