

function AddOneBag() {
    var model =
           {
               Type: ($(".type option:selected").val()),
               Weight: $(".weight").val(),
               Width: ($(".width").val()),
               Height: ($(".height").val()),
               Depth: ($(".depth").val())
           };

    $.ajax({
        type: "GET",
        url: "/Home/AddBag",
        data: model
    });
}

