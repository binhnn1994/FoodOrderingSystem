function checkLogin() {
    $('#login-error-message').hide();

    var inputs = document.forms["login-form"].elements;

    var request = new XMLHttpRequest();
    var url = "/api/Common/Login";
    var content = '{"Email": "' + inputs[0].value + '", "Password": "' + inputs[1].value + '"}';

    request.open('POST', url, true);
    request.setRequestHeader("Content-Type", "text/json");
    request.onload = function() {
        var result = JSON.parse(this.responseText);
        if (result.role === "Admin") {
            window.location.href = "../AdminDashboard/Index";
        } else
        if (result.role === "Customer") {
            window.location.href = "/CustomerDashboard/Index";
        } else
        if (result.role === "Staff") {
            window.location.href = "../StaffDashboard/Index";
        } else {
            $('#login-error-message').show();
            $('#login-error-message').html("Username or password is incorrect! Please try again.");
        }
    };
    request.send(content);
}