var DataTablesManager = function () {
    function initializeDataTable() {
        var lastFixedColumn = 2;
        var table = $('#table-result').DataTable({
            scrollX: true,
            scrollY: 300,
            scrollCollapse: true,
            fixedColumns: {
                leftColumns: 3
            },
            columnDefs: [
                { 'width': '20px', 'targets': [0, 1, 2] }
            ],
            autoWidth: false
        });

        table.columns().every(function (col) {
            if (col > lastFixedColumn) {
                return;
            }

            var column = this;
            var filter = $(column.header()).find('.column-filter').data('filter');

            $('.DTFC_LeftHeadWrapper [data-filter="' + filter + '"').on('keyup change', function () {
                column.search($(this).val()).draw();
            });

            $('.DTFC_LeftHeadWrapper [data-filter="' + filter + '"').on('click', function (event) {
                event.stopImmediatePropagation();
            });
        });
    }

    return {
        initializeDataTable: initializeDataTable
    }
};