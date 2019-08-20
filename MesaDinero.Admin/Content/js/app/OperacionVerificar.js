


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
            textFilter: '1',
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
                this.verificarPago.tipoValidacion = "VO";
                this.verificarPago.estado = "G"; 
                self.listaPagos.push(verificarPago);
                /*G*/
                codigos += ""+elemento.idTransaccion+",";
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



    this.checkearAll = function () {
        var chk = document.getElementById("idcheckgeneral");
        self.lstVerificarPagos.datasource().forEach(function (elemento) {
            elemento.checkPago(chk.checked);
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
                this.verificarPago.tipoValidacion = "VO";
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



    /*Modal Instruccion Pago*/
    //this.openModal = function (element) {
    //    debugger;
    //    var trasaccion = ko.toJS(element);
    //    self.contextoVerificar.idTransaccion = trasaccion.idTransaccion;
    //    self.contextoVerificar.estado = "H"
    //    var model = ko.toJS(self.contextoVerificar);

    //    PostJson("operaciones-instruccion-pago", model).done(function (resultado) {
    //        if (resultado.success == true) {
    //            debugger;
    //            var transaccion = ko.toJS(resultado.data[0]);
                
    //            self.subastaContexto.idTransaccion(transaccion.idTransaccion);
    //            self.subastaContexto.partnersAdjuntado(transaccion.partnersAdjuntado);
    //            self.subastaContexto.precioPactado(transaccion.precioPactado);
    //            self.subastaContexto.usuario(transaccion.usuario);
    //            self.subastaContexto.montoEnvia(transaccion.monto);
    //            self.subastaContexto.formatoTransaccion(transaccion.formatoTransaccion);

    //            self.subastaContexto.montoRecibe(transaccion.montoRecibe);
    //            self.subastaContexto.monedaEnvia(transaccion.monedaEnvia);
    //            self.subastaContexto.monedaRecibe(transaccion.monedaRecibe);

    //            self.subastaContexto.cuentaOrigen(transaccion.cuentaOrigen);
    //            self.subastaContexto.bancoOrigen(transaccion.bancoOrigen);
    //            self.subastaContexto.cuentaDestino(transaccion.cuentaDestino);
    //            self.subastaContexto.bancoDestino(transaccion.bancoDestino);

    //            self.subastaContexto.logoOrigen(transaccion.logoOrigen);
    //            self.subastaContexto.logoDestino(transaccion.logoDestino);

    //            self.subastaContexto.fechaHora(transaccion.fechaHora);

    //            $("#ficha").removeClass('hidden');
    //        } else {
    //            toastr.error(resultado.error);
    //            return false;
    //        }
    //    });
    //};
    /*---------------------*/

};

var modelo = new ViewModel();

$(function () {

    modelo.init();
    ko.applyBindings(modelo);

});