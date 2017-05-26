using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lifebuoy.Models
{
    public class UserListView
    {
        public IEnumerable<ApplicationUser> Users { get; set; }
        public SelectList Roles { get; set; }
    }
}