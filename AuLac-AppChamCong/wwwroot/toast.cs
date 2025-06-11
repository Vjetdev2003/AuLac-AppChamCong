window.showToast = (message, type) => {
    const toast = document.createElement("div");
    toast.className = `toast toast-${ type}`;
    toast.innerText = message;
    document.body.appendChild(toast);

    // Hiển thị toast
    setTimeout(() => {
        toast.classList.add("show");
    }, 100);

    // Ẩn toast sau 3 giây
    setTimeout(() => {
        toast.classList.remove("show");
        setTimeout(() => {
            document.body.removeChild(toast);
        }, 500); // Đợi animation kết thúc
    }, 3000);
};
