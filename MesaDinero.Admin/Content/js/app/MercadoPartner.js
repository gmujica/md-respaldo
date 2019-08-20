var ViewModelPartner = function () {

    var self = this;
    this.lstTiposCambio = ko.observableArray([]);
    this.hashSave = false;
    this.lstBancos = ko.observableArray([]);
    this.banco = ko.observable('');
    var data = {
        codBanco: self.banco()
    };

    //var model = ko.toJS(self.empresa);
    debugger;

  




    this.init = function () {
        debugger;

        PostJson("common/entidadesFinacieras", null).done(function (result) {
            if (result.success == true) {
                self.lstBancos(result.data);
                $("#combo_Bancos").trigger('chosen:updated');
            } else {
                toastr.error(result.error);
                return false;
            }

        });


        PostJson("partner-tipoCambioMercado", data).done(function (result) {

            if (result.success == true) {
                self.lstTiposCambio(result.data);

            } else {
                toastr.error(result.error);
                return false;
            }

        });

    };

    this.banco.subscribe(function (val) {
        debugger;
        var datas = {
            codBanco: val
        };

        PostJson("partner-tipoCambioMercado", datas).done(function (result) {

            if (result.success == true) {
                self.lstTiposCambio(result.data);

            } else {
                toastr.error(result.error);
                return false;
            }

        });
    });

    this.openFileExplorer = function () {
        debugger;
        $("#file").trigger("click");
    };
    this.save = function () {
        if (self.hashSave == true) {
            var model = ko.toJS(self.lstTiposCambio);
            debugger;
            PostJson("partner/save-tipoMercadoDiario", model).done(function (result) {

                if (result.success == true) {
                    debugger;

                    self.init();
                    self.hashSave = false;

                } else {
                    toastr.error(result.error);
                    return false;
                }

            });

        }
    };
    $("#file").change(function () {
        debugger;
        var e = this;
        var file = e.files[0];

        if (file === undefined) {
            toastr.warning('Debe seleccionar un archivo.');
            return false;
        }

        var form = new FormData();
        form.append('FileUpload', file);

        PostFile("partner-mercado/upload-file", form).done(function (result) {

            var input = $("#file");
            input = input.val('').clone(true);
            $("#file").val('');
            if (result.success == true) {
                debugger;
                self.lstTiposCambio(result.data);
                self.hashSave = true;

            } else {
                toastr.error(result.error);
                return false;
            }

        });

    });

};

var modelo = new ViewModelPartner();

$(function () {

    modelo.init();
    ko.applyBindings(modelo);

});

