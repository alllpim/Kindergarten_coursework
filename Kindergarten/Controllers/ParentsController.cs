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
using Microsoft.AspNetCore.Authorization;

namespace Kindergarten.Controllers
{
    [Authorize]
    public class ParentsController : Controller
    {
        private readonly kindergartenContext _context;
        private readonly CacheProvider _cache;
        private int pageSize = 10;
        private const string filterKey = "parents";

        public ParentsController(kindergartenContext context, CacheProvider cache)
        {
            _context = context;
            _cache = cache;
        }

        // GET: Parents
        public async Task<IActionResult> Index(SortState sortState, int page = 1)
        {
            ParentsFilterViewModel filter = HttpContext.Session.Get<ParentsFilterViewModel>(filterKey);
            if (filter == null)
            {
                filter = new ParentsFilterViewModel
                {
                    MFullName = string.Empty,
                    FFullName = string.Empty
                };
                HttpContext.Session.Set(filterKey, filter);
            }

            string key = $"{typeof(Parent).Name}-{page}-{sortState}-{filter.MFullName}-{filter.FFullName}";

            if (!_cache.TryGetValue(key, out ParentsViewModel model))
            {
                model = new ParentsViewModel();

                IQueryable<Parent> parents = GetSortedParents(sortState, filter.MFullName, filter.FFullName);

                int count = parents.Count();

                model.PageViewModel = new PageViewModel(page, count, pageSize);

                model.Parents = count == 0 ? new List<Parent>() : parents.Skip((model.PageViewModel.PageIndex - 1) * pageSize).Take(pageSize).ToList();
                model.SortViewModel = new SortViewModel(sortState);
                model.ParentsFilterViewModel = filter;

                _cache.Set(key, model);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(ParentsFilterViewModel filterModel, int page)
        {
            ParentsFilterViewModel filter = HttpContext.Session.Get<ParentsFilterViewModel>(filterKey);
            if (filter != null)
            {
                filter.MFullName = filterModel.MFullName;
                filter.FFullName = filterModel.FFullName;

                HttpContext.Session.Remove(filterKey);
                HttpContext.Session.Set(filterKey, filter);
            }

            return RedirectToAction("Index", new { page });
        }

        private IQueryable<Parent> GetSortedParents(SortState sortState, string mName, string fName)
        {
            IQueryable<Parent> parents = _context.Parents.AsQueryable();

            switch (sortState)
            {
                case SortState.ParMFullAsc:
                    parents = parents.OrderBy(x => x.Mfullname);
                    break;
                case SortState.ParMFullDesc:
                    parents = parents.OrderByDescending(x => x.Mfullname);
                    break;
                case SortState.ParFFullAsc:
                    parents = parents.OrderBy(x => x.Ffullname);
                    break;
                case SortState.ParFFullDesc:
                    parents = parents.OrderByDescending(x => x.Ffullname);
                    break;
            }

            if (!string.IsNullOrEmpty(mName))
            {
                parents = parents.Where(x => (x.Mfullname).Contains(mName))
                    .AsQueryable();
            }
            if (!string.IsNullOrEmpty(fName))
            {
                parents = parents.Where(x => x.Ffullname.Contains(fName))
                    .AsQueryable();
            }
            return parents
;
        }

        // GET: Parents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parent = await _context.Parents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parent == null)
            {
                return NotFound();
            }

            return View(parent);
        }

        // GET: Parents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Parents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Mfullname,Ffullname")] Parent parent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(parent);
                await _context.SaveChangesAsync();
                _cache.Clean();
                return RedirectToAction(nameof(Index));
            }
            return View(parent);
        }

        // GET: Parents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parent = await _context.Parents.FindAsync(id);
            if (parent == null)
            {
                return NotFound();
            }
            return View(parent);
        }

        // POST: Parents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Mfullname,Ffullname")] Parent parent)
        {
            if (id != parent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parent);
                    await _context.SaveChangesAsync();
                    _cache.Clean();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParentExists(parent.Id))
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
            return View(parent);
        }

        // GET: Parents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parent = await _context.Parents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parent == null)
            {
                return NotFound();
            }

            return View(parent);
        }

        // POST: Parents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parent = await _context.Parents.FindAsync(id);
            _context.Parents.Remove(parent);
            await _context.SaveChangesAsync();
            _cache.Clean();
            return RedirectToAction(nameof(Index));
        }

        private bool ParentExists(int id)
        {
            return _context.Parents.Any(e => e.Id == id);
        }
    }
}
