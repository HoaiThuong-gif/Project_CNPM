const form = document.querySelector(".medical-form");

if (form) {
    const diseaseSelect = document.getElementById("diseaseId");
    const symptomsInput = document.getElementById("symptoms");
    const symptomSeverityInput = document.getElementById("symptomSeverity");
    const noticeWrapper = document.getElementById("predictionNoticeWrapper");
    const diseasesUrl = form.dataset.diseasesUrl;
    const predictUrl = form.dataset.predictUrl;
    const resultUrl = form.dataset.resultUrl;
    const loginUrl = form.dataset.loginUrl;
    const submitButton = form.querySelector(".btn-submit");
    const allergyInput = document.getElementById("allergyInput");
    const allergyList = document.getElementById("allergyList");
    const addAllergyButton = document.getElementById("btnAddAllergy");
    const patientAgeInput = document.getElementById("patientAge");
    const patientGenderInput = document.getElementById("patientGender");
    const backgroundDiseaseInput = document.getElementById("backgroundDiseaseInput");
    const backgroundDiseaseList = document.getElementById("backgroundDiseaseList");
    const addBackgroundDiseaseButton = document.getElementById("btnAddBackgroundDisease");
    const currentMedicineInput = document.getElementById("currentMedicineInput");
    const currentMedicineList = document.getElementById("currentMedicineList");
    const addCurrentMedicineButton = document.getElementById("btnAddCurrentMedicine");

    let allergies = [];
    let backgroundDiseases = [];
    let currentMedicines = [];

    const showNotice = (message, color = "#1d4ed8", background = "#eff6ff") => {
        if (!noticeWrapper) {
            return;
        }

        noticeWrapper.innerHTML = `
            <div style="padding:12px 16px;border-radius:14px;background:${background};color:${color};font-weight:600;">
                ${message}
            </div>
        `;
    };

    const redirectToLogin = () => {
        const returnUrl = `${window.location.pathname}${window.location.search}`;
        window.location.href = `${loginUrl}?returnUrl=${encodeURIComponent(returnUrl)}`;
    };

    const renderTags = (items, container, type) => {
        if (!container) {
            return;
        }

        container.innerHTML = items.map((item, index) => `
            <span class="user-chip">
                ${item}
                <button type="button" data-type="${type}" data-index="${index}" aria-label="Xóa">×</button>
            </span>
        `).join("");
    };

    const renderAll = () => {
        renderTags(allergies, allergyList, "allergy");
        renderTags(backgroundDiseases, backgroundDiseaseList, "background");
        renderTags(currentMedicines, currentMedicineList, "medicine");
    };

    const getCollectionByType = (type) => {
        if (type === "allergy") {
            return allergies;
        }

        if (type === "background") {
            return backgroundDiseases;
        }

        return currentMedicines;
    };

    const addItem = (input, type) => {
        const value = input?.value.trim();
        if (!value) {
            return;
        }

        const collection = getCollectionByType(type);
        const normalizedValue = value.toLowerCase();

        if (collection.some((item) => item.toLowerCase() === normalizedValue)) {
            input.value = "";
            return;
        }

        collection.push(value);
        input.value = "";
        renderAll();
    };

    const loadDiseases = async () => {
        if (!diseasesUrl || !diseaseSelect) {
            return;
        }

        diseaseSelect.innerHTML = '<option value="">Chọn bệnh</option>';
        showNotice("Đang tải danh sách bệnh...", "#1d4ed8", "#eff6ff");

        try {
            const response = await fetch(diseasesUrl, {
                headers: {
                    Accept: "application/json"
                }
            });

            if (response.status === 401) {
                redirectToLogin();
                return;
            }

            if (!response.ok) {
                throw new Error("Không thể tải danh sách bệnh.");
            }

            const diseases = await response.json();
            (Array.isArray(diseases) ? diseases : [])
                .filter((disease) => disease.isActive !== false)
                .forEach((disease) => {
                    const option = document.createElement("option");
                    option.value = String(disease.diseaseId ?? disease.id);
                    option.textContent = disease.diseaseName ?? disease.name;
                    diseaseSelect.appendChild(option);
                });

            noticeWrapper.innerHTML = "";
        } catch (error) {
            diseaseSelect.innerHTML = '<option value="">Không tải được danh sách bệnh</option>';
            showNotice(error.message || "Không thể tải danh sách bệnh.", "#b91c1c", "#fef2f2");
        }
    };

    addAllergyButton?.addEventListener("click", () => addItem(allergyInput, "allergy"));
    addBackgroundDiseaseButton?.addEventListener("click", () => addItem(backgroundDiseaseInput, "background"));
    addCurrentMedicineButton?.addEventListener("click", () => addItem(currentMedicineInput, "medicine"));

    [allergyInput, backgroundDiseaseInput, currentMedicineInput].forEach((input) => {
        input?.addEventListener("keydown", (event) => {
            if (event.key !== "Enter") {
                return;
            }

            event.preventDefault();

            if (input === allergyInput) {
                addItem(allergyInput, "allergy");
            } else if (input === backgroundDiseaseInput) {
                addItem(backgroundDiseaseInput, "background");
            } else if (input === currentMedicineInput) {
                addItem(currentMedicineInput, "medicine");
            }
        });
    });

    [allergyList, backgroundDiseaseList, currentMedicineList].forEach((container) => {
        container?.addEventListener("click", (event) => {
            const button = event.target.closest("button");
            if (!button) {
                return;
            }

            const index = Number(button.dataset.index);
            const type = button.dataset.type;

            if (type === "allergy") {
                allergies = allergies.filter((_, itemIndex) => itemIndex !== index);
            } else if (type === "background") {
                backgroundDiseases = backgroundDiseases.filter((_, itemIndex) => itemIndex !== index);
            } else if (type === "medicine") {
                currentMedicines = currentMedicines.filter((_, itemIndex) => itemIndex !== index);
            }

            renderAll();
        });
    });

    form.addEventListener("submit", async (event) => {
        event.preventDefault();

        if (form.dataset.busy === "true") {
            return;
        }

        const token = localStorage.getItem("token");
        if (!token) {
            redirectToLogin();
            return;
        }

        const diseaseId = Number(diseaseSelect.value);
        const symptoms = symptomsInput.value.trim();
        const severity = symptomSeverityInput.value;

        if (!diseaseId || !symptoms || !severity) {
            alert("Vui lòng chọn bệnh, mức độ triệu chứng và nhập triệu chứng.");
            return;
        }

        form.dataset.busy = "true";
        submitButton.disabled = true;
        submitButton.textContent = "Đang dự đoán...";
        showNotice("Đang gửi dữ liệu lên hệ thống để phân tích...", "#1d4ed8", "#eff6ff");

        try {
            const response = await fetch(predictUrl, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${token}`
                },
                body: JSON.stringify({
                    diseaseId,
                    symptoms,
                    severity,
                    age: patientAgeInput?.value.trim() ? Number(patientAgeInput.value.trim()) : null,
                    gender: patientGenderInput?.value || null,
                    allergies,
                    backgroundDiseases,
                    currentMedicines
                })
            });

            if (response.status === 401) {
                showNotice("Phiên đăng nhập đã hết hạn. Đang chuyển về trang đăng nhập...", "#b45309", "#fff7ed");
                window.setTimeout(redirectToLogin, 800);
                return;
            }

            const result = await response.json();
            if (!response.ok) {
                throw new Error(result.message || "Dự đoán thất bại.");
            }

            const selectedDiseaseName = diseaseSelect.options[diseaseSelect.selectedIndex]?.text || "";
            sessionStorage.setItem("predictionInput", JSON.stringify({
                diseaseId,
                diseaseName: selectedDiseaseName,
                symptoms,
                severity,
                age: patientAgeInput?.value.trim() || "",
                gender: patientGenderInput?.value || "",
                allergies,
                backgroundDiseases,
                currentMedicines
            }));
            sessionStorage.setItem("predictionResults", JSON.stringify(result.data || []));
            window.location.href = resultUrl;
        } catch (error) {
            showNotice(error.message || "Dự đoán thất bại.", "#b91c1c", "#fef2f2");
        } finally {
            form.dataset.busy = "false";
            submitButton.disabled = false;
            submitButton.textContent = "Dự đoán";
        }
    });

    renderAll();
    loadDiseases();
}
