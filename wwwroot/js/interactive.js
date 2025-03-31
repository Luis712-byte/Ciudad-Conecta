document.addEventListener("DOMContentLoaded", async function () {
    const map = L.map('map').setView([10.0, -75.0], 6); 

    // Agregar capa base de OpenStreetMap
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '&copy; OpenStreetMap contributors'
    }).addTo(map);

    try {
        const response = await fetch('/api/reports'); 
        const reports = await response.json();

        reports.forEach(report => {
            if (!report.latitude || !report.longitude) {
                console.warn("Reporte con coordenadas inválidas:", report);
                return;
            }

            let formattedDate = "Fecha no disponible";
            if (report.occurredAt) {
                const dateObj = new Date(report.occurredAt);
                if (!isNaN(dateObj.getTime())) {
                    formattedDate = dateObj.toLocaleString();
                }
            }

            const marker = L.marker([report.latitude, report.longitude]).addTo(map)
                .bindPopup(`
                    <strong>${report.address || "Sin dirección"}</strong><br>
                    ${report.description || "Sin descripción"}<br>
                    <small>${formattedDate}</small>
                `);
        });
    } catch (error) {
        console.error("Error cargando reportes:", error);
    }
});
