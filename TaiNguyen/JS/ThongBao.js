// HIỂN THỊ THÔNG BÁO

function showNotification(message, type){

    const notification = document.createElement("div");

    notification.classList.add("notification");

    notification.innerText = message;

    // TYPE

    if(type === "success"){

        notification.style.background = "#38bdf8";

    }
    else if(type === "error"){

        notification.style.background = "#ef4444";

    }
    else{

        notification.style.background = "#64748b";

    }

    // STYLE

    notification.style.position = "fixed";

    notification.style.top = "30px";

    notification.style.right = "30px";

    notification.style.padding = "15px 25px";

    notification.style.color = "white";

    notification.style.borderRadius = "14px";

    notification.style.fontWeight = "bold";

    notification.style.zIndex = "9999";

    notification.style.boxShadow = "0 10px 30px rgba(0,0,0,0.15)";

    document.body.appendChild(notification);

    // REMOVE

    setTimeout(() => {

        notification.remove();

    }, 3000);

}