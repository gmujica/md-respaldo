//function CuentaBancariaModel() {
//    var self = this;

//    this.banco = ko.observable('').extend({ required: { params: true, message: "Campo Obligatorio" } });

//    this.banco.subscribe(function (val) {

//        if (val) {
//            $("#cuenta_bancaria").val('');
//            $("#cuenta_cci").val();
//            $("#cuenta_bancaria").inputmask(val.formatoNroCuenta, {});
//            $("#cuenta_cci").inputmask(val.formatoCCI, {});

//        } else {
//            $("#cuenta_bancaria").val('');
//            $("#cuenta_bancaria").inputmask('remove');
//            $("#cuenta_cci").inputmask('remove');
//        }

//    });

//    this.nroCuenta = ko.observable().extend({
//        required: { params: true, message: "Campo Obligatorio" },
//        isNumeric: true
//    });
//    this.nroCCI = ko.observable().extend({
//        required: { params: true, message: "Campo Obligatorio" },
//        isNumeric: true
//    });
//    this.moneda = ko.observable().extend({
//        required: { params: true, message: "Campo Obligatorio" }
//    });
//    this.tipoCuenta = ko.observable('').extend({
//        required: { params: true, message: "Campo Obligatorio" }
//    });

//    this.validationResult = null;
//    this.validarModelo = function () {
//        self.validationResult = ko.validation.group(this);
//        if (self.validationResult().length === 0) {
//            return true;
//        } else {
//            self.validationResult.showAllMessages();
//            return false;
//        }
//    };

//}

//function CuentaBancaria() {

//    var self = this;
//    this.codigo = ko.observable(0);
//    this.banco = ko.observable();
//    this.monedatext = ko.observable();

//    this.moneda = ko.observable();
//    this.tipoCuenta = ko.observable();
//    this.tipoCuentaText = ko.observable();
//    this.nroCuenta = ko.observable();
//    this.nroCCI = ko.observable();
//    this.estado = ko.observable(1);
//    this.logo = ko.observable();

    

//}

//var ViewModel = function () {

//    var self = this;
//    this.cuentaRemoveContext;
//    this.tiposMoneda = ko.observableArray([]);
//    this.cuentasBancarias = ko.observableArray([]);
//    this.lstBancos = ko.observableArray([]);
//    this.lstTipoCuentaBancaria = ko.observableArray([]);
//    this.cunetaBanco = new CuentaBancariaModel();

//    this.init = function () {
//        PostJson("combo/tipoMoneda", null).done(function (result) {

//            if (result.success == true) {

//                for (var i = 0; i < result.data.length; i++) {
//                    self.tiposMoneda.push(result.data[i]);
//                }
//            } else {
//                toastr.error(result.error);
//            }

//        });

//        PostJson("common/entidadesFinacieras", null).done(function (result) {

//            if (result.success == true) {

//                self.lstBancos(result.data);
//                $("#combo_Bancos").trigger('chosen:updated');

//            } else {
//                toastr.error(result.error);
//                return false;
//            }

//        });

//        PostJson("common/tipoCuentaBancarias", null).done(function (result) {

//            if (result.success == true) {
//                self.lstTipoCuentaBancaria(result.data);
//            } else {
//                toastr.error(result.error);
//                return false;
//            }


//        });

//        self.getCuentasBancarias();
//    };
 
//    this.openRemove = function (data) {
//        $("#remove_cuenta").removeClass("hidden");
//        self.cuentaRemoveContext = data;
//    };

//    this.removeCuentaBancaria = function () {
//        self.cuentaRemoveContext.estado(9);
//        $("#remove_cuenta").addClass("hidden");
//        self.cuentaRemoveContext = null;
//    };

//    this.addCuenta = function () {

//        if (!self.cunetaBanco.validarModelo()) {
//            return false;
//        }

//        var cuenta = new CuentaBancaria();

//        cuenta.banco(self.cunetaBanco.banco().codigo);
//        cuenta.logo(self.cunetaBanco.banco().icon);
//        cuenta.moneda(self.cunetaBanco.moneda().value);
//        cuenta.monedatext(self.cunetaBanco.moneda().text);
//        cuenta.tipoCuenta(self.cunetaBanco.tipoCuenta().value);
//        cuenta.tipoCuentaText(self.cunetaBanco.tipoCuenta().text);
//        cuenta.nroCuenta(self.cunetaBanco.nroCuenta());
//        cuenta.nroCCI(self.cunetaBanco.nroCCI());


//        self.cuentasBancarias.push(cuenta);

//        self.cunetaBanco.banco(undefined);
//        self.cunetaBanco.tipoCuenta(undefined);
//        self.cunetaBanco.nroCuenta('');
//        self.cunetaBanco.nroCCI('');
//        self.cunetaBanco.moneda(undefined);

//        $("#combo_Bancos").trigger('chosen:updated');
//        if (self.cunetaBanco.validationResult != null)
//            self.cunetaBanco.validationResult.showAllMessages(false);

//    };

//    this.guardar = function () {


//        if (self.cuentasBancarias().length < 1) {
//            toastr.warning("No se tienen cuentas bancarias para evaluar.");
//            return false;
//        }

//        var error_tarjetas = '';

//        for (var i = 0; i < self.cuentasBancarias().length; i++) {
//            var cuenta = self.cuentasBancarias()[i];
//            if (cuenta.moneda() == 'PEN')
//            { error_tarjetas = error_tarjetas + 'S' }

//            if (cuenta.moneda() == 'USD')
//            { error_tarjetas = error_tarjetas + 'D' }

//        }

//        if (error_tarjetas.indexOf("SD") < 0) {
//            toastr.warning("Debe ingresar como mínimo una cuenta bancaria en Soles y otra en Dólares, para poder continuar.");
//            return false;
//        }


//        var model = ko.toJS(self.cuentasBancarias);

//        PostJson("cliente/update-mis-datosBancarios", model).done(function (result) {

//            if (result.success == true) {
//                self.getCuentasBancarias();
//                toastr.success("Cuentas bancarias actualizadas correctamento");
//                return false;
//            }
//            else {
//                toastr.error(result.error);
//            }
//        });

//    };

//    this.getCuentasBancarias = function ()
//    {
//        PostJson("cliente/mis-datosBancarios", null).done(function (result) {
//            if (result.success == true) {
//                self.cuentasBancarias.removeAll();
//                for (var i = 0; i < result.data.length; i++) {

//                    var item = result.data[i];

//                    var cuenta = new CuentaBancaria();
//                    cuenta.codigo(item.codigo);
//                    cuenta.logo(item.logo);
//                    cuenta.banco(item.banco);
//                    cuenta.moneda(item.moneda);
//                    cuenta.monedatext(item.monedatext);
//                    cuenta.tipoCuenta(item.tipoCuenta);
//                    cuenta.tipoCuentaText(item.tipoCuentaText);
//                    cuenta.nroCuenta(item.nroCuenta);
//                    cuenta.nroCCI(item.nroCCI);
//                    cuenta.estado(item.estado);

//                    self.cuentasBancarias.push(cuenta);

//                }

//            } else {
//                toastr.error(result.error);
//                return false;
//            }

//        });
//    };

//    self.init();

//};

//$(function () {

//    $(".k-choseen").chosen({
//        disable_search: true,
//        allow_single_deselect: true,
//        no_results_text: "Sin resultados para: ",
//        placeholder_text_single: "--Seleccionar--"
//    });
//    ko.applyBindings(new ViewModel());

//});



function CuentaBancariaModel() {
    var self = this;

    this.banco = ko.observable('').extend({ required: { params: true, message: "Campo Obligatorio" } });

    this.banco.subscribe(function (val) {

        if (val) {
            $("#cuenta_bancaria").val('');
            //$("#cuenta_cci").val();
            $("#cuenta_bancaria").inputmask(val.formatoNroCuenta, {});
            //$("#cuenta_cci").inputmask(val.formatoCCI, {});

        } else {
            $("#cuenta_bancaria").val('');
            $("#cuenta_bancaria").inputmask('remove');
            //$("#cuenta_cci").inputmask('remove');
        }

    });

    this.nroCuenta = ko.observable('').extend({
        required: { params: true, message: "Campo Obligatorio" },
        isNumeric: true
    });
    //this.nroCCI = ko.observable().extend({
    //    required: { params: true, message: "Campo Obligatorio" },
    //    isNumeric: true
    //});
    this.moneda = ko.observable().extend({
        required: { params: true, message: "Campo Obligatorio" }
    });
    this.tipoCuenta = ko.observable().extend({
        required: { params: true, message: "Campo Obligatorio" }
    });

    this.validationResult = null;
    this.validarModelo = function () {
        self.validationResult = ko.validation.group(this);
        if (self.validationResult().length === 0) {
            return true;
        } else {
            self.validationResult.showAllMessages();
            return false;
        }
    };

}

function CuentaBancaria() {
    var self = this;
    this.codigo = ko.observable(0);
    this.banco = ko.observable();
    this.monedatext = ko.observable();
    this.moneda = ko.observable();
    this.tipoCuenta = ko.observable();
    this.tipoCuentaText = ko.observable();
    this.nroCuenta = ko.observable();
    this.nroCCI = ko.observable();
    this.estado = ko.observable(1);
    this.logo = ko.observable();
    this.estadoNombre = ko.observable();
}


function ViewModel() {

    var self = this;

    this.lstOrigenFondos = ko.observableArray([]);
    this.lsttiposMoneda = ko.observableArray([]);
    this.lstTipoCuentaBancaria = ko.observableArray([]);
    this.lstBancos = ko.observableArray([]);
    this.cuentaBanco = new CuentaBancariaModel();
    this.cuentasBancarias = ko.observableArray([]);
    this.cuentasBancariasBd = ko.observableArray([]);
    this.verAgregarCuenta = ko.observable();
    this.cuentaRemoveContext;
    $('#banco').addClass('active');
    this.init = function () {

        PostJson("listar-datos-cuentas-bancarias_cliente", null).done(function (resultado) {
            debugger;
            self.cuentasBancarias.removeAll();
            self.cuentasBancariasBd.removeAll();
            if (resultado.success == true) {

                var datos = ko.toJS(resultado.data);
                datos.cuentas.forEach(function (elemento) {
                    var cuenta = new CuentaBancaria();
                    cuenta.codigo(elemento.codigo);
                    cuenta.banco(elemento.banco);
                    cuenta.moneda(elemento.moneda);
                    cuenta.monedatext(elemento.monedatext);
                    cuenta.tipoCuenta(elemento.tipoCuenta);
                    cuenta.tipoCuentaText(elemento.tipoCuentaText);
                    cuenta.nroCuenta(elemento.nroCuenta);
                    cuenta.logo(elemento.logo);
                    cuenta.estadoNombre(elemento.estadoNombre);
                    if (elemento.estado == 3) {
                        self.verAgregarCuenta(elemento.estado);
                    }
                    
                    
                    self.cuentasBancarias.push(cuenta);
                    self.cuentasBancariasBd.push(cuenta);
                });

            } else {
                toastr.error(resultado.error);
                return false;
            }


        });


        PostJson("combo/tipoMoneda", null).done(function (result) {
            self.lsttiposMoneda(result.data);
        });

        PostJson("common/tipoCuentaBancarias", null).done(function (result) {
            self.lstTipoCuentaBancaria(result.data);
        });

        PostJson("common/entidadesFinacieras", null).done(function (result) {
            if (result.success == true) {
                self.lstBancos(result.data);
                $("#combo_Bancos").trigger('chosen:updated');
            } else {
                toastr.error(result.error);
                return false;
            }

        });


    }

    this.addCuenta = function () {
        debugger;
        if (!self.cuentaBanco.validarModelo()) {
            return false;
        }

        var cuenta = new CuentaBancaria();
        cuenta.codigo(0);
        cuenta.banco(self.cuentaBanco.banco().codigo);
        cuenta.logo(self.cuentaBanco.banco().icon);
        cuenta.moneda(self.cuentaBanco.moneda().value);
        cuenta.monedatext(self.cuentaBanco.moneda().text);
        cuenta.tipoCuenta(self.cuentaBanco.tipoCuenta().value);
        cuenta.tipoCuentaText(self.cuentaBanco.tipoCuenta().text);
        cuenta.nroCuenta(self.cuentaBanco.nroCuenta());
        cuenta.estadoNombre('Por Verificar');
        //cuenta.nroCCI(self.cuentaBanco.nroCCI());


        self.cuentasBancarias.push(cuenta);

        self.cuentaBanco.banco(undefined);
        self.cuentaBanco.tipoCuenta(undefined);
        self.cuentaBanco.nroCuenta('');
        //self.cuentaBanco.nroCCI('');
        self.cuentaBanco.moneda(undefined);

        $("#combo_Bancos").trigger('chosen:updated');
        if (self.cuentaBanco.validationResult != null)
            self.cuentaBanco.validationResult.showAllMessages(false);

    };

    this.openRemove = function (data) {
        debugger;
        $("#remove_cuenta").removeClass("hidden");
        self.cuentaRemoveContext = data;
    };

    this.removeCuentaBancaria = function () {
        debugger;
        var indice = 0;
        self.cuentasBancariasBd().forEach(function (elemento) {
            debugger;
            if (elemento.codigo() == self.cuentaRemoveContext.codigo()) {
                self.cuentaRemoveContext.estado(9);
                indice = 1;
            }
        });
        if (indice == 0) {
            self.cuentasBancarias.remove(self.cuentaRemoveContext);
        }


        console.log(ko.toJS(self.cuentasBancarias));
        $("#remove_cuenta").addClass("hidden");
        self.cuentaRemoveContext = null;
    };

    this.modificarCuentas = function () {
        debugger;
        var indice = 0;
        self.cuentasBancarias().forEach(function (elemento) {
            if (elemento.estado() == 1) {
                indice += 1;
            }

        });

        if (indice == 0) {
            toastr.warning("Debe Ingresar al menos una Cuenta Bancaria");
            return false;
        }
        debugger;
        var cuentasBancarias = ko.toJS(self.cuentaBanco);

        if (cuentasBancarias.nroCuenta != "") {
            toastr.warning("Primero debe agregar cuenta");
            return false;
        }

        var data = {
            cuentas: ko.toJS(self.cuentasBancarias)
        };

        //16-06-2019
        PostJson("modificar-cuentas-bancarias-cliente", data).done(function (resultado) {
            debugger;
            if (resultado.success == true) {
                self.init();
                if (resultado.other == "9") {
                    toastr.success('Se eliminó cuentas bancarias correctamente');
                }
                if (resultado.other == "1") {
                    toastr.success('Se ha registrado cuentas bancarias, se procederá a validar');
                }
                

            } else {
                toastr.error(resultado.error);
                return false;
            }


        });
    }

};

var modelo = new ViewModel();

$(function () {


    modelo.init();
    ko.applyBindings(modelo);

});