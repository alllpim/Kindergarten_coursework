using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kindergarten.Models;
using Kindergarten.ViewModels.FilterViewModels;

namespace Kindergarten.ViewModels.Models
{
    public class ParentsViewModel
    {
        public IEnumerable<Parent> Parents { get; set; }

        public Parent Parent { get; set; }

        public PageViewModel PageViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }
        public ParentsFilterViewModel ParentsFilterViewModel { get; set; }
    }
}
