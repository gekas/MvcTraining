using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcTraining.Models.View
{
    public class EditUserRoleViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string[] UserRolesNames { get; set; }
        public string[] UserRolesIds { get; set; }
    }
}