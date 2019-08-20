window.addEventListener('DOMContentLoaded', () => {
    const dashboardContainerContent = document.querySelector('.dashboard-container-content');
const barsIcon = document.querySelector('.bars-icon');

const userAreaContent = document.querySelector('.user-area-arrow');
const popupMenu = document.querySelector('.popup-menu');

//close the layout
barsIcon.addEventListener('click', () => {
    // Toggle the class that collapses the left menu.
    dashboardContainerContent.classList.toggle('collapsed')
});
userAreaContent.addEventListener('click', () => {
    // Toggle popup menu.
    popupMenu.classList.toggle('hidden');
});
document.addEventListener('click', event => {
    const target = event.target;
if (!target.closest(".user-area-arrow, .popup-menu")) {
    try {
        popupMenu.classList.add('hidden');
    } catch (error) {
        //
    }
}
});
});