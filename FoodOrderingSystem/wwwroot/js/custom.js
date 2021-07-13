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

    //===== Customer Management Page =====*/
    if (page === "CustomerManagement") {
        showCustomerList();
    }
}

function formatMoneyString() {
    var moneys = document.getElementsByClassName("money");
    for (let i = 0; i < moneys.length; i++) {
        moneys[i].innerHTML = Number(moneys[i].innerHTML).toLocaleString('en');
    }
}

function activateAccount(userID) {
    var request = new XMLHttpRequest();
    var url = "/api/AdminDashboard/ActiveAccount";
    var content = 'UserID=' + userID;

    request.open('POST', url, true);
    request.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    request.send(content);
}

function deactivateAccount(userID) {
    var request = new XMLHttpRequest();
    var url = "/api/AdminDashboard/InactiveAccount";
    var content = 'UserID=' + userID;
    content += "&Note=Banned";

    request.open('POST', url, true);
    request.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    request.send(content);
}