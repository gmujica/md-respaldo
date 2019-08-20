window.addEventListener('DOMContentLoaded', () => {
	const dashboardContainerContent = document.querySelector('.dashboard-container-content');
	const barsIcon = document.querySelector('.bars-icon');
	
	const userAreaContent = document.querySelector('.user-area-initials');
	const popupMenu = document.querySelector('.popup-menu');
	barsIcon.addEventListener('click', () => {
		// Toggle the class that collapses the left menu.
		dashboardContainerContent.classList.toggle('collapsed')
	});
	userAreaContent.addEventListener('click', () => {
		// Toggle popup menu.
		popupMenu.classList.toggle('hidden');
	});
});