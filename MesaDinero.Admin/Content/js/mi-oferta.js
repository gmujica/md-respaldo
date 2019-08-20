function enviar () {
	const value = document.querySelector('input#mi_oferta').value;
	if (value && !isNaN(value)) {
		// document.querySelector('#mi_oferta_enviada').textContent = value;
		document.querySelector(".enviar-oferta").removeEventListener('click', enviar);
		document.querySelector(".enviar-oferta").innerHTML = `<span class="btn-text">Enviado</span>`;
		document.querySelector(".enviar-oferta").className = "btn grey-style";
		//document.querySelector('input#mi_oferta').setAttribute('readonly', true);
	}
}

window.addEventListener('DOMContentLoaded', () => {
	const miOfertaInput = document.querySelector('input#mi_oferta');
	miOfertaInput.addEventListener('keypress', event => {
		miOfertaInput.value = miOfertaInput.value.replace(/[^0-9.]/g, '').replace(/(\..*)\./g, '$1');
		let key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
		if (isNaN(key) && (key !== '.' && miOfertaInput.value.indexOf('.') === -1)) {
			// miOfertaInput.value = miOfertaInput.value.replace(key, '');
			event.preventDefault();
		}
	});
	miOfertaInput.addEventListener('blur', event => {
		miOfertaInput.value = miOfertaInput.value.replace(/[^0-9.]/g, '').replace(/(\..*)\./g, '$1');
	});
	document.querySelector(".enviar-oferta").addEventListener('click', enviar);
});