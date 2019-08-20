function ViewModel() {

    var self = this;

    this.initSubasta = function () {
        debugger;
        var monedaEnvio = $(".exch1").attr('currency');
        var monedaRecibe = $(".exch2").attr('currency');

        var operacion = '', monto = '0';
        if (monedaEnvio == 'PEN') {
            operacion = 'C';
            monto = document.getElementById("valor_recibes").value;
        } else {
            operacion = 'V';
            monto = document.getElementById("valor_envio").value;
        }

        //monto = Number(monto);

        if (monto <= 0) {
            if (operacion == 'C') {
                document.getElementById("valor_recibes").focus();
                toastr.warning("Usted no puede comprar dólares por un valor de 0.");
            } else {
                document.getElementById("valor_envio").focus();
                toastr.warning("Usted no puede vender dólares por un valor de 0.");
            }
            return false;
        }

        document.getElementById("operacion").value = operacion;
        document.getElementById("monto").value = monto;
        document.getElementById("montotext").value = monto;

        document.getElementById("monedaEnvio").value = monedaEnvio;
        document.getElementById("monedaRecibe").value = monedaRecibe;

        $("#form_inicioSubasta").submit();


    };



}


$(function () {
    ko.applyBindings(new ViewModel());

});
