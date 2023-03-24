using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Strings;
using Umbraco.Cms.Core.Web;
using UmbracoTutorial.Core.Models;
using UmbracoTutorial.Core.UmbracoModels;

namespace UmbracoTutorial.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly Guid _productsMediaFolder = Guid.Parse(" 1ad4e9a3-4ad5-476c-aba2-9602e34e323f");

        private readonly IUmbracoContextFactory _umbracoContextFactory;
        private readonly IContentService _contentService;

        private readonly IMediaService _mediaService;

        private readonly MediaFileManager _mediaFileManager;
        private readonly IShortStringHelper _shortStringHelper;
        private readonly MediaUrlGeneratorCollection _mediaUrlGenerator;
        private readonly IContentTypeBaseServiceProvider _contentTypeBaseServiceProvider;

        private readonly IPublishedSnapshotAccessor _publishedSnapshotAccessor;

        public ProductRepository(IUmbracoContextFactory umbracoContextFactory, 
            IContentService contentService,
            IMediaService mediaService,
            MediaFileManager mediaFileManager,
            IShortStringHelper shortStringHelper,
            MediaUrlGeneratorCollection mediaUrlGenerators,
            IContentTypeBaseServiceProvider contentTypeBaseServiceProvider,
            IPublishedSnapshotAccessor publishedSnapshotAccessor)
        {
            _umbracoContextFactory = umbracoContextFactory;
            _contentService = contentService;

            _mediaService = mediaService;
            _shortStringHelper = shortStringHelper;
            _mediaFileManager = mediaFileManager;
            _mediaUrlGenerator = mediaUrlGenerators;
            _contentTypeBaseServiceProvider = contentTypeBaseServiceProvider;
            _publishedSnapshotAccessor = publishedSnapshotAccessor;
        }

        public List<Product> GetProducts(string? productSKU, decimal? maxPrice)
        {
            var products = GetProductRootPage();
            var final = new List<Product>();

            if(products is Products productsRoot)
            {
                var filteredProducts = productsRoot.Children<Product>()?.Where(x => x.IsPublished());

                if (!string.IsNullOrEmpty(productSKU))
                {
                    filteredProducts = filteredProducts?.Where(x => x.Sku.InvariantEquals(productSKU));
                }

                if (maxPrice is decimal MaxPrice)
                {
                    filteredProducts = filteredProducts?.Where(x => x.Price <= MaxPrice);
                }

                final = filteredProducts?.ToList() ?? new List<Product>();
            }
            return final;
        }

        public bool Delete(int Id)
        {
            var product = _contentService.GetById(Id);
            if (product != null) 
            {
                var result =  _contentService.Delete(product);

                return result.Success;
            }

            return false;
        }
        private Products? GetProductRootPage()
        {
            using var cref = _umbracoContextFactory.EnsureUmbracoContext();

            var rootNode = cref?.UmbracoContext?.Content
                ?.GetAtRoot().FirstOrDefault(x => x.ContentType.Alias == Home.ModelTypeAlias);

            return rootNode?.Descendant<Products>();
        }

        public Product Get(int Id)
        {
            using(var cref = _umbracoContextFactory.EnsureUmbracoContext())
            {
                var content = cref?.UmbracoContext?.Content;
                return (Product)content.GetById(Id);
            }
        }

        public Product Create(ProductCreationItem product)
        {
            var productImage = CreateProductImage(product.PhotoFileName, product.Photo);
            if(productImage == null)
            {
                return null;
            }

            var productsRoot = GetProductRootPage();

            var productContent = _contentService.Create(product.ProductName, productsRoot.Key, Product.ModelTypeAlias);

            var productNameAlias = Product.GetModelPropertyType(_publishedSnapshotAccessor, x => x.ProductName).Alias;
            var productPriceAlias = Product.GetModelPropertyType(_publishedSnapshotAccessor, x => x.Price).Alias;
            var productCategoriesAlias = Product.GetModelPropertyType(_publishedSnapshotAccessor, x => x.Category).Alias;
            var productDescriptionAlias = Product.GetModelPropertyType(_publishedSnapshotAccessor, x => x.Description).Alias;
            var productSKUAlias = Product.GetModelPropertyType(_publishedSnapshotAccessor, x => x.Sku).Alias;
            var productPhotoAlias = Product.GetModelPropertyType(_publishedSnapshotAccessor, x => x.Photos).Alias;

            productContent.SetValue(productNameAlias, product.ProductName);
            productContent.SetValue(productPriceAlias, product.Price);
            productContent.SetValue(productCategoriesAlias, string.Join(",", product.Categories));
            productContent.SetValue(productDescriptionAlias, product.Description);
            productContent.SetValue(productSKUAlias, product.SKU);
            productContent.SetValue(productPhotoAlias, productImage);

            _contentService.Save(productContent);

            return Get(productContent.Id);
        }

        private GuidUdi? CreateProductImage(string filename, string photo)
        {
            // Save image to a tmp path
            var tmpFilePath = Path.GetTempFileName();
            using var image = SixLabors.ImageSharp.Image.Load(Convert.FromBase64String(photo));
            image.Save(tmpFilePath, new JpegEncoder());

            // load file into a filestream
            var fileInfo = new FileInfo(tmpFilePath);
            var fileStream = fileInfo.OpenReadWithRetry();
            if(fileStream == null)
            {
                throw new InvalidOperationException("Could not acquire file stream");
            }


            var umbracoMedia = _mediaService.CreateMedia(filename, _productsMediaFolder, Core.UmbracoModels.Image.ModelTypeAlias);
            using (fileStream)
            {
                umbracoMedia.SetValue(_mediaFileManager, _mediaUrlGenerator, _shortStringHelper, _contentTypeBaseServiceProvider, Constants.Conventions.Media.File, filename, fileStream, null, null);
                var result = _mediaService.Save(umbracoMedia);
                if (!result.Success) 
                { 
                    return null; 
                }
            }

            return umbracoMedia.GetUdi();
        }
    }
}
