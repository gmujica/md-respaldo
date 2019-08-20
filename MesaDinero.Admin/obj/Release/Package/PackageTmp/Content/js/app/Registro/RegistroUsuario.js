function UsuarioModel() {
    var self = this;
    this.codigo = 0;
    this.nombre = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.apellidoPat = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.apellidoMat = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.email = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.tipoUsuario = ko.observable('').extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.rucEmpresa = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.nroDocumento = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.ructext = ko.observable();
    this.tipoDocumento = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.prefijo = ko.observable('').extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.celular = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });

    this.estado = ko.observable(1).extend({ required: { params: true, message: "Campo Obligatorio" } });

    this.nroDocumento.subscribe(function (val) {
        debugger;
        var data = {
            nro: val
        };

        PostJson("common-getdatospersonasimple", data).done(function (result) {

            if (result.success = true) {
                debugger;
                if (result.data != null) {
                    var per = result.data;

                    self.tipoDocumento(per.tipoDocumento);
                    self.nombre(per.nombres);
                    self.apellidoPat(per.apePaterno);
                    self.apellidoMat(per.apeMaterno);
                    //self.preCelular(per.preCelular);
                    //self.celular(per.celular);
                    //self.email(per.email);
                }
            } else {
                toastr.error(result.error);
                return false;
            }

        });

    });


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


function UsuarioPerfilModel() {
    var self = this;
    this.codigoPerfil = ko.observable();
    this.codigoUsuario = ko.observable();
}


function ViewModel() {

    var self = this;

    this.lst = new GridModelSP("listado-usuario", 0, 10);
    this.AddContext = ko.observable(new UsuarioModel());
    this.EditContext = new UsuarioModel();
    this.EliminarContext = new UsuarioModel();
    this.lstActividadEconomica = ko.observableArray([]);
    this.codigoEmpresa = ko.observable();
    this.tipoEmpresa = ko.observable();
    this.lstTipoDocumento = ko.observableArray([]);
    this.lstperfil = new GridModelSP("listado-perfil-usuario", 0, 10);
    this.userid = ko.observable('');
    this.UsuarioPerfilModel = ko.observableArray([]);

    self.clear = function () {
        self.AddContext(new UsuarioModel());
    }

    this.init = function () {

        PostJson("common/getTipoDocumentoPersona", null, false).done(function (result) {
            self.lstTipoDocumento(result.data);
        });

        self.lst.search({
            idFilter: 0,
            filter: '',
            searchFilter: ''
        });
    };

    self.AddContext().tipoUsuario.subscribe(function (val) {
        debugger;
        if (val) {
            debugger;
            PostJson("combo/empresa?filtro=" + val, false).done(function (result) {
                debugger;
                self.lstActividadEconomica(result.data);

                //self.AddContext().
            });
        } else {
            self.lstActividadEconomica.removeAll();
            //$("#combo_departamento_persona").trigger('chosen:updated');
        }
    });


    self.AddContext().rucEmpresa.subscribe(function (val) {
        debugger;
        if (val) {
            self.AddContext().ructext(val);
        } else {
            self.AddContext().ructext("");
            //self.lstActividadEconomica.removeAll();
            //$("#combo_departamento_persona").trigger('chosen:updated');
        }
    });

    this.openModalAdd = function () {
        $("#ficha_add_cargo").removeClass('hidden');
    };

    this.closeModalAdd = function () {
        $("#ficha_add_cargo").addClass('hidden');

    };
    var user = 0;
    this.openModalPerfil = function (elemento) {
        debugger;
        self.userid(elemento.codigo);
        user = ko.toJS(elemento.codigo);
        //self.EditContext=elemento;
        self.lstperfil.search({
            idFilter: 0,
            filter: '',
            textFilter: user,
            searchFilter: ''
        });
        debugger;
        $("#ficha_perfil").removeClass('hidden');
    };

    this.closeModalPerfil = function () {
        $("#ficha_perfil").addClass('hidden');

    };


    this.addNew = function () {
        debugger;
        if (self.AddContext().validarModelo() == false) {
            return false;
        }

        var model = ko.toJS(self.AddContext());

        PostJson("new-usuario", model).done(function (resultado) {
            debugger;
            if (resultado.success == true) {
                debugger;
                self.init();
                $("#ficha_add_cargo").addClass('hidden');
                toastr.success('Usuario Registrado Correctamente');
                self.clear();
            } else {
                toastr.error(resultado.error);
                return false;
            }


        });
    };

    this.editar = function () {
        debugger;
        if (self.EditContext.validarModelo() == false) {
            return false;
        }

        var model = ko.toJS(self.EditContext);

        PostJson("edit-usuario", model).done(function (resultado) {

            if (resultado.success == true) {
                self.init();
                $("#ficha_edit_cargo").addClass('hidden');
                toastr.info('Usuario editado Correctamente');
            } else {
                toastr.error(resultado.error);
                return false;
            }


        });


    };

    this.eliminar = function () {


        var model = ko.toJS(self.EliminarContext);
        console.log(model);
        PostJson("eliminar-usuario", model).done(function (resultado) {

            if (resultado.success == true) {
                self.init();
                $("#modal_remove_cargo").addClass('hidden');
                toastr.info('Usuario eliminado Correctamente');
            } else {
                toastr.error(resultado.error);
                return false;
            }


        });

    };

    this.asignarPerfil = function () {
        debugger;
        //var usuario = ko.toJS(self.EditContext);
        var codigoUsuario = user;

        var perfiles = ko.toJS(self.lstperfil.datasource);

        perfiles.forEach(function (elemento) {
            debugger;
            if (elemento.checkActivo) {
                debugger;
                var userPerfil = new UsuarioPerfilModel();
                userPerfil.codigoPerfil = elemento.codigo;
                userPerfil.codigoUsuario = codigoUsuario;
                self.UsuarioPerfilModel().push(userPerfil);
            }
        }
        );


        var model = ko.toJS(self.UsuarioPerfilModel());
        console.log(model);

        PostJson("new-perfil-usuario", model).done(function (resultado) {
            if (resultado.success == true) {
                self.init();
                $("#ficha_perfil").addClass('hidden');
                toastr.success('Perfil Asignado Correctamente');
                self.clear();
                self.UsuarioPerfilModel.removeAll();
            } else {
                toastr.error(resultado.error);
                return false;
            }


        });
    };

    this.openModalEdit = function (element) {
        debugger;
        var user = ko.toJS(element);

        self.EditContext.codigo = user.codigo;
        self.EditContext.nombre(user.nombre);
        self.EditContext.email(user.email);
        self.EditContext.tipoUsuario(user.tipoUsuario);

        PostJson("combo/empresa?filtro=" + user.tipoUsuario, false).done(function (result) {
            debugger;
            self.lstActividadEconomica(result.data);
            self.EditContext.rucEmpresa(user.rucEmpresa);
            self.EditContext.ructext(user.rucEmpresa);
            //self.AddContext().
        });

        self.EditContext.tipoUsuario.subscribe(function (val) {

            if (val) {

                PostJson("combo/empresa?filtro=" + val, false).done(function (result) {
                    debugger;
                    self.lstActividadEconomica(result.data);

                });
            } else {
                self.lstActividadEconomica.removeAll();

            }
        });

        self.EditContext.rucEmpresa.subscribe(function (val) {
            debugger;
            if (val) {
                self.EditContext.ructext(val);
            } else {
                self.EditContext.ructext("");
                //self.lstActividadEconomica.removeAll();
                //$("#combo_departamento_persona").trigger('chosen:updated');
            }
        });

        self.EditContext.rucEmpresa(user.rucEmpresa);
        self.EditContext.nroDocumento(user.nroDocumento);
        self.EditContext.estado(user.estado);
        self.EditContext.tipoDocumento(user.tipoDocumento);

        self.EditContext.nombre(user.nombre);
        self.EditContext.apellidoPat(user.apellidoPat);
        self.EditContext.apellidoMat(user.apellidoMat);

        self.EditContext.prefijo(user.preCelular);
        self.EditContext.celular(user.celular);



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

