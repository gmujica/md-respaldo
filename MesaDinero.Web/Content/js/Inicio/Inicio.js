


var ViewModel = function () {
    var self = this;
    /*ACTUALIZAR AHORA*/
    notificaciones();
    this.start = function () {
        setInterval(function () { return notificaciones(); }, 1000);
    };

    this.listaNotificaciones = ko.observableArray();

    function notificaciones() {
        PostJson2("lista-notificaciones", false).done(function (resultado) {

            if (resultado.success == true) {

                self.listaNotificaciones(resultado.data);
            }
        });
    }

    this.enviar = function (val) {
        var ruta = ko.toJS(val);
        debugger;
        if (ruta.Url != "") {
            location.href = ruta.Url;
        }


        debugger;
    }
    self.start();

};



$(function () {


    ko.applyBindings(new ViewModel());


});