function showStaffsList() {
    var searchValue = document.getElementById("search-value").value;

    var request = new XMLHttpRequest();
    var url, content;

    if (searchValue.length > 0) {
        url = "/api/AdminDashboard/ViewStaffListBySearching";
        content = '{"SearchValue": "' + searchValue + '", "RowsOnPage": 100, "RequestPage": 1}';
    } else {
        url = "/api/AdminDashboard/ViewStaffsList";
        content = '{"RowsOnPage": 100, "RequestPage": 1}';
    }

    request.open('POST', url, true);
    request.setRequestHeader("Content-Type", "text/json");
    request.onload = function() {
        var result = JSON.parse(this.responseText).data;
        renderStaffList(result);
    };
    request.send(content);
}

function renderStaffList(staffList) {
    var table = document.getElementById("staff-table");
    table.innerHTML = "";

    for (let i = 0; i < staffList.length; i++) {
        var row = table.insertRow(-1);

        var cellNo = row.insertCell(0);
        var cellName = row.insertCell(1);
        var cellPhone = row.insertCell(2);
        var cellEmail = row.insertCell(3);
        var cellAddress = row.insertCell(4);
        var cellAction = row.insertCell(5);
        var cellStatus = row.insertCell(6);
        var cellID = row.insertCell(7);

        cellNo.innerHTML = i + 1;
        cellName.innerHTML = staffList[i].fullname;
        cellPhone.innerHTML = staffList[i].phoneNumber;
        cellEmail.innerHTML = staffList[i].userEmail;
        cellAddress.innerHTML = staffList[i].address;
        cellID.innerHTML = staffList[i].userID;

        var button = document.createElement('button');
        button.classList = "brd-rd2 update-popup-btn";
        button.innerHTML = "UPDATE";
        cellAction.appendChild(button);

        var div = document.createElement('div');
        div.classList = "form-check form-switch";
        var input = document.createElement('input');
        input.type = "checkbox";
        input.classList = "form-check-input staff-status-switch";
        input.checked = (staffList[i].status === "Active");
        div.appendChild(input);
        cellStatus.appendChild(div);

        if (input.checked === true) {
            row.classList.remove("gray-bg");
        } else {
            row.classList.add("gray-bg");
        }
    }

    //===== Update Popup Script =====//
    $('.update-popup-btn').on('click', function() {
        $('html').addClass('update-popup-active');

        var $row = $(this).closest('tr');

        $('#staff-name-edit').val($row.children("td:nth-child(2)").text());
        $('#staff-phone-edit').val($row.children("td:nth-child(3)").text());
        $('#staff-email-edit').val($row.children("td:nth-child(4)").text());
        $('#staff-address-edit').val($row.children("td:nth-child(5)").text());
        $('#staff-id-edit').val($row.children("td:nth-child(8)").text());

        return false;
    });

    $('.update-close-btn, .update-submit-btn').on('click', function() {
        $('html').removeClass('update-popup-active');
        return false;
    });

    //===== Staff Status Script =====//
    $('.staff-status-switch').on('click', function() {
        var $row = $(this).closest('tr');

        if ($(this).is(':checked')) {
            activateAccount($row.children("td:nth-child(8)").text());
            $row.removeClass("gray-bg");
        } else {
            deactivateAccount($row.children("td:nth-child(8)").text());
            $row.addClass("gray-bg");
        }
    });
}

function updateStaff() {
    var inputs = document.forms["update-form"].elements;

    var request = new XMLHttpRequest();
    var url = "/api/AdminDashboard/UpdateStaffInformation";
    var content = 'userID=' + inputs[0].value;
    content += "&fullname=" + encodeURIComponent(inputs[2].value);
    content += "&phoneNumber=" + inputs[3].value;
    content += "&address=" + encodeURIComponent(inputs[4].value);

    request.open('POST', url, true);
    request.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    request.onload = function() {
        alert("Updated successfully!");
        showStaffsList();
    };
    request.send(content);
}

function createStaff() {
    var inputs = document.forms["create-form"].elements;

    if (isCreateStaffValid(inputs)) {
        var request = new XMLHttpRequest();
        var url = "/api/AdminDashboard/CreateStaff";
        var content = 'userEmail=' + inputs[0].value;
        content += "&hashedPassword=" + inputs[1].value;
        content += "&fullname=" + inputs[2].value;
        content += "&phoneNumber=" + inputs[3].value;
        content += "&address=" + inputs[4].value;

        request.open('POST', url, true);
        request.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        request.onload = function() {
            var message = JSON.parse(this.responseText).message;
            if (message === "success") {
                $('html').removeClass('create-popup-active');
                alert("Added successfully!");
                clearCreateStaffError(inputs)
                showStaffsList();
            } else if (message.includes("Duplicate") && message.includes("userEmail")) {
                $(".sign-form").find(".err-msg")[0].textContent = "This email address has been used";
                $(inputs[0]).css("border", "1px solid red");
            } else if (message.includes("Duplicate") && message.includes("phoneNumber")) {
                $(".sign-form").find(".err-msg")[3].textContent = "This phone number has been used";
                $(inputs[3]).css("border", "1px solid red");
            }
        };
        request.send(content);
    }
}

function isCreateStaffValid(inputs) {
    clearCreateStaffError(inputs);
    if (!IsEmail(inputs[0].value) || inputs[1].value == "" || inputs[2].value == "" || !IsPhone(inputs[3].value) || inputs[4].value == "") {
        if (!IsEmail(inputs[0].value)) {
            $(".sign-form").find(".err-msg")[0].textContent = "Must have a valid email address";
            $(inputs[0]).css("border", "1px solid red");
        }
        if (inputs[1].value == "") {
            $(".sign-form").find(".err-msg")[1].textContent = "Must have a password";
            $(inputs[1]).css("border", "1px solid red");
        }
        if (inputs[2].value == "") {
            $(".sign-form").find(".err-msg")[2].textContent = "Must have a name";
            $(inputs[2]).css("border", "1px solid red");
        }
        if (!IsPhone(inputs[3].value)) {
            $(".sign-form").find(".err-msg")[3].textContent = "Must have a valid phone number";
            $(inputs[3]).css("border", "1px solid red");
        }
        if (inputs[4].value == "") {
            $(".sign-form").find(".err-msg")[4].textContent = "Must have an address";
            $(inputs[4]).css("border", "1px solid red");
        }
        return false;
    }
    return true;
}

function clearCreateStaffError(inputs) {
    $(inputs[0]).css("border", "none");
    $(inputs[1]).css("border", "none");
    $(inputs[2]).css("border", "none");
    $(inputs[3]).css("border", "none");
    $(inputs[4]).css("border", "none");

    $(".sign-form").find(".err-msg")[0].textContent = "";
    $(".sign-form").find(".err-msg")[1].textContent = "";
    $(".sign-form").find(".err-msg")[2].textContent = "";
    $(".sign-form").find(".err-msg")[3].textContent = "";
    $(".sign-form").find(".err-msg")[4].textContent = "";
}