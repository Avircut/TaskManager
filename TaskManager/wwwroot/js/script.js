var mode="Create"
$(".add-btn").on("click", function () {
    $(".add").css("display", "flex")
    enableCover()
    mode = "Create"
    $("#name-input").val("")
    $("#descriptionTextArea").val("")
    $("#executors").val("")
    $("endDate").val("")
    $("#endDate").prop("disabled", false)
    $("#parentID").val("")
    $(".needs-validation>button").text("Добавить задачу")
})
function disableCover() {
    $(".add").css("display", "none")
    $(".confirmation").css("display", "none")
    $(".cover").css("display", "none")
}
function enableCover() {
    $(".cover").css("display", "block")
}
$(".edit-btn").on("click", function () {
    $(".add").css("display", "flex")
    mode = "Edit"
    enableCover()
    getTaskInfo(fillEditForm)
    $(".needs-validation>button").text("Изменить задачу")
})
$(".cover").on("click", function () {
    disableCover()
})
$(".delete-btn").on("click", function () {
    $(".confirmation").css("display", "flex")
    enableCover()
})
function showLoader() {
    $("#Loader").css("display", "block")
}
function hideLoader() {
    $("#Loader").css("display", "none")
}
function cutDate(date) {
    return moment(date).format("DD.MM.YYYY")

}
$(document).ready(function () {
    getMajorTasks(displayMajorTasks)
})
$(".list-group").on("click","a", function () {
    $(".list-group-item").removeClass("active")
    $(this).addClass("active")
    getTaskInfo(displayTaskInfo)
    return false
})
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
    $(".caption-top h4").text(response.name)
    $(".description p").text(response.description)
    $(".executors p").text(response.executors)
    $(".time p").text(`${cutDate(response.registerDate)} - ${cutDate(response.plannedEndDate)}`)
    if (response.factEndDate != "0001-01-01T00:00:00") $(".time p").text += "(" + cutDate(response.factEndDate) + ")"
    $(".status p").text(response.status)
    getSubtasks(displaySubtasks)
}
function fillEditForm(response) {
    $("#name-input").val(response.name)
    $("#descriptionTextArea").val(response.description)
    $("#executors").val(response.executors)
    $("#endDate").val("1999-01-01")
    $("#endDate").prop("disabled", true)
    $(`#parentID option[value="${response.parentTaskID}"]`).prop("selected",true)
}
function displaySubtasks(response) {
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
function nextSubGroupLevelCheck() {
    if ($(".active").hasClass("subgroup-l1")) return "subgroup-l2";
    if ($(".active").hasClass("subgroup-l2")) return "subgroup-l3";
    else return "subgroup-l1";
}
function getTaskInfo(displayTaskInfo) {
    var taskID = $(".active").attr("href")
    taskID=taskID.replace("?id=","")
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
function getSubtasks(displaySubtasks) {
    if (!$(".active").next().is(".subgroup-l1") && !$(".active.subgroup-l1").next().is(".subgroup-l2") && !$(".active.subgroup-l2").next().is(".subgroup-l3")) {
        var taskID = $(".active").attr("href")
        taskID = taskID.replace("?id=", "")
        $.ajax({
            type: "POST",
            url: "/Home/GetSubtasks",
            data: JSON.stringify(taskID),
            contentType: "application/json charset=utf-8",
            dataType: "json",
            beforeSend: showLoader(),
            success: displaySubtasks,
            complete: hideLoader(),
            failure: function (jqXHR, textsStatus, errorThrown) {
                alert("HTTP Status: " + jqXHR.status + "; Error Text: " + jqXHR.responseText)
            }
        })
    }
}

function addOrEdit(getMajorTasks) {
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
    if (mode == "Create")
        $.ajax({
            type: "POST",
            url: "/Home/AddTask",
            data: JSON.stringify(task),
            contentType: 'application/json',
            dataType: 'json',
            beforeSend: function () {
                $("#Loader").show();
            },
            success: getMajorTasks(displayMajorTasks),
            complete: function () {
                $("#Loader").hide();
            },
            failure: function (jqXHR, textStatus, errorThrown) {
                alert("status:" + jqXHR.textStatus + "; Error: " + jqXHR.responseText);
            }
        })
    else {
        console.log(task)
        $.ajax({
            type: "POST",
            url: "/Home/EditTask",
            data: JSON.stringify(task),
            contentType: 'application/json',
            dataType: 'json',
            beforeSend: function () {
                $("#Loader").show();
            },
            success: getMajorTasks(displayMajorTasks),
            complete: function () {
                $("#Loader").hide();
            },
            failure: function (jqXHR, textStatus, errorThrown) {
                alert("status:" + jqXHR.textStatus + "; Error: " + jqXHR.responseText);
            }
        })
    }
    $(".add").css("display", "none")
    $(".cover").css("display", "none")
}
$(".needs-validation").submit(function (event) {
    event.preventDefault()
    addOrEdit(getMajorTasks)
    
})

$(".confirmation #confirm").on("click", function () {
    deleteTask(getMajorTasks)
    disableCover()
    $(".confirmation").css("display", "none")
})
function deleteTask(getMajorTasks) {
    var taskID = $(".active").attr("href")
    taskID = taskID.replace("?id=", "")
    $.ajax({
        type: "POST",
        url: "/Home/DeleteTask",
        data: JSON.stringify(taskID),
        contentType: "application/json charset=utf-8",
        dataType: "json",
        beforeSend: showLoader(),
        success: getMajorTasks(displayMajorTasks),
        complete: hideLoader(),
        failure: function (jqXHR, textsStatus, errorThrown) {
            alert("HTTP Status: " + jqXHR.status + "; Error Text: " + jqXHR.responseText)
        }
    })
}


