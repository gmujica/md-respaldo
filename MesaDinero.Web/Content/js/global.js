function Patch(action, dataparams) {

    return ajaxMethod(action, 'PATCH', dataparams);
}

function Post(action, dataparams) {

    return ajaxMethod(action, 'POST', dataparams);
}

function Get(action, dataparams) {
    return ajaxMethod(action, 'GET', dataparams);
}

function PostFile(action, dataparams) {
    return ajaxMethod(action, 'POST', dataparams, true);
}

function PostJson(action, dataparams, async) {

    //async = async || true;

    return ajaxMethodJson(action, 'POST', dataparams,async);
}

function PostJson2(action, dataparams, async) {

    async = async || true;

    return ajaxMethodJson2(action, 'POST', dataparams, async);
}

function PostJson_Host(action, dataparams, async) {

    async = async || true;

    return ajaxMethodJson_Host(action, 'POST', dataparams, async);
}


function ajaxMethodJson(action, method, params, async) {

    $(".loading-spiner-holder").show();

    var resul =
        $.ajax({
            method: method,
            url: window.urlApi + action,
            dataType: 'json',
            contentType: 'application/json;charset=utf-8',
            data: JSON.stringify(params),
            async: async,
            cache: false
        }).done(function (jqXHR, textStatus, errorThrown) {
           
            //var token = errorThrown.getResponseHeader('Token');
            //if (token !== null && token !== undefined && token !== "") {
            //    sessionStorage.setItem(window.tokenKey, token);
            //}
        }).fail(function (jqXHR, textStatus, errorThrown) {
           
            console.log(jqXHR);
            console.log(textStatus);
            console.log(errorThrown);
            var error = ko.mapping.fromJSON(jqXHR.responseText);
            toastr.error(error.message());
        });
    return resul;
}

function ajaxMethodJson_Host(action, method, params, async) {

    $(".loading-spiner-holder").show();

    var resul =
        $.ajax({
            method: method,
            url: window.urlApp + action,
            dataType: 'json',
            contentType: 'application/json;charset=utf-8',
            data: JSON.stringify(params),
            async: async,
            cache: false
        }).done(function (jqXHR, textStatus, errorThrown) {

            //var token = errorThrown.getResponseHeader('Token');
            //if (token !== null && token !== undefined && token !== "") {
            //    sessionStorage.setItem(window.tokenKey, token);
            //}
        }).fail(function (jqXHR, textStatus, errorThrown) {

            console.log(jqXHR);
            console.log(textStatus);
            console.log(errorThrown);
            var error = ko.mapping.fromJSON(jqXHR.responseText);
            toastr.error(error.message());
        });
    return resul;
}

function ajaxMethodJson2(action, method, params, async) {

   

    var resul =
        $.ajax({
            method: method,
            url: window.urlApi + action,
            dataType: 'json',
            contentType: 'application/json;charset=utf-8',
            data: JSON.stringify(params),
            async: async,
            cache: false
        }).done(function (jqXHR, textStatus, errorThrown) {

            //var token = errorThrown.getResponseHeader('Token');
            //if (token !== null && token !== undefined && token !== "") {
            //    sessionStorage.setItem(window.tokenKey, token);
            //}
        }).fail(function (jqXHR, textStatus, errorThrown) {

            console.log(jqXHR);
            console.log(textStatus);
            console.log(errorThrown);
            var error = ko.mapping.fromJSON(jqXHR.responseText);
            toastr.error(error.message());
        });
    return resul;
}

function ajaxMethod(action, method, params, isMultipart, async) {
    var config = {
        method: method,
        url: window.urlApi + action,
        dataType: 'json',
        data: params,
        async: async,
        cache: false
    };

    if (isMultipart) {
        config.mimeType = "multipart/form-data";
        config.contentType = false;
        config.processData = false;
    }


    var resul = $.ajax(config)
        .done(function (jqXHR, textStatus, errorThrown) {
            //var token = errorThrown.getResponseHeader('Token');
            //if (token !== null && token !== undefined && token !== "") {
            //    sessionStorage.setItem(window.tokenKey, token);
            //}
        }).fail(function (jqXHR, textStatus, errorThrown) {
            console.log(jqXHR);
            console.log(textStatus);
            console.log(errorThrown);
            if (errorThrown === "Unauthorized") {
                document.getElementById('logoutForm').submit();
                return;
            }
            if (jqXHR.responseText === "") return;
            var error = ko.mapping.fromJSON(jqXHR.responseText);
            toastr.error(error());
        });

    return resul;
}
