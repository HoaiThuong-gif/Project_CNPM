const diseaseRoot = document.querySelector(".main");

if (diseaseRoot) {
    const apiUrl = diseaseRoot.dataset.diseaseApi;
    const loginUrl = diseaseRoot.dataset.loginUrl;
    const token = localStorage.getItem("token");
    const tableBody = document.getElementById("benhTable");
    const searchInput = document.getElementById("searchInput");
    const createButton = document.getElementById("btnCreateDisease");
    const modal = document.getElementById("diseaseModalBackdrop");
    const modalTitle = document.getElementById("diseaseModalTitle");
    const closeModalButton = document.getElementById("btnCloseDiseaseModal");
    const cancelModalButton = document.getElementById("btnCancelDiseaseModal");
    const form = document.getElementById("diseaseForm");
    const submitButton = document.getElementById("btnSubmitDiseaseModal");
    const diseaseIdInput = document.getElementById("diseaseId");
    const diseaseNameInput = document.getElementById("diseaseName");
    const diseaseGroupInput = document.getElementById("diseaseGroup");
    const diseaseDescriptionInput = document.getElementById("diseaseDescription");
    const diseaseSeverityInput = document.getElementById("diseaseSeverity");
    let diseases = [];
    let editingDisease = null;

    const request = async (url, options = {}) => {
        const headers = {
            "Content-Type": "application/json",
            ...(options.headers || {})
        };

        if (token) {
            headers.Authorization = `Bearer ${token}`;
        }

        const response = await fetch(url, {
            ...options,
            headers
        });

        if (response.status === 401 || response.status === 403) {
            alert("Phiên đăng nhập không hợp lệ hoặc bạn không có quyền truy cập.");
            window.location.href = loginUrl;
            throw new Error("Unauthorized");
        }

        return response;
    };

    const formatDate = (value) => value ? new Date(value).toLocaleDateString("vi-VN") : "";

    const levelLabel = (value) => {
        if (value === 3) return "Nặng";
        if (value === 2) return "Trung bình";
        return "Nhẹ";
    };

    const openModal = (disease) => {
        editingDisease = disease || null;
        modalTitle.textContent = disease ? "Cập nhật bệnh" : "Thêm bệnh";
        submitButton.textContent = disease ? "Cập nhật" : "Lưu";
        diseaseIdInput.value = disease?.diseaseId || "";
        diseaseNameInput.value = disease?.diseaseName || "";
        diseaseGroupInput.value = disease?.diseaseGroup || "";
        diseaseDescriptionInput.value = disease?.description || "";
        diseaseSeverityInput.value = String(disease?.severityLevel || 1);
        modal.classList.remove("hidden");
    };

    const closeModal = () => {
        modal.classList.add("hidden");
        form.reset();
        diseaseIdInput.value = "";
        editingDisease = null;
    };

    const renderDiseases = (source) => {
        if (!source.length) {
            tableBody.innerHTML = '<tr><td colspan="7">Không có dữ liệu bệnh.</td></tr>';
            return;
        }

        tableBody.innerHTML = source.map((disease) => `
            <tr>
                <td>#B${String(disease.diseaseId).padStart(3, "0")}</td>
                <td>${disease.diseaseName || ""}</td>
                <td>${disease.diseaseGroup || ""}</td>
                <td><span class="level ${disease.severityLevel === 3 ? "high" : disease.severityLevel === 2 ? "medium" : "low"}">${levelLabel(disease.severityLevel)}</span></td>
                <td>${disease.isActive ? "Hoạt động" : "Ngừng hoạt động"}</td>
                <td>${formatDate(disease.createdAt)}</td>
                <td class="action-group">
                    <button class="edit-btn" type="button" data-action="edit" data-id="${disease.diseaseId}">Sửa</button>
                    <button class="delete-btn" type="button" data-action="toggle" data-id="${disease.diseaseId}" ${!disease.isActive ? "disabled" : ""}>Tắt</button>
                </td>
            </tr>
        `).join("");
    };

    const loadDiseases = async () => {
        const response = await request(apiUrl, { method: "GET" });
        const data = await response.json();
        diseases = Array.isArray(data) ? data : [];
        renderDiseases(diseases);
    };

    createButton.addEventListener("click", () => openModal());
    closeModalButton.addEventListener("click", closeModal);
    cancelModalButton.addEventListener("click", closeModal);
    modal.addEventListener("click", (event) => {
        if (event.target === modal) closeModal();
    });

    form.addEventListener("submit", async (event) => {
        event.preventDefault();

        const payload = {
            diseaseName: diseaseNameInput.value.trim(),
            diseaseGroup: diseaseGroupInput.value.trim(),
            description: diseaseDescriptionInput.value.trim(),
            severityLevel: Number(diseaseSeverityInput.value || 1)
        };

        if (!payload.diseaseName || !payload.diseaseGroup) {
            alert("Vui lòng nhập đầy đủ tên bệnh và nhóm bệnh.");
            return;
        }

        try {
            const response = await request(editingDisease ? `${apiUrl}/Update` : `${apiUrl}/create`, {
                method: editingDisease ? "PATCH" : "POST",
                body: JSON.stringify(editingDisease ? {
                    diseaseId: editingDisease.diseaseId,
                    isActive: editingDisease.isActive,
                    ...payload
                } : payload)
            });

            const result = await response.json();
            if (!response.ok) {
                throw new Error(result.message || "Không thể lưu bệnh.");
            }

            alert(result.message || "Lưu bệnh thành công.");
            closeModal();
            await loadDiseases();
        } catch (error) {
            if (error.message !== "Unauthorized") alert(error.message);
        }
    });

    tableBody.addEventListener("click", async (event) => {
        const button = event.target.closest("button");
        if (!button) return;

        const diseaseId = Number(button.dataset.id);
        const action = button.dataset.action;

        try {
            if (action === "edit") {
                const existing = diseases.find((item) => item.diseaseId === diseaseId);
                if (!existing) return;
                openModal(existing);
                return;
            }

            if (action === "toggle") {
                if (!window.confirm("Bạn có chắc muốn tắt bệnh này?")) return;

                const response = await request(`${apiUrl}/toggle?id=${diseaseId}`, {
                    method: "PATCH"
                });

                const result = await response.json();
                if (!response.ok) {
                    throw new Error(result.message || "Không thể tắt bệnh.");
                }

                alert(result.message || "Đã tắt bệnh.");
                await loadDiseases();
            }
        } catch (error) {
            if (error.message !== "Unauthorized") alert(error.message);
        }
    });

    searchInput.addEventListener("keyup", () => {
        const filter = searchInput.value.trim().toLowerCase();
        const filtered = diseases.filter((disease) => {
            return [
                disease.diseaseName,
                disease.diseaseGroup,
                disease.description
            ].join(" ").toLowerCase().includes(filter);
        });

        renderDiseases(filtered);
    });

    loadDiseases().catch((error) => {
        if (error.message !== "Unauthorized") {
            tableBody.innerHTML = '<tr><td colspan="7">Không thể tải dữ liệu bệnh.</td></tr>';
        }
    });
}
