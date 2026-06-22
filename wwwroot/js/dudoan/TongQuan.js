const overviewRoot = document.querySelector(".user-dashboard");

if (overviewRoot) {
    const historyUrl = overviewRoot.dataset.historyUrl;
    const loginUrl = overviewRoot.dataset.loginUrl;
    const token = localStorage.getItem("token");
    const topDiseases = document.getElementById("overviewTopDiseases");
    const recentActivity = document.getElementById("overviewRecentActivity");
    const historyCount = document.getElementById("overviewHistoryCount");
    const latestMedicineCount = document.getElementById("overviewLatestMedicineCount");

    const formatDate = (value) => value ? new Date(value).toLocaleString("vi-VN") : "";
    const redirectToLogin = () => {
        const returnUrl = `${window.location.pathname}${window.location.search}`;
        window.location.href = `${loginUrl}?returnUrl=${encodeURIComponent(returnUrl)}`;
    };

    const renderTopDiseases = (items) => {
        if (!items.length) {
            topDiseases.innerHTML = '<div class="user-empty-state">Chưa có dữ liệu tra cứu.</div>';
            return;
        }

        const grouped = Object.values(items.reduce((accumulator, item) => {
            const key = item.diseaseName || "Không rõ";
            accumulator[key] ??= { diseaseName: key, count: 0 };
            accumulator[key].count += 1;
            return accumulator;
        }, {})).sort((a, b) => b.count - a.count).slice(0, 5);

        const max = Math.max(...grouped.map((item) => item.count), 1);

        topDiseases.innerHTML = grouped.map((item, index) => `
            <div class="user-rank-item">
                <span>${index + 1}</span>
                <div>
                    <strong>${item.diseaseName}</strong>
                    <div class="user-rank-bar"><span style="width:${Math.round((item.count / max) * 100)}%"></span></div>
                </div>
                <span>${item.count} lần</span>
            </div>
        `).join("");
    };

    const renderRecentActivity = (items) => {
        if (!items.length) {
            recentActivity.innerHTML = '<div class="user-empty-state">Chưa có hoạt động gần đây.</div>';
            return;
        }

        recentActivity.innerHTML = items.slice(0, 5).map((item) => `
            <div class="user-activity-item">
                <div class="user-activity-icon">
                    <svg viewBox="0 0 24 24" aria-hidden="true"><path d="M14.12 5.88 18.24 10 10 18.24 5.88 14.12Zm0-2.83a2 2 0 0 1 2.83 0L21.07 7.17a2 2 0 0 1 0 2.83l-1.41 1.41-6.95-6.95Z" fill="currentColor" /></svg>
                </div>
                <div>
                    <strong>Tra cứu thuốc</strong>
                    <p>${item.diseaseName || "Không rõ"} - ${(item.medicines || []).length} thuốc gợi ý</p>
                </div>
                <time>${formatDate(item.createdAt)}</time>
            </div>
        `).join("");
    };

    const loadOverview = async () => {
        if (!token) {
            redirectToLogin();
            return;
        }

        const response = await fetch(historyUrl, {
            headers: {
                Authorization: `Bearer ${token}`,
                Accept: "application/json"
            }
        });

        if (response.status === 401) {
            redirectToLogin();
            return;
        }

        const result = await response.json();
        if (!response.ok) {
            throw new Error(result.message || "Không thể tải dữ liệu tổng quan.");
        }

        const items = Array.isArray(result.data) ? result.data : [];
        historyCount.textContent = String(items.length);
        latestMedicineCount.textContent = String(items[0]?.medicines?.length || 0);
        renderTopDiseases(items);
        renderRecentActivity(items);
    };

    loadOverview().catch((error) => {
        console.error(error);
        if (topDiseases) {
            topDiseases.innerHTML = `<div class="user-empty-state">${error.message || "Không thể tải dữ liệu."}</div>`;
        }
        if (recentActivity) {
            recentActivity.innerHTML = `<div class="user-empty-state">${error.message || "Không thể tải dữ liệu."}</div>`;
        }
    });
}
