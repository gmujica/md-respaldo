// Will create smooth effect on input.
window.addEventListener('DOMContentLoaded', () => {
	document.querySelectorAll('fancy-input').forEach(inp => {
		const content = inp.querySelector('.fancy-input-content');
		const realInput = content.querySelector('input');
		const onSelect = () => {
			content.classList.add('selected');
		}
		content.addEventListener('click', () => {
			onSelect();
		});
		realInput.addEventListener('focus', () => {
			onSelect();
		});
		realInput.onblur = () => {
			if (!realInput.value) {
				content.classList.remove('selected');
			}
		};
	});
});