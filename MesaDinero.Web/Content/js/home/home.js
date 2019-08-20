function PersonaModel()
{
    var self = this;

    this.nroDocumento = ko.observable('').extend(
        {
            required: { params: true, message: "Campo Obligatorio" },
            isNumeric: true
        });
    this.nombre = ko.observable('').extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.apellido = ko.observable('').extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.email = ko.observable('').extend({
        required: { params: true, message: "Campo Obligatorio" },
        email: { params: true, message: "Ingrese un correo electrónico válido." }
    });
    this.celular = ko.observable('').extend({
        required: {
            params: true,
            message: "Campo Obligatorio"
        },
        isNumeric: true
    });
    this.tipoPersona;


    this.nroDocumento.subscribe(function (val) {

        if(val)
        {
            var data = {
                nro : val
            };

            PostJson("common-getdatospersonasimple", data, true).done(function (result) {

                if(result.success == true)
                {
                    
                    self.nombre(result.data.nombres);
                    self.apellido(result.data.apePaterno + ' ' + result.data.apeMaterno);
                    self.email(result.data.email);

                } else {
                    toastr.error = result.error;
                    return false;
                }

            });

        } else {
            self.nombre('');
            self.apellido('');
            self.email('');
        }



    });

    this.validationResult = null;
    this.validarModelo = function () {
        self.validationResult = ko.validation.group(this);
        if (self.validationResult().length === 0) {
            return true;
        } else {
            self.validationResult.showAllMessages();
            return false;
        }
    };
    
}

function EmpresaModel() {
    var self = this;

    this.nroDocumento = ko.observable('').extend(
    {
        required: { params: true, message: "Campo Obligatorio" },
        isNumeric: true,
        minLength: { params: 11, message: "Debe ingresar los 11 dígitos del RUC" },
        maxLength: { params: 11, message: "Debe ingresar los 11 dígitos del RUC" }
    });
    this.nombreEmpresa = ko.observable('');
    this.nroDocumentoContacto = ko.observable('');
    this.nombre = ko.observable('').extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.apellido = ko.observable('').extend({ required: { params: true, message: "Campo Obligatorio" } });
    this.email = ko.observable('').extend({
        required: { params: true, message: "Campo Obligatorio" },
        email: { params: true, message: "Ingrese un correo electrónico válido." }
    });
    this.celular = ko.observable('').extend({
        required: {
            params: true,
            message: "Campo Obligatorio"
        },
        isNumeric: true
    });
    this.tipoPersona;
    
    this.nroDocumentoContacto.subscribe(function (val) {

        if(val)
        {
            var data = {
                nro : val
            };

            PostJson("common-getdatospersonasimple", data, true).done(function (result) {

                if(result.success == true)
                {
                    
                    self.nombre(result.data.nombres);
                    self.apellido(result.data.apePaterno + ' ' + result.data.apeMaterno);
                    self.email(result.data.email);

                } else {
                    toastr.error = result.error;
                    return false;
                }

            });

        } else {
            self.nombre('');
            self.apellido('');
            self.email('');
        }
    });

    this.nroDocumento.subscribe(function (val) {

        if (val) {
            var data = {
                nro: val
            };

            PostJson("common-getdatosempresasimple", data, true).done(function (result) {

                if (result.success == true) {

                    self.nombreEmpresa(result.data.nombre);
                    

                } else {
                    toastr.error = result.error;
                    return false;
                }

            });

        } else {
            self.nombreEmpresa('');
        }
    });

    this.validationResult = null;
    this.validarModelo = function () {
        self.validationResult = ko.validation.group(this);
        if (self.validationResult().length === 0) {
            return true;
        } else {
            self.validationResult.showAllMessages();
            return false;
        }
    };


}

var ViewModel = function () {

    var self = this;
    
    this.host = ko.observable(window.urlApp);

    this.persona = new PersonaModel();
    this.empresa = new EmpresaModel();

    this.aceptarTerm = ko.observable(false);

    this.tipoCliente = ko.observable(1);

    this.secredId = ko.observable('');

    this.SelectTipoPersonaNatural = function () {
       
        self.persona.nombre('');
        self.persona.apellido('');
        self.persona.email('');
        self.persona.celular('');
        self.persona.nroDocumento('');

        self.tipoCliente(1);
        self.aceptarTerm(false);

        if (self.persona.validationResult != null)
            self.persona.validationResult.showAllMessages(false);

        var empresa = $("#login-company");
        $(empresa).removeClass('active');
        $(empresa).addClass('inactive');

        var persona = $("#login-people");
        $(persona).removeClass('inactive');
        $(persona).addClass('active');

        $("#tab_empresa").hide();
        $("#tab_persona").show();

    };

    this.SelectTipoPersonaJuridica = function () {

        self.empresa.nroDocumento('');
        self.empresa.nombre('');
        self.empresa.apellido('');
        self.empresa.email('');
        self.empresa.celular();

        self.tipoCliente(2);

        if (self.empresa.validationResult != null)
            self.empresa.validationResult.showAllMessages(false);

        var persona = $("#login-people");
        $(persona).removeClass('active');
        $(persona).addClass('inactive');

        var empresa = $("#login-company");
        $(empresa).removeClass('inactive');
        $(empresa).addClass('active');
               

        $("#tab_persona").hide();
        $("#tab_empresa").show();

    };

    this.clearModel = function () {
        self.persona.nombre('');
        self.persona.apellido('');
        self.persona.email('');
        self.persona.celular('');
        
        self.empresa.nroDocumento('');
        self.empresa.nombre('');
        self.empresa.apellido('');
        self.empresa.email('');
        self.empresa.celular('');

    };

    this.registrarCliente = function () {       

        if (self.tipoCliente() == 1)
        {
            if (!self.persona.validarModelo()) {
                return false;
            }
        }
        
        if (self.tipoCliente() == 2) {
            if (!self.empresa.validarModelo()) {
                return false;
            }
        }

        if (self.aceptarTerm() == false) {
            toastr.warning("Por favor revisar el contrato de servicio.");
            return false;
        }


        var data;

        if (self.tipoCliente() == 1){
            data = ko.toJS(self.persona);
            
        }           
        else {
            data = ko.toJS(self.empresa);
            
        }
        
        data.tipoPersona = self.tipoCliente();



   
       PostJson("registro/basico", data).done(function (result) {
           
           self.clearModel();
           self.SelectTipoPersonaNatural();
           $(".control_nombre")[0].focus();

           if (result.success) {

               $("#title_popup").text("La Mesa de Dinero");
               var htmlMsg = ' <p>Cliente registrado satisfactoriamente.</p><span>Acontinuación haga click el botom "Aceptar" para la verificación de su Nro. Celular.</span>';
               $("#message_popup").html(htmlMsg);
               var htmlFooter = '<a href="' + window.w_host + '/Registro/Continuar/' + result.data + '" class="" style="color: #fff; text-decoration: none;"><span class="btn-text">Aceptar</span></a>';
               $("#footer_popup").html(htmlFooter);
               //$("#popup_msm").removeClass("hidden");

               window.location.href = window.w_host + '/Registro/Continuar/' + result.data;
               
           } else {

               var htmlMsg = "", htmlFooter = "", errorModal = false;;

               if (result.other == "CLR")
               {   
                   htmlMsg = '<p>La cuenta ya existe </p><span> <b><u>' + result.data + '</u></b> ya eres parte de La Mesa de Dinero </br> inicia sesión para comerzar a subastar.</span>';
                   htmlFooter = '<a style="text-decoration: none;" onclick="closeModal();" class=""><span class="btn-text" style="color:#ffffff;">Aceptar</span></a>';
                   errorModal = true;
               }
               else if (result.other == "CLCE")
               {
                   htmlMsg = '<p>El correo <u>' + result.data + '</u>  ya se encuentra ocuapdo, por favor ingrese un correo distinto. </p>';
                   htmlFooter = '<a style="text-decoration: none;" onclick="closeModal();" class=""><span class="btn-text" style="color:#ffffff;">Aceptar</span></a>';
                   errorModal = true;
               }
               else if (result.other == "CLRP")
               {
                   htmlMsg = '<p> <u><b>' + result.data + '</b></u> ya tienes un registro en proceso </br> dirigete a la sección inicio de sesion para retomar tu registro. </p>';
                   htmlFooter = '<a style="text-decoration: none;" onclick="closeModal();" class=""><span class="btn-text" style="color:#ffffff;">Aceptar</span></a>';
                   errorModal = true;
               }

               if (errorModal == true)
               {
                   $("#title_popup").text("La Mesa de Dinero");
                   $("#message_popup").html(htmlMsg);
                   $("#footer_popup").html(htmlFooter);
                   $("#popup_msm").removeClass("hidden");
                   $(".control_nombre")[0].focus();
                   return false;
               }

               toastr.error(result.error);
               return false;
           }         

       });
    };

    self.init = function () { };
};

var modelo = new ViewModel();

function closeModal() {
    $("#popup_msm").addClass("hidden");
};

$(function () {

    modelo.init();
    ko.applyBindings(modelo);
});

