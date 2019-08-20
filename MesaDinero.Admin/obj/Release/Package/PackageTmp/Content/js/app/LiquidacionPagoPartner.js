
function CodigosLiquidacion() {
    var self = this;
    this.nroSubasta;
    this.estado;
}


var ViewModel = function () {
    var self = this;
    this.estado = ko.observable('2');
    this.numeroLiquidar = ko.observable('');


    debugger;
    this.lstliquidacion = new GridModelSP("operador/lista-liquidacion-soloPartner", 0, 10);
    this.estado.subscribe(function (val) {
        debugger;
        self.init();
    });

    this.numeroLiquidar.subscribe(function (val) {
        debugger;
        self.init();
    });
    this.listaSubasta = Array();
    this.init = function () {

        self.listaSubasta = Array();
        self.lstliquidacion.search({
            idFilter: self.estado(),
            filter: '',
            searchFilter: self.numeroLiquidar()
        });

    };

    /*-------------Accion para confirmar pago Subasta --------------------*/
    //this.confirmarPago = function () {
    //    debugger;
    //    var lista = ko.toJS(self.lstliquidacion.datasource);
    //    var codigos = "";
    //    var obs = self.observ();
    //    var indiceId = true;
    //    lista.forEach(function (elemento) {
    //        if (elemento.checkPago) {
    //            this.verificarPago = new verficacionPagoModel();
    //            this.verificarPago.idTransaccion = elemento.idTransaccion;
    //            this.verificarPago.idPago = elemento.IngresarIdPago;
    //            this.verificarPago.observacion = obs;
    //            this.verificarPago.tipoValidacion = "AP";
    //            this.verificarPago.estado = "I";
    //            self.listaPagos.push(verificarPago);
    //            debugger;
    //            codigos += "" + elemento.idTransaccion + ",";
    //            if (elemento.IngresarIdPago == null || elemento.IngresarIdPago.trim() == "") {
    //                indiceId = false;
    //            }
    //        }
    //    });

    //    if (codigos.length < 1) {
    //        toastr.error('Debe Seleccionar una Transaccion ');
    //        return false;
    //    }

    //    if (indiceId == false) {
    //        toastr.error("Debe Ingresar ID Pago, en las Transacciones Seleccionadas");
    //        return false;
    //    }

    //    var model = ko.toJS(self.listaPagos);

    //    PostJson("operaciones-actualiza-estado", model).done(function (resultado) {
    //        if (resultado.success == true) {
    //            self.init();
    //            $("#ficha_add_cargo").addClass('hidden');
    //            toastr.success('Transacciones Aprobadas Correctamente');
    //        } else {
    //            toastr.error(resultado.error);
    //            return false;
    //        }
    //    });


    //};
    /*-----------------------------------------------------------*/

    this.generarLiquidacion = function () {
        debugger;
        var lista = ko.toJS(self.lstliquidacion.datasource);
        var codigos = "";
        var indiceId = true;
        lista.forEach(function (elemento) {
            debugger;
            if (elemento.checkEstado) {
                debugger;
                this.subastaLiquidar = new CodigosLiquidacion();
                this.subastaLiquidar.nroSubasta = elemento.idTransaccion;
                this.subastaLiquidar.checkear = elemento.checkEstado;
                this.subastaLiquidar.estado = "3"
                this.subastaLiquidar.numVoucher = elemento.numVoucher;
                self.listaSubasta.push(subastaLiquidar);
                codigos += "" + elemento.idTransaccion + ",";

                if (elemento.numVoucher == null || elemento.numVoucher.trim() == "") {
                    indiceId = false;
                }

            }
        });

        if (codigos.length < 1) {
            toastr.error('Debe Seleccionar una Transaccion ');
            return false;
        }

        if (indiceId == false) {
            toastr.error("Debe Ingresar Numero de Voucher, en las Transacciones Seleccionadas");
            return false;
        }

        debugger;

        var model = ko.toJS(self.listaSubasta);

        PostJson("liquidacion-pagar-partner", model).done(function (result) {
            if (result.success == true) {
                debugger;
                toastr.success('Se finaliza Liquidacion Correctamente');
                self.init();
                var chk = document.getElementById("idcheckgeneralEstado");
                chk.checked = false;
            } else {
                toastr.error(result.error);
                return false;
            }
        });
    };



    this.init();

    //setInterval(function () { self.init(); }, 10000);
    this.checkearAllEstado = function () {
        debugger;
        var chk = document.getElementById("idcheckgeneralEstado");
        self.lstliquidacion.datasource().forEach(function (elemento) {
            debugger;
            if (elemento.estadoNumLiq() == '2') {
                elemento.checkEstado(chk.checked);
            }



        });
    };

    this.exportarTxt = function () {
        debugger;
        var frm = document.getElementById("frm");
        var lista = ko.toJS(self.lstliquidacion.datasource);
        var codigos = "";

        frm.numPartner.value = "";
        frm.numLiq.value = self.numeroLiquidar();
        frm.estadoLiq.value = self.estado();
        frm.tipo.value = 1; // Tipo Partner
        document.forms["frm"].submit(function (event) {

        });
    }

    this.newVoucher = function (data, e) {
        debugger;

        if (e.keyCode === 13) {
            debugger;
            var mdata = ko.toJS(data);
            console.log(mdata);
            var lista = ko.toJS(self.lstliquidacion.datasource);
            self.lstliquidacion.datasource().forEach(function (elemento) {
                debugger;
                if (elemento.checkEstado()) {

                    if (elemento.estadoNumLiq() == '2') {
                        elemento.numVoucher(mdata.numVoucher);
                    }
                }

            });



            //var total = self.lstTiposCambio().length;
            //for (var i = total ; i > mdata.rango; i--) {
            //    if (mdata.rango < i) { self.lstTiposCambio.remove(self.lstTiposCambio()[i - 1]); }
            //}
            //self.lstTiposCambio.push(new TipoCambioGarantisado(mdata.rango + 1, Number(mdata.montoMaximo) + 1, null, 0, 0));
        }


    };
};


//var modelo = new ViewModel();


$(function () {


    ko.applyBindings(new ViewModel());

    // ko.applyBindings(modelo);
});