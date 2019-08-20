/**
 * Tab effect
 */
window.addEventListener('DOMContentLoaded', () => {
	const people = document.querySelector('#login-people');
	const company = document.querySelector('#login-company');
	people.addEventListener('click', () => {
		people.classList.remove('inactive');
		people.classList.add('active');
		company.classList.add('inactive');
		company.classList.remove('active');
	});
	company.addEventListener('click', () => {
		company.classList.remove('inactive');
		company.classList.add('active');
		people.classList.add('inactive');
		people.classList.remove('active');
	});
});