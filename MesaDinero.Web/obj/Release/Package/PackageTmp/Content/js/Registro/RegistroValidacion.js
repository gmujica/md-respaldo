function ViewModel()
{

    var self = this;

    this.enviarValidacion = function () {

        var data = {
            sid : window.sid
        };

        PostJson("registro/enviar-validacion", data, false).done(function (result) {

            if(result.success == true)
            {
                window.location.href = window.web_host + '/Registro/Operaciones/'  + result.data;
               // location.reload();

            } else {
                toastr.error(result.error);
                return false;
            }

        });

    };

}


$(function () {


    ko.applyBindings(new ViewModel());
});
