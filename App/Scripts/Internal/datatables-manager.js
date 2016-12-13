var DataTablesManager = function () {
    var table;
    var visibilityHandlerAlreadyAttached = false;
    var letterPairs = initializeLetterPairs();

    function initializeDataTable() {
        var options = setReponsiveDependentOptions();
        table = $('#table-result').DataTable(options);

        replaceNonLatinLetters();
        setFilterEvents();

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

                setFilterEvents();
            });
        });

        function setFilterEvents() {
            var lastFilterColumn = 2;

            table.columns().every(function (col) {
                if (col > lastFilterColumn) {
                    return;
                }

                var column = this;
                var filter = $(column.header()).find('.column-filter').data('filter');

                $('#table-result_wrapper [data-filter="' + filter + '"').off('keyup change').on('keyup change', function () {
                    column.search($(this).val()).draw();
                });

                $('#table-result_wrapper [data-filter="' + filter + '"').off('click').on('click', function (event) {
                    event.stopImmediatePropagation();
                });
            });
        }
    }

    function setReponsiveDependentOptions() {
        var options = {
            scrollX: true,
            scrollY: 300,
            scrollCollapse: true,
            autoWidth: false,
            dom: 'lBfrtip',
            buttons: [
                {
                    extend: 'colvis',
                    columns: [':nth-last-child(-n+15)'],
                    text: 'Change criteria'
                }
            ]
        };
        var viewportWidth = genericHelper.getViewportWidth();

        if (viewportWidth >= 1024) {
            options.fixedColumns = {
                leftColumns: 3
            };
            options.columnDefs = [
               { 'width': '10%', 'targets': [0] },
               { 'width': '7%', 'targets': [1, 2, 3, 4, 5, 6] },
               { 'width': '2%', 'targets': [7] }
            ];
        } else if (viewportWidth >= 480) {
            options.fixedColumns = {
                leftColumns: 1
            };
            options.columnDefs = [
               { 'width': '60px', 'targets': [0, 1, 2, 3, 4, 5, 6] },
               { 'width': '20px', 'targets': [7] }
            ];
        } else if (viewportWidth >= 320) {
            options.fixedColumns = {
                leftColumns: 1
            };
            options.columnDefs = [
               { 'width': '50px', 'targets': [0, 1, 2, 3, 4, 5, 6] },
               { 'width': '20px', 'targets': [7] }
            ];
        } else {
            options.fixedColumns = {
                leftColumns: 1
            };
        }

        return options;
    }

    function initializeLetterPairs() {
        return {
            'ă': 'a',
            'á': 'a',
            'ã': 'a',
            'å': 'a',
            'ä': 'a',
            'ā': 'a',
            'â': 'a',
            'à': 'a',
            'ả': 'a',
            'ạ': 'a',
            'ẵ': 'a',
            'ậ': 'a',
            'ć': 'c',
            'č': 'c',
            'ç': 'c',
            'đ': 'd',
            'ð': 'd',
            'ë': 'e',
            'é': 'e',
            'ê': 'e',
            'è': 'e',
            'ė': 'e',
            'ệ': 'e',
            'ế': 'e',
            'ğ': 'g',
            'ï': 'i',
            'í': 'i',
            'ì': 'i',
            'î': 'i',
            'ı': 'i',
            'ĩ': 'i',
            'ị': 'i',
            'ñ': 'n',
            'ó': 'o',
            'ô': 'o',
            'ō': 'o',
            'õ': 'o',
            'ö': 'o',
            'ò': 'o',
            'ồ': 'o',
            'ố': 'o',
            'ø': 'o',
            'ş': 's',
            'š': 's',
            'ț': 't',
            'ü': 'u',
            'ú': 'u',
            'ū': 'u',
            'ũ': 'u',
            'ừ': 'u',
            'ž': 'z'
        };
    }

    function replaceNonLatinLetters() {
        var lastTextColumn = 6;

        table.rows().every(function (row) {
            for (var col = 0; col <= lastTextColumn; col++) {
                var replacedContent = replaceCellContent(table.cell(row, col).data());

                table.cell(row, col).data(replacedContent);
            }
        });

        table.draw();
    }

    function replaceCellContent(text) {
        var result = [];

        for (var i = 0; i < text.length; i++) {
            var letter = text[i];

            if (letter in letterPairs) {
                result.push(letterPairs[letter]);
            } else {
                result.push(letter);
            }
        }

        return result.join('');
    }

    return {
        initializeDataTable: initializeDataTable
    }
};