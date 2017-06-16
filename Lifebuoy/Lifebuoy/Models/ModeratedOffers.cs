using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Lifebuoy.Models
{
    public class ModeratedOffers
    {
        public int Id { get; set; }
        public int OfferId { get; set; }
        [Display(Name = "Модерировано")]
        public bool isModerated { get; set; }
    }
}