
function CodigosLiquidacion() {
    var self = this;
    this.nroSubasta;
    this.estado;
}


var ViewModel = function () {
    var self = this;
    
    this.lstpartner = ko.observableArray([]);
    this.codigoPartner = ko.observable('');
    this.numeroLiquidar = ko.observable('');
    this.estado = ko.observable('0');
    debugger;
    this.lstliquidacion = new GridModelSP("operador/lista-liquidacion-partners", 0, 10);
    
    this.listaSubasta = Array();

    this.estado.subscribe(function (val) {
        debugger;
        self.init();
    });

    this.codigoPartner.subscribe(function (val) {
        debugger;
        self.init();
    });

    this.numeroLiquidar.subscribe(function (val) {
        debugger;
        self.init();
    });


    this.init = function () {
        PostJson("combo/empresa?filtro=PAR", false).done(function (result) {
            debugger;
            self.lstpartner(result.data);

            //self.AddContext().
        });

        
        self.listaSubasta = Array();
        self.lstliquidacion.search({
            idFilter: self.estado(),
            filter: '',
            textFilter: self.codigoPartner(), /* Filtro Banco*/
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
                this.subastaLiquidar.estado = "1"
               
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
       
        PostJson("liquidacion-generar-codigo", model).done(function (result) {
            if (result.success == true) {
                debugger;
                self.init();
                toastr.success('Se genero pre-liquidacion Correctamente');
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
        //lista.forEach(function (elemento) {

        //    if (elemento.checkPago) {
        //        //if (elemento.estadoExportado == 0) {
        //        codigos += "" + elemento.idTransaccion + ",";
        //        //}
        //    }
        //});

        //if (codigos.length < 1) {
        //    toastr.error('Debe Seleccionar una Transaccion ');
        //    return false;
        //}
        //debugger;
        //frm.transacciones.value = "102";
        debugger;
        var codigoPartner = "";
        if (self.codigoPartner() == undefined) {
            codigoPartner = ""
        } else {
            codigoPartner = self.codigoPartner();
        } 
        frm.numPartner.value = codigoPartner;
        frm.numLiq.value = self.numeroLiquidar();
        frm.estadoLiq.value = self.estado();
        frm.tipo.value = 0; //Tipo Lmd
        document.forms["frm"].submit(function (event) {

        });
    }

};


//var modelo = new ViewModel();


$(function () {


    ko.applyBindings(new ViewModel());

    // ko.applyBindings(modelo);
});