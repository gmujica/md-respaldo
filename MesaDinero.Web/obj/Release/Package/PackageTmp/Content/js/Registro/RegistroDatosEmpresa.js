/// <reference path="RegistroDatosEmpresa.js" />
function Empresa()
{
    var self = this;

    self.enableAutoUbicacion = ko.observable(true);

    this.ruc = ko.observable(window.w_nroDocumento).extend({ required: { params: true, message: "Campo Obligatorio" } });

    this.nombre = ko.observable(window.w_nomnreEmpresa).extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.actividadEconomica = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.origenFondos = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });

    this.pais = ko.observable('PE').extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.departamento = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.provincia = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.distrito = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.direccion = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.rucArchivo = ko.observable();

    this.lstaDepartamento = ko.observableArray([]);
    this.lstProvincia = ko.observableArray([]);
    this.lstDistrito = ko.observableArray([]);

    this.pais.subscribe(function (val) {
        if (self.enableAutoUbicacion() == true)
        {
            if (val) {
                PostJson("common/getDepartatmentoXPais/" + val, null, false).done(function (result) {
                    self.lstaDepartamento(result.data);
                    $("#combo_departamento").trigger('chosen:updated');
                });
            } else {
                self.lstaDepartamento.removeAll();
                $("#combo_departamento").trigger('chosen:updated');
            }
        }

    });

    this.departamento.subscribe(function (val) {

        if (self.enableAutoUbicacion() == true)
        {
            if (val) {
                PostJson("common/getProvinciaXDepartamento/" + self.pais() + "/" + val, null, false).done(function (result) {
                    self.lstProvincia(result.data);
                    $("#combo_provincia").trigger('chosen:updated');
                });
            } else {
                self.lstProvincia.removeAll();
                $("#combo_provincia").trigger('chosen:updated');
            }
        }

    });

    this.provincia.subscribe(function (val) {

        if (self.enableAutoUbicacion() == true) {
            if (val) {
                PostJson("common/getDistritoXProvincia/" + self.pais() + "/" + self.departamento() + "/" + val, null, false).done(function (result) {
                    self.lstDistrito(result.data);
                    $("#combo_deistrito").trigger('chosen:updated');
                });
            } else {
                self.lstDistrito.removeAll();
                $("#combo_deistrito").trigger('chosen:updated');
            }
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

function Persona()
{
    var self = this;
    this.tipoDocumento = ko.observable('DNI').extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.nroDocumento = ko.observable('').extend({ required: { params: true, message: "Campo Obligatorio" } });

    this.nombres = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.apePaterno = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.apeMaterno = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.email = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.preCelular = ko.observable('51').extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.celular = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });

    this.enableAutoPersona = ko.observable(true);

    this.nroDocumento.subscribe(function (val) {

        if (self.enableAutoPersona() == true) {

            var data = {
                nro: val
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
                        self.email(per.email);
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



function Accionista() {
    var self = this;
    this.tipoDocumento = ko.observable('DNI').extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.nroDocumento = ko.observable('').extend({ required: { params: true, message: "Campo Obligatorio" } });

    this.nombres = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.apePaterno = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.apeMaterno = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    //this.email = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
  

    this.enableAutoPersona = ko.observable(true);

    this.nroDocumento.subscribe(function (val) {

        if (self.enableAutoPersona() == true) {

            var data = {
                nro: val
            };

            PostJson("common-getdatospersonasimple", data).done(function (result) {

                if (result.success = true) {
                    if (result.data != null) {
                        var per = result.data;

                        self.tipoDocumento(per.tipoDocumento);
                        self.nombres(per.nombres);
                        self.apePaterno(per.apePaterno);
                        self.apeMaterno(per.apeMaterno);
                        //self.preCelular(per.preCelular);
                        //self.celular(per.celular);
                        //self.email(per.email);
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

    this.lstAccionistas = ko.observableArray([]);
    this.representanteLegal = new Persona();
    this.empresa = new Empresa();
    this.lstPaises = ko.observableArray([]);
    this.lstOrigenFondos = ko.observableArray([]);
    this.lstActividadEconomica = ko.observableArray([]);
    this.lstTipoDocumentoPersona = ko.observableArray([]);
    this.rucDescargar = ko.observable();
    this.myDisable = ko.observable(false);

    this.addAccionista = function () {
        self.lstAccionistas.push(new Accionista());
    };

    this.saveDatosBancarios = function () {
      
       
        if (!self.empresa.validarModelo()) {
            return false;
        }

        if (!self.representanteLegal.validarModelo()) {
            return false;
        }

        if (self.lstAccionistas().length == 0)
        {
            toastr.error("Debe ingresar como minimo 1 accionista");
            return false;
        }
        
        for (var i = 0; i < self.lstAccionistas().length; i++) {
            self.lstAccionistas()[i].validarModelo();
           
        }

        var data = {
            sid: window.sid,
            empresa: ko.toJS(self.empresa),
            repreLegal: ko.toJS(self.representanteLegal),
            accionistas: ko.toJS(self.lstAccionistas)
        };

        debugger;
        var file = document.getElementById('file');

        var form = new FormData();
        
        form.append('jsData', JSON.stringify(data));
        var indice = 0;
        if (file.files.length != 0) {
            debugger;
            indice = 1;
            var farchivo = file.files[0];
            form.append('fu_imagen', farchivo);
        } else {
            debugger;
            var ruc = ko.toJS(self.rucDescargar());
            if (ruc == "") {
                toastr.error("Debe Adjuntar Ficha Ruc");
                return false;
            }
          
        }
        form.append('indice', indice);
        PostFile("registro/datosemperesa", form).done(function (result) {
        //PostJson("registro/datosemperesa", data).done(function (result) {
            if (result.success == true)
            {
                window.location.href = window.urlApp + "Registro/PersonaAutorizada/" + result.data;
            } else {
                toastr.error(result.error);
                return false;
            }
        });

    };

    this.init = function () {
        

        PostJson("common/getAllOrigenFondos", null, false).done(function (result) {
            self.lstOrigenFondos(result.data);
        });

        PostJson("common/getAllActividadEconomica", null, false).done(function (result) {
            self.lstActividadEconomica(result.data);
        });

        PostJson("common/getTipoDocumentoPersona", null, true).done(function (result) {
            self.lstTipoDocumentoPersona(result.data);
       
        var data = {
            nroDocumento: window.w_nroDocumento,
            sid : window.sid
        }
        PostJson("common/getdatosEmpresa-for-registto", data, false).done(function (result) {
            debugger;
            if(result.success == true)
            {
                self.empresa.enableAutoUbicacion(false);
                PostJson("common/getPaises", null, false).done(function (result1) {
                    self.lstPaises(result1.data);
                    self.empresa.pais(result.data != null ? result.data.pais : 'PE');
                    $("#combo_pais").trigger('chosen:updated');
                });
                debugger;
                self.empresa.nombre(window.w_nomnreEmpresa);

                if (result.data != null)
                {
                    // general
                    self.empresa.nombre(result.data.nombre);
                    self.empresa.actividadEconomica(result.data.actividadEconomica);
                    self.empresa.origenFondos(result.data.origenFondos);
                    self.empresa.rucArchivo(result.data.rucArchivo);
                    // ubicacion

                    self.empresa.direccion(result.data.direccion);
                    self.rucDescargar(result.data.rucArchivo)

                    if (result.data.pais) {

                        PostJson("common/getDepartatmentoXPais/" + result.data.pais, null, false).done(function (result1) {
                            self.empresa.lstaDepartamento(result1.data);

                            if (result.data.departamento) {
                                self.empresa.departamento(result.data.departamento);
                                $("#combo_departamento").trigger('chosen:updated');
                                PostJson("common/getProvinciaXDepartamento/" + result.data.pais + "/" + result.data.departamento, null, false).done(function (resultp) {
                                    self.empresa.lstProvincia(resultp.data);
                                    if (result.data.provincia) {
                                        self.empresa.provincia(result.data.provincia);
                                        $("#combo_provincia").trigger('chosen:updated');

                                        PostJson("common/getDistritoXProvincia/" + result.data.pais + "/" + result.data.departamento + "/" + result.data.provincia, null, false).done(function (result2) {
                                            self.empresa.lstDistrito(result2.data);

                                            if (result.data.distrito) {
                                                self.empresa.distrito(result.data.distrito);
                                                $("#combo_deistrito").trigger('chosen:updated');
                                            }

                                        });

                                    }
                                });

                            }

                        });
                    }

                    self.empresa.enableAutoUbicacion(true);

                    // Representante Legal

                    self.representanteLegal.enableAutoPersona(false);
                    self.representanteLegal.tipoDocumento(result.data.repreLegal.tipoDocumento);
                    self.representanteLegal.nroDocumento(result.data.repreLegal.nroDocumento);
                    self.representanteLegal.nombres(result.data.repreLegal.nombres);
                    self.representanteLegal.apePaterno(result.data.repreLegal.apePaterno);
                    self.representanteLegal.apeMaterno(result.data.repreLegal.apeMaterno);
                    self.representanteLegal.preCelular(result.data.repreLegal.preCelular);
                    self.representanteLegal.celular(result.data.repreLegal.celular);
                    self.representanteLegal.email(result.data.repreLegal.email);
                    self.representanteLegal.enableAutoPersona(true);

                    // Accionista

                    for (var i = 0; i < result.data.accionistas.length; i++) {

                        var macc = result.data.accionistas[i];

                        var acc = new Accionista();
                        acc.enableAutoPersona(false);
                        acc.tipoDocumento(macc.tipoDocumento);
                        acc.nroDocumento(macc.nroDocumento);
                        acc.nombres(macc.nombres);
                        acc.apePaterno(macc.apePaterno);
                        acc.apeMaterno(macc.apeMaterno);
                        //acc.preCelular(macc.preCelular);
                        //acc.celular(macc.celular);
                        //acc.email(macc.email);
                        
                        acc.enableAutoPersona(true);
                        self.lstAccionistas.push(acc);

                    }

                }
                else
                {
                    PostJson("common/getDepartatmentoXPais/" + self.empresa.pais(), null, false).done(function (result1) {
                        self.empresa.lstaDepartamento(result1.data);
                        $("#combo_departamento").trigger('chosen:updated');
                    });
                }
               
                self.empresa.enableAutoUbicacion(true);
            }
            else {
                toastr.error(result.error);
            }

        });
        });
    };

};

var modelo = new ViewModel();

$(function () {

    $(".k-choseen").chosen({
        allow_single_deselect: true,
        no_results_text: "Sin resultados para: ",
        placeholder_text_single: "--Seleccionar--"
    });


    modelo.init();
    ko.applyBindings(modelo);



});