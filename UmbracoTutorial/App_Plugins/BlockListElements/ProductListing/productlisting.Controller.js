(function () {
    "use strict";

    function productListingController($scope, productListingService) {
        var vm = this;

        vm.title = $scope.block.data.title;
        vm.numberOfProducts = $scope.block?.settingsData?.numberOfProducts;

        vm.products = [];

        getAllProducts();

        $scope.$watch("block.data", function () {

            vm.title = $scope.block.data.title;

        }, true);

        $scope.$watch("block.settingsData", function () {

            vm.numberOfProducts = $scope.block?.settingsData?.numberOfProducts;

            getAllProducts();

        }, true);

        function getAllProducts() {

            productListingService.getProducts(vm.numberOfProducts).then(function (response) {
                vm.products = response;
            }, function (error) { console.log(error); });
        }
    }

    angular.module("umbraco").controller("productListingController", productListingController);
})();