var options = {
	chart: {
		height: 320,
		type: 'area',
		toolbar: {
			show: false,
		},
		dropShadow: {
			enabled: true,
			opacity: 0.3,
			blur: 5,
			left: -10,
			top: 20
		},
	},
	colors: ['#cd9d63', '#383737'],
	dataLabels: {
		enabled: false,
	},
	legend: {
  	show: false,
  },
	stroke: {
		show: true,
		curve: 'smooth',
		width: 3,
		lineCap: 'square'
	},
	series: [{
		name: 'OrderReceived',
		data: [250, 350, 300, 290, 300, 275, 320]
	},{
    name: 'OrderCancel',
    data: [10, 50, 15, 100, 45, 60, 14]
  }],
	xaxis: {
		axisBorder: {
			show: false
		},
		categories: ["SUN", "MON", "TUE", "WED", "THU", "FRI", "SAT"],
		axisTicks: {
			show: true
		},
		crosshairs: {
			show: true
		},
		labels: {
			offsetX: 0,
			offsetY: 5,
		}
	},
	yaxis: {
		labels: {
			formatter: function(value, index) {
				return (value / 1000) + 'K'
			},
			offsetX: -15,
			offsetY: 20,
		}
	},
	grid: {
		borderColor: '#e0e6ed',
		strokeDashArray: 5,
		xaxis: {
			lines: {
				show: true
			}
		},   
		yaxis: {
			lines: {
				show: false,
			}	
		},
		padding: {
			top: 0,
			right: 0,
			bottom: 0,
			left: 0
		}, 
	}, 
	fill: {
		type:"gradient",
		gradient: {
			type: "vertical",
			shadeIntensity: 1,
			inverseColors: !1,
			opacityFrom: .3,
			opacityTo: .05,
			stops: [15, 100]
		}
	},
}

var chart = new ApexCharts(
	document.querySelector("#lineRevenueGraph"),
	options
);

chart.render();