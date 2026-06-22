const dashboardRoot = document.querySelector(".main-content");

if (dashboardRoot) {
    const statisticsUrl = dashboardRoot.dataset.statisticsUrl;
    const usersApi = dashboardRoot.dataset.usersApi;
    const usersUrl = dashboardRoot.dataset.usersUrl;
    const loginUrl = dashboardRoot.dataset.loginUrl;
    const token = localStorage.getItem("token");

    const totalUsers = document.getElementById("totalUsers");
    const totalPredictions = document.getElementById("totalPredictions");
    const totalMedicines = document.getElementById("totalMedicines");
    const totalDiseases = document.getElementById("totalDiseases");
    const viewAllButton = document.getElementById("btnViewAllUsers");
    const recentUsersTable = document.getElementById("recentUsersTable");
    const cards = document.querySelectorAll(".card");

    const formatNumber = (value) => Number(value || 0).toLocaleString("vi-VN");
    const formatDate = (value) => value ? new Date(value).toLocaleDateString("vi-VN") : "";

    const request = async (url) => {
        const headers = {
            Accept: "application/json"
        };

        if (token) {
            headers.Authorization = `Bearer ${token}`;
        }

        const response = await fetch(url, { headers });

        if (response.status === 401 || response.status === 403) {
            alert("Phiên đăng nhập không hợp lệ hoặc bạn không có quyền truy cập trang quản trị.");
            window.location.href = loginUrl;
            throw new Error("Unauthorized");
        }

        if (!response.ok) {
            throw new Error("Không thể tải dữ liệu thực tế từ hệ thống.");
        }

        return response.json();
    };

    const assignStatistics = (stats) => {
        totalUsers.textContent = formatNumber(stats.totalActiveUsers);
        totalPredictions.textContent = formatNumber(stats.totalPredictionsMade);
        totalMedicines.textContent = formatNumber(stats.totalActiveMedicines);
        totalDiseases.textContent = formatNumber(stats.totalActiveDiseases);
    };

    const renderRecentUsers = (users) => {
        if (!Array.isArray(users) || users.length === 0) {
            recentUsersTable.innerHTML = '<tr><td colspan="5">Chưa có dữ liệu người dùng.</td></tr>';
            return;
        }

        const rows = [...users]
            .sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt))
            .slice(0, 5)
            .map((user) => `
                <tr>
                    <td>${user.fullName || ""}</td>
                    <td>${user.email || ""}</td>
                    <td>${user.role || "User"}</td>
                    <td>${formatDate(user.createdAt)}</td>
                    <td><span class="${user.isDeleted || user.isLocked ? "pending" : "success"}">${user.isDeleted ? "Đã xóa" : user.isLocked ? "Tạm khóa" : "Hoạt động"}</span></td>
                </tr>
            `)
            .join("");

        recentUsersTable.innerHTML = rows;
    };

    const loadDashboard = async () => {
        const [statistics, users] = await Promise.all([
            request(statisticsUrl),
            request(usersApi)
        ]);

        assignStatistics(statistics);
        renderRecentUsers(users);
    };

    if (viewAllButton) {
        viewAllButton.addEventListener("click", () => {
            window.location.href = usersUrl;
        });
    }

    cards.forEach((card) => {
        card.addEventListener("mouseenter", () => {
            card.style.transform = "translateY(-8px)";
        });

        card.addEventListener("mouseleave", () => {
            card.style.transform = "translateY(0px)";
        });
    });

    loadDashboard().catch((error) => {
        if (error.message !== "Unauthorized") {
            console.error(error);
            recentUsersTable.innerHTML = '<tr><td colspan="5">Không thể tải dữ liệu dashboard.</td></tr>';
        }
    });
}
