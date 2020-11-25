using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Kindergarten.Models
{
    public partial class Staff
    {
        public Staff()
        {
            Groups = new HashSet<Group>();
        }

        public int Id { get; set; }
        [Display(Name = "Staff full name")]
        public string FullName { get; set; }
        public string Adress { get; set; }
        public int? Phone { get; set; }
        public int? PositionId { get; set; }
        public string Info { get; set; }
        public string Reward { get; set; }

        public virtual Position Position { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
    }
}
