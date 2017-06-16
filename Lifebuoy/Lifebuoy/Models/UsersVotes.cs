using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lifebuoy.Models
{
    public class UsersVotes
    {
        public int Id { get; set; }
        public int OfferId { get; set; }
        public string UserName { get; set; }     
    }
}