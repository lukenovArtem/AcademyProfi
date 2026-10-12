using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AcademyProfi.Data;
using AcademyProfi.Models;
using System.Text;

namespace AcademyProfi.Controllers
{
    public class ApplicationsController : Controller
    {
        private readonly ApplicationContext _context;

        public ApplicationsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Applications (Index + фильтр)
        public async Task<IActionResult> Index(int? statusId, string? search)
        {
            var query = _context.Applications
                .Include(a => a.Course)
                .Include(a => a.Status)
                .AsQueryable();

            if (statusId.HasValue && statusId > 0)
                query = query.Where(a => a.StatusId == statusId);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(a => a.StudentName.Contains(search));

            ViewBag.StatusList = new SelectList(_context.StatusDicts, "Id", "DisplayName", statusId);
            ViewBag.CurrentSearch = search;

            return View(await query.OrderByDescending(a => a.CreatedAt).ToListAsync());
        }

        // AJAX: фильтр по статусу + поиск (debounce на клиенте)
        [HttpGet]
        public async Task<IActionResult> Filter(int? statusId, string? search)
        {
            var query = _context.Applications
                .Include(a => a.Course)
                .Include(a => a.Status)
                .AsQueryable();

            if (statusId.HasValue && statusId > 0)
                query = query.Where(a => a.StatusId == statusId);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(a => a.StudentName.Contains(search));

            var result = await query.OrderByDescending(a => a.CreatedAt).ToListAsync();
            return PartialView("_ApplicationsTable", result);
        }

        // AJAX: смена статуса
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, int statusId)
        {
            var app = await _context.Applications.FindAsync(id);
            if (app == null) return NotFound();

            app.StatusId = statusId;
            await _context.SaveChangesAsync();

            var statusName = (await _context.StatusDicts.FindAsync(statusId))?.DisplayName;
            return Json(new { success = true, newStatus = statusName });
        }

        // Экспорт в CSV (доп. требование)
        public async Task<IActionResult> ExportCsv()
        {
            var apps = await _context.Applications
                .Include(a => a.Course)
                .Include(a => a.Status)
                .ToListAsync();

            var sb = new StringBuilder();
            sb.AppendLine("Id;ФИО;Email;Курс;Статус;Дата");

            foreach (var a in apps)
            {
                sb.AppendLine($"{a.Id};{a.StudentName};{a.Email};{a.Course?.Title};{a.Status?.DisplayName};{a.CreatedAt:yyyy-MM-dd HH:mm}");
            }

            // BOM для корректного отображения кириллицы в Excel
            var bytes = Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(sb.ToString())).ToArray();
            return File(bytes, "text/csv", $"applications_{DateTime.Now:yyyyMMdd}.csv");
        }

        // GET: Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Title");
            ViewData["StatusId"] = new SelectList(_context.StatusDicts, "Id", "DisplayName");
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentName,Email,CourseId,StatusId")] Application application)
        {
            if (ModelState.IsValid)
            {
                _context.Add(application);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Title", application.CourseId);
            ViewData["StatusId"] = new SelectList(_context.StatusDicts, "Id", "DisplayName", application.StatusId);
            return View(application);
        }

        // GET: Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var app = await _context.Applications.FindAsync(id);
            if (app == null) return NotFound();

            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Title", app.CourseId);
            ViewData["StatusId"] = new SelectList(_context.StatusDicts, "Id", "DisplayName", app.StatusId);
            return View(app);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentName,Email,CourseId,StatusId,CreatedAt")] Application application)
        {
            if (id != application.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(application);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Applications.Any(e => e.Id == application.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Title", application.CourseId);
            ViewData["StatusId"] = new SelectList(_context.StatusDicts, "Id", "DisplayName", application.StatusId);
            return View(application);
        }

        // GET: Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var app = await _context.Applications
                .Include(a => a.Course)
                .Include(a => a.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (app == null) return NotFound();
            return View(app);
        }

        // GET: Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var app = await _context.Applications
                .Include(a => a.Course)
                .Include(a => a.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (app == null) return NotFound();
            return View(app);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var app = await _context.Applications.FindAsync(id);
            if (app != null) _context.Applications.Remove(app);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}