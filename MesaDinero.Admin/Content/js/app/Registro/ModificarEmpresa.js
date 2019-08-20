function Empresa() {
    var self = this;

    self.enableAutoUbicacion = ko.observable(true);

    this.ruc = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.nombre = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.actividadEconomica = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.origenFondos = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.tipoEmpresa = ko.observable('').extend({ required: { params: true, message: "Campo Obligatorio" } });

    //this.pais = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    //this.departamento = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    //this.provincia = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    //this.distrito = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    //this.direccion = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });


    //this.lstaDepartamento = ko.observableArray([]);
    //this.lstProvincia = ko.observableArray([]);
    //this.lstDistrito = ko.observableArray([]);

    //this.pais.subscribe(function (val) {
    //    if (self.enableAutoUbicacion() == true) {
    //        if (val) {
    //            PostJson("common/getDepartatmentoXPais/" + val, null, false).done(function (result) {
    //                self.lstaDepartamento(result.data);
    //                $("#combo_departamento").trigger('chosen:updated');
    //            });
    //        } else {
    //            self.lstaDepartamento.removeAll();
    //            $("#combo_departamento").trigger('chosen:updated');
    //        }
    //    }

    //});

    //this.departamento.subscribe(function (val) {

    //    if (self.enableAutoUbicacion() == true) {
    //        if (val) {
    //            PostJson("common/getProvinciaXDepartamento/" + self.pais() + "/" + val, null, false).done(function (result) {
    //                self.lstProvincia(result.data);
    //                $("#combo_provincia").trigger('chosen:updated');
    //            });
    //        } else {
    //            self.lstProvincia.removeAll();
    //            $("#combo_provincia").trigger('chosen:updated');
    //        }
    //    }

    //});

    //this.provincia.subscribe(function (val) {

    //    if (self.enableAutoUbicacion() == true) {
    //        if (val) {
    //            PostJson("common/getDistritoXProvincia/" + self.pais() + "/" + self.departamento() + "/" + val, null, false).done(function (result) {
    //                self.lstDistrito(result.data);
    //                $("#combo_deistrito").trigger('chosen:updated');
    //            });
    //        } else {
    //            self.lstDistrito.removeAll();
    //            $("#combo_deistrito").trigger('chosen:updated');
    //        }
    //    }

    //});

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
    this.estado = ko.observable(1);
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
                    if (result.data != null) {
                        var per = result.data;
                        debugger;
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


    //this.enableAutoPersona = ko.observable(true);

    //this.nroDocumento.subscribe(function (val) {

    //    if (self.enableAutoPersona() == true) {

    //        var data = {
    //            nroDocumento: val
    //        };

    //        PostJson("common/getdatosPersona", data).done(function (result) {

    //            if (result.success = true) {
    //                if (result.data != null) {
    //                    var per = result.data;

    //                    self.tipoDocumento(per.tipoDocumento);
    //                    self.nombres(per.nombres);
    //                    self.apePaterno(per.apePaterno);
    //                    self.apeMaterno(per.apeMaterno);
    //                    self.preCelular(per.preCelular);
    //                    self.celular(per.celular);
    //                    self.email(per.email);
    //                }


    //            } else {
    //                toastr.error(result.error);
    //                return false;
    //            }

    //        });


    //    }




    //});

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
        debugger;
        var d = ko.toJS(data);
        data.estado(9);
        if (d.nroDocumento != "") {
            modelo.lstAccionistasBd().forEach(function (elemento) {
                debugger;
                if (elemento.nroDocumento() == d.nroDocumento) {
                    modelo.lstAccionistasEliminados.push(data);
                }
            });
            
        }

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

    this.estado = ko.observable(1);
    this.cargo = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.preCelular = ko.observable();
    //.extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.celular = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });


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

    this.removeAutorizadas = function (data) {
      
        var d = ko.toJS(data);
        data.estado(9);
        if (d.nroDocumento != "") {
            modelo.lstAutorizadasBd().forEach(function (elemento) {
                debugger;
                if (elemento.nroDocumento() == d.nroDocumento) {
                    modelo.lstAutorizadasEliminados.push(data);
                }
            });

        }

        modelo.lstAutorizadas.remove(data);
    }

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

   
    this.empresa = new Empresa();
    this.representanteLegal = ko.observable(new Persona());
    this.accionista = new Persona();
    this.personaAutorizada = new PersonaAutorizada();

    this.lstOrigenFondos = ko.observableArray([]);
    this.lstActividadEconomica = ko.observableArray([]);
    this.lstCargos = ko.observableArray([]);
    this.lstTipoDocumentoPersona = ko.observableArray([]);
    this.lstTipoDocumentoaccionista = ko.observableArray([]);
    this.lstTipoDocumentoAutorizada = ko.observableArray([]);
  
    this.lsttiposMoneda = ko.observableArray([]);
    this.lstTipoCuentaBancaria = ko.observableArray([]);
    this.lstBancos = ko.observableArray([]);
    this.cuentaBanco = new CuentaBancariaModel();
    this.cuentasBancarias = ko.observableArray([]);
    this.cuentaRemoveContext;

    /* Persona Legal*/

    this.removePersonaLegal = function (data) {
        debugger;
        //data(new Persona());
        //debugger;
        //data= new Persona();
        self.representanteLegal (new Persona());

        //self.representanteLegal.tipoDocumento('');
        //self.representanteLegal.nroDocumento('');
        //self.representanteLegal.nombres(datos.repreLegal.nombres);
        //self.representanteLegal.apePaterno(datos.repreLegal.apePaterno);
        //self.representanteLegal.apeMaterno(datos.repreLegal.apeMaterno);
        //self.representanteLegal.email(datos.repreLegal.email);
    }

    /* Accionistas */ 
    this.lstAccionistas = ko.observableArray([]);
    this.lstAccionistasEliminados = ko.observableArray([]);
    this.lstAccionistasBd = ko.observableArray([]);
    this.lstAccionModif = ko.observableArray([]);

    this.addAccionista = function () {
        debugger;
        self.lstAccionistas.push(new Persona());
    };

    this.removeAccionista = function (data) {
        debugger;
        self.lstAccionistas.remove(data);
    }

    /* Persona Autorizadas */

    this.lstAutorizadas = ko.observableArray([]);
    this.lstAutorizadasEliminados = ko.observableArray([]);
    this.lstAutorizadasBd = ko.observableArray([]);
    this.lstAutorizadasModif = ko.observableArray([]);

    this.addAutorizadas = function () {
        debugger;
        self.lstAutorizadas.push(new PersonaAutorizada());
    };

    this.removeAutorizadas = function (data) {
        debugger;
        //self.lstAutorizadas.remove(data);
      
    }




    this.init = function () {
        debugger;

        PostJson("listar-datos-empresa", null).done(function (resultado) {
            debugger;
            console.log(resultado);
            self.lstAccionistas.removeAll();
            self.lstAccionistasEliminados.removeAll();
            self.lstAccionistasBd.removeAll();


            self.lstAutorizadas.removeAll();
            self.lstAutorizadasBd.removeAll();
            self.lstAutorizadasEliminados.removeAll();

            if (resultado.success == true) {
                var datos = ko.toJS(resultado.data);
                self.empresa.ruc(datos.empresa.ruc);
                self.empresa.nombre(datos.empresa.nombre);
                self.empresa.actividadEconomica(datos.empresa.actividadEconomica);
                self.empresa.origenFondos(datos.empresa.origenFondos);
                self.empresa.tipoEmpresa(datos.tipoEmpresa);

                self.representanteLegal().tipoDocumento(datos.repreLegal.tipoDocumento);
                self.representanteLegal().nroDocumento(datos.repreLegal.nroDocumento);
                self.representanteLegal().nombres(datos.repreLegal.nombres);
                self.representanteLegal().apePaterno(datos.repreLegal.apePaterno);
                self.representanteLegal().apeMaterno(datos.repreLegal.apeMaterno);
                self.representanteLegal().email(datos.repreLegal.email);

                //self.personaAutorizada.tipoDocumento(datos.personaAutorizada.tipoDocumento);
                //self.personaAutorizada.nroDocumento(datos.personaAutorizada.nroDocumento);
                //self.personaAutorizada.nombres(datos.personaAutorizada.nombres);
                //self.personaAutorizada.apePaterno(datos.personaAutorizada.apePaterno);
                //self.personaAutorizada.apeMaterno(datos.personaAutorizada.apeMaterno);
                //self.personaAutorizada.email(datos.personaAutorizada.email);
                //self.personaAutorizada.cargo(datos.personaAutorizada.cargo);
                //self.personaAutorizada.preCelular(datos.personaAutorizada.preCelular);
                //self.personaAutorizada.celular(datos.personaAutorizada.celular);

                
                datos.accionistas.forEach(function (elemento) {
                    this.persona = new Persona();

                    if (elemento.estado == 1) {
                        this.persona.tipoDocumento(elemento.tipoDocumento);
                        this.persona.nroDocumento(elemento.nroDocumento);

                        this.persona.nombres(elemento.nombres);
                        this.persona.apePaterno(elemento.apePaterno);
                        this.persona.apeMaterno(elemento.apeMaterno);
                        this.persona.email(elemento.email);
                        this.persona.estado(elemento.estado);
                        self.lstAccionistas.push(persona);
                        self.lstAccionistasBd.push(persona);
                    }
                 
                });

                datos.personaAutorizadas.forEach(function (elemento) {
                    this.persona = new Persona();

                    if (elemento.estado == 1) {
                        this.persona.tipoDocumento(elemento.tipoDocumento);
                        this.persona.nroDocumento(elemento.nroDocumento);

                        this.persona.nombres(elemento.nombres);
                        this.persona.apePaterno(elemento.apePaterno);
                        this.persona.apeMaterno(elemento.apeMaterno);
                        this.persona.email(elemento.email);
                        this.persona.estado(elemento.estado);
                        this.persona.cargo(elemento.cargo);
                        this.persona.preCelular(elemento.preCelular);
                        this.persona.celular(elemento.celular);
                        self.lstAutorizadas.push(persona);
                        self.lstAutorizadasBd.push(persona);
                    }

                });
                
            } else {
                toastr.error(resultado.error);
                return false;
            }


        });

        PostJson("common/getAllOrigenFondos", null).done(function (result) {
            debugger;
            self.lstOrigenFondos(result.data);
        });

        PostJson("common/getAllActividadEconomica", null).done(function (result) {
            self.lstActividadEconomica(result.data);
        });

        PostJson("common/getAllCargos", null).done(function (result) {
            self.lstCargos(result.data);
        });

        PostJson("common/getTipoDocumentoPersona", null, false).done(function (result) {
            self.lstTipoDocumentoPersona(result.data);
            self.lstTipoDocumentoaccionista(result.data);
            self.lstTipoDocumentoAutorizada(result.data);
        });

        PostJson("combo/tipoMoneda", null).done(function (result) {
            self.lsttiposMoneda(result.data);

        });
        //PostJson("common/tipoCuentaBancarias", null).done(function (result) {

        //    //if (result.success == true) {
        //    self.lstTipoCuentaBancaria(result.data);
        //    //} else {
        //    //    toastr.error(result.error);
        //    //    return false;
        //    //}
        //});

        //PostJson("common/entidadesFinacieras", null).done(function (result) {

        //    if (result.success == true) {

        //        self.lstBancos(result.data);
        //        $("#combo_Bancos").trigger('chosen:updated');

        //    } else {
        //        toastr.error(result.error);
        //        return false;
        //    }

        //});


        //PostJson("common/getTipoMoneda", null).done(function (result) {
        //    self.lstMoneda(result.data);
        //});

    }

    this.registrar = function () {
        //alert("registrar");
        debugger;
        var indice = 0;
        if (self.empresa.validarModelo() == false) {
            return false;
        }

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
        console.log(data);
        PostJson("registro-datos-fideicomiso", data).done(function (resultado) {

            if (resultado.success == true) {
                self.init();
              
                toastr.success('Se Registrado Datos de la Empresa Correctamente');
              
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

    this.modificarEmpresa = function () {
        debugger;
        if (self.empresa.validarModelo() == false) {
            toastr.error("Complete los datos de la Empresa");
            return false;
        }
        if (self.representanteLegal().validarModelo() == false) {
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
              
                if (elemento.validarModelo() == false) {

                    indiceAccionista = 1;

                }
            });
        }
        

        if (indiceAccionista == 1) {
            toastr.error("Complete los datos del Accionista");
            return false;
        }

        if (self.lstAutorizadas().length == 0) {
            console.log(self.lstAutorizadas().length);
            toastr.error("Debe registrar al menos una Persona Autorizada");
            return false;
        }
        var indiceAutorizada = 0;

        if (self.lstAutorizadas().length > 0) {
            self.lstAutorizadas().forEach(function (elemento) {
          
                if (elemento.validarModelo() == false) {
                    indiceAutorizada = 1;
                }
            });
        }

        if (indiceAutorizada == 1) {
            toastr.error("Complete los datos de la Persona Autorizada");
            return false;
        }
        //if (self.personaAutorizada.validarModelo() == false) {
        //    toastr.error("Complete los datos del representante Legal");
        //    return false;
        //}
      
        debugger;

        self.lstAccionModif.removeAll();
        self.lstAutorizadasModif.removeAll();

        self.lstAccionistas().forEach(function (elemento) {
            //this.persona = new Persona();
            //debugger;
            //this.persona.tipoDocumento(elemento.tipoDocumento);
            //this.persona.nroDocumento(elemento.nroDocumento);

            //this.persona.nombres(elemento.nombres);
            //this.persona.apePaterno(elemento.apePaterno);
            //this.persona.apeMaterno(elemento.apeMaterno);
            //this.persona.email(elemento.email);
            //this.persona.estado(elemento.estado);
            self.lstAccionModif.push(elemento);
        });

        self.lstAccionistasEliminados().forEach(function (elemento) {
            //this.persona = new Persona();
            //debugger;
            //    this.persona.tipoDocumento(elemento.tipoDocumento);
            //    this.persona.nroDocumento(elemento.nroDocumento);

            //    this.persona.nombres(elemento.nombres);
            //    this.persona.apePaterno(elemento.apePaterno);
            //    this.persona.apeMaterno(elemento.apeMaterno);
            //    this.persona.email(elemento.email);
            //    this.persona.estado(elemento.estado);
            self.lstAccionModif.push(elemento);
        });

        /*Modificar Personas Autorizadas*/

        self.lstAutorizadas().forEach(function (elemento) {
            //this.persona = new Persona();
            //debugger;
            //this.persona.tipoDocumento(elemento.tipoDocumento);
            //this.persona.nroDocumento(elemento.nroDocumento);

            //this.persona.nombres(elemento.nombres);
            //this.persona.apePaterno(elemento.apePaterno);
            //this.persona.apeMaterno(elemento.apeMaterno);
            //this.persona.email(elemento.email);
            //this.persona.estado(elemento.estado);

            //this.persona.cargo(elemento.cargo);
            //this.persona.preCelular(elemento.preCelular);
            //this.persona.celular(elemento.celular);
            self.lstAutorizadasModif.push(elemento);
        });

        self.lstAutorizadasEliminados().forEach(function (elemento) {
            //this.persona = new Persona();
            //debugger;
            //this.persona.tipoDocumento(elemento.tipoDocumento);
            //this.persona.nroDocumento(elemento.nroDocumento);

            //this.persona.nombres(elemento.nombres);
            //this.persona.apePaterno(elemento.apePaterno);
            //this.persona.apeMaterno(elemento.apeMaterno);
            //this.persona.email(elemento.email);
            //this.persona.cargo(elemento.cargo);
            //this.persona.preCelular(elemento.preCelular);
            //this.persona.celular(elemento.celular);
            //this.persona.estado(elemento.estado);
            self.lstAutorizadasModif.push(elemento);
        });


        var data = {
            //sid: window.sid,
            tipoEmpresa: self.empresa.tipoEmpresa(),
            empresa: ko.toJS(self.empresa),
            repreLegal: ko.toJS(self.representanteLegal),
            personaAutorizadas: ko.toJS(self.lstAutorizadasModif),
            accionistas: ko.toJS(self.lstAccionModif),
            cuentas: ko.toJS(self.cuentasBancarias)
        };
        console.log(data);
        
        PostJson("modificar-datos-empresa", data).done(function (resultado) {

            if (resultado.success == true) {
                self.init();
               
                toastr.success('Se Modifico Datos de la Empresa Correctamente');
               
            } else {
                toastr.error(resultado.error);
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