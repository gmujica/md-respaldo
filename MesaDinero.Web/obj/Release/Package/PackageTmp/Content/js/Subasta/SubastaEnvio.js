function EnvioModel() {
    var self = this;

    this.cuentaOrigen = ko.observable().extend({
        required: { params: true, message: "Campo Obligatorio" }
    });
    this.cuentaDestino = ko.observable().extend({
        required: { params: true, message: "Campo Obligatorio" }
    });
    this.codigoOperacion = ko.observable().extend({
        required: { params: true, message: "Campo Obligatorio" }
    });

    this.numeroDocumento = ko.observable();
    this.nombreEmpresa = ko.observable();
    this.codigobanco = ko.observable();
    this.banco = ko.observable();
    this.codigoMoneda = ko.observable();

    this.moneda = ko.observable();
    this.codigoTipoCuenta = ko.observable();
    this.tipoCuenta = ko.observable();
    this.numeroCuenta = ko.observable();
    this.numeroCuentaInter = ko.observable();



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


var ViewModel = function () {

    var self = this;


    this.tiempo = ko.observable('--');
    this.envioContext = new EnvioModel();

    this.fideicomiso;

    //this.init = function () {
       

        PostJson("subasta-datos-fideicomiso?codBanco=", false).done(function (resultado) {
            debugger;
            if (resultado.success == true) {
                var datos = ko.toJS(resultado.data);
                console.log(datos);
                self.envioContext.nombreEmpresa(datos.nombreEmpresa);
                self.envioContext.numeroDocumento(datos.numeroDocumento);
                self.envioContext.codigobanco(datos.codigobanco);
                self.envioContext.banco(datos.banco);
                self.envioContext.codigoMoneda(datos.codigoMoneda);
                self.envioContext.moneda(datos.moneda);
                self.envioContext.codigoTipoCuenta(datos.codigoTipoCuenta);
                self.envioContext.tipoCuenta(datos.tipoCuenta);
                self.envioContext.numeroCuenta(datos.numeroCuenta);
                self.envioContext.numeroCuentaInter(datos.numeroCuentaInter);
            } else {
                toastr.error(resultado.error);
                return false;
            }
        });


    //}
        self.envioContext.cuentaOrigen.subscribe(function (val) {
            if (val) {
                debugger;
                //PostJson("common/getDepartatmentoXPais/" + val, null).done(function (result1) {
                //    self.lstaDepartamento(result1.data);
                  
                //});

                PostJson("subasta-datos-fideicomiso?codBanco="+val, false).done(function (resultado) {
                    debugger;
                    if (resultado.success == true) {
                        var datos = ko.toJS(resultado.data);
                        console.log(datos);
                        self.envioContext.nombreEmpresa(datos.nombreEmpresa);
                        self.envioContext.numeroDocumento(datos.numeroDocumento);
                        self.envioContext.codigobanco(datos.codigobanco);
                        self.envioContext.banco(datos.banco);
                        self.envioContext.codigoMoneda(datos.codigoMoneda);
                        self.envioContext.moneda(datos.moneda);
                        self.envioContext.codigoTipoCuenta(datos.codigoTipoCuenta);
                        self.envioContext.tipoCuenta(datos.tipoCuenta);
                        self.envioContext.numeroCuenta(datos.numeroCuenta);
                        self.envioContext.numeroCuentaInter(datos.numeroCuentaInter);
                    } else {
                        toastr.error(resultado.error);
                        return false;
                    }
                });

            } else {
              
                //$("#combo_departamento_persona").trigger('chosen:updated');
            }

        });

    this.start = function () {
        setInterval(function () { return tick(); }, 1000);
    };

    function tick() {
        var data = {
            sid: window.sid
        };

        PostJson2("common/getTiempoSubastaCurrent", data, true).done(function (result) {
          //16-06-2019
            if (result.success == true) {
                debugger;
                if (result.data.tiempo >= 0) {
                    debugger;
                    var secs = result.data.tiempo;
                    var hours = Math.floor(secs / (60 * 60));

                    var divisor_for_minutes = secs % (60 * 60);
                    var minutes = Math.floor(divisor_for_minutes / 60);
                    minutes = minutes < 10 ? '0' + minutes : minutes;

                    var divisor_for_seconds = divisor_for_minutes % 60;
                    var seconds = Math.ceil(divisor_for_seconds);
                    seconds = seconds < 10 ? '0' + seconds : seconds;

                    var formato = minutes + ":" + seconds;
                    self.tiempo(formato);

                    //self.tiempo(result.data.tiempo);


                } else {
                    $('#popup_incumplido').removeClass('hidden');
                }
            } else {

            }

        });

    };

    self.start();

    this.enviarConstanciaPago = function () {
        debugger;
        if (self.envioContext.validarModelo() == false) {
            return false;
        }

        var model = {
            subasta: window.sid,
            operacion: self.envioContext.codigoOperacion(),
            cuentaOrigen: self.envioContext.cuentaOrigen(),
            cuentaDestino: self.envioContext.cuentaDestino(),
            bancoOrigenFideicomiso: self.envioContext.codigobanco(),
            cuentaOrigenFideicomiso: self.envioContext.numeroCuenta()

            
        };

        PostJson("subasta/saveEnvioPagoSubastaXCliente", model, true).done(function (result) {
            debugger;
            if (result.success == true) {
                debugger;
                window.location.href = window.urlWebHost + '/Subasta/Verificacion/' + result.data;
                //window.location.href = window.urlWebHost + '/Inicio/Index';

            } else {
                toastr.error(result.error);
            }

        });



    };

    this.cerrar = function () {
        debugger;
        window.location.href = window.w_host + "/Inicio";
    }
};



$(function () {
    $(".k-choseen").chosen({
        disable_search: true,
        allow_single_deselect: true,
        no_results_text: "Sin resultados para: ",
        placeholder_text_single: "--Seleccionar--"
    });

    ko.applyBindings(new ViewModel());
});