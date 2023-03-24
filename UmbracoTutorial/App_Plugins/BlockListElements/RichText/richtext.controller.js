(function () {
    "use strict";

    function richtextController($scope) {
        var vm = this;

        vm.richTextContent = $scope.block.data.richText;

        vm.backgroundColor = $scope.block?.settingsData?.backgroundColor?.value;

        $scope.$watch("block.data", function () {

            vm.richTextContent = $scope.block.data.richText;

        }, true);

        $scope.$watch("block.settingsData", function () {

            vm.backgroundColor = $scope.block?.settingsData?.backgroundColor?.value;

        }, true);
    }

    angular.module("umbraco").controller("richtextController", richtextController);
})();