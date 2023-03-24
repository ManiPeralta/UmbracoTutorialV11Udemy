function productListingResource($http, umbRequestHelper) {
    return {
        getProducts: function (number) {
            return umbRequestHelper.resourcePromise(
                $http.get("/umbraco/backoffice/api/ProductListing/GetProducts?number=" + number), "Failed to retrieve products"
            );
        }
    }
}

angular.module("umbraco.services").factory("productListingResource", productListingResource)