(() => {
    const clearClientAuth = () => {
        localStorage.removeItem("token");
        localStorage.removeItem("userId");
        localStorage.removeItem("username");
        localStorage.removeItem("role");
        sessionStorage.removeItem("predictionInput");
        sessionStorage.removeItem("predictionResults");
        document.cookie = "token=; expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/";
    };

    window.appAuth = {
        clearClientAuth
    };

    document.addEventListener("click", (event) => {
        const logoutLink = event.target.closest("#logoutLink");
        if (!logoutLink) {
            return;
        }

        event.preventDefault();
        clearClientAuth();
        window.location.href = logoutLink.dataset.logoutUrl;
    });
})();
