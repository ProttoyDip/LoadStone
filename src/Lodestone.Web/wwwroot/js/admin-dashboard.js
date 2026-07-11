(function () {
  const body = document.body;
  if (!body.classList.contains('ls-admin')) {
    return;
  }

  const collapseBtn = document.getElementById('ls-admin-side-collapse');

  if (collapseBtn) {
    collapseBtn.addEventListener('click', () => {
      body.classList.toggle('ls-admin-sidebar-collapsed');
    });
  }

  const animateCounter = (element) => {
    const text = (element.textContent || '').trim();
    if (!/^[\d,.]+%?$/.test(text)) return;

    const hasPercent = text.endsWith('%');
    const target = Number(text.replace(/[^0-9.]/g, ''));
    if (!Number.isFinite(target)) return;

    const duration = 850;
    const start = performance.now();

    const tick = (now) => {
      const progress = Math.min((now - start) / duration, 1);
      const easing = 1 - Math.pow(1 - progress, 3);
      const current = target * easing;
      const formatted = text.includes(',')
        ? current.toLocaleString(undefined, { maximumFractionDigits: text.includes('.') ? 1 : 0 })
        : current.toFixed(text.includes('.') ? 1 : 0);

      element.textContent = hasPercent ? `${formatted}%` : formatted;

      if (progress < 1) {
        requestAnimationFrame(tick);
      }
    };

    requestAnimationFrame(tick);
  };

  document.querySelectorAll('.ls-admin-stat-card__value').forEach((element) => {
    animateCounter(element);
  });

  const drawSparkline = (canvas, values) => {
    if (!canvas || !Array.isArray(values) || !values.length) return;
    const ratio = window.devicePixelRatio || 1;
    const width = canvas.clientWidth || canvas.parentElement?.clientWidth || 220;
    const height = canvas.height || 38;
    canvas.width = width * ratio;
    canvas.height = height * ratio;

    const context = canvas.getContext('2d');
    if (!context) return;

    context.scale(ratio, ratio);
    context.clearRect(0, 0, width, height);

    const min = Math.min(...values);
    const max = Math.max(...values);
    const padding = 4;
    const span = Math.max(max - min, 1);

    context.beginPath();
    values.forEach((value, index) => {
      const x = padding + ((width - padding * 2) * index) / Math.max(values.length - 1, 1);
      const y = height - padding - ((height - padding * 2) * (value - min)) / span;
      if (index === 0) {
        context.moveTo(x, y);
      } else {
        context.lineTo(x, y);
      }
    });
    context.lineWidth = 2;
    context.strokeStyle = '#BC5138';
    context.stroke();
  };

  const safeParseJSON = (value, fallback) => {
    try {
      return JSON.parse(value);
    } catch (e) {
      return fallback;
    }
  };

  const toNumberArray = (values) => Array.isArray(values)
    ? values.map((value) => (typeof value === 'number' ? value : Number(value))).filter((value) => Number.isFinite(value))
    : [];

  document.querySelectorAll('.ls-admin-sparkline').forEach((canvas) => {
    const raw = canvas.dataset.sparkline;
    const values = safeParseJSON(raw ?? '[]', []);

    if (!Array.isArray(values) || values.length === 0) return;
    drawSparkline(canvas, values);
  });

  const chartCanvas = Array.from(document.querySelectorAll('[data-chart-config]'));
  const chartInstances = [];

  const themePalette = [
    { border: '#BC5138', fill: 'rgba(188,81,56,.16)' },
    { border: '#3C5647', fill: 'rgba(60,86,71,.16)' },
    { border: '#E0A85A', fill: 'rgba(224,168,90,.20)' },
    { border: '#1F2937', fill: 'rgba(31,41,55,.14)' }
  ];

  const getChartPalette = (index = 0) => themePalette[index % themePalette.length];

  if (window.Chart && chartCanvas.length) {
    chartCanvas.forEach((canvas) => {
      const chartType = (canvas.dataset.chartType || 'line').toLowerCase();
      const rawConfig = safeParseJSON(canvas.dataset.chartConfig || '{}', null);

      if (!rawConfig || !Array.isArray(rawConfig.datasets) || rawConfig.datasets.length === 0) {
        return;
      }

      const isDoughnut = chartType === 'doughnut';
      const isBar = chartType === 'bar';
      const isArea = chartType === 'area';
      const isLineLike = isArea || chartType === 'line';
      const resolvedType = isArea ? 'line' : chartType;

      const normalizedData = {
        labels: Array.isArray(rawConfig.labels) ? rawConfig.labels : [],
        datasets: rawConfig.datasets.map((dataset, index) => {
          const palette = getChartPalette(index);
          const data = toNumberArray(dataset.data);
          const hasExplicitBorder = typeof dataset.borderColor !== 'undefined' && dataset.borderColor !== null;
          const hasExplicitFill = typeof dataset.backgroundColor !== 'undefined' && dataset.backgroundColor !== null;

          return {
            ...dataset,
            data,
            borderWidth: isDoughnut ? 0 : (dataset.borderWidth ?? (isBar ? 0 : 3)),
            borderRadius: isBar ? (dataset.borderRadius ?? 12) : 0,
            borderSkipped: isBar ? false : (dataset.borderSkipped ?? undefined),
            hoverBorderWidth: isDoughnut ? 0 : (dataset.hoverBorderWidth ?? 2),
            showLine: isLineLike ? true : dataset.showLine,
            tension: isLineLike ? (dataset.tension ?? (isArea ? 0.42 : 0.34)) : dataset.tension,
            pointRadius: isLineLike ? (dataset.pointRadius ?? 4) : 0,
            pointHoverRadius: isLineLike ? (dataset.pointHoverRadius ?? 6) : 0,
            fill: isArea ? true : (dataset.fill ?? false),
            spanGaps: isLineLike ? (dataset.spanGaps ?? true) : dataset.spanGaps,
            backgroundColor: hasExplicitFill
              ? dataset.backgroundColor
              : (isDoughnut ? palette.fill : (isBar ? palette.border : palette.fill)),
            borderColor: hasExplicitBorder
              ? dataset.borderColor
              : (isDoughnut ? palette.border : palette.border),
            pointBackgroundColor: isLineLike ? (dataset.pointBackgroundColor ?? (hasExplicitBorder ? dataset.borderColor : palette.border)) : dataset.pointBackgroundColor,
            pointBorderColor: isLineLike ? (dataset.pointBorderColor ?? '#F4EEE3') : dataset.pointBorderColor,
            pointBorderWidth: isLineLike ? (dataset.pointBorderWidth ?? 2) : dataset.pointBorderWidth
          };
        })
      };

      const options = {
        responsive: true,
        maintainAspectRatio: false,
        animation: {
          duration: 900,
          easing: 'easeOutQuart'
        },
        layout: {
          padding: isDoughnut ? 12 : 8
        },
        interaction: {
          mode: 'index',
          intersect: false
        },
        plugins: {
          legend: {
            display: true,
            position: isDoughnut ? 'bottom' : 'top',
            labels: {
              boxWidth: 10,
              usePointStyle: true,
              padding: 14
            }
          },
          tooltip: {
            backgroundColor: 'rgba(32,28,23,.96)',
            titleColor: '#F4EEE3',
            bodyColor: '#F4EEE3',
            borderColor: 'rgba(244,238,227,.16)',
            borderWidth: 1,
            padding: 12,
            displayColors: true
          }
        },
        elements: {
          line: {
            borderCapStyle: 'round',
            borderJoinStyle: 'round'
          },
          point: {
            hoverBorderWidth: 2
          }
        },
        scales: isDoughnut
          ? {}
          : {
              x: {
                grid: {
                  display: false
                },
                ticks: {
                  color: '#857B6C',
                  maxRotation: 0,
                  autoSkip: true
                }
              },
              y: {
                beginAtZero: true,
                grid: {
                  color: 'rgba(32,28,23,.08)',
                  drawBorder: false
                },
                ticks: {
                  color: '#857B6C',
                  padding: 8
                }
              }
            }
      };

      if (isDoughnut) {
        options.cutout = '68%';
      }

      const existing = window.Chart.getChart(canvas);
      if (existing) {
        existing.destroy();
      }

      const chart = new Chart(canvas, {
        type: resolvedType,
        data: normalizedData,
        options
      });

      chartInstances.push(chart);
    });

    const resizeCharts = () => {
      chartInstances.forEach((chart) => {
        if (chart && typeof chart.resize === 'function') {
          chart.resize();
        }
      });
    };

    requestAnimationFrame(resizeCharts);
    window.addEventListener('load', resizeCharts, { once: true });
    window.addEventListener('resize', resizeCharts);
  }

  const badge = document.getElementById('ls-unread-badge');
  const connectNotifications = () => {
    if (!window.signalR || !badge) return;
    const connection = new signalR.HubConnectionBuilder()
      .withUrl('/hubs/admin-notifications')
      .withAutomaticReconnect()
      .build();

    connection.on('notificationCountUpdated', (count) => {
      badge.textContent = count;
      badge.classList.add('ls-admin-notification-pulse');
      setTimeout(() => badge.classList.remove('ls-admin-notification-pulse'), 1000);
    });

    connection.on('notificationReceived', (notification) => {
      badge.textContent = Number(badge.textContent || '0') + 1;
      badge.classList.add('ls-admin-notification-pulse');
      setTimeout(() => badge.classList.remove('ls-admin-notification-pulse'), 1000);
      console.info('Admin notification received', notification);
    });

    connection.start().catch((error) => console.warn('Admin notification hub connection failed', error));
  };

  connectNotifications();
})();
