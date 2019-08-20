var ViewModel = function () {

    var initMinuto = 0, initSegundo = 0;
    var self = this;
    this.clavemsm = ko.observable('');
    this.generarNuevoMsM = ko.observable(false);
    this.minutos = ko.observable(initMinuto);
    this.strMinutos = ko.computed(function () {
        if (self.minutos() < 10)
            return ('0' + self.minutos());
        else
            return(self.minutos());
    });
    this.segundos = ko.observable(initSegundo);
    this.strSegundos = ko.computed(function () {
        if (self.segundos() < 10)
            return ('0' + self.segundos());
        else
            return (self.segundos());
    });
 

    this.start = function () {
        setInterval(function () { return tick(); }, 1000);
    };

    function tick() {
        if (self.generarNuevoMsM() == false) {
            self.segundos(self.segundos() - 1);
            if (self.segundos() < 0) {
                self.segundos(59);
                self.minutos(self.minutos() - 1);
                if (self.minutos() < 0 || (self.minutos() == 0 && self.segundos() <= 1)) {
                    self.generarNuevoMsM(true);
                    self.minutos(0);
                    self.segundos(0);
                }
            }
        }        
    };

    this.EnviarMsM = function () {

        PostJson("registro/enviarMsM/" + window.SecredId, null).done(function (result) {

            if (result.success == true)
            {
                self.minutos(initMinuto);
                self.segundos(initSegundo);

                self.generarNuevoMsM(false);
                
            }
            else {
                alert(result.error);
            }

        });

    };

    this.validarSMS = function () {

        if (!self.clavemsm())
        {
            toastr.warning("Debe ingresar la clave SMS");
            return false;
        }

        var data = {
            sid: window.SecredId,
            clavemsm: self.clavemsm()
        };

        PostJson("registro/validar-sms", data).done(function (result) {

            if (result.success == true) {
                window.location.href = window.urlApp + "Registro/DatosBasicos/" + result.data;

            } else {
                toastr.error(result.error);
            }

        });

    };

    this.init = function () {

        Post("common/time-confirmacionMsM", null).done(function (result) {
                       
            initMinuto = result.minutos;
            initSegundo = result.segundos;
            self.minutos(initMinuto);
            self.segundos(initSegundo);
            self.start();
                      
        });
    };
}

var modelo = new ViewModel();

$(function () {
    modelo.init();
    ko.applyBindings(modelo);

   

});

