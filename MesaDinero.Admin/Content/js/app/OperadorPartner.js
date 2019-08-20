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


    //this.sid = ko.observable();
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

    var self = this;
    this.empreaTmp = new Empresa();
    this.partnersGridModel = new GridModelSP("operador/lista-partners", 0, 10);
   

    this.openModal = function (data) {
        debugger;
       
       
        var dt = ko.toJS(data);
        var model = {
            nroDocumento: dt.ruc,
            cid: '1'

        };

        PostJson("common/datosClienteRegistrarJuridicaAll", model, true).done(function (result) {
                debugger;
                if (result.success == true) {
                    debugger;
                    var mdata = result.data;

                    //self.empreaTmp.sid(dt.sid);
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

    this.init = function () {
        self.partnersGridModel.search({
            idFilter: 0,
            filter: '',
            searchFilter: '',
            textFilter: ''
        });
    };



};


var modelo = new ViewModel();


$(function () {

    modelo.init();
    ko.applyBindings(modelo);
});