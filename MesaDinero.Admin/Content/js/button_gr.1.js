window.addEventListener('DOMContentLoaded', () => {
    const btn = document.getElementById('btn-enviar');
    const oferta = document.querySelector('.mi_oferta');
    const statusE = 'Enviado';
    const statusEn = 'Enviar';

    btn.addEventListener('click', () => {
       //btn.style.background = '#B5B5B5';
        document.querySelector('.btn-text').innerHTML = statusE;
    });
    oferta.addEventListener('focusin', () => {
        btn.style.background = '#00d068';
        document.querySelector('.btn-text').innerHTML = statusEn;
    });
    oferta.addEventListener('focusout', () => {
        btn.style.background = '#B5B5B5';
        document.querySelector('.btn-text').innerHTML = statusE;
    });

});
