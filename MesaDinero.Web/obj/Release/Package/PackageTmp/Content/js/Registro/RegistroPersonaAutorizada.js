function Persona() {
    var self = this;
    this.sid = ko.observable();
    this.tipoDocumento = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.nroDocumento = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });

    this.nombres = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.apePaterno = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.apeMaterno = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.email = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.preCelular = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.celular = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.cargo = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });

    this.enableAutoPersona = ko.observable(true);

    this.nroDocumento.subscribe(function (val) {

        if (self.enableAutoPersona() == true) {
            var data = {
                nroDocumento: val
            };

            PostJson("common-getdatospersonasimple", data).done(function (result) {

                if (result.success = true) {
                    if (result.data != null) {
                        var per = result.data;

                        self.tipoDocumento(per.tipoDocumento);
                        self.nombres(per.nombres);
                        self.apePaterno(per.apePaterno);
                        self.apeMaterno(per.apeMaterno);
                        self.preCelular(per.preCelular);
                        self.celular(per.celular);
                        // self.email(per.email);
                    }


                } else {
                    toastr.error(result.error);
                    return false;
                }

            });
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

    this.removeAccionista = function (data) {
        modelo.lstAccionistas.remove(data);
    }

}


var ViewModel = function () {

    var self = this;

    this.lstTipoDocumentoPersona = ko.observableArray([]);
    this.lstCargos = ko.observableArray([]);

    this.lstAutorizado = ko.observableArray([]);
    self.lstAutorizado.push(new Persona());

    this.Autorizado = new Persona();




    this.guardar = function () {

        var errorValidacion = false;

        for (var i = 0; i < self.lstAutorizado().length; i++) {
            if (!self.lstAutorizado()[i].validarModelo()) {
                errorValidacion = true;
            }

        }


        if (errorValidacion) {
            return false;
        }

        var data ={
        
            sid: window.sid,
            autorizados: ko.toJS(self.lstAutorizado)

        } ;


        PostJson("registro/empresa-personaautorizada", data).done(function (result) {

            if (result.success == true)
            {
                window.location.href = window.urlApp + "Registro/DatosBancarios/" + result.data;
            } else {
                toastr.error(result.error);
                return false;
            }

        });

    };

    this.init = function () {


        PostJson("common/getTipoDocumentoPersona", null, false).done(function (result) {
            self.lstTipoDocumentoPersona(result.data);
        });

        PostJson("common/getAllCargos", null, false).done(function (result) {
            self.lstCargos(result.data);
        });

        var data = {
            sid : window.sid
        };
        
        PostJson("common/getdatosPerAuth-registro", data, false).done(function (result) {
            if(result.success == true)
            {
                self.lstAutorizado()[0].enableAutoPersona(false);
               
                self.lstAutorizado()[0].nroDocumento(result.data.nroDocumento || window.w_nroDocumento);
                self.lstAutorizado()[0].tipoDocumento(result.data.tipoDocumento || 'DNI');
                self.lstAutorizado()[0].nombres(result.data.nombres || window.w_nombres);
                self.lstAutorizado()[0].apePaterno(result.data.apePaterno || window.w_apPaterno);
                self.lstAutorizado()[0].apeMaterno(result.data.apeMaterno || window.w_apMaterno);
                self.lstAutorizado()[0].preCelular(result.data.preCelular || '51');
                self.lstAutorizado()[0].celular(result.data.celular || window.w_celular);
                self.lstAutorizado()[0].email(result.data.email || window.w_apEmail);
                self.lstAutorizado()[0].cargo(result.data.cargo || null);

                self.lstAutorizado()[0].enableAutoPersona(false);

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