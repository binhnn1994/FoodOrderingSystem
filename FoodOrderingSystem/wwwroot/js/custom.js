var page, user;

window.onload = function() {
    var userElement = document.getElementById("nav-user");
    if (userElement) {
        userElement.setAttribute("name", "e5f5ef3699a1");
        getUserInfo();
    }

    page = window.location.pathname.split("/").slice(-2).join("/");
    path = window.location.pathname;
    console.log(path);
    console.log(page);

    setTimeout(function() { initPages() }, 800);
}

function initPages() {
    //===== All Pages =====*/
    if (page.length >= 2) {
        var currentUser = document.getElementById("nav-user");
        if (currentUser) {
            if (user.fullname.length > 0) {
                currentUser.innerHTML = user.fullname;
            } else {
                currentUser.innerHTML = user.userEmail;
            }
        }
    }

    //===== Profile Pages =====*/
    if (path.includes("Profile") || path.includes("StaffDashboard")) {
        setProfileInfo();
        if (page === "AdminDashboard/Profile") {
            getTime();

            $('#date-from').change(function() {
                var date = $(this).val();
            });
            $('#date-to').change(function() {
                var date = $(this).val();
            });
        }
    }

    //===== Staff Management Page =====*/
    if (path.includes("AdminDashboard/StaffManagement")) {
        showStaffsList();
    }

    //===== Customer Management Page =====*/
    if (path.includes("AdminDashboard/CustomerManagement")) {
        showCustomerList();
    }

    //===== Food Management Page =====*/
    if (path.includes("AdminDashboard/Index") || page === "AdminDashboard/" || page === "/AdminDashboard") {
        loadCategories();
        showItemList();
    }

    //===== Order Page =====*/
    if (path.includes("CustomerDashboard/Index") || page === "CustomerDashboard/" || page === "/CustomerDashboard" || page === "/") {
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

    //===== Staff Page =====*/
    if (page.includes("StaffDashboard")) {
        showOrderList();
        showReviewList();

        //===== Accept Order Script =====//
        $('.accept-order').on('click', function() {
            var orderID = $(this).parent().find('span').text();
            acceptOrder(orderID);
            return false;
        });

        //===== Decline Order Script =====//
        $('.decline-order').on('click', function() {
            var orderID = $(this).parent().find('span').text();
            declineOrder(orderID);
            return false;
        });
    }

    //===== Customer Profile Page =====*/
    if (path.includes("CustomerDashboard/Profile")) {
        showOrderList();
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

function getUserInfo() {
    var request = new XMLHttpRequest();
    var userID = document.getElementById("nav-user").getAttribute("name");
    var url = "/api/AdminDashboard/ViewAccountDetail";
    var content = '{"UserID": "' + userID + '"}';

    request.open('POST', url, true);
    request.setRequestHeader("Content-Type", "text/json");
    request.onload = function() {
        user = JSON.parse(this.responseText);
    }
    request.send(content);
}

function format(time) {
    return (time < 10 ? '0' : '') + time;
}

function getTime() {
    var now = new Date();

    var year = now.getFullYear();
    var month = format(now.getMonth() + 1);
    var day = format(now.getDate());
    var preMonth;

    if (now.getMonth() === 0) {
        preMonth = 12;
        year = year - 1;
    } else {
        preMonth = format(now.getMonth());
    }

    var dateTo = document.getElementById("date-to");
    dateTo.value = [year, month, day].join('-');
    dateTo.max = dateTo.value;

    var dateFrom = document.getElementById("date-from");
    dateFrom.value = [year, preMonth, day].join('-');
    dateFrom.max = dateTo.value;
}

function setProfileInfo() {
    document.getElementById("info-name").innerHTML = user.fullname;
    document.getElementById("info-email").innerHTML = user.userEmail;
    document.getElementById("profile-name").value = user.fullname;
    document.getElementById("profile-phone").value = user.phoneNumber;
    document.getElementById("profile-address").value = user.address;
}

function formatDate(original) {
    var normalizedDate = "";
    normalizedDate += original.substring(8, 10) + '/';
    normalizedDate += original.substring(5, 7) + '/';
    normalizedDate += original.substring(0, 4) + ' ';
    normalizedDate += original.substring(11, 16);
    return normalizedDate;
}