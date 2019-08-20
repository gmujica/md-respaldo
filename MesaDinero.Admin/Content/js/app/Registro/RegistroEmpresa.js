function Empresa() {
    var self = this;

    self.enableAutoUbicacion = ko.observable(true);

    //this.ruc = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });

    this.ruc = ko.observable('').extend(
        {
            required: { params: true, message: "Campo Obligatorio" },
            isNumeric: true,
            minLength: { params: 11, message: "Debe ingresar los 11 dígitos del RUC" },
            maxLength: { params: 11, message: "Debe ingresar los 11 dígitos del RUC" }
        });
    this.nombre = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.actividadEconomica = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.origenFondos = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.tipoEmpresa = ko.observable('').extend({ required: { params: true, message: "Campo Obligatorio" } });

    this.ruc.subscribe(function (val) {

        if (val) {
            var data = {
                nro: val
            };

            PostJson("common-getdatosempresasimple", data, true).done(function (result) {
                
                if (result.success == true) {

                    self.nombre(result.data.nombre);


                } else {
                    toastr.error = result.error;
                    return false;
                }

            });

        } else {
            self.nombreEmpresa('');
        }
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

function Persona() {
    var self = this;
    this.tipoDocumento = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.nroDocumento = ko.observable('').extend({ required: { params: true, message: "Campo Obligatorio" } });

    this.nombres = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.apePaterno = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.apeMaterno = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.email = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    //.extend({ required: { params: true, message: "Campo Obligatorio" } });


    this.cargo = ko.observable('');
    this.preCelular = ko.observable('');
    this.celular = ko.observable();


    this.nroDocumento.subscribe(function (val) {
        debugger;
        var data = {
            nro: val
        };

        PostJson("common-getdatospersonasimple", data).done(function (result) {

            if (result.success = true) {
                debugger;
                if (result.data != null) {
                    var per = result.data;

                    self.tipoDocumento(per.tipoDocumento);
                    self.nombres(per.nombres);
                    self.apePaterno(per.apePaterno);
                    self.apeMaterno(per.apeMaterno);
                    self.preCelular(per.preCelular);
                    self.celular(per.celular);
                    self.email(per.email);
                }
            } else {
                toastr.error(result.error);
                return false;
            }

        });

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

    this.removeAccionista = function (data) {
        modelo.lstAccionistas.remove(data);
    }

}


function Accionista() {
    var self = this;
    this.tipoDocumento = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.nroDocumento = ko.observable('').extend({ required: { params: true, message: "Campo Obligatorio" } });

    this.nombres = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.apePaterno = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.apeMaterno = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.email = ko.observable();
    //.extend({ required: { params: true, message: "Campo Obligatorio" } });


    this.cargo = ko.observable('');
    this.preCelular = ko.observable('');
    this.celular = ko.observable();


    this.nroDocumento.subscribe(function (val) {
        debugger;
        var data = {
            nro: val
        };

        PostJson("common-getdatospersonasimple", data).done(function (result) {

            if (result.success = true) {
                debugger;
                if (result.data != null) {
                    var per = result.data;

                    self.tipoDocumento(per.tipoDocumento);
                    self.nombres(per.nombres);
                    self.apePaterno(per.apePaterno);
                    self.apeMaterno(per.apeMaterno);
                    self.preCelular(per.preCelular);
                    self.celular(per.celular);
                    self.email(per.email);
                }
            } else {
                toastr.error(result.error);
                return false;
            }

        });

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

    this.removeAccionista = function (data) {
        modelo.lstAccionistas.remove(data);
    }

}


function PersonaAutorizada() {
    var self = this;
    this.tipoDocumento = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.nroDocumento = ko.observable('').extend({ required: { params: true, message: "Campo Obligatorio" } });

    this.nombres = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.apePaterno = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.apeMaterno = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.email = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    //.extend({ required: { params: true, message: "Campo Obligatorio" } });


    this.cargo = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.preCelular = ko.observable();
    //.extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.celular = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });


    this.nroDocumento.subscribe(function (val) {
        debugger;
        var data = {
            nro: val
        };

        PostJson("common-getdatospersonasimple", data).done(function (result) {

            if (result.success = true) {
                debugger;
                if (result.data != null) {
                    var per = result.data;

                    self.tipoDocumento(per.tipoDocumento);
                    self.nombres(per.nombres);
                    self.apePaterno(per.apePaterno);
                    self.apeMaterno(per.apeMaterno);
                    self.preCelular(per.preCelular);
                    self.celular(per.celular);
                    self.email(per.email);
                }
            } else {
                toastr.error(result.error);
                return false;
            }

        });

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

    //this.openModalAdd = function () {
    //    $("#ficha_add_cargo").removeClass('hidden');
    //};
    this.empresa = new Empresa();
    this.representanteLegal = new Persona();
    this.accionista = new Persona();
    this.personaAutorizada = new PersonaAutorizada();

    this.lstOrigenFondos = ko.observableArray([]);
    this.lstActividadEconomica = ko.observableArray([]);
    this.lstCargos = ko.observableArray([]);
    this.lstTipoDocumentoPersona = ko.observableArray([]);
    this.lstTipoDocumentoaccionista = ko.observableArray([]);
    this.lstTipoDocumentoAutorizada = ko.observableArray([]);
    this.lstAccionistas = ko.observableArray([]);
    this.lsttiposMoneda = ko.observableArray([]);
    this.lstTipoCuentaBancaria = ko.observableArray([]);
    this.lstBancos = ko.observableArray([]);
    this.cuentaBanco = new CuentaBancariaModel();
    this.cuentasBancarias = ko.observableArray([]);
    this.cuentaRemoveContext;
    this.addAccionista = function () {
        self.lstAccionistas.push(new Accionista());
    };
    this.removeAccionista = function (data) {
        self.lstAccionistas.remove(data);
    }
    this.init = function () {
        debugger;
        PostJson("common/getAllOrigenFondos", null).done(function (result) {
            debugger;
            self.lstOrigenFondos(result.data);
        });

        PostJson("common/getAllActividadEconomica", null).done(function (result) {
            debugger;
            self.lstActividadEconomica(result.data);
        });

        PostJson("common/getAllCargos", null).done(function (result) {
            debugger;
            self.lstCargos(result.data);
        });

        PostJson("common/getTipoDocumentoPersona", null).done(function (result) {
            debugger;
            self.lstTipoDocumentoPersona(result.data);
            self.lstTipoDocumentoaccionista(result.data);
            self.lstTipoDocumentoAutorizada(result.data);
        });

        PostJson("combo/tipoMoneda", null).done(function (result) {
            self.lsttiposMoneda(result.data);

        });
        PostJson("common/tipoCuentaBancarias", null).done(function (result) {

            //if (result.success == true) {
            self.lstTipoCuentaBancaria(result.data);
            //} else {
            //    toastr.error(result.error);
            //    return false;
            //}
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


        //PostJson("common/getTipoMoneda", null).done(function (result) {
        //    self.lstMoneda(result.data);
        //});

    }

    this.addCuenta = function () {
        debugger;
        if (!self.cuentaBanco.validarModelo()) {
            return false;
        }

        var cuenta = new CuentaBancaria();

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
        self.cuentaRemoveContext.estado(9);
        $("#remove_cuenta").addClass("hidden");
        self.cuentaRemoveContext = null;
    };

    this.registrar = function () {
        //alert("registrar");
        debugger;
        var indice = 0;
        if (self.empresa.validarModelo() == false) {
            return false;
        }

        //if (self.cuentasBancarias().length < 1) {
        //    toastr.warning("No se tienen cuentas bancarias para evaluar.");
        //    return false;
        //}

        self.cuentasBancarias().forEach(function (elemento) {
            if (elemento.estado() == 1) {
                indice += 1;
            }

        });

        if (indice == 0) {
            toastr.warning("Debe Ingresar al menos una Cuenta Bancaria");
            return false;
        }

        var data = {
            //sid: window.sid,
            tipoEmpresa: self.empresa.tipoEmpresa(),
            empresa: ko.toJS(self.empresa),
            repreLegal: ko.toJS(self.representanteLegal),
            personaAutorizada: ko.toJS(self.personaAutorizada),
            accionistas: ko.toJS(self.lstAccionistas),
            cuentas: ko.toJS(self.cuentasBancarias)
        };

        var model = ko.toJS(self.empresa);

        PostJson("registro-datos-fideicomiso", data).done(function (resultado) {

            if (resultado.success == true) {
                self.init();
                $("#ficha_add_cargo").addClass('hidden');
                toastr.success('Se Registrado Datos de la Empresa Correctamente');
                self.clear();
            } else {
                toastr.error(resultado.error);
                return false;
            }


        });
    }

    this.verDatosEmpresa = function () {

        //$("#datos_persona_autorizada").addClass('hidden');
        //$("#datos_cuentas_bancarias").addClass('hidden');
        //$("#datos_empresa").removeClass('hidden');

        $("#datos_persona_autorizada").css('display', 'none');
        $("#datos_cuentas_bancarias").css('display', 'none');
        $("#datos_empresa").css('display', "block");
        /*Pintado*/
        $("#persona").removeClass('active');
        $("#cuenta").removeClass('active');
        $("#empresa").addClass('active');


        //$("#persona").css();


    };

    this.verDatosPersonaAutorizada = function () {
        debugger;
        if (self.empresa.validarModelo() == false) {
            toastr.error("Complete los datos de la Empresa");
            return false;
        }
        if (self.representanteLegal.validarModelo() == false) {
            toastr.error("Complete los datos del representante Legal");
            return false;
        }

        if (self.lstAccionistas().length == 0) {
            console.log(self.lstAccionistas().length);
            toastr.error("Debe registrar al menos un Accionista");
            return false;
        }
        var indiceAccionista = 0;
        if (self.lstAccionistas().length > 0) {
            self.lstAccionistas().forEach(function (elemento) {
                debugger;
                if (elemento.validarModelo() == false) {
                    debugger;
                    indiceAccionista = 1;

                }
            });
        }

        if (indiceAccionista == 1) {
            toastr.error("Complete los datos del Accionista");
            return false;

        }

        //if (self.accionista.validarModelo() == false) {
        //    return false;
        //}

        //$("#datos_empresa").addClass('hidden');
        //$("#datos_cuentas_bancarias").addClass('hidden');
        //$("#datos_persona_autorizada").removeClass('hidden');


        $("#datos_empresa").css('display', 'none');
        $("#datos_cuentas_bancarias").css('display', 'none');
        $("#datos_persona_autorizada").css('display', "block");
        /*Pintado*/
        $("#empresa").removeClass('active');
        $("#cuenta").removeClass('active');
        $("#persona").addClass('active');
        /*Border Pintado*/
        $("#rightEmpresa").css('background-color', "var(--font-blue)");
        $("#leftPersona").css('background-color', "var(--font-blue)");
        $("#statusPersona").removeClass('coloreGris');
        $("#persona").removeClass('deshabilitarDiv');


        //style = "background-color: var(--font-blue);"
        //$("#persona").css();


    };

    this.verCuentasBancarias = function () {


        debugger;
        if (self.personaAutorizada.validarModelo() == false) {
            toastr.error("Complete los datos de la Persona Autorizada");
            return false;
        }
        //$("#datos_empresa").addClass('hidden');
        //$("#datos_persona_autorizada").addClass('hidden');
        //$("#datos_cuentas_bancarias").removeClass('hidden');

        $("#datos_empresa").css('display', 'none');
        $("#datos_persona_autorizada").css('display', 'none');
        $("#datos_cuentas_bancarias").css('display', "block");
        /*Pintado*/
        $("#empresa").removeClass('active');
        $("#persona").removeClass('active');
        $("#cuenta").addClass('active');

        //$("#rightEmpresa").css('background-color', "var(--font-blue)");
        //$("#leftPersona").css('background-color', "var(--font-blue)");

        $("#rightPersona").css('background-color', "var(--font-blue)");
        $("#leftBanco").css('background-color', "var(--font-blue)");
        $("#statusBanco").removeClass('coloreGris');
        $("#cuenta").removeClass('deshabilitarDiv');
    };

    //$(".k-choseen").chosen({
    //    disable_search: true,
    //    allow_single_deselect: true,
    //    no_results_text: "Sin resultados para: ",
    //    placeholder_text_single: "--Seleccionar--"
    //});

};

var modelo = new ViewModel();

$(function () {


    modelo.init();
    ko.applyBindings(modelo);

});