using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventsMVC.Models
{
    public partial class UserMaster
    {
        public UserMaster()
        {
            FeedbackTables = new HashSet<FeedbackTable>();
            HistoryTables = new HashSet<HistoryTable>();
            ParticipantTables = new HashSet<ParticipantTable>();
            PaymentTables = new HashSet<PaymentTable>();
        }
        
        public int UserId { get; set; }
         
		public string? FullName { get; set; } 
		public string? Email { get; set; }
         
		public string? Password { get; set; }
         
		public string? Address { get; set; }
        public int? RoleId { get; set; }

        public virtual RoleTable? Role { get; set; }
        public virtual ICollection<FeedbackTable> FeedbackTables { get; set; }
        public virtual ICollection<HistoryTable> HistoryTables { get; set; }
        public virtual ICollection<ParticipantTable> ParticipantTables { get; set; }
        public virtual ICollection<PaymentTable> PaymentTables { get; set; }
    }
}
