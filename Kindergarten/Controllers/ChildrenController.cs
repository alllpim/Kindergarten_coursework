using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kindergarten.Cache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kindergarten.Data;
using Kindergarten.Infrastructure;
using Kindergarten.Models;
using Kindergarten.ViewModels;
using Kindergarten.ViewModels.FilterViewModels;
using Kindergarten.ViewModels.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.AspNetCore.Authorization;

namespace Kindergarten.Controllers
{
    [Authorize]
    public class ChildrenController : Controller
    {
        private readonly kindergartenContext _context;
        private readonly CacheProvider _cache;
        private int pageSize = 10;
        private const string filterKey = "children";

        public ChildrenController(kindergartenContext context, CacheProvider cache)
        {
            _context = context;
            _cache = cache;
        }

        // GET: Children
        public async Task<IActionResult> Index(SortState sortState, int page = 1)
        {
            ChildrenFilterViewModel filter = HttpContext.Session.Get<ChildrenFilterViewModel>(filterKey);
            if (filter == null)
            {
                filter = new ChildrenFilterViewModel
                {
                    FullName = string.Empty,
                    Group = string.Empty,
                    GroupType = string.Empty,
                    Age = null,
                    OtherGroups = string.Empty
                };
                HttpContext.Session.Set(filterKey, filter);
            }

            string key = $"{typeof(Child).Name}-{page}-{sortState}-{filter.FullName}-{filter.Group}-{filter.GroupType}-{filter.Age}-{filter.OtherGroups}";

            if (!_cache.TryGetValue(key, out ChildrenViewModel model))
            {
                model = new ChildrenViewModel();

                IQueryable<Child> children = GetSortedChildren(sortState, filter.FullName, filter.Group, filter.GroupType, filter.Age, filter.OtherGroups);

                int count = children.Count();

                model.PageViewModel = new PageViewModel(page, count, pageSize);

                model.Childs = count == 0 ? new List<Child>() : children.Skip((model.PageViewModel.PageIndex - 1) * pageSize).Take(pageSize).ToList();
                model.SortViewModel = new SortViewModel(sortState);
                model.ChildrenFilterViewModel = filter;

                _cache.Set(key, model);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(ChildrenFilterViewModel filterModel, int page)
        {
            ChildrenFilterViewModel filter = HttpContext.Session.Get<ChildrenFilterViewModel>(filterKey);
            if (filter != null)
            {
                filter.FullName = filterModel.FullName;
                filter.Group = filterModel.Group;
                filter.GroupType = filterModel.GroupType;
                filter.Age = filterModel.Age;
                filter.OtherGroups = filterModel.OtherGroups;

                HttpContext.Session.Remove(filterKey);
                HttpContext.Session.Set(filterKey, filter);
            }

            return RedirectToAction("Index", new { page });
        }

        private IQueryable<Child> GetSortedChildren(SortState sortState, string name, string group, string groupType, int? age, string otherGroup)
        {
            IQueryable<Child> children = _context.Children.Include(x => x.Parent).Include(x => x.Group).ThenInclude(x => x.Type).AsQueryable();

            switch (sortState)
            {
                case SortState.ChFullNameAsc:
                    children = children.OrderBy(x => x.FullName);
                    break;
                case SortState.ChFullNameDesc:
                    children = children.OrderByDescending(x => x.FullName);
                    break;
                case SortState.ChOtherAsc:
                    children = children.OrderBy(x => x.OtherGroup);
                    break;
                case SortState.ChOtherDesc:
                    children = children.OrderByDescending(x => x.OtherGroup);
                    break;
                case SortState.ChGenderAsc:
                    children = children.OrderBy(x => x.Gender);
                    break;
                case SortState.ChGenderDesc:
                    children = children.OrderByDescending(x => x.Gender);
                    break;
                case SortState.ChDateAsc:
                    children = children.OrderBy(x => x.BirthDate);
                    break;
                case SortState.ChDateDesc:
                    children = children.OrderByDescending(x => x.BirthDate);
                    break;
                case SortState.ChAdressAsc:
                    children = children.OrderBy(x => x.Adress);
                    break;
                case SortState.ChAdressDesc:
                    children = children.OrderByDescending(x => x.Adress);
                    break;
                case SortState.ChGroupAsc:
                    children = children.OrderBy(x => x.Group);
                    break;
                case SortState.ChGroupDesc:
                    children = children.OrderByDescending(x => x.Group);
                    break;
                case SortState.ChNoteAsc:
                    children = children.OrderBy(x => x.Note);
                    break;
                case SortState.ChNoteDesc:
                    children = children.OrderByDescending(x => x.Note);
                    break;
                case SortState.ChParentAsc:
                    children = children.OrderBy(x => x.Parent.Mfullname);
                    break;
                case SortState.ChParentDesc:
                    children = children.OrderByDescending(x => x.Parent.Mfullname);
                    break;
            }

            if (!string.IsNullOrEmpty(name))
            {
                children = children.Where(x => (x.FullName).Contains(name))
                    .AsQueryable();
            }
            if (age != null)
            {
                children = children.Where(x => 2020 - x.BirthDate.Value.Year == age)
                    .AsQueryable();
            }
            if (!string.IsNullOrEmpty(group))
            {
                children = children.Where(x => (x.Group.GroupName).Contains(group))
                    .AsQueryable();
            }
            if (!string.IsNullOrEmpty(groupType))
            {
                children = children.Where(x => (x.Group.Type.NameOfType).Contains(groupType))
                    .AsQueryable();
            }
            if (!string.IsNullOrEmpty(otherGroup))
            {
                children = children.Where(x => (x.OtherGroup).Contains(otherGroup))
                    .AsQueryable();
            }

            return children;
        }
        // GET: Children/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var child = await _context.Children
                .Include(c => c.Group)
                .Include(c => c.Parent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (child == null)
            {
                return NotFound();
            }

            return View(child);
        }

        // GET: Children/Create
        public IActionResult Create()
        {
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "GroupName");
            ViewData["ParentId"] = new SelectList(_context.Parents, "Id", "MFullName");
            return View();
        }

        // POST: Children/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,BirthDate,Gender,ParentId,Adress,GroupId,Note,OtherGroup")] Child child)
        {
            if (ModelState.IsValid)
            {
                _context.Add(child);
                await _context.SaveChangesAsync();
                _cache.Clean();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Id", child.GroupId);
            ViewData["ParentId"] = new SelectList(_context.Parents, "Id", "Id", child.ParentId);
            return View(child);
        }
        // GET: Children/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var child = await _context.Children.FindAsync(id);
            if (child == null)
            {
                return NotFound();
            }
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "GroupName", child.GroupId);
            ViewData["ParentId"] = new SelectList(_context.Parents, "Id", "MFullName", child.ParentId);
            return View(child);
        }

        // POST: Children/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,BirthDate,Gender,ParentId,Adress,GroupId,Note,OtherGroup")] Child child)
        {
            if (id != child.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(child);
                    await _context.SaveChangesAsync();
                    _cache.Clean();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChildExists(child.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Id", child.GroupId);
            ViewData["ParentId"] = new SelectList(_context.Parents, "Id", "Id", child.ParentId);
            return View(child);
        }
        // GET: Children/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var child = await _context.Children
                .Include(c => c.Group)
                .Include(c => c.Parent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (child == null)
            {
                return NotFound();
            }

            return View(child);
        }
        // POST: Children/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var child = await _context.Children.FindAsync(id);
            _context.Children.Remove(child);
            await _context.SaveChangesAsync();
            _cache.Clean();
            return RedirectToAction(nameof(Index));
        }

        private bool ChildExists(int id)
        {
            return _context.Children.Any(e => e.Id == id);
        }
    }
}
