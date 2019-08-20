function OrigenFondoModel() {
    var self = this;
    this.codigo = 0;
    this.nombre = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });;
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

    this.lstCargos = new GridModelSP("configuracion-listado-origen-fondo", 0, 10);
    this.origenFondoAddContext = ko.observable(new OrigenFondoModel());
    this.origenFondoEditContext = new OrigenFondoModel();
    this.origenFondoEliminarContext = new OrigenFondoModel();

    self.clear = function () {
        self.origenFondoAddContext(new OrigenFondoModel());
    }

    this.init = function () {
        self.lstCargos.search({
            idFilter: 0,
            filter: '',
            searchFilter: ''
        });
    };

    this.openModalAdd = function () {
        $("#ficha_add_origen_fondo").removeClass('hidden');
    };

    this.closeModalAdd = function () {
        $("#ficha_add_origen_fondo").addClass('hidden');

    };

    this.addNewOrigenFondo = function () {
        if (self.origenFondoAddContext().validarModelo() == false) {
            return false;
        }

        var model = ko.toJS(self.origenFondoAddContext());

        PostJson("configuracion-new-origen-fondo", model).done(function (resultado) {

            if (resultado.success == true) {
                self.init();
                $("#ficha_add_origen_fondo").addClass('hidden');
                toastr.success('Origen Fondo Registrado Correctamente');
                self.clear();
            } else {
                toastr.error(resultado.error);
                return false;
            }
        });
    };

    this.editOrigenFondo = function () {
        if (self.origenFondoEditContext.validarModelo() == false) {
            return false;
        }

        var model = ko.toJS(self.origenFondoEditContext);

        PostJson("configuracion-edit-origen-fondo", model).done(function (resultado) {

            if (resultado.success == true) {
                self.init();
                $("#ficha_edit_origen_fondo").addClass('hidden');
                toastr.info('Origen Fondo editado Correctamente');
            } else {
                toastr.error(resultado.error);
                return false;
            }


        });


    };

    this.eliminarOrigenFondo = function () {


        var model = ko.toJS(self.origenFondoEliminarContext);
        console.log(model);
        PostJson("configuracion-eliminar-origen-fondo", model).done(function (resultado) {

            if (resultado.success == true) {
                self.init();
                $("#modal_remove_origen_fondo").addClass('hidden');
                toastr.info('Origen Fondo eliminado Correctamente');
            } else {
                toastr.error(resultado.error);
                return false;
            }


        });

    };


    this.openModalEdit = function (element) {
        var carg = ko.toJS(element);
        //console.log(carg);
        self.origenFondoEditContext.codigo = carg.codigo;
        self.origenFondoEditContext.nombre(carg.nombre);
        self.origenFondoEditContext.estado(carg.estado);


        //console.log(self.cargoEditContext());

        $("#ficha_edit_origen_fondo").removeClass('hidden');
    };

    this.closeModalEdit = function () {
        $("#ficha_edit_origen_fondo").addClass('hidden');

    };


    this.openModalEliminar = function (element) {
        var carg = ko.toJS(element);

        self.origenFondoEliminarContext.codigo = carg.codigo;
        $("#modal_remove_origen_fondo").removeClass('hidden');
    };


};

var modelo = new ViewModel();

$(function () {

    modelo.init();
    ko.applyBindings(modelo);

});

