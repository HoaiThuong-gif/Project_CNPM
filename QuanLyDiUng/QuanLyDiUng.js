const searchInput = document.getElementById("searchInput");

searchInput.addEventListener("keyup", function(){

    const filter = searchInput.value.toLowerCase();

    const rows = document.querySelectorAll("#diUngTable tr");

    rows.forEach(function(row){

        const text = row.innerText.toLowerCase();

        if(text.includes(filter)){
            row.style.display = "";
        }
        else{
            row.style.display = "none";
        }

    });

});