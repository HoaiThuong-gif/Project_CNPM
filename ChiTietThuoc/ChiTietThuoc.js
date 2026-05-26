console.log("Bệnh Viện Nhóm Bar");

/* BUTTON EFFECT */

const buttons = document.querySelectorAll("button");

buttons.forEach(button => {

    button.addEventListener("mouseenter", () => {

        button.style.transform = "scale(1.05)";

    });

    button.addEventListener("mouseleave", () => {

        button.style.transform = "scale(1)";

    });

});

/* MENU MOBILE */

const menuToggle = document.getElementById("menuToggle");

const menu = document.getElementById("menu");

menuToggle.addEventListener("click", () => {

    menu.classList.toggle("active");

});