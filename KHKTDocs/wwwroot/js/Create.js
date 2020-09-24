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
        } else {
            alert("Lỗi, vui lòng kiểm tra lại.");
        }
    },
    error: function (e) {
        alert("Lỗi, vui lòng kiểm tra lại.");
    }
});