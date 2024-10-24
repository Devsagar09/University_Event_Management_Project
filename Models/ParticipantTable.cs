using System;
using System.Collections.Generic;

namespace EventsMVC.Models
{
    public partial class ParticipantTable
    {
        public int ParticipantId { get; set; }
        public int? UserId { get; set; }
        public int? EventId { get; set; }
        public DateTime? Date { get; set; }

        public virtual EventMaster? Event { get; set; }
        public virtual UserMaster? User { get; set; }
    }
}
