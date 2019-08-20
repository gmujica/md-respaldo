﻿function PersonaNatural() {
    var self = this;

    this.enableAutoUbicacion = ko.observable(true);

    this.sid = ko.observable(window.sid);
    this.tipoDocumento = ko.observable('DNI').extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.nroDocumento = ko.observable(window.w_nroDocumento).extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.nombres = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.apePaterno = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.apeMaterno = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.fnDia = ko.observable('').extend({ required: { params: true, message: "Campo Obligatorio" } });;
    this.fnMes = ko.observable('').extend({ required: { params: true, message: "Campo Obligatorio" } });;
    this.fnAnio = ko.observable('').extend({ required: { params: true, message: "Campo Obligatorio" } });;
    this.email = ko.observable().extend({
        required: { params: true, message: "Campo Obligatorio" },
        email: { params: true, message: "Ingrese un correo electrónico válido." }
    });
    this.preCelular = ko.observable('51').extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.celular = ko.observable().extend({
        required: { params: true, message: "Campo Obligatorio" },
        isNumeric: true
    });
    this.pais = ko.observable('PE').extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.departamento = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.provincia = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.distrito = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.direccion = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.origenFondos = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.expuesto = ko.observable('N');
    this.sictuacionLaboral = ko.observable('I');

    this.lstaDepartamento = ko.observableArray([]);
    this.lstProvincia = ko.observableArray([]);
    this.lstDistrito = ko.observableArray([]);

    this.nroDocumento.subscribe(function (val) {

     


    });

   

    this.entidadNombreExpuesto = ko.observable("");
    this.cargoExpuesto = ko.observable("");

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

    this.Persona = new PersonaNatural();
    this.lstPaises = ko.observableArray([]);
    this.lstOrigenFondos = ko.observableArray([]);



    this.init = function () {




        PostJson("common/getAllOrigenFondos", null, true).done(function (result) {
            self.lstOrigenFondos(result.data);

            var data = {
                sid: window.sid
            };



            $(".loading-spiner-holder").show();

            PostJson("registro/getdatosPersona-registro", data, false).done(function (result) {

                if (result.success == true) {

                    self.Persona.nombres(result.data.nombres || window.w_nombres);
                    self.Persona.apePaterno(result.data.apePaterno || window.w_apPaterno);
                    self.Persona.apeMaterno(result.data.apeMaterno || window.w_apMaterno);
                    self.Persona.email(result.data.email || window.w_apEmail);
                    self.Persona.fnAnio(result.data.fnAnio);
                    self.Persona.fnMes(result.data.fnMes);
                    self.Persona.fnDia(result.data.fnDia);
                    self.Persona.celular(result.data.celular || window.w_celular);
                    self.Persona.preCelular(result.data.preCelular || '51');

                    self.Persona.enableAutoUbicacion(false);


                    PostJson("common/getPaises", null, false).done(function (result) {
                        self.lstPaises(result.data);
                        self.Persona.pais(result.data.pais || 'PE');

                        $("#combo_pais_persona").trigger('chosen:updated');
                    });

                    if (result.data.pais) {
                        // self.Persona.pais(result.data.pais);

                        PostJson("common/getDepartatmentoXPais/" + result.data.pais, null, false).done(function (result1) {
                            self.Persona.lstaDepartamento(result1.data);
                            if (result.data.departamento) {
                                self.Persona.departamento(result.data.departamento);
                                $("#combo_departamento_persona").trigger('chosen:updated');
                                PostJson("common/getProvinciaXDepartamento/" + result.data.pais + "/" + result.data.departamento, null, false).done(function (resultp) {
                                    self.Persona.lstProvincia(resultp.data);
                                    if (result.data.provincia) {
                                        self.Persona.provincia(result.data.provincia);
                                        $("#combo_provincia_persona").trigger('chosen:updated');
                                        PostJson("common/getDistritoXProvincia/" + result.data.pais + "/" + result.data.departamento + "/" + result.data.provincia, null, false).done(function (result2) {
                                            self.Persona.lstDistrito(result2.data);

                                            if (result.data.distrito) {
                                                self.Persona.distrito(result.data.distrito);
                                                $("#combo_deistrito_persona").trigger('chosen:updated');
                                            }
                                        });
                                    }
                                });
                            }
                        });
                    }

                    self.Persona.enableAutoUbicacion(true);

                    self.Persona.direccion(result.data.direccion);
                    if (self.Persona.origenFondos != 0)
                        self.Persona.origenFondos(result.data.origenFondos);

                    self.Persona.sictuacionLaboral(result.data.sictuacionLaboral);
                    self.Persona.expuesto(result.data.expuesto);

                    self.Persona.entidadNombreExpuesto(result.data.entidadNombreExpuesto);
                    self.Persona.cargoExpuesto(result.data.cargoExpuesto);


                } else {
                    toastr.error(result.error);
                    return false;
                }

            });

        });








    };
};


var modelo = new ViewModel();

$(function () {

    $(".k-choseen").chosen({
        allow_single_deselect: false,
        no_results_text: "Sin resultados para: ",
        placeholder_text_single: "--Seleccionar--"
        
    });

    $(".k-choseen").trigger('chosen:updated');


    modelo.init();
    ko.applyBindings(modelo);




});