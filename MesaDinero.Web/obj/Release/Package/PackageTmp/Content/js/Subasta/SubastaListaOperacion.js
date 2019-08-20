


var ViewModel = function () {
    var self = this;
    
    this.tipo = ko.observable('G');
   
    debugger;
    $('#operacion').addClass('active');

    this.lstOperacionesConfirmadasRegistradosT = new GridModelSP("subasta-lista-operacion", 0, 10);


    this.listaRealizado = function () {
        $('#General').removeClass('active');
        $('#curso').removeClass('active');
        $('#Incumplidas').removeClass('active');
        $('#Curso').removeClass('active');
        $('#Realizadas').addClass('active');

        self.tipo('R');
        self.init();
    };

    this.listarIncumplidas = function () {
        $('#General').removeClass('active');
        $('#curso').removeClass('active');
        $('#Incumplidas').addClass('active');
        $('#Curso').removeClass('active');
        $('#Realizadas').removeClass('active');

        self.tipo('I');
        self.init();
    };

    this.listarCurso = function () {
        $('#General').removeClass('active');
        $('#curso').removeClass('active');
        $('#Incumplidas').removeClass('active');
        $('#Curso').addClass('active');
        $('#Realizadas').removeClass('active');

        self.tipo('C');
        self.init();
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
    //setInterval(function () { self.init(); }, 10000);
   

};


//var modelo = new ViewModel();


$(function () {


    ko.applyBindings(new ViewModel());

    // ko.applyBindings(modelo);
});