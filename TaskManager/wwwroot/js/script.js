$(".add-btn").on("click", function () {
    $(".add").css("display", "flex")
    $(".cover").css("display", "block")
})
$(".edit-btn").on("click", function () {
    $(".add").css("display", "flex")
    $(".cover").css("display", "block")
})
$(".cover").on("click", function () {
    $(".add").css("display", "none")
    $(".confirmation").css("display", "none")
    $(".cover").css("display", "none")
})
$(".delete-btn").on("click", function () {
    $(".confirmation").css("display", "flex")
    $(".cover").css("display", "block")
})