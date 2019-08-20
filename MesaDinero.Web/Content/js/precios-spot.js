window.addEventListener('DOMContentLoaded', () => {
	const ctx = document.getElementById("chart").getContext("2d");
	const myChart = new Chart(ctx, {
		type: 'line',
		data: {
			xAxisID: 'xaxis',
			yAxisID: 'yaxis',
			labels: [1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26],
			datasets: [{
				label: 'Usuario Vende',
				backgroundColor: '#E8762C',
				borderColor: '#E8762C',
				data: [
					{y:3, x:1},{y:3, x:3}, {y:2, x:4}, {y:2.5, x:7}, {y:2, x:9},
					{y:3, x:10},{y:3, x:12}, {y:2, x:13}, {y:2.5, x:16}, {y:2, x:18},
					{y:3, x:18},{y:3, x:20}, {y:2, x:21}, {y:2.5, x:24}, {y:2, x:26}
				],
				fill: false,
			}, {
				label: 'Usuario Compra',
				fill: false,
				backgroundColor: '#1072aa',
				borderColor: '#1072aa',
				data: [
					{y:2, x:1},{y:2, x:3}, {y:1, x:4}, {y:1.5, x:7}, {y:1, x:9},
					{y:2, x:10},{y:2, x:12}, {y:1, x:13}, {y:1.5, x:16}, {y:1, x:18},
					{y:2, x:18},{y:2, x:20}, {y:1, x:21}, {y:1.5, x:24}, {y:1, x:26}
				],
			}]
		},
		options: {
			responsive: true,
			title: {
				display: true,
				text: 'Tipo de Cambio Spot 25 Octubre'
			},
			tooltips: {
				mode: 'index',
				intersect: false,
			},
			hover: {
				mode: 'nearest',
				intersect: true
			},
			scales: {
				xAxes: [{
					display: true,
					ticks: {
						fontSize: 7,
						min: 1,
						max: 26,
						stepSize: 1,
						callback: function(label, index, labels) {
							console.log(index);
							return index+1;
						}
					},
					scaleLabel: {
						display: true,
						labelString: 'Hora'
					}
				}],
				yAxes: [{
					display: true,
					ticks: {
						min: 0,
						beginAtZero: true,
						max: 5,
						stepSize: 1,
						callback: function(label, index, labels) {
							return '3.2340';
						}
					},
					scaleLabel: {
						display: true,
						labelString: 'Tipo de Cambio'
					}
				}]
			}
		}
	});
});