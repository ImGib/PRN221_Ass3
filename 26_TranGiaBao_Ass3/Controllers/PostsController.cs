using _26_TranGiaBao_Ass3.Data;
using _26_TranGiaBao_Ass3.Models;
using _26_TranGiaBao_Ass3.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace _26_TranGiaBao_Ass3.Controllers
{
    public class PostsDto
    {
        public int PostId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool? PublishStatus { get; set; }
        public string AuthorName { get; set; }
        public string CategoryName { get; set; }
    }
    public class PostsController : Controller
    {
        private readonly SignalRContext _context;
        private readonly IHubContext<SignalrServer> _hubContext;
        public PostsController(SignalRContext context, IHubContext<SignalrServer> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }
        //[HttpGet]
        //public IActionResult GetPosts()
        //{
        //    //var res = _context.Posts.SingleOrDefault(p => p.PostID == id);
        //    var res = _context.Posts.Include(p => p.User).Include(p => p.Category).ToList();
        //    var res_2 = res.Select(p => new
        //    {
        //        p.PostID,
        //        p.AuthorID,
        //        p.CreatedDate,
        //        p.UpdatedDate,
        //        AuthorName = p.User.Fullname,
        //        CategoryName = p.Category.CategoryName,
        //        p.PublishStatus,
        //        p.Title
        //    });
        //    return Ok(res_2);
        //}
        // GET: Posts
        public async Task<IActionResult> Index(int? pageIndex, string searchValue, DateTime? startDate, DateTime? endDate)
        {
            if (_context.Posts != null)
            {
                int pageSize = 2;

                IQueryable<Posts> posts;

                if (!string.IsNullOrEmpty(searchValue))
                {
                    posts = _context.Posts.Include(p => p.User).Include(p => p.Category)
                        .Where(p => p.Title.ToUpper().Contains(searchValue.ToUpper())
                        || p.Content.ToUpper().Contains(searchValue.ToUpper())
                        //|| p.PostID.ToString().Contains(searchValue)
                        );
                }
                else
                {
                    posts = _context.Posts.Include(p => p.User).Include(p => p.Category);
                }

                if (startDate != null)
                {
                    posts = posts.Where(p => p.CreatedDate >= startDate);
                }

                if (endDate != null)
                {
                    posts = posts.Where(p => p.CreatedDate <= endDate);
                }

                Paganation<Posts> result = await Paganation<Posts>.CreateAsync(posts.OrderBy(x => x.CreatedDate), pageIndex ?? 1, pageSize);

                ViewData["startDate"] = startDate ?? DateTime.MinValue;
                ViewData["endDate"] = endDate ?? DateTime.Now;
                ViewData["searchValue"] = searchValue;
                ViewData["PostResult"] = result;
                ViewData["pageIndex"] = pageIndex ?? 1;
                return View();
            }

            return Problem("Entity set 'SignalRContext.Posts'  is null.");
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var posts = await _context.Posts.Include(p => p.User).Include(p => p.Category).SingleOrDefaultAsync(p => p.PostID == id);

            if (posts == null)
            {
                return NotFound();
            }

            return View(posts);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            ViewData["CategoryType"] = new SelectList(_context.PostCategories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostID,AuthorID,CreatedDate,UpdatedDate,Title,Content,PublishStatus,CategoryID")] Posts posts)
        {
            if (ModelState.IsValid)
            {
                _context.Add(posts);
                await _context.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("LoadPosts");
                return RedirectToAction(nameof(Index));
            }
            return View(posts);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            ViewData["CategoryType"] = new SelectList(_context.PostCategories, "CategoryID", "CategoryName");

            var posts = await _context.Posts.Include(p => p.User).Include(p => p.Category).SingleOrDefaultAsync(p => p.PostID == id);
            if (posts == null)
            {
                return NotFound();
            }
            return View(posts);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostID,AuthorID,CreatedDate,UpdatedDate,Title,Content,PublishStatus,CategoryID")] Posts posts)
        {
            if (id != posts.PostID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(posts);
                    await _hubContext.Clients.All.SendAsync("LoadPosts");
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostsExists(posts.PostID))
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
            return View(posts);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var posts = await _context.Posts
                .FirstOrDefaultAsync(m => m.PostID == id);
            if (posts == null)
            {
                return NotFound();
            }

            return View(posts);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Posts == null)
            {
                return Problem("Entity set 'SignalRContext.Posts'  is null.");
            }
            var posts = await _context.Posts.FindAsync(id);
            if (posts != null)
            {
                _context.Posts.Remove(posts);
            }

            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("LoadPosts");
            return RedirectToAction(nameof(Index));
        }

        private bool PostsExists(int id)
        {
            return (_context.Posts?.Any(e => e.PostID == id)).GetValueOrDefault();
        }
    }
}
