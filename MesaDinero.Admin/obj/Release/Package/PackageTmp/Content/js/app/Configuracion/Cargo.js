function CargoModel()
{
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

    this.lstCargos = new GridModelSP("configuracion-listado-cargos", 0, 10);
    this.cargoAddContext = ko.observable(new CargoModel());
    this.cargoEditContext = new CargoModel();
    this.cargoEliminarContext = new CargoModel();


    self.clear = function () {
        self.cargoAddContext(new CargoModel());
    }

    this.init = function () {
        self.lstCargos.search({
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

    this.addNewCargo = function () {
        if (self.cargoAddContext().validarModelo() == false) {
            return false;
        }

        var model = ko.toJS(self.cargoAddContext());

        PostJson("configuracion-new-cargo", model).done(function (resultado) {

            if (resultado.success == true) {
                self.init();
                $("#ficha_add_cargo").addClass('hidden');
                toastr.success('Cargo Registrado Correctamente');
                self.clear();
            } else {
                toastr.error(resultado.error);
                return false;
            }


        });
    };

    this.editcargo = function () {
        if (self.cargoEditContext.validarModelo() == false) {
            return false;
        }

        var model = ko.toJS(self.cargoEditContext);

        PostJson("configuracion-edit-cargo", model).done(function (resultado) {

            if (resultado.success == true) {
                self.init();
                $("#ficha_edit_cargo").addClass('hidden');
                toastr.info('Cargo editado Correctamente');
            } else {
                toastr.error(resultado.error);
                return false;
            }


        });


    };

    this.eliminarcargo = function () {
        

        var model = ko.toJS(self.cargoEliminarContext);
        console.log(model);
        PostJson("configuracion-eliminar-cargo", model).done(function (resultado) {

            if (resultado.success == true) {
                self.init();
                $("#modal_remove_cargo").addClass('hidden');
                toastr.info('Cargo eliminado Correctamente');
            } else {
                toastr.error(resultado.error);
                return false;
            }


        });

    };


    this.openModalEdit = function (element) {
        var carg = ko.toJS(element);
        //console.log(carg);
        self.cargoEditContext.codigo=carg.codigo;
        self.cargoEditContext.nombre(carg.nombre);
        self.cargoEditContext.estado(carg.estado);


        //console.log(self.cargoEditContext());

        $("#ficha_edit_cargo").removeClass('hidden');
    };

    this.closeModalEdit = function () {
        $("#ficha_edit_cargo").addClass('hidden');

    };


    this.openModalEliminar = function (element) {
        var carg = ko.toJS(element);
       
        self.cargoEliminarContext.codigo = carg.codigo;
        $("#modal_remove_cargo").removeClass('hidden');
    };


};

    var modelo = new ViewModel();

    $(function () {

        modelo.init();
        ko.applyBindings(modelo);

    });

