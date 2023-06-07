const URI = "https://valhallaapi-production.up.railway.app";

// Chart.defaults.global.font.size = 16;

Chart.defaults.color = '#FFFFFF';
Chart.defaults.borderColor = 'rgba(255, 255, 255, 0.3)';

fetch(`${URI}/api/users/metrics/leaderboards/top_kills`,{method: 'GET'})
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
                label: 'Kills',
                backgroundColor: colors,
                borderColor: borders,
                data: player_kills
            }]
        },

        options: {
            plugins: {
                title: {
                  display: true,
                  text: 'Top Players by Kills',
                  font: {
                      size: 30
                  }
                }
            },
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



fetch(`${URI}/api/deaths/death_place`,{method: 'GET'})
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
    const rooms = values.map(d => d['room']);
    const totals = values.map(d => d['total']);
    const borders = values.map(d => '#012A4A');

    const ctx_top_death_place = document.getElementById('death_place').getContext('2d');
    const death_place_chart = new Chart(ctx_top_death_place, {
        type: 'pie',
        data: {   
            labels: rooms,
            datasets: [{
                label: 'Deaths',
                backgroundColor: colors,
                borderColor: borders,
                data: totals
            }]
        },
        options: {
            plugins: {
              title: {
                display: true,
                text: 'Deaths by Room',
                font: {
                    size: 30
                }
              }
            }
        }
            
    });
});

fetch(`${URI}/api/deaths/death_cause`,{method: 'GET'})
.catch(err => console.log(err))
.then(response => response.json())
.then(data => {
    const colors = [
        "#175e54",
        "#279989",
        "#6fa287",
        "##4298B5"
        
    ];
    const values = Object.values(data);
    console.log(values);
    const killers = values.map(d => d['killer']);
    const totals = values.map(d => d['total']);
    const borders = values.map(d => '#012A4A');

    const ctx_top_death_cause = document.getElementById('death_cuause').getContext('2d');
    const death_cause_chart = new Chart(ctx_top_death_cause, {
        type: 'pie',
        data: {
            labels: killers,
            datasets: [{
                label: 'Death Caused',
                backgroundColor: colors,
                borderColor: borders,
                data: totals
            }]
        },
        options: {
            plugins: {
              title: {
                display: true,
                text: 'Deaths by Cause',
                font: {
                    size: 30
                }
              }
            }
        }
        
    });
});


