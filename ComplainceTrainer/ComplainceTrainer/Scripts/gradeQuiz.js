/*
 * AngularJS code for the GradeQuiz application, called on GradeQuiz.cshtml.
 */
var app = angular.module('QuizApp', ['ngRoute']);
app.controller('QuizCtrl', function ($scope, $http) {
    var gradeApprovals = [];
    var questionIterator = 0;
    $scope.data;
    $scope.quiz = {
        iterator: 0,
        answer: 0,
        quizId: 0,
        questions: [{
            questionId: 0,
            questionText: '',
            type: '',
            answerText: '',
            isApproved: false
        }]
    }
    var quizId = $('#quizId').val();
    var config = {
        headers: {
            'Content-Type': 'application/json;'
        }
    }
    $http.post('/Training/ViewGradeQuiz', JSON.stringify({ id: quizId }), config).then(function (res) {
        $scope.quiz.questions.splice(0,1)
        $scope.data = res.data;
        for (var i = 0; i < $scope.data.questions.length; i++) {
            if ($scope.data.questions[i].type == 'shortAnswer') {
                for (var j = 0; j < $scope.data.juqqas.length; j++) {
                    if ($scope.data.questions[i].id === $scope.data.juqqas[j].questionId) {
                        var question = $scope.data.questions[i];
                        var juqqa = $scope.data.juqqas[j];
                        $scope.quiz.questions.push({
                            iterator: i,
                            questionId: question.id,
                            questionText: question.text,
                            type: 'shortAnswer',
                            answerText: juqqa.text
                        });
                        questionIterator++;
                    }
                }
            }
        }
    });

    $scope.accept = function (i) {
        $scope.quiz.questions[i].isApproved = true;
    }
    $scope.reject = function (i) {
        $scope.quiz.questions[i].isApproved = false;
    }
    $scope.submit = function () {
        for (var i = 0; i < $scope.data.questions.length; i++) {
            var uqqa = {
                quizId: quizId,
                userId: userId,
                questionId: angular.copy($scope.quiz.question.questionId),
                isApproved: $scope.quiz.questions[i].isApproved
            }
            gradeApprovals.push(uqqa);
        }
        $('#confirm-submit').modal('hide');
        $http.post('/Training/SubmitGrade', JSON.stringify({ decisions: gradeApprovals }), config)
    }
});
