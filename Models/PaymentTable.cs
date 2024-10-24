using System;
using System.Collections.Generic;
using System.Composition;

namespace EventsMVC.Models
{
    public partial class PaymentTable
    {
        public int PaymentId { get; set; }
        public int? UserId { get; set; }
        public int? EventId { get; set; }
        public DateTime? Date { get; set; }
        public int? Amount { get; set; }

        public string? CardholderName { get; set; }

        public int Card_Number { get; set; }
        public string? Card_Type { get; set; }
		public DateTime? Expiry { get; set; }
        public int CVV { get; set; }

        public virtual EventMaster? Event { get; set; }
        public virtual UserMaster? User { get; set; }
    }
}

