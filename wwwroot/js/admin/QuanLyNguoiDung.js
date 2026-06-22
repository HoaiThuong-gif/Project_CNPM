const userRoot = document.querySelector(".main");

if (userRoot) {
    const apiUrl = userRoot.dataset.usersApi;
    const loginUrl = userRoot.dataset.loginUrl;
    const token = localStorage.getItem("token");
    const tableBody = document.getElementById("userTable");
    const searchInput = document.getElementById("searchInput");
    let users = [];

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

    const roleLabel = (role) => role === "Admin" ? "Admin" : "Bệnh nhân";

    const statusLabel = (user) => {
        if (user.isDeleted) return "Đã xóa";
        if (user.isLocked) return "Tạm khóa";
        return "Hoạt động";
    };

    const statusClass = (user) => {
        if (user.isDeleted || user.isLocked) return "inactive-status";
        return "active-status";
    };

    const renderUsers = (source) => {
        if (!source.length) {
            tableBody.innerHTML = '<tr><td colspan="7">Không có dữ liệu người dùng.</td></tr>';
            return;
        }

        tableBody.innerHTML = source.map((user) => `
            <tr>
                <td>#ND${String(user.userId).padStart(3, "0")}</td>
                <td>${user.fullName || ""}</td>
                <td>${user.email || ""}</td>
                <td><span class="role ${user.role === "Admin" ? "admin" : "patient"}">${roleLabel(user.role)}</span></td>
                <td><span class="status ${statusClass(user)}">${statusLabel(user)}</span></td>
                <td>${formatDate(user.createdAt)}</td>
                <td class="action-group">
                    <button class="edit-btn" type="button" data-action="lock" data-id="${user.userId}" data-locked="${user.isLocked}">
                        ${user.isLocked ? "Mở khóa" : "Khóa"}
                    </button>
                    <button class="delete-btn" type="button" data-action="delete" data-id="${user.userId}" ${user.isDeleted ? "disabled" : ""}>
                        Xóa
                    </button>
                </td>
            </tr>
        `).join("");
    };

    const loadUsers = async () => {
        const response = await request(apiUrl, { method: "GET" });
        const data = await response.json();
        users = Array.isArray(data) ? data : [];
        renderUsers(users);
    };

    tableBody.addEventListener("click", async (event) => {
        const button = event.target.closest("button");
        if (!button) return;

        const userId = button.dataset.id;
        const action = button.dataset.action;

        try {
            if (action === "lock") {
                const currentLocked = button.dataset.locked === "true";
                const response = await request(`${apiUrl}/${userId}/lock?isLocked=${(!currentLocked).toString().toLowerCase()}`, {
                    method: "PATCH"
                });

                const result = await response.json();
                if (!response.ok) {
                    throw new Error(result.message || "Không thể cập nhật trạng thái khóa.");
                }

                alert(result.message || "Cập nhật trạng thái thành công.");
            }

            if (action === "delete") {
                if (!window.confirm("Bạn có chắc muốn xóa mềm người dùng này?")) return;

                const response = await request(`${apiUrl}/${userId}`, { method: "DELETE" });
                const result = await response.json();
                if (!response.ok) {
                    throw new Error(result.message || "Không thể xóa người dùng.");
                }

                alert(result.message || "Xóa người dùng thành công.");
            }

            await loadUsers();
        } catch (error) {
            if (error.message !== "Unauthorized") {
                alert(error.message);
            }
        }
    });

    searchInput.addEventListener("keyup", () => {
        const filter = searchInput.value.trim().toLowerCase();
        const filtered = users.filter((user) => {
            return [
                user.fullName,
                user.email,
                user.role,
                statusLabel(user)
            ].join(" ").toLowerCase().includes(filter);
        });

        renderUsers(filtered);
    });

    loadUsers().catch((error) => {
        if (error.message !== "Unauthorized") {
            tableBody.innerHTML = '<tr><td colspan="7">Không thể tải dữ liệu người dùng.</td></tr>';
        }
    });
}
