function showCustomerList() {
    var request = new XMLHttpRequest();
    var url = "/api/AdminDashboard/ViewCustumerList";
    var content = '{"RowsOnPage": 100, "RequestPage": 1}';

    request.open('POST', url, true);
    request.setRequestHeader("Content-Type", "text/json");
    request.onload = function() {
        var result = JSON.parse(this.responseText).data;
        renderCustomerList(result);
    };
    request.send(content);
}

function renderCustomerList(customerList) {
    var table = document.getElementById("customer-table");
    table.innerHTML = "";

    for (let i = 0; i < customerList.length; i++) {
        var row = table.insertRow(-1);

        var cellNo = row.insertCell(0);
        var cellName = row.insertCell(1);
        var cellPhone = row.insertCell(2);
        var cellEmail = row.insertCell(3);
        var cellAddress = row.insertCell(4);
        var cellStatus = row.insertCell(5);
        var cellID = row.insertCell(6);

        cellNo.innerHTML = i + 1;
        cellName.innerHTML = customerList[i].fullname;
        cellPhone.innerHTML = customerList[i].phoneNumber;
        cellEmail.innerHTML = customerList[i].userEmail;
        cellAddress.innerHTML = customerList[i].address;
        cellID.innerHTML = customerList[i].userID;

        cellID.style.display = "none";

        var div = document.createElement('div');
        div.classList = "form-check form-switch";
        var input = document.createElement('input');
        input.type = "checkbox";
        input.classList = "form-check-input customer-status-switch";
        input.checked = (customerList[i].status === "Active");
        div.appendChild(input);
        cellStatus.appendChild(div);

        if (input.checked === true) {
            row.classList.remove("gray-bg");
        } else {
            row.classList.add("gray-bg");
        }
    }

    //===== Customer Status Script =====//
    $('.customer-status-switch').on('click', function() {
        var $row = $(this).closest('tr');

        if ($(this).is(':checked')) {
            activateAccount($row.children("td:nth-child(7)").text());
            $row.removeClass("gray-bg");
        } else {
            deactivateAccount($row.children("td:nth-child(7)").text());
            $row.addClass("gray-bg");
        }
    });
}