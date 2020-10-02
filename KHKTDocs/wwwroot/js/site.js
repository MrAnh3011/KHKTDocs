$(document).ready(async function () {

    //#region Bind tree data to DocTree and Select
    var lstMenu = await GetListMenu();

    var lstSelect = BindMenuSelect(lstMenu.listMenu);
    $("#doc_folder").select2ToTree({ treeData: { dataArr: lstSelect } });
    $("#role-user").select2();
    $("#doc_created").select2();

    $("#doctree").jstree({
        "core": {
            "check_callback": true,
            "data": lstMenu.listMenu
        },
        "plugins": ["contextmenu", "dnd", "types"],
        "contextmenu": {
            "select_node": false
        }
    });

    $("#doctree").on("select_node.jstree", function (e, data) {
        SearchDocsByFolder(data.node.id);
    });

    $("#doctree").on("rename_node.jstree", async function (e, data) {
        console.log("rename");
        let model = {
            id: data.node.id,
            parent: data.node.parent,
            text: data.node.text,
            action: data.node.id.includes("j") ? "Create" : "Rename"
        };
        var result = await SaveDocType(model);
        $("#doctree").jstree(true).set_id(data.node, result.result);
    });
    $("#doctree").on("delete_node.jstree", function (e, data) {
        console.log("delete");
        let model = {
            id: data.node.id,
            parent: data.node.parent,
            action: 'Delete'
        };
        SaveDocType(model);
    });
    //#endregion Bind tree data to DocTree and Select

    //#region Bind data to DataTable
    var table = $("#tbl-docs").DataTable({
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
        rowId: 'DocumentId',
        "columnDefs": [
            { "targets": [10, 11], "visible": false },
            { "width": "7%", "targets": [2, 3, 5, 8, 9] },
            { "width": "10%", "targets": [4, 6, 7] },
            { "width": "15%", "targets": 1 },
            { "width": "2%", "targets": 0 }
        ]
    });

    var columnFilter =
        "<tr><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th></tr >";
    $(columnFilter).appendTo("#tbl-docs thead");
    $("#tbl-docs thead tr:eq(1) th").each(function (i) {
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
    //#endregion Bind data to DataTable
    SearchAllDocs();

    $("#btnSearchDocs").click(function () {
        ShowLoadingScreen();
        let doc_folder = $("#doc_folder option:selected").val();
        let stage = $("#doc-stage").val();
        let doc_type = $("#doc-type").val();
        let doc_agency = $("#doc-agency").val();
        let status = $("#doc-status option:selected").text();

        let searchInfo = JSON.stringify({
            docfolder: doc_folder,
            stage: stage,
            doctype: doc_type,
            docagency: doc_agency,
            status: status
        });

        $.ajax({
            type: "POST",
            url: "/Home/SearchByConditions",
            data: searchInfo,
            contentType: "application/json",
            dataType: "json",
            success: function (response) {
                if (response.status == "success") {
                    BindDataToTable(response);
                    HideLoadingScreen();
                } else {
                    swal("Lỗi", "Vui lòng kiểm tra lại: " + response.message, "error");
                    HideLoadingScreen();
                }
            },
            error: function (e) {
                swal("Lỗi", "Vui lòng kiểm tra lại: " + e, "error");
                HideLoadingScreen();
            }
        });

    });
});
function ShowLoadingScreen() {
    $(".loading-screen").css({ "display": "block" });
}
function HideLoadingScreen() {
    $(".loading-screen").css({ "display": "none" });
}

async function GetListMenu() {
    let lst;
    try {
        lst = await $.ajax({
            url: "/Home/GetListMenu",
            type: "POST",
            contentType: false,
            processData: false,
            data: {}
        });
        return lst;
    } catch (e) {
        console.error(e);
    }
}

function BindMenuSelect(listAll) {
    var listTree = [];
    var parentItems = listAll.filter(x => x.parent === '#');
    for (var i = 0; i < parentItems.length; i++) {
        var pItem = parentItems[i];
        var treeItem = {
            id: pItem.id,
            text: pItem.text,
            parentId: pItem.parent,
            inc: BindSubMenuSelect(listAll, pItem)
        };
        listTree.push(treeItem);
    };
    return listTree;
}

function BindSubMenuSelect(listAll, pItem) {
    var treeItems = [];
    var childItems = listAll.filter(x => x.parent == pItem.id);
    for (var i = 0; i < childItems.length; i++) {
        var cItem = childItems[i];
        var treeItem = {
            id: cItem.id,
            text: cItem.text,
            parentId: cItem.parent,
            inc: BindSubMenuSelect(listAll, cItem)
        };

        treeItems.push(treeItem);
    };
    return treeItems;
}

function SearchAllDocs() {
    try {
        ShowLoadingScreen();
        $.ajax({
            url: "/Home/GetListDocuments",
            data: {},
            dataType: "json",
            type: "POST",
            success: function (result) {
                if (result.status == "success") {
                    BindDataToTable(result);
                } else {
                    swal("Lỗi", "Vui lòng kiểm tra lại: " + result.message, "error");
                }
                HideLoadingScreen();
            },
            error: function (e) {
                HideLoadingScreen();
                swal("Lỗi", "Vui lòng kiểm tra lại: " + e, "error");
            }
        });
    } catch (err) {
        HideLoadingScreen();
        swal("Lỗi", "Vui lòng kiểm tra lại: " + err, "error");
    }
}

function SearchDocsByFolder(folderId) {
    try {
        ShowLoadingScreen();
        $.ajax({
            url: "/Home/SearchDocsByFolderId",
            data: { data: folderId },
            type: "POST",
            success: function (result) {
                if (result.status == "success") {
                    BindDataToTable(result);
                }
                else {
                    swal("Lỗi", "Vui lòng kiểm tra lại: " + result.message, "error");
                }
                HideLoadingScreen();
            },
            error: function (err) {
                HideLoadingScreen();
                swal("Lỗi", "Vui lòng kiểm tra lại: " + err, "error");
            }
        });
    } catch (e) {
        HideLoadingScreen();
        swal("Lỗi", "Vui lòng kiểm tra lại: " + e, "error");
    }
}

function BindDataToTable(result) {
    var table = $('#tbl-docs').DataTable();
    var rs = result.listDocs;
    table.clear().draw();
    for (var i = 0; i < rs.length; i++) {
        var displayName = rs[i].document_name;
        var actionDown = "";
        var actionDelete = "";

        if (result.role.includes("Admin") || result.role.includes("SuperAdmin") || result.role.includes("Delete")) {
            actionDelete = "<a href='#' onclick='DeleteDoc(" + rs[i].id + ")' class='deleteDoc'><i class='fa fa-trash'></i></a>";
        }

        if (result.role.includes("Admin") || result.role.includes("SuperAdmin") || result.role.includes("Approve")) {
            var actionApprove = "<a href='#' onclick='ApproveDoc(" + rs[i].id + ")' class='deleteDoc'><i class='fa fa-check-circle'></i> Duyệt</a>";
        }
        else
            var actionApprove = "";

        var docName = rs[i].document_name + rs[i].document_extension;
        if (docName.includes('.ppt') ||
            docName.includes('.pptx') ||
            docName.includes('.doc') ||
            docName.includes('.docx') ||
            docName.includes('.xls') ||
            docName.includes('.xlsx')) {
            displayName = "<a target='_blank' href='https://view.officeapps.live.com/op/embed.aspx?src=http://docs.apecgroup.net/uploads/" +
                encodeURI(docName) +
                "'><i class='fa fa-eye'></i> " + rs[i].document_name + "</a>";
        } else if (docName[0].includes('.pdf') || docName[0].includes('.txt')) {
            displayName = "<a target='_blank' href='http://http://localhost:54523/uploads/" +
                docName[0] +
                "'><i class='fa fa-eye'></i> " + rs[i].display_name + "</a>";
        }
        actionDown = "<a href='/Home/DownloadFile/" + rs[i].id + "'><i class='fa fa-download'></i></a>";

        table.row.add([
            i + 1,
            displayName,
            rs[i].document_extension,
            rs[i].folder_name,
            rs[i].created_user,
            rs[i].document_receiver,
            rs[i].created_date,
            rs[i].approve_date,
            rs[i].status == "Chờ duyệt" ? rs[i].status + "<br>" + actionApprove : rs[i].status,
            actionDelete + "&emsp;" + actionDown,
            rs[i].document_description,
            rs[i].id
        ]);
    }
    table.draw(false);
}

function SaveDocType(model) {
    ShowLoadingScreen();
    return $.ajax({
        url: "/Home/FolderEvents",
        data: model,
        type: "POST",
        success: function (result) {
            if (result.status == "success") {
                HideLoadingScreen();
                swal("Thành công", model.action + " thành công", "success");
            } else {
                HideLoadingScreen();
                swal("Lỗi", "Vui lòng kiểm tra lại: " + result.message, "error");
            }

        },
        error: function (err) {
            HideLoadingScreen();
            swal("Lỗi", "Vui lòng kiểm tra lại: " + err, "error");
        }
    });
}
function ApproveDoc(id) {
    ShowLoadingScreen();
    $.ajax({
        url: "/Home/ApproveDoc",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify({ data: id.toString() }),
        type: "POST",
        success: function (result) {
            if (result.status == "success") {
                SearchAllDocs();
                swal("Thành công", "Duyệt thành công", "success");
            }
            else {
                swal("Lỗi", "Vui lòng kiểm tra lại: " + result.message, "error");
            }
            HideLoadingScreen();
        },
        error: function (err) {
            HideLoadingScreen();
            swal("Lỗi", "Vui lòng kiểm tra lại: " + err, "error");
        }
    });
}

function DeleteDoc(id) {
    swal({
        title: "Bạn chắc chắn ?",
        text: "Bạn có chắc muốn xoá mục này ?",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    }).then((res) => {
        if (res) {
            ShowLoadingScreen();
            $.ajax({
                url: "/Home/Delete",
                contentType: "application/json",
                dataType: "json",
                data: JSON.stringify({ data: id.toString() }),
                type: "POST",
                success: function (result) {
                    if (result.status == "success") {
                        SearchAllDocs();
                        swal("Thành công", "Xoá thành công", "success");
                    }
                    else {
                        swal("Lỗi", "Vui lòng kiểm tra lại: " + result.message, "error");
                    }
                    HideLoadingScreen();
                },
                error: function (err) {
                    HideLoadingScreen();
                    swal("Lỗi", "Vui lòng kiểm tra lại: " + err, "error");
                }
            });
        }
    });
}