// OPEN MODAL

function openModal(modalId){

    const modal = document.getElementById(modalId);

    modal.style.display = "flex";

}

// CLOSE MODAL

function closeModal(modalId){

    const modal = document.getElementById(modalId);

    modal.style.display = "none";

}

// CLOSE WHEN CLICK OUTSIDE

window.addEventListener("click", (event) => {

    const modals = document.querySelectorAll(".modal");

    modals.forEach(modal => {

        if(event.target === modal){

            modal.style.display = "none";

        }

    });

});