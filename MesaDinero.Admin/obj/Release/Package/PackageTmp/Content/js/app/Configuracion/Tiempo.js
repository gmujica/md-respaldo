function TiempoModel() {
    var self = this;
    this.codigo = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.tiempoStandar = ko.observable().extend({
        required: { params: true, message: "Campo Obligatorio" }
        //,
        //isNumeric: true
    });
    this.tiempoPremiun = ko.observable().extend({
        //isNumeric: true
    });
    this.tiempoVip = ko.observable().extend({
        //isNumeric: true
    });
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

    this.lstActividad = new GridModelSP("configuracion-listado-tiempo", 0, 10);
    this.tiempoAddContext =  ko.observable(new TiempoModel());
    this.tiempoEditContext = new TiempoModel();
    this.tiempoEliminarContext = new TiempoModel();


    self.clear = function () {
        self.tiempoAddContext(new TiempoModel());
    }

    this.init = function () {
        self.lstActividad.search({
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
        if (self.tiempoAddContext().validarModelo() == false) {
            return false;
        }

        var model = ko.toJS(self.tiempoAddContext());

        PostJson("configuracion-new-tiempo", model).done(function (resultado) {

            if (resultado.success == true) {
                self.init();
                $("#ficha_add").addClass('hidden');
                toastr.success('Tiempo Registrado Correctamente');
                self.clear();
            } else {
                toastr.error(resultado.error);
                return false;
            }


        });
    };

    this.editar = function () {
        if (self.tiempoEditContext.validarModelo() == false) {
            return false;
        }

        var model = ko.toJS(self.tiempoEditContext);

        PostJson("configuracion-edit-tiempo", model).done(function (resultado) {

            if (resultado.success == true) {
                self.init();
                $("#ficha_edit").addClass('hidden');
                toastr.info('Tiempo actualizada Correctamente');
            } else {
                toastr.error(resultado.error);
                return false;
            }


        });


    };

    this.eliminar = function () {


        var model = ko.toJS(self.tiempoEliminarContext);
        console.log(model);
        PostJson("configuracion-eliminar-tiempo", model).done(function (resultado) {

            if (resultado.success == true) {
                self.init();
                $("#modal_remove").addClass('hidden');
                toastr.info('Tiempo eliminado Correctamente');
            } else {
                toastr.error(resultado.error);
                return false;
            }


        });

    };


    this.openModalEdit = function (element) {
    
        var act = ko.toJS(element);
        //console.log(carg);
        self.tiempoEditContext.codigo(act.codigo);
        self.tiempoEditContext.tiempoStandar(parseInt(act.tiempoStandar));
        self.tiempoEditContext.tiempoPremiun(act.tiempoPremiun);
        
        self.tiempoEditContext.tiempoVip(act.tiempoVip);

        self.tiempoEditContext.estado(act.estado);


        //console.log(self.cargoEditContext());

        $("#ficha_edit").removeClass('hidden');
    };

    this.closeModalEdit = function () {
        $("#ficha_edit").addClass('hidden');

    };


    this.openModalEliminar = function (element) {
        var carg = ko.toJS(element);
        debugger;
        self.tiempoEliminarContext.codigo(carg.codigo);
        $("#modal_remove").removeClass('hidden');
    };


};

var modelo = new ViewModel();

$(function () {

    modelo.init();
    ko.applyBindings(modelo);

});

