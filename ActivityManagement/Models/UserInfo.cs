using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ActivityManagement.Models
{
    public class UserInfo
    {
		[Key]
		[ForeignKey("User")]
		public string UserId { get; set; }
		public ApplicationUser User { get; set; }
		public string FullName { get; set; }
		public int Age { get; set; }
	}
}