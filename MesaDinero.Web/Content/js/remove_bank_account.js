// Removes bank account
window.addEventListener('DOMContentLoaded', () => {
	window.item = null;
	const confirmationPopup = document.querySelector('.confirmation-popup');
	document.querySelectorAll('.bank-list .list-col.trash .list-col-item').forEach(btn => {
		btn.addEventListener('click', () => {
			if (confirmationPopup) {
				confirmationPopup.classList.remove('hidden');
				window.item = btn.parentElement.parentElement;
				confirmationPopup.querySelector('.close-popup-btn').addEventListener('click', () => {
					if (window.item) {
						window.item.remove();
						window.item = null;
					}
				});
			} else {
				const item = btn.parentElement.parentElement;
				item.remove();
			}
		});
	});
});
