﻿$.ajax({
    url: "/Home/BindDataSelect",
    type: "POST",
    contentType: false,
    processData: false,
    data: {},
    success: function (response) {
        if (response.status == "success") {
            let data = response.lstUser;
            $.each(data, function (index, value) {
                $("#role-user").append('<option value="' + value.username + '">' + value.full_name + '</option>');
            });
            $("#role-user").val(response.username).change();
        } else {
            alert("Lỗi select user.");
        }
    },
    error: function (e) {
        alert("Lỗi select user.");
    }
});

ShowAllRole();

$("#btnSaveRole").click(function () {
    ShowLoadingScreen();
    let idrole = parseInt($("#role-id").val());
    let usname = $("#role-user option:selected").val();
    let access = $("#role-access").is(":checked") ? 1 : 0;
    let approve = $("#role-approve").is(":checked") ? 1 : 0;
    let del = $("#role-delete").is(":checked") ? 1 : 0;
    let admin = $("#role-admin").is(":checked") ? 1 : 0;

    let roleModel = JSON.stringify({
        id: idrole,
        username: usname,
        isaccess: access,
        isapprove: approve,
        isdelete: del,
        isadmin: admin
    });

    $.ajax({
        type: "POST",
        url: "/Role/SaveUserRole",
        data: roleModel,
        contentType: "application/json",
        dataType: "json",
        success: function (response) {
            if (response.status == "success") {
                HideLoadingScreen();
                $("#modalAdd").modal("toggle");
                ShowAllRole();
                alert("Thêm quyền thành công");
            }
            else {
                HideLoadingScreen();
                alert("Thêm thất bại " + response.message);
            }
        },
        error: function (e) {
            HideLoadingScreen();
            alert("Lỗi " + e);
        }
    });
});

$("#tbl-role tbody").on('click', 'a .rowedit', function () {
    let tr = $(this).closest('tr');
    let row = table.row(tr).data();

    let data = row[9];

    $("#role-id").val(data.id);
    $("#role-user").val(data.username);
    $("#role-access").attr("checked", data.isaccess == 1);
    $("#role-approve").attr("checked", data.isapprove == 1);
    $("#role-delete").attr("checked", data.isdelete == 1);
    $("#role-admin").attr("checked", data.isadmin == 1);

    $("#modalAdd").modal('show');
});
$("#tbl-role tbody").on('click', 'a .rowdelete', function () {
    ShowLoadingScreen();
    let tr = $(this).closest('tr');
    let row = table.row(tr).data();

    let id = row[8].toString();

    $.ajax({
        url: "/Role/DeleteUserRole",
        type: "POST",
        contentType: false,
        processData: false,
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify({ data: id }),
        success: function (response) {
            if (response.status == "success") {
                HideLoadingScreen();
                alert("Xoá thành công");
                ShowAllRole();
            }
            else {
                alert("Xoá thất bại");
                HideLoadingScreen();
            }
        },
        error: function () {
            alert("Có lỗi xảy ra!");
            HideLoadingScreen();
        }
    });
});

//#region for create table
var table = $("#tbl-role").DataTable({
    searching: false,
    lengthChange: false,
    language: {
        "sProcessing": "Đang xử lý...",
        "sLengthMenu": "Hiển thị _MENU_ mục",
        "sZeroRecords": "Không tìm thấy dòng nào phù hợp",
        "sInfo": "Đang xem _START_ đến _END_ trong tổng số _TOTAL_ mục",
        "sInfoEmpty": "Đang xem 0 đến 0 trong tổng số 0 mục",
        "sInfoFiltered": "(được lọc từ _MAX_ mục)",
        "sInfoPostFix": "",
        "sSearch": "Tìm:",
        "sUrl": "",
        "oPaginate": {
            "sFirst": "Đầu",
            "sPrevious": "Trước",
            "sNext": "Tiếp",
            "sLast": "Cuối"
        }
    },
    rowId: 'roleId',
    "columnDefs": [
        { "targets": [8, 9], "visible": false },
        { "width": "5%", "targets": 0 },
        { "width": "7%", "targets": 7 },
        { "width": "25%", "targets": 2 },
        { "width": "12%", "targets": [1, 3, 4, 5, 6] }
    ]
});
var columnFilter = "<tr><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th></tr >";
$(columnFilter).appendTo("#tbl-role thead");
$("#tbl-role thead tr:eq(1) th").each(function (i) {
    var title = $(this).text();
    $(this).html('<input type="text" class="form-control" placeholder="' + title + '" />');

    $("input", this).on("keyup change",
        function () {
            if (table.column(i).search() !== this.value) {
                table
                    .column(i)
                    .search(this.value)
                    .draw();
            }
        });
});
//#endregion for create table

function ShowAllRole() {
    ShowLoadingScreen();
    $.ajax({
        url: "/Role/GetAllUserRole",
        data: {},
        dataType: "json",
        type: "POST",
        success: function (result) {
            if (result.status == "success") {
                BindDataToRoleTable(result);
                HideLoadingScreen();
            } else {
                alert(result.message);
                HideLoadingScreen();
            }
        },
        error: function (result) {
            HideLoadingScreen();
            alert(result.ListDocs);
        }
    });
}

function BindDataToRoleTable(result) {
    let table = $('#tbl-role').DataTable();
    let rs = result.listRole;
    table.clear().draw();
    for (var i = 0; i < rs.length; i++) {
        RoleEdit = "<a href='#'><i class='rowedit fa fa-edit'></i></a>";
        RoleDelete = "<a href='#'><i class='rowdelete fa fa-trash'></i></a>";

        table.row.add([
            i + 1,
            rs[i].username,
            rs[i].fullname,
            rs[i].isaccess == 1 ? "<i class='fa fa-check'></i>" : "",
            rs[i].isapprove == 1 ? "<i class='fa fa-check'></i>" : "",
            rs[i].isdelete == 1 ? "<i class='fa fa-check'></i>" : "",
            rs[i].isadmin == 1 ? "<i class='fa fa-check'></i>" : "",
            RoleEdit + "&emsp;" + RoleDelete,
            rs[i].id,
            rs[i]
        ]);
    }
    table.draw(false);
}