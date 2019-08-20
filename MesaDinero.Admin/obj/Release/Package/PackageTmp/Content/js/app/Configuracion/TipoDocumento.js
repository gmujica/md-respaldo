
function TipoDocumentoModel() {
    var self = this;
    this.codigo = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });;
    this.nombre = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });;
    this.tipo = ko.observable("P").extend({ required: { params: true, message: "Campo Obligatorio" } });;
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

    this.lstTipoDocumento = new GridModelSP("configuracion-listado-tipo-documento", 0, 10);
    this.tipoAddContext = ko.observable(new TipoDocumentoModel());
    this.tipoEditContext = new TipoDocumentoModel();
    this.tipoEliminarContext = new TipoDocumentoModel();

    self.clear = function () {
        self.tipoAddContext(new TipoDocumentoModel());
    }

    this.init = function () {
        self.lstTipoDocumento.search({
            idFilter: 0,
            filter: '',
            searchFilter: ''
        });
    };

    this.openModalAdd = function () {
        $("#ficha_add_tipodoc").removeClass('hidden');
    };

    this.closeModalAdd = function () {
        $("#ficha_add_tipodoc").addClass('hidden');

    };

    this.addNewTipoDoc = function () {
        if (self.tipoAddContext().validarModelo() == false) {
            return false;
        }

        var model = ko.toJS(self.tipoAddContext());

        PostJson("configuracion-new-tipo-documento", model).done(function (resultado) {

            if (resultado.success == true) {
                self.init();
                $("#ficha_add_tipodoc").addClass('hidden');
                toastr.success('Tipo Documento Registrado Correctamente');
                self.clear();
            } else {
                toastr.error(resultado.error);
                return false;
            }
        });
    };

    this.editdocumento = function () {
        if (self.tipoEditContext.validarModelo() == false) {
            return false;
        }

        var model = ko.toJS(self.tipoEditContext);

        PostJson("configuracion-edit-tipo-documento", model).done(function (resultado) {

            if (resultado.success == true) {
                self.init();
                $("#ficha_edit_tipo_doc").addClass('hidden');
                toastr.info('Tipo Documento editado Correctamente');
            } else {
                toastr.error(resultado.error);
                return false;
            }

        });


    };

    this.eliminarcargo = function () {


        var model = ko.toJS(self.tipoEliminarContext);

        PostJson("configuracion-eliminar-tipo-documento", model).done(function (resultado) {

            if (resultado.success == true) {
                self.init();
                $("#modal_remove_tipodoc").addClass('hidden');
                toastr.info('Tipo Documento eliminado Correctamente');
            } else {
                toastr.error(resultado.error);
                return false;
            }


        });

    };


    this.openModalEdit = function (element) {
        var carg = ko.toJS(element);

        self.tipoEditContext.codigo(carg.codigo);
        self.tipoEditContext.nombre(carg.nombre);
        self.tipoEditContext.tipo(carg.tipo);
        self.tipoEditContext.estado(carg.estado);


        //console.log(self.cargoEditContext());

        $("#ficha_edit_tipo_doc").removeClass('hidden');
    };

    this.closeModalEdit = function () {
        $("#ficha_edit_tipo_doc").addClass('hidden');

    };


    this.openModalEliminar = function (element) {
        var tipo = ko.toJS(element);

        self.tipoEliminarContext.codigo = tipo.codigo;
        $("#modal_remove_tipodoc").removeClass('hidden');
    };


};

var modelo = new ViewModel();

$(function () {

    modelo.init();
    ko.applyBindings(modelo);

});

