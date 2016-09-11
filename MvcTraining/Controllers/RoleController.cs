using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MvcTraining.Models;
using MvcTraining.Models.View;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MvcTraining.Controllers
{
    [Authorize(Roles ="Admin")]
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
            ViewData["Roles"] = _roleManager.Roles.ToList();
            foreach (var user in users)
            {
                var currentUserRoleNames = _userManger.GetRoles(user.Id)
                                        .ToArray();

                var currentUserRoleIds = _roleManager.Roles
                                        .Where(r => r.Users.Where(u => u.UserId == user.Id).FirstOrDefault() != null)
                                        .Select(r => r.Id)
                                        .ToArray();

                viewModel.Add(new EditUserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    UserRolesNames = currentUserRoleNames,
                    UserRolesIds = currentUserRoleIds
                });
            }

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Users(List<EditUserRoleViewModel> usersRoles)
        {
            var allRolesName = _roleManager.Roles.Select(r => r.Name).ToList();

            foreach(var userRolesInfo in usersRoles)
            {
                foreach (var roleName in allRolesName)
                    if (userRolesInfo.UserRolesNames.Contains(roleName))
                        _userManger.AddToRole(userRolesInfo.UserId, roleName);
                    else
                        _userManger.RemoveFromRole(userRolesInfo.UserId, roleName);
            }

            return RedirectToAction("Users"); 
        }
    }
}