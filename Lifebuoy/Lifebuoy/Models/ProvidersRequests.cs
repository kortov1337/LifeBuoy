using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lifebuoy.Models
{
    public class ProvidersRequests
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public bool isConfirmed { get; set; }
    }
}