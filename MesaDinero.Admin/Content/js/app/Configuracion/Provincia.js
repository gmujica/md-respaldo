function ProvinciaModel() {
    var self = this;
    this.codigo = 0;
    this.nombre = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.estado = ko.observable(1).extend({ required: { params: true, message: "Campo Obligatorio" } });



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

    this.lstActividad = new GridModelSP("configuracion-listado-provincia", 0, 10);
    this.actividadAddContext = ko.observable(new ProvinciaModel());
    this.actividadEditContext = new ProvinciaModel();
    this.actividadEliminarContext = new ProvinciaModel();
    this.lstPaises = ko.observableArray([]);

    this.init = function () {

        self.lstActividad.search({
            idFilter: 0,
            filter: '',
            searchFilter: ''
        });
    };

    self.clear = function () {
        self.actividadAddContext(new ProvinciaModel());
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
        debugger;
        var model = ko.toJS(self.actividadAddContext());

        PostJson("configuracion-new-provincia", model).done(function (resultado) {

            if (resultado.success == true) {
                self.init();
                $("#ficha_add").addClass('hidden');
                toastr.success('Provincia Registrado Correctamente');
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

        PostJson("configuracion-edit-provincia", model).done(function (resultado) {

            if (resultado.success == true) {
                self.init();
                $("#ficha_edit").addClass('hidden');
                toastr.info('Provincia actualizada Correctamente');
            } else {
                toastr.error(resultado.error);
                return false;
            }


        });


    };

    this.eliminar = function () {


        var model = ko.toJS(self.actividadEliminarContext);
        console.log(model);
        PostJson("configuracion-eliminar-provincia", model).done(function (resultado) {

            if (resultado.success == true) {
                self.init();
                $("#modal_remove").addClass('hidden');
                toastr.info('Provincia eliminado Correctamente');
            } else {
                toastr.error(resultado.error);
                return false;
            }


        });

    };


    this.openModalEdit = function (element) {
        var act = ko.toJS(element);
        self.actividadEditContext.codigo = act.codigo;
        self.actividadEditContext.nombre(act.nombre);
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

