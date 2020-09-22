$(document).ready(function () {
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
            {
                "targets": [10, 11],
                "visible": false
            },
            { "width": "5%", "targets": [0, 1, 2, 4, 6, 7, 8, 9] },
            { "width": "20%", "targets": [3, 5] }
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


});