function ContinuarRegistroModel() {
    var self = this;

    this.email = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.nroDocumento = ko.observable().extend({ required: { params: true, message: "Campo Obligatorio" } });

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

function Login() {
    var self = this;
    this.email = ko.observable("").extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.password = ko.observable("").extend({ required: { params: true, message: "Campo Obligatorio" } });

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

    this.loginContext = new Login();
    this.continuarRegistroContext = new ContinuarRegistroModel();

    this.login = function () {

        if (!self.loginContext.validarModelo()) {
            return false;
        }

        var data = ko.toJS(self.loginContext);

        PostJson_Host("Acceso/Login", data, false).done(function (result) {

            if (result.success == true) {
                debugger;
                if (window.returnUrl)
                    window.location.href = window.w_host +"/"+ window.returnUrl;
                else
                    window.location.href = window.w_host + "/Inicio";

            } else {
                toastr.error(result.error);
            }

        });


    };

    this.openModalRecuperarRegistro = function () {
        $("#modal-continuar-registro").removeClass("hidden");
    };
    this.closeModalRecuperarRegistro = function () {
        $("#modal-continuar-registro").addClass("hidden");
    };
    this.continuarRegistro = function () {
        if (self.continuarRegistroContext.validarModelo() == false) {
            return false;
        }

        var data = ko.toJS(self.continuarRegistroContext);

        PostJson("registro-reanudar", data, true).done(function (result) {

            if (result.success == true) {
                if (result.data != '') {
                    window.location.href = window.w_host + "/Registro/Continuar/" + result.data;
                } else {
                    toastr.warning("No tiene ningun registro en proceso.");
                }


            } else {
                toastr.error(result.error);
                return false;
            }

        });


    };

};

$(function () {

    ko.applyBindings(new ViewModel());
});