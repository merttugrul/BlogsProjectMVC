using APP.Models;
using CORE.APP.Services.MVC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IService<BlogRequest, BlogResponse> _blogService;
        private readonly IService<UserRequest, UserResponse> _userService;
        private readonly IService<TagRequest, TagResponse> _tagService;

        public BlogsController(
            IService<BlogRequest, BlogResponse> blogService,
            IService<UserRequest, UserResponse> userService,
            IService<TagRequest, TagResponse> tagService)
        {
            _blogService = blogService;
            _userService = userService;
            _tagService = tagService;
        }

        // GET: Blogs
        public IActionResult Index()
        {
            var list = _blogService.List();
            return View(list);
        }

        // GET: Blogs/Details/5
        public IActionResult Details(int id)
        {
            var item = _blogService.Item(id);
            if (item == null)
                return NotFound();
            return View(item);
        }

        // GET: Blogs/Create
        public IActionResult Create()
        {
            ViewBag.Users = new SelectList(_userService.List(), "Id", "UserName");
            ViewBag.Tags = new MultiSelectList(_tagService.List(), "Id", "Name");
            return View();
        }

        // POST: Blogs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BlogRequest blog)
        {
            if (ModelState.IsValid)
            {
                var result = _blogService.Create(blog);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }
            ViewBag.Users = new SelectList(_userService.List(), "Id", "UserName", blog.UserId);
            ViewBag.Tags = new MultiSelectList(_tagService.List(), "Id", "Name", blog.TagIds);
            return View(blog);
        }

        // GET: Blogs/Edit/5
        public IActionResult Edit(int id)
        {
            var item = _blogService.Edit(id);
            if (item == null)
                return NotFound();
            ViewBag.Users = new SelectList(_userService.List(), "Id", "UserName", item.UserId);
            ViewBag.Tags = new MultiSelectList(_tagService.List(), "Id", "Name", item.TagIds);
            return View(item);
        }

        // POST: Blogs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BlogRequest blog)
        {
            if (ModelState.IsValid)
            {
                var result = _blogService.Update(blog);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }
            ViewBag.Users = new SelectList(_userService.List(), "Id", "UserName", blog.UserId);
            ViewBag.Tags = new MultiSelectList(_tagService.List(), "Id", "Name", blog.TagIds);
            return View(blog);
        }

        // GET: Blogs/Delete/5
        public IActionResult Delete(int id)
        {
            var item = _blogService.Item(id);
            if (item == null)
                return NotFound();
            return View(item);
        }

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var result = _blogService.Delete(id);
            TempData["Message"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}