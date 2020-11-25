using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kindergarten.Models;
using Kindergarten.ViewModels.FilterViewModels;

namespace Kindergarten.ViewModels.Models
{
    public class ChildrenViewModel
    {
        public IEnumerable<Child> Childs { get; set; }

        public Child Child { get; set; }

        public PageViewModel PageViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }
        public ChildrenFilterViewModel ChildrenFilterViewModel { get; set; }
    }
}
