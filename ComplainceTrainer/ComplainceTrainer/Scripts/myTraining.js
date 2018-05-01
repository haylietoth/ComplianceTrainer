$(document).ready(function () {
    pageLoad();
});

function pageLoad() {
    showLoadingScreen("Loading Quizzes Please Wait");
    var timeout = setTimeout(function () {
        hideLoadingScreen();
    }, 10000);
        $.ajax({
            url: '/Training/GetUserQuizes',
            type: 'GET',
            contentType: "application/json",
            success: function (data, status) {
                var needsGraded = [];
                var notCompleted = [];
                var completed = [];

                $.each(data, function (index, value) {
                    var preferNum = value.preferredDate.slice(6, 19);
                    var preferDateDisplay = new Date(parseInt(preferNum, 10)).toDateString();
                    var expireNum = value.expirationDate.slice(6, 19);
                    var expireDateDisplay = new Date(parseInt(expireNum, 10)).toDateString();
                    var today = new Date();
                    var dd = today.getDate();
                    var mm = today.getMonth() + 1;
                    var yyyy = today.getFullYear();
                    if (dd < 10) {
                        dd = '0' + dd
                    }
                    if (mm < 10) {
                        mm = '0' + mm
                    }
                    today = mm + '/' + dd + '/' + yyyy;
                    var todayInt = Date.parse(today);

                    var pDate = "<span>" + preferDateDisplay + "</span>";

                    if (todayInt < preferNum) {
                        pDate = "<span class='greenFont'>" + preferDateDisplay + "</span>";
                    }
                    else if (todayInt >= preferNum && todayInt <= expireNum) {
                        pDate = "<span class='orangeFont'>" + preferDateDisplay + "</span>";
                    }

                    var eDate = "<span>" + expireDateDisplay + "</span>";

                    if (todayInt >= expireNum) {
                        pDate = "<span class='redFont'>" + preferDateDisplay + " QUIZ CLOSED</span>"
                        eDate = "<span class='redFont'>" + expireDateDisplay + " QUIZ CLOSED</span>";
                    }

                    $('#notCompleted').on("mouseover", "tbody tr", function () {
                        if (todayInt >= expireNum) {
                            var row = $('#notCompleted').DataTable().row(this).node();
                            $(row).addClass("cursorChange");
                        }
                    });
                    if (value.isGraded) {
                        completed.push([value.userId, value.quizId, value.quizTitle, (value.percentCorrect*100) + '%']);
                    } else if (value.isCompleted) {
                        needsGraded.push([value.userId, value.quizId, value.quizTitle, (value.percentCorrect * 100) + '%']);
                    } else {
                        notCompleted.push([value.userId, value.quizId, value.quizTitle, pDate, eDate]);
                    }
                });
                $('#needsGraded').DataTable({
                    data: needsGraded,
                    columns:
                    [
                        { title: "userId", visible: false },
                        { title: "quizId", visible: false },
                        { title: "Title" },
                        { title: "Percent Correct"}
                    ]
                });
                $('#notCompleted').DataTable({
                    data: notCompleted,
                    columns:
                    [
                        { title: "userId", visible: false },
                        { title: "quizId", visible: false },
                        { title: "Title" },
                        { title: "Preferred Date" },
                        { title: "Expiration Date" }
                    ]
                });
                $('#completed').DataTable({
                    data: completed,
                    columns:
                    [
                        { title: "userId", visible: false },
                        { title: "quizId", visible: false },
                        { title: "Title" },
                        { title: "Percent Correct" }
                    ]
                });
                $('#needsGraded tbody tr').on('click', function () {
                    var data = $('#needsGraded').DataTable().row(this).data();
                    window.location.href = "/Training/Quiz/?id=" + data[1];
                });
                $('#completed tbody tr').on('click', function () {
                    var data = $('#needsGraded').DataTable().row(this).data();
                    window.location.href = "/Training/Quiz/?id=" + data[1];
                });
                $('#notCompleted').on('click', 'tbody tr', function () {
                    var data = $('#notCompleted').DataTable().row(this).data();
                    window.location.href = "/Training/Quiz/?id=" + data[1];
                });
                $('#needsGraded').on('click', 'tbody tr', function () {
                    var data = $('#needsGraded').DataTable().row(this).data();
                });
                var notCompletedTable = $('#notCompleted').dataTable();
            }
        }).then(function () {
            hideLoadingScreen();
            clearTimeout(timeout);
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