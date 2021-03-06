using ActivityManagement.UniqueAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ActivityManagement.Models
{
    public class Course
    {
		[Key]
		public int Id { get; set; }
		[Required]
		[StringLength(255)]
		[Unique(ErrorMessage = "Course already exist !!")]
		[DisplayName("Course Name")]
		public string Name { get; set; }
		
		public int CategoryId { get; set; }
		public Category Category { get; set; }
		[Required]
		public DateTime StartDate { get; set; }
		[Required]
		public string Description { get; set; }
	}
}