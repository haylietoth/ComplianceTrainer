/* This file gets the data from the Training controller. It fills the table
   created in the 'AllTraining.cshtml' page with data from the Quiz table. */
$(document).ready(function () {
    pageLoad();

    $("body").on("click", ".edit", function () {
        window.location.href = "/Training/updateTraining/?id=" + this.id;
    });

    var id;
    var row;
    $("body").on("click", ".remove", function () {
        $("#confirmRemove").modal('show');
        id = $(this).attr("id");
        row = $(this);
    });

    $('#removeQuizBtn').on('click', function () {
        $("#confirmRemove").modal('hide');
        $('#trainingTable').DataTable().row(row.parents('tr')).remove().draw();
        $.ajax({
            method: 'post',
            url: '/Training/RemoveQuiz',
            contentType: "application/json",
            dataType: "json",
            data: JSON.stringify({ id: id }),
            success: function (data, status) {
            }
        });
    });
});


function pageLoad() {
    showLoadingScreen("Loading Quizzes Please Wait");
    var timeout = setTimeout(function () {
        hideLoadingScreen();
    }, 5000);
    var quizes = [];
    $.ajax({
        url: '/Training/GetAllQuizes',
        type: 'GET',
        success: function (data, status) {
            $.each(data, function (index, value) {

                var startNum = value.startDate.slice(6, 19);
                var startDate = new Date(parseInt(startNum, 10)).toDateString();
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
                var button = "<input type='button' style='width:60px;margin-right:1em;' value='Edit' class='btn btn-primary edit' id='" + value.id + "'/>";
                if (startNum <= todayInt) {
                    button = "<input type='button' style='width:60px;margin-right:1em;' value='View' class='btn btn-primary edit' id='" + value.id + "'/>";
                };
                quizes.push([value.id, value.title, value.description, startDate, button + "<input type='button' value='Remove' class='btn btn-primary remove' id='" + value.id + "'/>"]);
            });
            $('#trainingTable').DataTable({
                data: quizes,
                columns: [
                    { title: "id", visible: false },
                    { title: "Title" },
                    { title: "Description" },
                    { title: "Start Date" },
                    { title: "Edit or Remove Training" }
                ]
            });
        }
    }).then(function () {
        hideLoadingScreen();
        clearTimeout(timeout);
    });   
};

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


