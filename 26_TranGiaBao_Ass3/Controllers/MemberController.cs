using _26_TranGiaBao_Ass3.Data;
using _26_TranGiaBao_Ass3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _26_TranGiaBao_Ass3.Controllers
{
    public class MemberController : Controller
    {
        private readonly SignalRContext _context;

        public MemberController(SignalRContext context)
        {
            _context = context;
        }

        // GET: Member
        public async Task<IActionResult> Index()
        {
            return _context.AppUsers != null ?
                        View(await _context.AppUsers.ToListAsync()) :
                        Problem("Entity set 'SignalRContext.AppUsers'  is null.");
        }

        // GET: Member/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AppUsers == null)
            {
                return NotFound();
            }

            var appUsers = await _context.AppUsers
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (appUsers == null)
            {
                return NotFound();
            }

            return View(appUsers);
        }

        // GET: Member/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Member/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,Fullname,Address,Email,Password")] AppUsers appUsers)
        {

            if (ModelState.IsValid)
            {
            //TODO: Need to check duplicate
                _context.Add(appUsers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(appUsers);

        }

        // GET: Member/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AppUsers == null)
            {
                return NotFound();
            }

            var appUsers = await _context.AppUsers.FindAsync(id);
            if (appUsers == null)
            {
                return NotFound();
            }
            return View(appUsers);
        }

        // POST: Member/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,Fullname,Address,Email,Password")] AppUsers appUsers)
        {
            if (id != appUsers.UserID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appUsers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppUsersExists(appUsers.UserID))
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
            return View(appUsers);
        }

        // GET: Member/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AppUsers == null)
            {
                return NotFound();
            }

            var appUsers = await _context.AppUsers
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (appUsers == null)
            {
                return NotFound();
            }

            return View(appUsers);
        }

        // POST: Member/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AppUsers == null)
            {
                return Problem("Entity set 'SignalRContext.AppUsers'  is null.");
            }
            var appUsers = await _context.AppUsers.FindAsync(id);
            if (appUsers != null)
            {
                _context.AppUsers.Remove(appUsers);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppUsersExists(int id)
        {
            return (_context.AppUsers?.Any(e => e.UserID == id)).GetValueOrDefault();
        }
    }
}
