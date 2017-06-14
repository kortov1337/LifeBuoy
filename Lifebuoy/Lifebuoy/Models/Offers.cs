using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Lifebuoy.Models
{
    public class Offers
    {
        public int Id { get; set; }

        public string Images { get; set; }

        public int Rating { get; set; }

        public int VotesCount { get; set; }

        [Display(Name = "ID для каталога")]
        public string catalogID { get; set; }              

        [Display(Name = "Изображение в каталоге")]
        public string CatalogImg { get; set; }

        [Display(Name = "Категория")]
        [Required]
        public string Category { get; set; }

        [Display(Name = "Имя провайдера")]
        [Required]
        public string Provider { get; set; }

        [Display(Name = "Название")]
        [Required]
        public string CatalogName { get; set; }

        [Display(Name = "Город")]
        [Required]
        public string City { get; set; }

        [Display(Name = "Адрес")]
        [Required]
        public string Adress { get; set; }

        [Display(Name = "Описание")]
        [Required]
        public string Description { get; set; }

        [Display(Name = "Описание")]
        [Required]
        public string ShortDescription { get; set; }

        [Display(Name = "Рабочие часы")]
        [Required]
        public string WorkingHours { get; set; }

        [Display(Name = "Цены")]
        [Required]  
        public string Prices { get; set; }

        [Display(Name = "Контакты")]
        [Required]
        public string Contacts { get; set; }

        [Display(Name = "Статус")]
        public bool isModerated { get; set; }
    }
}