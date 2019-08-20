
function CodigosLiquidacion() {
    var self = this;
    this.nroSubasta;
    this.estado;
}


var ViewModel = function () {
    var self = this;

    this.lstpartner = ko.observableArray([]);
   
    this.lstliquidacion = new GridModelSP("partner/lista-adjudicaciones", 0, 10);

    this.listaSubasta = Array();
    

    this.init = function () {
        PostJson("combo/empresa?filtro=PAR", false).done(function (result) {
            debugger;
            self.lstpartner(result.data);

            //self.AddContext().
        });


        self.listaSubasta = Array();
        self.lstliquidacion.search({
            idFilter: 0,
            filter: '',
            textFilter:'', /* Filtro Banco*/
            searchFilter: ''
        });



    };

    //this.generarLiquidacion = function () {
    //    debugger;
    //    var lista = ko.toJS(self.lstliquidacion.datasource);
    //    var codigos = "";
    //    lista.forEach(function (elemento) {
    //        debugger;
    //        if (elemento.checkEstado) {
    //            debugger;
    //            this.subastaLiquidar = new CodigosLiquidacion();
    //            this.subastaLiquidar.nroSubasta = elemento.idTransaccion;
    //            this.subastaLiquidar.estado = "1"

    //            self.listaSubasta.push(subastaLiquidar);




    //            codigos += "" + elemento.idTransaccion + ",";

    //        }
    //    });

    //    if (codigos.length < 1) {
    //        toastr.error('Debe Seleccionar una Transaccion ');
    //        return false;
    //    }
    //    debugger;
    //    codigos = codigos.substring(0, codigos.length - 1);
    //    var model = ko.toJS(self.listaSubasta);

    //    PostJson("liquidacion-generar-codigo", model).done(function (result) {
    //        if (result.success == true) {
    //            debugger;
    //            self.init();
    //            toastr.success('Se genero pre-liquidacion Correctamente');
    //        } else {
    //            toastr.error(result.error);
    //            return false;
    //        }
    //    });



    //};



    this.init();
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
        //var lista = ko.toJS(self.lstliquidacion.datasource);
            //document.submit;
        //frm.numPartner.value = codigoPartner;
        //frm.numLiq.value = self.numeroLiquidar();
        //frm.estadoLiq.value = self.estado();
        //frm.tipo.value = 0; //Tipo Lmd
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
        frm.operaciones.value = codigos.substring(0, codigos.length - 1);
        if (codigos.length < 1) {
            toastr.error('Debe Seleccionar una Transaccion ');
            return false;
        }

        //var chk = document.getElementById("idcheckgeneralEstado");
        //chk.checked = false;
        document.forms["frm"].submit(function (event) {

        });
    }

};


$(function () {
    ko.applyBindings(new ViewModel());
    
});