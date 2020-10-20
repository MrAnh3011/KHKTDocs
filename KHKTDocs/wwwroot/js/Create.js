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
                $("#doc_approver").append('<option value="' + value.username + '">' + value.full_name + '</option>');
            });
            $("#doc_created").val(response.username).change();
            $("#doc_approver").val('lynv').change();
            HideLoadingScreen();
        } else {
            HideLoadingScreen();
            Swal.fire("Lỗi", "Vui lòng kiểm tra lại: " + response.message, "error");
        }
    },
    error: function (e) {
        HideLoadingScreen();
        Swal.fire("Lỗi", "Vui lòng kiểm tra lại: " + e, "error");
    }
});

$("#submitDocs").click(function () {
    ShowLoadingScreen();
    let doc_description = $("#doc_description").val();
    let create_user = $("#doc_created").val();
    let status = $("#doc_status option:selected").val();
    let created_date = $("#doc_created_date").val();
    let doc_folder = $("#doc_folder option:selected").val();
    let doc_approver = $("#doc_approver").val();

    let doc_file = $("#doc_file").get(0);

    if ( status === "-1" || doc_file.files.length === 0) {
        HideLoadingScreen();
        Swal.fire("Lỗi", "Vui lòng nhập đầy đủ thông tin", "error");
        return;
    }
    
    let fileUpload = doc_file.files;
    let data = new FormData();
    for (var i = 0; i < fileUpload.length; i++) {
        data.append(fileUpload[i].name, fileUpload[i]);
    }
    data.append("doc_description", doc_description);
    data.append("create_user", create_user);
    data.append("status", status);
    data.append("created_date", created_date);
    data.append("doc_folder", doc_folder);
    data.append("doc_approver", doc_approver);

    $.ajax({
        url: "/Home/CreateDoc",
        type: "POST",
        contentType: false,
        processData: false,
        data: data,
        success: function (response) {
            if (response.status == "success") {
                HideLoadingScreen();
                Swal.fire("Thành công", "Thêm tài liệu thành công", "success").then(res => {
                    if (res) {
                        window.location.href = "/Home/Index";
                    }
                });
            }
            else {
                HideLoadingScreen();
                Swal.fire("Lỗi", "Vui lòng kiểm tra lại: " + response.message, "error");
            }
        },
        error: function (responsse) {
            HideLoadingScreen();
            Swal.fire("Lỗi", "Vui lòng kiểm tra lại: " + e, "error");
        }
    });
});