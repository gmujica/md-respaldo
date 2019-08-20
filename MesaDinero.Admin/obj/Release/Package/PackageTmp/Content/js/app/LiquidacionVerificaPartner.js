
function CodigosLiquidacion() {
    var self = this;
    this.nroSubasta;
    this.estado;
}


var ViewModel = function () {
    var self = this;
    this.estado = ko.observable('1');
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
    //this.generarLiquidacion = function () {
    //    alert("Hola");
    //};
    
    this.generarLiquidacion = function () {
        debugger;
        var lista = ko.toJS(self.lstliquidacion.datasource);
        var codigos = "";
        lista.forEach(function (elemento) {
            debugger;
            if (elemento.checkEstado) {
                debugger;
                this.subastaLiquidar = new CodigosLiquidacion();
                this.subastaLiquidar.nroSubasta = elemento.idTransaccion;
                this.subastaLiquidar.checkear = elemento.checkEstado;
                this.subastaLiquidar.estado = "2";

                self.listaSubasta.push(subastaLiquidar);

                codigos += "" + elemento.idTransaccion + ",";

            }
        });

        if (codigos.length < 1) {
            toastr.error('Debe Seleccionar una Transaccion ');
            return false;
        }
        debugger;
        codigos = codigos.substring(0, codigos.length - 1);
        var model = ko.toJS(self.listaSubasta);

        PostJson("liquidacion-aprueba-partner", model).done(function (result) {
            if (result.success == true) {
                debugger;
                self.init();
                toastr.success('Se aprobo liquidaciones Correctamente');
            } else {
                toastr.error(result.error);
                return false;
            }
        });



    };


    this.rechazarLiquidacion = function () {
        debugger;
        var lista = ko.toJS(self.lstliquidacion.datasource);
        var codigos = "";
        lista.forEach(function (elemento) {
            debugger;
            if (elemento.checkEstado) {
                debugger;
                this.subastaLiquidar = new CodigosLiquidacion();
                this.subastaLiquidar.nroSubasta = elemento.idTransaccion;
                this.subastaLiquidar.checkear = elemento.checkEstado;
                this.subastaLiquidar.estado = "0";

                self.listaSubasta.push(subastaLiquidar);

                codigos += "" + elemento.idTransaccion + ",";

            }
        });

        if (codigos.length < 1) {
            toastr.error('Debe Seleccionar una Transaccion ');
            return false;
        }
        debugger;
        codigos = codigos.substring(0, codigos.length - 1);
        var model = ko.toJS(self.listaSubasta);

        PostJson("liquidacion-rechaza-partner", model).done(function (result) {
            if (result.success == true) {
                debugger;
                self.init();
                toastr.success('Se rechazo liquidacion Correctamente');
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
            elemento.checkEstado(chk.checked);
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



};


//var modelo = new ViewModel();


$(function () {


    ko.applyBindings(new ViewModel());

    // ko.applyBindings(modelo);
});