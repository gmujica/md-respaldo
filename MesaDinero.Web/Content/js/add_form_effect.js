window.addEventListener('DOMContentLoaded', () => {
	const removeForm = (newForm, removeFormButton) => {
		removeFormButton.addEventListener('click', () => {
			const forms = newForm.parentElement.querySelectorAll('.new-form').length;
			if (forms >= 2) {
				newForm.remove();
			}
		});
	}
	//document.querySelectorAll(".add-row-btn").forEach(btn => {
	//	try {
	//		const parent = btn.parentElement;
	//		const content = btn.parentElement.querySelector('.new-form');
	//		//const rB = content.querySelector('.remove-form');
	//		//removeForm(content, rB);
	//		btn.addEventListener('click', () => {
	//			const newForm = content.cloneNode(true);
	//			newForm.className = content.className === 'register-container-block block-1-1 new-form'
	//				? 
	//				'register-container-block block-1-1 new-form'
	//				: 'new-form';
	//			const removeFormButton = document.createElement('div');
	//			removeFormButton.className = 'remove-form';
	//			newForm.appendChild(removeFormButton);
	//			removeForm(newForm, removeFormButton);
	//			parent.insertBefore(newForm, btn);
	//		});
	//	} catch (error) {
	//		// do nothing
	//	}
	//});
	let count = 0;
	// for the minimize tabs
	document.querySelectorAll(".sub-title").forEach(btn => {
		try {
			btn.addEventListener('click', () => {
				if (btn.classList.length === 1) {
					btn.classList.add('hide');
					count++
				} else {
					btn.classList.remove('hide');
					count--;
				}
				if (count === 4) {
					document.querySelector('.logo-container.hide').classList.remove('hide');
				} else {
					document.querySelector('.logo-container') ? document.querySelector('.logo-container').classList.add('hide') : null
				}
			})
		} catch (error) {
			// do nothing
		}
		//
		
	})
});
