using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventsMVC.Models
{
    public partial class RoleTable
    {
        public RoleTable()
        {
            UserMasters = new HashSet<UserMaster>();
        }

        public int RoleId { get; set; }
		[Required(ErrorMessage = "Please Enter Role")]
		public string? RoleName { get; set; }

        public virtual ICollection<UserMaster> UserMasters { get; set; }
    }
}
