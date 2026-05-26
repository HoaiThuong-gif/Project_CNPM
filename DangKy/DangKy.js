console.log("Trang Đăng Ký Loaded");

const form = document.querySelector(".register-form");

form.addEventListener("submit", function(event){

    event.preventDefault();

    alert("Đăng ký thành công!");

});