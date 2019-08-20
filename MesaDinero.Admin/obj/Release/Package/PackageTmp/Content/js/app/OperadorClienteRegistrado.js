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
    this.listaOperaciones = ko.observableArray([]);
    this.estadoValidacion = ko.observable('P');
    this.estadoValidacionFideicomiso = ko.observable('P');
    this.comentarioOperador = ko.observable("");
    this.comentarioFideicomiso = ko.observable("");

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

    this.representanteLegal = new PersonaNatural();
    this.personaAutorizada = ko.observableArray([]);
    this.accionsistas = ko.observableArray([]);
    this.cuentasBancarias = ko.observableArray([]);
    this.listaOperaciones = ko.observableArray([]);
    this.estadoValidacion = ko.observable('P');

}


var ViewModel = function () {
    debugger;
    var self = this;
    this.clientesGridModel = new GridModelSP("operador/lista-clientes-registrado", 0, 10);
    this.nroDoc = ko.observable();
    this.personaTmp = new PersonaNatural();
    this.empreaTmp = new Empresa();
    this.comentario = ko.observable("");
   
    //this.estado = ko.observable("");



    this.openModal = function (data) {
        debugger;
        self.comentario('');

        var dt = ko.toJS(data);
        //dt.tipoCliente = "Empresa";
        //dt.nroDocumento = "21458454555";
        var model = {
            nroDocumento: dt.nroDocumento,
            cid: dt.sid

        };

      
        //tipoCliente: "Persona"
        //console.log(dt.tipoCliente);
        if (dt.tipoCliente == "Persona") {
            debugger;
            PostJson("common/datosClienteRegistradoAll", model, true).done(function (result) {
                debugger;
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

                    self.personaTmp.estadoValidacion(result.data.validacionOperador);
                    self.personaTmp.estadoValidacionFideicomiso(result.data.validacionFideicomiso);
                    self.personaTmp.comentarioOperador(result.data.msmOperador);
                    self.personaTmp.comentarioFideicomiso(result.data.msmFideicomiso);
                    self.personaTmp.listaOperaciones(result.data.clienteOperaciones);
                    debugger;
                    console.log(self.personaTmp.listaOperaciones);
                    $("#ficha-person").removeClass('hidden');
                } else {
                    toastr.error(result.error);
                }

            });

        }

        if (dt.tipoCliente == "Empresa") {
            PostJson("common/datosClienteRegistrarJuridicaAll", model, true).done(function (result) {
                debugger;
                if (result.success == true) {

                    var mdata = result.data;

                    self.empreaTmp.sid(dt.sid);
                    self.empreaTmp.ruc(mdata.ruc);
                    self.empreaTmp.nombre(mdata.nombre);
                    self.empreaTmp.actividadEconomica(mdata.actividadEconomica);
                    self.empreaTmp.origenFondos(mdata.origenFondos);

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

                    //var mperauth = mdata.personaAutorizada;
                    //self.empreaTmp.personaAutorizada.tipoDocumento(mperauth.tipoDocumento);
                    //self.empreaTmp.personaAutorizada.nombres(mperauth.nombres);
                    //self.empreaTmp.personaAutorizada.apePaterno(mperauth.apePaterno);
                    //self.empreaTmp.personaAutorizada.apeMaterno(mperauth.apeMaterno);
                    //self.empreaTmp.personaAutorizada.nroDocumento(mperauth.nroDocumento);
                    //self.empreaTmp.personaAutorizada.celular(mperauth.celular);
                    //self.empreaTmp.personaAutorizada.email(mperauth.email);

                    // CB

                    self.empreaTmp.cuentasBancarias(result.data.cuentasBancarias);

                    //self.empreaTmp.estadoValidacion(dt.estadoValidacion_Operador);
                    self.empreaTmp.listaOperaciones(result.data.clienteOperaciones);

                    $("#ficha-company").removeClass("hidden");
                } else {
                    toastr.error(result.error);
                }

            });
        }


    }

    //this.estado.subscribe(function (val) {
    //    self.init();
    //});

    this.filtrarLista = function () {
        self.init();
    }
   

    this.init = function () {
        self.clientesGridModel.search({
            idFilter: 0,
            filter: '',
            searchFilter: '',
            textFilter: self.nroDoc()
        });
    };



};


var modelo = new ViewModel();


$(function () {

    modelo.init();
    ko.applyBindings(modelo);
});