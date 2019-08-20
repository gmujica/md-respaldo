
function SubastaActiva()
{
    var self = this;
    this.codigo = ko.observable();
    this.nroSubasta = ko.observable();
    this.tiempoRestante = ko.observable();
    this.usuario = ko.observable();
    this.operacion = ko.observable();
    this.moneda = ko.observable();
    this.monto = ko.observable();
    this.tipoCambioPactado = ko.observable();
  
    this.updateTipoCambio = function () {

        var param = {
            subasta: self.codigo(),
            tipocambio : self.tipoCambioPactado()
        };
        debugger;
        PostJson("partner-update-tipocambio-subastaActiva", param).done(function (result) {

            toastr.success("tipo cambio, actualizado");

            //if (result.success == true)
            //{

                

            //} else {
            //    toastr.error(result.error);
            //    return false;
            //}
        });



    };

    this.tabOut = function (data, event) {
        
        if (event.keyCode == 9) {
            debugger;
            console.log(event.target.value);
            var param = {
                subasta: self.codigo(),
                tipocambio: event.target.value
            };
            debugger;
            PostJson("partner-update-tipocambio-subastaActiva", param).done(function (result) {

                toastr.success("tipo cambio, actualizado");

                //if (result.success == true)
                //{



                //} else {
                //    toastr.error(result.error);
                //    return false;
                //}
            });
        };
        return true;
    };
 
}


function ViewModel()
{
    var self = this;

    this.lstSubastas = ko.observableArray([]);
    this.lstSubastas2 = ko.observableArray([]);
    this.seq;

    this.init = function ()
    {
        PostJson2("partner-subastas-activas", null).done(function (result) {

            if(result.success == true)
            {
                if (result.data.length > 0) {

                    var array = [];

                    for (var i = 0; i < result.data.length; i++) {
                        var item = result.data[i];
                        var s = new SubastaActiva();
                        s.tiempoRestante(item.tiemporestante);
                        s.codigo(item.codigo);
                        s.nroSubasta(item.codigoTex);
                        s.moneda(item.moneda);
                        s.operacion(item.operacionText);
                        s.monto(item.monto);
                        s.tipoCambioPactado(item.tipoCambioPactado);
                        s.usuario(item.usuario);
                       
                        array.push(s);
                    }

                    if (self.seq != result.other) {
                        self.seq = result.other;
                        self.lstSubastas2(array);
                    }
                    self.lstSubastas(array);

                }
                else {
                    self.lstSubastas.removeAll();
                    self.lstSubastas2.removeAll();
                }
            }

            else {
                toastr.error(result.error);
                return false;
            }

        });
    };

    this.init2 = function () {
        setInterval(function () { return self.init() }, 1000, (0));
    };

    self.init2();
   
    //self.init();

};

$(function () {

    ko.applyBindings(new ViewModel());

});
