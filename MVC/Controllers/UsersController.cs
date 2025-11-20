using APP.Models;
using APP.Domain;
using CORE.APP.Services.MVC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace MVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly IService<UserRequest, UserResponse> _userService;
        private readonly IService<GroupRequest, GroupResponse> _groupService;
        private readonly IService<RoleRequest, RoleResponse> _roleService;

        public UsersController(
            IService<UserRequest, UserResponse> userService,
            IService<GroupRequest, GroupResponse> groupService,
            IService<RoleRequest, RoleResponse> roleService)
        {
            _userService = userService;
            _groupService = groupService;
            _roleService = roleService;
        }

        // GET: Users
        public IActionResult Index()
        {
            var list = _userService.List();
            return View(list);
        }

        // GET: Users/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            var user = _userService.Item(id.Value);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["GroupId"] = new SelectList(_groupService.List(), "Id", "Title");
            ViewData["RoleIds"] = new MultiSelectList(_roleService.List(), "Id", "Name");
            ViewData["Gender"] = new SelectList(Enum.GetValues(typeof(Genders)));
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UserRequest user)
        {
            if (ModelState.IsValid)
            {
                var result = _userService.Create(user);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }
            ViewData["GroupId"] = new SelectList(_groupService.List(), "Id", "Title", user.GroupId);
            ViewData["RoleIds"] = new MultiSelectList(_roleService.List(), "Id", "Name", user.RoleIds);
            ViewData["Gender"] = new SelectList(Enum.GetValues(typeof(Genders)), user.Gender);
            return View(user);
        }

        // GET: Users/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var user = _userService.Edit(id.Value);
            if (user == null)
                return NotFound();

            ViewData["GroupId"] = new SelectList(_groupService.List(), "Id", "Title", user.GroupId);
            ViewData["RoleIds"] = new MultiSelectList(_roleService.List(), "Id", "Name", user.RoleIds);
            ViewData["Gender"] = new SelectList(Enum.GetValues(typeof(Genders)), user.Gender);
            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UserRequest user)
        {
            if (ModelState.IsValid)
            {
                var result = _userService.Update(user);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }
            ViewData["GroupId"] = new SelectList(_groupService.List(), "Id", "Title", user.GroupId);
            ViewData["RoleIds"] = new MultiSelectList(_roleService.List(), "Id", "Name", user.RoleIds);
            ViewData["Gender"] = new SelectList(Enum.GetValues(typeof(Genders)), user.Gender);
            return View(user);
        }

        // GET: Users/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var user = _userService.Item(id.Value);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var result = _userService.Delete(id);
            TempData["Message"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}