ko.extenders.isNumeric = function (target, option) {

    var result = '';
    target.subscribe(function (newValue) {

        result = newValue;
        result = result.replace(/[^0-9\.]/g, '');
        target(result);
    });

    return target;
};


var GridModelSP_Dinamico = function (url, page, len) {
    var self = this;
    this.datasource = ko.observableArray([]);
    this.paging = ko.observable();
    this.offset = ko.observable(0);
    this.limit = ko.observable(0);
    this.pageIndex = ko.observable(page);
    this.itemPerPage = ko.observable(len);
    self.paging.paging = ko.observable();
    this.total = ko.observable(0);
    this.showPreviousPage = ko.observable(false);
    this.showNextPage = ko.observable(false);
    this.filter = {};
    this.indexActual = ko.observable(0);
    var getDatasource = function (page, itemPerPage, idFilter, textFilter, searchFilter) {

        PostJson(url, { pageIndex: page, itemPerPage: itemPerPage, idFilter: idFilter, textFilter: textFilter, searchFilter: searchFilter }, false).done(function (response) {

            //var total = Math.ceil(response.total / response.itemperpage);
            self.pageIndex = ko.observable(response.page);
            self.itemPerPage = ko.observable(response.itemperpage)
            self.limit(response.limit);
            self.offset(response.offset);
            self.total(response.total);


            var paging = new ko.mapping.fromJS(response);

            paging.NextPage = function () {

                if (self.pageIndex() >= self.paging().PageCount()) return;
                self.indexActual(self.pageIndex());
                if (self.filter) {
                    getDatasource(self.pageIndex(), self.itemPerPage(), self.filter.idFilter, self.filter.textFilter, self.filter.searchFilter);
                }
                else {
                    getDatasource(self.pageIndex(), self.itemPerPage());

                }

            };

            paging.Previus = function () {
                if (self.pageIndex() <= 1) return;
                self.indexActual(self.pageIndex() - 2);
                if (self.filter) {
                    getDatasource(self.pageIndex() - 2, self.itemPerPage(), self.filter.idFilter, self.filter.textFilter, self.filter.searchFilter);
                }
                else {
                    getDatasource(self.pageIndex() - 2, self.itemPerPage());
                }

            };

            paging.Pagination = function (obj) {

                var page = obj ? obj.Key() : 1;
                self.indexActual(page - 1);
                if (self.filter)
                    getDatasource(page - 1, self.itemPerPage(), self.filter.idFilter, self.filter.textFilter, self.filter.searchFilter);
                else
                    getDatasource(page - 1, self.itemPerPage());
            };

            self.paging(paging);

            var datalist = response.data.map(function (item) {
                var data = new ko.mapping.fromJS(item);
                return data;
            });
            self.datasource(datalist);

        });



    }

    this.search = function (filter) {
        self.pageIndex(0);
        self.filter = filter;
        getDatasource(self.pageIndex(), self.itemPerPage(), filter.idFilter, filter.textFilter, filter.searchFilter);
    };

    this.mantener = function () {
     
        var pagina = self.indexActual();
        if (pagina != - 1) {
            getDatasource(pagina, self.itemPerPage());
        } else {
            getDatasource(0, self.itemPerPage());
        }

    }
    this.init = function () {
        getDatasource(0);

    };

    this.initIndex = function (filter) {
        self.filter = filter;
        getDatasource(filter.pageIndex, self.itemPerPage(), filter.idFilter, filter.textFilter, filter.searchFilter);
    };

};



$.ajaxSetup({
    global: true
});

$(document).bind("ajaxSend", function () {
    $(".loading-spiner-holder").hide();
}).bind("ajaxComplete", function () {
    $(".loading-spiner-holder").hide();
});