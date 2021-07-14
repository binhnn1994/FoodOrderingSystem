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
    var list = document.getElementById("order-list");
    list.innerHTML = "";

    for (let i = 0; i < orderList.length; i++) {
        var box = document.createElement('div');
        var info = document.createElement('div');
        var time = document.createElement('h4');
        var price = document.createElement('span');
        var status = document.createElement('span');
        var detailBtn = document.createElement('a');
        var feedbackBtn = document.createElement('a');

        list.appendChild(box);
        box.appendChild(info);
        info.appendChild(time);
        info.appendChild(price);
        info.appendChild(status);
        info.appendChild(detailBtn);
        info.appendChild(feedbackBtn);

        box.classList = "order-item brd-rd5";
        info.classList = "order-info";
        price.classList = "price money";
        status.classList = "processing brd-rd3";
        detailBtn.classList = "brd-rd3 detail-popup-btn";
        feedbackBtn.classList = "brd-rd3 feedback-popup-btn";

        detailBtn.href = "#";
        feedbackBtn.href = "#";
        detailBtn.style.marginLeft = "1rem";

        time.innerHTML = formatDate(orderList[i].customerOrder.orderDate);
        price.innerHTML = orderList[i].customerOrder.total;
        status.innerHTML = orderList[i].customerOrder.status.toUpperCase();
        detailBtn.innerHTML = "Order Detail";
        feedbackBtn.innerHTML = "Feedback";
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
        return false;
    });

    $('.detail-close-btn, .detail-submit-btn').on('click', function() {
        $('html').removeClass('detail-popup-active');
        return false;
    });
}

function getOrderDetail() {
    var request = new XMLHttpRequest();
    var url = "/api/StaffDashboard/GetOrderDetailsByOrderID";
    var content = '{"OrderID": "' + $('#order-customer-id').html() + '"}';

    request.open('POST', url, true);
    request.setRequestHeader("Content-Type", "text/json");
    request.onload = function() {
        var result = JSON.parse(this.responseText);
        renderOrderDetail(result);
    }
    request.send(content);
}

function renderOrderDetail(orderDetail) {
    function getItemNameAndPrice(itemID) {
        var request = new XMLHttpRequest();
        var url = "/api/AdminDashboard/ViewItemDetail";
        var content = '{"ItemID": "' + itemID + '"}';

        request.open('POST', url, true);
        request.setRequestHeader("Content-Type", "text/json");
        request.onload = function() {
            console.log(this.responseText);
            var result = JSON.parse(this.responseText);
            values = {
                name: result.itemName,
                price: result.unitPrice
            }
        }
        request.send(content);
    }

    var list = document.getElementById("order-details-list");

    while (list.firstChild) {
        list.removeChild(list.firstChild);
    }

    for (let i = 0; i < orderDetail.length; i++) {
        var values = getItemNameAndPrice(orderDetail[i].itemID);
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
        name.innerHTML += " x " + values;
        price.innerHTML = values[1] * orderDetail[i].quantity;
    }
}

function sendFeedback() {
    var request = new XMLHttpRequest();
    var url = "/api/CustomerDashboard/AddFeedback";
    // var inputs = document.forms["reply-form"].elements;

    // var feedbackID = inputs[0].value;
    // var customerEmail = inputs[1].value;
    // var content = inputs[2].value;

    // var content = '{"FeedbackID": "' + feedbackID + '", "CustomerEmail": "' + customerEmail + '", "Content": "' + content + '"}';
    // request.open('POST', url, true);
    // request.setRequestHeader("Content-Type", "text/json");
    // request.onload = function() {
    //     var result = JSON.parse(this.responseText);
    //     alert(result.message);
    //     showReviewList();
    // };
    // request.send(content);
}