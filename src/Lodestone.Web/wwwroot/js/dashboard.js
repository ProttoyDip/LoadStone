// Dashboard bootstrap: hydrates metrics and delegates chart rendering to chart-config.js.
document.addEventListener('DOMContentLoaded', () => {
    if (window.LodestoneCharts) {
        window.LodestoneCharts.renderRiskTrend('#riskTrendChart');
    }
});
