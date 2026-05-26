// LichSuTraCuu.js

const historyCards = document.querySelectorAll(".history-card");

historyCards.forEach(card => {

    card.addEventListener("click", () => {

        card.style.transform = "scale(0.98)";

        setTimeout(() => {

            card.style.transform = "translateY(-6px)";

        }, 150);

    });

});