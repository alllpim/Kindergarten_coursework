using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Kindergarten.Models
{
    public partial class GroupType
    {
        public GroupType()
        {
            Groups = new HashSet<Group>();
        }

        public int Id { get; set; }
        [Display(Name = "Group Type name")]
        public string NameOfType { get; set; }
        public string Note { get; set; }

        public virtual ICollection<Group> Groups { get; set; }
    }
}
