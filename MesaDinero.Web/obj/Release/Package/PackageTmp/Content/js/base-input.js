window.addEventListener('DOMContentLoaded', () => {
	const bases = document.querySelectorAll('base-input');
	bases.forEach(base => {
		const input = base.querySelector('input');
		const onSelect = () => {
			base.classList.add('selected');
		}
		base.addEventListener('click', () => {
			onSelect();
		});
		input.addEventListener('focus', () => {
			onSelect();
		});
		input.onblur = () => {
			if (!input.value) {
				base.classList.remove('selected');
			}
		};
	});
});