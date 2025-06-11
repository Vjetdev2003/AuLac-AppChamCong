let chartInstance = null;

window.drawChart = (canvasId, type, data, options) => {
    const ctx = document.getElementById(canvasId).getContext('2d');

    // Hủy biểu đồ cũ nếu tồn tại
    if (chartInstance) {
        chartInstance.destroy();
    }

    // Tạo biểu đồ mới
    chartInstance = new Chart(ctx, {
        type: type,
        data: data,
        options: options
    });
};

window.destroyChart = (canvasId) => {
    if (chartInstance) {
        chartInstance.destroy();
        chartInstance = null;
    }
};