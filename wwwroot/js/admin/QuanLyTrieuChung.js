const symptomRoot = document.querySelector(".main");

if (symptomRoot) {
    const apiUrl = symptomRoot.dataset.symptomApi;
    const diseaseApiUrl = symptomRoot.dataset.diseaseApi;
    const loginUrl = symptomRoot.dataset.loginUrl;
    const token = localStorage.getItem("token");
    const tableBody = document.getElementById("symptomTable");
    const searchInput = document.getElementById("searchInput");
    const statusFilter = document.getElementById("statusFilter");
    const createButton = document.getElementById("btnCreateSymptom");
    const modal = document.getElementById("symptomModalBackdrop");
    const modalTitle = document.getElementById("symptomModalTitle");
    const closeModalButton = document.getElementById("btnCloseSymptomModal");
    const cancelModalButton = document.getElementById("btnCancelSymptomModal");
    const form = document.getElementById("symptomForm");
    const symptomIdInput = document.getElementById("symptomId");
    const symptomNameInput = document.getElementById("symptomName");
    const symptomDescriptionInput = document.getElementById("symptomDescription");
    const symptomDiseasesInput = document.getElementById("symptomDiseases");
    let symptoms = [];
    let diseases = [];
    let editingSymptom = null;

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

    const openModal = (symptom) => {
        editingSymptom = symptom || null;
        modalTitle.textContent = symptom ? "Cập nhật triệu chứng" : "Thêm triệu chứng";
        symptomIdInput.value = symptom?.symptomId || "";
        symptomNameInput.value = symptom?.symptomName || "";
        symptomDescriptionInput.value = symptom?.description || "";
        const selectedDiseaseIds = new Set((symptom?.diseaseIds || []).map((id) => Number(id)));
        [...symptomDiseasesInput.options].forEach((option) => {
            option.selected = selectedDiseaseIds.has(Number(option.value));
        });
        modal.classList.remove("hidden");
    };

    const closeModal = () => {
        modal.classList.add("hidden");
        form.reset();
        symptomIdInput.value = "";
        editingSymptom = null;
    };

    const renderSymptoms = (source) => {
        if (!source.length) {
            tableBody.innerHTML = '<tr><td colspan="7">Không có dữ liệu triệu chứng.</td></tr>';
            return;
        }

        tableBody.innerHTML = source.map((symptom) => `
            <tr>
                <td>TC${String(symptom.symptomId).padStart(3, "0")}</td>
                <td>${symptom.symptomName || ""}</td>
                <td>${symptom.description || ""}</td>
                <td>${(symptom.diseases || []).map((disease) => disease.diseaseName).join(", ")}</td>
                <td><span class="status ${symptom.isActive ? "active" : "inactive"}">${symptom.isActive ? "Hoạt động" : "Tạm ẩn"}</span></td>
                <td>${formatDate(symptom.createdAt)}</td>
                <td>
                    <button class="edit-btn" type="button" data-action="edit" data-id="${symptom.symptomId}">Sửa</button>
                    <button class="delete-btn" type="button" data-action="toggle" data-id="${symptom.symptomId}">
                        ${symptom.isActive ? "Tắt" : "Bật"}
                    </button>
                </td>
            </tr>
        `).join("");
    };

    const applyFilters = () => {
        const keyword = searchInput.value.trim().toLowerCase();
        const status = statusFilter.value;

        const filtered = symptoms.filter((symptom) => {
            const matchesKeyword = [
                symptom.symptomName,
                symptom.description,
                ...(symptom.diseases || []).map((disease) => disease.diseaseName)
            ].join(" ").toLowerCase().includes(keyword);

            const matchesStatus = !status || String(symptom.isActive) === status;
            return matchesKeyword && matchesStatus;
        });

        renderSymptoms(filtered);
    };

    const loadSymptoms = async () => {
        const response = await request(apiUrl, { method: "GET" });
        const data = await response.json();
        symptoms = Array.isArray(data) ? data : [];
        applyFilters();
    };

    const loadDiseases = async () => {
        const response = await request(diseaseApiUrl, { method: "GET" });
        const data = await response.json();
        diseases = Array.isArray(data) ? data : [];
        symptomDiseasesInput.innerHTML = diseases
            .filter((disease) => disease.isActive !== false)
            .map((disease) => `<option value="${disease.diseaseId}">${disease.diseaseName}</option>`)
            .join("");
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
            symptomId: Number(symptomIdInput.value || 0),
            symptomName: symptomNameInput.value.trim(),
            description: symptomDescriptionInput.value.trim(),
            diseaseIds: [...symptomDiseasesInput.selectedOptions].map((option) => Number(option.value)),
            isActive: editingSymptom?.isActive ?? true
        };

        if (!payload.symptomName) {
            alert("Vui lòng nhập tên triệu chứng.");
            return;
        }

        try {
            const response = await request(editingSymptom ? `${apiUrl}/update` : `${apiUrl}/create`, {
                method: editingSymptom ? "PATCH" : "POST",
                body: JSON.stringify(payload)
            });

            const result = await response.json();
            if (!response.ok) {
                throw new Error(result.message || "Không thể lưu triệu chứng.");
            }

            alert(result.message || "Lưu triệu chứng thành công.");
            closeModal();
            await loadSymptoms();
        } catch (error) {
            if (error.message !== "Unauthorized") alert(error.message);
        }
    });

    tableBody.addEventListener("click", async (event) => {
        const button = event.target.closest("button");
        if (!button) return;

        const symptomId = Number(button.dataset.id);
        const action = button.dataset.action;

        try {
            if (action === "edit") {
                const existing = symptoms.find((item) => item.symptomId === symptomId);
                if (!existing) return;
                openModal(existing);
                return;
            }

            if (action === "toggle") {
                const response = await request(`${apiUrl}/toggle?id=${symptomId}`, {
                    method: "PATCH"
                });

                const result = await response.json();
                if (!response.ok) {
                    throw new Error(result.message || "Không thể đổi trạng thái triệu chứng.");
                }

                alert(result.message || "Đổi trạng thái triệu chứng thành công.");
                await loadSymptoms();
            }
        } catch (error) {
            if (error.message !== "Unauthorized") alert(error.message);
        }
    });

    searchInput.addEventListener("keyup", applyFilters);
    statusFilter.addEventListener("change", applyFilters);

    Promise.all([loadDiseases(), loadSymptoms()]).catch((error) => {
        if (error.message !== "Unauthorized") {
            tableBody.innerHTML = '<tr><td colspan="7">Không thể tải dữ liệu triệu chứng.</td></tr>';
        }
    });
}
