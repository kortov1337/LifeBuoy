using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lifebuoy.Models
{
    public class SkinDetails
    {
        public int Id { get; set; }
        public int OfferId { get; set; }
        public string photoDisplayMode { get; set; }
        public string background { get; set; }
        public string hotOffer { get; set; }
        public string hightlight { get; set; }
    }
}