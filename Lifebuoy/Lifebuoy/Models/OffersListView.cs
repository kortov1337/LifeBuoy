using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lifebuoy.Models
{
    public class OffersListView
    {
        public IEnumerable<Offers> Offers { get; set; }
        public SelectList Categories { get; set; }
        public SelectList Providers { get; set; }
        public SelectList Cities { get; set; }
    }
}