using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lifebuoy.Models
{
    public class ProvidersOffers
    {
        public int Id { get; set; }
        public string Owner { get; set; }
        public int OfferId { get; set; }
    }
}