const medicineRoot = document.querySelector(".main");

if (medicineRoot) {
    const apiUrl = medicineRoot.dataset.medicineApi;
    const loginUrl = medicineRoot.dataset.loginUrl;
    const token = localStorage.getItem("token");
    const tableBody = document.getElementById("medicineTable");
    const searchInput = document.getElementById("searchInput");
    const statusFilter = document.getElementById("statusFilter");
    const createButton = document.getElementById("btnCreateMedicine");
    const modal = document.getElementById("medicineModalBackdrop");
    const modalTitle = document.getElementById("medicineModalTitle");
    const closeModalButton = document.getElementById("btnCloseMedicineModal");
    const cancelModalButton = document.getElementById("btnCancelMedicineModal");
    const form = document.getElementById("medicineForm");
    let medicines = [];
    let editingMedicine = null;

    const fields = {
        medicineId: document.getElementById("medicineId"),
        medicineName: document.getElementById("medicineName"),
        activeIngredient: document.getElementById("activeIngredient"),
        medicineGroup: document.getElementById("medicineGroup"),
        dosageForm: document.getElementById("dosageForm"),
        uses: document.getElementById("uses"),
        dosage: document.getElementById("dosage"),
        howToUse: document.getElementById("howToUse"),
        sideEffects: document.getElementById("sideEffects"),
        notes: document.getElementById("notes"),
        requiresPrescription: document.getElementById("requiresPrescription")
    };

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

    const collectPayload = () => ({
        medicineName: fields.medicineName.value.trim(),
        activeIngredient: fields.activeIngredient.value.trim(),
        medicineGroup: fields.medicineGroup.value.trim(),
        dosageForm: fields.dosageForm.value.trim(),
        uses: fields.uses.value.trim(),
        dosage: fields.dosage.value.trim(),
        howToUse: fields.howToUse.value.trim(),
        sideEffects: fields.sideEffects.value.trim(),
        notes: fields.notes.value.trim(),
        requiresPrescription: fields.requiresPrescription.checked
    });

    const openModal = (medicine) => {
        editingMedicine = medicine || null;
        modalTitle.textContent = medicine ? "Cập nhật thuốc" : "Thêm thuốc";
        fields.medicineId.value = medicine?.medicineId || "";
        fields.medicineName.value = medicine?.medicineName || "";
        fields.activeIngredient.value = medicine?.activeIngredient || "";
        fields.medicineGroup.value = medicine?.medicineGroup || "";
        fields.dosageForm.value = medicine?.dosageForm || "";
        fields.uses.value = medicine?.uses || "";
        fields.dosage.value = medicine?.dosage || "";
        fields.howToUse.value = medicine?.howToUse || "";
        fields.sideEffects.value = medicine?.sideEffects || "";
        fields.notes.value = medicine?.notes || "";
        fields.requiresPrescription.checked = Boolean(medicine?.requiresPrescription);
        modal.classList.remove("hidden");
    };

    const closeModal = () => {
        modal.classList.add("hidden");
        form.reset();
        editingMedicine = null;
    };

    const renderMedicines = (source) => {
        if (!source.length) {
            tableBody.innerHTML = '<tr><td colspan="7">Không có dữ liệu thuốc.</td></tr>';
            return;
        }

        tableBody.innerHTML = source.map((medicine) => `
            <tr>
                <td>TH${String(medicine.medicineId).padStart(3, "0")}</td>
                <td>${medicine.medicineName || ""}</td>
                <td>${medicine.activeIngredient || ""}</td>
                <td>${medicine.medicineGroup || ""}</td>
                <td>${medicine.dosage || ""}</td>
                <td><span class="status ${medicine.isActive ? "active" : "out"}">${medicine.isActive ? "Đang hoạt động" : "Ngừng hoạt động"}</span></td>
                <td>
                    <button class="edit-btn" type="button" data-action="edit" data-id="${medicine.medicineId}">Sửa</button>
                    <button class="delete-btn" type="button" data-action="toggle" data-id="${medicine.medicineId}" data-active="${medicine.isActive}">
                        ${medicine.isActive ? "Tắt" : "Bật"}
                    </button>
                </td>
            </tr>
        `).join("");
    };

    const applyFilters = () => {
        const keyword = searchInput.value.trim().toLowerCase();
        const status = statusFilter.value;

        const filtered = medicines.filter((medicine) => {
            const matchesKeyword = [
                medicine.medicineName,
                medicine.activeIngredient,
                medicine.medicineGroup
            ].join(" ").toLowerCase().includes(keyword);

            const matchesStatus = !status || String(medicine.isActive) === status;
            return matchesKeyword && matchesStatus;
        });

        renderMedicines(filtered);
    };

    const loadMedicines = async () => {
        const response = await request(apiUrl, { method: "GET" });
        const data = await response.json();
        medicines = Array.isArray(data) ? data : [];
        applyFilters();
    };

    createButton.addEventListener("click", () => openModal());
    closeModalButton.addEventListener("click", closeModal);
    cancelModalButton.addEventListener("click", closeModal);
    modal.addEventListener("click", (event) => {
        if (event.target === modal) closeModal();
    });

    form.addEventListener("submit", async (event) => {
        event.preventDefault();
        const payload = collectPayload();

        if (!payload.medicineName) {
            alert("Vui lòng nhập tên thuốc.");
            return;
        }

        try {
            const response = await request(editingMedicine ? `${apiUrl}/update` : `${apiUrl}/create`, {
                method: editingMedicine ? "PATCH" : "POST",
                body: JSON.stringify(editingMedicine ? {
                    medicineId: editingMedicine.medicineId,
                    isActive: editingMedicine.isActive,
                    ...payload
                } : payload)
            });

            const result = await response.json();
            if (!response.ok) {
                throw new Error(result.message || "Không thể lưu thuốc.");
            }

            alert(result.message || "Lưu thuốc thành công.");
            closeModal();
            await loadMedicines();
        } catch (error) {
            if (error.message !== "Unauthorized") alert(error.message);
        }
    });

    tableBody.addEventListener("click", async (event) => {
        const button = event.target.closest("button");
        if (!button) return;

        const medicineId = Number(button.dataset.id);
        const action = button.dataset.action;

        try {
            if (action === "edit") {
                const existing = medicines.find((item) => item.medicineId === medicineId);
                if (!existing) return;
                openModal(existing);
                return;
            }

            if (action === "toggle") {
                const currentActive = button.dataset.active === "true";
                const response = await request(`${apiUrl}/toggle?id=${medicineId}&isActive=${(!currentActive).toString().toLowerCase()}`, {
                    method: "PATCH"
                });

                const result = await response.json();
                if (!response.ok) {
                    throw new Error(result.message || "Không thể đổi trạng thái thuốc.");
                }

                alert(result.message || "Đổi trạng thái thuốc thành công.");
                await loadMedicines();
            }
        } catch (error) {
            if (error.message !== "Unauthorized") alert(error.message);
        }
    });

    searchInput.addEventListener("keyup", applyFilters);
    statusFilter.addEventListener("change", applyFilters);

    loadMedicines().catch((error) => {
        if (error.message !== "Unauthorized") {
            tableBody.innerHTML = '<tr><td colspan="7">Không thể tải dữ liệu thuốc.</td></tr>';
        }
    });
}
