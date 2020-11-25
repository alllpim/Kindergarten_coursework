using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Kindergarten.Models
{
    public partial class Parent
    {
        public Parent()
        {
            Children = new HashSet<Child>();
        }

        public int Id { get; set; }
        [Display(Name = "Mother full name")]
        public string Mfullname { get; set; }
        [Display(Name = "Father full name")]
        public string Ffullname { get; set; }

        public virtual ICollection<Child> Children { get; set; }
    }
}
