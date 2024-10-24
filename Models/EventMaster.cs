using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventsMVC.Models
{
    public partial class EventMaster
    {
        public EventMaster()
        {
            HistoryTables = new HashSet<HistoryTable>();
            ParticipantTables = new HashSet<ParticipantTable>();
            PaymentTables = new HashSet<PaymentTable>();
        }

        public int EventId { get; set; }
		[Required(ErrorMessage = "Event Name is required.")]
		public string? EventName { get; set; }
      
		public string? EventImage { get; set; }
		[Required(ErrorMessage = "Event Description is required.")]
		public string? Description { get; set; }
		[Required(ErrorMessage = "Event Date is required.")]
		public string? Date { get; set; }
		[Required(ErrorMessage = "Price is required.")]
		public int? Price { get; set; }
		[Required(ErrorMessage = "Category is required.")]
		public int? CategoryId { get; set; }

        public virtual EventCategory? Category { get; set; }
        public virtual ICollection<HistoryTable> HistoryTables { get; set; }
        public virtual ICollection<ParticipantTable> ParticipantTables { get; set; }
        public virtual ICollection<PaymentTable> PaymentTables { get; set; }
    }
}
