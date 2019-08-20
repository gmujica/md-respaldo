//function SubastaModel() {
//    var self = this;

//    this.idTransaccion=ko.observable();
//    this.tiempoRestante=ko.observable();

//    this.partnersAdjuntado=ko.observable();
//    this.usuario=ko.observable();
//    this.quiere=ko.observable();
//    this.tipoMoneda=ko.observable();
//    this.montoEnvia = ko.observable();
//    this.montoRecibe = ko.observable();

//    this.monedaEnvia = ko.observable();
//    this.monedaRecibe = ko.observable();

//    this.cuentaOrigen = ko.observable();
//    this.bancoOrigen = ko.observable();

//    this.origen = ko.computed(function () {
//        var banc = self.bancoOrigen() == null ? '' : self.bancoOrigen();
//        var cuent=self.cuentaOrigen()==null? '':self.cuentaOrigen();
//        return cuent ;
//    });

  
//    this.cuentaDestino = ko.observable();
//    this.bancoDestino = ko.observable();

//    this.destino = ko.computed(function () {
//        var banc = self.bancoDestino() == null ? '' : self.bancoDestino();
//        var cuent = self.cuentaDestino() == null ? '' : self.cuentaDestino();

//        return  cuent;
//    });
//    this.precioPactado=ko.observable();

//    this.fechaFinPago = ko.observable();
//    this.fechaHora = ko.observable();
//    this.totalm=ko.observable();
//    this.estado=ko.observable();
//    this.estadoSubasta=ko.observable();
//    this.total = ko.observable();
//    this.formatoTransaccion = ko.observable();
//    this.logoOrigen = ko.observable();
//    this.logoDestino = ko.observable();
//}


var ViewModel = function () {
    var self = this;
    this.subastaContexto = new SubastaModel();
    //var fec = new Date();

    let meses = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"];

    var fecha = new Date(); //Fecha actual
    var mes = fecha.getMonth(); //obteniendo mes
    var dia = fecha.getDate(); //obteniendo dia
    var anio = fecha.getFullYear(); //obteniendo año
    if (dia < 10)
        dia = '0' + dia; //agrega cero si el menor de 10
    
    this.fechaSistema = dia + ' ' + meses[mes] + " " + anio;
    
    this.lstOperacionesConfirmadasRegistradosT = new GridModelSP_Dinamico("operaciones-confirmadasT", 0, 10);


    /*Modal Instruccion Pago*/
    this.contextoVerificar = new verficacionPagoModel();
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
    //this.openModal = function (element) {
       
    //    var trasaccion = ko.toJS(element);

    //    self.subastaContexto.idTransaccion(trasaccion.idTransaccion);
    //    self.subastaContexto.partnersAdjuntado(trasaccion.partnersAdjuntado);
    //    self.subastaContexto.precioPactado(trasaccion.precioPactado);
    //    self.subastaContexto.usuario(trasaccion.usuario);
    //    self.subastaContexto.montoEnvia(trasaccion.monto);
    //    self.subastaContexto.formatoTransaccion(trasaccion.formatoTransaccion);
        
    //    self.subastaContexto.montoRecibe(trasaccion.montoRecibe);
    //    self.subastaContexto.monedaEnvia(trasaccion.monedaEnvia);
    //    self.subastaContexto.monedaRecibe(trasaccion.monedaRecibe);

    //    self.subastaContexto.cuentaOrigen(trasaccion.cuentaOrigen);
    //    self.subastaContexto.bancoOrigen(trasaccion.bancoOrigen);
    //    self.subastaContexto.cuentaDestino(trasaccion.cuentaDestino);
    //    self.subastaContexto.bancoDestino(trasaccion.bancoDestino);

    //    self.subastaContexto.logoOrigen(trasaccion.logoOrigen);
    //    self.subastaContexto.logoDestino(trasaccion.logoDestino);
     
    //    self.subastaContexto.fechaHora(trasaccion.fechaHora);

    //    $("#ficha").removeClass('hidden');
    //};

    //this.closeModal = function () {
        
    //    $("#ficha").addClass('hidden');

    //};




    this.init = function () {
        
        self.lstOperacionesConfirmadasRegistradosT.search({
            idFilter: 0,
            filter: '',
            searchFilter: ''
        });

    };
    
    this.init();
    //setInterval(function () { self.init(); }, 10000);
    setInterval(function () { self.lstOperacionesConfirmadasRegistradosT.mantener(); }, 1000);

};


//var modelo = new ViewModel();


$(function () {

    
    ko.applyBindings(new ViewModel());

   // ko.applyBindings(modelo);
});