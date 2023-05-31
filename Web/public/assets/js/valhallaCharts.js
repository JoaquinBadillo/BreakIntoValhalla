const DB_HOST = "http://localhost";
const DB_PORT = '5000';

// Chart.defaults.global.font.size = 16;

Chart.defaults.color = '#FFFFFF';
Chart.defaults.borderColor = 'rgba(255, 255, 255, 0.3)';

fetch(`${DB_HOST}:${DB_PORT}/api/metrics/leaderboards/top_kills`,{method: 'GET'})
.catch(err => console.log(err))
.then(response => response.json())
.then(data => {
    const colors = [
        "#3A87FD",
        "#5095FD",
        "#66A2FE",
        "#7CAFFE",
        "#92BDFE",
        "#A7CAFE",
        "#BDD7FE",
        "#D3E4FF",
        "#E9F2FF",
        "#FFFFFF"
    ];
    const values = Object.values(data);
    console.log(values);
    const player_names = values.map(d => d['username']);
    const player_kills = values.map(d => d['kills']);
    const borders = values.map(d => '#012A4A');

    const ctx_top_kills = document.getElementById('top_kills').getContext('2d');
    const top_kills_chart = new Chart(ctx_top_kills, {
        type: 'bar',
        data: {
            labels: player_names,
            datasets: [{
                label: 'Top Players by Kills',
                backgroundColor: colors,
                borderColor: borders,
                data: player_kills
            }]
        },

        options: {
            indexAxis: 'y',
            scales: {
                y: {
                    grid: {
                        drawOnChartArea: false,
                        drawTicks: false
                    }
                }
            }
        }
    });
});