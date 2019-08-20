function buildPage (page, pageTitle, pageContent) {
	const { title, content } = page;
	pageTitle.textContent = title;
	pageContent.innerHTML = `${
		content.map(item => {
			const { className } = item;
			if (className !== 'numbered') {
				return `<div class="${className}">${item.text}</div>`;
			} else {
				return `
					<div class="numbered">
						<div>${item.num}</div>
						<div class="${item.content.className}">${item.content.text}</div>
					</div>
				`;
			}
		}).join('')
	}`;
}

window.addEventListener('DOMContentLoaded', () => {
	const pageTitle = document.querySelector('#title');
	const pageContent = document.querySelector('#terminos-content');
	const page = window.terminos[0];
	let index = 0;
	buildPage(page, pageTitle, pageContent);
	const nextBtn = document.querySelector('#next');
	const prevBtn = document.querySelector('#previous');
	nextBtn.addEventListener('click', () => {
		if (window.terminos[index + 1]) {
			index += 1;
			buildPage(window.terminos[index], pageTitle, pageContent);
			window.scrollTo(0, 0);
			if (!window.terminos[index + 1]) nextBtn.classList.add('disabled');
			if (window.terminos[index - 1]) prevBtn.classList.remove('disabled');
		}
	});
	prevBtn.addEventListener('click', () => {
		if (window.terminos[index - 1]) {
			index -= 1;
			buildPage(window.terminos[index], pageTitle, pageContent);
			window.scrollTo(0, 0);
			if (window.terminos[index + 1]) nextBtn.classList.remove('disabled');
			if (!window.terminos[index - 1]) prevBtn.classList.add('disabled');
		}
	})
});