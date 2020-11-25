using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kindergarten.ViewModels.FilterViewModels
{
    public class ChildrenFilterViewModel
    {
        public string FullName { get; set; }
        public string Group { get; set; }
        public string GroupType { get; set; }
        public int? Age { get; set; }
        public string OtherGroups { get; set; }
    }
}
