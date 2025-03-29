const menuButton = document.getElementById('menu-button');
const mobileMenu = document.getElementById('mobile-menu');
if (menuButton) {
    menuButton.addEventListener('click', () => {
        mobileMenu.classList.toggle('hidden');
    });
}

const ctxIncidencias = document.getElementById('chartIncidencias').getContext('2d');
const chartIncidencias = new Chart(ctxIncidencias, {
    type: 'bar',
    data: {
        labels: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun'],
        datasets: [{
            label: 'Número de Incidencias',
            data: [12, 19, 3, 5, 2, 3],
            backgroundColor: 'rgba(54, 162, 235, 0.5)',
            borderColor: 'rgba(54, 162, 235, 1)',
            borderWidth: 1
        }]
    },
    options: {
        scales: {
            y: {
                beginAtZero: true
            }
        }
    }
});

const ctxTiempos = document.getElementById('chartTiempos').getContext('2d');
const chartTiempos = new Chart(ctxTiempos, {
    type: 'line',
    data: {
        labels: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun'],
        datasets: [{
            label: 'Tiempo de Respuesta (minutos)',
            data: [30, 25, 20, 22, 18, 15],
            backgroundColor: 'rgba(255, 206, 86, 0.5)',
            borderColor: 'rgba(255, 206, 86, 1)',
            fill: true,
            tension: 0.4
        }]
    },
    options: {
        scales: {
            y: {
                beginAtZero: true
            }
        }
    }
});

 const map = L.map('map').setView([10.9685, -74.7813], 13);

 L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
     attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
 }).addTo(map);

 const marker = L.marker([10.9685, -74.7813]).addTo(map);
 marker.bindPopup("<b>Incidencia Reportada</b><br>Ubicación: Barranquilla, Colombia.").openPopup();