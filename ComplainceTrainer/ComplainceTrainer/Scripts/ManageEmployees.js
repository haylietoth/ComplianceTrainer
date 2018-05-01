var userData = [];
$(document).ready(function () {
    pageLoad();
    $('#employeeTable tbody').on('click', 'tr', function () {

        var data = $('#employeeTable').DataTable().row(this).data()

        alert('You clicked on ' + data[0] + '\'s row');

    });
});
function pageLoad() {
    showLoadingScreen("Loading Employees Please Wait");
    var timeout = setTimeout(function () {
        hideLoadingScreen();
    }, 5000);

    if (document.getElementById("hrCheck").value == "True") {
        $.ajax({
            url: '/Training/GetAllUsers',
            type: 'GET',
            cache: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data, status) {
                fillDataTable(data);
            },
            error: function () {
            }
        }).then(function () {
            hideLoadingScreen();
            clearTimeout(timeout);
        }); 
    } else {
        $.ajax({
            url: '/Training/GetAllManagersUsers',
            type: 'GET',
            cache: false,
            success: function (data, status) {
                fillDataTable(data);
            },
            error: function () {
            }
        }).then(function () {
            hideLoadingScreen();
            clearTimeout(timeout);
        }); 
    }
} 

function fillDataTable(users) {
    if (document.getElementById("hrCheck").value == "True") {
        $.each(users, function (index, value) {
            if (value.hasUngradedQuiz) {
                userData.push([value.id, value.firstName + " " + value.lastName, value.SAMAccountName, value.manager, '<span class="glyphicon glyphicon-time"></span>']);
            } else {
                userData.push([value.id, value.firstName + " " + value.lastName, value.SAMAccountName, value.manager, '<span class="glyphicon glyphicon-ok"></span>']);
            }
        });
        $('#employeeTable').DataTable({
            data: userData,
            columns: 
            [
                { title: "id", visible: false },
                { title: "Name" },
                { title: "Username" },
                { title: "manager" },
                { title: "Graded" }
            ]
        });
        
    } else {
        $.each(users, function (index, value) {
            userData.push([value.id, value.firstName + " " + value.lastName, value.SAMAccountName, value.manager]);
        });
        $('#employeeTable').DataTable({
            data: userData,
            columns: [
                { title: "id", visible: false},
                { title: "Name" },
                { title: "Username" },
                { title: "manager" }
            ]
        });
    }
    $('#employeeTable tbody tr').on('click', function () {
        var data = $('#employeeTable').DataTable().row(this).data();
        window.location.href = "/Training/EmployeeQuizes/?id=" + data[0];
    });
}

function showLoadingScreen(message) {
    document.getElementById('spinnerText').innerHTML = message;
    $('#spinner').fadeIn("fast");
    document.getElementById('main-container').style.display = "none";
    document.getElementById('spinner').style.display = "block";
}

function hideLoadingScreen() {
    $("#spinner").fadeOut("fast");
    document.getElementById('spinner').style.display = "none";
    document.getElementById('main-container').style.display = "block";
    $('#spinner').stop();
}