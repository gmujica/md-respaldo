// Will make custom checkbox cheked and unchecked.
window.addEventListener('DOMContentLoaded', () => {
	document.querySelectorAll('custom-checkbox').forEach(chbx => {
		const otherCheckboxes = chbx.parentElement.querySelectorAll('.custom-checkbox-content');
		const content = chbx.querySelector('.custom-checkbox-content');
		content.addEventListener('click', () => {
			otherCheckboxes.forEach(item => {
				item.classList.remove('active');
				item.classList.add('inactive');
			});
			content.classList.toggle('active');
			content.classList.toggle('inactive');
		});
	});
});