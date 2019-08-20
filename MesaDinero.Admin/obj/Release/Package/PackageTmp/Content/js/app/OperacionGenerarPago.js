



function ViewModel() {

    var self = this;
  
    this.lstVerificarPagos = new GridModelSP("operaciones-lista-generar-pago", 0, 10);
    this.subastaContexto = new SubastaModel();
    this.contextoVerificar = new verficacionPagoModel();
    
    this.lstBancos = ko.observableArray([]);
    this.lstMoneda = ko.observableArray([]);
    this.codigoBanco = ko.observable();
    this.codigoMoneda = ko.observable();
    this.listaPagos = Array();

    this.codigoBanco.subscribe(function (val) {
            var chk = document.getElementById("idcheckgeneral");
            chk.checked = false;
            self.init();
    });

    this.codigoMoneda.subscribe(function (val) {
            var chk = document.getElementById("idcheckgeneral");
            chk.checked = false;
            self.init();
    });

    this.init = function () {
        self.listaPagos = Array();
        PostJson("common/getEntidadBancaria", null).done(function (result) {
            self.lstBancos(result.data);
            
        
        });

        PostJson("common/getTipoMoneda", null).done(function (result) {
            self.lstMoneda(result.data);
        });
        
        self.lstVerificarPagos.search({
            idFilter: 0,
            textFilter: self.codigoBanco(), /* Filtro Banco*/
            monedaFilter: 'PEN', 
            searchFilter: self.codigoMoneda() /* Filtro Moneda*/
        });
    };

    this.exportarTxt = function () {
        
        
        var frm = document.getElementById("frm");
        var lista = ko.toJS(self.lstVerificarPagos.datasource);
        var codigos = "";
        lista.forEach(function (elemento) {

            if (elemento.checkPago) {
                if (elemento.estadoExportado == 0) {
                    codigos += "" + elemento.idTransaccion + ",";
                }
                
            }
        });

        if (codigos.length < 1) {
            toastr.error('Debe Seleccionar una Transaccion ');
            return false;
        }
       
        frm.transacciones.value = codigos.substring(0, codigos.length - 1);
        //location.href = '@Url.Action("DescargarExportableTXT","Operador")';
        
        //frm.submit();
        for (var i = 0; i < self.lstVerificarPagos.datasource().length; i++) {

            if (self.lstVerificarPagos.datasource()[i].checkPago() == true) {
                self.lstVerificarPagos.datasource()[i].estadoExportado(1);
            }
        }

        document.forms["frm"].submit(function (event) {
               
            //event.preventDefault();
        });
        //$('#frm').submit(function () {
        //    console.log("hOLAA");
        //    debugger;
        //    $.ajax({
        //        url: $('#frm').attr('action'),
        //        type: 'POST',
        //        data: $('#frm').serialize(),
        //        success: function () {
        //            console.log('form submitted.');
        //        }
        //    });
        //    return false;
        //});
        //$("#frm").bind('ajax:complete', function () {
        //    debugger;
        //    for (var i = 0; i < self.lstVerificarPagos.datasource().length; i++) {

        //        if (self.lstVerificarPagos.datasource()[i].checkPago() == true) {
        //            self.lstVerificarPagos.datasource()[i].estadoExportado(1);
        //        }
        //    }
        //});

        
       


        //location.reload();
        //self.init();
    };

    this.descargar = function (elemento) {
        debugger;
        var lista = ko.toJS(elemento);
        console.log(lista);
        //window.location = '@Url.Action("Descargar","Operador")';
    };



    this.enviarSolicitud = function () {
        debugger;
        self.listaPagos = Array();
        var frm = document.getElementById("frm");
        var lista = ko.toJS(self.lstVerificarPagos.datasource);
        var codigos = "";
        var indiceExportado = true;
        lista.forEach(function (elemento) {
            debugger;
            if (elemento.checkEnviar) {
                debugger;
                if (elemento.estadoSubastaCodigo == 'P') {
                    if (elemento.estadoExportado == 1) {
                        this.verificarPago = new verficacionPagoModel();
                        this.verificarPago.idTransaccion = elemento.idTransaccion;
                        this.verificarPago.idPago = ""
                        this.verificarPago.observacion = "";
                        this.verificarPago.tipoValidacion = "EC";
                        this.verificarPago.estado = "M";
                        self.listaPagos.push(verificarPago);
                        codigos += "" + elemento.idTransaccion + ",";
                    } else {
                        indiceExportado = false;
                        codigos += "" + elemento.idTransaccion + ",";
                    }
               
                }
             
            }
        });

        if (codigos.length < 1) {
            toastr.error('Debe Seleccionar una Transaccion, con estado Pendiente');
            return false;
        }

        if (indiceExportado ==false) {
            toastr.error('Previamente ha Enviar Solicitud, debe generar todos los archivos de las transacciones SELECCIONADAS');
            return false;
        }

        var model = ko.toJS(self.listaPagos);
        
        PostJson("operaciones-actualiza-estado", model).done(function (resultado) {
            if (resultado.success == true) {
                self.init();
                $("#ficha_add_cargo").addClass('hidden');
                toastr.success('Se envio solicitud de pago Correctamente');
            } else {
                toastr.error(resultado.error);
                return false;
            }
        });
    };


    this.checkearAll = function () {
      
        var chk = document.getElementById("idcheckgeneral");
        //console.log(self.lstVerificarPagos.datasource());

        self.lstVerificarPagos.datasource().forEach(function (elemento) {
            elemento.checkPago(chk.checked);
        });

        //var allElems = document.getElementsByTagName('input');
        //for (i = 0; i < allElems.length; i++) {
        //    debugger;
        //    if (allElems[i].type == 'checkbox') {
        //        allElems[i].checked = chk.checked;
        //    }
        //}
     
    };

    this.checkearAllEnviar = function () {
        
        var chk = document.getElementById("idcheckgeneralEnviar");
        self.lstVerificarPagos.datasource().forEach(function (elemento) {
            elemento.checkEnviar(chk.checked);
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