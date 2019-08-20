function PerfilModel() {
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

    this.lst = new GridModelSP("listado-perfil", 0, 10);
    this.AddContext = ko.observable(new PerfilModel());
    this.EditContext = new PerfilModel();
    this.EliminarContext = new PerfilModel();


    self.clear = function () {
        self.AddContext(new PerfilModel());
    }

    this.init = function () {
        self.lst.search({
            idFilter: 0,
            filter: '',
            searchFilter: ''
        });
    };

    this.openModalAdd = function () {
        $("#ficha_add_cargo").removeClass('hidden');
    };

    this.closeModalAdd = function () {
        $("#ficha_add_cargo").addClass('hidden');

    };

    this.addNew = function () {
        if (self.AddContext().validarModelo() == false) {
            return false;
        }

        var model = ko.toJS(self.AddContext());

        PostJson("new-perfil", model).done(function (resultado) {

            if (resultado.success == true) {
                self.init();
                $("#ficha_add_cargo").addClass('hidden');
                toastr.success('Perfil Registrado Correctamente');
                self.clear();
            } else {
                toastr.error(resultado.error);
                return false;
            }


        });
    };

    this.editar = function () {
        if (self.EditContext.validarModelo() == false) {
            return false;
        }

        var model = ko.toJS(self.EditContext);

        PostJson("edit-perfil", model).done(function (resultado) {

            if (resultado.success == true) {
                self.init();
                $("#ficha_edit_cargo").addClass('hidden');
                toastr.info('Perfil editado Correctamente');
            } else {
                toastr.error(resultado.error);
                return false;
            }


        });


    };

    this.eliminar = function () {


        var model = ko.toJS(self.EliminarContext);
        console.log(model);
        PostJson("eliminar-perfil", model).done(function (resultado) {

            if (resultado.success == true) {
                self.init();
                $("#modal_remove_cargo").addClass('hidden');
                toastr.info('Perfil eliminado Correctamente');
            } else {
                toastr.error(resultado.error);
                return false;
            }


        });

    };


    this.openModalEdit = function (element) {
        var carg = ko.toJS(element);
        //console.log(carg);
        self.EditContext.codigo = carg.codigo;
        self.EditContext.nombre(carg.nombre);
        self.EditContext.estado(carg.estado);


        //console.log(self.cargoEditContext());

        $("#ficha_edit_cargo").removeClass('hidden');
    };

    this.closeModalEdit = function () {
        $("#ficha_edit_cargo").addClass('hidden');

    };


    this.openModalEliminar = function (element) {
        var carg = ko.toJS(element);

        self.EliminarContext.codigo = carg.codigo;
        $("#modal_remove_cargo").removeClass('hidden');
    };


};

var modelo = new ViewModel();

$(function () {

    modelo.init();
    ko.applyBindings(modelo);

});

