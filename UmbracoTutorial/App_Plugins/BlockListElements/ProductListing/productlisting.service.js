function productListingService(productListingResource) {
    return {

        getProducts: function (number) {
            return productListingResource.getProducts(number).then(function (data) {
                return data;
            }, function () { });

        }
    }
}

angular.module("umbraco.services").factory("productListingService", productListingService)