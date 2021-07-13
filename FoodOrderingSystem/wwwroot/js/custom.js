var user;

window.onload = function() {
    var page = window.location.pathname.split("/").slice(-2).join("/");
    getUserByID("e5f5ef3699a1");

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

        //===== Cash Method Popup Script =====//
        $('.cash-popup-btn').on('click', function() {
            $('html').addClass('cash-method-popup-active');

            $('#billing-name').val(user.fullname);
            $('#billing-email').val(user.userEmail);
            $('#billing-phone').val(user.phoneNumber);
            $('#billing-address').val(user.address);

            var subTotal = $('#subtotal-bill').text();
            var deliveryFee;
            var subTotalInt = parseInt(subTotal.replace(/\D/g, ''));
            if (subTotalInt > 500000) {
                deliveryFee = 5000;
            } else if (subTotalInt > 300000) {
                deliveryFee = 10000;
            } else {
                deliveryFee = 20000;
            }
            $('#billing-subtotal').html(subTotal);
            $('#billing-delivery').html(deliveryFee);
            $('#billing-total').html(subTotalInt + deliveryFee);

            formatMoneyString();
        });

        $('.cash-method a.payment-close-btn').on('click', function() {
            $('html').removeClass('cash-method-popup-active');
            return false;
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

function getUserByID(userID) {
    var request = new XMLHttpRequest();
    var url = "/api/AdminDashboard/ViewAccountDetail";
    var content = '{"UserID": "' + userID + '"}';

    request.open('POST', url, true);
    request.setRequestHeader("Content-Type", "text/json");
    request.onload = function() {
        user = JSON.parse(this.responseText);
    }
    request.send(content);
}