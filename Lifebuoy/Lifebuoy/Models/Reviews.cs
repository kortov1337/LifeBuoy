using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lifebuoy.Models
{
    public class Reviews
    {
        public int Id { get; set; }
        public int OfferId { get; set; }
        public string UserId { get; set; }
        public string Author { get; set; }
        public string Review { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
    }
}