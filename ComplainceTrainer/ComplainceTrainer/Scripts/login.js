$(document).ready(function () {
    $('#LogOnButton').click(function () {
        event.preventDefault();
        login();
    });
});

function login() {
    var myUsername = document.getElementById("username").value;
    var myPassword = document.getElementById("password").value;
    $.ajax({
        url: '/Home/Login',
        type: "Post",
        data: { username: myUsername, password: myPassword },
        contextType: "application/json"
    });
}