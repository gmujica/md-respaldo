function PersonaNatural() {
    var self = this;

    this.sid = ko.observable();
    this.tipoDocumento = ko.observable('');
    this.nroDocumento = ko.observable();
    this.nombres = ko.observable();
    this.apePaterno = ko.observable();
    this.apeMaterno = ko.observable();
    this.fechaNacimiento = ko.observable();
    this.email = ko.observable();
    this.celular = ko.observable();
    this.pais = ko.observable();
    this.departamento = ko.observable();
    this.provincia = ko.observable();
    this.distrito = ko.observable();
    this.direccion = ko.observable();
    this.origenFondos = ko.observable();
    this.expuesto = ko.observable();
    this.sictuacionLaboral = ko.observable();
    this.cuentasBancarias = ko.observableArray([]);
    this.observaciones = ko.observableArray([]);
    this.estadoValidacion = ko.observable('P');
    this.estadoValidacionFideicomiso = ko.observable('P');
    this.comentarioOperador = ko.observable("");
    this.comentarioFideicomiso = ko.observable("");
    this.estado = ko.observable();
    this.nombreEstado = ko.observable();

}

function Empresa() {
    var self = this;


    this.sid = ko.observable();
    this.ruc = ko.observable();
    this.nombre = ko.observable();
    this.actividadEconomica = ko.observable();
    this.origenFondos = ko.observable();
    this.pais = ko.observable();
    this.departamento = ko.observable();
    this.provincia = ko.observable();
    this.distrito = ko.observable();
    this.direccion = ko.observable();
    this.rucArchivo = ko.observable();
    this.estado = ko.observable();
    this.nombreEstado = ko.observable();
    

    this.representanteLegal = new PersonaNatural();
    this.personaAutorizada = ko.observableArray([]);
    this.accionsistas = ko.observableArray([]);
    this.cuentasBancarias = ko.observableArray([]);
    this.observaciones = ko.observableArray([]);
    this.estadoValidacion = ko.observable('P');
    this.estadoValidacion_Fideicomiso = ko.observable('P');

}


var ViewModel = function () {

    var self = this;
    this.clientesGridModel = new GridModelSP("operador/registro-clientes-all", 0, 10);

    this.personaTmp = new PersonaNatural();
    this.empreaTmp = new Empresa();
    this.comentario = ko.observable("");
    this.estado = ko.observable("P");
    this.rucDescargar = ko.observable('');
    this.openModal = function (data) {
        self.comentario('');
        debugger;
        var dt = ko.toJS(data);

        var model = {
            nroDocumento: dt.nroDocumento,
            cid: dt.sid

        };

        if (dt.tipocCliente == 1) {
            debugger;
            PostJson("common/datosPersonaAll", model, true).done(function (result) {
                if (result.success == true) {
                    debugger;
                    var mdata = result.data;
                    self.personaTmp.sid(dt.sid);
                    self.personaTmp.tipoDocumento(mdata.tipoDocumento);
                    self.personaTmp.nroDocumento(mdata.nroDocumento);
                    self.personaTmp.nombres(mdata.nombres);
                    self.personaTmp.apePaterno(mdata.apePaterno);
                    self.personaTmp.apeMaterno(mdata.apeMaterno);
                    self.personaTmp.fechaNacimiento(mdata.fechaNacimiento);
                    self.personaTmp.email(mdata.email);
                    self.personaTmp.celular(mdata.celular);
                    self.personaTmp.pais(mdata.pais);
                    self.personaTmp.departamento(mdata.departamento);
                    self.personaTmp.provincia(mdata.provincia);
                    self.personaTmp.distrito(mdata.distrito);
                    self.personaTmp.direccion(mdata.direccion);
                    self.personaTmp.sictuacionLaboral(mdata.sictuacionLaboral);
                    self.personaTmp.origenFondos(mdata.origenFondos);
                    self.personaTmp.expuesto(mdata.expuesto);
                    self.personaTmp.cuentasBancarias(result.data.cuentasBancarias);
                    self.personaTmp.observaciones(result.data.observacionesCliente);
                   
                    self.personaTmp.estadoValidacion(result.data.validacionOperador);
                    self.personaTmp.estadoValidacionFideicomiso(result.data.validacionFideicomiso);
                    self.personaTmp.comentarioOperador(result.data.msmOperador);
                    self.personaTmp.comentarioFideicomiso(result.data.msmFideicomiso);

                    //if(dt.tipoRegistro == 'A')
                        $("#ficha-person").removeClass('hidden');
                    //else if(dt.tipoRegistro == 'B')
                    //    $("#ficha-person_MD").removeClass('hidden');

                } else {
                    toastr.error(result.error);
                }

            });

        }

        if (dt.tipocCliente == 2) {

            PostJson("common/datosPerJuridicaXRegistroValidacion", model, true).done(function (result) {

                if (result.success == true) {
                    
                    var mdata = result.data;
                    debugger;
                    self.empreaTmp.sid(dt.sid);
                    self.empreaTmp.ruc(mdata.ruc);
                    self.empreaTmp.nombre(mdata.nombre);
                    self.empreaTmp.actividadEconomica(mdata.actividadEconomica);
                    self.empreaTmp.origenFondos(mdata.origenFondos);
                    self.rucDescargar(mdata.rucArchivo)
                    //RL
                    var mrepre = mdata.repreLegal;
                    self.empreaTmp.representanteLegal.tipoDocumento(mrepre.tipoDocumento);
                    self.empreaTmp.representanteLegal.nombres(mrepre.nombres);
                    self.empreaTmp.representanteLegal.apePaterno(mrepre.apePaterno);
                    self.empreaTmp.representanteLegal.apeMaterno(mrepre.apeMaterno);
                    self.empreaTmp.representanteLegal.nroDocumento(mrepre.nroDocumento);

                    // Accionistas

                    self.empreaTmp.accionsistas(result.data.accionistas);

                    // PA

                    self.empreaTmp.personaAutorizada(result.data.personaAutorizada);

                    // CB
                    debugger;
                    self.empreaTmp.cuentasBancarias(result.data.cuentasBancarias);
                    self.empreaTmp.observaciones(result.data.observacionesCliente);
                    
                    self.empreaTmp.estadoValidacion(result.data.estadoValidacion);
                    self.empreaTmp.estadoValidacion_Fideicomiso(result.data.estadoValidacionFideicomiso);



                    $("#ficha-company").removeClass("hidden");
                } else {
                    toastr.error(result.error);
                }

            });



        }


    }

    this.estado.subscribe(function (val) {
        self.init();
    });

    this.aprobarPCliente = function (data) {

        var d = ko.toJS(data);

        var mdata = {
            estado: 'A',
            sid: d.sid,
            observacion: self.comentario()
        };


        PostJson("operador/validarCliente", mdata, true).done(function (result) {




            if (result.success == true) {

                $("#ficha-person").addClass('hidden');
                self.init();
            }
            else {
                toastr.error(result.error);
            }


        });

    }

    this.observarPCliente = function (data) {

        var d = ko.toJS(data);

        var mdata = {
            estado: 'O',
            sid: d.sid,
            observacion: self.comentario()
        };



        if (self.comentario() == null || self.comentario() == "" || self.comentario() === undefined) {
            alert("Antes de observar el registro debe de ingresar un comentario.");
            return false;
        }



        PostJson("operador/validarCliente", mdata, true).done(function (result) {




            if (result.success == true) {

                $("#ficha-person").addClass('hidden');
                self.init();
            }
            else {
                toastr.error(result.error);
            }


        });

    }

    this.rechazarPCliente = function (data) {

        var d = ko.toJS(data);

        var mdata = {
            estado: 'R',
            sid: d.sid,
            observacion: self.comentario()
        };


        if (self.comentario() == null || self.comentario() == "" || self.comentario() === undefined) {
            alert("Antes de rechazar el registro debe de ingresar un comentario.");
            return false;
        }



        PostJson("operador/validarCliente", mdata, true).done(function (result) {




            if (result.success == true) {

                $("#ficha-person").addClass('hidden');
                self.init();
            }
            else {
                toastr.error(result.error);
            }


        });

    }


    // empresa

    this.aprobarECliente = function (data) {

        var d = ko.toJS(data);

        var mdata = {
            estado: 'A',
            sid: d.sid,
            observacion: self.comentario()
        };



        PostJson("operador/validarCliente", mdata, true).done(function (result) {




            if (result.success == true) {

                $("#ficha-company").addClass('hidden');
                self.init();
            }
            else {
                toastr.error(result.error);
            }


        });

    }

    this.observarECliente = function (data) {

        var d = ko.toJS(data);

        var mdata = {
            estado: 'O',
            sid: d.sid,
            observacion: self.comentario()
        };



        if (self.comentario() == null || self.comentario() == "" || self.comentario() === undefined) {
            alert("Antes de observar el registro debe de ingresar un comentario.");
            return false;
        }



        PostJson("operador/validarCliente", mdata, true).done(function (result) {

            if (result.success == true) {

                $("#ficha-company").addClass('hidden');
                self.init();
            }
            else {
                toastr.error(result.error);
            }


        });

    }

    this.rechazarECliente = function (data) {

        var d = ko.toJS(data);

        var mdata = {
            estado: 'R',
            sid: d.sid,
            observacion: self.comentario()
        };


        if (self.comentario() == null || self.comentario() == "" || self.comentario() === undefined) {
            alert("Antes de rechazar el registro debe de ingresar un comentario.");
            return false;
        }



        PostJson("operador/validarCliente", mdata, true).done(function (result) {




            if (result.success == true) {

                $("#ficha-company").addClass('hidden');
                self.init();
            }
            else {
                toastr.error(result.error);
            }


        });

    }


    this.init = function () {
        self.clientesGridModel.search({
            idFilter: 0,
            filter: '',
            searchFilter: '',
            textFilter:self.estado()
        });
    };



};


var modelo = new ViewModel();


$(function () {

    modelo.init();
    ko.applyBindings(modelo);
});