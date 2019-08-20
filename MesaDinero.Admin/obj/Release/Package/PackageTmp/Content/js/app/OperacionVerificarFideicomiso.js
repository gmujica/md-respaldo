function verficacionPagoModel() {
    var self = this;
    this.idTransaccion;
    this.tipoValidacion;
    this.estado;
}

/* Modal Instruccion de Pago*/
function SubastaModel() {
    var self = this;

    this.idTransaccion = ko.observable();
    this.tiempoRestante = ko.observable();

    this.partnersAdjuntado = ko.observable();
    this.usuario = ko.observable();
    this.quiere = ko.observable();
    this.tipoMoneda = ko.observable();
    this.montoEnvia = ko.observable();
    this.montoRecibe = ko.observable();

    this.monedaEnvia = ko.observable();
    this.monedaRecibe = ko.observable();

    this.cuentaOrigen = ko.observable();
    this.bancoOrigen = ko.observable();

    this.origen = ko.computed(function () {
        var banc = self.bancoOrigen() == null ? '' : self.bancoOrigen();
        var cuent = self.cuentaOrigen() == null ? '' : self.cuentaOrigen();
        return cuent;
    });


    this.cuentaDestino = ko.observable();
    this.bancoDestino = ko.observable();

    this.destino = ko.computed(function () {
        var banc = self.bancoDestino() == null ? '' : self.bancoDestino();
        var cuent = self.cuentaDestino() == null ? '' : self.cuentaDestino();

        return cuent;
    });
    this.precioPactado = ko.observable();

    this.fechaFinPago = ko.observable();
    this.fechaHora = ko.observable();
    this.totalm = ko.observable();
    this.estado = ko.observable();
    this.estadoSubasta = ko.observable();
    this.total = ko.observable();
    this.formatoTransaccion = ko.observable();
    this.logoOrigen = ko.observable();
    this.logoDestino = ko.observable();
}
/*-------------------------*/


function ViewModel() {

    var self = this;
    debugger;
    this.lstVerificarPagos = new GridModelSP("operaciones-verificar-pago", 0, 10);
    this.subastaContexto = new SubastaModel();
    this.contextoVerificar = new verficacionPagoModel();
    this.listaPagos = Array();

    this.init = function () {
        self.listaPagos = Array();
        self.lstVerificarPagos.search({
            idFilter: 0,
            textFilter: '2',
            searchFilter: ''
        });
    };

    this.verificarOperacion = function () {

        var lista = ko.toJS(self.lstVerificarPagos.datasource);
        var codigos = "";
        lista.forEach(function (elemento) {
            debugger;
            if (elemento.checkPago) {
                this.verificarPago = new verficacionPagoModel();
                this.verificarPago.idTransaccion = elemento.idTransaccion;
                this.verificarPago.idPago = ""
                this.verificarPago.observacion = "";
                this.verificarPago.tipoValidacion = "VF";
                this.verificarPago.estado = "G";
                self.listaPagos.push(verificarPago);
                /*G*/
                codigos += "" + elemento.idTransaccion + ",";
            }
        });

        if (codigos.length < 1) {
            toastr.error('Debe Seleccionar una Transaccion ');
            return false;
        }


        var model = ko.toJS(self.listaPagos);

        PostJson("operaciones-actualiza-estado", model).done(function (resultado) {
            if (resultado.success == true) {
                self.init();
                $("#ficha_add_cargo").addClass('hidden');
                toastr.success('Transacciones Verificadas Correctamente');
            } else {
                toastr.error(resultado.error);
                return false;
            }
        });


    };

    this.observarOperacion = function () {
        debugger;
        console.log(ko.toJS(self.lstVerificarPagos.datasource));
        var lista = ko.toJS(self.lstVerificarPagos.datasource);
        var codigos = "";
        lista.forEach(function (elemento) {

            if (elemento.checkPago) {
                this.verificarPago = new verficacionPagoModel();
                this.verificarPago.idTransaccion = elemento.idTransaccion;
                this.verificarPago.idPago = ""
                this.verificarPago.observacion = "";
                this.verificarPago.tipoValidacion = "VF";
                this.verificarPago.estado = "H";
                self.listaPagos.push(verificarPago);

                codigos += "" + elemento.idTransaccion + ",";
            }
        });

        if (codigos.length < 1) {
            toastr.error('Debe Seleccionar una Transaccion ');
            return false;
        }

        var model = ko.toJS(self.listaPagos);

        PostJson("operaciones-actualiza-estado", model).done(function (resultado) {
            if (resultado.success == true) {
                self.init();
                $("#ficha_add_cargo").addClass('hidden');
                toastr.success('Transacciones NO Encontrada');
            } else {
                toastr.error(resultado.error);
                return false;
            }
        });
    };

    /*Modal Instruccion Pago*/
    this.openModal = function (element) {
        debugger;
        var trasaccion = ko.toJS(element);
        self.contextoVerificar.idTransaccion = trasaccion.idTransaccion;
        self.contextoVerificar.estado = "H"
        var model = ko.toJS(self.contextoVerificar);

        PostJson("operaciones-instruccion-pago", model).done(function (resultado) {
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

                $("#ficha").removeClass('hidden');
            } else {
                toastr.error(resultado.error);
                return false;
            }
        });
    };
    /*---------------------*/

};

var modelo = new ViewModel();

$(function () {

    modelo.init();
    ko.applyBindings(modelo);

});