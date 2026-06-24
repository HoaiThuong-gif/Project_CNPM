const statsRoot = document.querySelector(".main");

if (statsRoot) {
    const statisticsUrl = statsRoot.dataset.statisticsUrl;
    const diseaseApi = statsRoot.dataset.diseaseApi;
    const medicineApi = statsRoot.dataset.medicineApi;
    const loginUrl = statsRoot.dataset.loginUrl;
    const token = localStorage.getItem("token");

    const request = async (url) => {
        const headers = {
            Accept: "application/json"
        };

        if (token) {
            headers.Authorization = `Bearer ${token}`;
        }

        const response = await fetch(url, { headers });

        if (response.status === 401 || response.status === 403) {
            alert("Phiên đăng nhập không hợp lệ hoặc bạn không có quyền truy cập.");
            window.location.href = loginUrl;
            throw new Error("Unauthorized");
        }

        return response;
    };

    const formatNumber = (value) => Number(value || 0).toLocaleString("vi-VN");

    const setBarHeight = (elementId, value, max) => {
        const element = document.getElementById(elementId);
        const ratio = max > 0 ? Math.max(20, Math.round((value / max) * 280)) : 20;
        element.style.height = `${ratio}px`;
    };

    const loadStats = async () => {
        const [statsResponse, diseaseResponse, medicineResponse] = await Promise.all([
            request(statisticsUrl),
            request(diseaseApi),
            request(medicineApi)
        ]);

        const stats = await statsResponse.json();
        const diseases = await diseaseResponse.json();
        const medicines = await medicineResponse.json();

        document.getElementById("statsUsers").textContent = formatNumber(stats.totalActiveUsers);
        document.getElementById("statsPredictions").textContent = formatNumber(stats.totalPredictionsMade);
        document.getElementById("statsMedicines").textContent = formatNumber(stats.totalActiveMedicines);
        document.getElementById("statsDiseases").textContent = formatNumber(stats.totalActiveDiseases);

        const maxValue = Math.max(
            stats.totalActiveUsers || 0,
            stats.totalPredictionsMade || 0,
            stats.totalActiveMedicines || 0,
            stats.totalActiveDiseases || 0
        );

        setBarHeight("barUsers", stats.totalActiveUsers, maxValue);
        setBarHeight("barPredictions", stats.totalPredictionsMade, maxValue);
        setBarHeight("barMedicines", stats.totalActiveMedicines, maxValue);
        setBarHeight("barDiseases", stats.totalActiveDiseases, maxValue);

        const activityPercent = stats.totalPredictionsMade > 0
            ? Math.min(100, Math.round((stats.totalActiveDiseases + stats.totalActiveMedicines) / stats.totalPredictionsMade * 100))
            : 0;

        const circle = document.getElementById("activityCircle");
        const circleText = document.getElementById("activityCircleText");
        circle.style.background = `conic-gradient(#38bdf8 0% ${activityPercent}%, #e2e8f0 ${activityPercent}% 100%)`;
        circleText.textContent = `${activityPercent}%`;

        const table = document.getElementById("statsDiseaseTable");
        const rows = (Array.isArray(diseases) ? diseases : [])
            .filter((item) => item.isActive)
            .slice(0, 5)
            .map((item) => `
                <tr>
                    <td>${item.diseaseName}</td>
                    <td>${item.diseaseGroup || ""}</td>
                    <td><span class="${item.severityLevel === 3 ? "high" : item.severityLevel === 2 ? "medium" : "low"}">${item.severityLevel === 3 ? "Cao" : item.severityLevel === 2 ? "Trung bình" : "Thấp"}</span></td>
                </tr>
            `)
            .join("");

        table.innerHTML = rows || '<tr><td colspan="3">Không có dữ liệu bệnh đang hoạt động.</td></tr>';

        const bar5 = document.querySelector(".bar5");
        const bar6 = document.querySelector(".bar6");
        const activeMedicineCount = (Array.isArray(medicines) ? medicines : []).filter((item) => item.isActive).length;
        const inactiveMedicineCount = (Array.isArray(medicines) ? medicines : []).filter((item) => !item.isActive).length;
        const medicineMax = Math.max(activeMedicineCount, inactiveMedicineCount, 1);
        bar5.style.height = `${Math.max(20, Math.round((activeMedicineCount / medicineMax) * 280))}px`;
        bar6.style.height = `${Math.max(20, Math.round((inactiveMedicineCount / medicineMax) * 280))}px`;
    };

    loadStats().catch((error) => {
        if (error.message !== "Unauthorized") {
            console.error(error);
        }
    });
}
