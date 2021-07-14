function showOrderList() {
    var request = new XMLHttpRequest();
    var url = "/api/StaffDashboard/GetPendingOrder";

    request.open('GET', url, true);
    request.onload = function() {
        var result = JSON.parse(this.responseText);
        renderOrderList(result);
    };
    request.send();
}

function renderCustomerList(orderList) {
    var table = document.getElementById("customer-table");
    table.innerHTML = "";

    for (let i = 0; i < orderList.length; i++) {
        var row = table.insertRow(-1);

        row.classList = "order-item";

        var cellId = row.insertCell(0);
        var cellName = row.insertCell(1);
        var cellDate = row.insertCell(2);
        var cellAddress = row.insertCell(3);
        var cellTotal = row.insertCell(4);

        cellId.innerHTML = orderList[i].orderID;
        cellName.innerHTML = orderList[i].name;
        cellDate.innerHTML = orderList[i].orderDate;
        cellAddress.innerHTML = orderList[i].toAddress;
        cellTotal.innerHTML = '<span class="money">' + orderList[i].total + '</span>';
    }

    //===== Customer Status Script =====//
    $('.order-item').on('click', function() {
        console.log("hihi");
    });
}