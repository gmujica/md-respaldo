window.addEventListener('DOMContentLoaded', () => {
    var separador = document.getElementById('input-miles');

    separador.addEventListener('keyup', (e) => {
        var entrada = e.target.value.split(',').join('');
        entrada = entrada.split('').reverse();
        
        var salida = [];
        var aux = '';

        var paginador = Math.ceil(entrada.length / 3);
        
        for(let i = 0; i < paginador; i++) {
            for(let j = 0; j < 3; j++) {
                if(entrada[j + (i * 3)] != undefined) {
                    aux +=entrada[j + (i * 3)];
                }
            }
            salida.push(aux);            
            aux = '';

            e.target.value = salida.join(',').split('').reverse().join('');

        }
    }, false);
});
