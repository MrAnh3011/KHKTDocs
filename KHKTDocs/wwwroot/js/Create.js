ShowLoadingScreen();
$.ajax({
    url: "/Home/BindDataSelect",
    type: "POST",
    contentType: false,
    processData: false,
    data: {},
    success: function (response) {
        if (response.status == "success") {
            let data = response.lstUser;
            $.each(data, function (index, value) {
                $("#doc_created").append('<option value="' + value.username + '">' + value.full_name + '</option>');
            });
            $("#doc_created").val(response.username).change();
            HideLoadingScreen();
        } else {
            HideLoadingScreen();
            alert("Lỗi, vui lòng kiểm tra lại.");
        }
    },
    error: function (e) {
        HideLoadingScreen();
        alert("Lỗi, vui lòng kiểm tra lại.");
    }
});

$("#submitDocs").click(function () {
    ShowLoadingScreen();
    let displayname = $("#display_name").val();
    let doc_file = $("#doc_file").get(0);
    let doc_description = $("#doc_description").val();
    let create_user = $("#doc_created").val();
    let status = $("#doc_status option:selected").val();
    let created_date = $("#doc_created_date").val();
    let doc_folder = $("#doc_folder option:selected").val();
    let doc_receiver = $("#doc_receiver").val();

    let fileUpload = doc_file.files;
    let data = new FormData();
    data.append(fileUpload[0].name, fileUpload[0]);
    data.append("display_name", displayname);
    data.append("doc_description", doc_description);
    data.append("create_user", create_user);
    data.append("status", status);
    data.append("created_date", created_date);
    data.append("doc_folder", doc_folder);
    data.append("doc_receiver", doc_receiver)

    $.ajax({
        url: "/Home/CreateDoc",
        type: "POST",
        contentType: false,
        processData: false,
        data: data,
        success: function (response) {
            if (response.status == "success") {
                alert("Thêm tài liệu thành công");
                HideLoadingScreen();
                window.location.href("~/Home/Index");
            }
            else {
                alert("Lỗi, vui lòng kiểm tra lại.");
            }
        },
        error: function (responsse) {
            HideLoadingScreen();
            alert("Lỗi, vui lòng kiểm tra lại.")
        }
    });
});