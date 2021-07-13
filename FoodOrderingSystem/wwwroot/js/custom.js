window.onload = function() {
    var page = window.location.pathname.split("/").slice(-2).join("/");

    // console.log(window.location.pathname);
    // console.log(page);

    //===== Profile Pages =====*/
    if (page.includes("Profile")) {
        setProfileInfo();
        document.getElementById("profile-name").value = "hihihaha";
        document.getElementById("profile-phone").value = "hihihehe";
        document.getElementById("profile-address").value = "hihihuhu";
    }

    //===== Staff Management Page =====*/
    if (page === "AdminDashboard/StaffManagement") {
        showStaffsList();
    }

    //===== Customer Management Page =====*/
    if (page === "AdminDashboard/CustomerManagement") {
        showCustomerList();
    }

    //===== Food Management Page =====*/
    if (page === "AdminDashboard/Index" || page === "AdminDashboard/") {
        loadCategories();
        showItemList();
    }

    //===== Order Page =====*/
    if (page === "CustomerDashboard/Index" || page === "/CustomerDashboard" || page === "/") {
        loadCategories();
        showItemList();

        $('.item-remove-btn').prop("onclick", null).off("click");
        $('.item-remove-btn').click(function(event) {
            event.preventDefault();
            var url = $(this).attr('href');
            $(this).closest('li').remove();
            $.get(url, function() {});
            var removePrice = parseInt($(this).parent().find('.price').text().replace(/\D/g, ''));
            subtotalBill = $("#subtotal-bill");
            subtotalBill.html(parseInt(subtotalBill.html().replace(/\D/g, ''), 10) - removePrice);
        });
    }

    //===== All Pages =====*/
    if (page.length >= 0) {
        // formatMoneyString();
    }
}

function formatMoneyString() {
    var moneys = document.getElementsByClassName("money");
    for (let i = 0; i < moneys.length; i++) {
        if (isNaN(moneys[i].innerHTML) === false) {
            moneys[i].innerHTML = Number(moneys[i].innerHTML).toLocaleString('en');
        }
    }
}

function activateAccount(userID) {
    var request = new XMLHttpRequest();
    var url = "/api/AdminDashboard/ActiveAccount";
    var content = 'UserID=' + userID;

    request.open('POST', url, true);
    request.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    request.send(content);
    alert("Enabled account successfully!");
}

function deactivateAccount(userID) {
    var request = new XMLHttpRequest();
    var url = "/api/AdminDashboard/InactiveAccount";
    var content = 'UserID=' + userID;
    content += "&Note=Banned";

    request.open('POST', url, true);
    request.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    request.send(content);
    alert("Disabled account successfully!");
}

function activateItem(itemID) {
    var request = new XMLHttpRequest();
    var url = "/api/AdminDashboard/ActiveItem";
    var content = 'ItemID=' + itemID;

    request.open('POST', url, true);
    request.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    request.send(content);
}

function deactivateItem(itemID) {
    var request = new XMLHttpRequest();
    var url = "/api/AdminDashboard/InactiveItem";
    var content = 'ItemID=' + itemID;
    content += "&Note=" + encodeURIComponent("Suspension Of Business");

    request.open('POST', url, true);
    request.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    request.send(content);
}