
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
    
    this.cuentaRemoveContext;
  
    this.init = function () {
      
        PostJson("listar-datos-cuentas-bancarias", null).done(function (resultado) {
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

        if (cuentasBancarias.nroCuenta != "" ) {
            toastr.warning("Primero debe agregar cuenta");
            return false;
        }
        
        var data = {
            cuentas: ko.toJS(self.cuentasBancarias)
        };
        

        PostJson("modificar-cuentas-bancarias", data).done(function (resultado) {
            if (resultado.success == true) {
                self.init();
                toastr.success('Se Registrado Cuentas Bancarias');
           
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