function EmailModel()
{
    var self = this;
    this.email = ko.observable().extend({
        required: { params: true, message: "Campo Obligatorio" },
        email: { params: true, message: "Ingrese un correo electrónico válido." }
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
    this.emailContext = new EmailModel();

    this.confirmar = function () {

        if (self.emailContext.validarModelo() == false)
            return false;

        var data = {
            email : self.emailContext.email()
        };

        PostJson("recuperar-password", data, false).done(function (result) {
            debugger;
            if (result.success == true)
            {
                self.emailContext.email('');
                
                $("#popup_msm").removeClass('hidden');


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
