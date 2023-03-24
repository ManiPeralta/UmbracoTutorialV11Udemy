using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmbracoTutorial.Core.Models
{
    public record ProductCreationItem
    {

        [Required]
        public string ProductName { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public List<string> Categories { get; set; }

        [Required]
        public string Description { get; set; }


        public string SKU { get; set; }

        [Required]
        public string PhotoFileName { get; set; }

        [Required]
        public string Photo { get; set; }
    }
}
