﻿function PasswordClass() {
    var self = this;

    this.sid = window.sid;

    this.password = ko.observable('').extend({
        required: { params: true, message: "Campo Obligatorio" }
    });

    this.rePassword = ko.observable('').extend({
        required: { params: true, message: "Campo Obligatorio" }
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


var ViewModel = function () {

    var self = this;

    this.passwordContext = new PasswordClass();

    this.crearPassword = function () {

        if (!self.passwordContext.validarModelo()) {
            return false;
        }

        var pass = self.passwordContext.password();

        if (pass.length < 6 || pass.length > 10) {
            toastr.warning("La contraseña debe tener entre 6 a 10 caracteres.");
            return false;
        }


        var hashNumber = pass.replace(/\D/g, '');
        var hashSpecialCharacter = /[ !@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/.test(pass);
        var hashLetters = pass.replace(/[^A-Za-z]+/g, '')

        if (hashNumber.length <= 0 || hashLetters.length <= 0) {
            toastr.warning("La contraseña debe constar de letras y numeros .");
            return false;
        }

        if (pass != self.passwordContext.rePassword()) {
            toastr.warning("Las contraseñas no coinciden. Por favor asegurece de haber escrito bien su contraceña en ambos campos.");
            return false;
        }

        var data = ko.toJS(self.passwordContext);
        debugger;
        PostJson_Host("Account/CrearUsuarioAdm", data, false).done(function (result) {
            debugger;
            if (result.success == true) {

                window.location.href = window.host + '/';

            } else {

                toastr.error(result.error);
                return false;
            }

        });


    };


    this.init = function () {

    };


};

var modelo = new ViewModel();

$(function () {

    modelo.init();
    ko.applyBindings(modelo);

});

function tooglePassword(el) {

    if ($(el).attr("type") == 'text') {
        $(el).attr("type", "password")
    } else {
        $(el).attr("type", "text")
    }

}
