using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ActivityManagement.Models;

namespace ActivityManagement.ViewModels
{
    public class CourseCategoriesViewModel
    {
        public Course Course { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}