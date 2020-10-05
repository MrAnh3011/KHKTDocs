﻿ShowLoadingScreen();
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
            swal("Lỗi", "Vui lòng kiểm tra lại: " + response.message, "error");
        }
    },
    error: function (e) {
        HideLoadingScreen();
        swal("Lỗi", "Vui lòng kiểm tra lại: " + e, "error");
    }
});

$("#submitDocs").click(function () {
    ShowLoadingScreen();
    let stage = $("#doc_stage").val();
    let doc_description = $("#doc_description").val();
    let create_user = $("#doc_created").val();
    let status = $("#doc_status option:selected").val();
    let created_date = $("#doc_created_date").val();
    let doc_folder = $("#doc_folder option:selected").val();
    let doc_receiver = $("#doc_receiver").val();
    let doc_agency = $("#doc_agency").val();

    let doc_file = $("#doc_file").get(0);

    
    if()
    let fileUpload = doc_file.files;
    let data = new FormData();
    for (var i = 0; i < fileUpload.length; i++) {
        data.append(fileUpload[i].name, fileUpload[i]);
    }
    data.append("stage", stage);
    data.append("doc_description", doc_description);
    data.append("create_user", create_user);
    data.append("status", status);
    data.append("created_date", created_date);
    data.append("doc_folder", doc_folder);
    data.append("doc_receiver", doc_receiver);
    data.append("doc_agency", doc_agency);

    $.ajax({
        url: "/Home/CreateDoc",
        type: "POST",
        contentType: false,
        processData: false,
        data: data,
        success: function (response) {
            if (response.status == "success") {
                HideLoadingScreen();
                swal("Thành công", "Thêm tài liệu thành công", "success").then(res => {
                    if (res) {
                        window.location.href = "/Home/Index";
                    }
                });
            }
            else {
                swal("Lỗi", "Vui lòng kiểm tra lại: " + response.message, "error");
            }
        },
        error: function (responsse) {
            HideLoadingScreen();
            swal("Lỗi", "Vui lòng kiểm tra lại: " + e, "error");
        }
    });
});