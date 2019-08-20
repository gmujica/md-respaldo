window.addEventListener('DOMContentLoaded', () => {
	const inputs = document.querySelectorAll('.dev-border.contact-border');
	inputs.forEach(element => {
		const input = element.querySelector('input');
		input.addEventListener('focus', () => {
			element.classList.add('selected');
		});
		input.onblur = () => {
			if (!input.value) {
				element.classList.remove('selected');
			}
		};
	});
});