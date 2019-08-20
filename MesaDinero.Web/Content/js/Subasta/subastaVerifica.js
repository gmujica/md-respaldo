function ViewModel() {

    var self = this;
    this.contextoVerificar = new verficacionPagoModel();
    this.subastaContexto = new SubastaModel();
    /*Modal Instruccion Pago*/
    this.estadoVerifica = ko.observable('E');

    setInterval(function () { return actualizarEstado(); }, 400);

    function actualizarEstado() {
     
        var idsubatas = document.getElementById('idtransacion').value;
        self.contextoVerificar.idTransaccion = idsubatas;
        var models = ko.toJS(self.contextoVerificar);

        PostJson("subasta-estado-verifica", models).done(function (resultado) {
            if (resultado.success == true) {
              
                var transaccion = ko.toJS(resultado.data);
                self.estadoVerifica(transaccion.estado);

            } else {
                toastr.error(resultado.error);
                return false;
            }
        });

    }

   

    this.openModal = function () {
        debugger;

        var idsubata=document.getElementById('idtransacion').value;
        self.contextoVerificar.idTransaccion = idsubata;
        self.contextoVerificar.estado = "H"
        var model = ko.toJS(self.contextoVerificar);

        PostJson("subasta-instruccion-pago", model).done(function (resultado) {
            if (resultado.success == true) {
                debugger;
                var transaccion = ko.toJS(resultado.data[0]);

                self.subastaContexto.idTransaccion(transaccion.idTransaccion);
                self.subastaContexto.partnersAdjuntado(transaccion.partnersAdjuntado);
                self.subastaContexto.precioPactado(transaccion.precioPactado);
                self.subastaContexto.usuario(transaccion.usuario);
                self.subastaContexto.montoEnvia(transaccion.monto);
                self.subastaContexto.formatoTransaccion(transaccion.formatoTransaccion);

                self.subastaContexto.montoRecibe(transaccion.montoRecibe);
                self.subastaContexto.monedaEnvia(transaccion.monedaEnvia);
                self.subastaContexto.monedaRecibe(transaccion.monedaRecibe);

                self.subastaContexto.cuentaOrigen(transaccion.cuentaOrigen);
                self.subastaContexto.bancoOrigen(transaccion.bancoOrigen);
                self.subastaContexto.cuentaDestino(transaccion.cuentaDestino);
                self.subastaContexto.bancoDestino(transaccion.bancoDestino);

                self.subastaContexto.logoOrigen(transaccion.logoOrigen);
                self.subastaContexto.logoDestino(transaccion.logoDestino);

                self.subastaContexto.fechaHora(transaccion.fechaHora);

                self.subastaContexto.bancoFideicomiso(transaccion.bancoFideicomiso);
                self.subastaContexto.monedaBancoFideicomiso(transaccion.monedaBancoFideicomiso);
                self.subastaContexto.nroCuentaFideicomiso(transaccion.nroCuentaFideicomiso);
                self.subastaContexto.RucFideicomiso(transaccion.RucFideicomiso);
                self.subastaContexto.nombreFideicomiso(transaccion.nombreFideicomiso);
                self.subastaContexto.tipoCuentaFideicomiso(transaccion.tipoCuentaFideicomiso);

                $("#ficha").removeClass('hidden');
            } else {
                toastr.error(resultado.error);
                return false;
            }
        });
    };
    this.closeModal = function () {

        $("#ficha").addClass('hidden');
    };
    /*---------------------*/

}


//$(function () {
//    ko.applyBindings(new ViewModel());

//});

var modelo = new ViewModel();

$(function () {

    //modelo.init();
    ko.applyBindings(modelo);

});