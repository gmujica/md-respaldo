ko.bindingHandlers.executeOnEnter = {
    init: function (element, valueAccessor, allBindings, viewModel) {
        var callback = valueAccessor();
        $(element).keypress(function (event) {
            var keyCode = (event.which ? event.which : event.keyCode);
            if (keyCode === 13) {
                callback.call(viewModel);
                return false;
            }
            return true;
        });
    }
};


function TipoCambioGarantisado(rango, mmin, mmax, tcventa, tccompra) {
    var self = this;

    this.rango = ko.observable(rango);
    this.montoMinimo = ko.observable(mmin);
    this.montoMaximo = ko.observable(mmax);
    this.moneda = ko.observable('USD');
    this.tcCompra = ko.observable(tcventa);
    this.tcVenta = ko.observable(tccompra);
    this.spread = ko.computed(function () {
        return (self.tcVenta() - self.tcCompra()).toFixed(3);
    }, this);

}


var ViewModelPartner = function () {

    var self = this;

    this.lstTiposCambio = ko.observableArray([]);

    this.lstTiposCambio_ = ko.observableArray([]);
    this.hashSave = false;

    this.newTipoCambioGarantizado = function (data, e) {

        if (e.keyCode === 13) {
            var mdata = ko.toJS(data);
            var total = self.lstTiposCambio().length;
            for (var i = total ; i > mdata.rango; i--) {
                if (mdata.rango < i) { self.lstTiposCambio.remove(self.lstTiposCambio()[i - 1]); }
            }
            self.lstTiposCambio.push(new TipoCambioGarantisado(mdata.rango + 1, Number(mdata.montoMaximo) + 1, null, 0, 0));
        }


    };

    this.init = function () {



        PostJson("partner/tipoCambio-delDia", null).done(function (result) {

            if (result.success == true) {
                self.llenadoMasivo(result);

            } else {
                toastr.error(result.error);
                return false;
            }

        });

    };
    this.openFileExplorer = function () {
        $("#file").trigger("click");
    };
    this.save = function () {

        var model = ko.toJS(self.lstTiposCambio);

        PostJson("partner/save-tipoCambioDiario", model).done(function (result) {

            if (result.success == true) {


                self.init();
                self.hashSave = false;

            } else {
                toastr.error(result.error);
                return false;
            }

        });


    };
    $("#file").change(function () {

        var e = this;
        var file = e.files[0];

        if (file === undefined) {
            toastr.warning('Debe seleccionar un archivo.');
            return false;
        }

        var form = new FormData();
        form.append('FileUpload', file);

        PostFile("partner/upload-file", form).done(function (result) {

            var input = $("#file");
            input = input.val('').clone(true);
            $("#file").val('');
            if (result.success == true) {

                self.llenadoMasivo(result);
                self.hashSave = true;

            } else {
                toastr.error(result.error);
                return false;
            }

        });

    });

    this.llenadoMasivo = function (result) {
        self.lstTiposCambio.removeAll();
        if (result.data.length > 0)
        {
            var i = 0;
            for (i = 0; i < result.data.length; i++) {
                var tc = result.data[i];
                self.lstTiposCambio.push(new TipoCambioGarantisado(tc.rango, tc.montoMinimo, tc.montoMaximo, tc.tcCompra, tc.tcVenta));

            }

            self.lstTiposCambio.push(new TipoCambioGarantisado((i + 1), Number(result.data[i - 1].montoMaximo) + 1, null, 0, 0));
        } else {
            self.lstTiposCambio.push(new TipoCambioGarantisado(1, 1, null, 0, 0));
        }
       
    }

};

var modelo = new ViewModelPartner();

$(function () {

    modelo.init();
    ko.applyBindings(modelo);

});

