using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MvcTraining.Models;
using MvcTraining.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcTraining.Controllers
{
    public class RoleController : Controller
    {
        ApplicationDbContext _dbContext;
        RoleManager<IdentityRole> _roleManager;
        UserManager<ApplicationUser> _userManger;

        public RoleController()
        {
            _dbContext = new ApplicationDbContext();
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_dbContext));
            _userManger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_dbContext));
        }

        // GET: Role
        public ActionResult Index()
        {
            var roles = _dbContext.Roles.Select(r => new RoleViewModel { Id = r.Id, Name = r.Name }).ToList();
            return View(roles);
        }

        // POST: Create
        [HttpPost]
        public string Create(string roleName)
        {
            string result;

            if (!_roleManager.RoleExists(roleName))
            {
                var role = new IdentityRole();
                role.Name = roleName;
                _roleManager.Create(role);
                result = "Role has been created";
            }
            else
            {
                result = $"{roleName} role is already exists";
            }

            return result;
        }

        [HttpGet]
        public ActionResult Edit(string roleId)
        {
            var role = _dbContext.Roles.Where(r => r.Id == roleId).Select(r => new RoleViewModel { Id = r.Id, Name = r.Name }).FirstOrDefault();
            return View(role);
        }

        [HttpPost]
        public ActionResult Edit(RoleViewModel role)
        {
            var oldRole = _roleManager.Roles.Where(r => r.Id == role.Id).FirstOrDefault();
            oldRole.Name = role.Name;
            _roleManager.Update(oldRole);
            return RedirectToAction("Index");
        }

        public ActionResult Users()
        {
            var roles = _roleManager.Roles.ToList();

            var users = _userManger.Users.ToList();
            List<EditUserRoleViewModel> viewModel = new List<EditUserRoleViewModel>();
            ViewData["Roles"] = new MultiSelectList(_roleManager.Roles.ToList(), "Id", "Name");
            foreach (var user in users)
            {
                var currentUserRoles = _roleManager.Roles
                                                   .Where(r => 
                                                            user.Roles.Count(userRole => userRole.RoleId == r.Id) 
                                                            > 0)
                                                   .Select(r => r.Id).ToArray();

                viewModel.Add(new EditUserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Roles = currentUserRoles
                });
            }

            return View(viewModel);
        }

        [HttpPost]
        public string Users(List<EditUserRoleViewModel> users)
        {
            if(users == null)
            {
                return "Users is null";
            }

            return "Something is there";
        }
    }
}