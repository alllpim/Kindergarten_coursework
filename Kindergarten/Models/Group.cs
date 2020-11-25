using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Kindergarten.Models
{
    public partial class Group
    {
        public Group()
        {
            Children = new HashSet<Child>();
        }

        public int Id { get; set; }
        [Display(Name = "Group Name")]
        public string GroupName { get; set; }
        public int? StaffId { get; set; }
        [Display(Name = "Children count")]
        public int? CountOfChildren { get; set; }
        [Display(Name = "Creation year")]
        public int? YearOfCreation { get; set; }
        public int? TypeId { get; set; }

        public virtual Staff Staff { get; set; }
        public virtual GroupType Type { get; set; }
        public virtual ICollection<Child> Children { get; set; }
    }
}
