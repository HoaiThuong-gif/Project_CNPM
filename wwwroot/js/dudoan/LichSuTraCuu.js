const historyRoot = document.querySelector(".history-container");

if (historyRoot) {
    const historyUrl = historyRoot.dataset.historyUrl;
    const resultUrl = historyRoot.dataset.resultUrl;
    const loginUrl = historyRoot.dataset.loginUrl;
    const token = localStorage.getItem("token");
    const historyList = document.getElementById("historyList");
    const searchInput = document.getElementById("historySearchInput");
    const searchButton = document.getElementById("btnHistorySearch");
    const deleteSelectedButton = document.getElementById("btnDeleteSelectedHistory");
    const selectAllCheckbox = document.getElementById("selectAllHistory");
    let historyItems = [];

    const formatDate = (value) => value ? new Date(value).toLocaleString("vi-VN") : "";

    const redirectToLogin = () => {
        const returnUrl = `${window.location.pathname}${window.location.search}`;
        window.location.href = `${loginUrl}?returnUrl=${encodeURIComponent(returnUrl)}`;
    };

    const getSelectedIds = () => {
        return [...historyList.querySelectorAll('input[type="checkbox"][data-history-id]:checked')]
            .map((checkbox) => Number(checkbox.dataset.historyId));
    };

    const buildInfoChips = (symptomsText) => {
        return (symptomsText || "")
            .split(". ")
            .map((item) => item.trim())
            .filter(Boolean)
            .map((item) => `<span class="user-chip">${item}</span>`)
            .join("");
    };

    const renderItems = (items) => {
        if (!items.length) {
            historyList.innerHTML = '<tr><td colspan="6">Tài khoản hiện tại chưa có lịch sử dự đoán nào.</td></tr>';
            return;
        }

        historyList.innerHTML = items.map((item) => `
            <tr>
                <td>
                    <input type="checkbox" data-history-id="${item.historyId}" aria-label="Chọn lịch sử">
                </td>
                <td><strong>${item.diseaseName || "Không có tên bệnh"}</strong></td>
                <td>
                    <div class="user-table-tags">
                        ${buildInfoChips(item.symptoms)}
                    </div>
                </td>
                <td>
                    <div class="user-table-tags">
                        ${(item.medicines || []).map((medicine) => `<span class="user-chip">${medicine}</span>`).join("")}
                    </div>
                </td>
                <td>${formatDate(item.createdAt)}</td>
                <td>
                    <button class="user-table-icon-button" type="button" data-action="view" data-id="${item.historyId}">Xem</button>
                    <button class="user-table-icon-button" type="button" data-action="delete" data-id="${item.historyId}">Xóa</button>
                </td>
            </tr>
        `).join("");
    };

    const filterItems = () => {
        const keyword = searchInput.value.trim().toLowerCase();
        const filtered = historyItems.filter((item) => {
            return [
                item.diseaseName,
                item.symptoms,
                ...(item.medicines || [])
            ].join(" ").toLowerCase().includes(keyword);
        });

        renderItems(filtered);
        if (selectAllCheckbox) {
            selectAllCheckbox.checked = false;
        }
    };

    const request = async (url, options = {}) => {
        const response = await fetch(url, {
            ...options,
            headers: {
                Authorization: `Bearer ${token}`,
                Accept: "application/json",
                ...(options.headers || {})
            }
        });

        if (response.status === 401) {
            redirectToLogin();
            throw new Error("Unauthorized");
        }

        return response;
    };

    const deleteHistoryByIds = async (historyIds) => {
        for (const historyId of historyIds) {
            const response = await request(`${historyUrl}/${historyId}`, {
                method: "DELETE"
            });
            const result = await response.json();
            if (!response.ok) {
                throw new Error(result.message || "Không thể xóa lịch sử.");
            }

            historyItems = historyItems.filter((item) => item.historyId !== historyId);
        }
    };

    const openHistoryDetail = (item) => {
        const predictionInput = {
            diseaseName: item.diseaseName || "",
            symptoms: item.symptoms || "",
            createdAt: item.createdAt || ""
        };

        const predictionResults = Array.isArray(item.results)
            ? item.results.map((result) => ({
                resultId: result.resultId,
                medicineId: result.medicineId,
                medicineName: result.medicineName,
                uses: result.uses,
                dosage: result.dosage,
                howToUse: result.howToUse,
                sideEffects: result.sideEffects,
                notes: result.notes,
                contraindications: result.contraindications,
                score: result.score,
                reason: result.reason,
                warnings: result.warnings
            }))
            : [];

        sessionStorage.setItem("predictionInput", JSON.stringify(predictionInput));
        sessionStorage.setItem("predictionResults", JSON.stringify(predictionResults));
        window.location.href = resultUrl;
    };

    const loadHistory = async () => {
        if (!token) {
            redirectToLogin();
            return;
        }

        const response = await request(historyUrl);
        const result = await response.json();
        if (!response.ok) {
            throw new Error(result.message || "Không thể tải lịch sử tra cứu.");
        }

        historyItems = Array.isArray(result.data) ? result.data : [];
        renderItems(historyItems);
    };

    historyList.addEventListener("click", async (event) => {
        const button = event.target.closest("button");
        if (!button) {
            return;
        }

        const historyId = Number(button.dataset.id);
        const action = button.dataset.action;
        const currentItem = historyItems.find((item) => item.historyId === historyId);

        if (action === "view" && currentItem) {
            openHistoryDetail(currentItem);
            return;
        }

        if (action === "delete") {
            if (!window.confirm("Bạn có chắc muốn xóa lịch sử này?")) {
                return;
            }

            try {
                await deleteHistoryByIds([historyId]);
                filterItems();
            } catch (error) {
                if (error.message !== "Unauthorized") {
                    alert(error.message || "Không thể xóa lịch sử.");
                }
            }
        }
    });

    deleteSelectedButton?.addEventListener("click", async () => {
        const selectedIds = getSelectedIds();
        if (!selectedIds.length) {
            alert("Vui lòng chọn ít nhất một lịch sử để xóa.");
            return;
        }

        if (!window.confirm(`Bạn có chắc muốn xóa ${selectedIds.length} lịch sử đã chọn?`)) {
            return;
        }

        deleteSelectedButton.disabled = true;
        deleteSelectedButton.textContent = "Đang xóa...";

        try {
            await deleteHistoryByIds(selectedIds);
            filterItems();
        } catch (error) {
            if (error.message !== "Unauthorized") {
                alert(error.message || "Không thể xóa lịch sử.");
            }
        } finally {
            deleteSelectedButton.disabled = false;
            deleteSelectedButton.textContent = "Xóa mục đã chọn";
        }
    });

    selectAllCheckbox?.addEventListener("change", () => {
        historyList.querySelectorAll('input[type="checkbox"][data-history-id]').forEach((checkbox) => {
            checkbox.checked = selectAllCheckbox.checked;
        });
    });

    searchInput?.addEventListener("keyup", filterItems);
    searchButton?.addEventListener("click", filterItems);

    loadHistory().catch((error) => {
        if (error.message !== "Unauthorized") {
            console.error(error);
            historyList.innerHTML = `<tr><td colspan="6">${error.message || "Đã xảy ra lỗi khi đọc dữ liệu từ backend."}</td></tr>`;
        }
    });
}
