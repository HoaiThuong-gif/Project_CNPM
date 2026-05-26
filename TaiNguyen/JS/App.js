// APP START

console.log("Bệnh Viện Nhóm Bar Started");

// LOADING EFFECT

window.addEventListener("load", () => {

    document.body.classList.add("loaded");

});

// SCROLL HEADER EFFECT

const header = document.querySelector(".header");

window.addEventListener("scroll", () => {

    if(window.scrollY > 50){

        header.style.boxShadow = "0 4px 20px rgba(0,0,0,0.08)";

    }
    else{

        header.style.boxShadow = "none";

    }

});