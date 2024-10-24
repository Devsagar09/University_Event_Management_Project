using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventsMVC.Models
{
    public partial class ContactTable
    {
        public int ContactId { get; set; }
		[Required(ErrorMessage = "Please Enter UserName")]
		public string? FullName { get; set; }
		[Required(ErrorMessage = "Please Enter EmailID")]
		public string? Email { get; set; }
		[StringLength(100, ErrorMessage = "The query must be less than 250 characters.")]
		public string? Query { get; set; }
    }
}
