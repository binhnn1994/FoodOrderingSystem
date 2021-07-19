function checkLogin() {
    var inputs = document.forms["login-form"].elements;

    if (isLoginValid(inputs)) {
        var request = new XMLHttpRequest();
        var url = "/api/Common/Login";
        var content = '{"Email": "' + inputs[0].value + '", "Password": "' + inputs[1].value + '"}';

        request.open('POST', url, true);
        request.setRequestHeader("Content-Type", "text/json");
        request.onload = function() {
            var result = JSON.parse(this.responseText);
            if (result.role === "Admin") {
                window.location.href = "../AdminDashboard/Index";
            } else if (result.role === "Customer") {
                window.location.href = "/CustomerDashboard/Index";
            } else if (result.role === "Staff") {
                window.location.href = "../StaffDashboard/Index";
            } else {
                $('#login-error-message').show();
                $('#login-error-message').html("Username or password is incorrect! Please try again.");
            }
        };
        request.send(content);
    } else {
        $('#login-error-message').show();
        $('#login-error-message').html("Please enter your email and password.");
    }
}

function isLoginValid(inputs) {
    $('#login-error-message').hide();

    if (inputs[0].value == "" || inputs[1].value == "") {
        return false;
    }
    return true;
}

function registerCustomer() {
    var inputs = document.forms["register-form"].elements;

    if (isRegisterValid(inputs)) {
        var request = new XMLHttpRequest();
        var url = "/api/Common/Register";
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
                $('html').removeClass('sign-popup-active');
                alert("Signed up successfully!");
                clearRegisterError(inputs);
                setTimeout(function() { location.reload(); }, 500);
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

function isRegisterValid(inputs) {
    clearRegisterError(inputs);
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

function clearRegisterError(inputs) {
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