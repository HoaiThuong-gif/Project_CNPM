const form = document.querySelector(".medical-form");

form.addEventListener("submit", function(e){

    e.preventDefault();

    alert("Đã gửi thông tin bệnh thành công!");
});