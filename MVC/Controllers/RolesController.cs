using APP.Models;
using CORE.APP.Services.MVC;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class RolesController : Controller
    {
        private readonly IService<RoleRequest, RoleResponse> _roleService;

        public RolesController(IService<RoleRequest, RoleResponse> roleService)
        {
            _roleService = roleService;
        }

        // GET: Roles
        public IActionResult Index()
        {
            var list = _roleService.List();
            return View(list);
        }

        // GET: Roles/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            var role = _roleService.Item(id.Value);
            if (role == null)
                return NotFound();

            return View(role);
        }

        // GET: Roles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RoleRequest role)
        {
            if (ModelState.IsValid)
            {
                var result = _roleService.Create(role);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }
            return View(role);
        }

        // GET: Roles/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var role = _roleService.Edit(id.Value);
            if (role == null)
                return NotFound();

            return View(role);
        }

        // POST: Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(RoleRequest role)
        {
            if (ModelState.IsValid)
            {
                var result = _roleService.Update(role);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }
            return View(role);
        }

        // GET: Roles/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var role = _roleService.Item(id.Value);
            if (role == null)
                return NotFound();

            return View(role);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var result = _roleService.Delete(id);
            TempData["Message"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}