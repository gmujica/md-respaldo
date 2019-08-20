// Crea el efecto de ver password.
window.addEventListener('DOMContentLoaded', () => {
	document.querySelectorAll('.view').forEach(view => {
		view.addEventListener('click', (event) => {
			const input = event.target.parentElement.querySelector('input');
			let type = input.getAttribute('type');
			type = type === 'password' ? 'text' : 'password';
			input.setAttribute('type', type);
		});
	}); 
});