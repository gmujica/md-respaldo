function ActividadModel() {
    var self = this;
    this.codigo = 0;
    this.codigoPais = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.codigoDepartamento = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.codigoProvincia = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.codigoDistrito = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.estado = ko.observable(1).extend({ required: { params: true, message: "Campo Obligatorio" } });;

   

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


function ViewModel() {

    var self = this;

    this.lstActividad = new GridModelSP("configuracion-listado-ubigeo", 0, 10);
    this.actividadAddContext = ko.observable(new ActividadModel());
    this.actividadEditContext = new ActividadModel();
    this.actividadEliminarContext = new ActividadModel();
    this.lstPaises = ko.observableArray([]);
    this.lstaDepartamento = ko.observableArray([]);

    this.lstProvincia = ko.observableArray([]);
    this.lstdistrito = ko.observableArray([]);

    debugger;
    this.actividadAddContext().codigoPais.subscribe(function (val) {
        if (val) {
            debugger;
            PostJson("common/getDepartatmentoXPais/" + val, null).done(function (result1) {
                self.lstaDepartamento(result1.data);
                //$("#combo_departamento_persona").trigger('chosen:updated');
            });
        } else {
            self.lstaDepartamento.removeAll();
            //$("#combo_departamento_persona").trigger('chosen:updated');
        }

    });

    debugger;
    this.init = function () {

        PostJson("common/getPaises", null).done(function (result) {
            self.lstPaises(result.data);
            //self.actividadAddContext().codigoPais('PE');
            debugger;
        });

        PostJson("configuracion-getProvincia", null).done(function (result) {
            self.lstProvincia(result.data);
            debugger;
        });

        PostJson("configuracion-getDistrito", null).done(function (result) {
            self.lstdistrito(result.data);
            debugger;
        });


        

        self.lstActividad.search({
            idFilter: 0,
            filter: '',
            searchFilter: ''
        });
    };

    self.clear = function () {
        self.actividadAddContext(new ActividadModel());
    }

    this.openModalAdd = function () {
        $("#ficha_add").removeClass('hidden');
    };

    this.closeModalAdd = function () {
        $("#ficha_add").addClass('hidden');

    };

    this.addNew = function () {
        if (self.actividadAddContext().validarModelo() == false) {
            return false;
        }

        var model = ko.toJS(self.actividadAddContext());

        PostJson("configuracion-new-ubigeo", model).done(function (resultado) {

            if (resultado.success == true) {
                self.init();
                $("#ficha_add").addClass('hidden');
                toastr.success('Ubigeo Registrado Correctamente');
                self.clear();
            } else {
                toastr.error(resultado.error);
                return false;
            }


        });
    };

    this.editar = function () {
        if (self.actividadEditContext.validarModelo() == false) {
            return false;
        }

        var model = ko.toJS(self.actividadEditContext);

        PostJson("configuracion-edit-ubigeo", model).done(function (resultado) {

            if (resultado.success == true) {
                self.init();
                $("#ficha_edit").addClass('hidden');
                toastr.info('Ubigeo actualizado Correctamente');
            } else {
                toastr.error(resultado.error);
                return false;
            }


        });


    };

    this.eliminar = function () {


        var model = ko.toJS(self.actividadEliminarContext);
        console.log(model);
        PostJson("configuracion-eliminar-ubigeo", model).done(function (resultado) {

            if (resultado.success == true) {
                self.init();
                $("#modal_remove").addClass('hidden');
                toastr.info('Ubigeo eliminado Correctamente');
            } else {
                toastr.error(resultado.error);
                return false;
            }


        });

    };


    this.openModalEdit = function (element) {
        var act = ko.toJS(element);
        //console.log(carg);
        ////self.actividadEditContext.codigo = act.codigo;
        //self.actividadEditContext.nombre(act.nombre);
        debugger;

        self.actividadEditContext.codigoPais.subscribe(function (val) {
            if (val) {
                debugger;
                PostJson("common/getDepartatmentoXPais/" + val, null).done(function (result1) {
                    self.lstaDepartamento(result1.data);
                    self.actividadEditContext.codigoDepartamento(act.codigoDepartamento);
                    //$("#combo_departamento_persona").trigger('chosen:updated');
                });
            } else {
                self.lstaDepartamento.removeAll();
                //$("#combo_departamento_persona").trigger('chosen:updated');
            }

        });
        self.actividadEditContext.codigoPais(act.codigoPais);
       
        self.actividadEditContext.codigoProvincia(act.codigoProvincia);
        self.actividadEditContext.codigoDistrito(act.codigoDistrito);
        
        
        self.actividadEditContext.estado(act.estado);


        //console.log(self.cargoEditContext());

        $("#ficha_edit").removeClass('hidden');
    };

    this.closeModalEdit = function () {
        $("#ficha_edit").addClass('hidden');

    };


    this.openModalEliminar = function (element) {
        var carg = ko.toJS(element);

        self.actividadEliminarContext.codigo = carg.codigo;
        $("#modal_remove").removeClass('hidden');
    };


};

var modelo = new ViewModel();

$(function () {

    modelo.init();
    ko.applyBindings(modelo);

});

