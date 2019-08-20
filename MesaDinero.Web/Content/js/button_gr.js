window.addEventListener('DOMContentLoaded', () => {
    const btn = document.getElementById('btn');
const status = 'Enviado';
const v = 'Verificando...';

btn.addEventListener('click', () => {
    btn.style.background = '#B5B5B5';
    
if(document.getElementById('btn').value == 'Enviar')
    document.querySelector('.btn-text').innerHTML = status;
else {
    document.querySelector('.btn-text').innerHTML = v;
    btn.disabled = true;
}
    

//window.alert('hola');
});
});