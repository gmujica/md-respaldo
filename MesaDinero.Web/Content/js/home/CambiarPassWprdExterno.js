function CambiarPassWordExterno(){

    var self = this;

    this.password = ko.observable('');

    this.cambiarPassword = function () {

        var data = {
            sid: window.sid,
            clave : self.password()
        };

        PostJson("cambiar-password-recuperacion", data, false).done(function (result) {

            if(result.success == true)
            {
                window.location.href = window.urlApp + 'Acceso';

            } else {
                toastr.error(result.error);
                return false;
            }

        });


    };



};

$(function () {

    ko.applyBindings(new CambiarPassWordExterno());

});