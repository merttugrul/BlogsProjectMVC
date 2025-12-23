using APP.Models;
using CORE.APP.Services.MVC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    [Authorize]
    public class TagsController : Controller
    {
        private readonly IService<TagRequest, TagResponse> _tagService;

        public TagsController(IService<TagRequest, TagResponse> tagService)
        {
            _tagService = tagService;
        }

        // GET: Tags
        public IActionResult Index()
        {
            var list = _tagService.List();
            return View(list);
        }

        // GET: Tags/Details/5
        public IActionResult Details(int id)
        {
            var item = _tagService.Item(id);
            if (item == null)
                return NotFound();
            return View(item);
        }

        // GET: Tags/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tags/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TagRequest tag)
        {
            if (ModelState.IsValid)
            {
                var result = _tagService.Create(tag);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }
            return View(tag);
        }

        // GET: Tags/Edit/5
        public IActionResult Edit(int id)
        {
            var item = _tagService.Edit(id);
            if (item == null)
                return NotFound();
            return View(item);
        }

        // POST: Tags/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TagRequest tag)
        {
            if (ModelState.IsValid)
            {
                var result = _tagService.Update(tag);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }
            return View(tag);
        }

        // GET: Tags/Delete/5
        public IActionResult Delete(int id)
        {
            var item = _tagService.Item(id);
            if (item == null)
                return NotFound();
            return View(item);
        }

        // POST: Tags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var result = _tagService.Delete(id);
            TempData["Message"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}