// MENU ADMIN

const menuButton = document.querySelector(".menu-button");

const sidebar = document.querySelector(".sidebar");

if(menuButton){

    menuButton.addEventListener("click", () => {

        sidebar.classList.toggle("show-sidebar");

    });

}

// ACTIVE MENU

const menuItems = document.querySelectorAll(".sidebar a");

menuItems.forEach(item => {

    item.addEventListener("click", () => {

        menuItems.forEach(link => {

            link.classList.remove("active");

        });

        item.classList.add("active");

    });

});