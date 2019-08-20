function ActividadModel() {
    var self = this;
    this.codigo = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });;
    this.nombre = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });;
    this.simbolo = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });;
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

    this.lstActividad = new GridModelSP("configuracion-listado-tipo-moneda", 0, 10);
    this.tipoMonedaAddContext =ko.observable( new ActividadModel());
    this.tipoMonedaEditContext = new ActividadModel();
    this.tipoMonedaEliminarContext = new ActividadModel();

    self.clear = function () {
        self.tipoMonedaAddContext(new ActividadModel());
    }

    this.init = function () {
        self.lstActividad.search({
            idFilter: 0,
            filter: '',
            searchFilter: ''
        });
    };

    this.openModalAdd = function () {
        debugger;
        $("#ficha_add").removeClass('hidden');
        //self.tipoMonedaAddContext = new ActividadModel();
        //self.tipoMonedaAddContext.limpiar();
    };

    this.closeModalAdd = function () {
        $("#ficha_add").addClass('hidden');

    };

    this.addNew = function () {
        if (self.tipoMonedaAddContext().validarModelo() == false) {
            return false;
        }
        debugger;
        var model = ko.toJS(self.tipoMonedaAddContext());

        PostJson("configuracion-new-tipo-moneda", model).done(function (resultado) {

            if (resultado.success == true) {
                self.init();
                $("#ficha_add").addClass('hidden');
                toastr.success('Tipo de Moneda Registrado Correctamente');
                self.clear();
            } else {
                toastr.error(resultado.error);
                return false;
            }


        });
    };

    this.editar = function () {
        if (self.tipoMonedaEditContext.validarModelo() == false) {
            return false;
        }

        var model = ko.toJS(self.tipoMonedaEditContext);

        PostJson("configuracion-edit-tipo-moneda", model).done(function (resultado) {

            if (resultado.success == true) {
                self.init();
                $("#ficha_edit").addClass('hidden');
                toastr.info('Tipo de Moneda actualizada Correctamente');
            } else {
                toastr.error(resultado.error);
                return false;
            }


        });


    };

    this.eliminar = function () {


        var model = ko.toJS(self.tipoMonedaEliminarContext);
        
        PostJson("configuracion-eliminar-tipo-moneda", model).done(function (resultado) {

            if (resultado.success == true) {
                self.init();
                $("#modal_remove").addClass('hidden');
                toastr.info('Tipo de Moneda eliminado Correctamente');
            } else {
                toastr.error(resultado.error);
                return false;
            }


        });

    };


    this.openModalEdit = function (element) {
        var act = ko.toJS(element);
        //console.log(carg);
        self.tipoMonedaEditContext.codigo(act.codigo);
        self.tipoMonedaEditContext.nombre(act.nombre);
        self.tipoMonedaEditContext.estado(act.estado);
        self.tipoMonedaEditContext.simbolo(act.simbolo);
        

        //console.log(self.cargoEditContext());

        $("#ficha_edit").removeClass('hidden');
    };

    this.closeModalEdit = function () {
        $("#ficha_edit").addClass('hidden');

    };


    this.openModalEliminar = function (element) {
        var carg = ko.toJS(element);

        self.tipoMonedaEliminarContext.codigo = carg.codigo;
        $("#modal_remove").removeClass('hidden');
    };


};

var modelo = new ViewModel();

$(function () {

    modelo.init();
    ko.applyBindings(modelo);

});

