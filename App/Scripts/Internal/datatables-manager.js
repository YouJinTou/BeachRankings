var DataTablesManager = function () {
    function initializeDataTable() {
        var lastFixedColumn = 2;
        var visibilityHandlerAlreadyAttached = false;
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
            autoWidth: false,
            dom: 'lBfrtip',
            buttons: [
                {
                    extend: 'colvis',
                    columns: [':nth-last-child(-n+15)'],
                    text: 'Add/remove criteria'
                }
            ]
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

        $('.buttons-colvis').on('click', function () {
            if (visibilityHandlerAlreadyAttached) {
                return;
            }

            visibilityHandlerAlreadyAttached = true;

            $('.buttons-columnVisibility').on('click', function () {
                var firstCriterionColIndex = 8;
                var lastColIndex = 22;
                var totalScoreColIndex = 7;

                table.rows().every(function (row) {                    
                    var totalScore = 0;
                    var nonSkippedCount = 0;

                    for (var col = firstCriterionColIndex; col <= lastColIndex; col++) {
                        var skipColumn = (!table.column(col).visible() || table.cell(row, col).data().length === 0);
                        
                        if (skipColumn) {
                            continue;
                        }

                        totalScore += parseFloat(table.cell(row, col).data())
                        nonSkippedCount++;
                    }

                    totalScore = (Math.round(((totalScore / nonSkippedCount) * 10)) / 10).toFixed(1);

                    table.cell(row, totalScoreColIndex).data(totalScore);
                });

                table.draw();
            });
        });
    }

    return {
        initializeDataTable: initializeDataTable
    }
};