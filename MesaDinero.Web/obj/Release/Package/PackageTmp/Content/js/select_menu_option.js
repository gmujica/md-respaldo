// Selects menu options from sidebar.
window.addEventListener('DOMContentLoaded', () => {
	const options = document.querySelectorAll('.menu-area-option');
	const unSelect = () => {
		options.forEach(option => {
			option.classList.remove('active');
		});
	};
	options.forEach(option => {
		option.addEventListener('click', () => {
			unSelect();
			option.classList.add('active');
		});
	});
});