function PaisModel() {
    var self = this;
    this.codigo = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
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

    this.lstPais = new GridModelSP("configuracion-listado-pais", 0, 10);
    this.paisAddContext = ko.observable(new PaisModel());
    this.paisEditContext = new PaisModel();
    this.paisEliminarContext = new PaisModel();

    self.clear = function () {
        self.paisAddContext(new PaisModel());
    }

    this.init = function () {
        self.lstPais.search({
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

    this.addNewPais = function () {
        if (self.paisAddContext().validarModelo() == false) {
            return false;
        }

        var model = ko.toJS(self.paisAddContext());
        
        PostJson("configuracion-new-pais", model).done(function (resultado) {

            if (resultado.success == true) {
                self.init();
                $("#ficha_add").addClass('hidden');
                toastr.success('Pais Registrado Correctamente');
                self.clear();
            } else {
                toastr.error(resultado.error);
                return false;
            }
            
        });
    };

    this.editPais = function () {
        if (self.paisEditContext.validarModelo() == false) {
            return false;
        }
       
        var model = ko.toJS(self.paisEditContext);

        PostJson("configuracion-edit-pais", model).done(function (resultado) {

            if (resultado.success == true) {
                self.init();
                $("#ficha_edit").addClass('hidden');
                toastr.info('Pais editado Correctamente');
            } else {
                toastr.error(resultado.error);
                return false;
            }
        });


    };

    this.eliminarPais = function () {


        var model = ko.toJS(self.paisEliminarContext);
        console.log(model);
        PostJson("configuracion-eliminar-pais", model).done(function (resultado) {

            if (resultado.success == true) {
                self.init();
                $("#modal_remove").addClass('hidden');
                toastr.info('Pais eliminado Correctamente');
            } else {
                toastr.error(resultado.error);
                return false;
            }


        });

    };


    this.openModalEdit = function (element) {
        var carg = ko.toJS(element);
        self.paisEditContext.codigo (carg.codigo);
        self.paisEditContext.nombre(carg.nombre);
        self.paisEditContext.estado(carg.estado);

        $("#ficha_edit").removeClass('hidden');
    };

    this.closeModalEdit = function () {
        $("#ficha_edit").addClass('hidden');

    };
    
    this.openModalEliminar = function (element) {
        var carg = ko.toJS(element);

        self.paisEliminarContext.codigo = carg.codigo;
        $("#modal_remove").removeClass('hidden');
    };


};

var modelo = new ViewModel();

$(function () {

    modelo.init();
    ko.applyBindings(modelo);

});

