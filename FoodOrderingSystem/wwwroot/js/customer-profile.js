function showOrderList() {
    var request = new XMLHttpRequest();
    var url = "/api/StaffDashboard/GetOrders/all";

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
    var userID = document.getElementById("nav-user-id").innerHTML;
    var list = document.getElementById("order-list");
    list.innerHTML = "";

    for (let i = 0; i < orderList.length; i++) {
        if (userID === orderList[i].account.userID) {
            console.log(userID + " - " + orderList[i].account.userID);
            var box = document.createElement('div');
            var info = document.createElement('div');
            var time = document.createElement('h4');
            var price = document.createElement('span');
            var status = document.createElement('span');
            var detailBtn = document.createElement('a');
            var feedbackBtn = document.createElement('a');
            var orderID = document.createElement('h5');

            list.appendChild(box);
            box.appendChild(info);
            info.appendChild(time);
            info.appendChild(price);
            info.appendChild(status);
            info.appendChild(detailBtn);
            info.appendChild(feedbackBtn);
            info.appendChild(orderID);

            box.classList = "order-item brd-rd5";
            info.classList = "order-info";
            price.classList = "price money";
            status.classList = "brd-rd3";
            detailBtn.classList = "brd-rd3 detail-popup-btn";
            feedbackBtn.classList = "brd-rd3 feedback-popup-btn";

            detailBtn.href = "#";
            feedbackBtn.href = "#";
            detailBtn.style.marginLeft = "1rem";
            orderID.style.display = "none";

            var orderStatus = orderList[i].customerOrder.status.toUpperCase();
            if (orderStatus === "PENDING") {
                status.classList.add("processing")
            } else if (orderStatus === "ACCEPTED") {
                status.classList.add("completed")
            } else if (orderStatus === "REJECTED") {
                status.classList.add("rejected")
            }

            time.innerHTML = formatDate(orderList[i].customerOrder.orderDate);
            price.innerHTML = orderList[i].customerOrder.total + calcDeliveryFee(orderList[i].customerOrder.total);
            status.innerHTML = orderStatus;
            orderID.innerHTML = orderList[i].customerOrder.orderID;
            detailBtn.innerHTML = "Order Detail";
            feedbackBtn.innerHTML = "Feedback";
        }
    }

    callback();

    //===== Feedback Popup Script =====//
    $('.feedback-popup-btn').on('click', function() {
        $('html').addClass('feedback-popup-active');
        return false;
    });

    $('.feedback-close-btn, .feedback-submit-btn').on('click', function() {
        $('html').removeClass('feedback-popup-active');
        return false;
    });

    //===== Detail Popup Script =====//
    $('.detail-popup-btn').on('click', function() {
        $('html').addClass('detail-popup-active');

        getOrderDetail($(this).parent().find('h5').text());

        return false;
    });

    $('.detail-close-btn, .detail-submit-btn').on('click', function() {
        $('html').removeClass('detail-popup-active');
        return false;
    });
}

function getOrderDetail(orderID) {
    var request = new XMLHttpRequest();
    var url = "/api/StaffDashboard/GetOrderDetailsByOrderID";
    var content = '{"OrderID": "' + orderID + '"}';

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

    deliveryFee = calcDeliveryFee(subtotal);

    document.getElementById("subtotal-order-details").innerHTML = subtotal;
    document.getElementById("delivery-order-details").innerHTML = deliveryFee;
    document.getElementById("total-order-details").innerHTML = subtotal + deliveryFee;

    callback();
}

function sendFeedback() {
    var request = new XMLHttpRequest();
    var url = "/api/CustomerDashboard/AddFeedback";
    var inputs = document.forms["feedback-form"].elements;

    var feedbackContent = inputs[0].value;
    var customerEmail = "huuquoc852@gmail.com";
    var content = '{"CustomerEmail": "' + customerEmail + '", "RequestContent": "' + feedbackContent + '"}';

    request.open('POST', url, true);
    request.setRequestHeader("Content-Type", "text/json");
    request.onload = function() {
        var result = JSON.parse(this.responseText);
        alert("Sent " + result.message + "fully");
    };
    request.send(content);
}