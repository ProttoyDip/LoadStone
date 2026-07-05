// Chart.js configuration. Chart.js is loaded from wwwroot/lib and used ONLY on the client.
window.LodestoneCharts = {
    renderRiskTrend(selector) {
        const el = document.querySelector(selector);
        if (!el || typeof Chart === 'undefined') return;
        const data = JSON.parse(el.dataset.points || '[]');
        new Chart(el, {
            type: 'line',
            data: {
                labels: data.map(p => p.dateUtc),
                datasets: [{ label: 'High-risk students', data: data.map(p => p.highRiskCount) }]
            }
        });
    }
};
