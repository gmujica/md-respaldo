// Selecciona el cambio deseado
window.addEventListener('DOMContentLoaded', () => {
	const options = document.querySelectorAll('.partners-list > div:not(.list-title)');
	const unSelect = () => {
		options.forEach(option => {
			option.classList.remove('active');
		});
	};
	options.forEach(option => {
		const name = option.querySelector('div:nth-child(2)').textContent;
		const rate = option.querySelector('div:nth-child(3)').textContent;
		option.addEventListener('click', () => {
			unSelect();
			option.classList.add('active');
			document.querySelector('#selected_exchange_rate').innerHTML = `${rate} ${name}`;
		});
	});
});