(function () {
	"use strict";

	function bleImageController($scope, mediaResource, imageUrlGeneratorResource) {
		var vm = this;
		vm.content = $scope.block.data;
		vm.imageUrl = "";
		vm.alternativeText = $scope.block.data.alternativeText;

		cropImage();
		//$watch(watchExpression, listener, [objectEquality]);
		$scope.$watch("block.data", function () {

			vm.content = $scope.block.data;
			vm.alternativeText = $scope.block.data.alternativeText;

			cropImage();

		}, true);

		function cropImage() {
			if (vm.content.image && vm.content.image.length > 0) {
				mediaResource.getById(vm.content.image[0].mediaKey)
					.then((media) => {
						imageUrlGeneratorResource.getCropUrl(media.mediaLink, 200, 200)
							.then((result) => {

								vm.imageUrl = result;
							})
					})
			}
			else {

				vm.imageUrl = "";
			}
		}
	}

	angular.module("umbraco").controller("bleImageController", bleImageController);
})();