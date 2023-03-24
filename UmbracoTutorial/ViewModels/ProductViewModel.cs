using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace UmbracoTutorial.ViewModels
{
    public class ProductViewModel : ContentModel
    {
        public ProductViewModel(IPublishedContent? content) : base(content)
        {
        }

        public string ProductName { get; set; }
    }
}
