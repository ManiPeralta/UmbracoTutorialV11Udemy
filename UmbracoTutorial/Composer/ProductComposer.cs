using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Web.Common.ApplicationBuilder;
using UmbracoTutorial.Core.Services;
using UmbracoTutorial.Mapping;
using UmbracoTutorial.Repository;

namespace UmbracoTutorial.Composer
{
    public class ProductComposer : IComposer
    {
        // example.com/product/1
        public void Compose(IUmbracoBuilder builder)
        {
            builder.Services.AddScoped<IProductService, ProductService>();

            builder.Services.Configure<UmbracoPipelineOptions>(options =>
            {
                options.AddFilter(new UmbracoPipelineFilter(
                    "Product integration",
                    applicationBuilder => { },
                    applicationBuilder => { },
                    applicationBuilder =>
                    {
                        applicationBuilder.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllerRoute(
                                "Product custom route",
                                "Product/{id}",
                                new
                                {
                                    Controller = "Product",
                                    Action = "Details"
                                }
                            );
                        });
                    }
                 ));
            });

            builder.Services.AddScoped<IProductRepository, ProductRepository>();

            builder.WithCollectionBuilder<MapDefinitionCollectionBuilder>()
                .Add<ProductMapping>();
        }
    }
}
