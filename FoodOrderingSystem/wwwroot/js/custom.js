window.onload = function() {
    var page = window.location.pathname.split("/").pop();

    //===== All Pages =====*/
    if (true) {
        // Format Money String Script
        var moneys = document.getElementsByClassName("money");
        for (let i = 0; i < moneys.length; i++) {
            moneys[i].innerHTML = Number(moneys[i].innerHTML).toLocaleString('en');
        }
    }

    //===== Profile Pages =====*/
    if (page.includes("profile")) {
        // Autofill Profile Info Script 
        document.getElementById("profile-name").value = "hihihaha";
        document.getElementById("profile-phone").value = "hihihehe";
        document.getElementById("profile-address").value = "hihihuhu";
    }
}