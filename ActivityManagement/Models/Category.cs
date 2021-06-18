using ActivityManagement.UniqueAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ActivityManagement.Models
{
    public class Category
    {
		[Key]
		public int Id { get; set; }
		[Required]
		[StringLength(255)]
		[Unique(ErrorMessage = "Category already exist !!")]
		public string Name { get; set; }
		[Required]
		[StringLength(255)]
		public string Description { get; set; }
	}
}