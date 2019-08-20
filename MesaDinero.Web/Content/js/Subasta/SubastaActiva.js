
function compararModel() {
    var self = this;
    this.venta;
    this.monto;

}

var cnt_partnets = 0;
function ViewModel() {

    var self = this;

    this.tiempo = ko.observable('--');
    this.lstParnet = ko.observableArray(window.wData.data.partners);
    
    //console.log(ko.toJSON( self.lstParnet.sort()));
    this.estado = ko.observable("B");
    this.orderPartnerts = ko.observable(window.wData.other)

    this.montoEnvio = ko.observable(window.wData.data.valorEnvioText);
    this.montoRecibe = ko.observable(window.wData.data.valorRecibeText);
    //self.montoRecibe("00.00");
    
    this.partnerGanador = ko.observable(window.wData.data.partner);
    this.tipoCambio = ko.observable(window.wData.data.tipoCambioText);
    this.codigoSeleccion = ko.observable(window.wData.data.codigoSeleccion);
    this.showContinuar = ko.observable(false);
    this.listaComparar = ko.observableArray([]);
    this.enviarComparar = new compararModel();
    this.operacion = ko.observable(window.wData.data.opr);
    this.montoEnv = ko.observable(window.wData.data.valorEnvio);
    this.montoRec = ko.observable(window.wData.data.valorRecibe);
    this.quiere = ko.observable();
    this.accion = ko.observable();
    this.montoLmd = ko.observable();
    this.montoLmdnum = ko.observable();
    this.montoAhorro = ko.observable();
    //this.montoRecibe2 = ko.observable(self.montoRec()*3.32);
    if (self.operacion() == "V") {
        self.quiere("Cuando Tú vendes $" + self.montoEnv().toLocaleString('eu-ES', { minimumFractionDigits: 2 }));
        self.accion("Tú recibes");
        self.montoLmd("S/ " + self.montoRecibe());
        self.montoLmdnum(self.montoRecibe());
        self.montoRecibe("00.00");
        self.montoAhorro("00.00");
    } else {
        self.quiere("Cuando Tú compras $" + self.montoRec().toLocaleString('eu-ES', { minimumFractionDigits: 2 }));
        self.accion("Tú envías");
        self.montoLmd("S/ " + self.montoEnvio());
        self.montoLmdnum(self.montoEnvio());
        self.montoEnvio("00.00");
        self.montoAhorro("00.00");
    }



    this.start = function () {
        setInterval(function () { return tick(); }, 1000);
    };

    function tick() {

        var data = {
            sid: window.wData.data.sid
        };


        if (cnt_partnets < 0 && self.estado() == "A") {

            PostJson2("common/subasta/actulizarTipoCambio", data, true).done(function (result) {
                if (self.orderPartnerts() != result.other) {


                    self.codigoSeleccion(result.other2);
                    self.lstParnet.removeAll();

                    for (var i = 0; i < result.data.puja.length; i++) {
                        if (self.codigoSeleccion() == result.data.puja[i].codigo) {
                            result.data.puja[i].classCss = "item item-s-k Activo";

                            self.partnerGanador(result.data.seleccion.partner);
                            self.tipoCambio(result.data.seleccion.tipoCambioText);
                            self.montoEnvio(result.data.seleccion.valorEnvioText);
                            self.montoRecibe(result.data.seleccion.valorRecibeText);
                            self.montoLmd("S/ "+result.data.seleccion.valorRecibeText);
                            self.montoLmdnum(result.data.seleccion.valorRecibe);
                            console.log(self.tipoCambio());
                        } else {
                            result.data.puja[i].classCss = "item item-s-k ";
                        }
                        self.lstParnet.push(result.data.puja[i]);
                    }

                    self.orderPartnerts(result.other);
                }

            });
        }

        self.mostrarValorAhorro();


        PostJson2("common/getTiempoSubastaCurrent", data, true).done(function (result) {

            if (result.success == true) {

                self.estado(result.data.estado);

                if (result.data.estado == "B") {
                    //$("#sub-title-siubasta").text('');
                    self.showContinuar(true);
                    $("#title-subasta").text("Confirmación de subasta...");
                    $("#text-tiempo-title").text("Tiempo de Confirmación");
                    $("#comparar").removeClass('hidden');
                    $("#texto-ahorro").removeClass('hidden');
                    self.mostrarValorAhorro();
                }
                if (result.data.estado == "B" && result.data.tiempo < 0) {
                    $('#popup_concluir').removeClass('hidden');

                    return false;
                }
                
                //16-06-2019
                var secs = result.data.tiempo;
                var hours = Math.floor(secs / (60 * 60));

                var divisor_for_minutes = secs % (60 * 60);
                var minutes = Math.floor(divisor_for_minutes / 60);
                minutes = minutes < 10 ? '0' + minutes : minutes;

                var divisor_for_seconds = divisor_for_minutes % 60;
                var seconds = Math.ceil(divisor_for_seconds);
                seconds = seconds < 10 ? '0' + seconds : seconds;

                var formato = minutes + ":" +seconds;
                self.tiempo(formato);

            } else {

            }

        });


    };

    this.seleccionarPartnert = function (data) {

        if (self.showContinuar() == true) {
            var data2 = {
                subasta: data.subasta,
                codigo: data.codigo
            };

            PostJson2("common/subasta/seleccionar-partner", data2, null).done(function (result) {

                if (result.success == true) {

                    self.codigoSeleccion(data.codigo);
                    self.partnerGanador(result.data.partner);
                    self.tipoCambio(result.data.tipoCambioText);
                    self.montoEnvio(result.data.valorEnvioText);
                    self.montoRecibe(result.data.valorRecibeText);

                    for (var i = 0; i < self.lstParnet().length; i++) {
                        if (self.lstParnet()[i].codigo == data.codigo) {
                            $("#" + self.lstParnet()[i].codigo).addClass("Activo");
                        } else {
                            $("#" + self.lstParnet()[i].codigo).removeClass("Activo");
                        }
                    }



                } else {
                    toastr.error(result.error);
                }

            });


        }

    };

    this.init = function () {

        self.start();



    };

    this.continuarSubasta = function () {
        debugger;
        if (self.showContinuar() == true) {
            
            document.getElementById("subasta").value = window.wData.data.sid;
            document.getElementById("partner").value = self.codigoSeleccion();
            var codigoElegido = self.codigoSeleccion();
            var codigoGanador = self.lstParnet()[0].codigo;
            if (codigoGanador != codigoElegido) {
                $('#mejorCambio').removeClass('hidden');
            } else {
                $("#form_confirmacion_subasta").submit();
            }
        }
    };


    this.aceptarContinuar = function () {

        debugger;
        $("#form_confirmacion_subasta").submit();
    };

    self.init();


    this.openModal = function () {

        var mont = 0;
        if (self.operacion() == "V") {
            mont = self.montoEnv();
        } else {
            mont = self.montoRec();
        }

        self.enviarComparar.venta = self.operacion();
        self.enviarComparar.monto = mont;

        var model = ko.toJS(self.enviarComparar);
        PostJson("subasta-lista-comparar-proveedor", model).done(function (result) {

            if (result.success == true) {
                
                var valorProv = ko.toJS(result.data[0]);
                var num1 = ko.toJS(self.montoLmdnum()).toString();;
                var num = mont * self.lstParnet()[0].tipoCambio;
               
                num = num.toLocaleString('eu-ES', { minimumFractionDigits: 2 })
                self.montoLmd("S/ " + num);
                self.tipoCambio(self.lstParnet()[0].tipoCambioText);
                var ahorr = 0;
               
                if (self.operacion() == "V") {
                    ahorr = num.replace(',', '') - valorProv.monto;
                } else {
                    ahorr = valorProv.monto - num.replace(',', '');
                }

                if (ahorr < 0) {
                    ahorr = 0;
                }

                //self.montoAhorro(parseFloat(ahorr).toFixed(2));
                self.montoAhorro(ahorr.toLocaleString('eu-ES', { minimumFractionDigits: 2 }));
                self.listaComparar(result.data);
                //result.data[0].
                $("#compararPopup").removeClass("hidden");
            } else {
                toastr.error(result.error);
            }

        });

    }
    
    
    this.mostrarValorAhorro = function () {

        var mont = 0;
        if (self.operacion() == "V") {
            mont = self.montoEnv();
        } else {
            mont = self.montoRec();
        }
        debugger;
        self.enviarComparar.venta = self.operacion();
        self.enviarComparar.monto = mont;

        var model = ko.toJS(self.enviarComparar);
        PostJson("subasta-lista-comparar-proveedor", model).done(function (result) {

            if (result.success == true) {
                debugger;
                var valorProv = ko.toJS(result.data[0]);
                var num1 = ko.toJS(self.montoLmdnum()).toString();
                var num = mont*self.lstParnet()[0].tipoCambio;
              
                num = num.toLocaleString('eu-ES', { minimumFractionDigits: 2 })
                var ahorr = 0;

                if (self.operacion() == "V") {
                    ahorr = num.replace(',', '') - (valorProv != null ? valorProv.monto : 0 );
                } else {
                    ahorr = valorProv.monto - num.replace(',', '');
                }
                if (ahorr < 0) {
                    ahorr = 0;
                }

                self.montoAhorro(ahorr.toLocaleString('eu-ES', { minimumFractionDigits: 2 }));
                self.listaComparar(result.data);
                //result.data[0].
                
            } else {
                toastr.error(result.error);
            }

        });

    }
    self.mostrarValorAhorro();



};


$(function () {

    ko.applyBindings(new ViewModel());
    $("#nombrePartner").text('');
    $("#tipoCanbio").text('');


    $('.item-s-k').hide();

    var items = $('.item-s-k');
    cnt_partnets = items.length;
    var cn_total = items.length - 1;
    //var cc = 0;
    //window.setInterval(function () {

    //    if (cnt_partnets >= 0) {
    //        $("#partners-list-k").show();
    //        $('.item-s-k').removeClass('Activo');
    //        //500

    //        $(items[cc]).show(200, 'swing', function () {
    //            $(".pre-load-partner").hide();
    //            $(this).addClass('Activo');
    //            $("#nombrePartner").text(window.wData.data.partners[cc - 1].nombre);
    //            $("#tipoCanbio").text(window.wData.data.partners[cc - 1].tipoCambioText);
    //            var monto = parseFloat($("#valor_envio").val().replace(',', ''));
    //            var tipoCamb = parseFloat(window.wData.data.partners[cc - 1].tipoCambio);
    //            var total = 0;
    //            total = monto * tipoCamb;
    //            $("#valor_recibes").val(total.toLocaleString('eu-ES', { minimumFractionDigits: 2 }));
    //            //self.montoRecibe(total.toLocaleString('eu-ES', { minimumFractionDigits: 2 }));
    //            //self.lstParnet.
    //            debugger;
    //            window.wData.data.partners[cc + 1].indice= 12;
    //            //console.log(ssS);
    //        });

    //        console.log(cc);
    //        cc+=1;
    //        cnt_partnets = cnt_partnets - 1;

    //    }

    //}, 850);

    if (window.wData.data.initSubasta == 0)
    {
        var cc = 1;
        debugger;
        window.setInterval(function () {

            if (cnt_partnets >= 0) {
                $("#partners-list-k").show();
                $('.item-s-k').removeClass('Activo');
                //500

                $(items[cnt_partnets]).show(200, 'swing', function () {
                    $(".pre-load-partner").hide();
                    $(this).addClass('Activo');
                    $("#nombrePartner").text(window.wData.data.partners[cnt_partnets + 1].nombre);
                    $("#tipoCanbio").text(window.wData.data.partners[cnt_partnets + 1].tipoCambioText);
                    if (window.wData.data.opr == 'V') {
                        var monto = parseFloat($("#valor_envio").val().replace(',', ''));
                        var tipoCamb = parseFloat(window.wData.data.partners[cnt_partnets + 1].tipoCambio);
                        var total = 0;
                        //alert(window.wData.data.opr);
                        total = monto * tipoCamb;
                        $("#valor_recibes").val(total.toLocaleString('eu-ES', { minimumFractionDigits: 2 }));
                    } else {
                        var monto = parseFloat($("#valor_recibes").val().replace(',', ''));
                        var tipoCamb = parseFloat(window.wData.data.partners[cnt_partnets + 1].tipoCambio);
                        var total = 0;
                        //alert(window.wData.data.opr);
                        total = monto * tipoCamb;
                        $("#valor_envio").val(total.toLocaleString('eu-ES', { minimumFractionDigits: 2 }));

                    }

                    if (cc == 1) {
                        var nombre = '#item_' + (cnt_partnets + 1);

                        $(nombre).html(cc);
                    } else {
                        var ccc = cc;
                        for (var i = cn_total; i >= cnt_partnets + 1; i--) {
                            var nombre = '#item_' + (i);
                            $(nombre).html(ccc);
                            ccc--;
                        }
                    }
                    console.log(cn_total);

                    cc++;
                    window.wData.data.partners[cnt_partnets + 1].indice = 12;
                    //console.log(ssS);
                });


                cnt_partnets = cnt_partnets - 1;

            }

        }, 950);
    } else {
        debugger;
        $("#nombrePartner").text(window.wData.data.partner);
        $("#tipoCanbio").text(window.wData.data.tipoCambioText);
        if (window.wData.data.opr == 'V') {
            //var monto = parseFloat($("#valor_envio").val().replace(',', ''));
            //var tipoCamb = parseFloat(window.wData.data.partners[cnt_partnets + 1].tipoCambio);
            //var total = 0;
            
            $("#valor_recibes").val(window.wData.data.valorRecibeText);
            //total = monto * tipoCamb;
            //$("#valor_recibes").val(total.toLocaleString('eu-ES', { minimumFractionDigits: 2 }));
        } else {
            //var monto = parseFloat($("#valor_recibes").val().replace(',', ''));
            //var tipoCamb = parseFloat(window.wData.data.partners[cnt_partnets + 1].tipoCambio);
            //var total = 0;
            ////alert(window.wData.data.opr);
            //total = monto * tipoCamb;
            $("#valor_envio").val(window.wData.data.valorEnvioText);

        }
        $(".pre-load-partner").hide();
        $('.item-s-k').show();
        $(".partners-list").show();
    }


    //window.setInterval(function () {
    //    debugger;
    //    if (cc <= cnt_partnets) {
    //        $("#partners-list-k").show();
    //        $('.item-s-k').removeClass('Activo');
    //        //500
    //        alert("hola");
    //        $(items[cc]).show(350, 'swing', function () {
    //            $(".pre-load-partner").hide();
    //            $(this).addClass('Activo');
    //            debugger;
    //            $("#nombrePartner").text(window.wData.data.partners[cc -1].nombre);
    //            $("#tipoCanbio").text(window.wData.data.partners[cc -1].tipoCambioText);
    //            var monto = parseFloat($("#valor_envio").val().replace(',', ''));
    //            var tipoCamb = parseFloat(window.wData.data.partners[cc -1].tipoCambio);
    //            var total = 0;
    //            total = monto * tipoCamb;
    //            $("#valor_recibes").val(total.toLocaleString('eu-ES', { minimumFractionDigits: 2 }));

    //        });
    //        console.log(cc);
    //        cc = cc + 1;

    //    }

    //}, 4000);




    //cnt_partnets = 0;


    //window.setInterval(function () {
    //    debugger;
    //    //if (cnt_partnets <= 20) {
    //    for (var i = 0; i <= items.length; i++) {
    //        $("#partners-list-k").show();
    //        $('.item-s-k').removeClass('Activo');
    //        //500
    //        $(items[i]).show(350, 'swing', function () {
    //            $(".pre-load-partner").hide();
    //            $(this).addClass('Activo');
    //            debugger;
    //            $("#nombrePartner").text(window.wData.data.partners[i - 1].nombre);
    //            $("#tipoCanbio").text(window.wData.data.partners[i - 1].tipoCambioText);
    //            var monto = parseFloat($("#valor_envio").val().replace(',', ''));
    //            var tipoCamb = parseFloat(window.wData.data.partners[i - 1].tipoCambio);
    //            var total = 0;
    //            //console.log(parseFloat($("#valor_envio").val().replace(',','')));
    //            //console.log(parseFloat(window.wData.data.partners[cnt_partnets + 1].tipoCambio));
    //            //console.log(monto * tipoCamb);
    //            total = monto * tipoCamb;
    //            $("#valor_recibes").val(total.toLocaleString('eu-ES', { minimumFractionDigits: 3 }));

    //        });
    //    }

    //        //console.log(cnt_partnets);
    //        //cnt_partnets = cnt_partnets + 1;

    //    //}

    //}, 3000);



    //window.setInterval(function () {
    //    debugger;
    //    if (cnt_partnets >= 0) {
    //        $("#partners-list-k").show();
    //        $('.item-s-k').removeClass('Activo');
    //        //500
    //        $(items[cnt_partnets]).show(350, 'swing', function () {
    //            $(".pre-load-partner").hide();
    //            $(this).addClass('Activo');
    //            debugger;
    //            $("#nombrePartner").text(window.wData.data.partners[cnt_partnets + 1].nombre);
    //            $("#tipoCanbio").text(window.wData.data.partners[cnt_partnets + 1].tipoCambioText);
    //            var monto = parseFloat($("#valor_envio").val().replace(',', ''));
    //            var tipoCamb = parseFloat(window.wData.data.partners[cnt_partnets + 1].tipoCambio);
    //            var total = 0;
    //            total = monto * tipoCamb;
    //            $("#valor_recibes").val(total.toLocaleString('eu-ES', { minimumFractionDigits: 3 }));

    //        });
    //        console.log(cnt_partnets);
    //        cnt_partnets = cnt_partnets - 1;

    //    }

    //}, 4000);




});


