using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Kindergarten.Models
{
    public partial class Child
    {
        public int Id { get; set; }
        [Display(Name = "Child Name")]
        public string FullName { get; set; }
        [Display(Name = "Birth date")]
        public DateTime? BirthDate { get; set; }
        public string Gender { get; set; }
        public int? ParentId { get; set; }
        public string Adress { get; set; }
        public int? GroupId { get; set; }
        public string Note { get; set; }
        [Display(Name = "Other Group")]
        public string OtherGroup { get; set; }

        public virtual Group Group { get; set; }
        public virtual Parent Parent { get; set; }
    }
}
