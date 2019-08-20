// Open popup
window.addEventListener('DOMContentLoaded', () => {
	try {
		document.querySelector('#open_popup').addEventListener('click', () => {
			document.querySelector('.popup-container').classList.remove('hidden');
		});
	} catch (error) {
		//
	}
	try {
		document.querySelector('#open_popup_bank_popup').addEventListener('click', () => {
			console.log('click');
			document.querySelector('.bank-popup').classList.remove('hidden');
		});
	} catch (error) {
		//
	}
	try {
		const popup_openers = document.querySelectorAll('.popup-opener');
		popup_openers.forEach(pO => {
			const target = pO.getAttribute('target-popup');
			pO.addEventListener('click', () => {
				document.querySelector(`.popup-container#${target}`).classList.remove('hidden');
			});
		});
	} catch (error) {
		//
	}
});
