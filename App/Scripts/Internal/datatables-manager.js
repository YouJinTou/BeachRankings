var DataTablesManager = function () {
    var table;

    function initializeDataTable() {
        var visibilityHandlerAlreadyAttached = false;
        var letterPairs = initializeLetterPairs();
        var criteriaColNumbers = initializeCriteriaColNumbers();
        var filterTypes = initializeFilterTypes();
        var options = setReponsiveDependentOptions();
        table = $('#table-result').DataTable(options);

        replaceNonLatinLetters();

        setFilterEvents();

        addCriteriaSelectionInfo();

        recalculateTable();

        $('.buttons-colvis').on('click', function () {
            if (visibilityHandlerAlreadyAttached) {
                return;
            }

            visibilityHandlerAlreadyAttached = true;

            $('.buttons-columnVisibility').on('click', function () {
                recalculateTable();

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

        function recalculateTable() {
            var firstCriterionColIndex = criteriaColNumbers['SandQuality'];
            var lastColIndex = criteriaColNumbers['LongStay'];
            var totalScoreColIndex = criteriaColNumbers['TotalScore'];

            table.rows().every(function (row) {
                var totalScore = 0.0;
                var nonSkippedCount = 0;

                for (var col = firstCriterionColIndex; col <= lastColIndex; col++) {
                    var skipColumn = (!table.column(col).visible() || table.cell(row, col).data().length === 0);

                    if (skipColumn) {
                        continue;
                    }

                    score = parseFloat(table.cell(row, col).data().replace(',', '.'));
                    totalScore += score;
                    nonSkippedCount++;
                }

                totalScore = (Math.round(((totalScore / nonSkippedCount) * 100)) / 100).toFixed(2);
                totalScore = isNaN(totalScore) ? '-' : totalScore;

                table.cell(row, totalScoreColIndex).data(totalScore);
            });

            table.draw();
        }

        function setReponsiveDependentOptions() {
            var filterTypeValue = $('[data-filter-type]').data('filter-type');
            var filterType = filterTypeValue ? filterTypes[filterTypeValue] : filterTypes['AllCriteria'];
            var sortingCriterion = $('[data-sorting-criterion]').data('sorting-criterion');
            var totalScoreCol = criteriaColNumbers['TotalScore'];
            var sortingCol = totalScoreCol + (sortingCriterion ? sortingCriterion : 0);
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
                        text: 'Select criteria'
                    }
                ],
                order: [[sortingCol, 'desc']]
            };
            var viewportWidth = genericHelper.getViewportWidth();

            if (viewportWidth >= 1024) {
                options.fixedColumns = {
                    leftColumns: 3
                };
                options.columnDefs = [
                    { 'width': '10%', 'targets': [0] },
                    { 'width': '7%', 'targets': [1, 2, 3, 4, 5, 6] },
                    { 'width': '2%', 'targets': [7] },
                    { 'visible': false, 'targets': filterType }
                ];
            } else if (viewportWidth >= 480) {
                options.fixedColumns = {
                    leftColumns: 1
                };
                options.columnDefs = [
                    { 'width': '60px', 'targets': [0, 1, 2, 3, 4, 5, 6] },
                    { 'width': '20px', 'targets': [7] },
                    { 'visible': false, 'targets': filterType }
                ];
            } else if (viewportWidth >= 320) {
                options.fixedColumns = {
                    leftColumns: 1
                };
                options.columnDefs = [
                    { 'width': '50px', 'targets': [0, 1, 2, 3, 4, 5, 6] },
                    { 'width': '20px', 'targets': [7] },
                    { 'visible': false, 'targets': filterType }
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

        function initializeCriteriaColNumbers() {
            return {
                'TotalScore': 7,
                'SandQuality': 8,
                'BeachCleanliness': 9,
                'BeautifulScenery': 10,
                'CrowdFree': 11,
                'Infrastructure': 12,
                'WaterVisibility': 13,
                'LitterFreeWater': 14,
                'FeetFriendly': 15,
                'SeaLife': 16,
                'CoralReef': 17,
                'Snorkeling': 18,
                'Kayaking': 19,
                'Walking': 20,
                'Camping': 21,
                'LongStay': 22
            }
        }

        function initializeFilterTypes() {
            return {
                'AllCriteria': [],
                'BestBeach': [
                    criteriaColNumbers['CrowdFree'],
                    criteriaColNumbers['Infrastructure'],
                    criteriaColNumbers['SeaLife'],
                    criteriaColNumbers['CoralReef'],
                    criteriaColNumbers['Snorkeling'],
                    criteriaColNumbers['Kayaking'],
                    criteriaColNumbers['Walking'],
                    criteriaColNumbers['Camping'],
                    criteriaColNumbers['LongStay'],
                ],
                'Camping': [
                    criteriaColNumbers['CrowdFree'],
                    criteriaColNumbers['Infrastructure'],
                    criteriaColNumbers['FeetFriendly'],
                    criteriaColNumbers['SeaLife'],
                    criteriaColNumbers['CoralReef'],
                    criteriaColNumbers['LongStay']
                ],
                'HolidayWithKids': [
                    criteriaColNumbers['SeaLife'],
                    criteriaColNumbers['CoralReef'],
                    criteriaColNumbers['Snorkeling'],
                    criteriaColNumbers['Kayaking'],
                    criteriaColNumbers['Walking'],
                    criteriaColNumbers['Camping'],
                    criteriaColNumbers['LongStay']
                ],
                'Kayaking': [
                    criteriaColNumbers['SandQuality'],
                    criteriaColNumbers['BeachCleanliness'],
                    criteriaColNumbers['CrowdFree'],
                    criteriaColNumbers['Infrastructure'],
                    criteriaColNumbers['WaterVisibility'],
                    criteriaColNumbers['LitterFreeWater'],
                    criteriaColNumbers['FeetFriendly'],
                    criteriaColNumbers['SeaLife'],
                    criteriaColNumbers['CoralReef'],
                    criteriaColNumbers['Snorkeling'],
                    criteriaColNumbers['Walking'],
                    criteriaColNumbers['Camping'],
                    criteriaColNumbers['LongStay']
                ],
                'LongTermStay': [
                    criteriaColNumbers['BeautifulScenery'],
                    criteriaColNumbers['CrowdFree'],
                    criteriaColNumbers['Infrastructure'],
                    criteriaColNumbers['WaterVisibility'],
                    criteriaColNumbers['SeaLife'],
                    criteriaColNumbers['CoralReef'],
                    criteriaColNumbers['Snorkeling'],
                    criteriaColNumbers['Kayaking'],
                    criteriaColNumbers['Camping']
                ],
                'Snorkeling': [
                    criteriaColNumbers['SandQuality'],
                    criteriaColNumbers['BeachCleanliness'],
                    criteriaColNumbers['BeautifulScenery'],
                    criteriaColNumbers['CrowdFree'],
                    criteriaColNumbers['Infrastructure'],
                    criteriaColNumbers['FeetFriendly'],
                    criteriaColNumbers['Kayaking'],
                    criteriaColNumbers['Walking'],
                    criteriaColNumbers['Camping'],
                    criteriaColNumbers['LongStay']
                ],
                'Walking': [
                    criteriaColNumbers['CrowdFree'],
                    criteriaColNumbers['Infrastructure'],
                    criteriaColNumbers['WaterVisibility'],
                    criteriaColNumbers['LitterFreeWater'],
                    criteriaColNumbers['FeetFriendly'],
                    criteriaColNumbers['SeaLife'],
                    criteriaColNumbers['CoralReef'],
                    criteriaColNumbers['Snorkeling'],
                    criteriaColNumbers['Kayaking'],
                    criteriaColNumbers['Camping'],
                    criteriaColNumbers['LongStay']
                ]
            }
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

        function addCriteriaSelectionInfo() {
            var infoIcon = '<span class="info-icon glyphicon glyphicon-info-sign"></span>';
            var popUp = '<div class="custom-popup-dt" style="display: none;">' +
                'Add/remove criteria, and the scores will update automatically.' +
                '</div>'

            $('div.dt-buttons').prepend(infoIcon + popUp);
        }
    }

    function initializeContributorsDataTable() {
        table = $('#contributors-table').DataTable();
    }

    return {
        initializeDataTable: initializeDataTable,
        initializeContributorsDataTable: initializeContributorsDataTable
    }
};

var dataTablesManager = new DataTablesManager();