window.onload = function() {
    var page = window.location.pathname.split("/").pop();

    //===== All Pages =====*/
    if (page.includes("")) {
        formatMoneyString();
    }

    //===== Profile Pages =====*/
    if (page.includes("profile")) {
        setProfileInfo();
        document.getElementById("profile-name").value = "hihihaha";
        document.getElementById("profile-phone").value = "hihihehe";
        document.getElementById("profile-address").value = "hihihuhu";
    }

    //===== Staff Management Page =====*/
    if (page === "StaffManagement") {
        showStaffsList();
    }
}

function formatMoneyString() {
    var moneys = document.getElementsByClassName("money");
    for (let i = 0; i < moneys.length; i++) {
        moneys[i].innerHTML = Number(moneys[i].innerHTML).toLocaleString('en');
    }
}