// KIỂM TRA EMAIL

function kiemTraEmail(email){

    const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    return regex.test(email);

}

// KIỂM TRA SỐ ĐIỆN THOẠI

function kiemTraSoDienThoai(sdt){

    const regex = /^[0-9]{10}$/;

    return regex.test(sdt);

}

// KIỂM TRA PASSWORD

function kiemTraMatKhau(password){

    return password.length >= 6;

}

// VALIDATE FORM

function validateForm(formId){

    const form = document.getElementById(formId);

    const inputs = form.querySelectorAll("input");

    let isValid = true;

    inputs.forEach(input => {

        if(input.value.trim() === ""){

            input.style.borderColor = "red";

            isValid = false;
        }
        else{

            input.style.borderColor = "#cbd5e1";

        }

    });

    return isValid;
}