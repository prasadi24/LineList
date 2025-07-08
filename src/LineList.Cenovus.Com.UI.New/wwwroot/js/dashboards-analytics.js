/**
 * Dashboard Analytics
 */

'use strict';

(function () {
    let cardColor, headingColor, axisColor, shadeColor, borderColor;

    cardColor = config.colors.white;
    headingColor = config.colors.headingColor;
    axisColor = config.colors.axisColor; 5
    borderColor = config.colors.borderColor;

    var chartOptions = {
        series: osData.map(item => item.value),
        labels: osData.map(item => item.key),
        chart: {
            type: 'pie',
            height: 350
        },
        legend: {
            position: 'bottom'
        }
    };

    var chart = new ApexCharts(document.querySelector("#osPieChart"), chartOptions);
    chart.render();

    var chartOptions = {
        series: locationData.map(item => item.value),
        labels: locationData.map(item => item.key),
        chart: {
            type: 'pie',
            height: 350
        },
        legend: {
            position: 'bottom'
        }
    };

    var chart = new ApexCharts(document.querySelector("#locationPieChart"), chartOptions);
    chart.render();

    var chartOptions = {
        series: assignmentGroupServerData.map(item => item.value),
        labels: assignmentGroupServerData.map(item => item.key),
        chart: {
            type: 'pie',
            height: 350
        },
        legend: {
            position: 'bottom'
        }
    };

    var chart = new ApexCharts(document.querySelector("#assignmentGroupPieChart"), chartOptions);
    chart.render();

    var chartOptions = {
        series: criticalityData.map(item => item.value),
        labels: criticalityData.map(item => item.key),
        chart: {
            type: 'pie',
            height: 350
        },
        legend: {
            position: 'bottom'
        }
    };

    var chart = new ApexCharts(document.querySelector("#criticalityPieChart"), chartOptions);
    chart.render();

    var chartOptions = {
        series: serviceOwnerData.map(item => item.value),
        labels: serviceOwnerData.map(item => item.key),
        chart: {
            type: 'pie',
            height: 350
        },
        legend: {
            position: 'bottom'
        }
    };

    var chart = new ApexCharts(document.querySelector("#serviceOwnerPieChart"), chartOptions);
    chart.render();

    var chartOptions = {
        series: portfolioData.map(item => item.value),
        labels: portfolioData.map(item => item.key),
        chart: {
            type: 'pie',
            height: 350
        },
        legend: {
            position: 'bottom'
        }
    };

    var chart = new ApexCharts(document.querySelector("#portfolioPieChart"), chartOptions);
    chart.render();
})();