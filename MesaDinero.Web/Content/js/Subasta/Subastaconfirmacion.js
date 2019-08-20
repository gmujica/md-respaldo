var ViewModel = function () {

    var self = this;

    this.password = ko.observable('');
    this.tiempo = ko.observable('--');


    this.start = function () {
        setInterval(function () { return tick(); }, 1000);
    };

    function tick() {

        var data = {
            sid: window.sid
        };

        PostJson2("common/getTiempoSubastaCurrent", data, true).done(function (result) {

            if (result.success == true) {
                //if (result.data.tiempo >= 0)
                //    self.tiempo(result.data.tiempo);
                if (result.data.tiempo >= 0) {
                    //16-06-2019
                    var secs = result.data.tiempo;
                    var hours = Math.floor(secs / (60 * 60));

                    var divisor_for_minutes = secs % (60 * 60);
                    var minutes = Math.floor(divisor_for_minutes / 60);
                    minutes = minutes < 10 ? '0' + minutes : minutes;

                    var divisor_for_seconds = divisor_for_minutes % 60;
                    var seconds = Math.ceil(divisor_for_seconds);
                    seconds = seconds < 10 ? '0' + seconds : seconds;

                    var formato = minutes + ":" + seconds;
                    self.tiempo(formato);

                } else {
                    $('#popup_concluir').removeClass('hidden');
                }
            } else {

            }

        });




    };

    this.confirmarPassword = function () {

        var data = {
            subasta: window.sid,
            clave: self.password()
        };

        PostJson("subasta/saveConfirmacionSubasta", data, true).done(function (result) {

            if (result.success == true) {

                window.location.href = window.urlWebHost + '/Subasta/Envio/' + result.data;




            } else {
                toastr.error(result.error);
                return false;
            }


        });


    };

    self.start();

};



$(function () {

    ko.applyBindings(new ViewModel());
    debugger;
    window.onbeforeunload = function (e) {
        debugger;
        window.wData.data.initSubasta=1;
        //e.preventDefault();
        
        //window.location.href = window.urlWebHost + '/Subasta/Activa/' + window.sid;
        //return "Esta seguro que quieres salir de la pagina";
        //if (window.history.back()) {
        //    debugger;
          
        //    //href="@Url.Action("Activa", "Subasta", new { id = Model.sid })"
        //}
        
    };
});