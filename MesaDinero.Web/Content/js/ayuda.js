window.addEventListener('DOMContentLoaded', () => {
	const content = document.querySelector('#busqueda-content');
	window.preguntas.forEach(section => {
		const { title, questions } = section;
		const questionSection = document.createElement('div');
		questionSection.className = 'question-section';
		questionSection.innerHTML = `<div class="soporte-text">${title}</div>`;
		questions.forEach(question => {
			const questionContainer = document.createElement('div');
			questionContainer.className = 'question-container';
			questionContainer.innerHTML = `
				<div class="soporte-text blue">${question.title}</div>
				<div class="question">
					${question.html}
				</div>
			`;
			questionContainer.addEventListener('click', () => {
				questionContainer.classList.toggle('displayed');
			});
			questionSection.appendChild(questionContainer);
		});
		content.appendChild(questionSection);
	});
});