using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventsMVC.Models
{
    public partial class EventCategory
    {
        public EventCategory()
        {
            EventMasters = new HashSet<EventMaster>();
        }

        public int CategoryId { get; set; }
		[Required(ErrorMessage = "Please Enter CategoryName")]
		public string? CategoryName { get; set; }

        public virtual ICollection<EventMaster> EventMasters { get; set; }
    }
}
