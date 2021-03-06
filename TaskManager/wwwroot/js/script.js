//Right-bottom side Controls

var mode = "Create"
var isHasSubtasks = false;
$(".add-btn").on("click", function () {
    $(".add").css("display", "flex")
    enableCover()
    $("#name-input").val("")
    $("#descriptionTextArea").val("")
    $("#executors").val("")
    $("endDate").val("")
    $("#endDate").prop("disabled", false)
    $("#parentID").val("")
    $(".needs-validation>button").eq(1).css("display", "none")
    $(".needs-validation>button").eq(0).css("display", "block")
    mode="Create"
})
$(".edit-btn").on("click", function () {
    $(".add").css("display", "flex")
    mode = "Edit"
    enableCover()
    getTaskInfo(fillEditForm)
    $(".needs-validation>button").eq(0).css("display", "none")
    $(".needs-validation>button").eq(1).css("display", "block")
    mode = "Edit"
})
// Fill full form with data provided by the server about current selected (.acive) task

function fillEditForm(response) {
    $("#name-input").val(response.name)
    $("#descriptionTextArea").val(response.description)
    $("#executors").val(response.executors)
    $("#endDate").val("1999-01-01")
    $("#endDate").prop("disabled", true)
    $(`#parentID option[value="${response.parentTaskID}"]`).prop("selected", true)
}
$(".delete-btn").on("click", function () {
    $(".confirmation").css("display", "flex")
    enableCover()
})

//Disable cover black half-transparent block on background or Enable

function disableCover() {
    $(".add").css("display", "none")
    $(".confirmation").css("display", "none")
    $(".cover").css("display", "none")
}
function enableCover() {
    $(".cover").css("display", "block")
}
$(".cover").on("click", function () {
    disableCover()
})

//Image for Loading while Ajax is processing

function showLoader() {
    $("#Loader").css("display", "block")
}
function hideLoader() {
    $("#Loader").css("display", "none")
}
function cutDate(date) {
    return moment(date).format("DD.MM.YYYY")

}

//Display major tasks (not subtasks) on open the app

$(document).ready(function () {
    getMajorTasks(displayMajorTasks)
})
//Selection of another task => Provide Additional task info on right side and slide-down all the subtasks

$(".list-group").on("click","a", function () {
    $(".list-group-item").removeClass("active")
    $(this).addClass("active")
    getTaskInfo(displayTaskInfo)
    return false
})
//Getting Major tasks from server

function getMajorTasks(displayMajorTasks) {
    $(".list-group").find(".list-group-item").remove()
    $.ajax({
        type: "GET",
        url: "/Home/GetMajorTasks",
        data: {},
        contentType: "application/json charset=utf-8",
        dataType: "json",
        beforeSend: showLoader(),
        success: displayMajorTasks,
        complete: hideLoader(),
        failure: function (jqXHR, textsStatus, errorThrown) {
            alert("HTTP Status: " + jqXHR.status + "; Error Text: " + jqXHR.responseText)
        }
    })
}
//Getting All info about current selected (.active class) task

function getTaskInfo(displayTaskInfo) {
    var taskID = $(".active").attr("href")
    taskID = taskID.replace("?id=", "")
    $.ajax({
        type: "POST",
        url: "/Home/GetTaskInfo",
        data: JSON.stringify(taskID),
        contentType: "application/json charset=utf-8",
        dataType: "json",
        beforeSend: showLoader(),
        success: displayTaskInfo,
        complete: hideLoader(),
        failure: function (jqXHR, textsStatus, errorThrown) {
            alert("HTTP Status: " + jqXHR.status + "; Error Text: " + jqXHR.responseText)
        }
    })
}
//Getting All current selected task's subtasks info

function getSubtasks(displaySubtasks) {
    var taskID = $(".active").attr("href")
    taskID = taskID.replace("?id=", "")
    $.ajax({
        type: "POST",
        url: "/Home/GetSubtasks",
        data: JSON.stringify(taskID),
        contentType: "application/json charset=utf-8",
        async:"true",
        beforeSend: showLoader(),
        complete: hideLoader(),
        failure: function (jqXHR, textsStatus, errorThrown) {
            alert("HTTP Status: " + jqXHR.status + "; Error Text: " + jqXHR.responseText)
        }
    }).done(function (data, textStatus, jqXHR) {
        return displaySubtasks(data)
    }).fail(function () {
        console.log("HTTP Status: " + jqXHR.status + "; Error Text: " + jqXHR.responseText)
    })
}
function displayMajorTasks(response) {
    $.each(response, function (index, task) {
        $(".list-group").append(`<a href="?id=${task.taskID}" class="list-group-item list-group-item-action py-3 lh-tight">
                    <div class= "d-flex w-100 align-items-center justify-content-between">
                <strong class="mb-1">${task.name}</strong>
                <small>${cutDate(task.plannedEndDate)}</small>
            </div>
                    <div class="col-10 mb-1 small">${task.description}</div>
        </a>`)
    })
    $("a.list-group-item:first").click();

}
function displayTaskInfo(response) {
    var registerDate = moment(cutDate(response.registerDate), "DD.MM.YYYY")
    var plannedEndDate = moment(cutDate(response.plannedEndDate), "DD.MM.YYYY")
    var factEndDate = moment(cutDate(response.factEndDate), "DD.MM.YYYY")
    var labor=0
    var laborFact = 0
    if (!getSubtasks(hasSubtasks)) {
        labor = moment.duration(plannedEndDate.diff(registerDate)).asHours();
        laborFact = moment.duration(factEndDate.diff(registerDate)).asHours();
    }
    $(".caption-top h4").text(response.name)
    $(".description p").text(response.description)
    $(".executors p").text(response.executors)
    $(".time p").eq(0).text(`${cutDate(response.registerDate)} - ${cutDate(response.plannedEndDate)}`)
    $(".time p").eq(1).text(`${labor} ч.`)
    if (laborFact > 0) $(".time").eq(2).text +=`(${laborFact} ч.)`
    if (response.factEndDate != "0001-01-01T00:00:00") $(".time p").text += "(" + cutDate(response.factEndDate) + ")"
    $(".status p").text(response.status)
    getSubtasks(displaySubtasks)

}
function hasSubtasks(response) {
    if (jQuery.isEmptyObject(response)) {
        console.log("This task has no subtasks")
        return false
    }
    else {
        console.log("This task has subtasks")
        console.log(response)
        labor = getSubtasks(calcLabor)
        laborFact = getSubtasks(calcLaborFact)
        return true
    }
}
function calcLabor(response) {
    var labor = 0
    var registerDate
    var plannedEndDate
    $.each(response, function (index, subtask) {
        registerDate = moment(cutDate(subtask.registerDate), "DD.MM.YYYY")
        plannedEndDate = moment(cutDate(subtask.plannedEndDate), "DD.MM.YYYY")
        labor += moment.duration(plannedEndDate.diff(registerDate)).asHours()
    })
    $(".time p").eq(1).text(`${labor} ч.`)
    return labor;
}
function calcLaborFact(response) {
    var laborFact = 0
    var registerDate
    var factEndDate
    $.each(response, function (index, subtask) {
        registerDate = moment(cutDate(subtask.registerDate), "DD.MM.YYYY")
        factEndDate = moment(cutDate(subtask.factEndDate), "DD.MM.YYYY")
        laborFact += moment.duration(factEndDate.diff(registerDate)).asHours()
    })
    if (laborFact > 0) $(".time").eq(2).text += `(${laborFact} ч.)`
    return laborFact;
}
function displaySubtasks(response) {
    if (!$(".active").next().is(".subgroup-l1") && !$(".active.subgroup-l1").next().is(".subgroup-l2") && !$(".active.subgroup-l2").next().is(".subgroup-l3")) {
        response.reverse()
        $.each(response, function (index, task) {
            $(".list-group .active").after(`<a href="?id=${task.taskID}" class="list-group-item ${nextSubGroupLevelCheck()} list-group-item-action py-3 lh-tight">
                    <div class= "d-flex w-100 align-items-center justify-content-between">
                <strong class="mb-1">${task.name}</strong>
                <small>${cutDate(task.plannedEndDate)}</small>
            </div>
                    <div class="col-10 mb-1 small">${task.description}</div>
        </a>`)
        })
    }
}
//Garbage function that would be better to change later... Check what class do we need to provide to our subtasks group

function nextSubGroupLevelCheck() {
    if ($(".active").hasClass("subgroup-l1")) return "subgroup-l2";
    if ($(".active").hasClass("subgroup-l2")) return "subgroup-l3";
    else return "subgroup-l1";
}

$(".needs-validation").submit(function (event) {
    event.preventDefault()
    if (mode == "Create") addTask(getMajorTasks)
    else editTask(getMajorTasks)
})

function editTask(getMajorTasks) {
    var taskID = $(".active").attr("href")
    taskID = taskID.replace("?id=", "")
    var task = {
        TaskID: taskID,
        Name: $("#name-input").val(),
        Description: $("#descriptionTextArea").val(),
        Executors: $("#executors").val(),
        PlannedEndDate: $("#endDate").val(),
        ParentTaskID: $("#parentID option:selected").attr("value")
    }
    $.ajax({
        type: "POST",
        url: "/Home/EditTask",
        data: JSON.stringify(task),
        contentType: 'application/json',
        beforeSend: showLoader(),
        complete: hideLoader()
    }).done(function (data, textStatus, jqXHR) {
        console.log("Task Edited")
        getMajorTasks(displayMajorTasks)
    }).fail(function () {
        console.log("HTTP Status: " + jqXHR.status + "; Error Text: " + jqXHR.responseText)
    })
    disableCover()
}
function addTask(getMajorTasks) {
    var taskID = $(".active").attr("href")
    taskID = taskID.replace("?id=", "")
    var task = {
        TaskID: taskID,
        Name: $("#name-input").val(),
        Description: $("#descriptionTextArea").val(),
        Executors: $("#executors").val(),
        PlannedEndDate: $("#endDate").val(),
        ParentTaskID: $("#parentID option:selected").attr("value")
    }
    $.ajax({
        type: "POST",
        url: "/Home/AddTask",
        data: JSON.stringify(task),
        contentType: 'application/json',
        beforeSend: showLoader(),
        complete: hideLoader()
    }).done(function (data,textStatus, jqXHR) {
        console.log("Task Created")
        getMajorTasks(displayMajorTasks)
    }).fail(function () {
        console.log("HTTP Status: " + jqXHR.status + "; Error Text: " + jqXHR.responseText)
    })
    disableCover()
}

//Confirmation for delete current task

$(".confirmation #confirm").on("click", function () {
    deleteTask(getMajorTasks)
    disableCover()
})
$("#reject").on("click", function () {
    disableCover()
})
function deleteTask(getMajorTasks) {
    var taskID = $(".active").attr("href")
    taskID = taskID.replace("?id=", "")
    $.ajax({
        type: "POST",
        url: "/Home/DeleteTask",
        data: JSON.stringify(taskID),
        contentType: "application/json charset=utf-8",
        beforeSend: showLoader(),
        complete: hideLoader()
    }).done(function (data, textStatus, jqXHR) {
        console.log("Task Deleted")
        getMajorTasks(displayMajorTasks)
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log("HTTP Status: " + jqXHR.status + "; Error Text: " + jqXHR.responseText)
    })
}
$(".container .btn-group").children().eq(0).on("click", function () {
    decreaseStatus()
})
$(".container .btn-group").children().eq(1).on("click", function () {
    increaseStatus()
})
function changeStatus(status) {
    var taskID = $(".active").attr("href")
    taskID = taskID.replace("?id=", "")
    data = {
        TaskID: taskID,
        Status: status
    }
    $.ajax({
        type: "POST",
        url: "/Home/ChangeStatus",
        data: JSON.stringify(data),
        contentType: "application/json charset=utf-8",
        beforeSend: showLoader(),
        complete: hideLoader()
    }).done(function (data, textStatus, jqXHR) {
        console.log("Status Saved")
        getTaskInfo(displayTaskInfo)
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log("HTTP Status: " + jqXHR.status + "; Error Text: " + jqXHR.responseText)
    })
}
function increaseStatus() {
    var currentStatus = $(".statustext").text()
    switch (currentStatus) {
        case "Назначена": { changeStatus("Выполняется"); break;}
        case "Приостановлена": { changeStatus("Выполняется"); break;}
        case "Выполняется": { changeStatus("Выполнена"); break;}
        case "Выполнена": { break;}
    }
}
function decreaseStatus() {
    var currentStatus = $(".statustext").text()
    switch (currentStatus) {
        case "Назначена": { break; }
        case "Приостановлена": { break; }
        case "Выполняется": { changeStatus("Приостановлена"); break; }
        case "Выполнена": { changeStatus("Выполняется"); break; }
    }
}

