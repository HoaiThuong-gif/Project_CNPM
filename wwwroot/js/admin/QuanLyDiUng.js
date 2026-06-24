const safetyRoot = document.querySelector(".main");

if (safetyRoot) {
    const apiRoot = safetyRoot.dataset.safetyApi;
    const medicineApi = safetyRoot.dataset.medicineApi;
    const loginUrl = safetyRoot.dataset.loginUrl;
    const token = localStorage.getItem("token");
    const allergyTable = document.getElementById("allergyTable");
    const backgroundDiseaseTable = document.getElementById("backgroundDiseaseTable");
    const interactionTable = document.getElementById("interactionTable");
    const modal = document.getElementById("safetyModalBackdrop");
    const modalTitle = document.getElementById("safetyModalTitle");
    const form = document.getElementById("safetyForm");
    const modeInput = document.getElementById("safetyMode");
    const entityIdInput = document.getElementById("entityId");
    const entityNameInput = document.getElementById("entityName");
    const entityIsActiveInput = document.getElementById("entityIsActive");
    const singleNameField = document.getElementById("singleNameField");
    const interactionFields = document.getElementById("interactionFields");
    const activeField = document.getElementById("activeField");
    const medicine1Id = document.getElementById("medicine1Id");
    const medicine2Id = document.getElementById("medicine2Id");
    const severityLevel = document.getElementById("severityLevel");
    const interactionDescription = document.getElementById("interactionDescription");
    const closeModalButton = document.getElementById("btnCloseSafetyModal");
    const cancelModalButton = document.getElementById("btnCancelSafetyModal");
    const addAllergyButton = document.getElementById("btnAddAllergy");
    const addBackgroundDiseaseButton = document.getElementById("btnAddBackgroundDisease");
    const addInteractionButton = document.getElementById("btnAddInteraction");

    let allergies = [];
    let backgroundDiseases = [];
    let interactions = [];
    let medicines = [];

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

    const openModal = (mode, data = null) => {
        modeInput.value = mode;
        entityIdInput.value = "";
        entityNameInput.value = "";
        entityIsActiveInput.checked = true;
        medicine1Id.value = "";
        medicine2Id.value = "";
        severityLevel.value = "1";
        interactionDescription.value = "";

        singleNameField.classList.add("hidden");
        interactionFields.classList.add("hidden");
        activeField.classList.add("hidden");

        if (mode === "allergy") {
            modalTitle.textContent = data ? "Cập nhật dị ứng" : "Thêm dị ứng";
            singleNameField.classList.remove("hidden");
            activeField.classList.remove("hidden");
            entityIdInput.value = data?.allergyId || "";
            entityNameInput.value = data?.allergyName || "";
            entityIsActiveInput.checked = data?.isActive ?? true;
        }

        if (mode === "background-disease") {
            modalTitle.textContent = data ? "Cập nhật bệnh nền" : "Thêm bệnh nền";
            singleNameField.classList.remove("hidden");
            activeField.classList.remove("hidden");
            entityIdInput.value = data?.backgroundDiseaseId || "";
            entityNameInput.value = data?.diseaseName || "";
            entityIsActiveInput.checked = data?.isActive ?? true;
        }

        if (mode === "interaction") {
            modalTitle.textContent = "Thêm tương tác thuốc";
            interactionFields.classList.remove("hidden");
        }

        modal.classList.remove("hidden");
    };

    const closeModal = () => {
        modal.classList.add("hidden");
        form.reset();
    };

    const fillMedicineOptions = () => {
        const options = ['<option value="">Chọn thuốc</option>']
            .concat(medicines.map((medicine) => `<option value="${medicine.medicineId}">${medicine.medicineName}</option>`));

        medicine1Id.innerHTML = options.join("");
        medicine2Id.innerHTML = options.join("");
    };

    const renderAllergies = () => {
        if (!allergies.length) {
            allergyTable.innerHTML = '<tr><td colspan="4">Không có dữ liệu dị ứng.</td></tr>';
            return;
        }

        allergyTable.innerHTML = allergies.map((item) => `
            <tr>
                <td>#DU${String(item.allergyId).padStart(3, "0")}</td>
                <td>${item.allergyName}</td>
                <td>${item.isActive ? "Hoạt động" : "Ngừng hoạt động"}</td>
                <td class="action-group">
                    <button class="edit-btn" type="button" data-type="allergy" data-action="edit" data-id="${item.allergyId}">Sửa</button>
                    <button class="delete-btn" type="button" data-type="allergy" data-action="toggle" data-id="${item.allergyId}" data-active="${item.isActive}">
                        ${item.isActive ? "Tắt" : "Bật"}
                    </button>
                </td>
            </tr>
        `).join("");
    };

    const renderBackgroundDiseases = () => {
        if (!backgroundDiseases.length) {
            backgroundDiseaseTable.innerHTML = '<tr><td colspan="4">Không có dữ liệu bệnh nền.</td></tr>';
            return;
        }

        backgroundDiseaseTable.innerHTML = backgroundDiseases.map((item) => `
            <tr>
                <td>#BN${String(item.backgroundDiseaseId).padStart(3, "0")}</td>
                <td>${item.diseaseName}</td>
                <td>${item.isActive ? "Hoạt động" : "Ngừng hoạt động"}</td>
                <td class="action-group">
                    <button class="edit-btn" type="button" data-type="background-disease" data-action="edit" data-id="${item.backgroundDiseaseId}">Sửa</button>
                    <button class="delete-btn" type="button" data-type="background-disease" data-action="toggle" data-id="${item.backgroundDiseaseId}" data-active="${item.isActive}">
                        ${item.isActive ? "Tắt" : "Bật"}
                    </button>
                </td>
            </tr>
        `).join("");
    };

    const levelLabel = (value) => {
        if (value === 3) return "Nghiêm trọng";
        if (value === 2) return "Trung bình";
        return "Nhẹ";
    };

    const renderInteractions = () => {
        if (!interactions.length) {
            interactionTable.innerHTML = '<tr><td colspan="5">Không có dữ liệu tương tác thuốc.</td></tr>';
            return;
        }

        interactionTable.innerHTML = interactions.map((item) => `
            <tr>
                <td>${item.medicine1Name}</td>
                <td>${item.medicine2Name}</td>
                <td>${levelLabel(item.severityLevel)}</td>
                <td>${item.description || ""}</td>
                <td class="action-group">
                    <button class="delete-btn" type="button" data-type="interaction" data-action="delete" data-id1="${item.medicine1Id}" data-id2="${item.medicine2Id}">
                        Xóa
                    </button>
                </td>
            </tr>
        `).join("");
    };

    const loadData = async () => {
        const [allergyResponse, backgroundResponse, interactionResponse, medicineResponse] = await Promise.all([
            request(`${apiRoot}/allergies`),
            request(`${apiRoot}/background-diseases`),
            request(`${apiRoot}/drug-interactions`),
            request(medicineApi)
        ]);

        allergies = await allergyResponse.json();
        backgroundDiseases = await backgroundResponse.json();
        interactions = await interactionResponse.json();
        medicines = await medicineResponse.json();

        fillMedicineOptions();
        renderAllergies();
        renderBackgroundDiseases();
        renderInteractions();
    };

    addAllergyButton.addEventListener("click", () => openModal("allergy"));
    addBackgroundDiseaseButton.addEventListener("click", () => openModal("background-disease"));
    addInteractionButton.addEventListener("click", () => openModal("interaction"));
    closeModalButton.addEventListener("click", closeModal);
    cancelModalButton.addEventListener("click", closeModal);
    modal.addEventListener("click", (event) => {
        if (event.target === modal) closeModal();
    });

    form.addEventListener("submit", async (event) => {
        event.preventDefault();
        const mode = modeInput.value;

        try {
            if (mode === "allergy") {
                const payload = {
                    allergyId: Number(entityIdInput.value || 0),
                    allergyName: entityNameInput.value.trim(),
                    isActive: entityIsActiveInput.checked
                };

                const response = await request(payload.allergyId ? `${apiRoot}/allergies/update` : `${apiRoot}/allergies/create`, {
                    method: payload.allergyId ? "PATCH" : "POST",
                    body: JSON.stringify(payload)
                });

                const result = await response.json();
                if (!response.ok) throw new Error(result.message || "Không thể lưu dị ứng.");
                alert(result.message || "Lưu dị ứng thành công.");
            }

            if (mode === "background-disease") {
                const payload = {
                    backgroundDiseaseId: Number(entityIdInput.value || 0),
                    diseaseName: entityNameInput.value.trim(),
                    isActive: entityIsActiveInput.checked
                };

                const response = await request(payload.backgroundDiseaseId ? `${apiRoot}/background-diseases/update` : `${apiRoot}/background-diseases/create`, {
                    method: payload.backgroundDiseaseId ? "PATCH" : "POST",
                    body: JSON.stringify(payload)
                });

                const result = await response.json();
                if (!response.ok) throw new Error(result.message || "Không thể lưu bệnh nền.");
                alert(result.message || "Lưu bệnh nền thành công.");
            }

            if (mode === "interaction") {
                const payload = {
                    medicine1Id: Number(medicine1Id.value),
                    medicine2Id: Number(medicine2Id.value),
                    severityLevel: Number(severityLevel.value),
                    description: interactionDescription.value.trim()
                };

                const response = await request(`${apiRoot}/drug-interactions/create`, {
                    method: "POST",
                    body: JSON.stringify(payload)
                });

                const result = await response.json();
                if (!response.ok) throw new Error(result.message || "Không thể lưu tương tác thuốc.");
                alert(result.message || "Lưu tương tác thuốc thành công.");
            }

            closeModal();
            await loadData();
        } catch (error) {
            if (error.message !== "Unauthorized") alert(error.message);
        }
    });

    const handleTableClick = async (event) => {
        const button = event.target.closest("button");
        if (!button) return;

        const type = button.dataset.type;
        const action = button.dataset.action;

        try {
            if (type === "allergy" && action === "edit") {
                const item = allergies.find((x) => x.allergyId === Number(button.dataset.id));
                if (item) openModal("allergy", item);
                return;
            }

            if (type === "allergy" && action === "toggle") {
                const id = Number(button.dataset.id);
                const nextActive = button.dataset.active !== "true";
                const response = await request(`${apiRoot}/allergies/toggle?id=${id}&isActive=${nextActive.toString().toLowerCase()}`, { method: "PATCH" });
                const result = await response.json();
                if (!response.ok) throw new Error(result.message || "Không thể đổi trạng thái dị ứng.");
                alert(result.message || "Đổi trạng thái dị ứng thành công.");
            }

            if (type === "background-disease" && action === "edit") {
                const item = backgroundDiseases.find((x) => x.backgroundDiseaseId === Number(button.dataset.id));
                if (item) openModal("background-disease", item);
                return;
            }

            if (type === "background-disease" && action === "toggle") {
                const id = Number(button.dataset.id);
                const nextActive = button.dataset.active !== "true";
                const response = await request(`${apiRoot}/background-diseases/toggle?id=${id}&isActive=${nextActive.toString().toLowerCase()}`, { method: "PATCH" });
                const result = await response.json();
                if (!response.ok) throw new Error(result.message || "Không thể đổi trạng thái bệnh nền.");
                alert(result.message || "Đổi trạng thái bệnh nền thành công.");
            }

            if (type === "interaction" && action === "delete") {
                if (!window.confirm("Bạn có chắc muốn xóa tương tác thuốc này?")) return;
                const response = await request(`${apiRoot}/drug-interactions/${button.dataset.id1}/${button.dataset.id2}`, { method: "DELETE" });
                const result = await response.json();
                if (!response.ok) throw new Error(result.message || "Không thể xóa tương tác thuốc.");
                alert(result.message || "Xóa tương tác thuốc thành công.");
            }

            await loadData();
        } catch (error) {
            if (error.message !== "Unauthorized") alert(error.message);
        }
    };

    allergyTable.addEventListener("click", handleTableClick);
    backgroundDiseaseTable.addEventListener("click", handleTableClick);
    interactionTable.addEventListener("click", handleTableClick);

    loadData().catch((error) => {
        if (error.message !== "Unauthorized") {
            console.error(error);
        }
    });
}
