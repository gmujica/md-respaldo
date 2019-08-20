function ActividadModel() {
    var self = this;
    this.codigo = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.nombre = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    
    this.tipo = ko.observable('B').extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.logo = ko.observable();
    this.formatoCCI = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.formatoCB = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
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

    this.lstActividad = new GridModelSP("configuracion-listado-financiera", 0, 10);
    this.actividadAddContext = ko.observable(new ActividadModel());
    this.actividadEditContext = new ActividadModel();
    this.actividadEliminarContext = new ActividadModel();

    self.clear = function () {
        self.actividadAddContext(new ActividadModel());
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
        if (self.actividadAddContext().validarModelo() == false) {
            return false;
        }

        var model = ko.toJS(self.actividadAddContext());
        var modelstr = JSON.stringify(model);

        debugger;
        var file = document.getElementById('fu_imagen');

        var form = new FormData();
        form.append('jsBanco', modelstr);
        var indice = 0;
        if (file.files.length != 0) {
            indice = 1;
            var farchivo = file.files[0];
            form.append('fu_imagen', farchivo);
        } 
        form.append('indice', indice);

        PostFile("configuracion-new-financiera", form).done(function (resultado) {

            if (resultado.success == true) {
                self.init();
                $("#ficha_add").addClass('hidden');
                toastr.success('Entidad Financiera Registrada Correctamente');
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

        var modelstr = JSON.stringify(model);

        debugger;
        var file = document.getElementById('fu_imagen_edit');

        var form = new FormData();
        form.append('jsBanco', modelstr);
        var indice = 0;
        if (file.files.length != 0) {
            indice = 1;
            var farchivo = file.files[0];
            form.append('fu_imagen', farchivo);
        }

        form.append('indice', indice);

        PostFile("configuracion-edit-financiera", form).done(function (resultado) {

            if (resultado.success == true) {
                debugger;
                self.init();
                $("#ficha_edit").addClass('hidden');
                toastr.info('Entidad Financiera actualizada Correctamente');
                
                document.getElementById('fu_imagen_edit').value = "";
            } else {
                toastr.error(resultado.error);
                return false;
            }


        });


    };

    this.eliminar = function () {


        var model = ko.toJS(self.actividadEliminarContext);
        PostJson("configuracion-eliminar-financiera", model).done(function (resultado) {

            if (resultado.success == true) {
                self.init();
                $("#modal_remove").addClass('hidden');
                toastr.info('Entidad Financiera eliminado Correctamente');
            } else {
                toastr.error(resultado.error);
                return false;
            }


        });

    };


    this.openModalEdit = function (element) {
        var act = ko.toJS(element);
        //console.log(carg);
        self.actividadEditContext.codigo(act.codigo);
        self.actividadEditContext.nombre(act.nombre);
        self.actividadEditContext.logo(act.logo);

        self.actividadEditContext.formatoCCI(act.formatoCCI);
        self.actividadEditContext.formatoCB(act.formatoCB);
        self.actividadEditContext.tipo(act.tipo);

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