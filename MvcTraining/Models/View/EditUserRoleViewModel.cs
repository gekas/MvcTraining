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
        public ApplicationUser User { get; set; }
        public MultiSelectList Roles { get; set; }
    }
}