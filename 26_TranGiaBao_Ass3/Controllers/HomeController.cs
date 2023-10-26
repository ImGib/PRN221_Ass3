using _26_TranGiaBao_Ass3.Data;
using _26_TranGiaBao_Ass3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace _26_TranGiaBao_Ass3.Controllers
{
    public class HomeSearch
    {
        [BindProperty]
        public string SearchValue { get; set; }
        [BindProperty]
        public string opt_search { get; set; }
        public List<Posts> Posts { get; set; }
    }
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignalRContext _context;
        private static HomeSearch model = new HomeSearch();

        public HomeController(ILogger<HomeController> logger, SignalRContext context)
        {
            _logger = logger;
            _context = context;
        }

        private List<string> optList = new List<string>() { "ID", "Title", "Description" };

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> Index()
        {
            ViewData["SearchOpts"] = new SelectList(optList);

            //switch (model.opt_search)
            //{
            //    case "ID":
            //        model.Posts = _context.Posts.Include(p => p.Category).Include(p => p.User).Where(p => p.PostID == int.Parse(model.SearchValue)).ToList();
            //        break;
            //    case "Title":
            //        model.Posts = _context.Posts.Include(p => p.Category).Include(p => p.User).Where(p => p.Title.Contains(model.SearchValue)).ToList();
            //        break;
            //    case "Description":
            //        model.Posts = _context.Posts.Include(p => p.Category).Include(p => p.User).Where(p => p.Content.Contains(model.SearchValue)).ToList();
            //        break;
            //    default:
            //        //model.Posts = _context.Posts.Include(p => p.Category).Include(p => p.User).ToList();
            //        model.Posts = _context.Posts.ToList();
            //        break;
            //}

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult SearchPosts(Posts posts)
        {

            return View(posts);
        }
    }
}