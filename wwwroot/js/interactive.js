document.addEventListener("DOMContentLoaded", async function () {
    // console.log("Script de mapa cargado");

    const map = L.map('map').setView([10.0, -75.0], 6);

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '&copy; OpenStreetMap contributors'
    }).addTo(map);

    try {
        console.log("Intentando obtener reportes...");
        const response = await fetch('/api/reports');

        if (!response.ok) {
            throw new Error(`Error en API: ${response.status}`);
        }

        const reports = await response.json();
        // console.log("Datos obtenidos:", reports);

        reports.forEach(report => {
            if (!report.latitude || !report.longitude) {
                console.warn("Reporte con coordenadas inválidas:", report);
                return;
            }

            console.log("Reporte válido:", report);

            let formattedDate = report.occurredAt 
                ? new Date(report.occurredAt).toLocaleString() 
                : "Fecha no disponible";

            const reportedBy = report.reportedByUsername && report.reportedByUsername.trim() !== "" 
                ? report.reportedByUsername 
                : "Desconocido";

            L.marker([report.latitude, report.longitude]).addTo(map)
                .bindPopup(`
                    <strong>${report.address || "Sin dirección"}</strong><br>
                    ${report.description || "Sin descripción"}<br>
                    <small>${formattedDate}</small><br>
                    <em>Reportado por: ${reportedBy}</em>
                `);
        });
    } catch (error) {
        console.error("Error cargando reportes:", error);
    }
});
