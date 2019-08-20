
var ViewModel = function () {
    var self = this;
    this.subastaContexto = new SubastaModel();
    this.tipo = ko.observable('G');



    this.lstOperacionesConfirmadasRegistradosT = new GridModelSP_Dinamico("corfid-operaciones-historicas", 0, 10);

    this.listaPagado = function () {
        $('#general').removeClass('active');
        $('#curso').removeClass('active');
        $('#pagado').addClass('active');
        $('#Incumplidas').removeClass('active');

        self.tipo('P');
        self.init();
    };

    this.listarCurso = function () {
        $('#general').removeClass('active');
        $('#pagado').removeClass('active');
        $('#Incumplidas').removeClass('active');
        $('#curso').addClass('active');

        self.tipo('C');
        self.init();
    };

    this.listarIncumplidas = function () {
        $('#general').removeClass('active');
        $('#curso').removeClass('active');
        $('#Incumplidas').addClass('active');
        $('#Curso').removeClass('active');
        $('#pagado').removeClass('active');

        self.tipo('I');
        self.init();
    };

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

    this.checkearAll = function () {
        debugger;
        var chk = document.getElementById("idcheckgeneral");
        self.lstOperacionesConfirmadasRegistradosT.datasource().forEach(function (elemento) {
            elemento.checkPago(chk.checked);
        });
    };

    this.init = function () {

        self.lstOperacionesConfirmadasRegistradosT.search({
            idFilter: 0,
            filter: '',
            searchFilter: '',
            textFilter: self.tipo()
        });

    };

    this.init();


    this.exportarTxt = function () {
        debugger;
        var frm = document.getElementById("frm");
        var lista = ko.toJS(self.lstOperacionesConfirmadasRegistradosT.datasource);
        var codigos = "";
        lista.forEach(function (elemento) {

            if (elemento.checkPago) {
                //if (elemento.estadoExportado == 0) {
                codigos += "" + elemento.idTransaccion + ",";
                //}
            }
        });

        if (codigos.length < 1) {
            toastr.error('Debe Seleccionar una Transaccion ');
            return false;
        }
        debugger;
        frm.transacciones.value = codigos.substring(0, codigos.length - 1);

        document.forms["frm"].submit(function (event) {

        });
    }


};


//var modelo = new ViewModel();


$(function () {


    ko.applyBindings(new ViewModel());

    // ko.applyBindings(modelo);
});