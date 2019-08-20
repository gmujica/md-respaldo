function TipoCuentaModel() {
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

    this.lstCuenta = new GridModelSP("configuracion-listado-tipo-cuenta", 0, 10);
    this.tipoCuentaAddContext = ko.observable(new TipoCuentaModel());
    this.tipoCuentaEditContext = new TipoCuentaModel();
    this.tipoCuentaEliminarContext = new TipoCuentaModel();

    self.clear = function () {
        self.tipoCuentaAddContext(new TipoCuentaModel());
    }

    this.init = function () {
        self.lstCuenta.search({
            idFilter: 0,
            filter: '',
            searchFilter: ''
        });
    };

    this.openModalAdd = function () {
        $("#ficha_add").removeClass('hidden');
    };

    this.closeModalAdd = function () {
        $("#ficha_add").addClass('hidden');

    };

    this.addNew = function () {
        if (self.tipoCuentaAddContext().validarModelo() == false) {
            return false;
        }

        var model = ko.toJS(self.tipoCuentaAddContext());

        PostJson("configuracion-new-tipo-cuenta", model).done(function (resultado) {

            if (resultado.success == true) {
                self.init();
                $("#ficha_add").addClass('hidden');
                toastr.success('Tipo de Cuenta Bancaria Registrado Correctamente');
                self.clear();
            } else {
                toastr.error(resultado.error);
                return false;
            }


        });
    };

    this.editar = function () {
        if (self.tipoCuentaEditContext.validarModelo() == false) {
            return false;
        }

        var model = ko.toJS(self.tipoCuentaEditContext);

        PostJson("configuracion-edit-tipo-cuenta", model).done(function (resultado) {

            if (resultado.success == true) {
                self.init();
                $("#ficha_edit").addClass('hidden');
                toastr.info('Tipo de Cuenta Bancaria actualizada Correctamente');
            } else {
                toastr.error(resultado.error);
                return false;
            }


        });


    };

    this.eliminar = function () {


        var model = ko.toJS(self.tipoCuentaEliminarContext);
        PostJson("configuracion-eliminar-tipo-cuenta", model).done(function (resultado) {

            if (resultado.success == true) {
                self.init();
                $("#modal_remove").addClass('hidden');
                toastr.info('Tipo de Cuenta Bancaria eliminado Correctamente');
            } else {
                toastr.error(resultado.error);
                return false;
            }


        });

    };


    this.openModalEdit = function (element) {
        var act = ko.toJS(element);
        self.tipoCuentaEditContext.codigo = act.codigo;
        self.tipoCuentaEditContext.nombre(act.nombre);
        self.tipoCuentaEditContext.estado(act.estado);

        $("#ficha_edit").removeClass('hidden');
    };

    this.closeModalEdit = function () {
        $("#ficha_edit").addClass('hidden');

    };


    this.openModalEliminar = function (element) {
        var carg = ko.toJS(element);

        self.tipoCuentaEliminarContext.codigo = carg.codigo;
        $("#modal_remove").removeClass('hidden');
    };


};

var modelo = new ViewModel();

$(function () {

    modelo.init();
    ko.applyBindings(modelo);

});

