
function CuentaBancaria() {
    this.codigo = ko.observable(0);
    this.banco = ko.observable();
    this.monedatext = ko.observable();

    this.moneda = ko.observable();
    this.tipoCuenta = ko.observable();
    this.tipoCuentaText = ko.observable();
    this.nroCuenta = ko.observable();
    this.nroCCI = ko.observable();
    this.logobanco = ko.observable();



}

var ViewModel = function () {

    var self = this;
    this.cuentaRemoveContext;
    
    this.tiposMoneda = ko.observableArray([]);
    this.cuentasBancarias = ko.observableArray([]);
    this.lstBancos = ko.observableArray([]);
    this.lstTipoCuentaBancaria = ko.observableArray([]);
    $('#banco').addClass('active');

    this.init = function () {

        var data = {
            sid: window.sid
        };

        PostJson("registro/getdatosBancarios-Registro", data, false).done(function (result) {
            if (result.success == true) {

                for (var i = 0; i < result.data.length; i++) {
                    var cuenta = new CuentaBancaria();

                    var dato = result.data[i];
                    cuenta.codigo(dato.codigo);
                    cuenta.banco(dato.banco);
                    cuenta.logobanco(dato.logo);
                    cuenta.moneda(dato.moneda);
                    cuenta.monedatext(dato.monedatext);
                    cuenta.tipoCuenta(dato.tipoCuenta);
                    cuenta.tipoCuentaText(dato.tipoCuentaText);
                    cuenta.nroCuenta(dato.nroCuenta);
                    cuenta.nroCCI(dato.nroCCI);


                    self.cuentasBancarias.push(cuenta);
                }

            } else {
                toastr.error(result.error);
                return false;
            }
        });

    };

};

var modelo = new ViewModel();




$(function () {


    modelo.init();
    ko.applyBindings(modelo);



});