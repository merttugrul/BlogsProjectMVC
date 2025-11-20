using APP.Models;
using CORE.APP.Services.MVC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC.Controllers
{
    public class GroupsController : Controller
    {
        private readonly IService<GroupRequest, GroupResponse> _groupService;

        public GroupsController(IService<GroupRequest, GroupResponse> groupService)
        {
            _groupService = groupService;
        }

        // GET: Groups
        public IActionResult Index()
        {
            var list = _groupService.List();
            return View(list);
        }

        // GET: Groups/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            var group = _groupService.Item(id.Value);
            if (group == null)
                return NotFound();

            return View(group);
        }

        // GET: Groups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Groups/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(GroupRequest group)
        {
            if (ModelState.IsValid)
            {
                var result = _groupService.Create(group);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }
            return View(group);
        }

        // GET: Groups/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var group = _groupService.Edit(id.Value);
            if (group == null)
                return NotFound();

            return View(group);
        }

        // POST: Groups/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(GroupRequest group)
        {
            if (ModelState.IsValid)
            {
                var result = _groupService.Update(group);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }
            return View(group);
        }

        // GET: Groups/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var group = _groupService.Item(id.Value);
            if (group == null)
                return NotFound();

            return View(group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var result = _groupService.Delete(id);
            TempData["Message"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}