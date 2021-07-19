function showOrderList() {
    var request = new XMLHttpRequest();
    var url = "/api/StaffDashboard/GetOrders/Pending";

    request.open('GET', url, true);
    request.onload = function() {
        var result = JSON.parse(this.responseText);
        renderOrderList(result, function() {
            formatMoneyString();
        });
    };
    request.send();
}

function renderOrderList(orderList, callback) {
    var table = document.getElementById("order-table");
    table.innerHTML = "";

    for (let i = 0; i < orderList.length; i++) {
        var row = table.insertRow(-1);

        row.classList.add("order-row");
        row.style.cursor = "pointer";

        var cellNo = row.insertCell(0);
        var cellName = row.insertCell(1);
        var cellDate = row.insertCell(2);
        var cellAddress = row.insertCell(3);
        var cellTotal = row.insertCell(4);
        var cellId = row.insertCell(5);

        var totalInt = orderList[i].customerOrder.total;
        totalInt += calcDeliveryFee(totalInt);

        cellNo.innerHTML = i + 1;
        cellName.innerHTML = orderList[i].account.fullname;
        cellDate.innerHTML = formatDate(orderList[i].customerOrder.orderDate);
        cellAddress.innerHTML = orderList[i].customerOrder.toAddress;
        cellTotal.innerHTML = '<span class="money">' + totalInt + '</span>';
        cellId.innerHTML = orderList[i].customerOrder.orderID;

        cellId.style.display = "none";
    }

    //===== Customer Status Script =====//
    $('.order-row').on('click', function() {
        $('html').addClass('detail-popup-active');

        $('#order-customer-id').html($(this).children("td:nth-child(6)").text());
        getOrderDetail();

        return false;
    });

    callback();
}

function getOrderDetail() {
    var request = new XMLHttpRequest();
    var url = "/api/StaffDashboard/GetOrderDetailsByOrderID";
    var content = '{"OrderID": "' + $('#order-customer-id').html() + '"}';

    request.open('POST', url, true);
    request.setRequestHeader("Content-Type", "text/json");
    request.onload = function() {
        var result = JSON.parse(this.responseText);
        renderOrderDetail(result, function() {
            formatMoneyString();
        });
    }
    request.send(content);
}

function renderOrderDetail(orderDetail, callback) {
    var list = document.getElementById("order-details-list");
    var subtotal = 0;
    var deliveryFee;

    while (list.firstChild) {
        list.removeChild(list.firstChild);
    }

    for (let i = 0; i < orderDetail.length; i++) {
        var li = document.createElement('li');
        var item = document.createElement('div');
        var quantity = document.createElement('span');
        var name = document.createElement('h6');
        var price = document.createElement('span');

        list.appendChild(li);
        li.appendChild(item);
        item.appendChild(name);
        name.appendChild(quantity);
        item.appendChild(price);

        price.classList = "price money";
        item.classList = "dish-name";

        quantity.innerHTML = orderDetail[i].quantity;
        name.innerHTML += " x " + orderDetail[i].item;
        price.innerHTML = orderDetail[i].unitPrice * orderDetail[i].quantity;
        subtotal += orderDetail[i].unitPrice * orderDetail[i].quantity;
    }

    if (subtotal > 500000) {
        deliveryFee = 5000;
    } else if (subtotal > 300000) {
        deliveryFee = 10000;
    } else {
        deliveryFee = 20000;
    }

    document.getElementById("subtotal-order-details").innerHTML = subtotal;
    document.getElementById("delivery-order-details").innerHTML = deliveryFee;
    document.getElementById("total-order-details").innerHTML = subtotal + deliveryFee;

    callback();
}

function showReviewList() {
    document.getElementById("review-list").innerHTML = "";
    showPendingReviewList();
    showClosedReviewList();
}

function showPendingReviewList() {
    var request = new XMLHttpRequest();
    var url = "/api/StaffDashboard/GetFeedbackByStatus";
    var contentPending = '{"Status": "Pending"}';

    request.open('POST', url, true);
    request.setRequestHeader("Content-Type", "text/json");
    request.onload = function() {
        var result = JSON.parse(this.responseText);
        renderReviewList(result);
    };
    request.send(contentPending);
}

function showClosedReviewList() {
    var request = new XMLHttpRequest();
    var url = "/api/StaffDashboard/GetFeedbackByStatus";
    var contentClosed = '{"Status": "Closed"}';

    request.open('POST', url, true);
    request.setRequestHeader("Content-Type", "text/json");
    request.onload = function() {
        var result = JSON.parse(this.responseText);
        renderReviewList(result);
    };
    request.send(contentClosed);
}

function renderReviewList(orderList, status) {
    var list = document.getElementById("review-list");

    for (let i = 0; i < orderList.length; i++) {
        var box = document.createElement('div');
        var name = document.createElement('h4');
        var id = document.createElement('h5');
        var description = document.createElement('p');
        var info = document.createElement('div');
        var inner = document.createElement('div');
        var time = document.createElement('i');
        var btnWrap = document.createElement('div');
        var button = document.createElement('a');

        list.appendChild(box);
        box.appendChild(id);
        box.appendChild(name);
        box.appendChild(description);
        box.appendChild(info);
        box.appendChild(btnWrap);
        info.appendChild(inner);
        inner.appendChild(time);
        btnWrap.appendChild(button);

        box.classList = "review-box brd-rd5";
        info.classList = "review-info";
        inner.classList = "review-info-inner";
        btnWrap.classList = "order-info review-details";
        button.classList = "brd-rd3 feedback-popup-btn";
        time.classList = "red-clr";

        if (orderList[i].status === "Closed") {
            box.style.backgroundColor = "#e3e3e3";
            btnWrap.style.display = "none";
        }

        id.style.display = "none";

        button.href = "#";
        button.title = "Reply";

        id.innerHTML = orderList[i].feedbackID;
        name.innerHTML = orderList[i].customerEmail;
        description.innerHTML = orderList[i].requestContent;
        time.innerHTML = formatDate(orderList[i].receiveDate);
        button.innerHTML = "Reply";
    }

    //===== Feedback Popup Script =====//
    $('.feedback-popup-btn').on('click', function() {
        $('html').addClass('feedback-popup-active');

        var $info = $(this).parent().parent();
        $('#respond-feedback-id').val($info.find('h5').text());
        $('#respond-feedback-email').val($info.find('h4').text());

        return false;
    });

    $('.feedback-close-btn, .feedback-submit-btn').on('click', function() {
        $('html').removeClass('feedback-popup-active');
        return false;
    });
}

function replyFeedback() {
    var request = new XMLHttpRequest();
    var url = "/api/StaffDashboard/RespondFeedback";
    var inputs = document.forms["reply-form"].elements;

    var feedbackID = inputs[0].value;
    var customerEmail = inputs[1].value;
    var replyContent = inputs[2].value;

    var content = '{"FeedbackID": "' + feedbackID + '", "CustomerEmail": "' + customerEmail + '", "Content": "' + replyContent + '"}';
    request.open('POST', url, true);
    request.setRequestHeader("Content-Type", "text/json");
    request.onload = function() {
        var result = JSON.parse(this.responseText);
        alert(result.message);
        showReviewList();
    };
    request.send(content);
}

function acceptOrder(orderID) {
    var request = new XMLHttpRequest();
    var url = "/api/StaffDashboard/ConfirmOrder";
    var content = '{"OrderID": "' + $('#order-customer-id').html() + '", "ConfirmButton": "Accept"}';

    request.open('POST', url, true);
    request.setRequestHeader("Content-Type", "text/json");
    request.onload = function() {
        var result = JSON.parse(this.responseText);
        alert(result.message);
        showOrderList();
    }
    request.send(content);
}

function declineOrder(orderID) {
    var request = new XMLHttpRequest();
    var url = "/api/StaffDashboard/ConfirmOrder";
    var content = '{"OrderID": "' + $('#order-customer-id').html() + '", "ConfirmButton": "Decline"}';

    request.open('POST', url, true);
    request.setRequestHeader("Content-Type", "text/json");
    request.onload = function() {
        var result = JSON.parse(this.responseText);
        alert(result.message);
        showOrderList();
    }
    request.send(content);
}