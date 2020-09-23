$(document).ready(async function () {
    var lstMenu = await GetListMenu();
    var lstSelect = BindMenuSelect(lstMenu.listMenu);
    //select2 
    $("#doc_folder").select2ToTree({ treeData: { dataArr: lstSelect } })
    //end select2 

    //tree menu 
    $("#doctree").jstree({
        "core": {
            "data": lstMenu.listMenu
        },
        "plugins": [
            "contextmenu", "dnd", "types"
        ]
    });

    //end tree menu 

    //data table
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
    SearchListDocs();

    //end data table



    //action on page
    $("#submitDocs").click(function () {
        ShowLoadingScreen();
        let document_name = $("#doc_name").val();
        let doc_file = $("#doc_file").get(0);
        let doc_description = $("#doc_description").val();
        let create_user = $("#doc_created").val();
        let status = $("#doc_status option:selected").val();
        let created_date = $("#doc_created_date").val();
        let doc_folder = $("#doc_folder option:selected").val();

        let fileUpload = doc_file.files;
        let data = new FormData();
        data.append(fileUpload[0].name, fileUpload[0]);
        data.append("document_name", document_name);
        data.append("doc_description", doc_description);
        data.append("create_user", create_user);
        data.append("status", status);
        data.append("created_date", created_date);
        data.append("doc_folder", doc_folder);

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
                    HideLoadingScreen();
                }
            },
            error: function (responsse) {
                HideLoadingScreen();
                alert("Lỗi, vui lòng kiểm tra lại.")
            }
        });
    });
    //End action on page
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
function SearchListDocs() {
    try {
        ShowLoadingScreen()
        //var groupId = $('#group-name').val();
        //var docName = $('#doc-name').val();
        //var docType = $('#doc-type').val();
        //var orgPublish = $('#org-publish').val();
        //var docContent = $('#doc-content').val();

        $.ajax({
            url: "/Home/GetListDocuments",
            data: {},
            dataType: "json",
            type: "POST",
            success: function (result) {
                var table = $('#tbl-docs').DataTable();
                var rs = result.listDocs;
                //exportData.tableData[0].data = rs;
                table.clear().draw();
                for (var i = 0; i < rs.length; i++) {
                    var displayName = rs[i].display_name;
                    var actionDown = "";
                    var actionEdit = "<a href='/Home/Edit/" + rs[i].DocumentId + "'><i class='fa fa-edit'></i></a>";
                    var actionDelete = "<a href='#' class='deleteDoc'><i class='fa fa-trash'></i></a>";
                    var docName = rs[i].display_name.split(",");
                    if (docName.length === 1) {
                        if (docName[0].includes('.ppt') ||
                            docName[0].includes('.pptx') ||
                            docName[0].includes('.doc') ||
                            docName[0].includes('.docx') ||
                            docName[0].includes('.xls') ||
                            docName[0].includes('.xlsx')) {
                            displayName = "<a target='_blank' href='https://view.officeapps.live.com/op/embed.aspx?src=http://docs.apec.com.vn/UploadedFiles/" +
                                encodeURI(docName[0]) +
                                "'><i class='fa fa-eye'></i> " + rs[i].DisplayName + "</a>";
                        } else if (docName[0].includes('.pdf') || docName[0].includes('.txt')) {
                            displayName = "<a target='_blank' href='http://docs.apec.com.vn/UploadedFiles/" +
                                docName[0] +
                                "'><i class='fa fa-eye'></i> " + rs[i].DisplayName + "</a>";
                        }
                    }
                    actionDown = "<a href='/Files/DownloadDoc?fileName=" + rs[i].DocumentName + "'><i class='fa fa-download'></i></a>";
                    var docName = rs[i].document_name.split(",");
                    if (docName.length > 1) {
                        actionDown = "<a href='/Files/DownloadDocZip?fileName=" + rs[i].DocumentName + "'><i class='fa fa-download'></i></a>";
                    }
                    table.row.add([
                        "",
                        rs[i].document_name,
                        rs[i].document_name,
                        displayName,
                        rs[i].created_user,
                        rs[i].status,
                        rs[i].approve_date,
                        actionEdit + " " + actionDelete + " " + actionDown,
                        rs[i].document_description,
                        rs[i].DocumentId
                    ]);
                }
                table.draw(false);
                HideLoadingScreen();
            },
            error: function (result) {
                HideLoadingScreen();
                alert(result.ListDocs);
            }
        });
    } catch (err) {
        HideLoadingScreen();
        alert(err);
    }
}
