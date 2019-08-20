function verficacionPagoModel() {
    var self = this;
    this.idTransaccion;
    this.idPago;
    this.tipoValidacion;
    this.observacion;
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

    this.bancoFideicomiso = ko.observable();
    this.monedaBancoFideicomiso = ko.observable();
    this.nroCuentaFideicomiso = ko.observable();
    this.RucFideicomiso = ko.observable();
    this.nombreFideicomiso = ko.observable();
    this.tipoCuentaFideicomiso = ko.observable();
    
}
/*-------------------------*/