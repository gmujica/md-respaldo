// Close popup
window.addEventListener('DOMContentLoaded', () => {
    document.querySelectorAll('.cross').forEach(cross => {
        cross.addEventListener('click', () => {
            const popup = cross.parentElement.parentElement;
popup.classList.add('hidden');
});
});
document.querySelectorAll('.close-popup-btn').forEach(btn => {
    btn.addEventListener('click', () => {
        const popup = btn.parentElement.parentElement.parentElement;
popup.classList.add('hidden');
});
});
try {
    document.querySelector('#confirmar_bancos').addEventListener('click', () => {
        document.querySelector('#confirmar_bancos').parentElement.parentElement.parentElement.classList.add('hidden');
}); 
} catch (error) {
    //
}
document.querySelectorAll('.cmpnt-4').forEach(cross => {
    cross.addEventListener('click', () => {
        document.location = '/dashboard/operacion_en_curso(sample%201).html';
});
});
});