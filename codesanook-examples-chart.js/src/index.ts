import { ChartConfiguration, Chart, ChartData, ChartOptions } from 'chart.js';

//import chart plugin as part of bundle.js
require('chartjs-plugin-labels')

let data: ChartData = {
    labels: [ //set label of each data set
        'female',
        'male',
    ],
    datasets: [{
        data: [//raw data of each set
            55, //female amount
            45 //male amount
        ],
        backgroundColor: [ //set color of each data in pie chart UI, support opacity value
            'rgba(255, 99, 132, 1)', //color for female
            'rgba(54, 162, 235, 1)' //color for male
        ]
    }]
};

var chartOptions: ChartOptions = {
    legend: {
        display: true //show legend at the top of a chart
    },
    tooltips: {
        enabled: false //not show tooltips when hover a chart
    },
    plugins: { //setup chartjs-plugin-labels plug in 
        labels: [
            {
                render: 'label',
                position: 'outside'
            },
            {
                render: 'value'
            }
        ],
    }
};

var configuration: ChartConfiguration = {
    type: 'pie',
    data: data,
    options: chartOptions
};

let element = document.getElementById('myChart') as HTMLCanvasElement;
new Chart(element, configuration);;