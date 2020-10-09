$(document).ready(async function () {
    const Toast = Swal.mixin({
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 3000,
    });
    //#region Bind tree data to DocTree and Select
    var lstMenu = await GetListMenu();

    var lstSelect = BindMenuSelect(lstMenu.listMenu);
    $("#doc_folder").select2ToTree({ treeData: { dataArr: lstSelect } });

    //#region change selected folder for select2 in view : Create
    var url = new URL(window.location.href);
    var c = url.searchParams.get("folderid");
    if (c !== null && url.pathname == "/Home/Create") {
        $("#doc_folder").val(c);
        $("#doc_folder").trigger('change');
    }
    if (c !== null && url.pathname == "/Home/Index") {
        SearchDocsByFolder(c);
    }
    //#endregion change selected folder for select2 in view : Create

    $("#role-user").select2();
    $("#doc_created").select2();

    $("#doctree").jstree({
        "core": {
            "check_callback": true,
            "data": lstMenu.listMenu
        },
        "plugins": ["contextmenu", "dnd", "types"],
        "contextmenu": {
            "select_node": false,
            "items": function (node) {
                var defaultItems = $.jstree.defaults.contextmenu.items();
                defaultItems.create.label = "Thêm mới thư mục";
                defaultItems.rename.label = "Đổi tên thư mục";
                defaultItems.remove.label = "Xoá thư mục";
                defaultItems.ccp = false;
                defaultItems.download = {
                    "label": "Tải về",
                    "separator_after": false,
                    "separator_before": false,
                    "action": function (obj) {
                        window.location.href = "/Home/DownloadFolder/" + node.id;
                    }
                };
                defaultItems.addfile = {
                    "label": "Thêm file",
                    "separator_after": false,
                    "separator_before": false,
                    "action": function (obj) {
                        window.location.href = "/Home/Create?folderid=" + node.id;
                    }
                };
                defaultItems.getLink = {
                    "label": "Get link",
                    "separator_after": false,
                    "separator_before": false,
                    "action": function (obj) {
                        let link = "docs.apecgroup.net/Home/Index?folderid=" + node.id;

                        let $temp = $("<input>");
                        $("body").append($temp);
                        $temp.val(link).select();
                        document.execCommand("copy");
                        $temp.remove();

                        Toast.fire({ title: "Đã lưu link vào bộ nhớ đệm", icon: "success" });
                    }
                };

                return defaultItems;
            }
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
        console.log(data.node.id);
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
            { "targets": [11, 12], "visible": false },
            { "width": "7%", "targets": [2, 8]},
            { "width": "5%", "targets": 3 },
            { "width": "8%", "targets": [4, 6, 7] },
            { "width": "10%", "targets": [5, 9] },
            { "width": "15%", "targets": 1 },
            { "width": "2%", "targets": 0 }
        ]
    });

    var columnFilter =
        "<tr><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th></tr >";
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
                    Swal.fire("Lỗi", "Vui lòng kiểm tra lại: " + response.message, "error");
                    HideLoadingScreen();
                }
            },
            error: function (e) {
                Swal.fire("Lỗi", "Vui lòng kiểm tra lại: " + e, "error");
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
                    Swal.fire("Lỗi", "Vui lòng kiểm tra lại: " + result.message, "error");
                }
                HideLoadingScreen();
            },
            error: function (e) {
                HideLoadingScreen();
                Swal.fire("Lỗi", "Vui lòng kiểm tra lại: " + e, "error");
            }
        });
    } catch (err) {
        HideLoadingScreen();
        Swal.fire("Lỗi", "Vui lòng kiểm tra lại: " + err, "error");
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
                    Swal.fire("Lỗi", "Vui lòng kiểm tra lại: " + result.message, "error");
                }
                HideLoadingScreen();
            },
            error: function (err) {
                HideLoadingScreen();
                Swal.fire("Lỗi", "Vui lòng kiểm tra lại: " + err, "error");
            }
        });
    } catch (e) {
        HideLoadingScreen();
        Swal.fire("Lỗi", "Vui lòng kiểm tra lại: " + e, "error");
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
        if (docName.toLowerCase().includes('.ppt') ||
            docName.toLowerCase().includes('.pptx') ||
            docName.toLowerCase().includes('.doc') ||
            docName.toLowerCase().includes('.docx') ||
            docName.toLowerCase().includes('.xls') ||
            docName.toLowerCase().includes('.xlsx')) {
            displayName = "<a target='_blank' href='https://view.officeapps.live.com/op/embed.aspx?src=http://docs.apecgroup.net/uploads/" +
                encodeURI(docName) +
                "'><i class='fa fa-eye'></i> " + rs[i].document_name + "</a>";
        } else if (docName.includes('.pdf') || docName.includes('.txt')) {
            displayName = "<a target='_blank' href='http://docs.apecgroup.net/uploads/" +
                docName +
                "'><i class='fa fa-eye'></i> " + rs[i].document_name + "</a>";
        }
        actionDown = "<a href='/Home/DownloadFile/" + rs[i].id + "'><i class='fa fa-download'></i></a>";

        table.row.add([
            i + 1,
            displayName,
            actionDelete + "&emsp;" + actionDown,
            rs[i].document_extension,
            "<a href='#' onclick='SearchDocsByFolder(" + rs[i].folder_id + ")'>" + rs[i].folder_name + "</a>",
            rs[i].created_user,
            rs[i].created_date,
            rs[i].approve_date,
            rs[i].status == "Chờ duyệt" ? rs[i].status + "<br>" + actionApprove : rs[i].status,
            rs[i].approver,
            rs[i].document_description,
            rs[i].document_receiver,
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
                Swal.fire("Thành công", model.action + " thành công", "success");
            } else {
                HideLoadingScreen();
                Swal.fire("Lỗi", "Vui lòng kiểm tra lại: " + result.message, "error");
            }
        },
        error: function (err) {
            HideLoadingScreen();
            Swal.fire("Lỗi", "Vui lòng kiểm tra lại: " + err, "error");
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
                Swal.fire("Thành công", "Duyệt thành công", "success");
            }
            else {
                Swal.fire("Lỗi", "Vui lòng kiểm tra lại: " + result.message, "error");
            }
            HideLoadingScreen();
        },
        error: function (err) {
            HideLoadingScreen();
            Swal.fire("Lỗi", "Vui lòng kiểm tra lại: " + err, "error");
        }
    });
}

function DeleteDoc(id) {
    Swal.fire({
        title: "Bạn chắc chắn ?",
        text: "Bạn có chắc muốn xoá mục này ?",
        icon: "warning",
        showCancelButton: true,
    }).then((res) => {
        if (res.isConfirmed) {
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
                        Swal.fire("Thành công", "Xoá thành công", "success");
                    }
                    else {
                        Swal.fire("Lỗi", "Vui lòng kiểm tra lại: " + result.message, "error");
                    }
                    HideLoadingScreen();
                },
                error: function (err) {
                    HideLoadingScreen();
                    Swal.fire("Lỗi", "Vui lòng kiểm tra lại: " + err, "error");
                }
            });
        }
    });
}