function CuentaBancariaModel()
{
    var self = this;

    this.banco = ko.observable('').extend({ required: { params: true, message: "Campo Obligatorio" } });

    this.banco.subscribe(function (val) {

        if (val)
        {
            $("#cuenta_bancaria").val('');
            $("#cuenta_cci").val();
            $("#cuenta_bancaria").inputmask(val.formatoNroCuenta, {});
            $("#cuenta_cci").inputmask(val.formatoCCI, {});

        } else {
            $("#cuenta_bancaria").val('');
            $("#cuenta_bancaria").inputmask('remove');
            $("#cuenta_cci").inputmask('remove');
        }

    });

    this.nroCuenta = ko.observable().extend({
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
    this.tipoCuenta = ko.observable('').extend({
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

function CuentaBancaria()
{
    this.codigo = ko.observable(0);
    this.banco = ko.observable();
    this.monedatext = ko.observable();

    this.moneda = ko.observable();
    this.tipoCuenta = ko.observable();
    this.tipoCuentaText = ko.observable();
    this.nroCuenta = ko.observable();
    this.nroCCI = ko.observable();
    this.logobanco = ko.observable();

    this.openRemove = function (data) {
        $("#remove_cuenta").removeClass("hidden");
        modelo.cuentaRemoveContext = data;
        //self.estado('D');
    };

}

var ViewModel = function () {

    var self = this;
    this.cuentaRemoveContext;
    this.closeRemoveCuenta = function () {
        $("#remove_cuenta").addClass("hidden");
    };

    this.removeCuentaBancaria = function () {
        self.cuentasBancarias.remove(self.cuentaRemoveContext);
        //self.cuentaRemoveContext.estado("D");
        $("#remove_cuenta").addClass("hidden");
        self.cuentaRemoveContext = null;
    };
        
    this.cunetaBanco = new CuentaBancariaModel();
    this.tiposMoneda = ko.observableArray([]);
    this.cuentasBancarias = ko.observableArray([]);
    this.lstBancos = ko.observableArray([]);
    this.lstTipoCuentaBancaria = ko.observableArray([]);

    this.addCuenta = function () {

        if (!self.cunetaBanco.validarModelo()) {
            return false;
        }
        
        var cuenta = new CuentaBancaria();
       
        cuenta.banco(self.cunetaBanco.banco().codigo);
        cuenta.logobanco(self.cunetaBanco.banco().icon);
        cuenta.moneda(self.cunetaBanco.moneda().value);
        cuenta.monedatext(self.cunetaBanco.moneda().text);
        cuenta.tipoCuenta(self.cunetaBanco.tipoCuenta().value);
        cuenta.tipoCuentaText(self.cunetaBanco.tipoCuenta().text);
        cuenta.nroCuenta(self.cunetaBanco.nroCuenta());
       // cuenta.nroCCI(self.cunetaBanco.nroCCI());


        self.cuentasBancarias.push(cuenta);

        self.cunetaBanco.banco(undefined);
        self.cunetaBanco.tipoCuenta(undefined);
        self.cunetaBanco.nroCuenta('');
        //self.cunetaBanco.nroCCI('');
        self.cunetaBanco.moneda(undefined);

        $("#combo_Bancos").trigger('chosen:updated');
        if (self.cunetaBanco.validationResult != null)
            self.cunetaBanco.validationResult.showAllMessages(false);

    };

    this.guardar = function () {

        debugger;
        if (self.cuentasBancarias().length < 1)
        {
            toastr.warning("No se tienen cuentas bancarias para evaluar.");
            return false;
        }

        var error_tarjetas = '';
       
        for (var i = 0; i < self.cuentasBancarias().length; i++) {
            var cuenta = self.cuentasBancarias()[i];
            if(cuenta.moneda() == 'PEN')
            { error_tarjetas = error_tarjetas + 'S' }

            if(cuenta.moneda() == 'USD')
            { error_tarjetas = error_tarjetas + 'D' }

        }
        var indS = error_tarjetas.indexOf("S");
        var indD = error_tarjetas.indexOf("D");
        if (error_tarjetas.indexOf("S") < 0 || error_tarjetas.indexOf("D") < 0)
        {
            toastr.warning("Debe ingresar como mínimo una cuenta bancaria en Soles y otra en Dólares, para poder continuar.");
            return false;
        }


        var model = {
            sid : window.sid,
            cuentas : ko.toJS(self.cuentasBancarias)
        };

        PostJson("registro/datoBancario", model).done(function (result) {

            if(result.success == true)
            {
                window.location.href = window.urlApp + "/Registro/Verificacion/" + result.data;
                return false;
            }
            else {
                toastr.error(result.error);
            }
        });

    };

    this.init = function () {

    PostJson("combo/tipoMoneda", null,true).done(function (result) {

        if (result.success == true)
        {
                
            for (var i = 0; i < result.data.length; i++) {
                self.tiposMoneda.push(result.data[i]);
            }
        } else {
            toastr.error(result.error);
        }

    });

    PostJson("common/entidadesFinacieras", null,true).done(function (result) {

        if (result.success == true) {

            self.lstBancos(result.data);
            $("#combo_Bancos").trigger('chosen:updated');

        } else {
            toastr.error(result.error);
            return false;
        }

    });

    PostJson("common/tipoCuentaBancarias", null,true).done(function (result) {

        if (result.success == true)
        {
            self.lstTipoCuentaBancaria(result.data);
        } else {
            toastr.error(result.error);
            return false;
        }
            

    });

    var data = {
        sid : window.sid
    };

    PostJson("registro/getdatosBancarios-Registro", data, false).done(function (result) {
        if(result.success == true)
        {

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

    

    $(".k-choseen").chosen({
        disable_search:true,
        allow_single_deselect: true,
        no_results_text: "Sin resultados para: ",
        placeholder_text_single: "--Seleccionar--"
    });

    modelo.init();
    ko.applyBindings(modelo);



});