$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
});

$.ajax({
    url: '/Training/GetUserQuizes',
    type: 'GET',
    contentType: "application/json",
    success: function (data, status) {
        var counter = 0;
        $.each(data, function (index, value) {
            if (!value.isGraded && !value.isCompleted) {
                counter++;  
            }
        });  
        $('#quizNum').html(counter);
    }
});

