const registerForm = document.querySelector(".register-form");

if (registerForm) {
    registerForm.addEventListener("submit", async (event) => {
        event.preventDefault();

        const name = document.getElementById("registerName").value.trim();
        const email = document.getElementById("registerEmail").value.trim();
        const password = document.getElementById("registerPassword").value;
        const confirmPassword = document.getElementById("registerConfirmPassword").value;
        const registerApi = registerForm.dataset.registerApi;
        const loginUrl = registerForm.dataset.loginUrl;
        const submitButton = registerForm.querySelector(".btn-register");

        if (!name || !email || !password || !confirmPassword) {
            alert("Vui lòng nhập đầy đủ thông tin bắt buộc.");
            return;
        }

        if (password !== confirmPassword) {
            alert("Mật khẩu xác nhận không khớp.");
            return;
        }

        submitButton.disabled = true;
        submitButton.textContent = "Đang đăng ký...";

        try {
            const response = await fetch(registerApi, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    name,
                    email,
                    password,
                    confirmPassword
                })
            });

            const result = await response.json();
            if (!response.ok) {
                throw new Error(result.message || "Đăng ký thất bại.");
            }

            alert(result.message || "Đăng ký thành công.");
            window.location.href = loginUrl;
        } catch (error) {
            alert(error.message || "Đăng ký thất bại.");
        } finally {
            submitButton.disabled = false;
            submitButton.textContent = "Tạo tài khoản";
        }
    });
}
