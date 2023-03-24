using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmbracoTutorial.Core.Models
{
    public record ProductUpdateItem
    {
        public string? ProductName { get; set; }

        public decimal? Price { get; set; }

        public List<string>? Categories { get; set; }

        public string? Description { get; set; }

        public string? SKU { get; set; }

        public string? PhotoFileName { get; set; }

        public string? Photo { get; set; }
    }
}
