const loginForm = document.querySelector(".login-form");

const parseJwtPayload = (token) => {
    try {
        const payload = token.split(".")[1];
        const base64 = payload.replace(/-/g, "+").replace(/_/g, "/");
        return JSON.parse(decodeURIComponent(atob(base64).split("").map((char) => {
            return `%${(`00${char.charCodeAt(0).toString(16)}`).slice(-2)}`;
        }).join("")));
    } catch {
        return null;
    }
};

if (loginForm) {
    loginForm.addEventListener("submit", async (event) => {
        event.preventDefault();

        const emailInput = document.getElementById("email");
        const passwordInput = document.getElementById("password");
        const loginApi = loginForm.dataset.loginApi;
        const returnUrl = loginForm.dataset.returnUrl || "";
        const homeUrl = loginForm.dataset.homeUrl;
        const adminUrl = loginForm.dataset.adminUrl;
        const loginButton = loginForm.querySelector(".btn-login");

        if (!emailInput.value.trim() || !passwordInput.value.trim()) {
            alert("Vui lòng nhập email và mật khẩu.");
            return;
        }

        loginButton.disabled = true;
        loginButton.textContent = "Đang đăng nhập...";

        try {
            const response = await fetch(loginApi, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    email: emailInput.value.trim(),
                    password: passwordInput.value
                })
            });

            const result = await response.json();
            if (!response.ok) {
                throw new Error(result.message || "Đăng nhập thất bại.");
            }

            const token = result.token;
            const payload = parseJwtPayload(token);
            const roleClaim =
                payload?.["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] ||
                payload?.role ||
                "User";
            const role = String(roleClaim).trim();

            localStorage.setItem("token", token);
            localStorage.setItem("userId", String(result.userId || ""));
            localStorage.setItem("username", result.username || "");
            localStorage.setItem("role", role);

            alert(result.message || "Đăng nhập thành công!");

            if (returnUrl) {
                window.location.href = returnUrl;
                return;
            }

            if (role.toLowerCase() === "admin") {
                window.location.href = adminUrl;
                return;
            }

            window.location.href = homeUrl;
        } catch (error) {
            alert(error.message || "Đăng nhập thất bại.");
        } finally {
            loginButton.disabled = false;
            loginButton.textContent = "Đăng nhập";
        }
    });
}
