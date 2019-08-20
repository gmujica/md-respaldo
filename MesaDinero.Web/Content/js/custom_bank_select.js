// enables custom select box (exchanger select)
window.addEventListener('DOMContentLoaded', () => {
	// Shows / Hides option list
	const unWrap = (box) => {
		box.classList.toggle('show-options');
	};
	const selectBoxes = document.querySelectorAll('.custom-select-box');
	selectBoxes.forEach(currentBox => {
		const clickables = currentBox.querySelectorAll('.custom-select-box > div:not(custom-select-option-list)');
		clickables.forEach(item => {
			item.addEventListener('click', () => {
				unWrap (currentBox);
			});
		});
		const options = currentBox.querySelectorAll('li');
		options.forEach(option => {
			option.addEventListener('click', () => {
				const value = option.getAttribute('value');
				const text = option.innerHTML;
				currentBox.querySelector('.custom-select-value').innerHTML = text;
				currentBox.setAttribute('value', value);
				currentBox.classList.remove('show-options');
			});
		});
	});
	document.addEventListener('click', event => {
		const target = event.target;
		if (!target.closest("div.custom-select-box")) {
			try {
				selectBoxes.forEach(currentBox => {
				currentBox.classList.remove('show-options');
			});
			} catch (error) {
				//
			}
		}
	});
});